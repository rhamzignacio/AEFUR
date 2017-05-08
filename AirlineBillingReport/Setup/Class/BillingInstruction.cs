using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirlineBillingReportRepository;

namespace AirlineBillingReport.Setup.Class
{
    public class BillingInstruction
    {
        public bool FreeFieldTick(string ticketNo)
        {
           try
            {
                using (var db = new TravComEntities())
                {
                    var invoiceDetails = db.IfInvoiceDetails.Where(r => r.TicketNumber.Contains(ticketNo)).ToList();

                    invoiceDetails.ForEach(inv =>
                    {
                        string returnFreeFields = "";

                        var temp = inv.FreeFieldA;

                        string[] freeFields = new string[99];

                        if(temp != "")
                            freeFields = temp.Split('/');

                        for(int x=0; x<freeFields.Count(); x++)
                        {
                            if (x < 99)
                                returnFreeFields += freeFields[x] + "/";
                            else if (x == 99)
                                returnFreeFields += "AEFUR-SUBMITTED(" + DateTime.Now.ToString().Replace("/","-") + ")";
                        }

                        if (freeFields.Count() < 100)
                        {
                            for (int x = freeFields.Count(); x < 100; x++)
                            {
                                if(x < 99)
                                 returnFreeFields += "/";

                                if (x == 99)
                                    returnFreeFields += "AEFUR-SUBMITTED(" + DateTime.Now.ToString().Replace("/", "-") + ")";
                            }
                        }

                        inv.FreeFieldA = returnFreeFields;

                        db.Entry(inv).State = System.Data.Entity.EntityState.Modified;
                    });

                    db.SaveChanges();
                    return true;
                }
            }
            catch(Exception error)
            {
                return false;
            }
        }
    }
}
