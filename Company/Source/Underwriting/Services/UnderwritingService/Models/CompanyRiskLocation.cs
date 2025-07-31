using Sistran.Core.Application.UnderwritingServices.Models.Base;
using Sistran.Core.Application.CommonService.Models;
using System.Runtime.Serialization;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyRiskLocation : BaseRiskLocation
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
