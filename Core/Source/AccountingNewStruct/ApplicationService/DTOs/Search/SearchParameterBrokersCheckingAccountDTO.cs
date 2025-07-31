using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.DTOs.Search;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class SearchParameterBrokersCheckingAccountDTO
    {
        [DataMember]
        public BranchDTO Branch { get; set; }
        [DataMember]
        public SalePointDTO SalePoint { get; set; }
        [DataMember]
        public PrefixDTO Prefix { get; set; }
        [DataMember]
        public CurrencyDTO Currency { get; set; }
        [DataMember]
        public int AgentId { get; set; }
        [DataMember]
        public int PolicyNumber { get; set; }
        [DataMember]
        public string InsuredDocumentNumber { get; set; }
        [DataMember]
        public string InsuredFullName { get; set; }
    }
}
