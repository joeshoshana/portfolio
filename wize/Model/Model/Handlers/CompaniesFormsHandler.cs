using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class CompaniesFormsHandler
    {
        public static void Save(sp_GetCompaniesForms_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveCompanyForm(item.GUID, item.CompanyID, item.FormID, item.AllowedRows).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetCompaniesForms_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.CompaniesForms.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                db.CompaniesForms.Remove(dt);
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetCompaniesForms_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "CompanyID":
                    if(!item.CompanyID.HasValue || item.CompanyID.Value == 0)
                        errors.Add("InvalidCompany");
                    break;
                case "FormID":
                    if (!item.FormID.HasValue || item.FormID.Value == 0)
                        errors.Add("InvalidForm");
                    break;
                case "AllowedRows":
                    if (item.AllowedRows < -1)
                        errors.Add("InvalidAllowedRows");
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetCompaniesForms_Result> Filter(sp_GetCompaniesForms_Result filter)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetCompaniesForms(filter.GUID,filter.CompanyID, filter.FormID).ToList().AsEnumerable();

                return data.ToList();
            }
        }


        public static IEnumerable<T> ReturnAllowedRows<T>(long id ,long companyID, IEnumerable<T> list)
        {
            sp_GetCompaniesForms_Result ct = Filter(new sp_GetCompaniesForms_Result { FormID = id, CompanyID = companyID }).FirstOrDefault();
            if (ct == null || ct.AllowedRows == -1)
                return list;

            return list.Take((int)ct.AllowedRows);
        }

        public static bool IsAllowedRaws(long data, long id, long companyID)
        {
            sp_GetCompaniesForms_Result ct = Filter(new sp_GetCompaniesForms_Result { FormID = id, CompanyID = companyID }).FirstOrDefault();
            if (ct == null || ct.AllowedRows == -1)
                return true;

            return ct.AllowedRows > data;
        }

        public static void AddOwnerLimit(List<sp_GetCompaniesForms_Result> companyTables, List<sp_GetCompaniesForms_Result> ownerTables)
        {

            foreach (sp_GetCompaniesForms_Result table in companyTables)
            {
                var ot = ownerTables.Where(i => i.FormID == table.FormID).FirstOrDefault();
                if (ot != null)
                    table.OwnerLimit = ot.AllowedRows;
                else
                    table.OwnerLimit = -1;
            }
        }
    }
}
