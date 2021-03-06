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
    
    public partial class RecordNoStorage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RecordNoStorage()
        {
            this.BilledTicket = new HashSet<BilledTicket>();
            this.ErrorLog = new HashSet<ErrorLog>();
            this.IATA1 = new HashSet<IATA>();
            this.NoRecord = new HashSet<NoRecord>();
            this.UnbilledTicket = new HashSet<UnbilledTicket>();
        }
    
        public string RecordNo { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public Nullable<System.Guid> ID { get; set; }
        public string C5J { get; set; }
        public string PAL { get; set; }
        public string IATA { get; set; }
        public string IASA { get; set; }
        public string AIRASIA { get; set; }
        public string IATAReference { get; set; }
        public string IATADateRange { get; set; }
        public string PALReportDate { get; set; }
        public string PALReportTime { get; set; }
        public string PALTotalRecord { get; set; }
        public string PALPCC { get; set; }
        public string PALDateRange { get; set; }
        public string IASAAccount { get; set; }
        public string IASARunOn { get; set; }
        public string IASAAsAt { get; set; }
        public string IASADueDate { get; set; }
        public string IASACurrency { get; set; }
        public string C5JOrgCode { get; set; }
        public string C5JCurrency { get; set; }
        public string C5JFOP { get; set; }
        public string C5JPHIA { get; set; }
        public string C5JPaymentDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BilledTicket> BilledTicket { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ErrorLog> ErrorLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IATA> IATA1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NoRecord> NoRecord { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnbilledTicket> UnbilledTicket { get; set; }
    }
}
