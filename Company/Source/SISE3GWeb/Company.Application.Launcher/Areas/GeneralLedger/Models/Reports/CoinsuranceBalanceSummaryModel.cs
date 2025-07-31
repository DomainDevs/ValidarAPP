using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.Reports
{
    [Serializable]
    [KnownType("CoinsuranceBalanceSummaryModel")]
    public class CoinsuranceBalanceSummaryModel
    {
        [DataMember]
        public int OperationId { get; set; }

        [DataMember]
        public int CoinsuranceCheckingAccountCd { get; set; }

        [DataMember]
        public int CurrencyCd { get; set; }


        [DataMember]
        public int AgentTypeCd { get; set; }

        [DataMember]
        public int AgentCd { get; set; }

        [DataMember]
        public string Names { get; set; }

        [DataMember]
        public decimal IncomeAmount { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal CoinsurancePct { get; set; }
    }
}