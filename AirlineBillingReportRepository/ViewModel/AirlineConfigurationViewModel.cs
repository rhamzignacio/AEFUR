using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBillingReportRepository.ViewModel
{
    public class AirlineConfigurationViewModel : ViewModelBase<AirlineConfiguration>
    {
        public int ConvertColumnToInteger(string _col)
        {
            string col = _col.ToUpper();

            if (col == "A")
                return 0;
            else if (col == "B")
                return 1;
            else if (col == "C")
                return 2;
            else if (col == "D")
                return 3;
            else if (col == "E")
                return 4;
            else if (col == "F")
                return 5;
            else if (col == "G")
                return 6;
            else if (col == "H")
                return 7;
            else if (col == "I")
                return 8;
            else if (col == "J")
                return 9;
            else if (col == "K")
                return 10;
            else if (col == "L")
                return 11;
            else if (col == "M")
                return 12;
            else if (col == "N")
                return 13;
            else if (col == "O")
                return 14;
            else if (col == "P")
                return 15;
            else if (col == "Q")
                return 16;
            else if (col == "R")
                return 17;
            else if (col == "S")
                return 18;
            else if (col == "T")
                return 19;
            else if (col == "U")
                return 20;
            else if (col == "V")
                return 21;
            else if (col == "W")
                return 22;
            else if (col == "X")
                return 23;
            else if (col == "Y")
                return 24;
            else if (col == "Z")
                return 25;
            else if (col == "AA")
                return 26;
            else if (col == "AB")
                return 27;
            else if (col == "AC")
                return 28;
            else if (col == "AD")
                return 29;
            else if (col == "AE")
                return 30;
            else if (col == "AF")
                return 31;
            else if (col == "AG")
                return 32;
            else if (col == "AH")
                return 33;
            else if (col == "AI")
                return 34;
            else if (col == "AJ")
                return 35;
            else if (col == "AK")
                return 36;
            else if (col == "AL")
                return 37;
            else if (col == "AM")
                return 38;
            else if (col == "AN")
                return 39;
            else if (col == "AO")
                return 40;
            else if (col == "AP")
                return 41;
            else if (col == "AQ")
                return 42;
            else if (col == "AR")
                return 43;
            else if (col == "AS")
                return 44;
            else if (col == "AT")
                return 45;
            else if (col == "AU")
                return 46;
            else if (col == "AV")
                return 47;
            else if (col == "AW")
                return 48;
            else if (col == "AX")
                return 49;
            else if (col == "AY")
                return 50;
            else if (col == "AZ")
                return 51;
            else
                return -1;
        }

        public AirlineConfiguration GetSelected(string _airline)
        {
            return GetEntity(r => r.Airline == _airline);
        }

        public bool UpdateConfiguration(AirlineConfiguration _config)
        {
            try
            {
                var airline = GetEntity(r => r.Airline == _config.Airline);

                airline.TabName = _config.TabName;

                airline.StartColumn = _config.StartColumn;

                airline.StartRow = _config.StartRow;

                airline.FirstNameCol = _config.FirstNameCol;

                airline.LastNameCol = _config.LastNameCol;

                airline.RecordLocatorCol = _config.RecordLocatorCol;

                airline.CreatedOrganizationCol = _config.CreatedOrganizationCol;

                airline.SourceOrganizationCodeCol = _config.SourceOrganizationCodeCol;

                airline.AgentCodeCol = _config.AgentCodeCol;

                airline.PaymentCodeCol = _config.PaymentCodeCol;

                airline.PaymentIDCol = _config.PaymentIDCol;

                airline.AuthorizationStatusCol = _config.AuthorizationStatusCol;

                airline.CurrencyCodeCol = _config.CurrencyCodeCol;

                airline.BookingAmountCol = _config.BookingAmountCol;

                airline.CollectedCurrencyCodeCol = _config.CollectedAmountCol;

                airline.CollectedAmountCol = _config.CollectedAmountCol;

                airline.ConvertedCurrencyCodeCol = _config.ConvertedCurrencyCodeCol;

                airline.ConvertedAmountCol = _config.ConvertedAmountCol;

                airline.PaymentText = _config.PaymentText;

                airline.PassengerFirstName = _config.PassengerFirstName;

                airline.PassengerLastName = _config.PassengerLastName;

                airline.RouteDeparture = _config.RouteDeparture;

                airline.RouteDestination = _config.RouteDestination;

                airline.PaymentDate = _config.PaymentDate;

                airline.TicketNo = _config.TicketNo;

                airline.AirlineCode = _config.AirlineCode;

                Update(airline);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
