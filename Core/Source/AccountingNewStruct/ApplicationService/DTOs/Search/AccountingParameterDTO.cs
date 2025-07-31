using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class AccountingParameterDTO 
    {
        [DataMember]
        public int CollectCode { get; set; }

        [DataMember]
        public int CollectConceptCode { get; set; }

        [DataMember]
        public DateTime RegisterDate { get; set; }

        [DataMember]
        public int PaymentCode { get; set; }

        [DataMember]
        public int PaymentMethodTypeCode { get; set; }

        [DataMember]
        public int IssuingBankCode { get; set; }
        
        [DataMember]
        public int CurrencyCode { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public decimal IncomeAmount { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public int PayerId { get; set; }

        [DataMember]
        public string PayerDocumentNumber { get; set; }

        [DataMember]
        public int CollectControlCode { get; set; }

        [DataMember]
        public int BranchCode { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public decimal PaymentsTotal { get; set; }

        [DataMember]
        public int ReceivingBankCode { get; set; }

        [DataMember]
        public string ReceivingAccountingNumber { get; set; }

        [DataMember]
        public string IssuingAccountingNumber { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }
    }
}
