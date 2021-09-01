using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Shkila.ScaleReaders.ScaleReaders;
using System.Threading;
using System.Net;
using WebSocket4Net;

namespace Shkila.ScaleReaders
{
    public delegate void WeightEventHandler(object sender, WeightArgs e);
    public delegate void ErrorEventHandler(string error);
    public abstract class ScaleReader : IDisposable
    {
        public event WeightEventHandler Weight;
        public event ErrorEventHandler Error;

        protected ConnectionArgs m_ConnectionArgs = null;
        protected WeightArgs m_WeightArgs = null;
        protected SerialPort m_serialPort = null;
        protected TcpClient m_tcpClient = null;
        protected WebSocket m_webSocket = null;
        protected const string Empty = "- - -";

        public bool IsRunning = false;
        protected bool IsTest = false;

        protected string m_websocket_url = "wss://mishkalim.co.il/api/WeightSocket/";

        public void OnError(string error)
        {
            if(Error != null)
            {
                Error(error);
            }
        }

        public ScaleReader(ConnectionArgs connArgs, bool isTest = false)
        {
            IsTest = isTest;
            m_ConnectionArgs = connArgs;
        }

        protected virtual void OnWeight(WeightArgs e)
        {
            WeightEventHandler handler = Weight;
            if(Weight != null)
            {
                handler(this, e);
            }
        }

        public virtual bool Connect()
        {
            try
            {
                if (IsTest)
                    return true;

                if (!IsConnectionArgsValid(m_ConnectionArgs))
                    return false;

                switch (m_ConnectionArgs.Type)
                {
                    case ConnectionType.Serial:
                        m_serialPort = new SerialPort(m_ConnectionArgs.Com, m_ConnectionArgs.BaudRate, m_ConnectionArgs.Parity, m_ConnectionArgs.DataBits, m_ConnectionArgs.StopBits);
                        m_serialPort.DtrEnable = m_ConnectionArgs.Dtr;
                        m_serialPort.RtsEnable = m_ConnectionArgs.Rts;
                        m_serialPort.DataReceived += SPDataRecieved;
                        m_serialPort.Open();
                        break;
                    case ConnectionType.Tcp:
                        m_tcpClient = new TcpClient();
                        var result = m_tcpClient.BeginConnect(m_ConnectionArgs.IP, m_ConnectionArgs.Port, null, null);
                        var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
                        if (!success)
                            throw new Exception("Failed To Connect");
                        break;
                    case ConnectionType.Cloud:

                       
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
                return false;
            }
            
        }       

        protected bool IsConnectionArgsValid(ConnectionArgs connArgs)
        {
            try
            {
                if (connArgs.Type == null)
                {
                    OnError("Missing Connection Type");
                    return false;
                }

                switch (connArgs.Type)
                {
                    case ConnectionType.Serial:
                        if (String.IsNullOrEmpty(connArgs.Com))
                        {
                            OnError("Missing Com");
                            return false;
                        }
                        if (connArgs.DataBits == 0)
                        {
                            OnError("Missing Data Bits");
                            return false;
                        }
                        if (connArgs.StopBits == null)
                        {
                            OnError("Missing Stop Bits");
                            return false;
                        }
                        if (connArgs.Parity == null)
                        {
                            OnError("Missing Parity");
                            return false;
                        }
                        break;
                    case ConnectionType.Tcp:
                        if (String.IsNullOrEmpty(connArgs.IP))
                        {
                            OnError("Missing IP");
                            return false;
                        }
                        else
                        {
                            if (!IsIPValid(connArgs.IP))
                            {
                                OnError("Invalid IP");
                                return false;
                            }
                        }
                        if (connArgs.Port == 0)
                        {
                            OnError("Missing Port");
                            return false;
                        }
                        break;
                    case ConnectionType.Cloud:
                        if (String.IsNullOrEmpty(connArgs.Username))
                        {
                            OnError("Missing Username");
                            return false;
                        }
                        if (String.IsNullOrEmpty(connArgs.Password))
                        {
                            OnError("Missing Password");
                            return false;
                        }
                        if (!connArgs.CompanyID.HasValue || connArgs.CompanyID.Value == 0)
                        {
                            OnError("Missing CompanyID");
                            return false;
                        }
                        if (!connArgs.ScaleID.HasValue || connArgs.ScaleID.Value == 0)
                        {
                            OnError("Missing ScaleID");
                            return false;
                        }
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
                return false;
            }
            
        }

        private bool IsIPValid(string ipString)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(ipString))
                {
                    return false;
                }

                string[] splitValues = ipString.Split('.');
                if (splitValues.Length != 4)
                {
                    return false;
                }

                byte tempForParsing;

                return splitValues.All(r => byte.TryParse(r, out tempForParsing));
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
                return false;
            }
            
        }

        public abstract void Process();
        public abstract WeightArgs Parse(string data);

        public abstract void SPDataRecieved(object sender, SerialDataReceivedEventArgs e);

        public void Disconnect()
        {
            try
            {
                Thread.Sleep(100);
                if (m_serialPort != null)
                {
                    Thread CloseDown = new Thread(() =>
                    {
                        try
                        {
                            
                            m_serialPort.Close();
                            m_serialPort = null;                        
                        }
                        catch(Exception ex)
                        {
                            OnError(ex.Message);
                        }
                        
                    }); //close port in new thread to avoid hang
                    CloseDown.IsBackground = true;
                    CloseDown.Start(); //close port in new thread to avoid hang                    
                }
                

                if (m_tcpClient != null)
                {
                    m_tcpClient.Close();
                }
                m_tcpClient = null;

                if(m_webSocket != null)
                {
                    m_webSocket.Close();
                    m_webSocket.Dispose();
                }

                m_webSocket = null;
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
            }
            
        }


        public static ScaleReader Factory(ScaleHeaders headerType, ConnectionArgs connArgs)
        {
            switch(headerType)
            {
                case ScaleHeaders.KernPCB:
                    return new KernPCBReader(connArgs);
                case ScaleHeaders.KernPFB:
                    return new KernPFBReader(connArgs);
                case ScaleHeaders.KernKFS:
                    return new KernKFSReader(connArgs);
                case ScaleHeaders.KernDS:
                    return new KernDSReader(connArgs);
                case ScaleHeaders.KernPLJ:
                    return new KernPLJReader(connArgs);
                case ScaleHeaders.J1000:
                    return new J1000Reader(connArgs);
                case ScaleHeaders.J1000Disprint:
                    return new J1000DisprintReader(connArgs);
                case ScaleHeaders.IPJ800:
                    return new IPJ800Reader(connArgs);
                case ScaleHeaders.IPE50:
                    return new IPE50Reader(connArgs);
                case ScaleHeaders.J6478:
                    return new J6478Reader(connArgs);
                case ScaleHeaders.J5478:
                    return new J5478Reader(connArgs);
                case ScaleHeaders.Winox:
                    return new WinoxReader(connArgs);
                case ScaleHeaders.Test:
                    return new TestReader(connArgs);
                case ScaleHeaders.OhausExplorer:
                    return new OhausExplorerReader(connArgs);
                case ScaleHeaders.Merav:
                    return new MeravReader(connArgs);
                    

                default: return null;
            }
        }

        public void Dispose()
        {
            try
            {
                if (m_serialPort != null)
                {
                    m_serialPort.Close();
                    m_serialPort.Dispose();
                }
                m_serialPort = null;

                if (m_tcpClient != null)
                {
                    m_tcpClient.Close();
                }
                m_tcpClient = null;
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
            }
            
        }
    }
}
