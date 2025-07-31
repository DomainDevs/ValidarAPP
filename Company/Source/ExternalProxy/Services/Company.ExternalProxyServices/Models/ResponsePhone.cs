using System.Runtime.Serialization;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponsePhone
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public int PhoneTypeId { get; set; }
        [DataMember]
        public bool IsPrincipal { get; set; }
        [DataMember]
        public int StreetTypeId { get; set; }
        [DataMember]
        public int CountryCode { get; set; }
        [DataMember]
        public int CityCode { get; set; }
        [DataMember]
        public int Extension { get; set; }
        [DataMember]
        public string ScheduleAvailability { get; set; }
        [DataMember]
        public string UpdateDate { get; set; }
    }
}
