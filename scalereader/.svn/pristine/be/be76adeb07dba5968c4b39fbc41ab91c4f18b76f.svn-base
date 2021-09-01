using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shkila.ScaleReaders.ScaleReaders
{
    public class MeravReader : ScaleReader
    {
        private string m_protocol = "w\r\n" ;
        public MeravReader(ConnectionArgs conn)
            : base(conn)
        {

        }

        public override void Process()
        {
            try
            {
                while (IsRunning)
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
            bool isNegative = data.StartsWith("-");
            bool under = data.Contains("UNDER");
            bool over = data.Contains("OVER");
            string weight = (isNegative ? "-" : "") + data.Substring(1).Trim().TrimStart('0');            
            return new WeightArgs
                {
                    Over = under,
                    Under = over,
                    Weight = (!over && !under? (isNegative ? "-" : "") + data.Substring(1).Trim().TrimStart('0'): Empty)
                };
        }
    }
}
