using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class IntCoEquivalenceCoverage
    {
        [DataMember]
        public int Coverage3GId { get; set; }

        [DataMember]
        public int Coverage2GId { get; set; }

        [DataMember]
        public int InsuredObject3GId { get; set; }

        [DataMember]
        public int InsuredObject2GId { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public int SubLineBusinessId { get; set; }

        [DataMember]
        public int CoverageId { get; set; }



    }
}
