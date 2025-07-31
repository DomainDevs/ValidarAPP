using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseProspectNatural : Extension
    {
        [DataMember]
        public int ProspectCode { get; set; }
        [DataMember]
        public int IndividualTyepCode { get; set; }
        [DataMember]
        public int? CountryCode { get; set; }
        [DataMember]
        public int? StateCode { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public int MaritalStatus { get; set; }
        [DataMember]
        public DateTime BirthDate { get; set; }
        [DataMember]
        public int? CityCode { get; set; }
        [DataMember]
        public int? IdCardTypeCode { get; set; }
        [DataMember]
        public string IdCardNo { get; set; }
        [DataMember]
        public int? AddressType { get; set; }
        [DataMember]
        public int EconomicActivity { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public long? PhoneNumber { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public int? TributaryIdTypeCode { get; set; }
        [DataMember]
        public string TributaryIdNumber { get; set; }
        [DataMember]
        public int? CompanyTypeCode { get; set; }
        [DataMember]
        public string MotherLastName { get; set; }
        [DataMember]
        public int IndividualTypePerson { get; set; }
        [DataMember]
        public string TradeName { get; set; }

    }
}
