using Sistran.Core.Application.ModelServices.Models.Param;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    [DataContract]
    public class FirstPayComponentsServiceModel : ErrorServiceModel
    {
        [DataMember]
        public FirstPayComponentServiceModel FirstPayComponentServiceModel { get; set; }
    }
}
