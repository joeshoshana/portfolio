using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shkila.ScaleReaders.ScaleReaders
{
    public class OhausExplorerReader : ScaleReader
    {
        private string m_protocol = "P\r\n" ;
        public OhausExplorerReader(ConnectionArgs conn)
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
                            break;                        
                    }                   
                }
            }
            catch(Exception ex)
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
                                
                m_WeightArgs = Parse(data);

                OnWeight(m_WeightArgs);
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
            }
               
        }

        public override WeightArgs Parse(string data)
        {
            return  new WeightArgs
            {
                Stable = !data.Contains("?"),
                Weight = data.Substring(0, 9).Trim(),
                Unit = data.Substring(10, 5).Trim()
            };
        }
    }
}
