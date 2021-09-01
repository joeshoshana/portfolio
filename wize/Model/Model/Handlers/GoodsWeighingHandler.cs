using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shkila.Model.Handlers
{
    public class GoodsWeighingHandler
    {
        public static void Save(sp_GetGoodsWeighing_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveGoodsWeighing(item.GUID, item.CompanyID, item.Certificate, item.SupplierID, item.Amount, item.Count, item.Date).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetGoodsWeighing_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.GoodsWeighing.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                db.GoodsWeighing.Remove(dt);
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetGoodsWeighing_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "Date":
                    break;
                case "Certificate":
                    if (String.IsNullOrEmpty(item.Certificate))
                        errors.Add("MissingCertificate");
                    break;
                case "CompanyID":
                    if (!item.CompanyID.HasValue || item.CompanyID.Value == 0)
                        errors.Add("InvalidCompany");
                    break;
                case "SupplierID":
                    if (item.SupplierID == null || item.SupplierID <= 0)
                        errors.Add("MissingSupplier");
                    break;
                case "Amount":
                    break;
                case "Count":
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetGoodsWeighing_Result> Filter(sp_GetGoodsWeighing_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetGoodsWeighing(filter.AllowedRows, filter.GUID, filter.CompanyID, filter.SupplierID, filter.Certificate).ToList().AsEnumerable();
                
                return data.ToList();
            }
        }
    }
}
