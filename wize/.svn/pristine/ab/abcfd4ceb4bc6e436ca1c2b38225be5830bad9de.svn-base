using Shkila.Model;
using Shkila.Model.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class Request
    {
        public String Command { get; set; }
        public String MAC { get; set; }
        public String Weight { get; set; }

        internal bool Process()
        {
            try
            {
                switch (Command)
                {
                    case "update_weight":
                        var scale = new sp_GetScales_Result { MAC = MAC, Weight = Weight };
                        if (!ScalesHandler.IsConnected(scale))
                            throw new Exception("Connection Failed");

                        ScalesHandler.UpdateWeight(scale);
                        return true;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}