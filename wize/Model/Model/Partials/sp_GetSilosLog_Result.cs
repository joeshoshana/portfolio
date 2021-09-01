using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shkila.Model
{
    public partial class sp_GetSilosLog_Result
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long AllowedRows = -1;
    }
}
