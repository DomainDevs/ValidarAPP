using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    [DataContract]
    public class CoverageDTO
    {
        [DataMember]
        public int LineBusinessId { get; set; }
        [DataMember]
        public int SubLineBusinessId { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int InsuredObjectId { get; set; }
        [DataMember]
        public bool Facultative { get; set; }
        [DataMember]
        public AmountDTO LimitAmount { get; set; }
        [DataMember]
        public AmountDTO Premium { get; set; }
        [DataMember]
        public int LineId { get; set; }
        [DataMember]
        public string CumulusKey { get; set; }
        [DataMember]
        public int ErrorId { get; set; }
    }
}
