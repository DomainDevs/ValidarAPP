using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.DTOs;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class ValidateParameterBrokerCoinsuranceReinsuranceDTO
    {
        // Broker
        [DataMember]
        public BranchDTO Branch { get; set; }
        [DataMember]
        public SalePointDTO SalePoint { get; set; }
        [DataMember]
        public CompanyDTO Company{ get; set; }
        [DataMember]
        public CurrencyDTO Currency { get; set; }
        [DataMember]
        public int AgentId { get; set; }
        [DataMember]
        public int AccountingNatureId { get; set; }
        [DataMember]
        public int CheckingAccountConceptId { get; set; }
        
        // Coinsurance
        [DataMember]
        public int CoinsuranceTypeId { get; set; }
        [DataMember]
        public int CoinsuranceId { get; set; }

        // Reinsurance
        [DataMember]
        public LineBusinessDTO Prefix { get; set; }
        [DataMember]
        public SubLineBusinessDTO SubPrefix { get; set; }
        [DataMember]
        public int ReinsuranceId { get; set; }
        [DataMember]
        public int ContractTypeId { get; set; }
        [DataMember]
        public string ContractNumber { get; set; }
        [DataMember]
        public int StretchId { get; set; }
        [DataMember]
        public int ApplicationYear { get; set; }
        [DataMember]
        public int ApplicationMonth { get; set; }


    }
}
