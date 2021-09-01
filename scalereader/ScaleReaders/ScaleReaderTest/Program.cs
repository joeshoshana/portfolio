using Shkila.ScaleReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleReaderTest
{
    class Program
    {
        static void Main(string[] args)
        {
             ScaleReader SR = ScaleReader.Factory(ScaleHeaders.KernKFS, new ConnectionArgs { Type = ConnectionType.Serial, BaudRate = 9600, Com = "COM15", DataBits = 8, Parity = System.IO.Ports.Parity.None, StopBits = System.IO.Ports.StopBits.One, IP = "100.100.101.171", Username="joe",Password="6274411",CompanyID=1,ScaleID=2, Port=10001});
            
            SR.Weight += SR_Weight;
            SR.Error += OnError;

            if (!SR.Connect())
                return;

             SR.IsRunning = true;
             SR.Process();
             while (true) ;
        }

        private static void OnError(string error)
        {
            Console.WriteLine(error);
        }

        private static void SR_Weight(object sender, WeightArgs e)
        {
            Console.WriteLine("Weight" + e.Weight);
            Console.WriteLine("Over" + e.Over);
            Console.WriteLine("Stable" + e.Stable);
            Console.WriteLine("Under" + e.Under);
            Console.WriteLine("Unit" + e.Unit);
        }

    }
}
