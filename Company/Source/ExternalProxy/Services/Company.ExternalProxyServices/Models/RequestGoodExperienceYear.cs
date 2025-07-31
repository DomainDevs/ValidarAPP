using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class RequestGoodExperienceYear
    {
        [DataMember]
        public int IdCardTypeCode { get; set; }
        [DataMember]
        public string IdCardNo { get; set; }
        [DataMember]
        public string LicensePlate { get; set; }
        [DataMember]
        public int ResetYears { get; set; }
        [DataMember]
        public Guid GuidProcess { get; set; }
    }
}
