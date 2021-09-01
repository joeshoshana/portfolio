using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class PermissionsInnerPermissionsHandler
    {
        public static void Save(sp_GetPermissionsInnerPermissions_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SavePermissionsInnerPermission(item.GUID, item.PermissionID, item.InnerPermissionID).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetPermissionsInnerPermissions_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.PermissionsInnerPermissions.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                db.PermissionsInnerPermissions.Remove(dt);
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetPermissionsInnerPermissions_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "PermissionID":
                    if (!item.PermissionID.HasValue || item.PermissionID.Value == 0)
                        errors.Add("MissingPermission");
                    break;
                case "InnerPermissionID":
                    if (!item.InnerPermissionID.HasValue || item.InnerPermissionID.Value == 0)
                        errors.Add("MissingInnerPermission");
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetPermissionsInnerPermissions_Result> Filter(sp_GetPermissionsInnerPermissions_Result filter)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetPermissionsInnerPermissions(filter.GUID, filter.InnerPermissionID, filter.PermissionID).ToList().AsEnumerable();
                
                return data.ToList();
            }
        }

    }
}
