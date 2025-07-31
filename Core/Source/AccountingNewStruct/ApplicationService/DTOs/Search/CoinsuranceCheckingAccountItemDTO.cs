using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class CoinsuranceCheckingAccountItemDTO
    {
        [DataMember]
        public int CoinsuranceCheckingAccountId { get; set; }
        [DataMember]
        public int CoinsuranceCheckingAccountItemId { get; set; }
        [DataMember]
        public int TempImputationId { get; set; }
        [DataMember]
        public int CoinsuranceType { get; set; }
        [DataMember]
        public string CoinsuranceTypeName { get; set; }
        [DataMember]
        public int TempCoinsuranceParentId { get; set; }
        [DataMember]
        public int CoinsuranceCheckingAccountItemChildId { get; set; }
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public string BranchName { get; set; }
        [DataMember]
        public int PosCode { get; set; }
        [DataMember]
        public string PosName { get; set; }
        [DataMember]
        public string CoinsurerName { get; set; }
        [DataMember]
        public string CoinsurerDocument { get; set; }
        [DataMember]
        public int CheckingAccountConceptCode { get; set; }
        [DataMember]
        public string ConceptName { get; set; }
        [DataMember]
        public int CurrencyCode { get; set; }
        [DataMember]
        public string CurrencyName { get; set; }
        [DataMember]
        public string TinyDescription { get; set; }
        [DataMember]
        public decimal CurrencyChange { get; set; }
        [DataMember]
        public decimal IncomeAmount { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string DebitCreditName { get; set; }
        [DataMember]
        public int AccountingNature { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int CompanyCode { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public int ClaimNumber { get; set; }
        [DataMember]
        public int ClaimCode { get; set; }
        [DataMember]
        public int ComplaintNumber { get; set; }
        [DataMember]
        public int AgentAgencyCode { get; set; }
        [DataMember]
        public int CollectNumber { get; set; }
        [DataMember]
        public int DebitCreditCode { get; set; }
        [DataMember]
        public int LineBusinessCode { get; set; }
        [DataMember]
        public int CoinsuranceCompanyCode { get; set; }
        [DataMember]
        public int CoinsuranceParentId { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public decimal Items { get; set; }
        [DataMember]
        public string PolicyNumber { get; set; }
        [DataMember]
        public int ImputationId { get; set; }
        [DataMember]
        public string PrefixName { get; set; }
        [DataMember]
        public int CoinsurancePolicyId { get; set; }
        [DataMember]
        public int ItemsEnabled { get; set; }
        [DataMember]
        public int SalePointCode { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public decimal CompanyParticipation { get; set; }
        [DataMember]
        public decimal CommissionFactor { get; set; }
        [DataMember]
        public decimal AdministrativeExpenses { get; set; }
        [DataMember]
        public decimal TaxAdministrativeExpenses { get; set; }
        [DataMember]
        public int SubLineBusinessCode { get; set; }
        [DataMember]
        public int AgentId { get; set; }
        [DataMember]
        public int InsuredId { get; set; } //aumentado por alejo
        [DataMember]
        public decimal AgentCommissionAmount { get; set; }
        [DataMember]
        public decimal AgentCommissionIncomeAmount { get; set; }

        //indicador de filas para paginación.
        [DataMember]
        public int Rows { get; set; }
    }
}
