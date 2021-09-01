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
    public class SiloWeighingController : Controller
    {
        //
        // GET: /SiloWeighing/
        public readonly int FormID = 1; 
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
        public ActionResult UpdateLog(string data, bool? loading, string weight, string time)
        {
            string log = Path.Combine( Server.MapPath("~/App_Data"),"log.txt");
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                if (String.IsNullOrEmpty(weight) || String.IsNullOrEmpty(time))
                    return Json(new { data = "", isSucceded = true });
                DateTime d = DateTime.ParseExact(time,"dd/MM/yyyy HH:mm:ss",null);
                sp_GetSilos_Result item = DomainObjects.FromJson<sp_GetSilos_Result>(data);
                item.CompanyID = User.CompanyID;
                string silosFolder = Server.MapPath("~/App_Data/" + item.CompanyID + "/Silos");
                if (!System.IO.Directory.Exists(silosFolder))
                    System.IO.Directory.CreateDirectory(silosFolder);
                string msg = String.Empty;
                if(item.IsLoad && loading.HasValue && loading.Value)
                    msg = String.Format( "{0} - {1} - {2}",time, ((DictionaryHandler)Models.Utilities.GetDicationary()).LoadStart , weight);
                else if(item.IsUnload && loading.HasValue && !loading.Value)
                    msg = String.Format("{0} - {1} - {2}", time, ((DictionaryHandler)Models.Utilities.GetDicationary()).UnloadStart, weight);
                else
                    msg = String.Format("{0} - {1} - {2}", time, ((DictionaryHandler)Models.Utilities.GetDicationary()).Weight, weight);
                
                string logFile = System.IO.Path.Combine(silosFolder, item.GUID.ToString() + "_" + d.ToString("ddMMyyyy") + ".txt");
                if(!System.IO.File.Exists(logFile))
                {
                    System.IO.File.Create(logFile);
                    SilosLogHandler.Save(new sp_GetSilosLog_Result {CompanyID = item.CompanyID,SiloID = item.GUID,LogDate = DateTime.Now,LogPath = logFile });
                }

                if(!String.IsNullOrEmpty(msg))
                    SilosLogHandler.UpdateLog(logFile, msg);

                return Json(new { data = "", isSucceded = true });
            }
            catch (Exception ex)
            {
                
                Logger.Log(ex, log);                
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



                sp_GetSilos_Result item = DomainObjects.FromJson<sp_GetSilos_Result>(data);
                item.CompanyID = User.CompanyID;
                var items = SilosHandler.Filter(item);
                items = CompaniesFormsHandler.ReturnAllowedRows<sp_GetSilos_Result>(FormID, User.CompanyID.Value ,items);
                
                return Json(new { data = items, isSucceded = true });
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

                sp_GetSilos_Result item = DomainObjects.FromJson<sp_GetSilos_Result>(data);
                item.CompanyID = User.CompanyID;
                if (item.GUID == 0)
                    item.Active = true;

                List<string> errors = new List<string>();
                bool isValid = true;

                //isValid = SilosHandler.Validate("ScaleID", item, ref errors);
                var fields = CompaniesFormsFieldsHandler.RequiredFields(new sp_GetCompaniesFormsFields_Result { FormID = 1, CompanyID = User.CompanyID });

                if (fields.Any(i => i.FormsFieldName.Equals("SiteName")))
                    isValid = SilosHandler.Validate("SiteName", item, ref errors);
                if (fields.Any(i => i.FormsFieldName.Equals("SiloName")))
                    isValid = SilosHandler.Validate("Name", item, ref errors);
                if (fields.Any(i => i.FormsFieldName.Equals("MaxCapacity")))
                    isValid = SilosHandler.Validate("MaxCapacity", item, ref errors);

                if (!isValid)
                {
                    Models.Utilities.ConvertErrorsPhraseToLanguage(errors);

                    return Json(new { message = DomainObjects.LinkString(errors, Environment.NewLine), isSucceded = false });
                }
                else
                {
                    SilosHandler.Save(item);
                }
                
                return Json(new { message = "", isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }

        }

        [HttpGet]
        public ActionResult Log(string data, string fromDate, string toDate)
        {
            string logF = Path.Combine(Server.MapPath("~/App_Data"), "log.txt");
            try
            {
                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                sp_GetSilos_Result item = DomainObjects.FromJson<sp_GetSilos_Result>(data);
                item.CompanyID = User.CompanyID;


                DateTime from = DateTime.MinValue;
                if(!String.IsNullOrEmpty(fromDate))
                    from = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                
                DateTime to = DateTime.MinValue;
                if(!String.IsNullOrEmpty(toDate))
                to = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);

                sp_GetSilosLog_Result logFilter = new sp_GetSilosLog_Result
                {
                    CompanyID = item.CompanyID,
                    SiloID = item.GUID
                };

                if (from > DateTime.MinValue)
                    logFilter.FromDate = from;

                if (to > DateTime.MinValue)
                    logFilter.ToDate = to;

                var logs = SilosLogHandler.Filter(logFilter);
                MemoryStream finalFile = new MemoryStream();
                foreach (sp_GetSilosLog_Result log in logs)
                {
                    if (!String.IsNullOrEmpty(log.LogPath) && System.IO.File.Exists(log.LogPath))
                    {
                        using(FileStream fs = System.IO.File.OpenRead(log.LogPath))
                            fs.CopyTo(finalFile);
                    }
                }

                return File(finalFile.GetBuffer(), "text/plain", "log.txt"); ;
            }
            catch (Exception ex)
            {
                Logger.Log(ex, logF);
                return Json(new { message = ex.Message, isSucceded = false });
            }

        }

    }
}
