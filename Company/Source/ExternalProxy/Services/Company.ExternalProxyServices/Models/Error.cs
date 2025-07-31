using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class Error
    {
        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public bool IsError { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
