using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shkila.ScaleReaders.ScaleReaders
{
    public class KernPFBReader : ScaleReader
    {
        private int m_bytesToRead = 18;
        private string m_protocol = "w" ;
        public KernPFBReader(ConnectionArgs conn)
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
                if (data.Contains("Error"))
                {
                    m_WeightArgs = new WeightArgs
                    {
                        Over = true,
                        Under = true
                    };
                }
                else
                {
                    string weightStr = data.Substring(1, 11).Replace(" ", "");
                    string weightUnit = String.Empty;
                    if (data.Length > 16)
                        weightUnit = data.Substring(13, 3).Replace(" ", "");
                    m_WeightArgs = new WeightArgs
                    {
                        Stable = !String.IsNullOrEmpty(weightUnit),
                        Unit = weightUnit,
                        Weight = weightStr
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
