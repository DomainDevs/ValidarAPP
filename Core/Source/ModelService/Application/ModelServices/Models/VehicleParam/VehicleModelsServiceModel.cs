using Sistran.Core.Application.ModelServices.Models.Param;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    [DataContract]
    public class VehicleModelsServiceModel : ErrorServiceModel
    {
        [DataMember]
        public List<VehicleModelServiceModel> VehicleModelServiceModel { get; set; }

    }
}
