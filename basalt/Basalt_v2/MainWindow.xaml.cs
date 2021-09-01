
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Shkila.LicenseManager;
using Shkila.ScaleReaders;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace Basalt_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Engine m_engine = Engine.Instance;
        
        private System.Windows.Forms.NotifyIcon m_notification = new System.Windows.Forms.NotifyIcon();
        public MainWindow()
        {
            try
            {
                string error = String.Empty;
               // if(!LicenseManager.LicenseProcees(ref error))
               // {
               //     MessageBox.Show(error);
               //     Close();
               // }

                Process p = Engine.IsAppOpen();
                if(p == null)
                {
                    InitializeComponent();
                    this.DataContext = m_engine.GetViewModel();
                    ViewModel vm = this.DataContext as ViewModel;
                    vm.Weight = "- - -";
                    InitNotifyIcon();
                    lblToggle.Content = Dictionary.Start;
                    if (m_engine.GetConfig().AutoStart)
                    {
                        ActivateProcess();
                    }
                }
                else
                {
                    MessageBox.Show(Dictionary.ProgramAlreadyRunning);
                    this.Close();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
        }

        private void InitNotifyIcon()
        {
            try
            {
                m_notification.Icon = new System.Drawing.Icon("./Images/ShkilaT.ico");
                m_notification.Visible = false;
                m_notification.MouseUp += delegate(object sender, System.Windows.Forms.MouseEventArgs e)
                {
                    if(e.Button == System.Windows.Forms.MouseButtons.Right)
                        if (!String.IsNullOrEmpty((this.DataContext as ViewModel).Weight))
                        {
                            m_notification.BalloonTipText = (this.DataContext as ViewModel).Weight;
                            m_notification.ShowBalloonTip(5);
                        }
                };
                m_notification.DoubleClick += delegate(object sender, EventArgs e)
                {
                    m_notification.Visible = false;
                    this.Show();
                    this.WindowState = System.Windows.WindowState.Normal;
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        protected override void OnStateChanged(EventArgs e)
        {
            try
            {
                if (WindowState == System.Windows.WindowState.Minimized)
                {
                    m_notification.Visible = true;
                    this.Hide();
                }

                base.OnStateChanged(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((sender as Border).Focusable)
                    (sender as Border).Focus();

                var elem = ((sender as Border).Parent as Grid);

                DoubleAnimation wAnim = null;
                if (elem.Width == elem.MinWidth)
                {
                    ViewModel vm = this.DataContext as ViewModel;
                    vm.Weight = "- - -";
                    m_engine.Stop();
                    lblToggle.Content = Dictionary.Start;
                    wAnim = new DoubleAnimation { From = elem.Width, To = elem.MaxWidth, Duration = TimeSpan.FromMilliseconds(500) };
                }
                else
                {
                    m_engine.SaveConfig();
                    m_engine.Start();
                    lblToggle.Content = Dictionary.Stop;
                    wAnim = new DoubleAnimation { From = elem.Width, To = elem.MinWidth, Duration = TimeSpan.FromMilliseconds(500) };
                }
                elem.BeginAnimation(FrameworkElement.WidthProperty, wAnim);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void cmbType_Changed(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBox cmb = sender as ComboBox;
                (this.DataContext as ViewModel).IsSerial = (ConnectionType)cmb.SelectedItem == ConnectionType.Serial;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
                
        }

        private void btnClose_Clicked(object sender, MouseButtonEventArgs e)
        {
            try
            {
                m_engine.Stop();
                m_engine.SaveConfig();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnMinimize_Clicked(object sender, MouseButtonEventArgs e)
        {
            try
            {
                WindowState = System.Windows.WindowState.Minimized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void imgLink_Clicked(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ViewModel vm = this.DataContext as ViewModel;
                if(vm.Language  == Basalt_v2.Language.HE)
                    System.Diagnostics.Process.Start("http://www.shkila.co.il");
                else if(vm.Language == Basalt_v2.Language.EN)
                    System.Diagnostics.Process.Start("http://www.shkila.com");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Toggler_Clicked(object sender, MouseButtonEventArgs e)
        {
            try
            {

                var elem = ((sender as Path).Parent as Grid);

                if (elem.Focusable)
                    elem.Focus();

                DoubleAnimation wAnim = null;
                if (elem.Width == elem.MinWidth)
                {
                    ViewModel vm = this.DataContext as ViewModel;
                    vm.Weight = "- - -";
                    m_engine.Stop();
                    lblToggle.Content = Dictionary.Start;
                    wAnim = new DoubleAnimation { From = elem.Width, To = elem.MaxWidth, Duration = TimeSpan.FromMilliseconds(500) };
                }
                else
                {
                    m_engine.SaveConfig();
                    m_engine.Start();
                    lblToggle.Content = Dictionary.Stop;
                    wAnim = new DoubleAnimation { From = elem.Width, To = elem.MinWidth, Duration = TimeSpan.FromMilliseconds(500) };
                }
                elem.BeginAnimation(FrameworkElement.WidthProperty, wAnim);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ActivateProcess()
        {
            Toggler_Clicked(Toggler, null);
            btnMinimize_Clicked(null,null);
            OnStateChanged(null);
        }
        private void cpMainColor_Changed(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            (this.DataContext as ViewModel).MainColor = new SolidColorBrush((sender as Xceed.Wpf.Toolkit.ColorPicker).SelectedColor.Value);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_engine.Stop();
        }

        private void keyDisplayClicked_Clicked(object sender, MouseButtonEventArgs e)
        {
            KeyWindow win = new KeyWindow();
            win.ShowDialog();
        }

        private void txtUsername_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void GetScales()
        {
            try
            {
                var m = (this.DataContext as ViewModel);
                m.ConnectionArgs.Password = password.Password.ToString();
                if (!String.IsNullOrEmpty(m.ConnectionArgs.Username) && !String.IsNullOrEmpty(m.ConnectionArgs.Password) && !String.IsNullOrEmpty(m.Company))
                {
                    if ((m.ConnectionArgs.CompanyID = m_engine.IsCompanyExists(m.Company)) != -1)
                    {
                        if (m_engine.CheckUser(m.ConnectionArgs))
                        {
                            m.Scales = new System.Collections.ObjectModel.ObservableCollection<sp_GetScales_Result>(m_engine.GetScales(m.ConnectionArgs));
                            if (m.Scales.Count == 1)
                            {
                                m.ConnectionArgs.ScaleID = m.Scales.ElementAt(0).GUID;
                                m.ConnectionArgs = m.ConnectionArgs;
                            }
                            return;
                        }
                    }
                }
                if (m.Scales != null)
                    m.Scales.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException. Message);
            }
            
        }

        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void txtCompany_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void btnGetScales_clicked(object sender, RoutedEventArgs e)
        {
            GetScales();
        }

        private void btnBrowse_clicked(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var m = (this.DataContext as ViewModel);
                    m.LogFilePath = System.IO.Path.Combine(dialog.SelectedPath, "log.txt");
                }
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {            
            
            var m = (this.DataContext as ViewModel);            
            Point pointToWindow = Mouse.GetPosition(this);

            if (pointToWindow.X <= 150 )
            {
                if (pointToWindow.Y <= 200)
                {
                    m.IsUpButtonsAnim = true;
                    m.IsDownButtonsAnim = false;
                }
                else if (pointToWindow.Y >= 320)
                {
                    m.IsUpButtonsAnim = false;
                    m.IsDownButtonsAnim = true;
                }
                else
                {
                    m.IsUpButtonsAnim = false;
                    m.IsDownButtonsAnim = false;
                }
            }
            else
            {
                m.IsUpButtonsAnim = false;
                m.IsDownButtonsAnim = false;
            } 
        }
    }
}
 