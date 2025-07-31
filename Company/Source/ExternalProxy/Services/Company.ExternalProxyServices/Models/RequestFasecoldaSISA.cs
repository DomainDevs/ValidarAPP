namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class RequestFasecoldaSISA
    {
        [DataMember]
        public string Plate { get; set; }

        [DataMember]
        public string Engine { get; set; }

        [DataMember]
        public string Chassis { get; set; }

        [DataMember]
        public Guid GuidProcess { get; set; }
    }
}