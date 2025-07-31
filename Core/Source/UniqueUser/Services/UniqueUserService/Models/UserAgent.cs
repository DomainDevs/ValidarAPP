using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.UniqueUserServices.Models
{
    [DataContract]
    public class UserAgent
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public DateTime? DateDeclined { get; set; }

        [DataMember]
        public AgentType AgentType { get; set; }
    }
}