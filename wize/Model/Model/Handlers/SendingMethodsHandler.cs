using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class SendingMethodsHandler
    {
        public static IEnumerable<sp_GetSendingMethods_Result> Filter(sp_GetSendingMethods_Result filter)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetSendingMethods(filter.GUID, filter.Name).ToList().AsEnumerable();
                
                return data.ToList();
            }
        }

    }
}
