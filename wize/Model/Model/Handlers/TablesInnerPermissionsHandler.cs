using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class TablesInnerPermissionsHandler
    {
        public static IEnumerable<sp_GetTablesInnerPermissions_Result> Filter(sp_GetTablesInnerPermissions_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetTablesInnerPermissions(filter.GUID, filter.TableID, filter.Name).ToList().AsEnumerable();
                
                return data.ToList();
            }
        }

    }
}
