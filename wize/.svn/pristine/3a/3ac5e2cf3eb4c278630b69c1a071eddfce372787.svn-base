using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class UnitsHandler
    {
        public static IEnumerable<sp_GetUnits_Result> Filter(sp_GetUnits_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetUnits(filter.GUID, filter.Name).ToList().AsEnumerable();
                
                return data.ToList();
            }
        }

    }
}
