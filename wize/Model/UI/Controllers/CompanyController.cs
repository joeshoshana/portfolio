using Shkila.Model;
using Shkila.Model.Handlers;
using Shkila.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UI.Controllers
{
    public class CompanyController : ApiController
    {
        public string Get(string tag)
        {
            var companyName = "Wrong tag - No company found with this ID";

            try
            {
                companyName = ConnectionsHandler.Filter(new sp_GetConnections_Result { Tag = tag }).FirstOrDefault().CompanyName;
            }
            catch (Exception)
            {

            }

            return companyName;
        }
    }
}
