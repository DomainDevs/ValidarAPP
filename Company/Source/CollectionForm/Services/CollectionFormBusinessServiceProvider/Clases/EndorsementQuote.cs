using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CollectionFormBusinessServiceProvider.Clases
{
    public class EndorsementQuote
    {
        public EndorsementQuote()
        { }

        public EndorsementQuote(int endorsementId, int endorsementNumber, int quoteNum, int payerId, string name, DateTime payerexpdate, int status, decimal amount, decimal totalpremium)
        {
            this.endorsementId = endorsementId;
            this.endorsementNumber = endorsementNumber;
            this.quoteNum = quoteNum;
            this.payerId = payerId;
            this.name = name;
            this.payerexpdate = payerexpdate;
            this.status = status;
            this.amount = amount;
            this.totalpremium = totalpremium;
        }

        private int endorsementId;
        public int EndorsementId
        {
            get { return endorsementId; }
            set { endorsementId = value; }
        }

        private int endorsementNumber;
        public int EndorsementNumber
        {
            get { return endorsementNumber; }
            set { endorsementNumber = value; }
        }

        private int quoteNum;
        public int QuoteNum
        {
            get { return quoteNum; }
            set { quoteNum = value; }
        }

        private int payerId;
        public int PayerId
        {
            get { return payerId; }
            set { payerId = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private DateTime payerexpdate;
        public DateTime Payerexpdate
        {
            get { return payerexpdate; }
            set { payerexpdate = value; }
        }

        private int status;
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        private decimal amount;
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        private decimal totalpremium;
        public decimal TotalPremium
        {
            get { return totalpremium; }
            set { totalpremium = value; }
        }

        public override string ToString()
        {
            string ans = string.Concat("EndorsementId: ", EndorsementId.ToString(),
                                       "EndorsementNumber: ", EndorsementNumber.ToString(),
                                       "QuoteNum: ", QuoteNum.ToString(),
                                       "PayerId: ", PayerId.ToString(),
                                       "Name: ", Name,
                                       "Payerexpdate: ", Payerexpdate.ToString(),
                                        "Status: ", Status.ToString(),
                                        "Amount: ", Amount.ToString(),
                                        "TotalPremium: ", TotalPremium.ToString());
            //age.ToString();
            return ans;
        }
    }
}
