using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Web.WebSockets;
using System.Web;
using Shkila.Model;
using Shkila.Model.Handlers;

namespace UI.Controllers
{
    class ChatWebSocketHandler : Microsoft.Web.WebSockets.WebSocketHandler
    {
        private  WebSocketCollection _chatClients = new WebSocketCollection();
        private bool isOpen = false;
        
        public override void OnOpen()
        {
            _chatClients.Add(this);
            isOpen = true;
        }

        public override void OnClose()
        {
            base.OnClose();
            isOpen = false;
            
        }
        public override void OnMessage(string recordType)
        {
            sp_GetUsers_Result User = null;
           // User = (sp_GetUsers_Result)Models.Utilities.GetUser();
            while (isOpen)
            {
                try
                {
            /*        if (User.CompanyID == 2)
                        _chatClients.Broadcast(new Random().Next(5000).ToString());
                    else*/
                    long id;
                    if (long.TryParse(recordType, out id))
                    {
                        _chatClients.Broadcast(ScalesHandler.Weight(new sp_GetScales_Result { GUID = id }));
                    }
                    else
                        throw new Exception(((DictionaryHandler)Models.Utilities.GetDicationary()).InvalidWeight);
                }
                catch (Exception ex)
                {
                    _chatClients.Broadcast(ex.Message); 
                }
                
                System.Threading.Thread.Sleep(1000);
            }           
        }
    }

    public class WeightSocketController : ApiController
    {
        // GET api/weightsocket
        public HttpResponseMessage Get()
        {
            try
            {
                HttpContext.Current.AcceptWebSocketRequest(new ChatWebSocketHandler());
                return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);  
            }
            catch(Exception ex)
            {
                return null;
            }


            
        }

        // GET api/weightsocket/5
        public String Get(int id)
        {
            return String.Empty;
        }

          
        // POST api/weightsocket
        public void Post([FromBody]string value)
        {
        }

        // PUT api/weightsocket/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/weightsocket/5
        public void Delete(int id)
        {
        }
    }
}
