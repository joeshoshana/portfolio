using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class FormsFieldsHandler
    {
        public static void Save(sp_GetFormsFields_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveFormsField(item.GUID, item.Name, item.FormID).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetFormsFields_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.FormsFields.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                db.FormsFields.Remove(dt);
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetFormsFields_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "Name":
                    if (String.IsNullOrEmpty(item.Name))
                        errors.Add("MissingName");
                    break;
                case "FormID":
                    if (!item.FormID.HasValue || item.FormID.Value == 0)
                        errors.Add("InvalidForm");
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetFormsFields_Result> Filter(sp_GetFormsFields_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetFormsFields(filter.GUID, filter.FormID, filter.Name).ToList().AsEnumerable();
                
                return data.ToList();
            }
        }

    }
}
