using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shkila.ScaleReaders;

namespace TourmalineUI
{
    public class ModelsToScaleConverter
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public ScaleHeaders ScaleHeader { get; set; }
        private static List<ModelsToScaleConverter> m_data = new List<ModelsToScaleConverter>();

        public static void LoadData()
        {
            m_data.Add(new ModelsToScaleConverter { Manufacturer = "Kern", Model = "PCB", ScaleHeader = ScaleHeaders.KernPCB });
            m_data.Add(new ModelsToScaleConverter { Manufacturer = "Kern", Model = "DS", ScaleHeader = ScaleHeaders.KernDS });
            m_data.Add(new ModelsToScaleConverter { Manufacturer = "Kern", Model = "PFB", ScaleHeader = ScaleHeaders.KernPFB });
            m_data.Add(new ModelsToScaleConverter { Manufacturer = "Kern", Model = "PLJ", ScaleHeader = ScaleHeaders.KernPLJ });
            m_data.Add(new ModelsToScaleConverter { Manufacturer = "Shkila", Model = "IPE50", ScaleHeader = ScaleHeaders.IPE50 });
            m_data.Add(new ModelsToScaleConverter { Manufacturer = "Shkila", Model = "IPJ800", ScaleHeader = ScaleHeaders.IPJ800 });
            m_data.Add(new ModelsToScaleConverter { Manufacturer = "Shkila", Model = "J1000", ScaleHeader = ScaleHeaders.J1000 });
            m_data.Add(new ModelsToScaleConverter { Manufacturer = "Shkila", Model = "J5478", ScaleHeader = ScaleHeaders.J5478 });
            m_data.Add(new ModelsToScaleConverter { Manufacturer = "Shkila", Model = "J6478", ScaleHeader = ScaleHeaders.J6478 });
            m_data.Add(new ModelsToScaleConverter { Manufacturer = "Shkila", Model = "Test", ScaleHeader = ScaleHeaders.Test });
        }

        public static ScaleHeaders? GetScale(string manufacturer, string model)
        {
            var data = m_data.Where(s => s.Manufacturer.Equals(manufacturer) && s.Model.Equals(model)).FirstOrDefault();
            if (data == null)
                return null;

            return data.ScaleHeader;
        }

        public static string GetModel(ScaleHeaders sh)
        {
            var data = m_data.Where(s => s.ScaleHeader == sh).FirstOrDefault();
            if (data == null)
                return null;

            return data.Model;
        }

        public static string GetManufacturer(ScaleHeaders sh)
        {
            var data = m_data.Where(s => s.ScaleHeader == sh).FirstOrDefault();
            if (data == null)
                return null;

            return data.Manufacturer;
        }

        public static List<string> GetModels(string manufacturer = null)
        {
            List<string> list = new List<string>();
            foreach (ModelsToScaleConverter item in m_data)
            {                
                if(item.Manufacturer == manufacturer || manufacturer == null)
                    list.Add(item.Manufacturer + "," + item.Model);
            }

            return list;
        }

        public static List<string> GetManufacturers()
        {
            List<string> list = new List<string>();
            foreach (ModelsToScaleConverter item in m_data)
                if(!list.Contains(item.Manufacturer))
                    list.Add(item.Manufacturer);

            return list;
        }
    }
}
