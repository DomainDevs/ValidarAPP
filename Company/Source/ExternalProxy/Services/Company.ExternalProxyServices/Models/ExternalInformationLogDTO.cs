using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    [Serializable]
    public class ExternalInformationLogDTO
    {
        public static readonly string SucsessInvoke = "SucsessInvoke";
        public static readonly string ServiceClient = "ServiceClient";

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Guid GuidProcess { get; set; }        

        [DataMember]
        public bool SuccessInvoke { get; set; }

        [DataMember]
        public string ServerClient { get; set; }

        [DataMember]
        public string ServiceMethod { get; set; }

        [DataMember]
        public string JsonRequestParams { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public long TotalTimeResponse { get; set; }

        [DataMember]
        public DateTime LocalRequestDate { get; set; }
    }
}
