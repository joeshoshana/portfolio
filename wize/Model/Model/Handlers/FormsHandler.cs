using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shkila.Model.Handlers
{
    public class FormsHandler
    {
        public static void Save(sp_GetForms_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var a = db.sp_SaveForm(item.GUID, item.Name, item.Link, item.Active).ToList();
                if (a.Count() > 0)
                    item.GUID = (long)a.ElementAt(0);
            }
        }

        public static void Delete(sp_GetForms_Result item)
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var dt = db.Forms.Where(i => i.GUID == item.GUID).FirstOrDefault();
                if (dt == null)
                    return;

                dt.Active = false;
                db.SaveChanges();
            }
        }

        public static bool Validate(string field, sp_GetForms_Result item, List<string> errors)
        {
            switch (field)
            {
                case "GUID":
                    break;
                case "Name":
                    if (String.IsNullOrEmpty(item.Name))
                        errors.Add("MissingName");
                    break;
                case "Link":
                    if (String.IsNullOrEmpty(item.Link))
                        errors.Add("MissingLink");
                    break;
                case "Active":
                    break;
            }

            return errors.Count == 0;
        }

        public static IEnumerable<sp_GetForms_Result> Filter(sp_GetForms_Result filter )
        {
            using (WanagerDBEntities db = new WanagerDBEntities())
            {
                var data = db.sp_GetForms(filter.GUID, filter.Link, filter.Name).ToList().AsEnumerable();
                
                return data.ToList();
            }
        }

    }
}
