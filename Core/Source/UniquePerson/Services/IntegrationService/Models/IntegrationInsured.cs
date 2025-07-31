using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePerson.IntegrationService.Models
{
    [DataContract]
    public class IntegrationInsured
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string IdentificationNumber { get; set; }

        [DataMember]
        public int IdentificationTypeId { get; set; }

        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string FullName { get; set; }
        
        [DataMember]
        public DateTime? BirthDate { get; set; }

        [DataMember]
        public DateTime? DeclinedDate { get; set; }

        [DataMember]
        public CustomerType CustomerType { get; set; }

        [DataMember]
        public int PaymentId { get; set; }
    }
}