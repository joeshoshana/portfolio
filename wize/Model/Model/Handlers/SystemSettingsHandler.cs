using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class SystemSettingsHandler
    {
        public static void Save(sp_GetSystemSettings_Result item)
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveSystemSetting(item.GUID,item.LanguageID,item.LogoPath).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetSystemSettings_Result item)
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.SystemSettings.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                db.SystemSettings.Remove(dt);
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetSystemSettings_Result item, List<string> errors)
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

        public static IEnumerable<sp_GetSystemSettings_Result> Filter(sp_GetSystemSettings_Result filter = null)
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetSystemSettings().ToList().AsEnumerable();
                
                if(filter != null)
                {
                    if (filter.GUID > 0)
                        data = data.Where(i => i.GUID == filter.GUID);
                }

                return data.ToList();
            }
        }

    }
}
