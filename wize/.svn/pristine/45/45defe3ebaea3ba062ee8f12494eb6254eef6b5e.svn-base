using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shkila.Model.Handlers
{
    public class GoodsWeighingLinesHandler
    {
        public static void Save(sp_GetGoodsWeighingLines_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveGoodsWeighingLine(item.GUID, item.GoodsWeighingID, item.ScaleID, item.Weight, item.ItemID).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetGoodsWeighingLines_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.GoodsWeighingLines.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                db.GoodsWeighingLines.Remove(dt);
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetGoodsWeighingLines_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "ItemID":
                    break;
                case "ScaleID":
                    if (!item.ScaleID.HasValue || item.ScaleID.Value <= 0)
                        errors.Add("MissingScale");
                    break;
                case "GoodsWeighingID":
                    if (!item.GoodsWeighingID.HasValue || item.GoodsWeighingID.Value <= 0)
                        errors.Add("InvalidCertificate");
                    break;
                case "Weight":
                    if (!item.Weight.HasValue)
                        errors.Add("InvalidWeight");
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetGoodsWeighingLines_Result> Filter(sp_GetGoodsWeighingLines_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetGoodsWeighingLines(filter.AllowedRows, filter.GUID, filter.GoodsWeighingID, filter.ItemID, filter.ScaleID).ToList().AsEnumerable();

                return data.ToList();
            }
        }
    }
}
