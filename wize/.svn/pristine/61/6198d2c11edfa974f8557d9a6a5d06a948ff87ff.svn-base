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
    public class SendingMethodsController : Controller
    {
      
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

                sp_GetSendingMethods_Result item = DomainObjects.FromJson<sp_GetSendingMethods_Result>(data);

                if (item == null)
                    item = new sp_GetSendingMethods_Result();
                
                int totalRecords = 0;
                var weighingModes = SendingMethodsHandler.Filter(new sp_GetSendingMethods_Result { });

                foreach (sp_GetSendingMethods_Result table in weighingModes)
                    table.Name = ((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(table.Name);

                totalRecords = weighingModes.Count();
                weighingModes = Models.Utilities.TableManipulation<sp_GetSendingMethods_Result>(weighingModes, Request.Form).ToList();

                return Json(new { data = weighingModes, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }
        }

    }
}
