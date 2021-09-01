using Shkila.ScaleReaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using Shkila.LicenseManager;
using System.Windows.Media;

namespace TourmalineUI
{
    class Engine : IDisposable
    {
        private static Engine m_instance = null;
        private static object m_object = new object();        
        private Configuration m_configuration = Configuration.Instance;
        private string m_logFilename = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "log.txt");
        private string m_configFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Configuration.xml");        
        private ViewModel m_viewModel = new ViewModel();
        private string m_ServiceFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Tourmaline_v2.exe");
        private string m_serviceName = "Tourmaline 2.0";
        private Timer m_checkStatus = null;
        private Engine()
        {
            if (!m_configuration.LoadConfig(m_configFileName))
                throw new Exception(m_configuration.Error);

            m_viewModel.Language = (Language)Enum.Parse(typeof(Language), m_configuration.Lang);
            m_viewModel.Port = m_configuration.Port;
            m_viewModel.SelectedCom = m_configuration.COM;            


            string error = String.Empty;
            LicenseManager.LicenseProcees(ref error);
            LicenseData dt = LicenseManager.GetLicenseDataFromFile("6274411");
            string daysLeft = "";
            if (dt.LicenseType == LicenseType.Subscription)
                daysLeft = dt.StartDate.AddDays(dt.Period + 1).Subtract(DateTime.Now).Days.ToString();

            if (String.IsNullOrEmpty(error))
            {
                m_viewModel.License = daysLeft;
                m_viewModel.LicenseColor = Brushes.Green;
            }
            else
            {
                m_viewModel.License = Dictionary.Invalid;
                m_viewModel.LicenseColor = Brushes.Red;
                WriteToLog("Error", error);
            }

            ModelsToScaleConverter.LoadData();
                      
            m_viewModel.Manufacturers =new System.Collections.ObjectModel.ObservableCollection<string>(ModelsToScaleConverter.GetManufacturers());            
            SetModels();

            m_viewModel.Languages = new System.Collections.ObjectModel.ObservableCollection<Language>(Enum.GetValues(typeof(Language)).Cast<Language>());
            
            m_viewModel.SelectedManufacturer = ModelsToScaleConverter.GetManufacturer((ScaleHeaders)Enum.Parse(typeof(ScaleHeaders), m_configuration.WeightID));
            m_viewModel.SelectedModel = m_viewModel.SelectedManufacturer + "," + ModelsToScaleConverter.GetModel((ScaleHeaders)Enum.Parse(typeof(ScaleHeaders), m_configuration.WeightID));
            
            m_checkStatus = new Timer(CheckStatus, null, 1000, 1000 * 2);
        }

