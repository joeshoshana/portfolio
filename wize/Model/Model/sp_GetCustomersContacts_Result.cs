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
    
    public partial class sp_GetCustomersContacts_Result
    {
        public bool Active { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public long GUID { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Remarks { get; set; }
        public Nullable<long> CustomerID { get; set; }
        public Nullable<long> CustomerCompanyID { get; set; }
        public string CustomerName { get; set; }
        public string CompanyName { get; set; }
        public Nullable<bool> IsSendWeightsByMail { get; set; }
        public Nullable<long> SendingMethodID { get; set; }
    }
}
