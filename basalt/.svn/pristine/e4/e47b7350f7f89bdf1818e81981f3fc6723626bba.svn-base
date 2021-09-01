using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shkila.ScaleReaders;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Windows.Media;
using System.Windows.Forms;
namespace Basalt_v2
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Dictionary m_Dictionary = Dictionary.Instance;
        public Dictionary Dictionary
        {
            get { return m_Dictionary; }
            set
            {
                m_Dictionary = value;
                NotifyPropertyChanged("Dictionary");
            }
        }

        private Language m_language = Language.HE;
        public Language Language
        {
            get { return m_language; }
            set { 
                m_language = value;
                Dictionary.Lang = m_language;
                Dictionary = Dictionary.Instance;
                NotifyPropertyChanged("Language");
            }
        }

        private ObservableCollection<Language> m_Languages = null;
        public ObservableCollection<Language> Languages
        {
            get { return m_Languages; }
            set
            {
                m_Languages = value;
                NotifyPropertyChanged("Languages");
            }
        }

        private string m_ActivationKey;
        public string ActivationKey
        {
            get { return m_ActivationKey; }
            set
            {
                m_ActivationKey = value;
                NotifyPropertyChanged("ActivationKey");
            }
        }

        private bool m_IsUpButtonsAnim;
        public bool IsUpButtonsAnim
        {
            get { return m_IsUpButtonsAnim; }
            set
            {
                m_IsUpButtonsAnim = value;
                NotifyPropertyChanged("IsUpButtonsAnim");
            }
        }


        private bool m_IsDownButtonsAnim;
        public bool IsDownButtonsAnim
        {
            get { return m_IsDownButtonsAnim; }
            set
            {
                m_IsDownButtonsAnim = value;
                NotifyPropertyChanged("IsDownButtonsAnim");
            }
        }

        private ObservableCollection<string> m_Keys = null;
        public ObservableCollection<string> Keys
        {
            get { return m_Keys; }
            set
            {
                m_Keys = value;
                NotifyPropertyChanged("Keys");
            }
        }

        private ObservableCollection<sp_GetScales_Result> m_Scales = null;
        public ObservableCollection<sp_GetScales_Result> Scales
        {
            get { return m_Scales; }
            set
            {
                m_Scales = value;
                NotifyPropertyChanged("Scales");
            }
        }

        private string m_FinishChar;
        public string FinishChar
        {
            get { return m_FinishChar; }
            set
            {
                m_FinishChar = value;
                NotifyPropertyChanged("FinishChar");
            }
        }

        private Brush m_mainColor = Brushes.Blue;
        public Brush MainColor
        {
            get { return m_mainColor; }
            set
            {
                m_mainColor = value;
                NotifyPropertyChanged("MainColor");
            }
        }

        private string m_Error;
        public string Error
        {
            get { return m_Error; }
            set
            {
                m_Error = value;
                NotifyPropertyChanged("Error");
            }
        }

        private ConnectionArgs m_ConenctionArgs = null;
        public ConnectionArgs ConnectionArgs
        {
            get { return m_ConenctionArgs; }
            set
            {
                m_ConenctionArgs = value;
                NotifyPropertyChanged("ConnectionArgs");
            }
        }

        private string m_Company = null;
        public string Company
        {
            get { return m_Company; }
            set
            {
                m_Company = value;
                NotifyPropertyChanged("Company");
            }
        }

        private string m_LogFilePath = null;
        public string LogFilePath
        {
            get { return m_LogFilePath; }
            set
            {
                m_LogFilePath = value;
                NotifyPropertyChanged("LogFilePath");
            }
        }

        
        private float m_Multiple = 1;
        public float Multiple
        {
            get { return m_Multiple; }
            set
            {
                m_Multiple = value;
                NotifyPropertyChanged("Multiple");
            }
        }

        private bool m_IsSerial = true;
        public bool IsSerial
        {
            get { return m_IsSerial; }
            set
            {
                m_IsSerial = value;
                NotifyPropertyChanged("IsSerial");
            }
        }

        private ScaleHeaders m_ScaleHeader;
        public ScaleHeaders ScaleHeader
        {
            get { return m_ScaleHeader; }
            set
            {
                m_ScaleHeader = value;
                NotifyPropertyChanged("ScaleHeader");
            }
        }

        private ObservableCollection<ConnectionType> m_ConnectionType = null;
        public ObservableCollection<ConnectionType> ConnectionType
        {
            get { return m_ConnectionType; }
            set
            {
                m_ConnectionType = value;
                NotifyPropertyChanged("ConnectionType");
            }
        }

        private ObservableCollection<ScaleHeaders> m_ScaleHeaders = null;
        public ObservableCollection<ScaleHeaders> ScaleHeaders
        {
            get { return m_ScaleHeaders; }
            set
            {
                m_ScaleHeaders = value;
                NotifyPropertyChanged("ScaleHeaders");
            }
        }

        private ObservableCollection<String> m_ComPorts = new ObservableCollection<string>(SerialPort.GetPortNames());
        public ObservableCollection<String> ComPorts
        {
            get { return m_ComPorts; }
            set
            {
                m_ComPorts = value;
                NotifyPropertyChanged("ComPorts");
            }
        }

        private ObservableCollection<Parity> m_Parity = null;
        public ObservableCollection<Parity> Parity
        {
            get { return m_Parity; }
            set
            {
                m_Parity = value;
                NotifyPropertyChanged("Parity");
            }
        }

        private ObservableCollection<StopBits> m_StopBits = null;
        public ObservableCollection<StopBits> StopBits
        {
            get { return m_StopBits; }
            set
            {
                m_StopBits = value;
                NotifyPropertyChanged("StopBits");
            }
        }

        private ObservableCollection<int> m_BaudRates = null;
        public ObservableCollection<int> BaudRates
        {
            get { return m_BaudRates; }
            set
            {
                m_BaudRates = value;
                NotifyPropertyChanged("BaudRates");
            }
        }

        private string m_weight = String.Empty;
        public string Weight
        {
            get { return m_weight; }
            set
            {
                m_weight = value;
                NotifyPropertyChanged("Weight");
            }
        }

        private Brush m_ColorStable = Brushes.Gray;
        public Brush ColorStable
        {
            get { return m_ColorStable; }
            set
            {
                m_ColorStable = value;
                NotifyPropertyChanged("ColorStable");
            }
        }

        private Brush m_ColorUnder = Brushes.Gray;
        public Brush ColorUnder
        {
            get { return m_ColorUnder; }
            set
            {
                m_ColorUnder = value;
                NotifyPropertyChanged("ColorUnder");
            }
        }

        private Brush m_ColorOver = Brushes.Gray;
        public Brush ColorOver
        {
            get { return m_ColorOver; }
            set
            {
                m_ColorOver = value;
                NotifyPropertyChanged("ColorOver");
            }
        }

        private Brush m_ColorGross = Brushes.Gray;
        public Brush ColorGross
        {
            get { return m_ColorGross; }
            set
            {
                m_ColorGross = value;
                NotifyPropertyChanged("ColorGross");
            }
        }

        private string m_Key;
        public string Key
        {
            get { return m_Key; }
            set
            {
                m_Key = value;
                NotifyPropertyChanged("Key");
            }
        }
        #region
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}
