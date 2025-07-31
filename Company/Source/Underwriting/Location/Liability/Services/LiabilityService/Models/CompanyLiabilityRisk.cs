
using Sistran.Company.Application.Locations.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Location.LiabilityServices.Models.Base;
using Sistran.Core.Application.Locations.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Company.Application.Location.LiabilityServices.Models
{
    /// <summary>
    /// Riesgo RC
    /// </summary>
    [DataContract]
    public class CompanyLiabilityRisk : BaseLiabilityRisk
    {
        [DataMember]
        public CompanyRisk Risk { get; set; }

        [DataMember]
        public NomenclatureAddress NomenclatureAddress { get; set; }

        [DataMember]
        public City City { get; set; }

        [DataMember]
        public ConstructionType ConstructionType { get; set; }

        [DataMember]
        public RiskType RiskType { get; set; }

        [DataMember]
        public CompanyRiskUse RiskUse { get; set; }
        [DataMember]
        public CompanyAssuranceMode AssuranceMode { get; set; }

        [DataMember]
        public CompanyRiskSubActivity RiskSubActivity { get; set; }

        [DataMember]
        public List<PoliciesAut> InfringementPolicies { get; set; }
    }
}
