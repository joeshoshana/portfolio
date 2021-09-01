using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

namespace Shkila.Model.Handlers
{
    public class ConnectionsHandler
    {
        public static void Save(sp_GetConnections_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveConnection(item.GUID, item.Tag, item.CompanyID, item.VehicleID, item.ItemID, item.TransportID, item.DriverID, item.InSiteID, item.OutSiteID, item.CustomerID).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetConnections_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.Connections.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                db.Connections.Remove(dt);
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetConnections_Result item, ref List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "Tag":
                    break;
                case "CompanyID":
                    if (!item.CompanyID.HasValue || item.CompanyID.Value == 0)
                        errors.Add("InvalidCompany");
                    break;
                case "VehicleID":
                    break;
                case "ItemID":
                    break;
                case "TransportID":
                    break;
                case "DriverID":
                    break;
                case "InSiteID":
                    break;
                case "OutSiteID":
                    break;
                case "CustomerID":
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetConnections_Result> Filter(sp_GetConnections_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetConnections(filter.AllowedRows, filter.GUID, filter.Tag, filter.CompanyID, filter.VehicleID, filter.TransportID, filter.ItemID, filter.InSiteID, filter.OutSiteID, filter.CustomerID, filter.DriverID).ToList().AsEnumerable();

                return data.ToList();
            }
        }
    }
}
