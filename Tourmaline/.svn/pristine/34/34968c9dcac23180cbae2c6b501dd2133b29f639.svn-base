using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using Shkila.ScaleReaders;
using System.Windows.Media;

namespace TourmalineUI
{
    class ViewModel : INotifyPropertyChanged
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

        private StringBuilder m_log = new StringBuilder();
        public string Log
        {
            get { return m_log.ToString(); }
            set
            {
                m_log.Append(value);
                NotifyPropertyChanged("Log");
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

        private Language m_language = Language.HE;
        public Language Language
        {
            get { return m_language; }
            set
            {
                m_language = value;
                Dictionary.Lang = m_language;
                Dictionary = Dictionary.Instance;
                NotifyPropertyChanged("Language");
            }
        }
        private ObservableCollection<String> m_Coms = new ObservableCollection<string>(SerialPort.GetPortNames());
        public ObservableCollection<String> Coms
        {
            get { return m_Coms; }
            set
            {
                m_Coms = value;
                NotifyPropertyChanged("Coms");
            }
        }

        private ObservableCollection<String> m_Scales = new ObservableCollection<string>(Enum.GetNames(typeof(ScaleHeaders)));
        public ObservableCollection<String> Scales
        {
            get { return m_Scales; }
            set
            {
                m_Scales = value;
                NotifyPropertyChanged("Scales");
            }
        }

        private ObservableCollection<String> m_Manufacturers = new ObservableCollection<string>();
        public ObservableCollection<String> Manufacturers
        {
            get { return m_Manufacturers; }
            set
            {
                m_Manufacturers = value;
                NotifyPropertyChanged("Manufacturers");
            }
        }

        private ObservableCollection<String> m_Models = new ObservableCollection<string>();
        public ObservableCollection<String> Models
        {
            get { return m_Models; }
            set
            {
                m_Models = value;
                NotifyPropertyChanged("Models");
            }
        }

        private String m_Port = String.Empty;
        public String Port
        {
            get { return m_Port; }
            set
            {
                m_Port = value;
                NotifyPropertyChanged("Port");
            }
        }

        private String m_SelectedModel = String.Empty;
        public String SelectedModel
        {
            get { return m_SelectedModel; }
            set
            {
                m_SelectedModel = value;
                NotifyPropertyChanged("SelectedModel");
            }
        }

        private String m_SelectedManufacturer = String.Empty;
        public String SelectedManufacturer
        {
            get { return m_SelectedManufacturer; }
            set
            {
                m_SelectedManufacturer = value;
                NotifyPropertyChanged("SelectedManufacturer");
            }
        }

        private String m_Service = String.Empty;
        public String Service
        {
            get { return m_Service; }
            set
            {
                m_Service = value;
                NotifyPropertyChanged("Service");
            }
        }

        private Brush m_ServiceColor = null;
        public Brush ServiceColor
        {
            get { return m_ServiceColor; }
            set
            {
                m_ServiceColor = value;
                NotifyPropertyChanged("ServiceColor");
            }
        }

        private Brush m_LicenseColor = Brushes.Red;
        public Brush LicenseColor
        {
            get { return m_LicenseColor; }
            set
            {
                m_LicenseColor = value;
                NotifyPropertyChanged("LicenseColor");
            }
        }

        private String m_License = String.Empty;
        public String License
        {
            get { return m_License; }
            set
            {
                m_License = value;
                NotifyPropertyChanged("License");
            }
        }

        private String m_Key = String.Empty;
        public String Key
        {
            get { return m_Key; }
            set
            {
                m_Key = value;
                NotifyPropertyChanged("Key");
            }
        }

        private String m_Weight = String.Empty;
        public String Weight
        {
            get { return m_Weight; }
            set
            {
                m_Weight = value;
                NotifyPropertyChanged("Weight");
            }
        }

        private String m_SelectedCom = String.Empty;
        public String SelectedCom
        {
            get { return m_SelectedCom; }
            set
            {
                m_SelectedCom = value;
                NotifyPropertyChanged("SelectedCom");
            }
        }

        private Brush m = Brushes.Purple;
        public Brush M
        {
            get { return m; }
            set
            {
                m = value;
                NotifyPropertyChanged("M");
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
