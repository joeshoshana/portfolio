using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shkila.ScaleReaders.ScaleReaders
{
    public class J1000Reader : ScaleReader
    {
        private byte[] m_protocol = new byte[1];
        private byte[] m_protocol2 = new byte[1];
        private byte[] responseBytes = new byte[1024];
        private WebRequest m_request = null;
        public J1000Reader(ConnectionArgs conn)
            : base(conn)
        {
            m_protocol[0] = 128;
            m_protocol2[0] = 71;
        }

        public override bool Connect()
        {
            try
            {
                if (!IsConnectionArgsValid(m_ConnectionArgs))
                    return false;

                if (m_ConnectionArgs.Type == ConnectionType.Tcp)
                {
                
                    return true;
                } 
                else
                    return base.Connect(); 
            }
            catch(Exception ex)
            {
                OnError(ex.Message);
                return false;
            }
            
        }

        public override void Process()
        {
            try
            {
                while (IsRunning)
                {
                    switch (m_ConnectionArgs.Type)
                    {
                        case ConnectionType.Serial:
                           /* m_serialPort.Write(m_protocol, 0, 1);
                            m_serialPort.Write(m_protocol2, 0, 1);*/
                            break;
                        case ConnectionType.Tcp:
                            m_request = (WebRequest)HttpWebRequest.Create("http://" + m_ConnectionArgs.IP + "/NET420_DISPLAY.shtml");
                            m_request.Timeout = 2000;
                            WebResponse response = (HttpWebResponse)m_request.GetResponse();
                            StreamReader sr = new StreamReader(response.GetResponseStream());
                            TcpReadData(sr.ReadToEnd());
                            sr.Close();
                            response.Close();
                            
                            /*NetworkStream stream = m_tcpClient.GetStream();
                            byte[] send = Encoding.ASCII.GetBytes("GET /NET420_DISPLAY.shtml HTTP/1.0\r\n\r\n");
                            stream.Write(send,0,send.Length);
                            stream.ReadTimeout = 2000;
                            if (stream.CanRead)
                            {
                                Thread.Sleep(100);
                                TcpReadData(stream);
                            }*/


                            break;
                    }
                }
                
            }
            catch( IOException ex )
            {
                OnError(ex.Message);
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
            }
            
        }

        private void TcpReadData(string data)
        {
            try
            {
                int startWeightAIdx = data.IndexOf("Weight A =") + ("Weight A =").Length;
                int startWeightBIdx = data.IndexOf("Weight B =") + ("Weight B =").Length;
                int startWeightCIdx = data.IndexOf("Weight C =") + ("Weight C =").Length;
                int endWeightAIdx = data.IndexOf("</h2>", startWeightAIdx);
                int endWeightBIdx = data.IndexOf("</h2>", startWeightBIdx);
                int endWeightCIdx = data.IndexOf("</h2>", startWeightCIdx);

                string weightA = String.Empty;
                string weightB = String.Empty;
                string weightC = String.Empty;

                if(startWeightAIdx > 0 && endWeightAIdx > -1)
                    weightA = data.Substring(startWeightAIdx, endWeightAIdx - startWeightAIdx);
                if(startWeightBIdx > 0 && endWeightBIdx > -1)
                    weightB = data.Substring(startWeightBIdx, endWeightBIdx - startWeightBIdx);
                if(startWeightCIdx > 0 && endWeightCIdx > -1)
                    weightC = data.Substring(startWeightCIdx, endWeightCIdx - startWeightCIdx);

                if (String.IsNullOrEmpty(weightC))
                    return;

                m_WeightArgs = new WeightArgs
                {
                    Over = weightC.Contains("O") || weightC.Contains("V") || weightC.Contains("E"),
                    Under = weightC.Contains("D") || weightC.Contains("U") || weightC.Contains("N") || weightC.Contains("G"),
                    Weight = weightC,
                };
                OnWeight(m_WeightArgs);
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
            }
            finally
            {
                //stream.Close();
            }
        }

        public override void SPDataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(50);
                string data = m_serialPort.ReadExisting();
                int startIdx = data.IndexOf("\r");
                int endIdx = data.IndexOf("?");
                if (data.Contains("UNDER"))
                {
                    m_WeightArgs = new WeightArgs
                    {
                        Over = false,
                        Under = true
                    };
                }
                else if (data.Contains("OVER"))
                {
                    m_WeightArgs = new WeightArgs
                    {
                        Over = true,
                        Under = false
                    };
                }
                else if (startIdx >= 0 && endIdx > 0 && startIdx < endIdx)
                {
                    m_WeightArgs = new WeightArgs
                    {
                        Weight = data.Substring(startIdx + 1, endIdx - startIdx - 1).Replace(" ", "")
                    };
                }
                else
                    m_WeightArgs = new WeightArgs
                    {
                        Weight = data.Substring(startIdx + 1,7).Replace(" ", "")
                    };


                OnWeight(m_WeightArgs);
                //            Console.WriteLine(data);
                //          Console.WriteLine(data.Length);
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
            }
            
        }
        public override WeightArgs Parse(string data)
        {
            return null;
        }
    }
}
