using Sistran.Core.Application.ModelServices.Models.Param;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    [DataContract]
    public class TechnicalPlanCoveragesServiceRelationModel : ErrorServiceModel
    {
        [DataMember]
        public List<TechnicalPlanCoverageServiceRelationModel> TechnicalPlanCoverageServiceRelationModel { get; set; }
    }
}
