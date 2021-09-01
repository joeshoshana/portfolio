using Shkila.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

namespace Shkila.Model.Handlers
{
    public class DriversHandler
    {
        public static void Save(sp_GetDrivers_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                
                var a = db.sp_SaveDriver(item.GUID, item.Name, item.ID, item.CompanyID, item.Active, item.Code, item.IsAddInForm).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetDrivers_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.Drivers.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                dt.Active = false;
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetDrivers_Result item, ref List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "ID":
                    if (!String.IsNullOrEmpty(item.ID))
                        if (!DomainObjects.IsIDValid(item.ID))
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
                        errors.Add("MissingDriver");
                    break;      
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetDrivers_Result> Filter(sp_GetDrivers_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetDrivers(filter.AllowedRows, filter.GUID, filter.CompanyID, filter.Name, filter.ID, filter.Active, filter.Code).ToList().AsEnumerable();

                return data.ToList();
            }
        }
    }
}
