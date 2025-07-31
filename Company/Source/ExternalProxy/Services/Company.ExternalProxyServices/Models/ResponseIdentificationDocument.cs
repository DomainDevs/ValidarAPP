using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponseIdentificationDocument
    {
        [DataMember]
        public string Number { get; set; }
        [DataMember]
        public DateTime ExpeditionDate { get; set; }
        [DataMember]
        public ResponseDocumentType DocumentType { get; set; }
    }
}
