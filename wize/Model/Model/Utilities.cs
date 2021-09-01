using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Shkila.Model
{
    public enum Model
    {
        VehiclesWeighing = 1
    }

    public enum Language
    {
        HE = 1,
        EN = 2
    }

    public class Utilities
    {        

        public static DataTable ToDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity,null);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }       
        

        public static void DeleteFileInDirecotryNotContainsName(string directory, string partOfFileName)
        {
            if(Directory.Exists(directory))
            {
                var files = Directory.GetFiles(directory).Where( i => !i.Contains(partOfFileName));
                foreach(string file in files)
                    File.Delete(file);
            }
        }

        public static double GetDirectorySizeMb(string filePath)
        {
            if (!Directory.Exists(filePath))
                return -1;

            string[] a = Directory.GetFiles(filePath, "*.*");
            
            long b = 0;
            foreach (string name in a)
            {
                FileInfo info = new FileInfo(name);
                b += info.Length;
            }
            
            return b/1000000;
        }

        public static string SaveReport(LocalReport rpt, long id)
        {
            string directory = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/" + id);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            Utilities.DeleteFileInDirecotryNotContainsName(directory, DateTime.Now.ToString("dd-MM-yyyy"));

            return Path.Combine(directory, DateTime.Now.ToString("dd-MM-yyyy_HH_mm_ss") + ".pdf");
        }

        public static bool ByteArrayToFile(string fileName, byte[] byteArray, int offset)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, offset, byteArray.Length - offset);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }

        }
        public static FileStream ReportToPDF(LocalReport report, string reportPath)
        {
            try
            {
                byte[] Bytes = report.Render(format: "PDF", deviceInfo: "");

                FileStream stream = new FileStream(reportPath, FileMode.Create);
                stream.Write(Bytes, 0, Bytes.Length);
                stream.Close();
                return stream;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }       
    }
}
