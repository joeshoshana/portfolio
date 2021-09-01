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
    public class SitesController : Controller
    {
        public readonly int TableID = 9; 
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

            sp_GetSites_Result item = DomainObjects.FromJson<sp_GetSites_Result>(data);
            if (item == null)
                item = new sp_GetSites_Result();

            item.CompanyID = User.CompanyID;

            int totalRecords = 0;

            var items = SitesHandler.Filter(item);
            items = CompaniesTablesHandler.ReturnAllowedRows<sp_GetSites_Result>(TableID, User.CompanyID.Value, items);
            
            totalRecords = items.Count();
            items = Models.Utilities.TableManipulation<sp_GetSites_Result>(items, Request.Form);




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

            return Json(new { data = CompaniesTablesHandler.IsAllowedRaws(data, TableID, User.CompanyID.Value), isSucceded = true });
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

                sp_GetSites_Result item = DomainObjects.FromJson<sp_GetSites_Result>(data);
                if (item.CompanyID == null)
                    item.CompanyID = User.CompanyID;

                if (item.GUID == 0)
                    item.Active = true;

                SitesHandler.Save(item);

                return Json(new { message = "", isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }

        }

    }
}
