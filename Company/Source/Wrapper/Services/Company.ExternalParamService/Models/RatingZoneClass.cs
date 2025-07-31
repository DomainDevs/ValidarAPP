using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class RatingZoneClass
    {
        [DataMember]
        public int RatingZoneCode { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public int PrefixCode { get; set; }
        [DataMember]
        public bool IsDefault { get; set; }
    }
}
