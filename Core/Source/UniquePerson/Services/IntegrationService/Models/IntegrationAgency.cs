using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePerson.IntegrationService.Models
{

    [DataContract]
    public class IntegrationAgency
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public DateTime? DeclinedDate { get; set; }
        [DataMember]
        public IntegrationAgent Agent { get; set; }

    }
}