using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class ReinsuranceAccountingParameter
    {
        [DataMember]
        public int BranchCd { get; set; }

        [DataMember]
        public string BranchDescription { get; set; }

        [DataMember]
        public int CompanyTypeId { get; set; }

        [DataMember]
        public int ConceptId { get; set; }

        [DataMember]
        public int ContractTypeId { get; set; }

        [DataMember]
        public int CurrencyCd { get; set; }

        [DataMember]
        public string EndorsementDocumentNumber { get; set; }

        [DataMember]
        public int EndorsementId { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public decimal IncomeAmountValue { get; set; }

        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public string PolicyDocumentNumber { get; set; }

        [DataMember]
        public int PolicyId { get; set; }

        [DataMember]
        public int PrefixCd { get; set; }

        [DataMember]
        public string PrefixDescription { get; set; }

        [DataMember]
        public decimal LocalAmount { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
