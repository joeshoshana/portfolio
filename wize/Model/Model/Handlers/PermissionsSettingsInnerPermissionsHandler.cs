using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class PermissionsSettingsInnerPermissionsHandler
    {
        public static void Save(sp_GetPermissionsSettingsInnerPermissions_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SavePermissionsSettingsInnerPermission(item.GUID, item.PermissionID, item.SettingsInnerPermissionID, item.Read, item.Write).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetPermissionsSettingsInnerPermissions_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.PermissionsSettingsInnerPermissions.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                db.PermissionsSettingsInnerPermissions.Remove(dt);
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetPermissionsSettingsInnerPermissions_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "PermissionID":
                    if (!item.PermissionID.HasValue || item.PermissionID.Value == 0)
                        errors.Add("MissingPermission");
                    break;
                case "SettingsInnerPermissionID":
                    if (!item.SettingsInnerPermissionID.HasValue || item.SettingsInnerPermissionID.Value == 0)
                        errors.Add("MissingSettingPermission");
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetPermissionsSettingsInnerPermissions_Result> Filter(sp_GetPermissionsSettingsInnerPermissions_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetPermissionsSettingsInnerPermissions(filter.GUID, filter.SettingsInnerPermissionID, filter.PermissionID).ToList().AsEnumerable();
                
                return data.ToList();
            }
        }

    }
}
