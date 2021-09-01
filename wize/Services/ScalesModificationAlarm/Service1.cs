using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace WeightModificationAlarm
{
    public partial class Service1 : ServiceBase
    {
        private Engine m_engine = new Engine();
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
                m_engine.Start();                
            }
            catch (Exception e)
            {
                OnStop();
            }
        }

        protected override void OnStop()
        {
            m_engine.Stop();
        }
    }
}
