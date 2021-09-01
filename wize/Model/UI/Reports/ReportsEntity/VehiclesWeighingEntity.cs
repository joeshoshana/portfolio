using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Shkila.Model.Reports.ReportsEntity
{
    public class VehiclesWeighingEntity
    {
        public string CertifiedID { get; set; }
        public string InSite { get; set; }
        public string OutSite { get; set; }
        public string InDate { get; set; }
        public string OutDate { get; set; }
        public string InWeight { get; set; }
        public string OutWeight { get; set; }
        public string Netto { get; set; }
        public string Supplier { get; set; }
        public string Driver { get; set; }
        public string Item { get; set; }
        public string LicenseNumber { get; set; }
        public string Remarks { get; set; }
        public string User { get; set; }
        public string Customer { get; set; }
        public string CustomerID { get; set; }
        public string UpperTitle { get; set; }
        public string Logo { get; set; }
        public string LicensePlate { get; set; }
        public Image LprImage1 { get; set; }
        public Image LprImage2 { get; set; }
    }
}
