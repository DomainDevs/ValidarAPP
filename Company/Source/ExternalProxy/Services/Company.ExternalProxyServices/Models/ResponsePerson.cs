using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponsePerson
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public string Role { get; set; }
        [DataMember]
        public ResponseIdentificationDocument IdentificationDocument { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string SecondSurname { get; set; }
        [DataMember]
        public string Names { get; set; }
        [DataMember]
        public int DocumentTypeId { get; set; }
        [DataMember]
        public string Document { get; set; }
        [DataMember]
        public DateTime BirthDate { get; set; }
        [DataMember]
        public string BirthPlace { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public int MaritalStatusId { get; set; }
        [DataMember]
        public int? ExonerationTypeCode { get; set; }
        [DataMember]
        public List<ResponseAddress> Addresses { get; set; }
        [DataMember]
        public List<ResponseEmail> Emails { get; set; }
        [DataMember]
        public List<ResponsePhone> Phones { get; set; }
        [DataMember]
        public int EconomicActivityId { get; set; }
        [DataMember]
        public string EconomicActivityDescription { get; set; }
        [DataMember]
        public string CheckPayable { get; set; }
        [DataMember]
        public int OperationId { get; set; }
    }
}
