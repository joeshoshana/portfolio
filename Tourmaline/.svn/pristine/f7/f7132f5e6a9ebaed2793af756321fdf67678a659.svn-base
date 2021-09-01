using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourmalineUI
{
    public enum Language
    {
        HE,
        EN
    }

    class Dictionary
    {
        private static Dictionary m_instance = null;
        private static object m_lock = new object();
        public static Language Lang { get; set; }
        private Dictionary() { }

        public static Dictionary Instance
        {
            get
            {
                lock (m_lock)
                {
                    if (m_instance == null)
                        m_instance = new Dictionary();
                }

                return m_instance;
            }
        }

        public static string Serial
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "יציאה סיריאלית";
                    case Language.EN:
                        return "Com";
                    default: return String.Empty;
                }
            }
        }

        public static string Exit
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "יציאה";
                    case Language.EN:
                        return "Exit";
                    default: return String.Empty;
                }
            }
        }

        public static string Port
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "פורט";
                    case Language.EN:
                        return "Port";
                    default: return String.Empty;
                }
            }
        }

        public static string Start
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "התחל";
                    case Language.EN:
                        return "Start";
                    default: return String.Empty;
                }
            }
        }

        public static string Stop
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "הפסק";
                    case Language.EN:
                        return "Stop";
                    default: return String.Empty;
                }
            }
        }

        public static string Install
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "התקן";
                    case Language.EN:
                        return "Install";
                    default: return String.Empty;
                }
            }
        }

        public static string Uninstall
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "הסר";
                    case Language.EN:
                        return "Uninstall";
                    default: return String.Empty;
                }
            }
        }

        public static string Test
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "בדיקה";
                    case Language.EN:
                        return "Test";
                    default: return String.Empty;
                }
            }
        }

        public static string Save
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "שמירה";
                    case Language.EN:
                        return "Save";
                    default: return String.Empty;
                }
            }
        }

        public static string Model
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "דגם";
                    case Language.EN:
                        return "Model";
                    default: return String.Empty;
                }
            }
        }

        public static string Manufacturer
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "יצרן";
                    case Language.EN:
                        return "Manufacturer";
                    default: return String.Empty;
                }
            }
        }

        public static string ServiceStatus
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "סטטוס שירות";
                    case Language.EN:
                        return "Service Status";
                    default: return String.Empty;
                }
            }
        }

        public static string LicenseStatus
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "תוקף רשיון";
                    case Language.EN:
                        return "License Validity";
                    default: return String.Empty;
                }
            }
        }

        public static string Software
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "תוכנה";
                    case Language.EN:
                        return "Software";
                    default: return String.Empty;
                }
            }
        }

        public static string Installer
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "התקנה";
                    case Language.EN:
                        return "Installer";
                    default: return String.Empty;
                }
            }
        }

        public static string Service
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "שירות";
                    case Language.EN:
                        return "Service";
                    default: return String.Empty;
                }
            }
        }

        public static string Valid
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "בתוקף";
                    case Language.EN:
                        return "Valid";
                    default: return String.Empty;
                }
            }
        }

        public static string Invalid
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "לא בתוקף";
                    case Language.EN:
                        return "Invalid";
                    default: return String.Empty;
                }
            }
        }

        public static string Started
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "פועל";
                    case Language.EN:
                        return "Active";
                    default: return String.Empty;
                }
            }
        }

        public static string Stopped
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "לא פועל";
                    case Language.EN:
                        return "Inactive";
                    default: return String.Empty;
                }
            }
        }

        public static string NotInstalled
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "לא מותקן";
                    case Language.EN:
                        return "Not Installed";
                    default: return String.Empty;
                }
            }
        }

        public static string Load
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "טען";
                    case Language.EN:
                        return "Load";
                    default: return String.Empty;
                }
            }
        }

        public static string CantsaveMissingManufOrModel
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "לא ניתן לשמור, מכיוון שחסר יצרן ו/או מודל";
                    case Language.EN:
                        return "Cannot save, missing Manufacturer or model";
                    default: return String.Empty;
                }
            }
        }

        public static string ChangesSaved
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "שינויים נשמרו";
                    case Language.EN:
                        return "Changes Saved";
                    default: return String.Empty;
                }
            }
        }

        public static string RestartForChanges
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "יש להפסיק את השירות ולהתחיל מחדש על מנת שהשינויים יכנסו לתוקף";
                    case Language.EN:
                        return "Please Restart The service for the changes to take effect";
                    default: return String.Empty;
                }
            }
        }

        public static string ServiceExist
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "שירות קיים";
                    case Language.EN:
                        return "Service Exist";
                    default: return String.Empty;
                }
            }
        }

        public static string ServiceNotExist
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "שירות לא קיים";
                    case Language.EN:
                        return "Service Not Exist";
                    default: return String.Empty;
                }
            }
        }

        public static string InstallService
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "מתקין שירות";
                    case Language.EN:
                        return "Install Service";
                    default: return String.Empty;
                }
            }
        }

        public static string ServiceInstalled
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "שירות הותקן";
                    case Language.EN:
                        return "Service Installed";
                    default: return String.Empty;
                }
            }
        }

        public static string RemoveService
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "מסיר שירות";
                    case Language.EN:
                        return "Remove Service";
                    default: return String.Empty;
                }
            }
        }

        public static string ServiceRemoved        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "שירות הוסר";
                    case Language.EN:
                        return "Service Removed";
                    default: return String.Empty;
                }
            }
        }

        public static string StartService
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "מתחיל שירות";
                    case Language.EN:
                        return "Start Service";
                    default: return String.Empty;
                }
            }
        }

        public static string ServiceStarted
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "שירות התחיל";
                    case Language.EN:
                        return "Service Started";
                    default: return String.Empty;
                }
            }
        }

        public static string StopService
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "מפסיק שירות";
                    case Language.EN:
                        return "Stop Service";
                    default: return String.Empty;
                }
            }
        }

        public static string ServiceStopped
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "שירות הפסיק";
                    case Language.EN:
                        return "Service Stopped";
                    default: return String.Empty;
                }
            }
        }

     
        
        public static string SaveChanges
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "שומר נתונים";
                    case Language.EN:
                        return "Save Changes";
                    default: return String.Empty;
                }
            }
        }

        public static string Langua
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "שפה";
                    case Language.EN:
                        return "Language";
                    default: return String.Empty;
                }
            }
        }

        
    }
}
