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
    public class SuppliersHandler
    {
        public static void Save(sp_GetSuppliers_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveSupplier(item.GUID, item.Name, item.ID, item.Address, item.CompanyID, item.Active).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetSuppliers_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.Suppliers.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                dt.Active = false;
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetSuppliers_Result item,ref List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "ID":
                    if (!String.IsNullOrEmpty(item.ID))
                    {
                        if (!DomainObjects.IsIDValid(item.ID))
                            errors.Add("InvalidID");
                    }
                    else
                        errors.Add("MissingID");
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

        public static IEnumerable<sp_GetSuppliers_Result> Filter(sp_GetSuppliers_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetSuppliers(filter.AllowedRows, filter.GUID, filter.CompanyID, filter.Name, filter.ID, filter.Address, filter.Active).ToList().AsEnumerable();

                return data.ToList();
            }
        }
    }
}
