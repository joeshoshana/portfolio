using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class PermissionsTablesInnerPermissionsHandler
    {
        public static void Save(sp_GetPermissionsTablesInnerPermissions_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SavePermissionsTablesInnerPermission(item.GUID, item.PermissionID, item.TablesInnerPermissionID, item.Read, item.Write).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetPermissionsTablesInnerPermissions_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.PermissionsTablesInnerPermissions.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                db.PermissionsTablesInnerPermissions.Remove(dt);
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetPermissionsTablesInnerPermissions_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "PermissionID":
                    if (!item.PermissionID.HasValue || item.PermissionID.Value == 0)
                        errors.Add("MissingPermission");
                    break;
                case "TablesInnerPermissionID":
                    if (!item.TablesInnerPermissionID.HasValue || item.TablesInnerPermissionID.Value == 0)
                        errors.Add("MissingTablePermission");
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetPermissionsTablesInnerPermissions_Result> Filter(sp_GetPermissionsTablesInnerPermissions_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetPermissionsTablesInnerPermissions(filter.GUID, filter.TablesInnerPermissionID, filter.PermissionID).ToList().AsEnumerable();
                
                return data.ToList();
            }
        }

    }
}
