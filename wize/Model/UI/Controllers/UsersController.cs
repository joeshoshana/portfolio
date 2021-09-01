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
    public class UsersController : Controller
    {
        public readonly int SettingsID = 1; 
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


            sp_GetUsers_Result item = DomainObjects.FromJson<sp_GetUsers_Result>(data);

            if (item == null)
                item = new sp_GetUsers_Result { Active = true };

            item.CompanyID = User.CompanyID;

            int totalRecords = 0;
            sp_GetCompaniesSettings_Result ct = CompaniesSettingsHandler.Filter(new sp_GetCompaniesSettings_Result { SettingID = SettingsID, CompanyID = User.CompanyID.Value }).FirstOrDefault();
            item.AllowedRows = (ct == null ? -1 : ct.AllowedRows);
            var items = UsersHandler.Filter(item);
            
            totalRecords = items.Count();
            items = Models.Utilities.TableManipulation<sp_GetUsers_Result>(items, Request.Form);

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
        public ActionResult Save(string data)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetUsers_Result item = DomainObjects.FromJson<sp_GetUsers_Result>(data);
                if (item.CompanyID == null)
                    item.CompanyID = User.CompanyID;

                if(item.GUID == 0)
                    item.Active = true;

                UsersHandler.Save(item);

                return Json(new { message = "", isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }

        }

        [HttpPost]
        public ActionResult DefaultScale()
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                return Json(new { scale = UsersHandler.DefaultScale(User), isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }

        }

        

    }
}
