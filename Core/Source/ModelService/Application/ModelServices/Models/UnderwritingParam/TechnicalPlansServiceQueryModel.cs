using Sistran.Core.Application.ModelServices.Models.Param;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    [DataContract]
    public class TechnicalPlansServiceQueryModel : ErrorServiceModel
    {
        [DataMember]
        public List<TechnicalPlanServiceQueryModel> TechnicalPlanServiceQueryModel { get; set; }
    }
}
