using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class InnerPermissionsHandler
    {
        public static IEnumerable<sp_GetInnerPermissions_Result> Filter(sp_GetInnerPermissions_Result filter)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetInnerPermissions(filter.GUID, filter.Name).ToList().AsEnumerable();
                
                return data.ToList();
            }
        }

    }
}
