using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Shkila.ScaleReaders;
using Shkila.LicenseManager;

namespace Tourmaline_v2
{
    class Engine
    {
        private static Engine m_instance = null;
        private static object m_object = new object();
        private string m_logFilename = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "servicelog.txt");
        private string m_confiFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Configuration.xml");
        private Configuration m_configuration = Configuration.Instance;
        private ScaleReader m_scaleReader = null;
        private TcpListener m_tcpListener = null;
        private Thread m_processThread = null;
        private Thread m_scaleThread = null;
        private string currentString = "";
        
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
                WriteToLog("Info", "Init Listener");
                InitListener();
                WriteToLog("Info", "Start Scale");
                StartScaleThread();
                WriteToLog("Info", "Start Listener");
                StartListen();

            }
            catch (Exception e)
            {
                WriteToLog("Error", e.Message);
            }
        }

        private void InitScale()
        {
            int weightID;            
            WriteToLog("Info", String.Format("Try Parse Weight Head - {0} and Com - {1}", m_configuration.WeightID, m_configuration.COM));
            if (int.TryParse(m_configuration.WeightID, out weightID))
            {

                ConnectionArgs conn= new ConnectionArgs
                {
                    BaudRate = 9600,
                    Com = m_configuration.COM,
                    DataBits = 8,
                    Parity = System.IO.Ports.Parity.None,
                    StopBits = System.IO.Ports.StopBits.One,
                    Type = ConnectionType.Serial
                };

                m_scaleReader = ScaleReader.Factory((ScaleHeaders)weightID, conn);
                m_scaleReader.Error += OnError;
                m_scaleReader.Weight += GetWeight;
                m_scaleReader.Connect();
            }

        }       

        private void InitListener()
        {
            int port;
            WriteToLog("Info", String.Format("Try Parse Port - {0}", m_configuration.Port));
            if (int.TryParse(m_configuration.Port, out port))
            {
                m_tcpListener = new TcpListener(IPAddress.Any, port);
            }
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

        private void StartListen()
        {
            m_tcpListener.Start();

            while (true)
            {
                TcpClient client = m_tcpListener.AcceptTcpClient();
                WriteToLog("Info", String.Format("Got Client"));
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                WriteToLog("Info", String.Format("Starts Client Handler"));
                clientThread.Start(client);
            }
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = null;
            int tries = 0;
            int maxTries = 0;
            if (!int.TryParse(m_configuration.MaxTries, out maxTries))
                maxTries = 10;

            WriteToLog("Info", String.Format("Max Tries {0}", maxTries));
            while (tries < maxTries)
            {
                if (!String.IsNullOrEmpty(currentString))
                {
                    WriteToLog("Info", String.Format("Get Weight {0}", currentString));
                    buffer = encoder.GetBytes(currentString);
                    currentString = "";
                    break;
                }
                else
                {
                    buffer = encoder.GetBytes("No Connection To Scale");
                    tries++;
                    WriteToLog("Info", String.Format("Tries {0}", tries));
                }
            }

            WriteToLog("Info", String.Format("Write Data"));
            clientStream.Write(buffer, 0, buffer.Length);
            WriteToLog("Info", String.Format("Flush"));
            clientStream.Flush();
        }

        private void GetWeight(object sender, WeightArgs e)
        {
            double o;
            if (double.TryParse(e.Weight, out o))
                currentString = e.Weight;
            else
                currentString = "";
        }        

        internal void StopProcess()
        {
            DisconnectScale();
            DisconnectListener();
        }

        private void DisconnectListener()
        {
            WriteToLog("Info", String.Format("Stop Listener"));
            if (m_tcpListener != null)
                m_tcpListener.Stop();

            m_tcpListener = null;
            WriteToLog("Info", String.Format("Listener Stoped"));
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
    }
}
