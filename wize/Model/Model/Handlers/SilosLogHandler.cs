using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

namespace Shkila.Model.Handlers
{
    public class SilosLogHandler
    {
        private static ReaderWriterLockSlim m_log_lock = new ReaderWriterLockSlim();
        public static void Save(sp_GetSilosLog_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveSiloLog(item.GUID, item.SiloID, item.LogDate, item.LogPath, item.CompanyID).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetSilosLog_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.SilosLog.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                db.SilosLog.Remove(dt);
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetSilosLog_Result item, ref List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "SiloID":
                    if (!item.SiloID.HasValue || item.SiloID.Value == 0)
                        errors.Add("InvalidScale");
                    break;
                case "CompanyID":
                    if (!item.CompanyID.HasValue || item.CompanyID.Value == 0)
                        errors.Add("InvalidCompany");
                    break;
                case "LogDate":
                    break;
                case "LogPath":
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetSilosLog_Result> Filter(sp_GetSilosLog_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetSilosLog(filter.AllowedRows, filter.GUID, filter.CompanyID, filter.SiloID).ToList().AsEnumerable();

                if (filter.FromDate.HasValue)
                    data = data.Where(i => i.LogDate >= filter.FromDate.Value);

                if (filter.ToDate.HasValue)
                    data = data.Where(i => i.LogDate < filter.ToDate.Value.AddDays(1));

                return data.ToList();
            }
        }

        public static void UpdateLog(string path, string msg)
        {
            StreamWriter sw = null;
            try
            {

                m_log_lock.EnterWriteLock();
                sw = new StreamWriter(path, true, ASCIIEncoding.UTF8);
                sw.WriteLine(msg);
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
                sw = null;
                m_log_lock.ExitWriteLock();
            }
            

        }
    }
}