        private void CheckStatus(object state)
        {
            if (!DoesServiceExist())
            {
                m_viewModel.Service = Dictionary.NotInstalled;
                m_viewModel.ServiceColor = Brushes.Orange;
            }
            else
                using (ServiceController service = new ServiceController(m_serviceName))
                {
                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        m_viewModel.Service = Dictionary.Started;
                        m_viewModel.ServiceColor = Brushes.Green;
                    }
                    else if (service.Status == ServiceControllerStatus.Stopped)
                    {
                        m_viewModel.Service = Dictionary.Stopped;
                        m_viewModel.ServiceColor = Brushes.Red;
                    }
                }           
        }

        public ViewModel GetViewModel()
        {
            return m_viewModel;
        }

        public static Engine Instance
        {
            get
            {
                lock (m_object)
                {
                    if (m_instance == null)
                        m_instance = new Engine();

                    return m_instance;
                }
            }
        }

        public void WriteToLog(string msgType, string msg)
        {
            string log = String.Format("{0} - {1} - {2}", DateTime.Now, msgType, msg);
            m_viewModel.Log = log + Environment.NewLine;
            StreamWriter sw = new StreamWriter(m_logFilename, true);
            sw.WriteLine(log);
            sw.Close();
        }

        public void SetModels(string manufacturer = null)
        {
            m_viewModel.Models = new System.Collections.ObjectModel.ObservableCollection<string>(ModelsToScaleConverter.GetModels(manufacturer));                
        }

        public static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        public void SaveConfig()
        {
            try
            {
                WriteToLog("Info", Dictionary.SaveChanges);
                if (String.IsNullOrEmpty(m_viewModel.SelectedModel) || String.IsNullOrEmpty(m_viewModel.SelectedManufacturer))
                    throw new Exception(Dictionary.CantsaveMissingManufOrModel);

                m_configuration.Port = m_viewModel.Port;
                m_configuration.COM = m_viewModel.SelectedCom;
                m_configuration.Lang = ((int)m_viewModel.Language).ToString();
                m_configuration.WeightID = ((int)ModelsToScaleConverter.GetScale(m_viewModel.SelectedManufacturer, m_viewModel.SelectedModel.Split(',')[1])).ToString();
                m_configuration.SaveConfig(m_configFileName);

                WriteToLog("Info", Dictionary.ChangesSaved);
                WriteToLog("Info", Dictionary.RestartForChanges);
            }
            catch(Exception ex)
            {
                WriteToLog("Error",ex.Message);
            }
            
        }

        public void InstallService()
        {
            try
            {
                if (DoesServiceExist())
                    throw new Exception(Dictionary.ServiceExist);

                WriteToLog("Info", Dictionary.InstallService);

                System.Configuration.Install.AssemblyInstaller Installer = new System.Configuration.Install.AssemblyInstaller(m_ServiceFilePath, null);
                Installer.UseNewContext = true;                
                Installer.Install(null);
                Installer.Commit(null);
                WriteToLog("Info", Dictionary.ServiceInstalled);
            }
            catch(Exception ex)
            {
                WriteToLog("Error", ex.Message);
            }
            
        }

        bool DoesServiceExist()
        {
            return ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals(m_serviceName));
        }

        public void UninstallService()
        {
            try
            {                
                if (!DoesServiceExist())
                    throw new Exception(Dictionary.ServiceNotExist);

                WriteToLog("Info", Dictionary.RemoveService);

                System.Configuration.Install.AssemblyInstaller Installer = new System.Configuration.Install.AssemblyInstaller(m_ServiceFilePath, null);
                Installer.UseNewContext = true;
                Installer.Uninstall(null);

                WriteToLog("Info", Dictionary.ServiceRemoved);
            }
            catch(Exception ex)
            {
                WriteToLog("Error", ex.Message);
            }            
        }

        public void StartService()
        {
            try
            {                
                if (!DoesServiceExist())
                    throw new Exception(Dictionary.ServiceNotExist);

                WriteToLog("Info", Dictionary.StartService);
                ServiceController service = new ServiceController(m_serviceName);
                try
                {
                    
                    int millisec1 = Environment.TickCount;
                    TimeSpan timeout = TimeSpan.FromMilliseconds(5000);

                    int millisec2 = Environment.TickCount;
                    timeout = TimeSpan.FromMilliseconds(5000 - (millisec2 - millisec1));

                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                }
                catch
                {

                }
                WriteToLog("Info", Dictionary.ServiceStarted);
            }
            catch(Exception ex)
            {
                WriteToLog("Error", ex.Message);
            }
            
        }

        public void StopService()
        {
            try
            {
               
                if (!DoesServiceExist())
                    throw new Exception(Dictionary.ServiceNotExist);

                WriteToLog("Info", Dictionary.StopService);

                ServiceController service = new ServiceController(m_serviceName);
                try
                {
                    int millisec1 = Environment.TickCount;
                    TimeSpan timeout = TimeSpan.FromMilliseconds(5000);

                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                }
                catch
                {

                }

                WriteToLog("Info", Dictionary.ServiceStopped);
            }
            catch(Exception ex)
            {
                WriteToLog("Error", ex.Message);
            }
        }

        public void TestService()
        {
            try
            {                
                WriteToLog("Info", "מתחיל בדיקה");
                TcpClient client = new TcpClient();
                //client.ReceiveTimeout = 3000;
                WriteToLog("Info", "שולח בקשה לשירות המקומי");
                client.Connect("127.0.0.1", Convert.ToInt32(m_viewModel.Port));                
                var stream = client.GetStream();
                //stream.ReadTimeout = 2000;
                if(stream.CanRead)
                {
                    byte[] bytes = new byte[1024];

                    WriteToLog("Info", "מנסה לקרוא תגובה");
                    int numOfBytes = stream.Read(bytes, 0, bytes.Length);
                    WriteToLog("Info", String.Format("נמצאו {0} תווים", numOfBytes));
                    byte[] data = bytes.Take(numOfBytes).ToArray();
                    m_viewModel.Weight = Encoding.ASCII.GetString(data);
                    WriteToLog("Info", String.Format("תגובה {0} התקבלה", m_viewModel.Weight));
                }
                                
            }
            catch (Exception ex)
            {
                WriteToLog("Error", ex.Message);
            }
        }

        public void Dispose()
        {
            m_checkStatus.Dispose();            
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
            catch (Exception ex)
            {
                WriteToLog("Error", ex.Message);
                return false;
            }
        }
    }
}
