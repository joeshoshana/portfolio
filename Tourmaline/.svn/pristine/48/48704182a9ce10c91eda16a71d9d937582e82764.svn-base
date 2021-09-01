using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Tourmaline_v2
{
    public partial class Service1 : ServiceBase
    {
        private Engine m_engine = Engine.Instance;  
        public Service1()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                m_engine.WriteToLog("Info", "Checking License");
                string error = m_engine.IsLicenseValid();
                if (!String.IsNullOrEmpty(error))
                {
                    m_engine.WriteToLog("Info", error);
                    this.Stop();
                    return;
                }  
                m_engine.WriteToLog("Info", "Start Process");
                m_engine.StartProcessThread();
            }
            catch (Exception e)
            {
                OnStop();
            }
        }

        protected override void OnStop()
        {
            m_engine.WriteToLog("Info", "Stop Process");
            m_engine.StopProcessThread();
        }

        
    }
}
