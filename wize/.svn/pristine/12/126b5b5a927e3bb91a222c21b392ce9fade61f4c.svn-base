using Shkila.Model;
using Shkila.Model.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shkila.Utilities;

namespace UI.Controllers
{
    public class CompaniesController : Controller
    {

        public ActionResult Index()
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.Dictionary = Models.Utilities.GetDicationary();
            ViewBag.IsOwner = Models.Utilities.IsOwner();
            return View();
        }             

        [HttpPost]
        public ActionResult Get(string data)
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            sp_GetCompanies_Result item = DomainObjects.FromJson<sp_GetCompanies_Result>(data);

            if (item == null)
                item = new sp_GetCompanies_Result();

            if (User.CompanyIsOwner)
                item.OwnerID = User.CompanyID;

            if(!User.CompanyIsSuper)
            {
                var company = CompaniesHandler.Filter(new sp_GetCompanies_Result{GUID = User.CompanyID.Value}).FirstOrDefault();
                if(company == null)
                    item.AllowedRows = 0;

                item.AllowedRows = company.CompaniesLimit;
            }

            int totalRecords = 0;
            var companies = CompaniesHandler.Filter(item);
            
            totalRecords = companies.Count();
            companies = Models.Utilities.TableManipulation<sp_GetCompanies_Result>(companies, Request.Form);

