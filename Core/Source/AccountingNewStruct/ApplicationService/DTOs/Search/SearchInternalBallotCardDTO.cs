using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{

    [DataContract]
    public class SearchInternalBallotCardDTO 
    {

        [DataMember]
        public int PaymentTicketCode { get; set; }
        [DataMember]
        public string BankDescription { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public string CreditCardDescription { get; set; }
        [DataMember]
        public int PaymentMethodTypeCode { get; set; }
        [DataMember]
        public string PaymentMethodTypeDescription { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string RegisterDate { get; set; }
        [DataMember]
        public string PaymentBallotNumber { get; set; }
        [DataMember]
        public string DepositDate { get; set; }
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public string BranchDescription { get; set; }
        [DataMember]
        public int Rows { get; set; }
    }
}
