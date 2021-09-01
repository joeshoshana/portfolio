using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Shkila.LicenseManager;

namespace LicenseCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cmbProducts.ItemsSource = Enum.GetValues(typeof(Products)).Cast<Products>();
            cmbProducts.SelectedIndex = 0;
            cmbLicenseTypes.ItemsSource = Enum.GetValues(typeof(LicenseType)).Cast<LicenseType>();
            cmbLicenseTypes.SelectedIndex = 0;
            dpStartDate.SelectedDate = DateTime.Now;
            txtPeriod.Text = (7).ToString();
        }

        private void btnCreateKey_Clicked(object sender, RoutedEventArgs e)
        {
            LicenseData dt = new LicenseData();
            dt.Period = Convert.ToInt32(txtPeriod.Text);
            dt.ProductID = (Products)cmbProducts.SelectedValue;
            dt.LicenseType = (LicenseType)cmbLicenseTypes.SelectedValue;
            dt.ToUpdate = (!cbUpdate.IsChecked.HasValue) ? false : (bool)cbUpdate.IsChecked.Value;
            dt.ToRegistry = (!cbRegistry.IsChecked.HasValue) ? false : (bool)cbRegistry.IsChecked.Value;
            dt.MacAddress = txtMacAddress.Text;
            txtKey.Text = LicenseManager.Create(dt);

        }

        private void btnSaveKey_Clicked(object sender, RoutedEventArgs e)
        {
            LicenseData dt = new LicenseData();
            dt.Period = Convert.ToInt32(txtPeriod.Text);
            dt.ProductID = (Products)cmbProducts.SelectedValue;
            dt.LicenseType = (LicenseType)cmbLicenseTypes.SelectedValue;
            dt.StartDate = Convert.ToDateTime(dpStartDate.Text);
            dt.ToUpdate = (!cbUpdate.IsChecked.HasValue) ? false : (bool)cbUpdate.IsChecked.Value;
            dt.ToRegistry = (!cbRegistry.IsChecked.HasValue) ? false : (bool)cbRegistry.IsChecked.Value;
            dt.MacAddress = txtMacAddress.Text;
            LicenseManager.LicenseFile(dt);
        }

        private void btnUploadKey_Clicked(object sender, RoutedEventArgs e)
        {
            string error = String.Empty;
            if (!LicenseManager.LicenseProcees(ref error))
                MessageBox.Show(error);
        }

        private void btnLoadFile_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "License files (*.lic)|*.lic";
            if (openFileDialog.ShowDialog() == true)
            {
                System.IO.File.Copy(openFileDialog.FileName, "Shkila.lic", true);

                LicenseData ld = LicenseManager.GetLicenseDataFromFile("6274411");
                txtPeriod.Text = ld.Period.ToString();
                cmbProducts.SelectedValue = ld.ProductID;
                cmbLicenseTypes.SelectedValue = ld.LicenseType;
                //dpStartDate.SelectedDate = ld.StartDate;
                cbUpdate.IsChecked = ld.ToUpdate;
                cbRegistry.IsChecked = ld.ToRegistry;
                txtMacAddress.Text = ld.MacAddress;

            }
                
        }

        private void btnLoadKey_Clicked(object sender, RoutedEventArgs e)
        {
            if(!String.IsNullOrEmpty(txtKey.Text))
            {
                LicenseData ld = LicenseManager.GetLicenseDataFromKey("6274411",txtKey.Text);
                txtPeriod.Text = ld.Period.ToString();
                cmbProducts.SelectedValue = ld.ProductID;
                cmbLicenseTypes.SelectedValue = ld.LicenseType;
                dpStartDate.SelectedDate = ld.StartDate;
                cbUpdate.IsChecked = ld.ToUpdate;
                cbRegistry.IsChecked = ld.ToRegistry;
                txtMacAddress.Text = ld.MacAddress;
            }

        }
    }
}
