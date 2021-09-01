using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shkila.ScaleReaders
{
    public enum ConnectionType
    {
        Serial,
        Tcp,
        Cloud
    }

    public enum ScaleHeaders
    {
        KernPCB,
        KernPLJ,
        KernPFB,
        KernKFS,
        KernDS,
        J1000,
        J1000Disprint,
        IPJ800,
        IPE50,
        J5478,
        J6478,
        Winox,
        Test,
        OhausExplorer,
        Merav
    }
}
