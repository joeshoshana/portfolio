using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shkila.ScaleReaders.ScaleReaders
{
    public class IPE50Reader : ScaleReader
    {
        private string m_protocol = "READ\r\n" ;
        public IPE50Reader(ConnectionArgs conn)
            : base(conn)
        {

        }

        public override void Process()
        {
            try
            {
                while (IsRunning)
                {
                    m_serialPort.Write(m_protocol);
                    Thread.Sleep(80);
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
