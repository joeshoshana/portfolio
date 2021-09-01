using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Service_Windows
{
    public class Configuration
    {
        public string Weight { get; set; }
        public string Web { get; set; }
        public string COM { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string IsDemo { get; set; }
        public string m_error = "";
        public string Error { get { return m_error; } }


        private static volatile Configuration instance;
        private static object syncRoot = new Object();

        private Configuration() { }

        public static Configuration Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Configuration();
                    }
                }

                return instance;
            }
        }

        public bool LoadConfig(string xmlPath)
        {
            try
            {
                if (!File.Exists(xmlPath))
                {
                    m_error = "קובץ קונפיגורציה לא קיים";
                    return false;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath);
                XmlNodeList list = doc.SelectNodes("Configuration");

                if (list.Count <= 0)
                {
                    m_error = "בעיה בקונפיגורציה";
                    return false;
                }

                XmlNodeList children = list[0].ChildNodes;
                foreach (XmlNode child in children)
                {
                    string key = child.Name;
                    string value = child.InnerText;

                    if (!String.IsNullOrEmpty(value))
                    {
                        try
                        {
                            System.Reflection.PropertyInfo propertyInfo = this.GetType().GetProperty(key);
                            propertyInfo.SetValue(this, value, null);
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                m_error = ex.Message;
                return false;
            }
        }

        public bool SaveConfig(string xmlPath)
        {
            try
            {
                if (!File.Exists(xmlPath))
                {
                    m_error = "קובץ קונפיגורציה לא קיים";
                    return false;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath);

                XmlNodeList list = doc.SelectNodes("Configuration");

                if (list.Count <= 0)
                {
                    m_error = "בעיה בקונפיגורציה";
                    return false;
                }

                XmlNodeList children = list[0].ChildNodes;
                foreach (XmlNode child in children)
                {
                    string key = child.Name;
                    try
                    {
                        System.Reflection.PropertyInfo propertyInfo = this.GetType().GetProperty(key);
                        child.InnerText = propertyInfo.GetValue(this, null).ToString();
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }
                doc.Save(xmlPath);
                return true;
            }
            catch (Exception ex)
            {
                m_error = ex.Message;
                return false;
            }
        }
    }
}
