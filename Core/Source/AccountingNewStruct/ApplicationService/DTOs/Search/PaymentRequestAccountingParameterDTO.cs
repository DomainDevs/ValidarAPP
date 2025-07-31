using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class PaymentRequestAccountingParameterDTO
    {
        
        [DataMember]
        public int SourceCode { get; set; }

        [DataMember]
        public int BranchCode { get; set; }

        [DataMember]
        public decimal IncomeAmount { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public int CurrencyCode { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public int PayerId { get; set; }

        [DataMember]
        public int AccountingConceptId { get; set; }

        [DataMember]
        public int AccountingAccountId { get; set; }
        
    }
}
