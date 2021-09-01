using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class PermissionsHandler
    {
        public static void Save(sp_GetPermissions_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SavePermission(item.GUID, item.Name, item.Active, item.CompanyID).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetPermissions_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.Permissions.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                dt.Active = false;
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetPermissions_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "Name":
                    if (String.IsNullOrEmpty(item.Name))
                        errors.Add("MissingName");
                    break;
                case "CompanyID":
                    if(!item.CompanyID.HasValue || item.CompanyID.Value == 0)
                        errors.Add("InvalidCompany");
                    break;
                case "Active":
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetPermissions_Result> Filter(sp_GetPermissions_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetPermissions(filter.AllowedRows, filter.GUID, filter.CompanyID, filter.Name).ToList().AsEnumerable();
                
                return data.ToList();
            }
        }


        public static void ModifyTablesPermissions(sp_GetPermissions_Result item, IEnumerable<sp_GetPermissionsTablesInnerPermissions_Result> list)
        {
            var newList = list.Where(i => i.PermissionID > 0);
            var oldList = PermissionsTablesInnerPermissionsHandler.Filter(new sp_GetPermissionsTablesInnerPermissions_Result { PermissionID = item.GUID });

            var toBeAdded = newList.Where(i => !oldList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeAdded.Count(); i++)
            {
                toBeAdded.ElementAt(i).PermissionID = item.GUID;
                PermissionsTablesInnerPermissionsHandler.Save(toBeAdded.ElementAt(i));
            }
            var toBeDeleted = oldList.Where(i => !newList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeDeleted.Count(); i++)
            {
                PermissionsTablesInnerPermissionsHandler.Delete(toBeDeleted.ElementAt(i));
            }

            var toBeModified = newList.Where(i => oldList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeModified.Count(); i++)
            {
                toBeModified.ElementAt(i).PermissionID = item.GUID;
                PermissionsTablesInnerPermissionsHandler.Save(toBeModified.ElementAt(i));
            }
        }

        public static void ModifyFormsPermissions(sp_GetPermissions_Result item, IEnumerable<sp_GetPermissionsFormsInnerPermissions_Result> list)
        {
            var newList = list.Where(i => i.PermissionID > 0);
            var oldList = PermissionsFormsInnerPermissionsHandler.Filter(new sp_GetPermissionsFormsInnerPermissions_Result { PermissionID = item.GUID });

            var toBeAdded = newList.Where(i => !oldList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeAdded.Count(); i++)
            {
                toBeAdded.ElementAt(i).PermissionID = item.GUID;
                PermissionsFormsInnerPermissionsHandler.Save(toBeAdded.ElementAt(i));
            }
            var toBeDeleted = oldList.Where(i => !newList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeDeleted.Count(); i++)
            {
                PermissionsFormsInnerPermissionsHandler.Delete(toBeDeleted.ElementAt(i));
            }

            var toBeModified = newList.Where(i => oldList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeModified.Count(); i++)
            {
                toBeModified.ElementAt(i).PermissionID = item.GUID;
                PermissionsFormsInnerPermissionsHandler.Save(toBeModified.ElementAt(i));
            }
        }

        public static void ModifySettingsPermissions(sp_GetPermissions_Result item, IEnumerable<sp_GetPermissionsSettingsInnerPermissions_Result> list)
        {
            var newList = list.Where(i => i.PermissionID > 0);
            var oldList = PermissionsSettingsInnerPermissionsHandler.Filter(new sp_GetPermissionsSettingsInnerPermissions_Result { PermissionID = item.GUID });

            var toBeAdded = newList.Where(i => !oldList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeAdded.Count(); i++)
            {
                toBeAdded.ElementAt(i).PermissionID = item.GUID;
                PermissionsSettingsInnerPermissionsHandler.Save(toBeAdded.ElementAt(i));
            }
            var toBeDeleted = oldList.Where(i => !newList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeDeleted.Count(); i++)
            {
                PermissionsSettingsInnerPermissionsHandler.Delete(toBeDeleted.ElementAt(i));
            }

            var toBeModified = newList.Where(i => oldList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeModified.Count(); i++)
            {
                toBeModified.ElementAt(i).PermissionID = item.GUID;
                PermissionsSettingsInnerPermissionsHandler.Save(toBeModified.ElementAt(i));
            }
        }
    }
}
