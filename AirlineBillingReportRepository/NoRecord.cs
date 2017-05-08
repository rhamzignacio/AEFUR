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
    
    public partial class NoRecord
    {
        public System.Guid ID { get; set; }
        public string RecordNo { get; set; }
        public string RecordLocator { get; set; }
        public string Airline { get; set; }
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public string DateRange { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string TicketNo { get; set; }
        public string BookingAmount { get; set; }
        public string Department { get; set; }
        public string PassengerName { get; set; }
        public string Itinerary { get; set; }
        public string Currency { get; set; }
    
        public virtual RecordNoStorage RecordNoStorage { get; set; }
    }
}
