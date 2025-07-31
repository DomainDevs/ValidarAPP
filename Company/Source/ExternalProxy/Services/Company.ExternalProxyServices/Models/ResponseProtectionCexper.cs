using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponseProtectionCexper
    {

        [DataMember]
        public short CompanyCode { get; set; }

        [DataMember]
        public string SinisterNumber { get; set; }

        [DataMember]
        public DateTime SinisterDate { get; set; }

        [DataMember]
        public string Plate { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string ProtectedName { get; set; }

        [DataMember]
        public long ProtectedClaimValue { get; set; }

        [DataMember]
        public long ProtectedPaidValue { get; set; }

        
    }
}
