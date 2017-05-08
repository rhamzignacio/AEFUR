using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBillingReportRepository.ViewModel
{

    public class Invoice
    {
    //    public string _Remarks;
    //    public string Remarks
    //    {
    //        get
    //        {
    //            if (FreeFields != null && FreeFields.Contains("AEFUR"))
    //                return "SUBMITTED";
    //            else
    //                return "";
    //        }
    //        set
    //        {
    //            _Remarks = value;
    //        }
    //    }

        public string _recordType;
        public string RecordType
        {
            get
            {
                if (FreeFields != null && FreeFields.Contains("AEFUR"))
                    return "SUBMITTED";
                else
                    return _recordType.ToUpper();
            }
            set
            {
                _recordType = value;
            }
        }
        
        public decimal? InvoideDetailID { get; set; }

        public decimal InvoiceID { get; set; }

        public string InvoiceNo { get; set; }

        public string RecordLocator { get; set; }

        public string TicketNo { get; set; }
      
        public byte? TransactionType { get; set; }

        public DateTime? TransactionDate { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public string ClientName { get; set; }

        public string BookingAgent { get; set; }

        public string TicketingAgent { get; set; }

        public string PassengerName { get; set; }

        public string DestinationCity { get; set; }

        public string DepartureCity { get; set; }

        public decimal? GrossAmount { get; set; }

        public string BookingAgentName { get; set; }

        public string TicketingAgentName { get; set; }

        public string Currency { get; set; }

        public DateTime? BookingDate { get; set; }

        public string AirlineCode { get; set; }

        public string Itinerary { get; set; }

        public string Department { get; set; }

        private string _supplier;

        public string Supplier
        {
            get
            {
                //if (_supplier == "10000058")
                //    return "Philippine Airlines";
                //else if (_supplier == "10000029")
                //    return "Cebu Pacific";
                //else if (_supplier == "10000008")
                //    return "Air Asia";
                //else if (_supplier == "10000001")
                //    return "IATA";
                //else if (_supplier == "10000044")
                //    return "IASA";
                //else
                //    return "";

                using (var db = new TravComEntities())
                {
                    var supplier = db.Profiles.FirstOrDefault(r => r.ProfileNumber == _supplier);

                    if (supplier != null)
                        return supplier.FirstName;
                    else
                        return "";
                }
            }
            set
            {
                _supplier = value;
            }
        }

        public string FreeFields { get; set; }       
    }

    public class AgentCode
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Department { get; set; }

        //Airline
        public string CebuPac { get; set; }

        public string PAL { get; set; }

        public string AirAsia { get; set; }

        public string IATA { get; set; }

        public string IASA { get; set; }
        //TravCom
        public string TravCom1 { get; set; }

        public string TravCom2 { get; set; }

        public string TravCom3 { get; set; }

        public string TravCom4 { get; set; }

        public string TravCom5 { get; set; }

        public bool Tick { get; set; }
    }

    public class TravcomViewModel : TravcomViewModelBase<ARInvoices>
    {
        public Currencies GetCurrency(string code)
        {
            using (var db = new TravComEntities())
            {
                return db.Currencies.FirstOrDefault(r => r.CurrencyCode == code);
            }
        }

        public List<AgentCode> GetAllAgentByDepartment(string department)
        {
            using (var db = new AirlineBillingReportEntities())
            {
                var query = from agent in db.AgentProfile
                            where agent.Department.ToUpper() == department.ToUpper()
                            select new AgentCode
                            {
                                ID = agent.ID,
                                Name = agent.Name,
                                CebuPac = agent.CebuPacific,
                                PAL = agent.PAL,
                                IASA = agent.IASA,
                                IATA = agent.IATA,
                                AirAsia = agent.AirAsia,
                                TravCom1 = agent.TravCom1,
                                TravCom2 = agent.TravCom2,
                                TravCom3 = agent.TravCom3,
                                TravCom4 = agent.TravCom4,
                                TravCom5 = agent.TravCom5,
                                Department = agent.Department,
                                Tick = true
                            };

                return query.ToList();
            }
        }
        public List<AgentCode> GetAllAgent()
        {
            using (var db = new AirlineBillingReportEntities())
            {
                var query = from agent in db.AgentProfile
                            select new AgentCode
                            {
                                ID = agent.ID,
                                Name = agent.Name,
                                CebuPac = agent.CebuPacific,
                                PAL = agent.PAL,
                                IASA = agent.IASA,
                                IATA = agent.IATA,
                                AirAsia =agent.AirAsia,
                                TravCom1 = agent.TravCom1,
                                TravCom2 = agent.TravCom2,
                                TravCom3 = agent.TravCom3,
                                TravCom4 =agent.TravCom4,
                                TravCom5 = agent.TravCom5,
                                Department = agent.Department,
                                Tick = true
                            };

                return query.ToList();
            }
        }

        public List<Invoice> GetAllUnpostedviaSQL(DateTime fromDate, DateTime to)
        {
            var db = new TravComEntities();

            var query = from inv in db.AEFURUnbilled
                        join s in db.IfSegments on inv.InvoiceDetailID equals s.InvoiceDetailID into qSegment
                        from segment in qSegment.DefaultIfEmpty()
                        where inv.BookingDate >= fromDate && inv.BookingDate <= to
                        select new Invoice
                        {
                            RecordType = "Unbilled",
                            InvoiceID = inv.InvoiceID,
                            RecordLocator = inv.RecordLocator,
                            TicketNo = inv.TicketNumber,
                            InvoiceNo = inv.InvoiceNumber,
                            TransactionType = inv.TransactionType,
                            InvoiceDate = inv.InvoiceDate,
                            ClientName = inv.ProfileName,
                            BookingAgent = inv.BookingAgentNumber,
                            PassengerName = inv.PassengerName,
                            GrossAmount = inv.GrossAmount,
                            BookingAgentName = inv.FullName,
                            InvoideDetailID = inv.InvoiceDetailID,
                            Currency = inv.CurrencyCode,
                            BookingDate = inv.InvoiceDate, //Ticket issuance date
                            AirlineCode = segment.AirlineCode,
                            Itinerary = segment.DepartureCityCode + " - " + segment.ArrivalCityCode,
                            Supplier = inv.VendorNumber,
                            TransactionDate = inv.TransactionDate,
                            FreeFields = inv.FreeFieldA,
                            Department = inv.Department,
                            TicketingAgent = inv.TicketingAgentNumber,
                            TicketingAgentName = inv.TicketingAgent
                        };

            return query.ToList();
        }

        public List<Invoice> GetAllUnpostedViaSQLPerTC(string travcom1, string travcom2, string travcom3, string travcom4, string travcom5)
        {
            var db = new TravComEntities();

                var query = from inv in db.AEFURUnbilled
                            join s in db.IfSegments on inv.InvoiceDetailID equals s.InvoiceDetailID into qSegment
                            from segment in qSegment.DefaultIfEmpty()
                            where inv.BookingAgentNumber == travcom1 || inv.BookingAgentNumber == travcom2 || inv.BookingAgentNumber == travcom3
                                || inv.BookingAgentNumber == travcom4 || inv.BookingAgentNumber == travcom5
                            select new Invoice
                            {
                                RecordType = "Unbilled",
                                InvoiceID = inv.InvoiceID,
                                RecordLocator = inv.RecordLocator,
                                TicketNo = inv.TicketNumber,
                                InvoiceNo = inv.InvoiceNumber,
                                TransactionType = inv.TransactionType,
                                InvoiceDate = inv.InvoiceDate,
                                ClientName = inv.ProfileName,
                                BookingAgent = inv.BookingAgentNumber,
                                PassengerName = inv.PassengerName,
                                GrossAmount = inv.GrossAmount,
                                BookingAgentName = inv.FullName,
                                InvoideDetailID = inv.InvoiceDetailID,
                                Currency = inv.CurrencyCode,
                                BookingDate = inv.InvoiceDate, //Ticket issuance date
                                AirlineCode = segment.AirlineCode,
                                Itinerary = segment.DepartureCityCode + " - " + segment.ArrivalCityCode,
                                Supplier = inv.VendorNumber,
                                TransactionDate = inv.TransactionDate,
                                FreeFields = inv.FreeFieldA,
                                Department = inv.Department
                                //TicketingAgent = inv.TicketingAgentNumber,
                                //TicketingAgentName = inv.TicketingAgent
                            };

                return query.ToList();
        }

        public List<Invoice> GetAllUnpostedViaSQLPerDepartment(string department, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var db = new TravComEntities();

            if (fromDate != null && toDate != null)
            {
                var query = from inv in db.AEFURUnbilled
                            join s in db.IfSegments on inv.InvoiceDetailID equals s.InvoiceDetailID into qSegment
                            from segment in qSegment.DefaultIfEmpty()
                            where inv.TransactionDate >= fromDate && inv.TransactionDate <= toDate
                            && inv.Department == department
                            select new Invoice
                            {
                                RecordType = "Unbilled",
                                InvoiceID = inv.InvoiceID,
                                RecordLocator = inv.RecordLocator,
                                TicketNo = inv.TicketNumber,
                                InvoiceNo = inv.InvoiceNumber,
                                TransactionType = inv.TransactionType,
                                InvoiceDate = inv.InvoiceDate,
                                ClientName = inv.ProfileName,
                                BookingAgent = inv.BookingAgentNumber,
                                PassengerName = inv.PassengerName,
                                GrossAmount = inv.GrossAmount,
                                BookingAgentName = inv.FullName,
                                InvoideDetailID = inv.InvoiceDetailID,
                                Currency = inv.CurrencyCode,
                                BookingDate = inv.InvoiceDate, //Ticket issuance date
                                AirlineCode = segment.AirlineCode,
                                Itinerary = segment.DepartureCityCode + " - " + segment.ArrivalCityCode,
                                Supplier = inv.VendorNumber,
                                TransactionDate = inv.TransactionDate,
                                FreeFields = inv.FreeFieldA,
                                Department = inv.Department,
                                //TicketingAgent = inv.TicketingAgentNumber,
                                //TicketingAgentName = inv.TicketingAgent
                            };

                return query.ToList();
            }
            else
            {
                var query = from inv in db.AEFURUnbilled
                            join s in db.IfSegments on inv.InvoiceDetailID equals s.InvoiceDetailID into qSegment
                            from segment in qSegment.DefaultIfEmpty()
                            where inv.Department == department
                            select new Invoice
                            {
                                RecordType = "Unbilled",
                                InvoiceID = inv.InvoiceID,
                                RecordLocator = inv.RecordLocator,
                                TicketNo = inv.TicketNumber,
                                InvoiceNo = inv.InvoiceNumber,
                                TransactionType = inv.TransactionType,
                                InvoiceDate = inv.InvoiceDate,
                                ClientName = inv.ProfileName,
                                BookingAgent = inv.BookingAgentNumber,
                                PassengerName = inv.PassengerName,
                                GrossAmount = inv.GrossAmount,
                                BookingAgentName = inv.FullName,
                                InvoideDetailID = inv.InvoiceDetailID,
                                Currency = inv.CurrencyCode,
                                BookingDate = inv.InvoiceDate, //Ticket issuance date
                                AirlineCode = segment.AirlineCode,
                                Itinerary = segment.DepartureCityCode + " - " + segment.ArrivalCityCode,
                                Supplier = inv.VendorNumber,
                                TransactionDate = inv.TransactionDate,
                                FreeFields = inv.FreeFieldA,
                                Department = inv.Department,
                                //TicketingAgent = inv.TicketingAgentNumber,
                                //TicketingAgentName = inv.TicketingAgent
                            };

                return query.ToList();
            }
        }

        public List<Invoice> GetAllUnpostedViaSQL(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var db = new TravComEntities();

            if (fromDate != null && toDate != null)
            {
                var query = from inv in db.AEFURUnbilled
                            join s in db.IfSegments on inv.InvoiceDetailID equals s.InvoiceDetailID into qSegment
                            from segment in qSegment.DefaultIfEmpty()
                            where inv.TransactionDate >= fromDate && inv.TransactionDate <= toDate
                            && inv.Department != null
                            select new Invoice
                            {
                                RecordType = "Unbilled",
                                InvoiceID = inv.InvoiceID,
                                RecordLocator = inv.RecordLocator,
                                TicketNo = inv.TicketNumber,
                                InvoiceNo = inv.InvoiceNumber,
                                TransactionType = inv.TransactionType,
                                InvoiceDate = inv.InvoiceDate,
                                ClientName = inv.ProfileName,
                                BookingAgent = inv.BookingAgentNumber,
                                PassengerName = inv.PassengerName,
                                GrossAmount = inv.GrossAmount,
                                BookingAgentName = inv.FullName,
                                InvoideDetailID = inv.InvoiceDetailID,
                                Currency = inv.CurrencyCode,
                                BookingDate = inv.InvoiceDate, //Ticket issuance date
                                AirlineCode = segment.AirlineCode,
                                Itinerary = segment.DepartureCityCode + " - " + segment.ArrivalCityCode,
                                Supplier = inv.VendorNumber,
                                TransactionDate = inv.TransactionDate,
                                FreeFields = inv.FreeFieldA,
                                Department = inv.Department,
                                //TicketingAgent = inv.TicketingAgentNumber,
                                //TicketingAgentName = inv.TicketingAgent
                            };

                return query.ToList();
            }
            else
            {
                var query = from inv in db.AEFURUnbilled
                            join s in db.IfSegments on inv.InvoiceDetailID equals s.InvoiceDetailID into qSegment
                            from segment in qSegment.DefaultIfEmpty()
                            where inv.Department != null
                            select new Invoice
                            {
                                RecordType = "Unbilled",
                                InvoiceID = inv.InvoiceID,
                                RecordLocator = inv.RecordLocator,
                                TicketNo = inv.TicketNumber,
                                InvoiceNo = inv.InvoiceNumber,
                                TransactionType = inv.TransactionType,
                                InvoiceDate = inv.InvoiceDate,
                                ClientName = inv.ProfileName,
                                BookingAgent = inv.BookingAgentNumber,
                                PassengerName = inv.PassengerName,
                                GrossAmount = inv.GrossAmount,
                                BookingAgentName = inv.FullName,
                                InvoideDetailID = inv.InvoiceDetailID,
                                Currency = inv.CurrencyCode,
                                BookingDate = inv.InvoiceDate, //Ticket issuance date
                                AirlineCode = segment.AirlineCode,
                                Itinerary = segment.DepartureCityCode + " - " + segment.ArrivalCityCode,
                                Supplier = inv.VendorNumber,
                                TransactionDate = inv.TransactionDate,
                                FreeFields = inv.FreeFieldA,
                                Department = inv.Department,
                                //TicketingAgent = inv.TicketingAgentNumber,
                                //TicketingAgentName = inv.TicketingAgent
                            };

                return query.ToList();
            }  
        }

        public List<Invoice> GetBilledTickets(DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var db = new TravComEntities())
            {
                IQueryable<Invoice> query;

                if (fromDate == null && toDate == null)
                {
                    query = from inv in db.ARInvoices
                            join invDetail in db.ARInvoiceDetails on inv.InvoiceID equals invDetail.InvoiceID
                            join agent in db.Profiles on inv.BookingAgentNumber equals agent.ProfileNumber into qAgent
                            from y in qAgent.DefaultIfEmpty()
                            join ticket in db.Profiles on inv.TicketingAgentNumber equals ticket.ProfileNumber into qTicket
                            from tkt in qTicket.DefaultIfEmpty()
                            join s in db.IfSegments on invDetail.InvoiceDetailID equals s.InvoiceDetailID into qSegment
                            from segment in qSegment.DefaultIfEmpty()
                            where invDetail.GrossAmount > 0
                            select new Invoice
                            {
                                RecordType = "Billed",
                                InvoiceID = inv.InvoiceID,
                                RecordLocator = inv.RecordLocator,
                                TicketNo = invDetail.TicketNumber,
                                InvoiceNo = inv.InvoiceNumber,
                                TransactionType = invDetail.TransactionType,
                                InvoiceDate = inv.InvoiceDate,
                                ClientName = inv.ProfileName,
                                BookingAgent = inv.BookingAgentNumber,
                                PassengerName = invDetail.PassengerName,
                                GrossAmount = invDetail.GrossAmount,
                                BookingAgentName = y.FullName,
                                InvoideDetailID = invDetail.InvoiceDetailID,
                                Currency = invDetail.CurrencyCode,
                                BookingDate = inv.InvoiceDate, //Ticket issuance date
                                AirlineCode = segment.AirlineCode,
                                Itinerary = segment.DepartureCityCode + " - " + segment.ArrivalCityCode,
                                Supplier = invDetail.VendorNumber,
                                TransactionDate = invDetail.TransactionDate,
                                TicketingAgent = inv.TicketingAgentNumber,
                                TicketingAgentName = tkt.FullName
                            };

                    return query.ToList();
                }
                else
                {
                    query = from inv in db.ARInvoices
                            join invDetail in db.ARInvoiceDetails on inv.InvoiceID equals invDetail.InvoiceID
                            join agent in db.Profiles on inv.BookingAgentNumber equals agent.ProfileNumber into qAgent
                            from y in qAgent.DefaultIfEmpty()
                            join ticket in db.Profiles on inv.TicketingAgentNumber equals ticket.ProfileNumber into qTicket
                            from tkt in qTicket.DefaultIfEmpty()
                            join s in db.IfSegments on invDetail.InvoiceDetailID equals s.InvoiceDetailID into qSegment
                            from segment in qSegment.DefaultIfEmpty()
                            where invDetail.GrossAmount > 0
                            && invDetail.TransactionDate >= fromDate
                            && invDetail.TransactionDate <= toDate
                            select new Invoice
                            {
                                RecordType = "Billed",
                                InvoiceID = inv.InvoiceID,
                                RecordLocator = inv.RecordLocator,
                                TicketNo = invDetail.TicketNumber,
                                InvoiceNo = inv.InvoiceNumber,
                                TransactionType = invDetail.TransactionType,
                                InvoiceDate = inv.InvoiceDate,
                                ClientName = inv.ProfileName,
                                BookingAgent = inv.BookingAgentNumber,
                                PassengerName = invDetail.PassengerName,
                                GrossAmount = invDetail.GrossAmount,
                                BookingAgentName = y.FullName,
                                InvoideDetailID = invDetail.InvoiceDetailID,
                                Currency = invDetail.CurrencyCode,
                                BookingDate = inv.InvoiceDate, //Ticket issuance date
                                AirlineCode = segment.AirlineCode,
                                Itinerary = segment.DepartureCityCode + " - " + segment.ArrivalCityCode,
                                Supplier = invDetail.VendorNumber,
                                TransactionDate = invDetail.TransactionDate,
                                TicketingAgent = inv.TicketingAgentNumber,
                                TicketingAgentName = tkt.FullName
                            };

                    return query.ToList();
                }

            }
        }

        public List<Invoice> GetUnbilledTickets(bool showUnbilled, bool showSubmitted,DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var db = new TravComEntities())
            {
                if (fromDate == null && toDate == null)
                {
                    var query = from inv in db.IfInvoices
                                join invDetail in db.IfInvoiceDetails on inv.InvoiceID equals invDetail.InvoiceID
                                join agent in db.Profiles on inv.BookingAgentNumber equals agent.ProfileNumber into qAgent
                                from y in qAgent.DefaultIfEmpty()
                                join ticket in db.Profiles on inv.TicketingAgentNumber equals ticket.ProfileNumber into qTicket
                                from tkt in qTicket.DefaultIfEmpty()
                                join s in db.IfSegments on invDetail.InvoiceDetailID equals s.InvoiceDetailID into qSegment
                                from segment in qSegment.DefaultIfEmpty()
                                where invDetail.GrossAmount > 0 && invDetail.TransactionType != 2
                                select new Invoice
                                {
                                    RecordType = "Unbilled",
                                    InvoiceID = inv.InvoiceID,
                                    RecordLocator = inv.RecordLocator,
                                    TicketNo = invDetail.TicketNumber,
                                    InvoiceNo = "",
                                    TransactionType = invDetail.TransactionType,
                                    InvoiceDate = inv.InvoiceDate,
                                    ClientName = inv.ProfileName,
                                    BookingAgent = inv.BookingAgentNumber,
                                    PassengerName = invDetail.PassengerName,
                                    GrossAmount = invDetail.GrossAmount,
                                    BookingAgentName = y.FullName,
                                    InvoideDetailID = invDetail.InvoiceDetailID,
                                    Currency = invDetail.CurrencyCode,
                                    BookingDate = inv.BookingDate,
                                    AirlineCode = segment.AirlineCode,
                                    Itinerary = segment.DepartureCityCode + " - " + segment.ArrivalCityCode,
                                    Supplier = invDetail.VendorNumber,
                                    TransactionDate = invDetail.TransactionDate,
                                    FreeFields = invDetail.FreeFieldA,
                                    TicketingAgent = inv.TicketingAgentNumber,
                                    TicketingAgentName = tkt.FullName
                                };

                    if (!showSubmitted)
                    {
                        query = query.Where(r => !r.FreeFields.Contains("AEFUR"));
                    }

                    if(!showUnbilled)
                    {
                        query = query.Where(r => r.FreeFields.Contains("AEFUR"));
                    }

                    return query.ToList();
                }
                else // With DateRange
                {
                    var query = from inv in db.IfInvoices
                                join invDetail in db.IfInvoiceDetails on inv.InvoiceID equals invDetail.InvoiceID
                                join agent in db.Profiles on inv.BookingAgentNumber equals agent.ProfileNumber into qAgent
                                from y in qAgent.DefaultIfEmpty()
                                join ticket in db.Profiles on inv.TicketingAgentNumber equals ticket.ProfileNumber into qTicket
                                from tkt in qTicket.DefaultIfEmpty()
                                join s in db.IfSegments on invDetail.InvoiceDetailID equals s.InvoiceDetailID into qSegment
                                from segment in qSegment.DefaultIfEmpty()                             
                                where invDetail.GrossAmount > 0 && invDetail.TransactionType != 2
                                && invDetail.TransactionDate >= fromDate
                                && invDetail.TransactionDate <= toDate
                                select new Invoice
                                {
                                    RecordType = "Unbilled",
                                    InvoiceID = inv.InvoiceID,
                                    RecordLocator = inv.RecordLocator,
                                    TicketNo = invDetail.TicketNumber,
                                    InvoiceNo = "",
                                    TransactionType = invDetail.TransactionType,
                                    InvoiceDate = inv.InvoiceDate,
                                    ClientName = inv.ProfileName,
                                    BookingAgent = inv.BookingAgentNumber,
                                    PassengerName = invDetail.PassengerName,
                                    GrossAmount = invDetail.GrossAmount,
                                    BookingAgentName = y.FullName,
                                    InvoideDetailID = invDetail.InvoiceDetailID,
                                    Currency = invDetail.CurrencyCode,
                                    BookingDate = inv.BookingDate,
                                    AirlineCode = segment.AirlineCode,
                                    Itinerary = segment.DepartureCityCode + " - " + segment.ArrivalCityCode,
                                    Supplier = invDetail.VendorNumber,
                                    TransactionDate = invDetail.TransactionDate,
                                    FreeFields = invDetail.FreeFieldA,
                                    TicketingAgent = inv.TicketingAgentNumber,
                                    TicketingAgentName = tkt.FullName
                                };

                    if (!showSubmitted)
                    {
                        query = query.Where(r => !r.FreeFields.Contains("AEFUR"));
                    }

                    if (!showUnbilled)
                    {
                        query = query.Where(r => r.FreeFields.Contains("AEFUR"));
                    }

                    return query.ToList();
                }
            }
        }

        public List<Invoice> GetAllUnposted(DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var db = new TravComEntities())
            {
                var query = from inv in db.IfInvoices
                            join invDetail in db.IfInvoiceDetails on inv.InvoiceID equals invDetail.InvoiceID
                            join agent in db.Profiles on inv.BookingAgentNumber equals agent.ProfileNumber into qAgent
                            from y in qAgent.DefaultIfEmpty()
                            join ticket in db.Profiles on inv.TicketingAgentNumber equals ticket.ProfileNumber into qTicket
                            from tkt in qTicket.DefaultIfEmpty()
                            join s in db.IfSegments on invDetail.InvoiceDetailID equals s.InvoiceDetailID into qSegment
                            from segment in qSegment.DefaultIfEmpty()
                            select new Invoice
                            {
                                RecordType = "Unbilled",
                                InvoiceID = inv.InvoiceID,
                                RecordLocator = inv.RecordLocator,
                                TicketNo = invDetail.TicketNumber,
                                InvoiceNo = "",
                                TransactionType = invDetail.TransactionType,
                                InvoiceDate = inv.InvoiceDate,
                                ClientName = inv.ProfileName,
                                BookingAgent = inv.BookingAgentNumber,
                                PassengerName = invDetail.PassengerName,
                                GrossAmount = invDetail.GrossAmount,
                                BookingAgentName = y.FullName,
                                InvoideDetailID = invDetail.InvoiceDetailID,
                                Currency = invDetail.CurrencyCode,
                                BookingDate = inv.BookingDate,
                                AirlineCode = segment.AirlineCode,
                                Itinerary = segment.DepartureCityCode + " - " + segment.ArrivalCityCode,
                                Supplier = invDetail.VendorNumber,
                                FreeFields = invDetail.FreeFieldA,
                                TransactionDate = invDetail.TransactionDate,
                                TicketingAgent = inv.TicketingAgentNumber,
                                TicketingAgentName = tkt.FullName
                            };

                var temp = query.ToList();

                return query.ToList();
            }
        }

        public List<Invoice> GetVoidedTickets()
        {
                using (var db = new TravComEntities())
                {
                    var query = from inv in db.IfInvoices
                                join invDetail in db.IfInvoiceDetails on inv.InvoiceID equals invDetail.InvoiceID
                                join agent in db.Profiles on inv.BookingAgentNumber equals agent.ProfileNumber into qAgent
                                from y in qAgent.DefaultIfEmpty()
                                join ticket in db.Profiles on inv.TicketingAgentNumber equals ticket.ProfileNumber into qTicket
                                from tkt in qTicket.DefaultIfEmpty()
                                join s in db.IfSegments on invDetail.InvoiceDetailID equals s.InvoiceDetailID into qSegment
                                from segment in qSegment
                                where invDetail.GrossAmount < 0 || invDetail.TransactionType == 2
                                select new Invoice
                                {
                                    RecordType = "Unbilled",
                                    InvoiceID = inv.InvoiceID,
                                    RecordLocator = inv.RecordLocator,
                                    TicketNo = invDetail.TicketNumber,
                                    InvoiceNo = "",
                                    TransactionType = invDetail.TransactionType,
                                    InvoiceDate = inv.InvoiceDate,
                                    ClientName = inv.ProfileName,
                                    BookingAgent = inv.BookingAgentNumber,
                                    PassengerName = invDetail.PassengerName,
                                    GrossAmount = invDetail.GrossAmount,
                                    BookingAgentName = y.FullName,
                                    InvoideDetailID = invDetail.InvoiceDetailID,
                                    Currency = invDetail.CurrencyCode,
                                    BookingDate = inv.BookingDate,
                                    AirlineCode = segment.AirlineCode,
                                    Itinerary = segment.DepartureCityCode + " - " + segment.ArrivalCityCode,
                                    Supplier = invDetail.VendorNumber,
                                    FreeFields = invDetail.FreeFieldA,
                                    TransactionDate = invDetail.TransactionDate,
                                    TicketingAgent = inv.TicketingAgentNumber,
                                    TicketingAgentName = tkt.FullName
                                };

                    var temp = query.ToList();

                    return query.ToList();
                }              
        }

        public List<Invoice> GetAllPosted()
        {
            using (var db = new TravComEntities())
            {
                var query = from inv in db.ARInvoices
                            join invDetail in db.ARInvoiceDetails on inv.InvoiceID equals invDetail.InvoiceID
                            join agent in db.Profiles on inv.BookingAgentNumber equals agent.ProfileNumber into qAgent
                            from y in qAgent.DefaultIfEmpty()
                            join ticket in db.Profiles on inv.TicketingAgentNumber equals ticket.ProfileNumber into qTicket
                            from tkt in qTicket.DefaultIfEmpty()
                            join segment in db.Segments on invDetail.InvoiceDetailID equals segment.InvoiceDetailID into qSegment
                            from s in qSegment.DefaultIfEmpty()
                            where invDetail.TicketNumber != "" && invDetail.TicketNumber != null 
                            && invDetail.GrossAmount >= 0
                            select new Invoice
                            {
                                RecordType = "Billed",
                                InvoiceID = inv.InvoiceID,
                                RecordLocator = inv.RecordLocator,
                                TicketNo = invDetail.TicketNumber,
                                InvoiceNo = inv.InvoiceNumber,
                                TransactionType = invDetail.TransactionType,
                                InvoiceDate = inv.InvoiceDate,
                                ClientName = inv.ProfileName,
                                BookingAgent = inv.BookingAgentNumber,
                                PassengerName = invDetail.PassengerName,
                                GrossAmount = invDetail.GrossAmount,
                                BookingAgentName = y.FullName,
                                InvoideDetailID = invDetail.InvoiceDetailID,
                                Currency = invDetail.CurrencyCode,
                                BookingDate = inv.BookingDate,
                                AirlineCode = s.AirlineCode,
                                Itinerary = s.DepartureCityCode + " - " + s.ArrivalCityCode,
                                Supplier = invDetail.VendorNumber,
                                TransactionDate = invDetail.TransactionDate,
                                TicketingAgent = inv.TicketingAgentNumber,
                                TicketingAgentName = tkt.FullName
                            };

                return query.ToList();
            }
        }

        public string GetInvoiceNo(decimal _invoiceID)
        {
            return GetEntity(r => r.InvoiceID == _invoiceID).InvoiceNumber;
        }

        public string CheckIfPosted(string _recordLocator)
        {
            _recordLocator = _recordLocator.Replace(" ", "");

            var invoice = GetEntity(r => r.RecordLocator == _recordLocator);

            if (invoice != null)
            {
                return invoice.InvoiceNumber;
            }
            else
            {
                return "";
            }
        }
    }
}
