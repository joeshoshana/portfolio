using Shkila.Model;
using Shkila.Model.Handlers;
using Shkila.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class VehiclesWeighingController : Controller
    {
        public readonly int FormID = 4;
        public ActionResult Index()
        {
            sp_GetUsers_Result User = null;
            //Check if user is logined
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {   //Activate Login method of Login controller (Action,Controller)
                return RedirectToAction("Login", "Login");
            }

            ViewBag.Dictionary = Models.Utilities.GetDicationary();
            ViewBag.IsOwner = Models.Utilities.IsOwner();
            return View();
        }

        public ActionResult Certificates()
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.Dictionary = Models.Utilities.GetDicationary();
            ViewBag.IsOwner = Models.Utilities.IsOwner();
            return View("Certificates");
        }

        public ActionResult Reports()
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.Dictionary = Models.Utilities.GetDicationary();
            ViewBag.IsOwner = Models.Utilities.IsOwner();
            ViewBag.CompanyID = User.CompanyID;
            return View("Reports");

        }

        [HttpPost]
        public ActionResult Yard()
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }


                var weighing = VehiclesWeighingHandler.Filter(new sp_GetVehiclesWeighing_Result { IsCancelled = false, CompanyID = User.CompanyID }).Where(i => i.OutDate == null).ToList();
                if (User.DriverID.HasValue && User.DriverID.Value > 0)
                    weighing = weighing.Where(i => i.DriverID == User.DriverID.Value).ToList();

                return Json(new { data = weighing, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
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

                sp_GetVehiclesWeighing_Result item = DomainObjects.FromJson<sp_GetVehiclesWeighing_Result>(data);
                item.CompanyID = User.CompanyID;

                if (item.GUID == 0)
                {
                    item.UserID = User.GUID;
                    item.InDate = DateTime.Now;
                }

                if (item.GUID != 0 || item.OutWeight != null)
                {
                    if (item.OutDate == null)
                    {
                        item.OutDate = DateTime.Now;
                        item.CertificateID = VehiclesWeighingHandler.NextID(item);
                    }
                }

                if (item.InDate.HasValue)
                    item.InDate = item.InDate.Value.ToLocalTime();

                if (item.OutDate.HasValue)
                    item.OutDate = item.OutDate.Value.ToLocalTime();

                if (item.VehicleID == -1 && !String.IsNullOrEmpty(item.LicenseNumber))
                {
                    sp_GetVehicles_Result dt = new sp_GetVehicles_Result { LicenseNumber = item.LicenseNumber, CompanyID = User.CompanyID, Active = true, IsAddInForm = true };
                    VehiclesHandler.Save(dt);
                    item.VehicleID = dt.GUID;
                }

                if (item.ItemID == -1 && !String.IsNullOrEmpty(item.ItemName))
                {
                    sp_GetItems_Result dt = new sp_GetItems_Result { Name = item.ItemName, CompanyID = User.CompanyID, Active = true, IsAddInForm = true };
                    ItemsHandler.Save(dt);
                    item.ItemID = dt.GUID;
                }

                if (item.CustomerID == -1 && !String.IsNullOrEmpty(item.CustomerName))
                {
                    sp_GetCustomers_Result dt = new sp_GetCustomers_Result { Name = item.CustomerName, CompanyID = User.CompanyID, Active = true, IsAddInForm = true };
                    CustomersHandler.Save(dt);
                    item.CustomerID = dt.GUID;
                }

                if (item.TransportID == -1 && !String.IsNullOrEmpty(item.TransportName))
                {
                    sp_GetTransports_Result dt = new sp_GetTransports_Result { Name = item.TransportName, CompanyID = User.CompanyID, Active = true, IsAddInForm = true };
                    TransportsHandler.Save(dt);
                    item.TransportID = dt.GUID;
                }

                if (item.DriverID == -1 && !String.IsNullOrEmpty(item.DriverName))
                {
                    if (User.DriverID.HasValue && User.DriverID.Value > 0)
                        throw new Exception(((DictionaryHandler)Models.Utilities.GetDicationary()).SelectDriver);

                    sp_GetDrivers_Result dt = new sp_GetDrivers_Result { Name = item.DriverName, CompanyID = User.CompanyID, Active = true, IsAddInForm = true };
                    DriversHandler.Save(dt);
                    item.DriverID = dt.GUID;
                }

                if (item.InSiteID == -1 && !String.IsNullOrEmpty(item.InSiteName))
                {
                    sp_GetSites_Result dt = new sp_GetSites_Result { Name = item.InSiteName, CompanyID = User.CompanyID, Active = true, IsAddInForm = true };
                    SitesHandler.Save(dt);
                    item.InSiteID = dt.GUID;
                }

                if (item.OutSiteID == -1 && !String.IsNullOrEmpty(item.OutSiteName))
                {
                    sp_GetSites_Result dt = new sp_GetSites_Result { Name = item.OutSiteName, CompanyID = User.CompanyID, Active = true, IsAddInForm = true };
                    SitesHandler.Save(dt);
                    item.OutSiteID = dt.GUID;
                }

                List<string> errors = new List<string>();
                bool isValid = true;
                var fields = CompaniesFormsFieldsHandler.RequiredFields(new sp_GetCompaniesFormsFields_Result { FormID = 4, CompanyID = User.CompanyID });

                isValid = VehiclesWeighingHandler.Validate("ScaleID", item, ref errors);
                isValid = VehiclesWeighingHandler.Validate("OutWeight", item, ref errors);
                isValid = VehiclesWeighingHandler.Validate("InWeight", item, ref errors);
                isValid = VehiclesWeighingHandler.Validate("UserID", item, ref errors);

                if (fields.Any(i => i.FormsFieldName.Equals("DriverID")))
                    isValid = VehiclesWeighingHandler.Validate("DriverID", item, ref errors);
                if (fields.Any(i => i.FormsFieldName.Equals("LicenseNumber")))
                    isValid = VehiclesWeighingHandler.Validate("VehicleID", item, ref errors);
                if (fields.Any(i => i.FormsFieldName.Equals("ItemName")))
                    isValid = VehiclesWeighingHandler.Validate("ItemID", item, ref errors);
                if (fields.Any(i => i.FormsFieldName.Equals("TransportName")))
                    isValid = VehiclesWeighingHandler.Validate("TransportID", item, ref errors);
                if (fields.Any(i => i.FormsFieldName.Equals("CustomerName")))
                    isValid = VehiclesWeighingHandler.Validate("CustomerID", item, ref errors);
                if (fields.Any(i => i.FormsFieldName.Equals("DestinationSite")))
                    isValid = VehiclesWeighingHandler.Validate("OutSiteID", item, ref errors);
                if (fields.Any(i => i.FormsFieldName.Equals("SourceSite")))
                    isValid = VehiclesWeighingHandler.Validate("InSiteID", item, ref errors);
                if (fields.Any(i => i.FormsFieldName.Equals("Remarks")))
                    isValid = VehiclesWeighingHandler.Validate("Remarks", item, ref errors);


                if (!isValid)
                {
                    Models.Utilities.ConvertErrorsPhraseToLanguage(errors);

                    return Json(new { message = DomainObjects.LinkString(errors, Environment.NewLine), isSucceded = false });
                }
                else
                {
                    VehiclesWeighingHandler.Save(item);
                    VehiclesWeighingHandler.MailContacts(item);
                }


                return Json(new { message = item.GUID.ToString(), isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }


        [HttpPost]
        public ActionResult Report(string filter, int rptNum)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetVehiclesWeighing_Result item = DomainObjects.FromJson<sp_GetVehiclesWeighing_Result>(filter);
                item.CompanyID = User.CompanyID;

                List<sp_GetVehiclesWeighing_Result> res = VehiclesWeighingHandler.Filter(item).ToList();
                string data = DomainObjects.ToJson(res);
                string[] columnsArr = { "InDate", "InWeight", "OutDate", "OutWeight", "CompanyName", "Netto", "ItemName", "LicenseNumber", "Remarks", "InSiteName", "OutSiteName", "IsManual", "CustomerName", "TransportName", "DriverName", "Reference" };
                string columns = DomainObjects.ToJson(columnsArr);

                return Json(new { data = data, columns = columns, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }
        /// <summary>
        /// New Report for Shomron municipal
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EvacuationReport()
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                List<sp_GetVehiclesWeighing_Result> items = new List<sp_GetVehiclesWeighing_Result>();
                var weighing = VehiclesWeighingHandler.Filter(new sp_GetVehiclesWeighing_Result { IsCancelled = false, CompanyID = User.CompanyID }).ToList();

                return Json(new { data = weighing }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { message = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult Preview(long guid)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetVehiclesWeighing_Result item = VehiclesWeighingHandler.Filter(new sp_GetVehiclesWeighing_Result { GUID = guid }).FirstOrDefault();
                item.CompanyID = User.CompanyID;

                var report = VehiclesWeighingHandler.Report(item);
                string reportPath = Utilities.SaveReport(report, User.CompanyID.Value);
                FileStream stream = Utilities.ReportToPDF(report, reportPath);


                return File(reportPath, "application/pdf");
            }
            catch (Exception ex)
            {
                string log = Path.Combine(Server.MapPath("~/App_Data"), "log.txt");
                Logger.Log(ex, log);

                if (ex.InnerException != null)
                    return Json(new { message = ex.InnerException.Message, isSucceded = false });
                else
                    return Json(new { message = ex.Message, isSucceded = false });
            }

        }

        [HttpPost]
        public ActionResult CheckForConnections(string data)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetConnections_Result item = DomainObjects.FromJson<sp_GetConnections_Result>(data);
                item.CompanyID = User.CompanyID;


                var items = ConnectionsHandler.Filter(item);

                return Json(new { data = items, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }

        [HttpPost]
        public ActionResult Get(string data)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetVehiclesWeighing_Result item = DomainObjects.FromJson<sp_GetVehiclesWeighing_Result>(data);

                if (item == null)
                    throw new Exception(((DictionaryHandler)Models.Utilities.GetDicationary()).WeightNotFound);

                item.CompanyID = User.CompanyID;

                int totalRecords = 0;
                var items = VehiclesWeighingHandler.Filter(item);
                totalRecords = items.Count();
                items = Models.Utilities.TableManipulation<sp_GetVehiclesWeighing_Result>(items, Request.Form);

                return Json(new { data = items, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }

        [HttpPost]
        public ActionResult GetCertificates(string data)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetVehiclesWeighing_Result item = DomainObjects.FromJson<sp_GetVehiclesWeighing_Result>(data);

                if (item == null)
                    throw new Exception(((DictionaryHandler)Models.Utilities.GetDicationary()).WeightNotFound);

                item.CompanyID = User.CompanyID;

                if (User.DriverID.HasValue && User.DriverID.Value > 0)
                    item.DriverID = User.DriverID.Value;

                int totalRecords = 0;
                var items = VehiclesWeighingHandler.Filter(item).Where(i => i.OutDate != null && i.CertificateID != null);

                if (item.WeighingMode != 0)
                    items = items.Where(i => i.WeighingMode == item.WeighingMode);


                items = CompaniesFormsHandler.ReturnAllowedRows<sp_GetVehiclesWeighing_Result>(FormID, User.CompanyID.Value, items);
                totalRecords = items.Count();
                items = Models.Utilities.TableManipulation<sp_GetVehiclesWeighing_Result>(items, Request.Form);
                return Json(new { data = items, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }

        [HttpPost]
        public ActionResult AllowedRows(long data)
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return Json(new { data = CompaniesFormsHandler.IsAllowedRaws(data, FormID, User.CompanyID.Value), isSucceded = true });
        }
    }
}
