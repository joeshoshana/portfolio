using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class FormsInnerPermissionsHandler
    {
        public static IEnumerable<sp_GetFormsInnerPermissions_Result> Filter(sp_GetFormsInnerPermissions_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetFormsInnerPermissions(filter.GUID, filter.FormID, filter.Name).ToList().AsEnumerable();
                
                return data.ToList();
            }
        }

    }
}
