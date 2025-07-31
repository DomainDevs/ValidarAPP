using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class IssuanceAgentDTO
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public DateTime? DateDeclined { get; set; }
        [DataMember]
        public IssuanceAgentTypeDTO AgentType { get; set; }
    }
}
