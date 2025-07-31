using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.AccountingClosing.Models
{
    [Serializable]
    [KnownType("ExchangeDifferenceReportModel")]
    public class ExchangeDifferenceReportModel
    {
        [DataMember]
        public int AccountingAccountId { get; set; }

        [DataMember]
        public string AccountingAccountNumber { get; set; }

        [DataMember]
        public string AccountingAccountName { get; set; }

        [DataMember]
        public string AccountingAccountFullName { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }
        
        [DataMember]
        public string CurrencyDescription { get; set; }

        [DataMember]
        public decimal LocalAmountBalance { get; set; }

        [DataMember]
        public decimal ForeignAmountBalance { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public decimal ExchangeDifference { get; set; }
    }
}