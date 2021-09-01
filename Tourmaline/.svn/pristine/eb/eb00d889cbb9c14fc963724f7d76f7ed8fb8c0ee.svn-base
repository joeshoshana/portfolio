using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TourmalineUI
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private Engine m_engine = Engine.Instance;
        public Main()
        {
            InitializeComponent();
            this.DataContext = m_engine.GetViewModel();
        }

        private void LogChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox).ScrollToEnd();
        }

        private void cmbManufacturer_Changed(object sender, SelectionChangedEventArgs e)
        {
             var manufacturer = (sender as ComboBox).SelectedItem as string;
             m_engine.SetModels(manufacturer);
        }

        private void txtPort_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Engine.IsTextAllowed(e.Text);
        }

        private void btnExit_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Clicked(object sender, RoutedEventArgs e)
        {
            m_engine.SaveConfig();
        }

        private void btnStart_Clicked(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(() =>
            {
                m_engine.StartService();
            });
            t.IsBackground = true;
            t.Start();            
        }

        private void btnStop_Clicked(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(() =>
            {
                m_engine.StopService();
            });
            t.IsBackground = true;
            t.Start();            
        }

        private void btnInstall_Clicked(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(() =>
            {
                m_engine.InstallService();
            });
            t.IsBackground = true;
            t.Start();            
        }

        private void btnUninstall_Clicked(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(() =>
            {
                m_engine.UninstallService();
            });
            t.IsBackground = true;
            t.Start();
            
        }

        private void btnTest_Clicked(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(() =>
            {
                m_engine.TestService();
            });
            t.IsBackground = true;
            t.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_engine.Dispose();
        }

        private void btnUpdateLicense_Clicked(object sender, RoutedEventArgs e)
        {
            KeyWindow win = new KeyWindow();
            win.ShowDialog();
        }
    }
}
