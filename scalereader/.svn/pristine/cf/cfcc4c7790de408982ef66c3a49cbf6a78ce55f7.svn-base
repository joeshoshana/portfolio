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
    public class IPJ800Reader : ScaleReader
    {
        private string m_protocol = "READ\r\n" ;
        private byte[] responseBytes = new byte[1024];

        public IPJ800Reader(ConnectionArgs conn)
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
                        Thread.Sleep(80);
                        break;
                    case ConnectionType.Tcp:
                        NetworkStream stream = m_tcpClient.GetStream();
                        stream.Write(Encoding.ASCII.GetBytes(m_protocol),0,m_protocol.Length);
                        stream.ReadTimeout = 2000;
                        if(stream.CanRead)
                        {
                            TcpReadData(stream);
                        }
                        
                        
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
            }
        }

        private void TcpReadData(NetworkStream stream)
        {
            try
            {
                stream.Read(responseBytes, 0, responseBytes.Length);
                string data = Encoding.ASCII.GetString(responseBytes);
                string[] dt = data.Split(',');
                m_WeightArgs = new WeightArgs
                {
                    Over = dt[0].Contains("OL"),
                    Under = dt[0].Contains("UL"),
                    Stable = dt[0].Contains("ST"),
                    Gross = dt[1].Contains("G"),
                    Weight = dt[2].Replace(" ", ""),
                    Unit = dt[3].Substring(0, 2)
                };
                OnWeight(m_WeightArgs);
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
            }
            
        }

        private void TcpWriteData(IAsyncResult ar)
        {
            try
            {
                NetworkStream stream = ar.AsyncState as NetworkStream;
                stream.EndWrite(ar);
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
                string data = m_serialPort.ReadLine();
                /*Console.WriteLine(data.Length);
                Console.WriteLine(data);*/
                string[] dt = data.Split(',');
                m_WeightArgs = new WeightArgs
                {
                    Over = dt[0].Contains("OL"),
                    Under = dt[0].Contains("UL"),
                    Stable = dt[0].Contains("ST"),
                    Gross = dt[1].Contains("G"),
                    Weight = dt[2].Replace(" ", ""),
                    Unit = dt[3].Substring(0, 2)
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
            string[] dt = data.Split(',');
            return new WeightArgs
            {
                Over = dt[0].Contains("OL"),
                Under = dt[0].Contains("UL"),
                Stable = dt[0].Contains("ST"),
                Gross = dt[1].Contains("G"),
                Weight = dt[2].Replace(" ", ""),
                Unit = dt[3].Substring(0, 2)
            };
        }

    }
}
