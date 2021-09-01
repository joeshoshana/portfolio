using Shkila.Model;
using Shkila.Model.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WeightServer.Models;

namespace WeightServer.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(string request)
        {
            Response res = new Response();
            try
            {
                Request req = Utilities.FromJson<Request>(request);
                res.isSucceded = false;
                res.msg = String.Empty;
                if (req.Process())
                {
                    res.isSucceded = true;
                    res.msg = "OK";
                }
                else
                {
                    res.isSucceded = false;
                    res.msg = "Failed";
                }
            }
            catch (Exception ex)
            {
                res.isSucceded = false;
                res.msg = ex.Message;
            }
            return Utilities.ToJson(res); 
        }

        // POST api/values
        public string Post([FromBody]Request req)
        {
            Response res = new Response();
            try
            {
                res.isSucceded = false;
                res.msg = String.Empty;
                if (req.Process())
                {
                    res.isSucceded = true;
                    res.msg = "OK";
                }
                else
                {
                    res.isSucceded = false;
                    res.msg = "Failed";
                }
            }
            catch (Exception ex)
            {
                res.isSucceded = false;
                res.msg = ex.Message;
            }
            return Utilities.ToJson(res); 
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        public string Weight(string guid)
        {
            try
            {

                string weight = String.Empty;
                string error = String.Empty;
                    try
                    {
                        var Scales = ScalesHandler.Filter(new sp_GetScales_Result { GUID = Convert.ToInt64(guid), Active = true });

                        foreach (var scale in Scales)
                        {
                            scale.Status = true;
                            ScalesHandler.CheckConnection(scale);
                        }

                        weight = ScalesHandler.Weight(Scales.ElementAt(0));
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }

                if (!String.IsNullOrEmpty(error))
                    throw new Exception(error);

                return "{ msg :"+weight+", isSucceded = true}";
            }
            catch (Exception ex)
            {
                return "{ msg :" + ex.Message + ", isSucceded = true}";
            }
        }
    }
}