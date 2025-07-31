using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    [XmlType("ProspectRequestType")]
    public class Prospect : Sistran.Core.Application.UniquePersonService.V1.Models.Prospect
    {
        [DataMember]
        public int? VerifyDigit { get; set; }

        [DataMember]
        public Boolean OriginQuote { get; set; }

        [DataMember]
        public bool ToPersist { get; set; }
    }
}
