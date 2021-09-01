using Shkila.Model.Handlers;
using Shkila.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using Shkila.Utilities.LPR;
using System.Drawing;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;


namespace UI.Models
{
    public class RequestEntity
    {
        private sp_GetScales_Result scale;
        public String _command { get; set; }
        public String _mac { get; set; }
        public String _weight { get; set; }
        public String _tag { get; set; }
        public sbyte[] _pic { get; set; }
        public String _weighingTime { get; set; }

        internal bool Process()
        {
            try
            {
                if (String.IsNullOrEmpty(_mac))
                    throw new Exception("Empty mac address");

                var myLPRData = new LprData();
                var sussess = false;

                switch (_command)
                {
                    case "update_weight":
                        return InsertWeighToDB();

                    case "vehicle_weight_modify":
                        if (_pic != null && _pic.Length > 0)
                        {
                            var myLPR = LprCamera.GetLprCamera(VENDORS.Hikvision);
                            myLPRData = myLPR.GetData((byte[])(Array)(_pic));
                        }
                        var dt = ScalesHandler.Filter(new sp_GetScales_Result { MAC = this._mac });
                        var dt2 = dt.Where(i => i.MAC.Equals(this._mac));
                        string res = VehiclesWeighingHandler.WeightFromTag(this._tag, dt2.FirstOrDefault(), myLPRData);
                        if (!String.IsNullOrEmpty(res))
                            throw new Exception(res);//((DictionaryHandler)Models.Utilities.GetDicationary()).GetText(res));
                        return true;

                    case "vehicle_self_weighing":
                        sussess = InsertWeighToDB();

                        if (_pic != null && _pic.Length > 0)
                        {
                            var myLPR = LprCamera.GetLprCamera(VENDORS.Hikvision);
                            myLPRData = myLPR.GetData((byte[])(Array)(_pic));
                        }

                        var scalesTable = ScalesHandler.Filter(new sp_GetScales_Result { MAC = this._mac });
                        var scalesTableFiltered = scalesTable.Where(i => i.MAC.Equals(this._mac));
                        string result = VehiclesWeighingHandler.WeightFromTag(this._tag, scalesTableFiltered.FirstOrDefault(), myLPRData);
                        if (!String.IsNullOrEmpty(result))
                            throw new Exception(result);

                        return sussess;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool InsertWeighToDB()
        {
            scale = new sp_GetScales_Result
            {
                MAC = _mac,
                Weight = _weight
            };

            if (!ScalesHandler.IsConnected(scale))
                throw new Exception("Database refused to log weight");

            ScalesHandler.UpdateWeight(scale);
            return true;
        }
    }
}