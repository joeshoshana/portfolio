using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class ScalesTypesHandler
    {
        public static void Save(sp_GetScalesTypes_Result item)
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveScalesType(item.GUID, item.Name, item.Active).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetScalesTypes_Result item)
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.ScalesTypes.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                dt.Active = false;
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetScalesTypes_Result item, List<string> errors)
        {
            switch(field)
            {
                case "GUID":
                    break;
                case "Name":
                    if (String.IsNullOrEmpty(item.Name))
                        errors.Add("MissingName");
                    break;
                case "Active":
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetScalesTypes_Result> Filter(sp_GetScalesTypes_Result filter )
        {
            using(WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetScalesTypes(filter.GUID, filter.Name, filter.Active).ToList().AsEnumerable();

                return data.ToList();
            }
        }
    }
}
