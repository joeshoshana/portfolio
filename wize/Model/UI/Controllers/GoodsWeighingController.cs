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
    public class GoodsWeighingController : Controller
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

        [HttpPost]
        public ActionResult Get(string data)
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            sp_GetGoodsWeighing_Result item = DomainObjects.FromJson<sp_GetGoodsWeighing_Result>(data);

            if (item == null)
                item = new sp_GetGoodsWeighing_Result();


            int totalRecords = 0;
            var goodsWeighing = GoodsWeighingHandler.Filter(item);
            totalRecords = goodsWeighing.Count();
            goodsWeighing = Models.Utilities.TableManipulation<sp_GetGoodsWeighing_Result>(goodsWeighing, Request.Form);
            
            


            return Json(new { data = goodsWeighing, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
        }

        [HttpPost]
        public ActionResult Save(string data, string tables, string forms)
        {
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetGoodsWeighing_Result item = DomainObjects.FromJson<sp_GetGoodsWeighing_Result>(data);

                GoodsWeighingHandler.Save(item);
                
                return Json(new { message = "", isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }

        }

        public ActionResult Weighing()
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
    }
}
