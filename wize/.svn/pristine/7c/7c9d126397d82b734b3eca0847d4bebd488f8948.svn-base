using Shkila.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class UsersHandler
    {
        public static void Save(sp_GetUsers_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveUser(item.GUID, item.FirstName, item.LastName, item.ID, item.BirthDate, item.Username, item.Password, item.CompanyID, item.Active, item.Email, item.DefaultScaleID, item.DefaultFormID, item.LanguageID, item.PermissionID, item.DriverID).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetUsers_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.Users.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                dt.Active = false;
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetUsers_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "FirstName":
                    if (!String.IsNullOrEmpty(item.FirstName))
                        errors.Add("MissingLastName");
                    break;
                case "LastName":
                    if (!String.IsNullOrEmpty(item.FirstName))
                        errors.Add("MissingFirstName");
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
                case "BirthDate":
                    break;
                case "Username":
                    if (String.IsNullOrEmpty(item.Username))
                        errors.Add("MissingUsername");
                    break;
                case "Password":
                    if (String.IsNullOrEmpty(item.Password))
                        errors.Add("MissingPassword");
                    break;
                case "CompanyID":
                    if (!item.CompanyID.HasValue || item.CompanyID.Value == 0)
                        errors.Add("InvalidCompany");
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
                case "Active":
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetUsers_Result> Filter(sp_GetUsers_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetUsers(filter.AllowedRows, filter.GUID, filter.CompanyID, filter.DefaultFormID, filter.DefaultScaleID, filter.FirstName, filter.LastName, filter.Username, filter.Password, filter.ID, filter.Email, filter.Active).ToList().AsEnumerable();

                if(filter.FromBirthDate != null)
                    data = data.Where(i => i.BirthDate >= filter.FromBirthDate);

                if (filter.ToBirthDate != null)
                    data = data.Where(i => i.BirthDate < filter.ToBirthDate.Value.AddDays(1));

                data = data.Where(i =>  i.Active == filter.Active);

                return data.ToList();
            }
        }

        public static long? DefaultScale(sp_GetUsers_Result user)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_GetUserScale(user.GUID).ToList();
                if (a.Count() > 0)
                    return a.ElementAt(0);
                else
                    return null;
            }
        }

       
    }
}
