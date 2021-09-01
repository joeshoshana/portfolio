using Shkila.Model;
using Shkila.Model.Handlers;
using Shkila.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class ScalesController : Controller
    {
        public readonly int SettingsID = 5;
        public ActionResult Index()
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.Dictionary = Models.Utilities.GetDicationary();
            ViewBag.IsOwner = Models.Utilities.IsOwner();

            var Scales = ScalesHandler.Filter(new sp_GetScales_Result { CompanyID = User.CompanyID, Active = true });

            foreach (var scale in Scales)
            {
                scale.Status = true;
                ScalesHandler.Save(scale);
                ScalesHandler.CheckConnection(scale);
            }

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

            sp_GetScales_Result item = DomainObjects.FromJson<sp_GetScales_Result>(data);
            if (item == null)
                item = new sp_GetScales_Result();


            item.CompanyID = User.CompanyID;
            item.Active = true;

            if (User.CompanyIsSuper)
            {
                item.OwnerID = 0;
                item.CompanyID = 0;
            }
            else if (User.CompanyIsOwner)
            {
                item.CompanyID = 0;
                item.OwnerID = User.CompanyID;
            }

            int totalRecords = 0;
            sp_GetCompaniesSettings_Result ct = CompaniesSettingsHandler.Filter(new sp_GetCompaniesSettings_Result { SettingID = SettingsID, CompanyID = User.CompanyID.Value }).FirstOrDefault();
            item.AllowedRows = (ct == null ? -1 : ct.AllowedRows);
            var items = ScalesHandler.Filter(item);

            totalRecords = items.Count();
            items = Models.Utilities.TableManipulation<sp_GetScales_Result>(items, Request.Form);

            return Json(new { data = items, recordsTotal = totalRecords, recordsFiltered = totalRecords, isSucceded = true });
        }

        [HttpPost]
        public ActionResult GetScalesTypes()
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var Scales = ScalesTypesHandler.Filter(new sp_GetScalesTypes_Result { Active = true });


            return Json(new { data = Scales, isSucceded = true });
        }

        [HttpPost] // not require an authentication' becuase it calls from out side the web(raspberry pi)
        public ActionResult IsConnected(string data)
        {
            return Json(new { Connected = ScalesHandler.IsConnected(DomainObjects.FromJson<sp_GetScales_Result>(data)) });
        }

        [HttpPost] // not require an authentication' becuase it calls from out side the web(raspberry pi)
        public ActionResult UpdateWeight(string data)
        {
            sp_GetScales_Result scale = DomainObjects.FromJson<sp_GetScales_Result>(data);
            if (!ScalesHandler.IsConnected(scale))
                return Json(new { message = "Connection Failed", isSucceded = false });

            ScalesHandler.UpdateWeight(scale);
            return Json(new { message = "Weight Updated", isSucceded = true });
        }

        [HttpPost]
        public ActionResult ChangeConnectionState(string data)
        {
            sp_GetUsers_Result User = null;
            if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
            {
                return RedirectToAction("Login", "Login");
            }

            bool status = false;
            Thread t = new Thread(() =>
            {
                sp_GetScales_Result scale = DomainObjects.FromJson<sp_GetScales_Result>(data);
                status = ScalesHandler.UpdateStatus(scale);
            });

            t.IsBackground = true;
            t.Start();
            t.Join();

            return Json(new { message = status ? "Connected" : "Disconnected", isSucceded = true });
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


                sp_GetScales_Result item = DomainObjects.FromJson<sp_GetScales_Result>(data);
                if (!item.CompanyID.HasValue || item.CompanyID.Value == 0)
                    item.CompanyID = User.CompanyID;

                if (User.CompanyIsOwner)
                    item.OwnerID = User.CompanyID;

                var company = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = (long)item.CompanyID }).FirstOrDefault();
                if (company != null)
                {
                    if (company.IsOwner)
                        item.OwnerID = company.GUID;
                }

                if (item.GUID == 0)
                {
                    item.Active = true;
                    item.Status = false;
                }

                item.MAC = item.MAC.TrimStart(' ');
                item.MAC = item.MAC.TrimEnd(' ');
                ScalesHandler.Save(item);

                return Json(new { message = "", isSucceded = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, isSucceded = false });
            }

        }

        [HttpPost]
        public ActionResult Client(string request)
        {
            try
            {
                Models.RequestEntity req = DomainObjects.FromJson<Models.RequestEntity>(request);
                if (req.Process())
                    return Json(new { msg = "OK " + req._command, isSucceded = true });
                else
                    return Json(new { msg = "Failed", isSucceded = false });
            }
            catch (Exception ex)
            {
                var req = request.Length > 30 ? request.Substring(13, 30) : "";
                return Json(new { msg = "Server - " + ex.Message + req, isSucceded = false });
            }
        }

        [HttpPost]
        public ActionResult Weight(string guid)
        {
            try
            {
                //return Json(new { msg = "5.23", isSucceded = false });

                sp_GetUsers_Result User = null;
                if ((User = (sp_GetUsers_Result)Models.Utilities.GetUser()) == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                string weight = String.Empty;
                string error = String.Empty;
                Thread t = new Thread(() =>
                {
                    try
                    {
                        if (User.CompanyID == 2)
                            weight = new Random().Next(5000).ToString();
                        else
                        {
                            long id;
                            if (long.TryParse(guid, out id))
                                weight = ScalesHandler.Weight(new sp_GetScales_Result { GUID = id });
                            else
                                throw new Exception(((DictionaryHandler)Models.Utilities.GetDicationary()).InvalidInWeight);
                        }

                        /*var Scales = ScalesHandler.Filter(new sp_GetScales_Result { CompanyID = User.CompanyID, Active = true });

                        long id;
                        if (long.TryParse(guid, out id))
                            Scales = Scales.Where(i => i.GUID == id);

                        foreach (var scale in Scales)
                        {
                            scale.Status = true;
                            ScalesHandler.CheckConnection(scale);
                        }*/

                        //weight = ScalesHandler.Weight(Scales.ElementAt(0));
                        //weight = "5.23";
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }
                });
                t.IsBackground = true;
                t.Start();
                t.Join();

                if (!String.IsNullOrEmpty(error))
                    throw new Exception(error);

                return Json(new { msg = weight, isSucceded = false });
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, isSucceded = false });
            }
        }
    }
}
