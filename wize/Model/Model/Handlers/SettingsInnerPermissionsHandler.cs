using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class SettingsInnerPermissionsHandler
    {
        public static IEnumerable<sp_GetSettingsInnerPermissions_Result> Filter(sp_GetSettingsInnerPermissions_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetSettingsInnerPermissions(filter.GUID, filter.SettingID, filter.Name).ToList().AsEnumerable();
                
                return data.ToList();
            }
        }

    }
}
