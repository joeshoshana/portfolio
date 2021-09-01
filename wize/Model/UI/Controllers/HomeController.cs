using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shkila.Model;
using Shkila.Model.Handlers;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            sp_GetUsers_Result User = null;
            if((User=(sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.Dictionary = Models.Utilities.GetDicationary();
            ViewBag.IsOwner = Models.Utilities.IsOwner();
            //ViewBag.Grettings = "שלום," + User.FirstName + " " + User.LastName;
            if (User.DefaultFormID != null && User.DefaultFormID > 0)
            {
                string[] str = User.FormLink.Split('/');
                return RedirectToAction(str[2], str[1]);
            }
            return View();
        }

        public ActionResult Disconnect()
        {
            Models.Utilities.SetUser(null);
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Greeting()
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return Json(new { message = ((DictionaryHandler)Models.Utilities.GetDicationary()).Hello +  "," + User.FirstName + " " + User.LastName, isSucceded = true });
        }

        [HttpPost]
        public ActionResult Dictionary()
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return Json(new { message = Models.Utilities.GetDicationary(), isSucceded = true });
        }

        [HttpPost]
        public ActionResult Language()
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            List<Models.LanguageEntity> languages = new List<Models.LanguageEntity>();
            foreach (var name in Enum.GetNames(typeof(Language)))
            {
                languages.Add(new Models.LanguageEntity { GUID = (int)Enum.Parse(typeof(Language), name) , Name = name});
            }

            return Json(new { data = languages, isSucceded = true });
        }

        [HttpPost]
        public ActionResult Units()
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var units = UnitsHandler.Filter(new sp_GetUnits_Result { });
            
            return Json(new { data = units, isSucceded = true });
        }



        [HttpPost]
        public ActionResult CompanyTables()
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }


            if (User.DriverID.HasValue && User.DriverID.Value > 0)
                return Json(new { data = new List<sp_GetTables_Result>(), isSucceded = true });

            IEnumerable<sp_GetPermissionsTablesInnerPermissions_Result> userTables = null;
            var companyTables = CompaniesTablesHandler.Filter(new sp_GetCompaniesTables_Result { CompanyID = User.CompanyID });
            if (User.PermissionID.HasValue && User.PermissionID.Value > 0)
                userTables = PermissionsTablesInnerPermissionsHandler.Filter(new sp_GetPermissionsTablesInnerPermissions_Result { PermissionID = User.PermissionID});

            if (userTables != null)
                companyTables = companyTables.Where(i =>  i.TableID.HasValue && userTables.Any(j => j.TableID == i.TableID.Value));

            var tables = TablesHandler.Filter(new sp_GetTables_Result { Active = true }).Where(i => companyTables.Any(j => j.TableID == i.GUID));
            foreach(sp_GetTables_Result table in tables)
            {
                
                table.Name = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.Name);
                if (userTables != null)
                {
                   var ut = userTables.Where(i => i.TableID == table.GUID).FirstOrDefault();
                   if (ut != null)
                   {
                       table.Read = ut.Read.HasValue ? ut.Read.Value : false;
                       table.Write = ut.Write.HasValue ? ut.Write.Value : false;
                   }
                   else
                   {
                       table.Read = false;
                       table.Write = false;
                   }

                }
                else
                {
                    table.Read = false;
                    table.Write = false;
                }
            }

            return Json(new { data = tables, isSucceded = true });
        }

        [HttpPost]
        public ActionResult IsOwner()
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var company = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = (long)User.CompanyID }).FirstOrDefault();
            if(company != null)
                return Json(new { data = company.IsOwner, isSucceded = true });

            return Json(new { data = false, isSucceded = true });
        }

        [HttpPost]
        public ActionResult IsSuper()
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var company = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = (long)User.CompanyID }).FirstOrDefault();
            if (company != null)
                return Json(new { data = company.IsSuper, isSucceded = true });

            return Json(new { data = false, isSucceded = true });
        }

        [HttpPost]
        public ActionResult CompanyForms()
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            IEnumerable<sp_GetPermissionsFormsInnerPermissions_Result> userForms = null;
            var companyForms = CompaniesFormsHandler.Filter(new sp_GetCompaniesForms_Result { CompanyID = User.CompanyID });
            if (User.PermissionID.HasValue && User.PermissionID.Value > 0)
                userForms = PermissionsFormsInnerPermissionsHandler.Filter(new sp_GetPermissionsFormsInnerPermissions_Result { PermissionID = User.PermissionID });

            if (userForms != null)
                companyForms = companyForms.Where(i => i.FormID.HasValue && userForms.Any(j => j.FormID == i.FormID.Value));
            
            var tables = FormsHandler.Filter(new sp_GetForms_Result { Active = true }).Where(i => companyForms.Any(j => j.FormID == i.GUID));
            if (User.DriverID.HasValue && User.DriverID.Value > 0)
                if (tables.Where(i => i.GUID == 4).FirstOrDefault() != null)
                {
                    var tl = tables.ToList();
                    tl.RemoveAll(item => item.GUID != 4);
                    tables = tl;
                }
                    
                else
                    return Json(new { data = new List<sp_GetForms_Result>(), isSucceded = true });

            foreach (sp_GetForms_Result table in tables)
            {
                table.Name = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.Name);
                if (userForms != null)
                {
                    var ut = userForms.Where(i => i.FormID == table.GUID).FirstOrDefault();
                    if (ut != null)
                    {
                        table.Read = ut.Read.HasValue ? ut.Read.Value : false;
                        table.Write = ut.Write.HasValue ? ut.Write.Value : false;
                    }
                    else
                    {
                        table.Read = false;
                        table.Write = false;
                    }

                }
                else
                {
                    table.Read = false;
                    table.Write = false;
                }
            }
            return Json(new { data = tables, isSucceded = true });
        }

        [HttpPost]
        public ActionResult CompanySettings()
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (User.DriverID.HasValue && User.DriverID.Value > 0)
                return Json(new { data = new List<sp_GetSettings_Result>(), isSucceded = true });

            IEnumerable<sp_GetPermissionsSettingsInnerPermissions_Result> userSettings = null;
            var companySettings = CompaniesSettingsHandler.Filter(new sp_GetCompaniesSettings_Result { CompanyID = User.CompanyID });
            if (User.PermissionID.HasValue && User.PermissionID.Value > 0)
                userSettings = PermissionsSettingsInnerPermissionsHandler.Filter(new sp_GetPermissionsSettingsInnerPermissions_Result { PermissionID = User.PermissionID });

            if (userSettings != null)
                companySettings = companySettings.Where(i => i.SettingID.HasValue && userSettings.Any(j => j.SettingID == i.SettingID.Value));

            var tables = SettingsHandler.Filter(new sp_GetSettings_Result { Active = true }).Where(i => companySettings.Any(j => j.SettingID == i.GUID));
            foreach (sp_GetSettings_Result table in tables)
            {
                table.Name = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.Name);
                if (userSettings != null)
                {
                    var ut = userSettings.Where(i => i.SettingID == table.GUID).FirstOrDefault();
                    if (ut != null)
                    {
                        table.Read = ut.Read.HasValue ? ut.Read.Value : false;
                        table.Write = ut.Write.HasValue ? ut.Write.Value : false;
                    }
                    else
                    {
                        table.Read = false;
                        table.Write = false;
                    }
                        
                }
                else
                {
                    table.Read = false;
                    table.Write= false;
                }

                    
            }
            return Json(new { data = tables, isSucceded = true });
        }

        [HttpPost]
        public ActionResult Logo()
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            string logoPath = "/Images/logo.ico";
            var company = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = (long)User.CompanyID }).FirstOrDefault();
            if (company != null)
            {
                if (User.CompanyIsOwner && !String.IsNullOrEmpty(company.SystemLogoPath) && System.IO.File.Exists(Server.MapPath("/") + company.SystemLogoPath))
                    logoPath = company.SystemLogoPath;
                else if (!User.CompanyIsOwner && company.OwnerID.HasValue)
                {
                    company = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = company.OwnerID.Value }).FirstOrDefault();
                    if (company != null && System.IO.File.Exists(Server.MapPath("/") + company.SystemLogoPath))
                        logoPath = company.SystemLogoPath;
                }
            }
            return Json(new { data = logoPath, isSucceded = true });
        }

        [HttpPost]
        public ActionResult InvisibleFields(int FormID)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                var fields = CompaniesFormsFieldsHandler.InvisibleFields(new sp_GetCompaniesFormsFields_Result { FormID = FormID, CompanyID = User.CompanyID });
                return Json(new { data = fields, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }
    }

}
