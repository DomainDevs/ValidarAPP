using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;



namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class BrokerBalanceDTO 
    {
        [DataMember]
        public int BrokerBalanceId { get; set; }
        [DataMember]
        public int AgentTypeCode { get; set; }
        [DataMember]
        public int AgentCode { get; set; }
        [DataMember]
        public DateTime BalanceDate { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public DateTime LastBalanceDate { get; set; }
        [DataMember]
        public decimal PartialBalanceAmount { get; set; }
        [DataMember]
        public decimal PartialBalanceIncomeAmount { get; set; }
        [DataMember]
        public decimal TaxPartialSum { get; set; }
        [DataMember]
        public decimal TaxPartialSubtraction { get; set; }
        [DataMember]
        public decimal TaxSum { get; set; }
        [DataMember]
        public decimal TaxSubtraction { get; set; }
        [DataMember]
        public int NumSheet { get; set; }
    }
}
