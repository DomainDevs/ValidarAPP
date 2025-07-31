using Sistran.Core.Application.ModelServices.Models.Param;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{

    [DataContract]
    public class CoCoverageServiceModel : ParametricServiceModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public bool IsImpression { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string ImpressionValue { get; set; }
        [DataMember]
        public bool IsAccMinPremium { get; set; }
        [DataMember]
        public bool IsAssistance { get; set; }

        [DataMember]
        public bool IsChild { get; set; }
        [DataMember]
        public bool IsSeriousOffer { get; set; }
    }
}
