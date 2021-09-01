using Shkila.Model;
using Shkila.Model.Handlers;
using Shkila.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class PermissionsController : Controller
    {
        public readonly int SettingsID = 7; 
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


            sp_GetPermissions_Result item = DomainObjects.FromJson<sp_GetPermissions_Result>(data);

            if (item == null)
                item = new sp_GetPermissions_Result();
        
            item.CompanyID = User.CompanyID;

            int totalRecords = 0;
            sp_GetCompaniesSettings_Result ct = CompaniesSettingsHandler.Filter(new sp_GetCompaniesSettings_Result { SettingID = SettingsID, CompanyID = User.CompanyID.Value }).FirstOrDefault();
            item.AllowedRows = (ct == null ? -1 : ct.AllowedRows);
            var items = PermissionsHandler.Filter(item);
            
            
            totalRecords = items.Count();
            items = Models.Utilities.TableManipulation<sp_GetPermissions_Result>(items, Request.Form);

            return Json(new { data = items, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
        }

        [HttpPost]
        public ActionResult AllowedRows(long data)
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return Json(new { data = CompaniesSettingsHandler.IsAllowedRaws(data, SettingsID, User.CompanyID.Value), isSucceded = true });

        }
        [HttpPost]
        public ActionResult Save(string data, string tablesPermissions, string formsPermissions, string settingsPermissions)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetPermissions_Result item = DomainObjects.FromJson<sp_GetPermissions_Result>(data);
                var t = DomainObjects.FromJson<List<sp_GetPermissionsTablesInnerPermissions_Result>>(tablesPermissions).AsEnumerable();
                var f = DomainObjects.FromJson<List<sp_GetPermissionsFormsInnerPermissions_Result>>(formsPermissions).AsEnumerable();
                var s = DomainObjects.FromJson<List<sp_GetPermissionsSettingsInnerPermissions_Result>>(settingsPermissions).AsEnumerable();

                if(item.GUID == 0)
                    item.Active = true;
                
                item.CompanyID = User.CompanyID;

                PermissionsHandler.Save(item);
                PermissionsHandler.ModifyTablesPermissions(item, t);
                PermissionsHandler.ModifyFormsPermissions(item, f);
                PermissionsHandler.ModifySettingsPermissions(item, s);

                return Json(new { message = "", isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }

        [HttpPost]
        public ActionResult GetTablesPermissions(string data)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetPermissions_Result item = DomainObjects.FromJson<sp_GetPermissions_Result>(data);

                if (item == null)
                    item = new sp_GetPermissions_Result();
                
                item.CompanyID = User.CompanyID;

                int totalRecords = 0;
                var tables = CompaniesTablesHandler.Filter(new sp_GetCompaniesTables_Result {CompanyID = User.CompanyID });
                var tablesPermissions = TablesInnerPermissionsHandler.Filter(new sp_GetTablesInnerPermissions_Result { }).Where(i => tables.Any(j => j.TableID == i.TableID));
                var permissionTables = PermissionsTablesInnerPermissionsHandler.Filter(new sp_GetPermissionsTablesInnerPermissions_Result { PermissionID = item.GUID}).GroupBy(i => i.TableID).Select(j => j.First()).ToList();

                foreach (sp_GetPermissionsTablesInnerPermissions_Result table in permissionTables)
                {
                    table.TableName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.TableName);
                    table.TablesInnerPermissionName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.TablesInnerPermissionName);
                }

                foreach (sp_GetTablesInnerPermissions_Result table in tablesPermissions)
                {
                    if (permissionTables.Any(i => i.TablesInnerPermissionID == table.GUID))
                        continue;
             
                    permissionTables.Add(
                        new sp_GetPermissionsTablesInnerPermissions_Result
                        {
                            GUID = 0,
                            PermissionID = 0,
                            TableName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.TableName),
                            TablesInnerPermissionID = table.GUID,
                            TablesInnerPermissionName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.Name)
                        });
                }

                totalRecords = permissionTables.Count();
                permissionTables = Models.Utilities.TableManipulation<sp_GetPermissionsTablesInnerPermissions_Result>(permissionTables, Request.Form).ToList();

                permissionTables = permissionTables.OrderBy(i => i.TablesInnerPermissionName).ToList();
                return Json(new { data = permissionTables, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }

        [HttpPost]
        public ActionResult GetFormsPermissions(string data)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetPermissions_Result item = DomainObjects.FromJson<sp_GetPermissions_Result>(data);

                if (item == null)
                    item = new sp_GetPermissions_Result();

                item.CompanyID = User.CompanyID;
                int totalRecords = 0;
                var forms = CompaniesFormsHandler.Filter(new sp_GetCompaniesForms_Result { CompanyID = User.CompanyID });
                var FormsPermissions = FormsInnerPermissionsHandler.Filter(new sp_GetFormsInnerPermissions_Result { }).Where(i => forms.Any(j => j.FormID == i.FormID));
                var permissionForms = PermissionsFormsInnerPermissionsHandler.Filter(new sp_GetPermissionsFormsInnerPermissions_Result { PermissionID = item.GUID }).GroupBy(i => i.FormID).Select(j => j.First()).ToList();

                foreach (sp_GetPermissionsFormsInnerPermissions_Result table in permissionForms)
                {
                    table.FormName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.FormName);
                    table.FormsInnerPermissionName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.FormsInnerPermissionName);
                }

                foreach (sp_GetFormsInnerPermissions_Result Form in FormsPermissions)
                {
                    if (permissionForms.Any(i => i.FormsInnerPermissionID == Form.GUID))
                        continue;
                    permissionForms.Add(
                        new sp_GetPermissionsFormsInnerPermissions_Result
                        {
                            GUID = 0,
                            PermissionID = 0,
                            FormName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(Form.FormName),
                            FormsInnerPermissionID = Form.GUID,
                            FormsInnerPermissionName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(Form.Name)
                        });
                }

                totalRecords = permissionForms.Count();
                permissionForms = Models.Utilities.TableManipulation<sp_GetPermissionsFormsInnerPermissions_Result>(permissionForms, Request.Form).ToList();

                permissionForms = permissionForms.OrderBy(i => i.FormsInnerPermissionName).ToList();
                return Json(new { data = permissionForms, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }

        [HttpPost]
        public ActionResult GetSettingsPermissions(string data)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetPermissions_Result item = DomainObjects.FromJson<sp_GetPermissions_Result>(data);

                if (item == null)
                    item = new sp_GetPermissions_Result();

                item.CompanyID = User.CompanyID;
                int totalRecords = 0;
                var Settings = CompaniesSettingsHandler.Filter(new sp_GetCompaniesSettings_Result { CompanyID = User.CompanyID });
                var SettingsPermissions = SettingsInnerPermissionsHandler.Filter(new sp_GetSettingsInnerPermissions_Result { }).Where(i => Settings.Any(j => j.SettingID == i.SettingID));
                var permissionSettings = PermissionsSettingsInnerPermissionsHandler.Filter(new sp_GetPermissionsSettingsInnerPermissions_Result { PermissionID = item.GUID }).GroupBy(i => i.SettingID).Select(j => j.First()).ToList();

                foreach (sp_GetPermissionsSettingsInnerPermissions_Result table in permissionSettings)
                {
                    table.SettingName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.SettingName);
                    table.SettingsInnerPermissionName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.SettingsInnerPermissionName);
                }

                foreach (sp_GetSettingsInnerPermissions_Result Setting in SettingsPermissions)
                {
                    if (permissionSettings.Any(i => i.SettingsInnerPermissionID == Setting.GUID))
                        continue;
                    permissionSettings.Add(
                        new sp_GetPermissionsSettingsInnerPermissions_Result
                        {
                            GUID = 0,
                            PermissionID = 0,
                            SettingName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(Setting.SettingName),
                            SettingsInnerPermissionID = Setting.GUID,
                            SettingsInnerPermissionName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(Setting.Name)
                        });
                }

                totalRecords = permissionSettings.Count();
                permissionSettings = Models.Utilities.TableManipulation<sp_GetPermissionsSettingsInnerPermissions_Result>(permissionSettings, Request.Form).ToList();

                permissionSettings = permissionSettings.OrderBy(i => i.SettingsInnerPermissionName).ToList();
                return Json(new { data = permissionSettings, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }

        [HttpPost]
        public ActionResult GetInnerPermissions(string data)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetPermissions_Result item = DomainObjects.FromJson<sp_GetPermissions_Result>(data);

                if (item == null)
                    item = new sp_GetPermissions_Result();

                item.CompanyID = User.CompanyID;
                int totalRecords = 0;
                var innerPermissions = InnerPermissionsHandler.Filter(new sp_GetInnerPermissions_Result { });
                var permissionsInnerPermissions = PermissionsInnerPermissionsHandler.Filter(new sp_GetPermissionsInnerPermissions_Result { PermissionID  = User.PermissionID  }).ToList();

                foreach (sp_GetPermissionsInnerPermissions_Result table in permissionsInnerPermissions)
                    table.InnerPermissionName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.InnerPermissionName);
                

                foreach (sp_GetInnerPermissions_Result table in innerPermissions)
                {
                    if (permissionsInnerPermissions.Any(i => i.InnerPermissionID == table.GUID))
                        continue;

                    permissionsInnerPermissions.Add(
                        new sp_GetPermissionsInnerPermissions_Result
                        {
                            GUID = 0,
                            PermissionID = 0,
                            InnerPermissionName = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.Name),                            
                        });
                }

                totalRecords = permissionsInnerPermissions.Count();
                permissionsInnerPermissions = Models.Utilities.TableManipulation<sp_GetPermissionsInnerPermissions_Result>(permissionsInnerPermissions, Request.Form).ToList();

                permissionsInnerPermissions = permissionsInnerPermissions.OrderBy(i => i.InnerPermissionName).ToList();
                return Json(new { data = permissionsInnerPermissions, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }

    }
}
