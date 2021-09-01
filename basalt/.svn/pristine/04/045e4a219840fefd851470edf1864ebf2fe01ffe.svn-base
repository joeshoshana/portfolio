using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;

using Shkila.ScaleReaders;

namespace Basalt_v2
{
    public class Configuration
    {
        private string m_ConfigurationPath = "Configuration.xml";

        public string ComPort { get; set; }
        public int? DataBits { get; set; }
        public StopBits? StopBits { get; set; }
        public Parity? Parity { get; set; }
        public ConnectionType? ConnectionType { get; set; }
        public ScaleHeaders? ScaleHeader { get; set; }
        public float? Multiple { get; set; }
        public int? BaudRate { get; set; }
        public bool? RTS { get; set; }
        public bool? DTR { get; set; }
        public int? Port{ get; set; }
        public string IP { get; set; }
        public Brush MainColor { get; set; }
        public string ActivationKey { get; set; }
        public string FinishChar { get; set; }
        public string Error { get; set; }
        public string Username { get; set; }
        public string Company { get; set; }
        public string LogFilePath { get; set; }           
        public bool AutoStart { get; set; }
        public bool AutoComCheck { get; set; }

        public bool LoadConfig()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(m_ConfigurationPath);
            XmlNode conNodes = doc.SelectSingleNode("Configuration");
            XmlNodeList nodes = conNodes.ChildNodes;
            foreach (XmlNode node in nodes)
            {
                string key = node.Name;
                string val = node.InnerText;
                PropertyInfo prop = this.GetType().GetProperty(key);
                if (prop != null)
                {
                    if (prop.PropertyType.FullName.Contains("Int32"))
                    {
                        int v;
                        if (int.TryParse(val, out v))
                        {
                            prop.SetValue(this, v, null);
                        }
                    }
                    else if (prop.PropertyType.FullName.Contains("Boolean"))
                    {
                        bool v;
                        if (bool.TryParse(val, out v))
                        {
                            prop.SetValue(this, v, null);
                        }
                    }
                    else if (prop.PropertyType.FullName.Contains("Parity"))
                    {
                        Parity v;
                        if (Enum.TryParse(val, out v))
                        {
                            prop.SetValue(this, v, null);
                        }
                    }
                    else if (prop.PropertyType.FullName.Contains("StopBits"))
                    {
                        StopBits v;
                        if (Enum.TryParse(val, out v))
                        {
                            prop.SetValue(this, v, null);
                        }
                    }
                    else if (prop.PropertyType.FullName.Contains("ConnectionType"))
                    {
                        ConnectionType v;
                        if (Enum.TryParse(val, out v))
                        {
                            prop.SetValue(this, v, null);
                        }
                    }
                    else if (prop.PropertyType.FullName.Contains("ScaleHeaders"))
                    {
                        ScaleHeaders v;
                        if (Enum.TryParse(val, out v))
                        {
                            prop.SetValue(this, v, null);
                        }
                    }
                    else if (prop.PropertyType.FullName.Contains("Brush"))
                    {
                        try
                        {
                             MainColor = (Brush)new BrushConverter().ConvertFromString(val); 
                        }catch(Exception ex)
                        {
                            MainColor = Brushes.Red;
                        }
                        
                    }
                    else if (prop.PropertyType.FullName.Contains("Int64"))
                    {
                        long v;
                        if (long.TryParse(val, out v))
                        {
                            prop.SetValue(this, v, null);
                        }
                    }
                    else if (prop.PropertyType.FullName.Contains("Single"))
                    {
                        float v;
                        if (float.TryParse(val, out v))
                        {
                            prop.SetValue(this, v, null);
                        }
                    }
                    else
                    {
                        if(String.IsNullOrEmpty(val))
                            prop.SetValue(this, String.Empty, null);
                        else
                            prop.SetValue(this, val, null);
                    }
                        
                }
            }
            return true;
        }

        public bool SaveConfig()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(m_ConfigurationPath);
            XmlNode conNodes = doc.SelectSingleNode("Configuration");
            XmlNodeList nodes = conNodes.ChildNodes;
            foreach (XmlNode node in nodes)
            {
                string key = node.Name;
                PropertyInfo prop = this.GetType().GetProperty(key);
                if (prop != null)
                {
                    var val = prop.GetValue(this, null);
                    if (val != null)
                        node.InnerText = val.ToString();
                    else
                        node.InnerText = String.Empty;
                }
            }
            doc.Save(m_ConfigurationPath);
            return true;
        }
    }
}
