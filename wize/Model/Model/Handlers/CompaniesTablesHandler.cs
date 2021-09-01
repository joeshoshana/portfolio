using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class CompaniesTablesHandler
    {
        public static void Save(sp_GetCompaniesTables_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveCompanyTable(item.GUID, item.CompanyID, item.TableID, item.AllowedRows).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetCompaniesTables_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.CompaniesTables.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                db.CompaniesTables.Remove(dt);
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetCompaniesTables_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "CompanyID":
                    if (!item.CompanyID.HasValue || item.CompanyID.Value == 0)
                        errors.Add("InvalidCompany");
                    break;
                case "TableID":
                    if (!item.TableID.HasValue || item.TableID.Value == 0)
                        errors.Add("InvalidTable");
                    break;
                case "AllowedRows":
                    if (item.AllowedRows < -1 )
                        errors.Add("InvalidAllowedRows");
                    break;
                    
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetCompaniesTables_Result> Filter(sp_GetCompaniesTables_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetCompaniesTables(filter.GUID, filter.CompanyID, filter.TableID).ToList().AsEnumerable();

                return data.ToList();
            }
        }


        public static IEnumerable<T> ReturnAllowedRows<T>(long id, long companyID, IEnumerable<T> list)
        {
            sp_GetCompaniesTables_Result ct = Filter(new sp_GetCompaniesTables_Result { TableID = id, CompanyID = companyID }).FirstOrDefault();
            if (ct == null || ct.AllowedRows == -1)
                return list;

            return list.Take((int)ct.AllowedRows);
        }

        public static bool IsAllowedRaws(long data, long id, long companyID)
        {
            sp_GetCompaniesTables_Result ct = Filter(new sp_GetCompaniesTables_Result { TableID = id, CompanyID = companyID }).FirstOrDefault();
            if (ct == null || ct.AllowedRows == -1)
                return true;

            return ct.AllowedRows > data;
        }

        public static void AddOwnerLimit(List<sp_GetCompaniesTables_Result> companyTables, long ownerID)//List<sp_GetCompaniesTables_Result> ownerTables)
        {

            foreach (sp_GetCompaniesTables_Result table in companyTables)
            {
                if(table.TableID.HasValue)
                    table.OwnerLimit = GetRemnantOfAllowedRows(ownerID, table.TableID.Value);
                /*var ot = ownerTables.Where(i => i.TableID == table.TableID).FirstOrDefault();
                if (ot != null)
                    table.OwnerLimit = ot.AllowedRows;
                else
                    table.OwnerLimit = -1;*/
            }
        }

        public static long GetRemnantOfAllowedRows(long ownerID, long tablID)
        {
            var owner = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = ownerID }).FirstOrDefault();
            var ot = Filter(new sp_GetCompaniesTables_Result { CompanyID = ownerID, TableID = tablID }).FirstOrDefault();
            if (owner != null)
            {
                if (!owner.IsSuper && ot == null)
                    return 0;

                if (owner.IsSuper)
                    return -1;
            }
            else
                return 0;

            if(ot.AllowedRows == -1)
                return -1;

            var totalRows = ot.AllowedRows;

            long sum = 0;
            var companies = CompaniesHandler.Filter(new sp_GetCompanies_Result { OwnerID = ownerID });
            foreach (var company in companies)
            {
                var ct  = Filter(new sp_GetCompaniesTables_Result { CompanyID = company.GUID, TableID = tablID }).FirstOrDefault();
                if (ct == null)
                    continue;

                if (ct.AllowedRows == -1)
                    return 0;

                sum += ct.AllowedRows;

            }

            return Math.Max(0, totalRows - sum);
        }
    }
}
