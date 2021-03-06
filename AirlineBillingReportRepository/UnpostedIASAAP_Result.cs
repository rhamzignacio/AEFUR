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
    
    public partial class UnpostedIASAAP_Result
    {
        public System.Guid ID { get; set; }
        public string Date { get; set; }
        public string TransactionType { get; set; }
        public string Reference { get; set; }
        public string Vessel { get; set; }
        public string ReferenceOrder { get; set; }
        public string TravelledDate { get; set; }
        public string PassengerName { get; set; }
        public string Route { get; set; }
        public string Document { get; set; }
        public string AirTicket { get; set; }
        public string BookingClass { get; set; }
        public Nullable<decimal> NetFare { get; set; }
        public Nullable<decimal> Taxes { get; set; }
        public Nullable<decimal> Amount_DR_CR { get; set; }
        public string InvoiceNo { get; set; }
        public string IsPosted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string RecordNo { get; set; }
    }
}
