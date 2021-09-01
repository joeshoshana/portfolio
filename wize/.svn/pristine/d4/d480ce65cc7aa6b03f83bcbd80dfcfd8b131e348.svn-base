using Server.Models;
using Shkila.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
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
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}