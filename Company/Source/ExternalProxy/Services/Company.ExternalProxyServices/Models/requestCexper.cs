using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class RequestCexper
    {
        [DataMember]
        public string Plate { get; set; }

        [DataMember]
        public int DocumentType { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public Guid GuidProcess { get; set; }
    }
}
