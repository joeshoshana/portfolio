using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class WeighingModesHandler
    {
        public static IEnumerable<sp_GetWeighingModes_Result> Filter(sp_GetWeighingModes_Result filter)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetWeighingModes(filter.GUID, filter.Name).ToList().AsEnumerable();
                
                return data.ToList();
            }
        }

    }
}
