using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shkila.ScaleReaders.ScaleReaders
{
    public class TestReader : ScaleReader
    {
        private string m_protocol = "";
        public TestReader(ConnectionArgs conn)
            : base(conn, true)
        {

        }

        public override void Process()
        {
            try
            {
                while (IsRunning)
                {
                    Random rand = new Random();
                    m_WeightArgs = new WeightArgs
                    {
                        Gross = (rand.Next(2) == 1),
                        Over = (rand.Next(2) == 1),
                        Stable = (rand.Next(2) == 1),
                        Under = (rand.Next(2) == 1),
                        Unit = (rand.Next(2) == 1 ? "Kg" : "g"),
                        Weight = rand.Next(7800).ToString()
                    };
                    OnWeight(m_WeightArgs);
                    Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
            }
            
        }

        public override void SPDataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            
        }

        public override WeightArgs Parse(string data)
        {
            return null;
        }
    }
}
