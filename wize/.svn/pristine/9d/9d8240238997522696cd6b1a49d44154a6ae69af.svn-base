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
    public class CompaniesSettingsController : Controller
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

        public ActionResult Logo()
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                string logoPath = String.Empty;
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        var company = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = (long)User.CompanyID }).FirstOrDefault();
                        if (company != null)
                        {
                            var fileName = System.IO.Path.GetFileName(file.FileName);
                            string companyFolder = Server.MapPath("~" + company.ImagesFolder);
                            logoPath = System.IO.Path.Combine(companyFolder, fileName);
                            file.SaveAs(logoPath);


                            company.LogoPath = company.ImagesFolder + "/" + fileName;// logoPath;
                            CompaniesHandler.Save(company);
                            logoPath = company.ImagesFolder + "/" + fileName;
                        }
                        
                    }
                }

                return Json(new { Message = logoPath });
            }
            catch(Exception ex)
            {
                return Json(new { Message = ex.Message });
            }   

        }

        

        [HttpPost]
        public ActionResult Settings()
        {
            try
            {
                
               

                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                var company = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = (long)User.CompanyID }).FirstOrDefault();
                if (company != null && !String.IsNullOrEmpty(company.LogoPath))
                {
                    var fileName = System.IO.Path.GetFileName(company.LogoPath);
                    company.LogoPath = company.ImagesFolder + "/" + fileName; 
                }

                

                return Json(new { data = company, isSucceded = true });
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

                sp_GetCompanies_Result item = DomainObjects.FromJson<sp_GetCompanies_Result>(data);
                if (item.GUID == 0)
                    item.GUID = User.CompanyID.Value;

                if (item.Hour == -1)
                    item.Hour = null;
                if (item.Minute == -1)
                    item.Minute = null;
                item.Active = true;
                CompaniesHandler.Save(item);

                return Json(new { message = "", isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }

       
    }
}
