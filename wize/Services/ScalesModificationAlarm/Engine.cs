using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Shkila.Utilities;
using System.Threading;

namespace WeightModificationAlarm
{
    public class Engine
    {
        private string m_configPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Configuration.xml");
        private string m_logPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Log.txt");
        private Configuration config = null;
        private Dictionary<string, DateTime?> m_scales = new Dictionary<string, DateTime?>();
        private Dictionary<string, DateTime?> m_stuckScales = new Dictionary<string,DateTime?>();
        private Dictionary<long, List<sp_GetScales_Result>> stuckScales = new Dictionary<long, List<sp_GetScales_Result>>();
        private Dictionary<long, List<sp_GetScales_Result>> reactiveScales = new Dictionary<long, List<sp_GetScales_Result>>();
        private List<string> to = new List<string>();
        private Timer m_checkertimer;
        private Thread t = null;

        public Engine()
        {
           
            try
            {
                config = XmlSerializerHandler.Load<Configuration>(m_configPath);
                foreach (Mail m in config.Mails)
                    to.Add(m.MailAddress);
            }
            catch(Exception ex)
            {
                Logger.Log(ex, m_logPath);
            }            
        }

        public void Stop()
        {
            try
            {
                if (t != null)
                    t.Abort();
                
                t = null;
            }
            catch(Exception ex)
            {
                Logger.Log(ex, m_logPath);                
            }
            
        }

        public void Start()
        {
            if (config.Interval.HasValue)
                m_checkertimer = new Timer(_Start, null, 1000, 1000 * 60 * config.Interval.Value);                         
        }

        private void _Start(object state)
        {
            try
                {
                    using (WanagerDBEntities db = new WanagerDBEntities())
                    {
                        var scales = db.sp_GetScales(-1, 0, null, null, null, null, null, null, true).ToList();                        
                        var scalesGropByMAC = scales.GroupBy(i => i.MAC);

                        RemoveOfflineStuckScales();

                        foreach (var scale in scalesGropByMAC)
                        {
                            if (!m_scales.ContainsKey(scale.Key))
                                m_scales.Add(scale.Key, scale.ElementAt(0).WeightDate);
                            else
                            {
                                if (m_scales[scale.Key].Value == scale.ElementAt(0).WeightDate.Value)
                                {
                                    if (!m_stuckScales.ContainsKey(scale.Key))
                                    {
                                        m_stuckScales.Add(scale.Key, scale.ElementAt(0).WeightDate);
                                        foreach (var sc in scale)
                                        {
                                            sc.Weight = "999999";
                                            db.sp_SaveScale(sc.GUID,sc.MAC,sc.CompanyID,sc.Status,sc.Weight,sc.WeightDate,sc.Active,sc.Name,sc.ScalesTypeID,sc.IsDemo,sc.UnitID,sc.OwnerID);

                                            if (!stuckScales.ContainsKey(sc.CompanyID.Value))
                                                stuckScales.Add(sc.CompanyID.Value, new List<sp_GetScales_Result>());
                                            stuckScales[sc.CompanyID.Value].Add(sc);

                                            //var usersToNotify = db.sp_GetUsers(-1,0,sc.CompanyID,null,null,null,null,null,null,null,null,null);

                                        }
                                    }
                                }
                                else
                                {
                                    if (m_stuckScales.ContainsKey(scale.Key))
                                    {
                                        m_stuckScales.Remove(scale.Key);
                                        
                                        foreach (var sc in scale)
                                        {
                                            if (!reactiveScales.ContainsKey(sc.CompanyID.Value))
                                                reactiveScales.Add(sc.CompanyID.Value, new List<sp_GetScales_Result>());
                                            reactiveScales[sc.CompanyID.Value].Add(sc);
                                            if (stuckScales.ContainsKey(sc.CompanyID.Value))
                                                stuckScales[sc.CompanyID.Value].RemoveAll(i => i.MAC.Equals(sc.MAC));
                                        }
                                    }

                                    m_scales[scale.Key] = scale.ElementAt(0).WeightDate;
                                }
                            }
                        }

                        AlarmScaleStatus(reactiveScales, "Reactivated Scales", true);
                        AlarmScaleStatus(stuckScales, "Deactivated Scales", true);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex, m_logPath);
                    DomainObjects.SendMail(to, null, "Error", ex.Message + " --> " + ex.StackTrace);
                }
        }

        private void RemoveOfflineStuckScales()
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var notActiveScales = db.sp_GetScales(-1, 0, null, null, null, null, null, null, false).Where(i => !i.Active).ToList();
                var removeFromStuck = m_stuckScales.Where(i => notActiveScales.Any(j => j.MAC == i.Key)).ToList();
                foreach (var rm in removeFromStuck)
                {
                    if (m_stuckScales.ContainsKey(rm.Key))
                        m_stuckScales.Remove(rm.Key);
                }
            }
        }

        private void AlarmScaleStatus(Dictionary<long, List<sp_GetScales_Result>> scales, string title, bool clear)
        {
            try
            {
                if (scales.Count > 0)
                {
                    string msg = String.Empty;
                    foreach (var rs in scales)
                    {
                        foreach (var sc in rs.Value)
                            msg += String.Format("{0}: {1}:{2}{3}", sc.CompanyName, sc.Name, sc.MAC, Environment.NewLine);
                    }

                    DomainObjects.SendMail(to, null, title, msg);
                    if (clear)
                        scales.Clear();
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ex, m_logPath);
                DomainObjects.SendMail(to, null, "Error", ex.Message + " --> " + ex.StackTrace);
            }
            
        }
    }
}
