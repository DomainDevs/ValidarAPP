namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    public class ApplicationPremiumCommision
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// AppComponentId
        /// </summary>
        public int AppComponentId { get; set; }
        /// <summary>
        /// CommisionType
        /// </summary>
        public int CommissionType { get; set; }
        /// <summary>
        /// AgentTypeId
        /// </summary>
        public int AgentTypeId { get; set; }
        /// <summary>
        /// AgentId
        /// </summary>
        public int AgentIndividualId { get; set; }
        /// <summary>
        /// AgenteAgencyId
        /// </summary>
        public int AgentAgencyId { get; set; }
        /// <summary>
        /// Currency
        /// </summary>
        public int CurrencyId { get; set; }
        /// <summary>
        /// ExchangeRate
        /// </summary>
        public decimal ExchangeRate { get; set; }
        /// <summary>
        /// Amount
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// LocalAmount
        /// </summary>
        public decimal LocalAmount { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal CommissionDiscountIncomeAmount { get; set; }
        public decimal CommissionDiscountAmount { get; set; }
        public int ApplicationPremiumId { get; set; }
        public bool IsUsedCommission { get; set; }
        public int DiscountedCommissionId { get; set; }
        public string AgentName { get; set; }
        public decimal BaseIncomeAmount { get; set; }
        public decimal CommissionPercentage { get; set; }
        public decimal AgentPercentageParticipation { get; set; }
    }
}
