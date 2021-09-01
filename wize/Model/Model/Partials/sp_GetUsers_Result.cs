
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model
{
    public partial class sp_GetUsers_Result
    {
        public DateTime? FromBirthDate { get; set; }
        public DateTime? ToBirthDate { get; set; }
        public long AllowedRows = -1;
    }
}
