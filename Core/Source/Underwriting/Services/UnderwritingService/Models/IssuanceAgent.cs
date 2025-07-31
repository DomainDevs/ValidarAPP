using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class IssuanceAgent : BaseIssuanceAgent
    {
        [DataMember]
        public IssuanceAgentType AgentType { get; set; }
    }
}