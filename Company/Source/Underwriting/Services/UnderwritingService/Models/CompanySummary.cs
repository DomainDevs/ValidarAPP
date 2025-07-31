using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Resumen de la Pliza
    /// </summary>
    [DataContract]
    public class CompanySummary : BaseSummary
    {
        [DataMember]
        public List<CompanyRisk> Risks { get; set; }
		
		[DataMember]
        public List<CompanyRiskInsured> RisksInsured { get; set; }

        [DataMember]
        public CompanyIssuanceInsured companyContract { get; set; }
    }
}
