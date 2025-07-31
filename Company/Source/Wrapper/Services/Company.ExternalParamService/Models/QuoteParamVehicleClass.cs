using Sistran.Company.ExternalParamService.DTO;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class QuoteParamVehicleClass
    {
        [DataMember]
        public List<VehicleParameterDTO> VehicleParameterDTOCo { get; set; }

        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
