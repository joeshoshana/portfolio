using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class PermissionsFormsInnerPermissionsHandler
    {
        public static void Save(sp_GetPermissionsFormsInnerPermissions_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SavePermissionsFormsInnerPermission(item.GUID, item.PermissionID, item.FormsInnerPermissionID, item.Read, item.Write).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetPermissionsFormsInnerPermissions_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.PermissionsFormsInnerPermissions.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                db.PermissionsFormsInnerPermissions.Remove(dt);
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetPermissionsFormsInnerPermissions_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "PermissionID":
                    if (!item.PermissionID.HasValue || item.PermissionID.Value == 0)
                        errors.Add("MissingPermission");
                    break;
                case "FormsInnerPermissionID":
                    if (!item.FormsInnerPermissionID.HasValue || item.FormsInnerPermissionID.Value == 0)
                        errors.Add("MissingFormPermission");
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetPermissionsFormsInnerPermissions_Result> Filter(sp_GetPermissionsFormsInnerPermissions_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetPermissionsFormsInnerPermissions(filter.GUID, filter.FormsInnerPermissionID, filter.PermissionID).ToList().AsEnumerable();
                
                return data.ToList();
            }
        }

    }
}
