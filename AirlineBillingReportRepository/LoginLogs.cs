//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AirlineBillingReportRepository
{
    using System;
    using System.Collections.Generic;
    
    public partial class LoginLogs
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> UserAccountID { get; set; }
        public Nullable<System.DateTime> LoginDate { get; set; }
        public Nullable<System.TimeSpan> LoginTime { get; set; }
    
        public virtual UserAccount UserAccount { get; set; }
    }
}
