using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

namespace Shkila.Model.Handlers
{
    public class ScalesHandler
    {
        public static void Save(sp_GetScales_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveScale(item.GUID, item.MAC, item.CompanyID, item.Status, item.Weight, item.WeightDate, item.Active, item.Name, item.ScalesTypeID, item.IsDemo, item.UnitID, item.OwnerID).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetScales_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.Scales.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                dt.Active = false;
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetScales_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "MAC": if (String.IsNullOrEmpty(item.MAC))
                        errors.Add("MissingMAC");
                    break;
                case "CompanyID":
                    if (!item.CompanyID.HasValue || item.CompanyID.Value == 0)
                        errors.Add("InvalidCompany");
                    break;
                case "OwnerID":
                    if (!item.OwnerID.HasValue || item.OwnerID.Value == 0)
                        errors.Add("InvalidOwner");
                    break;
                case "Status":
                    break;
                case "Weight":
                    break;
                case "WeightDate":
                    break;
                case "Active":
                    break;
                case "Name":
                    if (String.IsNullOrEmpty(item.Name))
                        errors.Add("MissingName");
                    break;      
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetScales_Result> Filter(sp_GetScales_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetScales(filter.AllowedRows, filter.GUID, filter.CompanyID, filter.OwnerID, filter.Name, filter.MAC, filter.ScalesTypeID, filter.Status, filter.Active).ToList().AsEnumerable();

                return data.ToList();
            }
        }




        public static bool IsConnected(sp_GetScales_Result item)
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var scale = Filter(item).FirstOrDefault();
                if (scale == null)
                    return false;

                if (scale.Active == false)
                    return false;

                if (scale.Status == false)
                    return false;
                
                return true;
            }
        }

        public static void UpdateWeight(sp_GetScales_Result item)
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var scales = Filter(item);
                if (scales.Count() == 0)
                    return;

                foreach(var scale in scales)
                {
                    scale.Weight = item.Weight;
                    scale.WeightDate = DateTime.Now;

                    Save(scale);
                }                
            }
        }

        public static bool UpdateStatus(sp_GetScales_Result item)
        {
            try
            {
                using (WanagerDBEntities db = new WanagerDBEntities())
                {
                    var scale = Filter(item).FirstOrDefault();
                    if (scale == null)
                        return false;

                    if (scale.Status == true || scale.Status == null)
                    {
                        scale.Status = false;
                    }
                    else
                    {
                        scale.Status = true;
                    }

                    Save(scale);

                    CheckConnection(scale);

                    return (bool)scale.Status;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        private static bool IsGotWeight(sp_GetScales_Result item)
        {
            int maxCount = 3;
            int counter = 0;
            DateTime? prevWeightDate = null;
            while(counter < maxCount)
            {
                counter++;
                var scale = Filter(item).FirstOrDefault();
                if (scale == null)
                    return false;

                if (scale.IsDemo)
                    return true;

                if (prevWeightDate == null)
                {
                    prevWeightDate = scale.WeightDate;                    
                }
                else
                {
                    if (prevWeightDate < scale.WeightDate)
                        return true;
                }
                Thread.Sleep(500);                                
            }
            return false;            
        }

        public static void CheckConnection(sp_GetScales_Result scale)
        {
            if (scale.Status == true)
                if (!IsGotWeight(scale))
                {
                    scale.Status = false;
                    Save(scale);
                }
        }

        public static string Weight(sp_GetScales_Result item)
        {
            try
            {
                using (WanagerDBEntities db = new WanagerDBEntities())
                {
                    return db.sp_GetWeight(item.GUID).ElementAt(0);
                    /*var scale = Filter(item).FirstOrDefault();
                    if (scale == null)
                        throw new Exception("No Scale Detected");

                    if(scale.Status == false)
                        throw new Exception("Scale Not Connected");

                    if (scale.Weight == null)
                        return "0";

                    return scale.Weight;*/
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
