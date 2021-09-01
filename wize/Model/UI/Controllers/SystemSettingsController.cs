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
    public class SystemSettingsController : Controller
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
                        if(User.CompanyIsOwner)
                        {
                            var company = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = (long)User.CompanyID }).FirstOrDefault();
                            if(company != null)
                            {
                                var root = Server.MapPath("/");
                                string ext = System.IO.Path.GetExtension(file.FileName);
                                
                                if (String.IsNullOrEmpty(company.ImagesFolder))
                                    logoPath =  "/Images/" + company.OwnerID + "/" + User.CompanyID +"/logo" + ext;
                                else
                                    logoPath = company.ImagesFolder + "/logo" + ext;
                                file.SaveAs(root + logoPath);
                                company.SystemLogoPath = logoPath;
                                CompaniesHandler.Save(company);
                            }
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
                if(User.CompanyIsOwner)
                {
                    var company = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = (long)User.CompanyID }).FirstOrDefault();
                    if (company != null)
                    {
                        return Json(new { data = company, isSucceded = true });
                    }
                }

                return Json(new { message = "בעיית הרשאות אנא פנה לספק", isSucceded = false});
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
