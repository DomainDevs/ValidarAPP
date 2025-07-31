using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class RiskLocation : BaseRiskLocation
    {
        [DataMember]
        public Risk Risk { get; set; }

        [DataMember]
        public Country Country { get; set; }

        [DataMember]
        public State State { get; set; }

        [DataMember]
        public City City { get; set; }

        [DataMember]
        public RiskType RiskType { get; set; }
    }
}
