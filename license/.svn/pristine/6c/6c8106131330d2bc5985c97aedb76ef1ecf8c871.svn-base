using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shkila.LicenseManager
{
    public class LicenseData
    {
        public Products ProductID { get; set; }
        public DateTime StartDate { get; set; }
        public int Period { get; set; }
        public LicenseType LicenseType { get; set; }
        public bool ToUpdate { get; set; }
        public bool ToRegistry { get; set; }
        public string MacAddress { get; set; }

        private const string m_dateFormat = "ddMMyyyyHHmmss";
        private const int m_keySize = 22;
        public override string ToString()
        {
            return String.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}",
                this.ProductID.ToString("D").PadLeft(3, '0'),
                this.LicenseType.ToString("D").PadLeft(2, '0'),
                this.StartDate.ToString(m_dateFormat),
                this.Period.ToString().PadLeft(4, '0'),
                this.ToUpdate.ToString(),
                this.ToRegistry.ToString(),
                this.MacAddress);
        }

        public static LicenseData ToLicenseData(string key)
        {
            if (String.IsNullOrEmpty(key))
                return null;

            LicenseData dt = new LicenseData();
            /*if (key.Length > m_keySize || key.Length < m_keySize)
                return null;*/
            string[] data = key.Split('-');

            if (data.Length != 7 && data.Length != 6)
                return null;

            string productId = data[0];// key.Substring(0, 3);
            string licenseType = data[1];//key.Substring(3, 2);
            string startDate = data[2];//key.Substring(5, 14);
            string period = data[3];//key.Substring(19, 3);
            string toUpdate = data[4];
            string toRegistry = data[5];

            string macAddress = String.Empty;
            if (data.Length > 6)
                macAddress = data[6];

            Products prodId;
            if (!Enum.TryParse<Products>(productId, out prodId))
                return null;

            dt.ProductID = prodId;

            LicenseType licType;
            if (!Enum.TryParse<LicenseType>(licenseType, out licType))
                return null;

            dt.LicenseType = licType;

            DateTime sttDate;
            if (!DateTime.TryParseExact(startDate, m_dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out sttDate))
                return null;

            dt.StartDate = sttDate;

            int pr;
            if (!int.TryParse(period, out pr))
                return null;

            dt.Period = pr;

            bool tUpdt;
            if (!bool.TryParse(toUpdate, out tUpdt))
                return null;
            
            dt.ToUpdate = tUpdt;

            bool tReg;
            if (!bool.TryParse(toRegistry, out tReg))
                return null;

            dt.ToRegistry = tReg;

            if (!String.IsNullOrEmpty(macAddress))
                dt.MacAddress = macAddress;

            return dt;
        }
    }
}
