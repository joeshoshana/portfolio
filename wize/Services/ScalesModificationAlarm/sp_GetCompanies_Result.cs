//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WeightModificationAlarm
{
    using System;
    
    public partial class sp_GetCompanies_Result
    {
        public bool Active { get; set; }
        public string Address { get; set; }
        public long GUID { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public bool IsOwner { get; set; }
        public string LogoPath { get; set; }
        public string CertificateTitle { get; set; }
        public Nullable<int> LanguageID { get; set; }
        public bool IsSuper { get; set; }
        public Nullable<long> OwnerID { get; set; }
        public string DataFolder { get; set; }
        public string ImagesFolder { get; set; }
        public string SystemLogoPath { get; set; }
        public long CompaniesLimit { get; set; }
        public Nullable<int> Hour { get; set; }
        public Nullable<int> Minute { get; set; }
    }
}
