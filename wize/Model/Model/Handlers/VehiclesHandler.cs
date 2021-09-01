using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

namespace Shkila.Model.Handlers
{
    public class VehiclesHandler
    {
        public static void Save(sp_GetVehicles_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveVehicle(item.GUID, item.CompanyID, item.LicenseNumber, item.Active, item.Tare, item.CustomerID, item.TransportID, item.IsAddInForm, item.WeighingModeID).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetVehicles_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.Vehicles.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                dt.Active = false;
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetVehicles_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "LicenseNumber":
                    if (String.IsNullOrEmpty(item.LicenseNumber))
                        errors.Add("MissingLicenseNumber");
                    break;
                case "CompanyID":
                    if (!item.CompanyID.HasValue || item.CompanyID.Value == 0)
                        errors.Add("InvalidCompany");
                    break;
                case "CustomerID":
                    if (!item.CustomerID.HasValue || item.CustomerID.Value == 0)
                        errors.Add("InvalidCustomer");
                    break;
                case "TransportID":
                    if (!item.TransportID.HasValue || item.TransportID.Value == 0)
                        errors.Add("InvalidTransport");
                    break;
                case "Active":
                    break;
                case "Tare":
                    if (!item.Tare.HasValue)
                        errors.Add("InvalidTare");
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetVehicles_Result> Filter(sp_GetVehicles_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetVehicles(filter.AllowedRows, filter.GUID, filter.CompanyID, filter.LicenseNumber, filter.Active, filter.CustomerID, filter.TransportID).ToList().AsEnumerable();

                return data.ToList();
            }
        }
    }
}
