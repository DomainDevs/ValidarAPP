namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ResponseFasecoldaSISA
    {
        [DataMember]
        public ResponseGuideInfo GuideInfo { get; set; }

        [DataMember]
        public List<ResponseSimitSISA> Simit { get; set; }

        [DataMember]
        public List<ResponseClaims> Claims { get; set; }

        [DataMember]
        public ResponseVINVerification VINVerification { get; set; }

        [DataMember]
        public List<ResponsePoliciesInfo> PoliciesInfo { get; set; }
    }
}