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
    
    public partial class UnpostedIATAUSD_Result
    {
        public System.Guid ID { get; set; }
        public string RecordNo { get; set; }
        public string AirlineCode { get; set; }
        public string TicketNo { get; set; }
        public string IssueDate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string ClientName { get; set; }
        public string AgentName { get; set; }
        public string RecordLocator { get; set; }
        public string PassengerName { get; set; }
        public string Itinerary { get; set; }
        public Nullable<decimal> USDAmount { get; set; }
        public string InvoiceStatus { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string IsPosted { get; set; }
        public string IATACurrency { get; set; }
        public Nullable<decimal> PHPAmount { get; set; }
    }
}
