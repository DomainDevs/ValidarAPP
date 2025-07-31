using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingApplicationService.DTOs
{
    [DataContract]
    public class CountryDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
    }
}