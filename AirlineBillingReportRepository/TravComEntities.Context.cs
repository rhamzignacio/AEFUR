﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TravComEntities : DbContext
    {
        public TravComEntities()
            : base("name=TravComEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ARInvoiceDetails> ARInvoiceDetails { get; set; }
        public virtual DbSet<ARInvoices> ARInvoices { get; set; }
        public virtual DbSet<IfInvoiceDetails> IfInvoiceDetails { get; set; }
        public virtual DbSet<IfInvoices> IfInvoices { get; set; }
        public virtual DbSet<IfSegments> IfSegments { get; set; }
        public virtual DbSet<Profiles> Profiles { get; set; }
        public virtual DbSet<Segments> Segments { get; set; }
        public virtual DbSet<Currencies> Currencies { get; set; }
        public virtual DbSet<AEFURUnbilled> AEFURUnbilled { get; set; }
    }
}
