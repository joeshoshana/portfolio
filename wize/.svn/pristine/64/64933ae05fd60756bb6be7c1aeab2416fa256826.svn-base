using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using Shkila.Utilities;
using Shkila.Utilities.LPR;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace Shkila.Model.Handlers
{
    public class VehiclesWeighingHandler
    {
        const int MinTimeWeighingInterval = 1;

        public static void Save(sp_GetVehiclesWeighing_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveVehicleWeighing(item.GUID, item.CompanyID, item.ScaleID, item.VehicleID, item.ItemID, item.TransportID,
                    item.InWeight, item.OutWeight, item.Netto, item.InDate, item.OutDate, item.IsCancelled, item.InSiteID, item.OutSiteID,
                    item.Remarks, item.IsManual, item.CertificateID, item.UserID, item.CustomerID, item.WeighingMode, item.DriverID,
                    item.Tag, item.Reference).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetVehiclesWeighing_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.VehiclesWeighing.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                dt.IsCancelled = true;
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetVehiclesWeighing_Result item, ref List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "ScaleID":
                    if ((item.ScaleID == null || item.ScaleID <= 0) && item.IsManual == false)
                        errors.Add("InvalidScale");
                    break;
                case "CompanyID":
                    if (!item.CompanyID.HasValue || item.CompanyID.Value == 0)
                        errors.Add("InvalidCompany");
                    break;
                case "VehicleID":
                    if (!item.VehicleID.HasValue || item.VehicleID.Value <= 0)
                        errors.Add("InvalidVehicle");
                    break;
                case "ItemID":
                    if (!item.ItemID.HasValue || item.ItemID.Value <= 0)
                        errors.Add("InvalidItem");
                    break;
                case "TransportID":
                    if (!item.TransportID.HasValue || item.TransportID.Value <= 0)
                        errors.Add("InvalidTransport");
                    break;
                case "InWeight":
                    if (item.InWeight == null || item.InWeight < 0)
                        errors.Add("InvalidInWeight");
                    break;
                case "OutWeight":
                    if (item.GUID > 0 && (item.OutWeight == null || item.OutWeight <= 0))
                        errors.Add("InvalidOutWeight");
                    break;
                case "CustomerID":
                    if (!item.CustomerID.HasValue || item.CustomerID.Value <= 0)
                        errors.Add("InvalidCustomer");
                    break;
                case "DriverID":
                    if (!item.DriverID.HasValue || item.DriverID.Value <= 0)
                        errors.Add("InvalidDriver");
                    break;
                case "WeighingMode":
                    if (!item.WeighingMode.HasValue || item.CustomerID.Value <= 0)
                        errors.Add("InvalidWeighingMode");
                    break;
                case "Netto":
                    break;
                case "InDate":
                    break;
                case "OutDate":
                    break;
                case "InSiteID":
                    if (item.InSiteID == null || item.InSiteID <= 0)
                        errors.Add("InvalidSourceSite");
                    break;
                case "OutSiteID":
                    if (item.OutSiteID == null || item.OutSiteID <= 0)
                        errors.Add("InvalidDestinationSite");
                    break;
                case "IsCancelled":
                    break;
                case "Remarks":
                    if (String.IsNullOrEmpty(item.Remarks))
                        errors.Add("MissingRemarks");
                    break;
                case "CertificateID":
                    if (item.CertificateID == null)
                        errors.Add("MissingCertificate");
                    break;
                case "Reference":
                    if (String.IsNullOrEmpty(item.Reference))
                        errors.Add("MissingReference");
                    break;
                case "UserID":
                    if (item.UserID == null || item.UserID <= 0)
                        errors.Add("MissingUser");
                    break;

            }

            return errors.Count == 0;
        }

        public static long NextID(sp_GetVehiclesWeighing_Result data)
        {
            if (data != null)
            {
                long? id = Filter(new sp_GetVehiclesWeighing_Result { CompanyID = data.CompanyID }).Max(i => i.CertificateID);
                if (id == null)
                    return 1;
                else
                    return (long)id + 1;
            }

            return 1;
        }

        public static IEnumerable<sp_GetVehiclesWeighing_Result> Filter(sp_GetVehiclesWeighing_Result filter)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {

                var data = db.sp_GetVehiclesWeighing(filter.AllowedRows, filter.GUID, filter.CompanyID, filter.CertificateID, filter.UserID, filter.VehicleID,
                    filter.ScaleID, filter.TransportID, filter.ItemID, filter.InSiteID, filter.OutSiteID, filter.CustomerID, filter.WeighingMode, filter.IsCancelled,
                    filter.DriverID, filter.IsManual).ToList().AsEnumerable();

                if (filter.FromOutDate != null)
                    data = data.Where(i => i.OutDate != null && i.OutDate >= filter.FromOutDate);

                if (filter.ToOutDate != null)
                    data = data.Where(i => i.OutDate != null && i.OutDate < filter.ToOutDate.Value.AddDays(1));

                if (!String.IsNullOrEmpty(filter.Tag))
                    data = data.Where(i => i.Tag != null && i.Tag.Equals(filter.Tag));

                if (filter.Customers != null && filter.Customers.Count > 0 && filter.Customers[0] != 0)
                    data = data.Where(i => filter.Customers.Any(j => j == i.CustomerID));

                if (filter.Drivers != null && filter.Drivers.Count > 0 && filter.Drivers[0] != 0)
                    data = data.Where(i => filter.Drivers.Any(j => j == i.DriverID));

                if (filter.InSites != null && filter.InSites.Count > 0 && filter.InSites[0] != 0)
                    data = data.Where(i => filter.InSites.Any(j => j == i.InSiteID));

                if (filter.OutSites != null && filter.OutSites.Count > 0 && filter.OutSites[0] != 0)
                    data = data.Where(i => filter.OutSites.Any(j => j == i.OutSiteID));

                if (filter.Items != null && filter.Items.Count > 0 && filter.Items[0] != 0)
                    data = data.Where(i => filter.Items.Any(j => j == i.ItemID));

                if (filter.Transports != null && filter.Transports.Count > 0 && filter.Transports[0] != 0)
                    data = data.Where(i => filter.Transports.Any(j => j == i.TransportID));

                if (filter.Vehicles != null && filter.Vehicles.Count > 0 && filter.Vehicles[0] != 0)
                    data = data.Where(i => filter.Vehicles.Any(j => j == i.VehicleID));

                return data.ToList();
            }
        }

        public static LocalReport Report(sp_GetVehiclesWeighing_Result data)
        {
            string rptName = "VehiclesWeighing_en.rdlc";
            var company = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = data.CompanyID.Value }).FirstOrDefault();
            if (company != null && company.LanguageID.HasValue)
            {
                if ((Language)company.LanguageID.Value == Language.HE)
                    rptName = "VehiclesWeighing.rdlc";
            }

            string rptPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Reports/"), rptName);
            string mainPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
            if (data == null || data.GUID == 0)
                return null;

            sp_GetVehiclesWeighing_Result dt = Filter(new sp_GetVehiclesWeighing_Result { GUID = data.GUID }).FirstOrDefault();
            if (dt == null)
                return null;

            string title = String.Empty;
            string logo = String.Empty;
            if (company != null)
            {
                title = company.CertificateTitle;
                if (!String.IsNullOrEmpty(company.LogoPath))
                    logo = Path.Combine(mainPath, company.LogoPath.Remove(0, 1).Replace("/", @"\"));
            }
            string customerID = String.Empty;
            if (dt.CustomerID.HasValue)
            {
                sp_GetCustomers_Result customer = CustomersHandler.Filter(new sp_GetCustomers_Result { GUID = dt.CustomerID.Value, CompanyID = dt.CompanyID }).FirstOrDefault();
                if (customer != null && !String.IsNullOrEmpty(customer.ID))
                    customerID = customer.ID;
            }

            var lprImagesPath = GetLprPicPath(dt.GUID);
            List<Reports.ReportsEntity.VehiclesWeighingEntity> src = new List<Reports.ReportsEntity.VehiclesWeighingEntity>();
            Reports.ReportsEntity.VehiclesWeighingEntity temp = new Reports.ReportsEntity.VehiclesWeighingEntity
            {
                CertifiedID = dt.CertificateID.Value.ToString("D6"),
                InDate = dt.InDate.Value.ToString("dd/MM/yyyy HH:mm"),
                InSite = dt.InSiteName,
                InWeight = dt.InWeight.Value.ToString(),
                Item = dt.ItemName,
                Netto = dt.Netto.Value.ToString(),
                OutDate = dt.OutDate.Value.ToString("dd/MM/yyyy HH:mm"),
                OutSite = dt.OutSiteName,
                OutWeight = dt.OutWeight.Value.ToString(),
                Remarks = dt.Remarks,
                Supplier = dt.TransportName,
                Driver = dt.DriverName,
                User = dt.UserFirstName,
                Customer = dt.CustomerName,
                CustomerID = customerID,
                UpperTitle = title,
                Logo = String.IsNullOrEmpty(logo) ? null : "file:\\" + @logo,
                LicenseNumber = dt.LicenseNumber,
                LprImage1 = lprImagesPath[0],
                LprImage2 = lprImagesPath[1]
            };

            src.Add(temp);
            ReportDataSource ds = new ReportDataSource("VehiclesWeighingEntity", Utilities.ToDataTable<Reports.ReportsEntity.VehiclesWeighingEntity>(src));

            LocalReport rpt = new LocalReport();
            rpt.ReportPath = rptPath;
            rpt.EnableExternalImages = true;
            rpt.DataSources.Add(ds);

            return rpt;
        }

        private static string[] GetLprPicPath(long guid)
        {
            var result = new string[2];

            try
            {
                var path = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Images_LPR/" + DateTime.Now.ToString("yyyyMM") + "/");
                var taskDirectory = new DirectoryInfo(path);
                var taskFiles = taskDirectory.GetFiles("*.jpg").Where(p => p.Name.StartsWith(guid.ToString()));

                var path0 = path + taskFiles.First().Name;
                var absoluteUriImg0 = new System.Uri(path0).AbsoluteUri;
                result[0] = File.Exists(path0) ? absoluteUriImg0 : null;

                var path1 = path + taskFiles.Skip(1).First().Name;
                var absoluteUriImg1 = new System.Uri(path1).AbsoluteUri;
                result[1] = File.Exists(path1) ? absoluteUriImg1 : null;
            }
            catch
            { }

            return result;
        }

        public static string WeightFromTag(string tag, sp_GetScales_Result scale, LprData myLprData = null)
        {
            try
            {
                if (String.IsNullOrEmpty(tag))
                    return "InvalidTag";

                if (scale == null)
                    return "InvalidScale";

                decimal weight;
                if (!decimal.TryParse(scale.Weight, out weight))
                    return "InvalidWeight";

                var connection = ConnectionsHandler.Filter(new sp_GetConnections_Result { Tag = tag, CompanyID = scale.CompanyID }).Where(i => i.Tag.Equals(tag)).FirstOrDefault();                

                if (connection == null)
                    return "InvalidConnection";

                if (myLprData != null && myLprData.LicensePlateString != null) 
                    connection.VehicleID = GetVehicleID(myLprData.LicensePlateString, connection);

                var check = Filter(new sp_GetVehiclesWeighing_Result { Tag = tag }).OrderByDescending(i => i.GUID).FirstOrDefault();
                if (check != null)
                {
                    if (check.InDate.HasValue && DateTime.Now.Subtract(check.InDate.Value).TotalMinutes < MinTimeWeighingInterval)
                        return "InWeightTooSoon";
                    else if (check.OutDate.HasValue && DateTime.Now.Subtract(check.OutDate.Value).TotalMinutes < MinTimeWeighingInterval)
                        return "OutWeightTooSoon";
                }

                var record = Filter(new sp_GetVehiclesWeighing_Result
                {
                    CompanyID = connection.CompanyID,
                    CustomerID = connection.CustomerID,
                    DriverID = connection.DriverID,
                    InSiteID = connection.InSiteID,
                    ItemID = connection.ItemID,
                    OutSiteID = connection.OutSiteID,
                    TransportID = connection.TransportID,
                    VehicleID = connection.VehicleID,
                }).Where(i => i.OutDate == null).FirstOrDefault();

                if (record == null)
                {                    
                    record = CreateRecordFromConnection(connection, tag, scale, weight);
                }
                else
                {
                    if (DateTime.Now.Subtract(record.InDate.Value).TotalMinutes < MinTimeWeighingInterval)
                        return "WeightTooSoon";                    

                    if (DoCompleteToWeight(record))
                    {
                        record.OutDate = DateTime.Now;
                        record.OutWeight = 0;
                        record.ScaleID = scale.GUID;
                        record.Netto = record.OutWeight.HasValue && record.InWeight.HasValue ? Math.Abs(record.OutWeight.Value - record.InWeight.Value) : 0;
                        record.CertificateID = NextID(record);
                        Save(record);
                        MailContacts(record);
                        record = CreateRecordFromConnection(connection, tag, scale, weight);
                    }
                    else
                    {
                        record.OutDate = DateTime.Now;
                        record.OutWeight = weight;
                        record.ScaleID = scale.GUID;
                        record.Netto = record.OutWeight.HasValue && record.InWeight.HasValue ? Math.Abs(record.OutWeight.Value - record.InWeight.Value) : 0;
                        record.CertificateID = NextID(record);
                    }
                }

                Save(record);

                if (myLprData.LicensePlateImage != null)
                    SaveImage(myLprData.LicensePlateImage, record.GUID);

                MailContacts(record);
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static long? GetVehicleID(string licensePlate, sp_GetConnections_Result connection)
        {
            long? result;
            var vehicle = VehiclesHandler.Filter(new sp_GetVehicles_Result { LicenseNumber = licensePlate }).FirstOrDefault();

            if (vehicle != null)
            {
                result = vehicle.GUID;
            }
            else
            {
                var newVehicle = new sp_GetVehicles_Result() {
                    Active = true,
                    CompanyID = connection.CompanyID,
                    LicenseNumber = licensePlate,
                    Tare = 0,
                    WeighingModeID = 1
                };

                VehiclesHandler.Save(newVehicle);
                newVehicle = VehiclesHandler.Filter(new sp_GetVehicles_Result { LicenseNumber = licensePlate }).FirstOrDefault();
                result = newVehicle.GUID;
            }

            return result;
        }

        private static void SaveImage(Image myImage, long myGUID)
        {
            try
            {
                var directory = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Images_LPR/" + DateTime.Now.ToString("yyyyMM"));
                var path = Path.Combine(directory, myGUID + DateTime.Now.ToString("_yyyyMMdd_HHmmss") + ".jpg");
                if(!Directory.Exists(directory))
                    System.IO.Directory.CreateDirectory(directory);
                myImage.Save(path);
            }
            catch (Exception ex)
            {
                throw ex;
             /*   string log = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data"), "log.txt");
                Logger.Log(ex, log);*/
            }
        }

        public static void MailContacts(sp_GetVehiclesWeighing_Result record)
        {
            try
            {
                if (record.CustomerID.HasValue)
                {
                    var contactsToSendMail = CustomersContactsHandler.Filter(new sp_GetCustomersContacts_Result { Active = true, CustomerID = record.CustomerID.Value, IsSendWeightsByMail = true }).ToList();
                    foreach (sp_GetCustomersContacts_Result contact in contactsToSendMail)
                    {
                        if (!record.OutDate.HasValue)
                        {
                            if (contact.SendingMethodID == 1)//שקילה
                            {
                                if (String.IsNullOrEmpty(record.LicenseNumber) && record.VehicleID.HasValue)
                                {
                                    var v = VehiclesHandler.Filter(new sp_GetVehicles_Result { GUID = record.VehicleID.Value }).FirstOrDefault();
                                    if (v != null)
                                        record.LicenseNumber = v.LicenseNumber;
                                }

                                var company = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = record.CompanyID.Value }).FirstOrDefault();
                                DictionaryHandler dictionary = new DictionaryHandler();
                                Language lang = Language.HE;
                                if (company != null)
                                {
                                    if (company.LanguageID.HasValue)
                                        lang = (Language)company.LanguageID.Value;
                                }

                                dictionary.LoadText(lang);
                                List<string> tos = new List<string>();
                                tos.Add(contact.Email);
                                DomainObjects.SendMail(tos, null, dictionary.Weight, String.Format("{0}:{1}{2}{3}:{4}{2}{5}:{6}{2}", dictionary.Date, record.InDate.Value.ToString("dd/MM/yyyy HH:mm"), Environment.NewLine, dictionary.InWeight, record.InWeight.Value, dictionary.LicenseNumber, record.LicenseNumber), null);
                            }
                        }
                        else
                        {
                            LocalReport report = VehiclesWeighingHandler.Report(record);
                            string id = record.CertificateID.Value.ToString("D6");
                            string reportPath = Utilities.SaveReport(report, record.CompanyID.Value);
                            Utilities.ReportToPDF(report, reportPath);
                            CustomersContactsHandler.Mail(contact, id, reportPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static sp_GetVehiclesWeighing_Result CreateRecordFromConnection(sp_GetConnections_Result connection, string tag, sp_GetScales_Result scale, decimal weight)
        {
            return new sp_GetVehiclesWeighing_Result
            {
                CompanyID = connection.CompanyID,
                CustomerID = connection.CustomerID,
                DriverID = connection.DriverID,
                InDate = DateTime.Now,
                InSiteID = connection.InSiteID,
                ItemID = connection.ItemID,
                OutSiteID = connection.OutSiteID,
                TransportID = connection.TransportID,
                VehicleID = connection.VehicleID,
                InWeight = weight,
                ScaleID = scale.GUID,
                IsCancelled = false,
                IsManual = false,
                WeighingMode = 1,
                Tag = tag,
            };
        }

        private static bool DoCompleteToWeight(sp_GetVehiclesWeighing_Result record)
        {
            try
            {
                if (record == null) return false;

                if (record.CompanyID == null || !record.CompanyID.HasValue)
                    return false;

                var company = CompaniesHandler.Filter(new sp_GetCompanies_Result { GUID = record.CompanyID.Value, Active = true }).FirstOrDefault();
                if (company == null) return false;
                if (company.Hour.HasValue && company.Minute.HasValue)
                {
                    DateTime now = DateTime.Now;
                    if ((now - record.InDate.Value).Days >= 1 && company.Hour.Value <= now.Hour && company.Minute.Value <= now.Minute)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ResetUnfinishedWeightsOfCompany(sp_GetCompanies_Result company)
        {
            if (company != null)
            {
                var records = Filter(new sp_GetVehiclesWeighing_Result
                {
                    CompanyID = company.GUID
                }).Where(i => i.OutDate == null);

                foreach (sp_GetVehiclesWeighing_Result record in records)
                {
                    record.OutDate = DateTime.Now;
                    record.OutWeight = 0;
                    record.Netto = record.OutWeight.HasValue && record.InWeight.HasValue ? Math.Abs(record.OutWeight.Value - record.InWeight.Value) : 0;
                    record.CertificateID = NextID(record);
                    //record.LicensePlate = myLprData != null ? myLprData.LicensePlateString : "";
                    Save(record);
                }
            }
        }
    }
}
