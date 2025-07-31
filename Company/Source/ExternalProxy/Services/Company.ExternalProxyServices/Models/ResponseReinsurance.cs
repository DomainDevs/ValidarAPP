using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponseReinsurance 
    {
        [DataMember]
        public int PolicyStatus { get; set; }
        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public List<string> ErrorMessage { get; set; }
    }
}
