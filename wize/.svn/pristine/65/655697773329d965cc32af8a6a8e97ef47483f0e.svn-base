using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

namespace Shkila.Model.Handlers
{
    public class SilosHandler
    {
        private static ReaderWriterLockSlim m_log_lock = new ReaderWriterLockSlim();
        public static void Save(sp_GetSilos_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveSilo(item.GUID, item.CompanyID, item.ScaleID, item.Name, item.SiteName, item.MaxCapacity, item.Active, item.IsLoad, item.IsUnload, item.LoadInterval, item.UnloadInterval, item.LoadIntervalTime, item.UnloadIntervalTime, item.IsLogWeight,item.LogWeightTime).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetSilos_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.Silos.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                dt.Active = false;
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetSilos_Result item,ref List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "ScaleID":
                    if (!item.ScaleID.HasValue || item.ScaleID.Value == 0)
                        errors.Add("InvalidScale");
                    break;
                case "CompanyID":
                    if (!item.CompanyID.HasValue || item.CompanyID.Value == 0)
                        errors.Add("InvalidCompany");
                    break;
                    case "Name":
                    if (String.IsNullOrEmpty(item.Name))
                        errors.Add("MissingName");
                    break;
                    case "SiteName":
                    if (String.IsNullOrEmpty(item.SiteName))
                        errors.Add("MissingSite");
                    break;
                    case "MaxCapacity":
                    if (!item.MaxCapacity.HasValue || item.MaxCapacity.Value < 0)
                        errors.Add("InvalidMaxCapacity");
                    break;
                case "Active":
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetSilos_Result> Filter(sp_GetSilos_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetSilos(filter.AllowedRows, filter.GUID, filter.CompanyID, filter.Name, filter.SiteName, filter.Active).ToList().AsEnumerable();

                return data.ToList();
            }
        }

        public static void UpdateLog(string path, string msg)
        {
            try
            {
                m_log_lock.EnterWriteLock();
                StreamWriter sw = new StreamWriter(path, true, ASCIIEncoding.UTF8);
                sw.WriteLine(msg);
                sw.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_log_lock.ExitWriteLock();
            }
            

        }
    }
}
