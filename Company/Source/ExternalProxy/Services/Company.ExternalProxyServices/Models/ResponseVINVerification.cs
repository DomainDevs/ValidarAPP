namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ResponseVINVerification
    {
        [DataMember]
        public string WMICode { get; set; }

        [DataMember]
        public string MakerName { get; set; }

        [DataMember]
        public string MakerAddress1 { get; set; }
        
        [DataMember]
        public string MakerAddress2 { get; set; }

        [DataMember]
        public DateTime CreationDate { get; set; }

        [DataMember]
        public DateTime ModificationDate { get; set; }

        [DataMember]
        public string Country { get; set; }
    }
}
