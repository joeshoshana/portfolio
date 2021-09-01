using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shkila.ScaleReaders.ScaleReaders
{
    public class WinoxReader : ScaleReader
    {
        private string m_protocol = "$01n6F\r";
        private byte[] responseBytes = new byte[1024];
        public WinoxReader(ConnectionArgs conn)
            : base(conn)
        {

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
                            Thread.Sleep(500);
                            break;
                        case ConnectionType.Tcp:
                            NetworkStream stream = m_tcpClient.GetStream();
                            stream.Write(Encoding.ASCII.GetBytes(m_protocol), 0, m_protocol.Length);
                            stream.ReadTimeout = 2000;
                            if (stream.CanRead)
                            {
                                stream.Read(responseBytes, 0, responseBytes.Length);
                                string data = Encoding.ASCII.GetString(responseBytes);
                                Console.Write(data); 
                                TcpReadData(stream);
                            }
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                OnError(ex.Message);
            }
            
        }
        private void TcpReadData(NetworkStream stream)
        {
            throw new NotImplementedException();
        }

        public override void SPDataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = m_serialPort.ReadExisting();
                if (data.Length < 9)
                {
                    Thread.Sleep(100);
                    data += m_serialPort.ReadExisting();
                }
                if (data.Length < 9)
                {
                    Thread.Sleep(100);
                    data += m_serialPort.ReadExisting();
                }
                /*Console.WriteLine(data.Length);
                Console.WriteLine(data);*/
                if(!data.Contains("?"))
                {
                    int start = data.IndexOf("n");                     
                    string weight = data.Substring(3,start - 3);                    
                    m_WeightArgs = new WeightArgs
                    {
                        Over = false,
                        Under = false,
                        Stable = true,
                        Gross = false,
                        Weight = weight.TrimStart('0'),
                        Unit = ""
                    };
                }
                
                
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
