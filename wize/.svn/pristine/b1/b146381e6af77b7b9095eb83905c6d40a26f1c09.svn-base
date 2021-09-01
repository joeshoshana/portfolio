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
    public class VehiclesController : Controller
    {
        public readonly int TableID = 8; 
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

            sp_GetVehicles_Result item = DomainObjects.FromJson<sp_GetVehicles_Result>(data);
            if (item == null)
                item = new sp_GetVehicles_Result();

            item.CompanyID = User.CompanyID;

            int totalRecords = 0;
            sp_GetCompaniesTables_Result ct = CompaniesTablesHandler.Filter(new sp_GetCompaniesTables_Result { TableID = TableID, CompanyID = User.CompanyID.Value }).FirstOrDefault();
            item.AllowedRows = (ct == null ? -1 : ct.AllowedRows);

            var items = VehiclesHandler.Filter(item);

            if (User.DriverID.HasValue && User.DriverID.Value > 0)
            {
                var connections = ConnectionsHandler.Filter(new sp_GetConnections_Result { DriverID = User.DriverID.Value });
                items = items.Where(i => connections.Any(j => j.VehicleID == i.GUID));
            }

            totalRecords = items.Count();
            items = Models.Utilities.TableManipulation<sp_GetVehicles_Result>(items, Request.Form);

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

                sp_GetVehicles_Result item = DomainObjects.FromJson<sp_GetVehicles_Result>(data);
                if (item.CompanyID == null)
                    item.CompanyID = User.CompanyID;

                if (item.GUID == 0)
                    item.Active = true;

                VehiclesHandler.Save(item);

                return Json(new { message = "", isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }

        }

        [HttpPost]
        public ActionResult UpdateTare(string data)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetVehicles_Result item = DomainObjects.FromJson<sp_GetVehicles_Result>(data);
                if (item.CompanyID == null)
                    item.CompanyID = User.CompanyID;

                sp_GetVehicles_Result dt = VehiclesHandler.Filter(item).FirstOrDefault();
                if (dt == null)
                    throw new Exception(((DictionaryHandler)Models.Utilities.GetDicationary()).VehicleNotExist);

                dt.Tare = item.Tare;

                VehiclesHandler.Save(dt);

                return Json(new { message = "", isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }

        }

        

    }
}
