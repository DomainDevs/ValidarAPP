using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class RequestInfringement
    {
        [DataMember]
        public int DocumentType { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public Guid Guid { get; set; }

    }
}
