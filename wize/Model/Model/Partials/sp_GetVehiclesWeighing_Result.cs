using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model
{
    public partial class sp_GetVehiclesWeighing_Result
    {
        public DateTime? FromOutDate { get; set; }
        public DateTime? ToOutDate { get; set; }
        public long AllowedRows = -1;
        public List<long> Customers { get; set; }
        public List<long> Vehicles { get; set; }
        public List<long> Transports { get; set; }
        public List<long> InSites { get; set; }
        public List<long> OutSites { get; set; }
        public List<long> Drivers { get; set; }
        public List<long> Items { get; set; }        
    }
}
