using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{

    [DataContract]
    public class SearchInternalBallotDTO 
    {
        [DataMember]
        public int PaymentTicketCode { get; set; }
        [DataMember]
        public int BankCode { get; set; }
        [DataMember]
        public string BankDescription { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public double CashAmount { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string RegisterDate { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string User { get; set; }
        [DataMember]
        public string PaymentBallotNumber { get; set; }
        [DataMember]
        public string DespositDate { get; set; }
        [DataMember]
        public int Rows { get; set; }
        //*****************************************
        //BE
        [DataMember]
        public int CurrencyId { get; set; }

    }
}
