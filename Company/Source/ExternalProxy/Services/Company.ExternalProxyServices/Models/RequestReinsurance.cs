using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class RequestReinsurance 
    {
        [DataMember]
        public int Branch { get; set; }
        [DataMember]
        public int Prefix { get; set; }
        [DataMember]
        public int DocumentNumber { get; set; }
        [DataMember]
        public int EndorsementNumber { get; set; }
    }
}
