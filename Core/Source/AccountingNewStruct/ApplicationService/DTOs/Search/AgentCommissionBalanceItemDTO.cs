using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

//Sistran


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class AgentCommissionBalanceItemDTO : AgentCommissionBalanceDTO
    {
        [DataMember]
        public int AgentCommissionBalanceItemCode { get; set; }
        [DataMember]
        public int InsuredId { get; set; }
        [DataMember]
        public int PrefixId { get; set; }
        [DataMember]
        public int SubPrefixId { get; set; }
        [DataMember]
        public int LineBusinessCode { get; set; }
        [DataMember]
        public int PolicyId { get; set; }
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public int CommissionTypeCode { get; set; }
        [DataMember]
        public decimal CommissionPercentage { get; set; }
        [DataMember]
        public decimal CommissionAmount { get; set; }
        [DataMember]
        public decimal CommissionDiscounted { get; set; }
        [DataMember]
        public decimal CommissionTax { get; set; }
        [DataMember]
        public decimal CommissionRetention { get; set; }
        [DataMember]
        public decimal CommissionBalance { get; set; }
        [DataMember]
        public string DocumentNumEndorsement { get; set; }
        [DataMember]
        public string DocumentNumPolicy { get; set; }
        [DataMember]
        public string InsuredName { get; set; }
        [DataMember]
        public string LineBusinessDescription { get; set; }
        [DataMember]
        public string PrefixDescription { get; set; }
        [DataMember]
        public DateTime AccountingDate { get; set; }
        [DataMember]
        public string AgentName { get; set; }
        [DataMember]
        public int AgentTypeCode { get; set; } 
        [DataMember]
        public string AgentTypeDescription { get; set; } 
        [DataMember]
        public string DocumentNumInsured { get; set; } 
        [DataMember]
        public int BrokerCheckingAccountId { get; set; }
        [DataMember]
        public string BrokerCheckingAccountDescription { get; set; }
        [DataMember]
        public int AccountingNature { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public decimal IncomeAmount { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal ParticipationPercentage { get; set; }
        [DataMember]
        public decimal CommissionPct { get; set; }
        [DataMember]
        public decimal AdditCommissionPct { get; set; }
        
    }
}
