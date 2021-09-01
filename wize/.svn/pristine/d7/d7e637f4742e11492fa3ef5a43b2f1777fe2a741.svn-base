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
    public class SuppliersController : Controller
    {
        public readonly int TableID = 3; 
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

            sp_GetSuppliers_Result item = DomainObjects.FromJson<sp_GetSuppliers_Result>(data);

            if (item == null)
                item = new sp_GetSuppliers_Result { Active = true };

            item.CompanyID = User.CompanyID;

            int totalRecords = 0;
            sp_GetCompaniesTables_Result ct = CompaniesTablesHandler.Filter(new sp_GetCompaniesTables_Result { TableID = TableID, CompanyID = User.CompanyID.Value }).FirstOrDefault();
            item.AllowedRows = (ct == null ? -1 : ct.AllowedRows);
            var items = SuppliersHandler.Filter(item);
            
            totalRecords = items.Count();
            items = Models.Utilities.TableManipulation<sp_GetSuppliers_Result>(items, Request.Form);

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

                sp_GetSuppliers_Result item = DomainObjects.FromJson<sp_GetSuppliers_Result>(data);
                item.CompanyID = User.CompanyID;

                if (item.GUID == 0)
                    item.Active = true;

                List<string> errors = new List<string>();
                bool isValid = true;
                isValid = SuppliersHandler.Validate("ID", item, ref errors);
                isValid = SuppliersHandler.Validate("Name", item, ref errors);

                if (!isValid)
                {
                    Models.Utilities.ConvertErrorsPhraseToLanguage(errors);
                    return Json(new { message = DomainObjects.LinkString(errors, Environment.NewLine), isSucceded = false });
                }
                else
                {
                    SuppliersHandler.Save(item);
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

            sp_GetSuppliers_Result item = DomainObjects.FromJson<sp_GetSuppliers_Result>(data);

            if (item == null)
                item = new sp_GetSuppliers_Result();

            item.CompanyID = User.CompanyID;

            int totalRecords = 0;
            var suppliersContacts = SuppliersContactsHandler.Filter(new sp_GetSuppliersContacts_Result {SupplierID = item.GUID, Active = true});
            totalRecords = suppliersContacts.Count();
            suppliersContacts = Models.Utilities.TableManipulation<sp_GetSuppliersContacts_Result>(suppliersContacts, Request.Form);

            return Json(new { data = suppliersContacts, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
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

                sp_GetSuppliersContacts_Result item = DomainObjects.FromJson<sp_GetSuppliersContacts_Result>(data);

                if (item.GUID == 0)
                    item.Active = true;

                List<string> errors = new List<string>();
                bool isValid = true;
                isValid = SuppliersContactsHandler.Validate("SupplierID", item, ref errors);
                isValid = SuppliersContactsHandler.Validate("FirstName", item, ref errors);

                if (!isValid)
                {
                    Models.Utilities.ConvertErrorsPhraseToLanguage(errors);
                    return Json(new { message = DomainObjects.LinkString(errors, Environment.NewLine), isSucceded = false });
                }
                else
                {
                    SuppliersContactsHandler.Save(item);
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
