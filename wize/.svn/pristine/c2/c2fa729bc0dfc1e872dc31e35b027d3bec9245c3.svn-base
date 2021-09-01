using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shkila.Model;
using Shkila.Model.Handlers;
using Shkila.Utilities;

namespace UI.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Login()
        {
            Models.Utilities.SetUser(null);
            ViewBag.Dictionary = Models.Utilities.GetDicationary();
            ViewBag.Direction = Models.Utilities.GetDirection();
            ViewBag.IsOwner = Models.Utilities.IsOwner();
            return View();
        }

        [HttpPost]
        public ActionResult Enter(string data)
        {
            try
            {
                var employess = DomainObjects.FromJson<sp_GetUsers_Result>(data);
                if (String.IsNullOrEmpty(employess.Username))
                    return Json(new { message = ((DictionaryHandler)Models.Utilities.GetDicationary()).FillUsername, isSucceded = false });

                if (String.IsNullOrEmpty(employess.Password))
                    return Json(new { message = ((DictionaryHandler)Models.Utilities.GetDicationary()).FillPassword, isSucceded = false });

                if (String.IsNullOrEmpty(employess.CompanyName))
                    return Json(new { message = ((DictionaryHandler)Models.Utilities.GetDicationary()).FillCompany, isSucceded = false });

                var company = CompaniesHandler.Filter(new sp_GetCompanies_Result { Name = employess.CompanyName }).FirstOrDefault();
                if (company == null)
                    return Json(new { message = ((DictionaryHandler)Models.Utilities.GetDicationary()).CompanyNotExist, isSucceded = false });
                else
                {
                    if (company.Active == false)
                        return Json(new { message = ((DictionaryHandler)Models.Utilities.GetDicationary()).InactiveComapny, isSucceded = false });

                    //var User = UsersHandler.Filter(new sp_GetUsers_Result {Active = true, Username = employess.Username, Password = DomainObjects.Sha256Hash(employess.Password), CompanyID = company.GUID }).FirstOrDefault();
                    var User = UsersHandler.Filter(new sp_GetUsers_Result { Active = true, Username = employess.Username, Password = employess.Password, CompanyID = company.GUID }).FirstOrDefault();
                    if (User == null)
                        return Json(new { message = ((DictionaryHandler)Models.Utilities.GetDicationary()).UserNotFound, isSucceded = false });

                    Models.Utilities.SetUser(User);
                }

                return Json(new { message = "הצלחה", isSucceded = true });
            }
            catch(Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
            
        }

    }
}
