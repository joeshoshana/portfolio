using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class CompaniesFormsFieldsHandler
    {
        public static void Save(sp_GetCompaniesFormsFields_Result item)
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveCompaniesFormsField(item.GUID,item.CompanyID,item.FormsFieldID,item.FormID,item.ValidationRequired, item.IsShowing).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetCompaniesFormsFields_Result item)
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.CompaniesFormsFields.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                db.CompaniesFormsFields.Remove(dt);
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetCompaniesFormsFields_Result item, List<string> errors)
        {
            switch(field)
            {
                case "GUID":
                    break;
                case "CompanyID":
                    if(!item.CompanyID.HasValue || item.CompanyID.Value == 0)
                        errors.Add("InvalidCompany");
                    break;
                case "FormsFieldID":
                    if (!item.FormsFieldID.HasValue || item.FormsFieldID.Value == 0)
                        errors.Add("InvalidFormsField");
                    break;
                case "FormID":
                    if (!item.FormID.HasValue || item.FormID.Value == 0)
                        errors.Add("InvalidForm");
                    break;
                case "ValidationRequired":
                    break;
                case "IsShowing":
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetCompaniesFormsFields_Result> Filter(sp_GetCompaniesFormsFields_Result filter)
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetCompaniesFormsFields(filter.GUID, filter.CompanyID, filter.FormsFieldID, filter.FormID).ToList().AsEnumerable();

                return data.ToList();
            }
        }

        public static List<sp_GetCompaniesFormsFields_Result> UpdateFields(List<sp_GetCompaniesFormsFields_Result> companyFormsFields, IEnumerable<sp_GetFormsFields_Result> formsFields, long companyID)
        {
            foreach (sp_GetFormsFields_Result formField in formsFields)
            {
                if (companyFormsFields.Any(i => i.FormsFieldID == formField.GUID))
                    continue;
                companyFormsFields.Add(
                    new sp_GetCompaniesFormsFields_Result
                    {
                        CompanyID = companyID,
                        FormsFieldName = formField.Name,
                        FormsFieldID = formField.GUID,
                        FormID = formField.FormID,
                        IsShowing = true,
                        ValidationRequired = false
                    });
            }
            foreach (sp_GetCompaniesFormsFields_Result cff in companyFormsFields)
                Save(cff);

            return companyFormsFields;
        }

        public static IEnumerable<sp_GetCompaniesFormsFields_Result> InvisibleFields(sp_GetCompaniesFormsFields_Result data)
        {
            return Filter(data).Where(i => i.IsShowing == false);
        }

        public static IEnumerable<sp_GetCompaniesFormsFields_Result> RequiredFields(sp_GetCompaniesFormsFields_Result data)
        {
            return Filter(data).Where(i => i.ValidationRequired == true);
        }
    }
}
