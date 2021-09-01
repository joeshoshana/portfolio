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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TourmalineUI
{
    /// <summary>
    /// Interaction logic for KeyWindow.xaml
    /// </summary>
    public partial class KeyWindow : Window
    {
        private Engine m_engine = Engine.Instance;
        public KeyWindow()
        {
            InitializeComponent();
            this.DataContext = m_engine.GetViewModel();
            (this.DataContext as ViewModel).Key = m_engine.GetKey();
            txtKey.Focus();
            txtKey.SelectAll();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Clicked(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void btnLoad_Clicked(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtKey.Text))
                    MessageBox.Show("Empty Key");

                if (!m_engine.LoadKey())
                    MessageBox.Show("Invalid Key");

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 
    }
}
