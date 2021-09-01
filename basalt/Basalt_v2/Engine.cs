using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Media;
using System.Threading;
using System.Windows.Forms;

using Shkila.ScaleReaders;
using Shkila.KeyboardHook;
using System.Globalization;
using Shkila.LicenseManager;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace Basalt_v2
{
    public class Engine
    {
        private ScaleReader sr;
        private ViewModel m_viewModel;
        private int[] baudRate = {2400, 9600, 115200 };
        private Configuration m_Config;
        private Thread m_weightThread;
        private Thread m_keysThread;
        private InterceptKeys m_InterceptKeys = new InterceptKeys();
        private static Engine m_instance = null;
        private static object m_object = new object();
        private bool IsFoundCom = false;
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

        public static Engine Instance
        {
            get
            {
                lock(m_object)
                {
                    if (m_instance == null)
                        m_instance = new Engine();

                    return m_instance;
                }
            }
        }

        private Engine()
        {
            try
            {

                m_viewModel = new ViewModel
                {
                    ConnectionArgs = new ConnectionArgs { BaudRate = 9600, Com = "COM1", DataBits = 8, Dtr = false, Rts = false, Type = Shkila.ScaleReaders.ConnectionType.Serial, StopBits = System.IO.Ports.StopBits.One },
                    BaudRates = new System.Collections.ObjectModel.ObservableCollection<int>(baudRate),
                    Parity = new System.Collections.ObjectModel.ObservableCollection<Parity>(Enum.GetValues(typeof(Parity)).Cast<Parity>()),
                    StopBits = new System.Collections.ObjectModel.ObservableCollection<StopBits>(Enum.GetValues(typeof(StopBits)).Cast<StopBits>()),
                    Languages = new System.Collections.ObjectModel.ObservableCollection<Language>(Enum.GetValues(typeof(Language)).Cast<Language>()),
                    ConnectionType = new System.Collections.ObjectModel.ObservableCollection<ConnectionType>(Enum.GetValues(typeof(ConnectionType)).Cast<ConnectionType>()),
                    ScaleHeaders = new System.Collections.ObjectModel.ObservableCollection<ScaleHeaders>(Enum.GetValues(typeof(ScaleHeaders)).Cast<ScaleHeaders>()),
                    Keys = new System.Collections.ObjectModel.ObservableCollection<string>(Enum.GetNames(typeof(Keys))),
                    FinishChar = Keys.Enter.ToString(),
                    ActivationKey = Keys.Pause.ToString()
                };

                m_Config = new Configuration();
                LoadConfig();
                if(m_Config.AutoComCheck)                
                    ScanConnectedComs();
                m_viewModel.Error = String.Empty;
            }
            catch (Exception ex)
            {
                Error(ex.Message);
                throw ex;
            }   
            
        }
        private void ScanConnectedComs()
        {
            string oldCom = m_viewModel.ConnectionArgs.Com;
            foreach(string com in m_viewModel.ComPorts)
            {
                m_viewModel.ConnectionArgs.Com = com;
                StartScaleThread();
                Thread.Sleep(2000);
                Stop();
                if (IsFoundCom)
                    return;
            }
            m_viewModel.ConnectionArgs.Com = oldCom;
        }

        public void LoadConfig()
        {
            try
            {
                if (m_Config.LoadConfig())
                {
                    if (m_Config.BaudRate != null)
                        m_viewModel.ConnectionArgs.BaudRate = (int)m_Config.BaudRate;
                    if (m_Config.DataBits != null)
                        m_viewModel.ConnectionArgs.DataBits = (int)m_Config.DataBits;
                    if (m_Config.Port != null)
                        m_viewModel.ConnectionArgs.Port = (int)m_Config.Port;
                    if (m_Config.DTR != null)
                        m_viewModel.ConnectionArgs.Dtr = (bool)m_Config.DTR;
                    if (m_Config.RTS != null)
                        m_viewModel.ConnectionArgs.Rts = (bool)m_Config.RTS;
                    if (m_Config.ConnectionType != null)
                        m_viewModel.ConnectionArgs.Type = (ConnectionType)m_Config.ConnectionType;
                    if (m_Config.Parity != null)
                        m_viewModel.ConnectionArgs.Parity = (Parity)m_Config.Parity;
                    if (m_Config.StopBits != null)
                        m_viewModel.ConnectionArgs.StopBits = (StopBits)m_Config.StopBits;
                    if (m_Config.Multiple != null)
                        m_viewModel.Multiple = (float)m_Config.Multiple;
                    if (m_Config.ScaleHeader != null)
                        m_viewModel.ScaleHeader = (ScaleHeaders)m_Config.ScaleHeader;
                    if (!String.IsNullOrEmpty(m_Config.ComPort))
                        m_viewModel.ConnectionArgs.Com = m_Config.ComPort;
                    if (!String.IsNullOrEmpty(m_Config.IP))
                        m_viewModel.ConnectionArgs.IP = m_Config.IP;
                    if (!String.IsNullOrEmpty(m_Config.ActivationKey))
                        m_viewModel.ActivationKey = m_Config.ActivationKey;
                    if (!String.IsNullOrEmpty(m_Config.FinishChar))
                        m_viewModel.FinishChar = m_Config.FinishChar;
                    if (!String.IsNullOrEmpty(m_Config.Username))
                        m_viewModel.ConnectionArgs.Username = m_Config.Username;
                    if (!String.IsNullOrEmpty(m_Config.Company))
                        m_viewModel.Company = m_Config.Company;
                    if (!String.IsNullOrEmpty(m_Config.LogFilePath))
                        m_viewModel.LogFilePath = m_Config.LogFilePath;
                    if (m_Config.MainColor != null)
                        m_viewModel.MainColor = m_Config.MainColor;

                }
                else
                    throw new Exception(m_Config.Error);
                m_viewModel.Error = String.Empty;
            }
            catch (Exception ex)
            {
                Error(ex.Message);
                throw ex;
            }
            
        }

        public void Start()
        {
            StartScaleThread();
            StartKeysThread();
            m_viewModel.Error = String.Empty;
        }

        private void StartKeysThread()
        {
            m_keysThread = new Thread(() =>
            {
                try
                {
                    Keys key = (Keys)Enum.Parse(typeof(Keys), m_viewModel.ActivationKey, true);
                    InterceptKeys.KeysToHandle.Add(key);
                    InterceptKeys.OnCaptureKey += SendWeight;
                    InterceptKeys.Main();
                }catch(Exception ex)
                {
                    Error(ex.Message);
                }
               
            });
            m_keysThread.IsBackground = true;
            m_keysThread.Start();
            
        }

        public void StartScaleThread()
        {
            try
            {
                m_weightThread = new Thread(() =>
                {
                    _Start();
                });

                m_weightThread.IsBackground = true;
                m_weightThread.Start();
                
            }
            catch(Exception ex)
            {
                Error(ex.Message);
                throw ex;
            }
            
        }

        private void _Start()
        {
            try
            {
                if (sr != null)
                    Stop();

                sr = ScaleReader.Factory(m_viewModel.ScaleHeader, m_viewModel.ConnectionArgs);
                if (sr == null)
                    throw new Exception("");

                sr.Weight += Weight;
                sr.Error += Error;
                if(!sr.Connect())
                {
                    Stop();
                    return;
                }
                sr.IsRunning = true;
                sr.Process();

                m_viewModel.Error = String.Empty;
            }
            catch(ThreadAbortException ex)
            {
                
            }
            catch(Exception ex)
            {
                Error(ex.Message);
            }
            
        }

        private void Error(string error)
        {
            Log(error);
            m_viewModel.Error = error;
        }

        private void Log(string error)
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                StreamWriter sw = new StreamWriter("log.txt", true);
                sw.WriteLine(error);
                sw.Close();
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
            
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);

        private void SendWeight(object sender, EventArgs e)
        {
            try
            {
                if(!String.IsNullOrEmpty(m_viewModel.LogFilePath) && Directory.Exists(Path.GetDirectoryName(m_viewModel.LogFilePath)))
                {
                    StreamWriter sw = new StreamWriter(m_viewModel.LogFilePath);
                    sw.Write(m_viewModel.Weight);
                    sw.Close();
                }

                bool CapsLock = (((ushort)GetKeyState(0x14)) & 0xffff) != 0;
                string message = (CapsLock ? "" : "{CAPSLOCK}") + m_viewModel.Weight + (m_viewModel.FinishChar.Equals("None") ? "" : "{" + m_viewModel.FinishChar.ToString() + "}");
                SendKeys.SendWait(message);                
            }
            catch(Exception ex)
            {
                Error(ex.Message);
            }

            
        }

        public void Stop()
        {
            try
            {
                if (sr != null)
                {
                    sr.IsRunning = false;
                    sr.Disconnect();
                    sr.Weight -= Weight;
                    sr.Error -= Error;
                }
                sr = null;

                if (m_weightThread != null)
                    m_weightThread.Abort();
                m_weightThread = null;

                InterceptKeys.OnCaptureKey -= SendWeight;
                InterceptKeys.KeysToHandle.Clear();

                m_viewModel.Error = String.Empty;
            }
            catch (ThreadAbortException ex)
            {
                Error(ex.Message);
            }
            catch (Exception ex)
            {
                Error(ex.Message);
                throw ex;
            }
            
        }

        private readonly int maxNoWeightExceptionsCount = 2; //sometimes the weight is not read correctly, it helps us not to loose the weight display
        private static int notWeightExceptionsCount = 0;
        private void Weight(object sender, WeightArgs e)
        {
            try
            {
                double weight;
                if (double.TryParse(e.Weight, out weight) && m_viewModel.Multiple > 0)
                {
                    m_viewModel.Weight = String.Format("{0:G}", Math.Round(weight * m_viewModel.Multiple, 4));
                    notWeightExceptionsCount = 0;
                }
                else
                {
                    notWeightExceptionsCount++;
                    if (notWeightExceptionsCount > maxNoWeightExceptionsCount)
                        m_viewModel.Weight = e.Weight;
                }
                m_viewModel.ColorStable = (!e.Stable ? Brushes.Gray : Brushes.Green);
                m_viewModel.ColorGross = (!e.Gross ? Brushes.Gray : Brushes.Green);
                m_viewModel.ColorUnder = (!e.Under ? Brushes.Gray : Brushes.Green);
                m_viewModel.ColorOver = (!e.Over ? Brushes.Gray : Brushes.Green);
                m_viewModel.Error = String.Empty;
                if (m_Config.AutoComCheck)
                    IsFoundCom = true;
            }
            catch (Exception ex)
            {
                Error(ex.Message);
                throw ex;
            }
            
        }

        public void SaveConfig()
        {
            try
            {
                m_Config.BaudRate = m_viewModel.ConnectionArgs.BaudRate;
                m_Config.DataBits = m_viewModel.ConnectionArgs.DataBits;
                m_Config.Port = m_viewModel.ConnectionArgs.Port;
                m_Config.DTR = m_viewModel.ConnectionArgs.Dtr;
                m_Config.RTS = m_viewModel.ConnectionArgs.Rts;
                m_Config.ConnectionType = m_viewModel.ConnectionArgs.Type;
                m_Config.Parity = m_viewModel.ConnectionArgs.Parity;
                m_Config.StopBits = m_viewModel.ConnectionArgs.StopBits;
                m_Config.Multiple = m_viewModel.Multiple;
                m_Config.ScaleHeader = m_viewModel.ScaleHeader;
                m_Config.ComPort = m_viewModel.ConnectionArgs.Com;
                m_Config.IP = m_viewModel.ConnectionArgs.IP;
                m_Config.MainColor = m_viewModel.MainColor;
                m_Config.FinishChar = m_viewModel.FinishChar;
                m_Config.ActivationKey = m_viewModel.ActivationKey;
                m_Config.Username= m_viewModel.ConnectionArgs.Username;
                m_Config.Company = m_viewModel.Company;
                m_Config.LogFilePath = m_viewModel.LogFilePath;

                if (!m_Config.SaveConfig())
                    throw new Exception(m_Config.Error);
                m_viewModel.Error = String.Empty;
            }
            catch (Exception ex)
            {
                Error(ex.Message);
                throw ex;
            }
                
        }

        public ViewModel GetViewModel()
        {
            try
            {
                return m_viewModel;   
            }
            catch (Exception ex)
            {
                Error(ex.Message);
                throw ex;
            }
            
        }

        public string GetKey()
        {
            return _GetKey();
        }

        private string _GetKey()
        {
            return LicenseManager.Create(LicenseManager.GetLicenseDataFromFile("6274411"));
        }

        public bool LoadKey()
        {
            return _LoadKey();
        }

        private bool _LoadKey()
        {
            try
            {
                var dt = LicenseManager.IsKeyOK(m_viewModel.Key);
                if (dt == null)
                    return false;
                else
                {
                    dt.ToUpdate = true;
                    LicenseManager.LicenseFile(dt);
                    return true;
                }
            }
            catch(Exception ex)
            {
                Error(ex.Message);
                throw ex;
            }
        }
        internal Configuration GetConfig()
        {
            return m_Config;
        }

        internal long IsCompanyExists(string companyName)
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var company = db.sp_GetCompanies(-1, 0, companyName, null, null, null, false, true).Where(i => i.Name.Equals(companyName)).FirstOrDefault();
                if (company == null || company.GUID == 0)
                    return -1;

                return company.GUID;
            }
        }

        internal bool CheckUser(ConnectionArgs connectionArgs)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var user = db.sp_GetUsers(-1, 0, connectionArgs.CompanyID, null, null, null, null, connectionArgs.Username, connectionArgs.Password, null, null, true).Where(i => i.Username.ToLower().Equals(connectionArgs.Username.ToLower()) && i.Password.ToLower().Equals(connectionArgs.Password.ToLower())).FirstOrDefault();                
                return user != null;
            }
        }

        internal IEnumerable<sp_GetScales_Result> GetScales(ConnectionArgs connectionArgs)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                return db.sp_GetScales(-1, 0,connectionArgs.CompanyID,null,null,null,null,true,true).ToList();
            }
        }



        public static Process IsAppOpen()
        {
            Process curr = Process.GetCurrentProcess();
            Process[] procs = Process.GetProcessesByName(curr.ProcessName);
            foreach (Process p in procs)
            {
                if ((p.Id != curr.Id) &&
                    (p.MainModule.FileName == curr.MainModule.FileName))
                    return p;
            }
            return null;
        }       
    }
}
