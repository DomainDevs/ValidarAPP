using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class ReinsuranceCheckingAccountItemDTO
    {
        [DataMember]
        public int ReinsuranceCheckingAccountId { get; set; }
        [DataMember]
        public int ReinsuranceCheckingAccountItemId { get; set; }
        [DataMember]
        public int TempImputationId { get; set; }
        [DataMember]
        public int TempReinsuranceParentId { get; set; }
        [DataMember]
        public int ReinsuranceCheckingAccountItemChildId { get; set; }
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public string BranchName { get; set; }
        [DataMember]
        public int PosCode { get; set; }
        [DataMember]
        public string PosName { get; set; }
        [DataMember]
        public int LineBusinessCode { get; set; }
        [DataMember]
        public string PrefixName { get; set; }
        [DataMember]
        public int SubLineBusinessCode { get; set; }
        [DataMember]
        public string SubPrefixName { get; set; }
        [DataMember]
        public string BrokerName { get; set; }
        [DataMember]
        public string ReinsurerName { get; set; }
        [DataMember]
        public string SlipNumber { get; set; }
        [DataMember]
        public int ContractTypeCode { get; set; }
        [DataMember]
        public string Contract { get; set; }
        [DataMember]
        public string Stretch { get; set; }
        [DataMember]
        public string Region { get; set; }
        [DataMember]
        public int Excercise { get; set; }
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
        public string YearMonthApplies { get; set; }
        [DataMember]
        public string PolicyEndorsement { get; set; }
        [DataMember]
        public int CompanyCode { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public int AgentTypeCode { get; set; }
        [DataMember]
        public int AgentCode { get; set; }
        [DataMember]
        public int AgentAgencyCode { get; set; }
        [DataMember]
        public int CollectNumber { get; set; }
        [DataMember]
        public int DebitCreditCode { get; set; }
        [DataMember]
        public int FacultativeCode { get; set; }
        [DataMember]
        public int ReinsuranceCompanyCode { get; set; }
        [DataMember]
        public bool IsFacultative { get; set; }
        [DataMember]
        public int ApplicationMonth { get; set; }
        [DataMember]
        public int ApplicationYear { get; set; }
        [DataMember]
        public int ReinsurancePolicyId { get; set; }
        [DataMember]
        public int ReinsuranceEndorsementId { get; set; }
        [DataMember]
        public int TransactionNumber { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public decimal Items { get; set; }
        [DataMember]
        public int ImputationId { get; set; }
        [DataMember]
        public int ReinsuranceParentId { get; set; }

        //indicador de filas para paginación.
        [DataMember]
        public int Rows { get; set; }
    }
}
