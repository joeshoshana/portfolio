using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class CompaniesSettingsHandler
    {
        public static void Save(sp_GetCompaniesSettings_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveCompanySetting(item.GUID, item.CompanyID, item.SettingID, item.AllowedRows).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }


        public static void Delete(sp_GetCompaniesSettings_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.CompaniesSettings.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                db.CompaniesSettings.Remove(dt);
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetCompaniesSettings_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "CompanyID":
                    if(!item.CompanyID.HasValue || item.CompanyID.Value == 0)
                        errors.Add("InvalidCompany");
                    break;
                case "SettingID":
                    if (!item.SettingID.HasValue || item.SettingID.Value == 0)
                        errors.Add("InvalidSetting");
                    break;
                case "AllowedRows":
                    if (item.AllowedRows < -1)
                        errors.Add("InvalidAllowedRows");
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetCompaniesSettings_Result> Filter(sp_GetCompaniesSettings_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetCompaniesSettings(filter.GUID, filter.CompanyID, filter.SettingID).ToList().AsEnumerable();

                return data.ToList();
            }
        }

        public static IEnumerable<T> ReturnAllowedRows<T>(long id, long companyID, IEnumerable<T> list)
        {
            sp_GetCompaniesSettings_Result ct = Filter(new sp_GetCompaniesSettings_Result { SettingID = id, CompanyID = companyID }).FirstOrDefault();
            if (ct == null || ct.AllowedRows == -1)
                return list;

            return list.Take((int)ct.AllowedRows);
        }

        public static bool IsAllowedRaws(long data, long id, long companyID)
        {
            sp_GetCompaniesSettings_Result ct = Filter(new sp_GetCompaniesSettings_Result { SettingID = id, CompanyID = companyID }).FirstOrDefault();
            if (ct == null || ct.AllowedRows == -1)
                return true;

            return ct.AllowedRows > data;
        }

        public static void AddOwnerLimit(List<sp_GetCompaniesSettings_Result> companyTables, List<sp_GetCompaniesSettings_Result> ownerTables)
        {

            foreach (sp_GetCompaniesSettings_Result table in companyTables)
            {
                var ot = ownerTables.Where(i => i.SettingID == table.SettingID).FirstOrDefault();
                if (ot != null)
                    table.OwnerLimit = ot.AllowedRows;
                else
                    table.OwnerLimit = -1;
            }
        }
    }
}
