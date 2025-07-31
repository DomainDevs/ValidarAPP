using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class AccountingConceptDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public AccountingAccountDTO AccountingAccount { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool AgentEnabled { get; set; }

        [DataMember]
        public bool CoInsurancedEnabled { get; set; }

        [DataMember]
        public bool ReInsuranceEnabled { get; set; }

        [DataMember]
        public bool InsuredEnabled { get; set; }

        [DataMember]
        public bool ItemEnabled { get; set; }
    }
}
