using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class CheckBallotAccountingParameterDTO
    {
        [DataMember]
        public int PaymentBallotCode { get; set; }

        [DataMember]
        public int BankCode { get; set; }

        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public int CurrencyCode { get; set; }

        [DataMember]
        public DateTime RegisterDate { get; set; }

        [DataMember]
        public int BranchCode { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal CommissionAmount { get; set; }

        [DataMember]
        public decimal CashAmount { get; set; }

        [DataMember]
        public decimal BallotAmount { get; set; }

        [DataMember]
        public string BallotNumber { get; set; }

        [DataMember]
        public int PaymentAccountingAccountId { get; set; }
    }
}
