using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shkila.ScaleReaders.ScaleReaders
{
    public class J1000DisprintReader : ScaleReader
    {
        private string m_protocol = "" ;
        public J1000DisprintReader(ConnectionArgs conn)
            : base(conn)
        {            
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
                byte[] toBytes = new byte[512];
                int lentgh = m_serialPort.Read(toBytes, 0, 512);
                m_serialPort.DiscardInBuffer();                
                byte s = 255;
                int start = Array.LastIndexOf(toBytes,s);
                if (start < 0 || (toBytes.Length - start) < 5)
                    return;

                string byteStr = Convert.ToString(toBytes[1], 2).PadLeft(8, '0');
                string data = BitConverter.ToString(toBytes, 2, lentgh - 2);
                string[] dt = data.Split('-');
                double weight = 0;
                for (int i = dt.Length - 1; i >= 0; i--)
                {                    
                    if(i==dt.Length -1 )
                        weight = weight * 100 + (toBytes[i + 2] >= 10 ? 0 : Convert.ToInt32(dt[i]));
                    else
                        weight = weight * 100 + (toBytes[i + 2] > 153 ? Convert.ToInt32(dt[i].Substring(1)) : Convert.ToInt32(dt[i]));
                }
                    
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
