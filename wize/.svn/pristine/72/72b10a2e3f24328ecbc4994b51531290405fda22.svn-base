using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shkila.Model.Handlers
{
    public class DictionaryHandler
    {
        public void SetText(string phrase, string text)
        {
            try
            {
                typeof(DictionaryHandler).GetProperty(phrase).SetValue(this, text, null);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void LoadText(Language lang)
        {
            try
            {
                using(WanagerDBEntities db = new WanagerDBEntities())
                {
                    var dictionary = db.Dictionary;
                    foreach(Dictionary dic in dictionary)
                        switch(lang)
                        {
                            case  Language.HE:
                                SetText(dic.Phrase, dic.HE);
                                break;
                            case Language.EN:
                                SetText(dic.Phrase, dic.EN);
                                break;
                        }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string GetText(string phrase)
        {
            try
            {
                return typeof(DictionaryHandler).GetProperty(phrase).GetValue(this, null).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetText(string phrase,Language lang)
        {
            try
            {
                using (WanagerDBEntities db = new WanagerDBEntities())
                {
                    var dictionary = db.Dictionary.Where( i => i.Phrase.ToLower().Equals(phrase.ToLower())).FirstOrDefault();
                    if(dictionary != null)
                        switch (lang)
                        {
                            case Language.HE:
                                return dictionary.HE;
                            case Language.EN:
                                return dictionary.EN;
                            default: return null;
                        }

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UserSettings { get; set; }
        public string Scales { get; set; }
        public string CompanySettings	 { get; set; }
        public string Disconnected	 { get; set; }
        public string Users	 { get; set; }
        public string Customers	 { get; set; }
        public string Suppliers	 { get; set; }
        public string Items	 { get; set; }
        public string Permissions	 { get; set; }
        public string Vehicles	 { get; set; }
        public string Sites	 { get; set; }
        public string Companies	 { get; set; }
        public string Tranports { get; set; }
        public string Settings { get; set; }
        public string Tables { get; set; }
        public string Forms { get; set; }
        public string Hello { get; set; }
        public string MissingVehicle { get; set; }
        public string Filters	{ get; set; }
        public string CompanyName	{ get; set; }
        public string Address	{ get; set; }
        public string ID	{ get; set; }
        public string ActiveOnly	{ get; set; }
        public string Active	{ get; set; }
        public string CompanyEdit	{ get; set; }
        public string TableName	{ get; set; }
        public string Attach	{ get; set; }
        public string FormName	{ get; set; }
        public string Status	{ get; set; }
        public string ScaleType	{ get; set; }
        public string Weight	{ get; set; }
        public string WeightDate { get; set; }
        public string UpperTitle { get; set; }
        public string Logo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEdit { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Pelephone { get; set; }
        public string Remarks { get; set; }
        public string ContactEdit { get; set; }
        public string ItemName { get; set; }
        public string SerialNumber { get; set; }
        public string ItemEdit { get; set; }
        public string SystemConnection { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Connect { get; set; }
        public string ScaleName { get; set; }
        public string ConnectedOnly { get; set; }
        public string Check { get; set; }
        public string ScaleEdit { get; set; }
        public string SiloView { get; set; }
        public string SiteName { get; set; }
        public string SiloName { get; set; }
        public string SiloEdit { get; set; }
        public string MaximumCapacity { get; set; }
        public string LoadDetection { get; set; }
        public string UnloadDetection { get; set; }
        public string LoadInterval { get; set; }
        public string LoadIntervalTime { get; set; }
        public string UnloadInterval { get; set; }
        public string UnloadIntervalTime { get; set; }
        public string SiteEdit { get; set; }
        public string SupplierName { get; set; }
        public string SupplierEdit { get; set; }
        public string TransportName { get; set; }
        public string TransportEdit { get; set; }
        public string FromBirthDate { get; set; }
        public string ToBirthDate { get; set; }
        public string BirthDate { get; set; }
        public string UserEdit { get; set; }
        public string LicenseNumber { get; set; }
        public string Tare { get; set; }
        public string VehiclesEdit { get; set; }
        public string EmptyTable { get; set; }
        public string Info { get; set; }
        public string InfoEmpty { get; set; }
        public string InfoFiltered { get; set; }
        public string LengthMenu { get; set; }
        public string LoadingRecords { get; set; }
        public string Processing { get; set; }
        public string Search { get; set; }
        public string ZeroRecords { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public string Saved { get; set; }
        public string AddLogo { get; set; }
        public string SelectScales { get; set; }
        public string FillLoadData { get; set; }
        public string FillUnloadData { get; set; }
        public string LoadStart { get; set; }
        public string UnloadStart { get; set; }
        public string InActive { get; set; }
        public string MissingPhone { get; set; }
        public string MissingEmail { get; set; }
        public string SendTo { get; set; }
        public string Sent { get; set; }
        public string Selectitem { get; set; }
        public string SelectTransport { get; set; }
        public string CustomerNotExist { get; set; }
        public string InvalidWeight { get; set; }
        public string Save { get; set; }
        public string ToYard { get; set; }
        public string SelectSite { get; set; }
        public string SelectVehicle { get; set; }
        public string SelectCustomer { get; set; }
        public string CertificateNumber{ get; set; }
        public string SourceSite{ get; set; }
        public string DestinationSite{ get; set; }
        public string FromWeightDate{ get; set; }
        public string ToWeightDate{ get; set; }
        public string CancelledOnly{ get; set; }
        public string ManualOnly{ get; set; }
        public string InDate{ get; set; }
        public string OutDate{ get; set; }
        public string InWeight{ get; set; }
        public string OutWeight{ get; set; }
        public string Netto{ get; set; }
        public string IsManual { get; set; }
        public string Cancelled{ get; set; }
        public string CertificateID{ get; set; }
        public string SendByMail{ get; set; }
        public string SendBySMS{ get; set; }
        public string CertificateEdit{ get; set; }
        public string Contacts{ get; set; }
        public string DualWeights{ get; set; }
        public string InAndTare{ get; set; }
        public string TareAndOut{ get; set; }
        public string SingleWeight{ get; set; }
        public string Automatic{ get; set; }
        public string Weigh{ get; set; }
        public string SaveAndPrint{ get; set; }
        public string Yard{ get; set; }
        public string UpdateTare{ get; set; }
        public string Certificates{ get; set; }
        public string Reports{ get; set; }
        public string Print { get; set; }
        public string CompanyNotExist { get; set; }
        public string ContactNotExist { get; set; }
        public string CertificateNotExist { get; set; }
        public string FillUsername{ get; set; }
        public string FillPassword{ get; set; }
        public string FillCompany{ get; set; }
        public string InactiveComapny{ get; set; }
        public string UserNotFound{ get; set; }
        public string VehicleNotExist { get; set; }
        public string WeightNotFound { get; set; }
        public string MissingCustomer { get; set; }
        public string MissingFirstName { get; set; }
        public string MissingLastName { get; set; }
        public string InvalidEmail { get; set; }
        public string MissingSupplier { get; set; }
        public string InvalidID { get; set; }
        public string InvalidScale { get; set; }
        public string InvalidVehicle { get; set; }
        public string InvalidItem { get; set; }
        public string InvalidTransport { get; set; }
        public string InvalidInWeight { get; set; }
        public string InvalidOutWeight { get; set; }
        public string InvalidSourceSite { get; set; }
        public string InvalidDestinationSite { get; set; }
        public string MissingCertificate { get; set; }
        public string MissingUser { get; set; }
        public string InvalidCompany { get; set; }
        public string InvalidForm { get; set; }
        public string MissingID { get; set; }
        public string MissingName { get; set; }
        public string MissingAddress { get; set; }
        public string InvalidTable { get; set; }
        public string InvalidPhone { get; set; }
        public string MissingLink { get; set; }
        public string MissingSerialNumber { get; set; }
        public string MissingMAC { get; set; }
        public string InvalidMaxCapacity { get; set; }
        public string MissingUsername { get; set; }
        public string MissingPassword { get; set; }
        public string InvalidTare { get; set; }
        public string MissingLicenseNumber { get; set; }
        public string InvalidWeighingMode { get; set; }
        public string Lang{ get; set; }
        public string SelectLang { get; set; }
        public string Field { get; set; }
        public string InvalidCertificate { get; set; }
        public string Date { get; set; }
        public string MG { get; set; }
        public string G { get; set; }
        public string KG { get; set; }
        public string Unit { get; set; }
        public string SelectUnit { get; set; }
        public string InvalidFormsField { get; set; }
        public string FormsFieldsEdit { get; set; }
        public string ValidationRequired { get; set; }
        public string IsShowing { get; set; }
        public string FieldName { get; set; }
        public string Fields { get; set; }
        public string MissingSilo{ get; set; }
        public string MissingSite { get; set; }
        public string PermissionsNotExist { get; set; }
        public string MissingPermission{ get; set; }
        public string MissingFormPermission{ get; set; }
        public string MissingTablePermission{ get; set; }
        public string PermissionName{ get; set; }
        public string PermissionEdit { get; set; }
        public string ManageWizeSilo{ get; set; }
        public string ManageWizeBridge{ get; set; }
        public string ManageUsers{ get; set; }
        public string ManageCustomers{ get; set; }
        public string WIZEGoods{ get; set; }
        public string WIZEBridge{ get; set; }
        public string WIZESilo { get; set; }
        public string ManageSuppliers{ get; set; }
        public string ManageItems{ get; set; }
        public string ManagePermissions{ get; set; }
        public string ManageVehicle{ get; set; }
        public string ManageSites{ get; set; }
        public string ManageTransports { get; set; }
        public string SelectForm { get; set; }
        public string SelectPermission { get; set; }
        public string LogWeight { get; set; }
        public string LogWeightTime { get; set; }
        public string FillLogWeightData { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Download { get; set; }
        public string SystemSettings { get; set; }
        public string IsOwner { get; set; }
        public string OwnerOnly { get; set; }
        public string InvalidOwner { get; set; }
        public string SelectCompany { get; set; }
        public string IsDemo { get; set; }
        public string AllowedRows { get; set; }
        public string InvalidAllowedRows { get; set; }
        public string Limit { get; set; }
        public string CompaniesLimit { get; set; }
        public string InvalidCustomer { get; set; }
        public string Drivers { get; set; }
        public string DriverName { get; set; }
        public string DriverEdit { get; set; }
        public string SelectDriver { get; set; }
        public string InvalidDriver { get; set; }
        public string Tag { get; set; }
        public string ConnectionEdit { get; set; }
        public string Connections { get; set; }
        public string Duplicate { get; set; }
        public string Delete { get; set; }
        public string Q_ConnectionFound_Display { get; set; }
        public string InvalidTag { get; set; }
        public string InvalidConnection { get; set; }
        public string New { get; set; }
        public string Back { get; set; }
        public string Cancel { get; set; }
        public string CustomerCode { get; set; }
        public string DriverCode { get; set; }
        public string TimeToResetUnfinishedWeights { get; set; }
        public string Minute { get; set; }
        public string Hour { get; set; }
        public string Read { get; set; }
        public string Write { get; set; }
        public string Manager { get; set; }
        public string Reference { get; set; }
        public string MissingReference { get; set; }
        public string InSiteName { get; set; }
        public string OutSiteName { get; set; }
        public string EvacuationReport { get; set; }
        public string SelectUser { get; set; }
        public string Appearances { get; set; }
        public string MissingRemarks { get; set; }
        public string WeightsReport { get; set; }
        public string CreatePassword { get; set; }
        public string MissingInnerPermission { get; set; }
        public string InnerPermissions { get; set; }
        public string MaxObligo { get; set; }
        public string ObligoLeft { get; set; }
        public string MonthlyObligo	 { get; set; }
        public string ObligoWeighings	 { get; set; }
        public string AddObligo	 { get; set; }
        public string ClearObligo	 { get; set; }
        public string ObligoDeletedMsg	 { get; set; }
        public string ObligoAddedMsg	 { get; set; }
        public string AddObligoTitle	 { get; set; }
        public string ExtraObligo	 { get; set; }
        public string Add	 { get; set; }
        public string ApproveDeleteObligoMsg { get; set; }
        public string WeighingMode { get; set; }
        public string VIP { get; set; }
        public string SendWeightsByMail { get; set; }
        public string SendingMethod { get; set; }
        public string InvalidSendingMethod { get; set; }
        public string InvalidCompaniesLimit { get; set; }
        
    }
}
