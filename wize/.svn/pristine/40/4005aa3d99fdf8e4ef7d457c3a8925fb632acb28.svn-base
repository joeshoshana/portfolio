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
    public class SuppliersContactsHandler
    {
        public static void Save(sp_GetSuppliersContacts_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveSuppliersContact(item.GUID, item.SupplierID,item.FirstName,item.LastName,item.Email,item.Phone,item.Remarks,item.Active).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetSuppliersContacts_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.SuppliersContacts.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                dt.Active = false;
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetSuppliersContacts_Result item,ref List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "SupplierID":
                    if (item.SupplierID == null || item.SupplierID <= 0)
                        errors.Add("MissingSupplier");
                    break;
                case "FirstName":
                    if (String.IsNullOrEmpty(item.FirstName))
                        errors.Add("MissingFirstName");
                    break;
                case "Active":
                    break;
                case "LastName":
                    if (String.IsNullOrEmpty(item.LastName))
                        errors.Add("MissingLastName");
                    break;    
                case "Email":
                    if (!String.IsNullOrEmpty(item.Email))
                    {
                        if (!DomainObjects.IsValidEmail(item.Email))
                            errors.Add("InvalidEmail");
                    }
                    else
                        errors.Add("MissingEmail");
                   break;
                case "Phone":
                   if (!String.IsNullOrEmpty(item.Phone))
                   {
                       if (!DomainObjects.IsDigitOnly(item.Phone))
                           errors.Add("InvalidPhone");
                   }
                   else
                       errors.Add("MissingPhone");
                    break;    
                case "Remarks":
                    break;    
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetSuppliersContacts_Result> Filter(sp_GetSuppliersContacts_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetSuppliersContacts(filter.AllowedRows, filter.GUID, filter.SupplierID, filter.FirstName, filter.LastName, filter.Email, filter.Active).ToList().AsEnumerable();

                return data.ToList();
            }
        }
    }
}
