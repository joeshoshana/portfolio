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
    public class CustomersContactsHandler
    {
        public static void Save(sp_GetCustomersContacts_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveCustomersContact(item.GUID, item.CustomerID, item.FirstName, item.LastName, item.Email, item.Phone, item.Remarks, item.Active, item.IsSendWeightsByMail, item.SendingMethodID).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetCustomersContacts_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.CustomersContacts.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                dt.Active = false;
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetCustomersContacts_Result item,ref List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "CustomerID":
                    if (item.CustomerID == null || item.CustomerID <= 0)
                        errors.Add("MissingCustomer");
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
                       if(!DomainObjects.IsDigitOnly(item.Phone))
                           errors.Add("InvalidPhone");
                   }
                   else
                       errors.Add("MissingPhone");
                    break;    
                case "Remarks":
                    break;
                case "SendingMethodID":
                    if (!item.SendingMethodID.HasValue)
                    {
                        errors.Add("InvalidSendingMethod");
                    }
                    break;   
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetCustomersContacts_Result> Filter(sp_GetCustomersContacts_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetCustomersContacts(filter.AllowedRows, filter.GUID, filter.CustomerID, filter.FirstName, filter.LastName, filter.Email, filter.Active, filter.IsSendWeightsByMail).ToList().AsEnumerable();

                return data.ToList();
            }
        }

        public static void Mail(sp_GetCustomersContacts_Result item,string id, string rptPath)
        {
            if (!DomainObjects.IsValidEmail(item.Email))
                throw new Exception("מייל לא תקין");

            List<string> to = new List<string>();
            to.Add(item.Email);
            List<string> cc = new List<string>();
            List<string> attachment = new List<string>();
            attachment.Add(rptPath);
            DomainObjects.SendMail(to, cc, String.Format("תעודה {0}", id), "", attachment);
        }

        public static void SMS(sp_GetCustomersContacts_Result item, string id, string rptPath)
        {
            
        }
    }
}
