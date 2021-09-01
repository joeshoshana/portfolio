using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Shkila.ScaleReaders;
using Shkila.LicenseManager;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Service_Windows
{
    class Engine
    {
        private static Engine m_instance = null;
        private static object m_object = new object();
        private string m_logFilename = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "servicelog.txt");
        private string m_confiFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Configuration.xml");
        private Configuration m_configuration = Configuration.Instance;
        private ScaleReader m_scaleReader = null;
        private Thread m_processThread = null;
        private Thread m_scaleThread = null;

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
            StreamWriter sw = new StreamWriter(m_logFilename, true);
            sw.WriteLine(String.Format("{0} - {1} - {2}", DateTime.Now, msgType, msg));
            sw.Close();
        }

        public void StartProcess()
        {
            try
            {
                WriteToLog("Info", "Load Configuration");
                if (!m_configuration.LoadConfig(m_confiFileName))
                    throw new Exception(m_configuration.Error);

                WriteToLog("Info", "Init Scale");
                InitScale();
                WriteToLog("Info", "Start Scale");
                StartScaleThread();

            }
            catch (Exception e)
            {
                WriteToLog("Error", e.Message);
            }
        }

        private void InitScale()
        {
            WriteToLog("Info", String.Format("Try Parse Weight Head - {0} and Com - {1}", m_configuration.Weight, m_configuration.COM));
            ScaleHeaders header;
            if (Enum.TryParse(m_configuration.Weight, out header))
            {
                   ConnectionArgs conn = new ConnectionArgs
                {
                    BaudRate = 9600,
                    Com = m_configuration.COM,
                    DataBits = 8,
                    Parity = System.IO.Ports.Parity.None,
                    StopBits = System.IO.Ports.StopBits.One,
                    Type = ConnectionType.Serial
                };

                   m_scaleReader = ScaleReader.Factory(header, conn);
                m_scaleReader.Error += OnError;
                m_scaleReader.Weight += GetWeight;
                m_scaleReader.Connect();
            }      
            else
                WriteToLog("Error", "Failed in Parse Header");
        }

        private void StartScaleThread()
        {
            m_scaleThread = new Thread(() =>
            {
                StartScale();
            });
            m_scaleThread.IsBackground = true;
            m_scaleThread.Start();
        }

        private void StartScale()
        {
            m_scaleReader.IsRunning = true;
            m_scaleReader.Process();
        }

        private void GetWeight(object sender, WeightArgs e)
        {
            double o;
            if (double.TryParse(e.Weight, out o))
            {
                Request req = new Request();
                req.Command = "update_weight";
                bool isDemo = false;
                bool.TryParse(m_configuration.IsDemo, out isDemo);

                if (isDemo)
                {
                    req.MAC = "demo";
                    req.Weight = "1258.35";
                }
                else
                {
                    req.MAC = GetMac();
                    req.Weight = e.Weight;
                }

                if (String.IsNullOrEmpty(req.MAC))
                {
                    WriteToLog("Error", "Empty Mac");
                    return;
                }                

                req.Send(m_configuration.Web, Response);
            }
        }

        private void Response(string resp)
        {
            if(!resp.Contains("OK"))
                WriteToLog("Info", resp);
        }

        private string GetMac()
        {
            return GetVolumeSerial("C");
        }

        [DllImport("kernel32.dll")]
        private static extern long GetVolumeInformation(string PathName, StringBuilder VolumeNameBuffer, UInt32 VolumeNameSize, ref UInt32 VolumeSerialNumber, ref UInt32 MaximumComponentLength, ref UInt32 FileSystemFlags, StringBuilder FileSystemNameBuffer, UInt32 FileSystemNameSize);
        public static string GetVolumeSerial(string strDriveLetter)
        {
            uint serNum = 0;
            uint maxCompLen = 0;
            StringBuilder VolLabel = new StringBuilder(256); // Label
            UInt32 VolFlags = new UInt32();
            StringBuilder FSName = new StringBuilder(256); // File System Name
            strDriveLetter += ":\\"; // fix up the passed-in drive letter for the API call
            long Ret = GetVolumeInformation(strDriveLetter, VolLabel, (UInt32)VolLabel.Capacity, ref serNum, ref maxCompLen, ref VolFlags, FSName, (UInt32)FSName.Capacity);

            return Convert.ToString(serNum);
        }

        internal void StopProcess()
        {
            DisconnectScale();
        }

        private void DisconnectScale()
        {
            WriteToLog("Info", String.Format("Stop Scale"));
            if (m_scaleReader != null)
            {
                m_scaleReader.IsRunning = false;
                m_scaleReader.Error -= OnError;
                m_scaleReader.Weight -= GetWeight;
                m_scaleReader.Disconnect();
            }
            m_scaleReader = null;
            m_scaleThread.Abort();
            WriteToLog("Info", String.Format("Scale Stoped"));
        }

        private void OnError(string error)
        {
            WriteToLog("Error", String.Format("Scale Stoped"));
        }

        internal void StartProcessThread()
        {
            m_processThread = new Thread(() =>
            {
                StartProcess();
            });
            m_processThread.IsBackground = true;
            m_processThread.Start();
        }

        internal void StopProcessThread()
        {
            try
            {
                StopProcess();
                m_processThread.Abort();
            }
            catch (Exception e)
            {
                WriteToLog("Error", e.Message);
            }

        }

        internal string IsLicenseValid()
        {
            string error = String.Empty;
            LicenseManager.LicenseProcees(ref error);
            return error;
        }

        internal void Start()
        {
            Process proc = new Process();
            proc.StartInfo.WorkingDirectory = @"C:\Wize";
            proc.StartInfo.FileName = "Wize.bat";
            proc.StartInfo.CreateNoWindow = false;
            proc.Start();
            //proc.WaitForExit();
        }
    }
}
