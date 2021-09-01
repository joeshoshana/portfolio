using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

namespace Shkila.Model.Handlers
{
    public class ItemsHandler
    {
        public static void Save(sp_GetItems_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveItem(item.GUID, item.Name, item.SN, item.Active, item.CompanyID, item.IsAddInForm).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetItems_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.Items.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                dt.Active = false;
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetItems_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "SN": if (String.IsNullOrEmpty(item.SN))
                        errors.Add("MissingSerialNumber");
                    break;
                case "CompanyID":
                    if (!item.CompanyID.HasValue || item.CompanyID.Value == 0)
                        errors.Add("InvalidCompany");
                    break;
                case "Active":
                    break;
                case "Name":
                    if (String.IsNullOrEmpty(item.Name))
                        errors.Add("MissingName");
                    break;      
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetItems_Result> Filter(sp_GetItems_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetItems(filter.AllowedRows, filter.GUID, filter.CompanyID, filter.Name, filter.SN, filter.Active).ToList().AsEnumerable();

                return data.ToList();
            }
        }
    }
}
