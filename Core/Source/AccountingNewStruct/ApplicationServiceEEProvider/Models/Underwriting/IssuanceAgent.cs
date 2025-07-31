using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting
{
    [DataContract]
    public class IssuanceAgent : BaseIssuanceAgent
    {
        [DataMember]
        public IssuanceAgentType AgentType { get; set; }
    }
}