using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class IndividualDTO
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int? OwnerRoleCode { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public string SecondSurname { get; set; }

        [DataMember]
        public string CustomerTypeDescription { get; set; }

        [DataMember]
        public DateTime? BirthDate { get; set; }

        [DataMember]
        public int DocumentTypeId { get; set; }

        [DataMember]
        public string DocumentTypeDescription { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public int IndividualType { get; set; }

        [DataMember]
        public DateTime? LastUpdate { get; set; }

        [DataMember]
        public string UpdateBy { get; set; }

        [DataMember]
        public int EconomicActivityId { get; set; }

        [DataMember]
        public int EconomicActivityDescription { get; set; }

        [DataMember]
        public int CustomerType { get; set; }

        [DataMember]
        public int PaymentMethodId { get; set; }

        [DataMember]
        public int PaymentMethodPaymentId { get; set; }

        [DataMember]
        public int PaymentMethodIndividualId { get; set; }

        [DataMember]
        public int PaymentMethodRoleId { get; set; }

        [DataMember]
        public bool PaymentMethodEnabled { get; set; }

        [DataMember]
        public int IndividualTypeId { get; set; }
        
    }
}
