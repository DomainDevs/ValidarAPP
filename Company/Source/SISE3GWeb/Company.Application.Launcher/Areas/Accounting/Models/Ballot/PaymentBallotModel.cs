using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Ballot
{
    public class PaymentBallotModel
    {
        public int PaymentBallotId { get; set; }
        public string PaymentBallotNumber { get; set; }
        public string PaymentAccountNumber { get; set; }
        public int PaymentBallotBankId { get; set; }
        public decimal PaymentBallotAmount { get; set; }
        public decimal PaymentBallotBankAmount { get; set; }
        public int PaymentCurrency { get; set; }
        public DateTime PaymentBankDate { get; set; }
        public int PaymentStatus { get; set; }
        public List<PaymentTicketBallotModel> PaymentTicketBallotModels { get; set; }
        public int PaymentAccountingAccountId { get; set; }
    }

    [KnownType("PaymentTicketBallotModel")]
    public class PaymentTicketBallotModel
    {
        public int PaymentTicketBallotId { get; set; }
    }
}