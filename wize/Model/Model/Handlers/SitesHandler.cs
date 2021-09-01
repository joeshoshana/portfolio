using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

namespace Shkila.Model.Handlers
{
    public class SitesHandler
    {
        public static void Save(sp_GetSites_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveSite(item.GUID, item.Name, item.CompanyID, item.Active, item.IsAddInForm).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetSites_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.Sites.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                dt.Active = false;
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetSites_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
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

        public static IEnumerable<sp_GetSites_Result> Filter(sp_GetSites_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetSites(filter.AllowedRows, filter.GUID, filter.CompanyID, filter.Name, filter.Active).ToList().AsEnumerable();

                return data.ToList();
            }
        }
    }
}
