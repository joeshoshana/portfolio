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
    public class DriversController : Controller
    {
        public readonly int TableID = 15; 
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


            sp_GetDrivers_Result item = DomainObjects.FromJson<sp_GetDrivers_Result>(data);

            if (item == null)
                item = new sp_GetDrivers_Result { Active = true };

            item.CompanyID = User.CompanyID;

            int totalRecords = 0;
            sp_GetCompaniesTables_Result ct = CompaniesTablesHandler.Filter(new sp_GetCompaniesTables_Result { TableID = TableID, CompanyID = User.CompanyID.Value }).FirstOrDefault();
            item.AllowedRows = (ct == null ? -1 : ct.AllowedRows);
            if (User.DriverID.HasValue && User.DriverID.Value > 0)
                item.GUID = User.DriverID.Value;
            var items = DriversHandler.Filter(item);
            
            totalRecords = items.Count();
            items = Models.Utilities.TableManipulation<sp_GetDrivers_Result>(items, Request.Form);

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

                sp_GetDrivers_Result item = DomainObjects.FromJson<sp_GetDrivers_Result>(data);
                item.CompanyID = User.CompanyID;

                if (item.GUID == 0)
                    item.Active = true;

                List<string> errors = new List<string>();
                bool isValid = true;
                isValid = DriversHandler.Validate("ID", item, ref errors);
                isValid = DriversHandler.Validate("Name", item, ref errors);

                if (!isValid)
                {
                    Models.Utilities.ConvertErrorsPhraseToLanguage(errors);
                    return Json(new { message = DomainObjects.LinkString(errors, Environment.NewLine), isSucceded = false });
                }
                else
                {
                    DriversHandler.Save(item);
                }

                return Json(new { message = "", isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }
    }
}
