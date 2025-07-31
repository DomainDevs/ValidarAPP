using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    public class IssuanceAgencyDTO
    {
        [DataMember]
        public int DiscountedCommissionId { get; set; }
        [DataMember]
        public int ApplicationPremiumId { get; set; }
        [DataMember]
        public int AgentTypeCode { get; set; }
        [DataMember]
        public int AgentIndividualId { get; set; }
        [DataMember]
        public string AgentName { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public decimal BaseIncomeAmount { get; set; }
        [DataMember]
        public decimal BaseAmount { get; set; }
        [DataMember]
        public decimal CommissionPercentage { get; set; }
        [DataMember]
        public decimal AgentPercentageParticipation { get; set; }
        [DataMember]
        public int CommissionType { get; set; }
        [DataMember]
        public decimal CommissionDiscountIncomeAmount { get; set; }
        [DataMember]
        public decimal CommissionDiscountAmount { get; set; }
        [DataMember]
        public bool IsUsedCommission { get; set; }
        [DataMember]
        public int AgentAgencyId { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal LocalAmount { get; set; }
        [DataMember]
        public int AgentId { get; set; }


    }
}
