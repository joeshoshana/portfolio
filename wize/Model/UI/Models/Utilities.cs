using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using Shkila.Model;
using System.Collections.Specialized;
using Shkila.Model.Handlers;

namespace UI.Models
{
    public class Utilities
    {
        public static object GetUser()
        {
            object employee = (object)HttpContext.Current.Session["User"];
            if (employee != null && employee.GetType() == typeof(sp_GetUsers_Result) && ((sp_GetUsers_Result)employee).GUID > 0)
                return employee;
            else
                return null;
        }

        internal static void SetUser(object employee)
        {
            Language lang = Language.EN;
            Enum.TryParse(CompaniesSystemSettingsHandler.Filter(new sp_GetCompaniesSystemSettings_Result { }).FirstOrDefault().LanguageID.ToString(), out lang);
            bool isOwner = false;
            bool isSuper = false;
            HttpContext.Current.Session["User"] = employee;
            if (employee != null)
            {
                if (((sp_GetUsers_Result)employee).LanguageID.HasValue && ((sp_GetUsers_Result)employee).LanguageID.Value > 0)
                {
                    Enum.TryParse(((sp_GetUsers_Result)employee).LanguageID.Value.ToString(), out lang);
                }
                else
                {
                    if (((sp_GetUsers_Result)employee).CompanyID.HasValue)
                    {
                        sp_GetCompanies_Result company = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = ((sp_GetUsers_Result)employee).CompanyID.Value }).FirstOrDefault();
                        if (company != null && company.LanguageID.HasValue && company.LanguageID.Value > 0)
                            Enum.TryParse(company.LanguageID.Value.ToString(), out lang);
                    }
                }

                sp_GetCompanies_Result comp = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = ((sp_GetUsers_Result)employee).CompanyID.Value }).FirstOrDefault();
                isOwner = comp.IsOwner;
                isSuper = comp.IsSuper;
            }


            HttpContext.Current.Session["IsOwner"] = isOwner;
            HttpContext.Current.Session["IsSuper"] = isSuper;
            DictionaryHandler dic = new DictionaryHandler();
            dic.LoadText(lang);
            HttpContext.Current.Session["Language"] = dic;
            if(lang == Language.HE)
                HttpContext.Current.Session["Direction"] = "rtl";
            else
                HttpContext.Current.Session["Direction"] = "ltr";
        }


        internal static dynamic GetDirection()
        {
            return HttpContext.Current.Session["Direction"];
        }

        internal static object GetDicationary()
        {
            return HttpContext.Current.Session["Language"];
        }

        internal static object IsOwner()
        {
            return HttpContext.Current.Session["IsOwner"];
        }

        internal static object IsSuper()
        {
            return HttpContext.Current.Session["IsSuper"];
        }

        internal static IEnumerable<T> TableManipulation<T>(IEnumerable<T> items, NameValueCollection manipulation)
        {
            if (manipulation.Count > 1)
            {
                var draw = manipulation.GetValues("draw").FirstOrDefault();
                var start = manipulation.GetValues("start").FirstOrDefault();
                var length = manipulation.GetValues("length").FirstOrDefault();
                var a = manipulation.GetValues("order[0][column]").FirstOrDefault();
                var sortColumns = manipulation.GetValues("columns[" + a + "][data]").FirstOrDefault();
                var sortColumnDir = manipulation.GetValues("order[0][dir]").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                items = items.OrderBy(sortColumns + " " + sortColumnDir);
                items = items.Skip(skip).Take(pageSize);
            }
            return items;
        }

        internal static void ConvertErrorsPhraseToLanguage(List<string> errors)
        {
            var dic = (DictionaryHandler)Models.Utilities.GetDicationary();
            for (int i = 0; i < errors.Count; i++)
                errors[i] = dic.GetText(errors[i]);
        }
    }
}