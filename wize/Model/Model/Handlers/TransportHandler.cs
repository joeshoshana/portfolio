using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

namespace Shkila.Model.Handlers
{
    public class TransportsHandler
    {
        public static void Save(sp_GetTransports_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveTransport(item.GUID, item.Name, item.ID, item.Address, item.CompanyID, item.Active).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetTransports_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.Transports.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                dt.Active = false;
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetTransports_Result item, ref List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "ID":
                    if (!String.IsNullOrEmpty(item.ID))
                        if (!Utilities.IsIDValid(item.ID))
                            errors.Add("InvalidID");
                    break;
                case "CompanyID":
                    if (!item.CompanyID.HasValue || item.CompanyID.Value == 0)
                        errors.Add("InvalidCompany");
                    break;
                case "Active":
                    break;
                case "Name":
                    if (String.IsNullOrEmpty(item.Name))
                        errors.Add("MissingTransport");
                    break;      
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetTransports_Result> Filter(sp_GetTransports_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetTransports(filter.AllowedRows, filter.GUID, filter.CompanyID, filter.Name, filter.ID, filter.Address, filter.Active).ToList().AsEnumerable();

                return data.ToList();
            }
        }
    }
}
