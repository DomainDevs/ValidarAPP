using Sistran.Company.Application.CommonAplicationServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class PhoneDTO
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
        public string PhoneTypeDescription { get; set; }
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
        [DataMember]
        public AplicationStaus AplicationStaus { get; set; }
    }
}
