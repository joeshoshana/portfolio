using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class CompaniesSystemSettingsHandler
    {
        public static void Save(sp_GetCompaniesSystemSettings_Result item)
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveCompaniesSystemSetting(item.GUID, item.LanguageID, item.LogoPath, item.DataFolder,item.ImagesFolder,item.SystemLogoPath,item.CompanyID).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetCompaniesSystemSettings_Result item)
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.CompaniesSystemSettings.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                db.CompaniesSystemSettings.Remove(dt);
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetCompaniesSystemSettings_Result item, List<string> errors)
        {
            switch(field)
            {
                case "GUID":
                    break;
                case "LanguageID":
                    break;
                case "LogoPath":
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetCompaniesSystemSettings_Result> Filter(sp_GetCompaniesSystemSettings_Result filter )
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetCompaniesSystemSettings(filter.AllowedRows, filter.GUID).ToList().AsEnumerable();
                
                return data.ToList();
            }
        }

    }
}
