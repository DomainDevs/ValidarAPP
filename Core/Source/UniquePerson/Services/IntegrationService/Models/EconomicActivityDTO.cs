using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePerson.IntegrationService.Models
{

    [DataContract]
    public class EconomicActivityDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
    }
}