            return Json(new { data = companies, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
        }

        [HttpPost]
        public ActionResult AllowedRows(long data)
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var company = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = User.CompanyID.Value }).FirstOrDefault();
            return Json(new { data = (company == null ? 0 : company.CompaniesLimit), isSucceded = true });
        }

        [HttpPost]
        //public ActionResult Save(string data, string tables, string forms, string settings)
        public ActionResult Save(string data)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetCompanies_Result item = DomainObjects.FromJson<sp_GetCompanies_Result>(data);
                //var t = Utilities.FromJson<List<sp_GetCompaniesTables_Result>>(tables).AsEnumerable();
                //var f = Utilities.FromJson<List<sp_GetCompaniesForms_Result>>(forms).AsEnumerable();
                //var s = Utilities.FromJson<List<sp_GetCompaniesSettings_Result>>(settings).AsEnumerable();

                if (item.GUID == -1 || item.GUID == 0)
                {
                    item.GUID = 0;
                    item.Active = true;
                }
                    

                if (User.CompanyIsOwner)
                    item.OwnerID = User.CompanyID;

                List<string> errors = new List<string>();
                bool valid = CompaniesHandler.Validate("Name",item, errors);
                valid = CompaniesHandler.Validate("ID", item, errors);
                if (!valid)
                    throw new Exception(DomainObjects.LinkString(errors, ""));

                item.DataFolder = "/App_Data/" + item.OwnerID + "/" + item.GUID;
                item.ImagesFolder = "/Images/" + item.OwnerID + "/" + item.GUID;
                //item.LogoPath = CompaniesHandler.SetImageLogo(item);
                item.SystemLogoPath = CompaniesHandler.SetSystemLogo(item);

                CompaniesHandler.ModifyFoldersData(item, Server.MapPath("/"));        
                CompaniesHandler.Save(item);
                CompaniesHandler.ModifyTables(item, item.Tables);
                CompaniesHandler.ModifyForms(item, item.Forms);
                CompaniesHandler.ModifySettings(item, item.Settings);
                
                var users = UsersHandler.Filter(new sp_GetUsers_Result { CompanyID = item.GUID, Active = true });
                if(users.Count() == 0)
                {
                    string permissionName = DictionaryHandler.GetText("Manager", item.LanguageID.HasValue ? (Language)item.LanguageID.Value : Language.EN);
                    sp_GetPermissions_Result perm = new sp_GetPermissions_Result { Active = true, CompanyID = item.GUID, Name = permissionName };
                    PermissionsHandler.Save(perm);
                    foreach (sp_GetCompaniesTables_Result table in item.Tables)
                    {
                        if (table.CompanyID <= 0)
                            continue;

                        if(table.TableID.HasValue)
                        {
                            sp_GetTablesInnerPermissions_Result it = TablesInnerPermissionsHandler.Filter(new sp_GetTablesInnerPermissions_Result { TableID = table.TableID.Value }).FirstOrDefault();
                            if (it != null)
                                PermissionsTablesInnerPermissionsHandler.Save(new sp_GetPermissionsTablesInnerPermissions_Result { Read = true, Write = true, PermissionID = perm.GUID,TablesInnerPermissionID = it.GUID });
                        }
                        
                    }

                    foreach (sp_GetCompaniesForms_Result table in item.Forms)
                    {
                        if (table.CompanyID <= 0)
                            continue;

                        if (table.FormID.HasValue)
                        {
                            sp_GetFormsInnerPermissions_Result it = FormsInnerPermissionsHandler.Filter(new sp_GetFormsInnerPermissions_Result { FormID = table.FormID.Value }).FirstOrDefault();
                            if (it != null)
                                PermissionsFormsInnerPermissionsHandler.Save(new sp_GetPermissionsFormsInnerPermissions_Result { Read = true, Write = true, PermissionID = perm.GUID, FormsInnerPermissionID = it.GUID });
                        }

                    }

                    foreach (sp_GetCompaniesSettings_Result table in item.Settings)
                    {
                        if (table.CompanyID <= 0)
                            continue;

                        if (table.SettingID.HasValue)
                        {
                            sp_GetSettingsInnerPermissions_Result it = SettingsInnerPermissionsHandler.Filter(new sp_GetSettingsInnerPermissions_Result { SettingID = table.SettingID.Value }).FirstOrDefault();
                            if (it != null)
                                PermissionsSettingsInnerPermissionsHandler.Save(new sp_GetPermissionsSettingsInnerPermissions_Result { Read = true, Write = true, PermissionID = perm.GUID, SettingsInnerPermissionID = it.GUID });
                        }
                    }

                    string password = DomainObjects.GuidString(8, 1);
                    /*List<string> to = new List<string>();
                    to.Add("office@shkila.com");
                    DomainObjects.SendMail(to,null,String.Format("סיסמא לחברת: {0}",item.Name),password);*/

                    //UsersHandler.Save(new sp_GetUsers_Result { Active = true, CompanyID = item.GUID, Username = "admin", Password = DomainObjects.Sha256Hash(password), PermissionID = perm.GUID });
                    UsersHandler.Save(new sp_GetUsers_Result { Active = true, CompanyID = item.GUID, Username = "admin", Password = password, PermissionID = perm.GUID });
                }
                    

                var formsFields = FormsFieldsHandler.Filter(new sp_GetFormsFields_Result { });
                var companyFormsFields = CompaniesFormsFieldsHandler.Filter(new sp_GetCompaniesFormsFields_Result { CompanyID = item.GUID }).ToList();
                companyFormsFields = CompaniesFormsFieldsHandler.UpdateFields(companyFormsFields, formsFields, item.GUID);
                
                return Json(new { message = "", isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }

        [HttpPost]
        public ActionResult SaveFields(string data, string formsfields)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetCompanies_Result item = DomainObjects.FromJson<sp_GetCompanies_Result>(data);
                var f = DomainObjects.FromJson<List<sp_GetCompaniesFormsFields_Result>>(formsfields).AsEnumerable();

                if (item.GUID == 0)
                    item.Active = true;

                CompaniesHandler.Save(item);
                CompaniesHandler.ModifyFormsFields(item, f);

                return Json(new { message = "", isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }

        }

        [HttpPost]
        public ActionResult GetTables(string data)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetCompanies_Result item = DomainObjects.FromJson<sp_GetCompanies_Result>(data);

                if (item == null)
                    item = new sp_GetCompanies_Result();
                    //throw new Exception(((DictionaryHandler)Models.Utilities.GetDicationary()).CompanyNotExist);


                int totalRecords = 0;
                var tables = TablesHandler.Filter(new sp_GetTables_Result { });
                var companyTables = CompaniesTablesHandler.Filter(new sp_GetCompaniesTables_Result { CompanyID = item.GUID }).GroupBy(i => i.TableID).Select(j => j.First()).ToList();
                var ownerTables = CompaniesTablesHandler.Filter(new sp_GetCompaniesTables_Result { CompanyID = User.CompanyID }).GroupBy(i => i.TableID).Select(j => j.First()).ToList();
                if (!User.CompanyIsSuper && User.CompanyIsOwner)
                {
                    tables = tables.Where(i => ownerTables.Any(j => j.TableID == i.GUID));
                    companyTables = companyTables.Where(i => ownerTables.Any(j => j.TableID == i.TableID)).ToList();
                }

                foreach (sp_GetCompaniesTables_Result table in companyTables)
                {
                    table.TabelName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.TabelName);
                }
                foreach (sp_GetTables_Result table in tables)
                {
                    if (companyTables.Any(i => i.TableID == table.GUID))
                        continue;

                    
                    
                    companyTables.Add(
                        new sp_GetCompaniesTables_Result
                        {
                            CompanyID = 0,
                            TabelName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.Name),
                            TableID = table.GUID,
                            TableLink = table.Link
                        });
                }

                if (User.CompanyIsOwner)
                {
                    CompaniesTablesHandler.AddOwnerLimit(companyTables,User.CompanyID.Value);// ownerTables);   
                }

                totalRecords = companyTables.Count();
                companyTables = Models.Utilities.TableManipulation<sp_GetCompaniesTables_Result>(companyTables, Request.Form).ToList();

                return Json(new { data = companyTables, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }

        [HttpPost]
        public ActionResult GetScales(string data)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetCompanies_Result item = DomainObjects.FromJson<sp_GetCompanies_Result>(data);
                if (item == null)
                    item = new sp_GetCompanies_Result();
                if (item.GUID == 0)
                    item.GUID = -1;
                    //throw new Exception(((DictionaryHandler)Models.Utilities.GetDicationary()).CompanyNotExist);
                

                int totalRecords = 0;
                var scales = ScalesHandler.Filter(new sp_GetScales_Result { CompanyID = item.GUID});
                if (item.IsOwner)
                {
                    scales = ScalesHandler.Filter(new sp_GetScales_Result { OwnerID = item.GUID });
                }
                totalRecords = scales.Count();
                scales = Models.Utilities.TableManipulation<sp_GetScales_Result>(scales, Request.Form);

                return Json(new { data = scales, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
            
        }

        [HttpPost]
        public ActionResult GetForms(string data)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetCompanies_Result item = DomainObjects.FromJson<sp_GetCompanies_Result>(data);

                if (item == null)
                    item = new sp_GetCompanies_Result();
                    //throw new Exception(((DictionaryHandler)Models.Utilities.GetDicationary()).CompanyNotExist);


                int totalRecords = 0;
                var forms = FormsHandler.Filter(new sp_GetForms_Result { });
                var companyForms = CompaniesFormsHandler.Filter(new sp_GetCompaniesForms_Result { CompanyID = item.GUID }).GroupBy(i => i.FormID).Select(j => j.First()).ToList();
                var ownerTables = CompaniesFormsHandler.Filter(new sp_GetCompaniesForms_Result { CompanyID = User.CompanyID }).GroupBy(i => i.FormID).Select(j => j.First()).ToList();
                if (!User.CompanyIsSuper && User.CompanyIsOwner)
                {
                    forms = forms.Where(i => ownerTables.Any(j => j.FormID == i.GUID));
                    companyForms = companyForms.Where(i => ownerTables.Any(j => j.FormID == i.FormID)).ToList();
                }

                foreach (sp_GetCompaniesForms_Result table in companyForms)
                {
                    table.TabelName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.TabelName);
                }

                foreach (sp_GetForms_Result form in forms)
                {
                    if (companyForms.Any(i => i.FormID == form.GUID))
                        continue;
                    companyForms.Add(
                        new sp_GetCompaniesForms_Result
                        {
                            CompanyID = 0,
                            TabelName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(form.Name),
                            FormID = form.GUID,
                            TableLink = form.Link
                        });
                }

                if (User.CompanyIsOwner)
                {
                    CompaniesFormsHandler.AddOwnerLimit(companyForms, ownerTables);
                }

                totalRecords = companyForms.Count();
                companyForms = Models.Utilities.TableManipulation<sp_GetCompaniesForms_Result>(companyForms, Request.Form).ToList();

                return Json(new { data = companyForms, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }

        [HttpPost]
        public ActionResult GetFormsFields(string data,string form)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetCompanies_Result item = DomainObjects.FromJson<sp_GetCompanies_Result>(data);
                sp_GetCompaniesForms_Result f = DomainObjects.FromJson<sp_GetCompaniesForms_Result>(form);

                if (item == null)
                    throw new Exception(((DictionaryHandler)Models.Utilities.GetDicationary()).CompanyNotExist);

                if (f == null)
                    throw new Exception(((DictionaryHandler)Models.Utilities.GetDicationary()).CompanyNotExist);

                int totalRecords = 0;
                var formsFields = FormsFieldsHandler.Filter(new sp_GetFormsFields_Result { FormID = f.FormID});
                var companyFormsFields = CompaniesFormsFieldsHandler.Filter(new sp_GetCompaniesFormsFields_Result { CompanyID = item.GUID, FormID = f.FormID }).GroupBy(i => i.FormsFieldID).Select(j => j.First()).ToList();

                

                companyFormsFields = CompaniesFormsFieldsHandler.UpdateFields(companyFormsFields, formsFields, item.GUID);
                

                foreach (sp_GetCompaniesFormsFields_Result formField in companyFormsFields)
                {
                    formField.FormsFieldName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(formField.FormsFieldName);
                }

                totalRecords = companyFormsFields.Count();
                companyFormsFields = Models.Utilities.TableManipulation<sp_GetCompaniesFormsFields_Result>(companyFormsFields, Request.Form).ToList();

                return Json(new { data = companyFormsFields, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }

        [HttpPost]
        public ActionResult GetSettings(string data)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetCompanies_Result item = DomainObjects.FromJson<sp_GetCompanies_Result>(data);

                if (item == null)
                    item = new sp_GetCompanies_Result();
                    //throw new Exception(((DictionaryHandler)Models.Utilities.GetDicationary()).CompanyNotExist);


                int totalRecords = 0;
                var settings = SettingsHandler.Filter(new sp_GetSettings_Result { });
                var companySettings = CompaniesSettingsHandler.Filter(new sp_GetCompaniesSettings_Result { CompanyID = item.GUID }).GroupBy(i => i.SettingID).Select(j => j.First()).ToList();
                var ownerTables = CompaniesSettingsHandler.Filter(new sp_GetCompaniesSettings_Result { CompanyID = User.CompanyID }).GroupBy(i => i.SettingID ).Select(j => j.First()).ToList();
                
                if (!User.CompanyIsSuper && User.CompanyIsOwner)
                {    
                    settings = settings.Where(i => ownerTables.Any(j => j.SettingID == i.GUID));
                    companySettings = companySettings.Where(i => ownerTables.Any(j => j.SettingID == i.SettingID)).ToList();
                }

                foreach (sp_GetCompaniesSettings_Result table in companySettings)
                {
                    table.SettingName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.SettingName);
                }

                foreach (sp_GetSettings_Result setting in settings)
                {
                    if (companySettings.Any(i => i.SettingID == setting.GUID))
                        continue;
                    companySettings.Add(
                        new sp_GetCompaniesSettings_Result
                        {
                            CompanyID = 0,
                            SettingName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(setting.Name),
                            SettingID = setting.GUID,
                            SettingLink = setting.Link
                        });
                }

                if (User.CompanyIsOwner)
                {
                    CompaniesSettingsHandler.AddOwnerLimit(companySettings, ownerTables);
                }
                totalRecords = companySettings.Count();
                companySettings = Models.Utilities.TableManipulation<sp_GetCompaniesSettings_Result>(companySettings, Request.Form).ToList();

                return Json(new { data = companySettings, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }
    }
}
