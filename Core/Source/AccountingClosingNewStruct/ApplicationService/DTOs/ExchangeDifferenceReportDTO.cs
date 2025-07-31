using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingClosingServices.DTOs
{
    [DataContract]
    public class ExchangeDifferenceReportDTO : ClosureTempEntryGenerationDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string AccountingAccountNumber { get; set; }
        [DataMember]
        public string AccountingAccountName { get; set; }
        [DataMember]
        public decimal ExchangeDifference { get; set; }
        [DataMember]
        public bool Posted { get; set; }
        [DataMember]
        public int AccountingYear { get; set; }

        [DataMember]
        public string AccountingAccountFullName { get; set; }

        [DataMember]
        public string CurrencyDescription { get; set; }

        [DataMember]
        public decimal LocalAmountBalance { get; set; }

        [DataMember]
        public decimal ForeignAmountBalance { get; set; }
    }
}

