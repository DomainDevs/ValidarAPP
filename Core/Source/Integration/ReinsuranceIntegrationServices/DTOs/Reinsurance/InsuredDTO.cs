using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    [DataContract]
    public class InsuredDTO
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
        public int CustomerType { get; set; }
        [DataMember]
        public int PaymentId { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public int DocumentTypeId { get; set; }
    }
}
