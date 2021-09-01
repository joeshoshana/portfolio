using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basalt_v2
{
    public enum Language
    {
        HE,
        EN
    }

    public class Dictionary
    {
        private static Dictionary m_instance = null;
        private static object m_lock = new object();
        public static Language Lang { get; set; }
        private Dictionary() { }

        public static Dictionary Instance
        {
            get
            {
                lock(m_lock)
                {
                    if (m_instance == null)
                        m_instance = new Dictionary();
                }

                return m_instance;
            }
        }

        public static string BaudRate
        {
            get
            {
                switch(Lang)
                {
                    case Language.HE:
                        return "קצב";
                    case Language.EN:
                        return "Baud Rate";
                    default: return String.Empty;
                }
            }
        }

        public static string Browse
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "חפש";
                    case Language.EN:
                        return "Browse";
                    default: return String.Empty;
                }
            }
        }


        public static string LogFilePath
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "ניתוב קובץ";
                    case Language.EN:
                        return "File Path";
                    default: return String.Empty;
                }
            }
        }

        public static string ConnectionType
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "סוג חיבור";
                    case Language.EN:
                        return "Connection Type";
                    default: return String.Empty;
                }
            }
        }
        public static string HeadScale
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "ראש שקילה";
                    case Language.EN:
                        return "Scale Head";
                    default: return String.Empty;
                }
            }
        }
        public static string Langu
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
        public static string Multiple
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "מכפיל";
                    case Language.EN:
                        return "Multiple";
                    default: return String.Empty;
                }
            }
        }
        
        public static string Com
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "יציאה";
                    case Language.EN:
                        return "Com";
                    default: return String.Empty;
                }
            }
        }

        public static string Username
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "שם משתמש";
                    case Language.EN:
                        return "Username";
                    default: return String.Empty;
                }
            }
        }

        public static string Password
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "סיסמא";
                    case Language.EN:
                        return "Password";
                    default: return String.Empty;
                }
            }
        }

        public static string Company
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "חברה";
                    case Language.EN:
                        return "Company";
                    default: return String.Empty;
                }
            }
        }

        public static string Scales
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "מאזניים";
                    case Language.EN:
                        return "Scales";
                    default: return String.Empty;
                }
            }
        }

        public static string Parity
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "זוגיות";
                    case Language.EN:
                        return "Parity";
                    default: return String.Empty;
                }
            }
        }

        public static string StopBits
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "StopBits";
                    case Language.EN:
                        return "StopBits";
                    default: return String.Empty;
                }
            }
        }

        public static string DataBits
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "DataBits";
                    case Language.EN:
                        return "DataBits";
                    default: return String.Empty;
                }
            }
        }

        public static string RTS
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "RTS";
                    case Language.EN:
                        return "RTS";
                    default: return String.Empty;
                }
            }
        }

        public static string DTR
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "DTR";
                    case Language.EN:
                        return "DTR";
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

        public static string IP
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "כתובת";
                    case Language.EN:
                        return "IP";
                    default: return String.Empty;
                }
            }
        }

        public static string ActivationKey
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "מקש הפעלה";
                    case Language.EN:
                        return "Activation Key";
                    default: return String.Empty;
                }
            }
        }

        public static string FinishChar
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "תו סיום";
                    case Language.EN:
                        return "Finish Char";
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

        public static string ProgramAlreadyRunning
        {
            get
            {
                switch (Lang)
                {
                    case Language.HE:
                        return "תכנית רצה";
                    case Language.EN:
                        return "Program Already Running";
                    default: return String.Empty;
                }
            }
        }
    }
}
