using Microsoft.Reporting.WebForms;
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
    public class CustomersController : Controller
    {
        public readonly int TableID = 2; 
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

            sp_GetCustomers_Result item = DomainObjects.FromJson<sp_GetCustomers_Result>(data);
            if (item == null)
                item = new sp_GetCustomers_Result();

            item.CompanyID = User.CompanyID;

            int totalRecords = 0;

            sp_GetCompaniesTables_Result ct = CompaniesTablesHandler.Filter(new sp_GetCompaniesTables_Result { TableID = TableID, CompanyID = User.CompanyID.Value }).FirstOrDefault();
            item.AllowedRows = (ct == null ? -1 : ct.AllowedRows);
            var items = CustomersHandler.Filter(item);
            
            totalRecords = items.Count();
            items = Models.Utilities.TableManipulation<sp_GetCustomers_Result>(items, Request.Form);

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

            return Json(new { data = CompaniesTablesHandler.IsAllowedRaws(data,TableID, User.CompanyID.Value), isSucceded = true });
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

                sp_GetCustomers_Result item = DomainObjects.FromJson<sp_GetCustomers_Result>(data);
                if (item.CompanyID == null)
                    item.CompanyID = User.CompanyID;

                if (item.GUID == 0)
                    item.Active = true;

                if (!item.IsVIP.HasValue)
                    item.IsVIP = false;

                List<string> errors = new List<string>();
                bool isValid = true;
                isValid = CustomersHandler.Validate("Name", item, ref errors);
                if(item.IsVIP.Value)
                    isValid = CustomersHandler.Validate("ID", item, ref errors);

                if (!isValid)
                {
                    Models.Utilities.ConvertErrorsPhraseToLanguage(errors);
                    return Json(new { message = DomainObjects.LinkString(errors, Environment.NewLine), isSucceded = false });
                }
                else
                {
                    CustomersHandler.Save(item);
                }

                return Json(new { message = "", isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }

        }

        [HttpPost]
        public ActionResult GetContacts(string data)
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            sp_GetCustomers_Result item = DomainObjects.FromJson<sp_GetCustomers_Result>(data);

            if (item == null)
                item = new sp_GetCustomers_Result();

            item.CompanyID = User.CompanyID;
            int totalRecords = 0;
            if (item.GUID == 0)
                return Json(new { data = "", recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
            
            var CustomersContacts = CustomersContactsHandler.Filter(new sp_GetCustomersContacts_Result { CustomerID = item.GUID, Active = true });
            totalRecords = CustomersContacts.Count();
            CustomersContacts = Models.Utilities.TableManipulation<sp_GetCustomersContacts_Result>(CustomersContacts, Request.Form);

            return Json(new { data = CustomersContacts, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
        }


        [HttpPost]
        public ActionResult SaveContact(string data)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetCustomersContacts_Result item = DomainObjects.FromJson<sp_GetCustomersContacts_Result>(data);

                if (item.GUID == 0)
                    item.Active = true;

                if (!item.IsSendWeightsByMail.HasValue)
                    item.IsSendWeightsByMail = false;

                List<string> errors = new List<string>();
                bool isValid = true;
                isValid = CustomersContactsHandler.Validate("CustomerID", item, ref errors);
                isValid = CustomersContactsHandler.Validate("FirstName", item, ref errors);
                if (item.IsSendWeightsByMail.Value)
                {
                    isValid = CustomersContactsHandler.Validate("SendingMethodID", item, ref errors);
                    isValid = CustomersContactsHandler.Validate("Email", item, ref errors);                    
                }

                if (!isValid)
                {
                    Models.Utilities.ConvertErrorsPhraseToLanguage(errors);
                    return Json(new { message = DomainObjects.LinkString(errors, Environment.NewLine), isSucceded = false });
                }
                else
                {
                    CustomersContactsHandler.Save(item);
                }

                return Json(new { message = "", isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }

        [HttpPost]
        public ActionResult MailContact(string data,Model type ,long weighingVehicleID)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetCustomersContacts_Result item = DomainObjects.FromJson<sp_GetCustomersContacts_Result>(data);

                
                if (item == null)
                    throw new Exception(((DictionaryHandler)Models.Utilities.GetDicationary()).ContactNotExist);


                sp_GetVehiclesWeighing_Result  vehicleweighing = VehiclesWeighingHandler.Filter(new sp_GetVehiclesWeighing_Result { GUID = weighingVehicleID }).FirstOrDefault();
                if (vehicleweighing == null)
                    throw new Exception(((DictionaryHandler)Models.Utilities.GetDicationary()).CertificateNotExist);

                LocalReport report = VehiclesWeighingHandler.Report(vehicleweighing);
                string id = vehicleweighing.CertificateID.Value.ToString("D6"); ;
                
                string reportPath = Utilities.SaveReport(report, vehicleweighing.CompanyID.Value);
                Utilities.ReportToPDF(report, reportPath);

                CustomersContactsHandler.Mail(item,id, reportPath);

                return Json(new { message = ((DictionaryHandler)Models.Utilities.GetDicationary()).Sent, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }

        [HttpPost]
        public ActionResult SMSContact(string data,Model type, long weighingVehicleID)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetCustomersContacts_Result item = DomainObjects.FromJson<sp_GetCustomersContacts_Result>(data);

                if (item == null)
                    throw new Exception(((DictionaryHandler)Models.Utilities.GetDicationary()).ContactNotExist);

                LocalReport report = null;
                string id = string.Empty;
                switch (type)
                {
                    case Model.VehiclesWeighing:
                        {
                            sp_GetVehiclesWeighing_Result vehicleweighing = VehiclesWeighingHandler.Filter(new sp_GetVehiclesWeighing_Result { GUID = weighingVehicleID }).FirstOrDefault();
                            if (vehicleweighing == null)
                                throw new Exception(((DictionaryHandler)Models.Utilities.GetDicationary()).CertificateNotExist);

                            string rpt = "VehiclesWeighing_en.rdlc";
                            var company = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = User.CompanyID.Value }).FirstOrDefault();
                            if(company != null && company.LanguageID.HasValue)
                            {
                                if((Language)company.LanguageID.Value == Language.HE)
                                    rpt = "VehiclesWeighing.rdlc";
                            }
                            report = VehiclesWeighingHandler.Report(vehicleweighing);
                            id = vehicleweighing.CertificateID.Value.ToString("D6");
                        }
                        break;
                }


                string reportPath = Utilities.SaveReport(report, User.CompanyID.Value);
                FileStream stream = Utilities.ReportToPDF(report, reportPath);

                CustomersContactsHandler.SMS(item, id, reportPath);

                return Json(new { message = ((DictionaryHandler)Models.Utilities.GetDicationary()).Sent, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }
    }
}
