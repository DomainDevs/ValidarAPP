using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Transports.TransportBusinessService.Models
{
    [DataContract]
    public class CompanyEndorsementPeriod
    {
        [DataMember]
        public decimal Id { get; set; }
        [DataMember]
        public DateTime CurrentFrom { get; set; }
        [DataMember]
        public DateTime CurrentTo { get; set; }
        [DataMember]
        public int DeclarationPeriod { get; set; }
        [DataMember]
        public int AdjustPeriod { get; set; }
        [DataMember]
        public int TotalDeclarations { get; set; }
        [DataMember]
        public int TotalAdjustment { get; set; }
        [DataMember]
        public decimal PolicyId { get; set; }
        [DataMember]
        public int Version { get; set; }
    }
}
