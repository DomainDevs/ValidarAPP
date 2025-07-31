using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class QuoteParamRatingZoneClass
    {
        [DataMember]
        public List<RatingZoneClass> RatingZoneClass { get; set; }

        [DataMember]

        public string ProcessMessage { get; set; }
    }
}
