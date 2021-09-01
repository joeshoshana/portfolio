using Shkila.Model;
using Shkila.Model.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class ItemsWeighingController : Controller
    {
        public readonly int FormID = 6; 
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

        public ActionResult Weighing(long id)
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var item = ItemsHandler.Filter(new sp_GetItems_Result { Active = true, GUID = id, CompanyID = User.CompanyID}).FirstOrDefault();
            ViewBag.ItemID = id;
            return View(item);
        }

        public ActionResult Configuration()
        {
            return View();
        }

    }
}
