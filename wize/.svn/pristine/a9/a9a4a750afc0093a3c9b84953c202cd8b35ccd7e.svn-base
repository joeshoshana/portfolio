using Shkila.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class CompaniesHandler
    {
        public static void Save(sp_GetCompanies_Result item)
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveCompany(item.GUID, item.Name, item.Address, item.ID, item.Active, item.IsOwner, item.LogoPath, item.CertificateTitle, item.LanguageID, item.IsSuper, item.OwnerID, item.DataFolder, item.ImagesFolder, item.SystemLogoPath, item.CompaniesLimit, item.Hour, item.Minute).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetCompanies_Result item)
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.Companies.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                dt.Active = false;
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetCompanies_Result item, List<string> errors)
        {
            switch(field)
            {
                case "GUID":
                    break;
                case "Name":
                    if(String.IsNullOrEmpty(item.Name))
                        errors.Add("MissingName");
                    break;
                case "Address":
                    if (String.IsNullOrEmpty(item.Address))
                        errors.Add("MissingAddress");
                    break;
                case "ID":
                    if (!String.IsNullOrEmpty(item.ID))
                    {
                        if (!DomainObjects.IsIDValid(item.ID))
                            errors.Add("InvalidID");
                    }
                    else
                        errors.Add("MissingID");
                    break;
                case "Active":
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetCompanies_Result> Filter(sp_GetCompanies_Result filter)
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetCompanies(filter.AllowedRows, filter.GUID, filter.Name, filter.OwnerID,filter.ID,filter.Address,filter.IsOwner,filter.Active).ToList().AsEnumerable();

                return data.ToList();
            }
        }

        public static void ModifyForms(sp_GetCompanies_Result item, IEnumerable<sp_GetCompaniesForms_Result> list)
        {
            var newList = list.Where(i => i.CompanyID > 0).ToList();
            var oldList = CompaniesFormsHandler.Filter(new sp_GetCompaniesForms_Result { CompanyID = item.GUID });

            var toBeAdded = newList.Where( i => !oldList.Any(j=>j.GUID == i.GUID));
            for(int i = 0; i<toBeAdded.Count();i++ )
            {
                toBeAdded.ElementAt(i).CompanyID = item.GUID;
                CompaniesFormsHandler.Save(toBeAdded.ElementAt(i));
            }

            var toBeDeleted = oldList.Where(i => !newList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeDeleted.Count(); i++)
            {
                CompaniesFormsHandler.Delete(toBeDeleted.ElementAt(i));
            }

            var toBeModified = newList.Where(i => oldList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeModified.Count(); i++)
            {
                CompaniesFormsHandler.Save(toBeModified.ElementAt(i));
            }

            if (item.IsOwner)
            {
                var ownerCompanies = Filter(new sp_GetCompanies_Result { OwnerID = item.GUID });
                foreach (sp_GetCompanies_Result c in ownerCompanies)
                {
                    if (c.GUID == item.GUID)
                        continue;

                    var ct = CompaniesFormsHandler.Filter(new sp_GetCompaniesForms_Result { CompanyID = c.GUID });
                    var l = CompaniesFormsHandler.Filter(new sp_GetCompaniesForms_Result { CompanyID = item.GUID });
                    ct = ct.Where(i => l.Any(j => j.FormID == i.FormID));
                    ModifyForms(c, ct);
                }
            }
        }

        public static void ModifyTables(sp_GetCompanies_Result item, IEnumerable<sp_GetCompaniesTables_Result> list)
        {
            var newList = list.Where(i => i.CompanyID > 0).ToList();
            var oldList = CompaniesTablesHandler.Filter(new sp_GetCompaniesTables_Result { CompanyID = item.GUID });

            var toBeAdded = newList.Where(i => !oldList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeAdded.Count(); i++)
            {
                toBeAdded.ElementAt(i).GUID = 0;
                toBeAdded.ElementAt(i).CompanyID = item.GUID;
                CompaniesTablesHandler.Save(toBeAdded.ElementAt(i));
            }
            var toBeDeleted = oldList.Where(i => !newList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeDeleted.Count(); i++)
            {
                CompaniesTablesHandler.Delete(toBeDeleted.ElementAt(i));
            }

            var toBeModified = newList.Where(i => oldList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeModified.Count(); i++)
            {
                CompaniesTablesHandler.Save(toBeModified.ElementAt(i));
            }

            if(item.IsOwner)
            {
                var ownerCompanies = Filter(new sp_GetCompanies_Result { OwnerID = item.GUID });
                foreach (sp_GetCompanies_Result c in ownerCompanies)
                {
                    if (c.GUID == item.GUID)
                        continue;

                    var ct = CompaniesTablesHandler.Filter(new sp_GetCompaniesTables_Result { CompanyID = c.GUID });
                    var l = CompaniesTablesHandler.Filter(new sp_GetCompaniesTables_Result { CompanyID = item.GUID });
                    ct = ct.Where(i => l.Any(j => j.TableID == i.TableID));
                    ModifyTables(c, ct);
                }
            }
        }

        public static void ModifyFormsFields(sp_GetCompanies_Result item, IEnumerable<sp_GetCompaniesFormsFields_Result> list)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                list.ElementAt(i).CompanyID = item.GUID;
                CompaniesFormsFieldsHandler.Save(list.ElementAt(i));
            }
        }

        public static void ModifySettings(sp_GetCompanies_Result item, IEnumerable<sp_GetCompaniesSettings_Result> list)
        {
            var newList = list.Where(i => i.CompanyID > 0).ToList();
            var oldList = CompaniesSettingsHandler.Filter(new sp_GetCompaniesSettings_Result { CompanyID = item.GUID });

            var toBeAdded = newList.Where(i => !oldList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeAdded.Count(); i++)
            {
                toBeAdded.ElementAt(i).CompanyID = item.GUID;
                CompaniesSettingsHandler.Save(toBeAdded.ElementAt(i));
            }
            var toBeDeleted = oldList.Where(i => !newList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeDeleted.Count(); i++)
            {
                CompaniesSettingsHandler.Delete(toBeDeleted.ElementAt(i));
            }


            var toBeModified = newList.Where(i => oldList.Any(j => j.GUID == i.GUID));
            for (int i = 0; i < toBeModified.Count(); i++)
            {
                CompaniesSettingsHandler.Save(toBeModified.ElementAt(i));
            }

            if (item.IsOwner)
            {
                var ownerCompanies = Filter(new sp_GetCompanies_Result { OwnerID = item.GUID });
                foreach (sp_GetCompanies_Result c in ownerCompanies)
                {
                    if (c.GUID == item.GUID)
                        continue;

                    var ct = CompaniesSettingsHandler.Filter(new sp_GetCompaniesSettings_Result { CompanyID = c.GUID });
                    var l = CompaniesSettingsHandler.Filter(new sp_GetCompaniesSettings_Result { CompanyID = item.GUID });
                    ct = ct.Where(i => l.Any(j => j.SettingID == i.SettingID));
                    ModifySettings(c, ct);
                }
            }
        }

        public static void ModifyFoldersData(sp_GetCompanies_Result item, string root)
        {
            string companyFolder = root + item.DataFolder;
            if (!System.IO.Directory.Exists(companyFolder))
                System.IO.Directory.CreateDirectory(companyFolder);
            string silosFolder = companyFolder + "/Silos";
            if (!System.IO.Directory.Exists(silosFolder))
                System.IO.Directory.CreateDirectory(silosFolder);
            companyFolder = root + item.ImagesFolder;
            if (!System.IO.Directory.Exists(companyFolder))
                System.IO.Directory.CreateDirectory(companyFolder);
        }

        public static string SetSystemLogo(sp_GetCompanies_Result item)
        {
            if (String.IsNullOrEmpty(item.SystemLogoPath))
            {
                string defaultlogopath = "/Images/logo.ico";
                if (item.OwnerID.HasValue)
                {
                    var owner = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = item.OwnerID.Value }).FirstOrDefault();
                    if (owner != null && !String.IsNullOrEmpty(owner.SystemLogoPath))
                    {
                        defaultlogopath = owner.SystemLogoPath;
                    }
                }
                return defaultlogopath;
            }
            else
                return item.SystemLogoPath;
        }

        public static string SetImageLogo(sp_GetCompanies_Result item)
        {
            if (String.IsNullOrEmpty(item.LogoPath))
            {
                string defaultlogopath = "/Images/" + item.OwnerID + "/" + item.GUID + "/logo.png";
                return defaultlogopath;
            }
            else
                return item.SystemLogoPath;
        }
    }
}
