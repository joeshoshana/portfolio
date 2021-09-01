using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shkila.ScaleReaders.ScaleReaders
{
    public class J5478Reader : ScaleReader
    {
        private string m_protocol = "" ;
        public J5478Reader(ConnectionArgs conn)
            : base(conn)
        {
            byte[] a= new byte[1];
            byte[] b= new byte[1];
            a[0] = 2;
            b[0] = 3;
            m_protocol = Encoding.ASCII.GetString(a) + "B" + Encoding.ASCII.GetString(b);
        }

        public override void Process()
        {
            try
            {
                while (IsRunning)
                {
                    //m_serialPort.Write(m_protocol);
                    //Thread.Sleep(80);
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
                byte[] toBytes = new byte[6];
                m_serialPort.Read(toBytes, 0, 6);
                m_serialPort.DiscardInBuffer();


                string byteStr = Convert.ToString(toBytes[1], 2).PadLeft(8, '0');
                string data = BitConverter.ToString(toBytes, 2);
                string[] dt = data.Split('-');
                double weight = 0;
                for (int i = dt.Length - 1; i >= 0; i--)
                    weight = weight * 100 + Convert.ToInt32(dt[i]);
                int dp = (Convert.ToInt32(byteStr.Substring(5, 3), 2) - 1) * 1;
                weight /= Math.Pow(10, dp);

                if (byteStr.Substring(2, 1) == "1")
                    weight *= -1;

                m_WeightArgs = new WeightArgs
                {
                    Weight = weight.ToString("N" + dp.ToString()),
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
