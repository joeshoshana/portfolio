//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shkila.Model
{
    using System;
    
    public partial class sp_GetCompaniesFormsFields_Result
    {
        public long GUID { get; set; }
        public Nullable<long> CompanyID { get; set; }
        public Nullable<long> FormsFieldID { get; set; }
        public Nullable<long> FormID { get; set; }
        public bool ValidationRequired { get; set; }
        public bool IsShowing { get; set; }
        public string FormsFieldName { get; set; }
        public string FormName { get; set; }
        public string FormLink { get; set; }
        public string CompanyName { get; set; }
    }
}
