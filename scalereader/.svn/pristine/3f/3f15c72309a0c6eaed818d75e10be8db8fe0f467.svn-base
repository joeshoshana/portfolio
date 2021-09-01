using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocket4Net;

namespace Shkila.ScaleReaders.ScaleReaders
{
    public class J6478Reader : ScaleReader
    {
        private string m_protocol = "" ;
        public J6478Reader(ConnectionArgs conn)
            : base(conn)
        {
            byte[] a= new byte[1];
            byte[] b= new byte[1];
            a[0] = 2;
            b[0] = 3;
            m_protocol = Encoding.ASCII.GetString(a) + "B" + Encoding.ASCII.GetString(b);
        }

        public override bool Connect()
        {
            try
            {
                if (!IsConnectionArgsValid(m_ConnectionArgs))
                    return false;

                if (m_ConnectionArgs.Type == ConnectionType.Cloud)
                {
                    m_webSocket = new WebSocket(m_websocket_url);                    
                    m_webSocket.Opened += new EventHandler(websocket_Opened);
                    m_webSocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocket_Error);
                    m_webSocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
                    m_webSocket.Open();
                    return true;
                }
                else
                    return base.Connect();
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
                return false;
            }

        }

        private void websocket_MessageReceived(object sender, EventArgs e)
        {
            try
            {
                m_WeightArgs = new WeightArgs
                {
                    Weight = (e as MessageReceivedEventArgs).Message
                };
                OnWeight(m_WeightArgs);
            }
            catch(Exception ex)
            {
                OnError(ex.Message);
            }
            
        }

        private void websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
           try
           {
               if (m_webSocket.State == WebSocketState.Open)
               {
                    m_webSocket.Send(m_ConnectionArgs.ScaleID.ToString());
               }
               else
                   OnError(e.Exception.Message);
                   /*m_webSocket.Open();
               else
               {
                   if (e.Exception.GetType() == typeof(IOException))
                   {
                       m_webSocket.Close();
                       m_webSocket.Open();
                   }
                   else
                    OnError(e.Exception.Message);
               }*/
                   
           }
            catch(Exception ex)
           {
               OnError(ex.Message);
           }
            
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            //m_webSocket.Send(m_ConnectionArgs.ScaleData)
            m_webSocket.Send(m_ConnectionArgs.ScaleID.ToString());
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
                            m_serialPort.Write(m_protocol);
                            Thread.Sleep(300);
                            break;
                        case ConnectionType.Cloud:                            
                           // Thread.Sleep(300);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
            }
            
        }

        public override void SPDataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(80);
                string data = m_serialPort.ReadExisting();
                /*Console.WriteLine(data.Length);
                Console.WriteLine(data);*/

                int startWeight = data.IndexOf(":") + 1;
                int endWeight = data.IndexOf("(");
                int startUnit = endWeight + 1;
                m_WeightArgs = new WeightArgs
                {
                    Over = data.Contains("999.999"),
                    Under = data.Contains("999.999"),
                    Gross = data.Substring(0, data.IndexOf(":")).Contains("G"),
                    Weight = String.IsNullOrEmpty(data.Substring(startWeight, endWeight - startWeight).TrimStart('0')) ? "0" : Convert.ToDouble(data.Substring(startWeight, endWeight - startWeight)).ToString(),
                    Unit = data.Substring(startUnit, 2)
                };
                OnWeight(m_WeightArgs);
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
