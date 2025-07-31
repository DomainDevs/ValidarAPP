
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class EndorsementBaseDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int DocumentNum { get; set; }
        [DataMember]
        public int PolicyId { get; set; }
    }
}