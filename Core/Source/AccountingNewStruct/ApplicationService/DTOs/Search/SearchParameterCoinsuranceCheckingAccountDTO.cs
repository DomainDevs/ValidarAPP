using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System.Runtime.Serialization;




namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class SearchParameterCoinsuranceCheckingAccountDTO
    {
        [DataMember]
        public BranchDTO Branch { get; set; }
        [DataMember]
        public SalePointDTO SalePoint { get; set; }
        [DataMember]
        public int CoinsuranceType { get; set; }
        [DataMember]
        public PrefixDTO Prefix { get; set; }
        [DataMember]
        public int CoinsurerId { get; set; }
        [DataMember]
        public int CoinsuranceCompanyId { get; set; }
        [DataMember]
        public string CoinsuranceCompanyFullName { get; set; }
        [DataMember]
        public int PolicyNumber { get; set; }
        [DataMember]
        public int ComplaintNumber { get; set; }
        [DataMember]
        public int ClaimNumber { get; set; }

    }
}
