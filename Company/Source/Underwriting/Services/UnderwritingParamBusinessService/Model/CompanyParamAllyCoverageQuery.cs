using Sistran.Company.Application.UnderwritingParamApplicationService.DTOs;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingParamBusinessService.Model
{
    [DataContract]
    public class CompanyParamAllyCoverageQuery
    {
        [DataMember]
        public List<AllyCoverageQueryDTO> AllyCoverageDTO { get; set; }
    }
}
