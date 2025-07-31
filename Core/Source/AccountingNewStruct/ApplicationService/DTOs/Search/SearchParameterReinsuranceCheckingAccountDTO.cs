using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.DTOs.Search;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class SearchParameterReinsuranceCheckingAccountDTO
    {
        [DataMember]
        public BranchDTO Branch { get; set; }

        [DataMember]
        public SalePointDTO SalePoint { get; set; }

        [DataMember]
        public CompanyDTO AccountingCompany { get; set; }

        [DataMember]
        public PrefixDTO Prefix { get; set; }

        [DataMember]
        public CurrencyDTO Currency { get; set; }

        [DataMember]
        public int ContractType { get; set; }

        [DataMember]
        public string ContractNumber { get; set; }

        [DataMember]
        public int ReinsurerId { get; set; }

        [DataMember]
        public int AgentId { get; set; }

        [DataMember]
        public string AgentFullName { get; set; }

        [DataMember]
        public int ReinsuranceCompanyId { get; set; }

        [DataMember]
        public string ReinsuranceCompanyFullName { get; set; }

        [DataMember]
        public string SlipNumber { get; set; }

        [DataMember]
        public string DateFrom { get; set; }

        [DataMember]
        public string DateUntil { get; set; }
    }
}
