using Sistran.Company.ExternalParamService.DTO;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class QuoteParamVehicleFasecoldaClass
    {
        [DataMember]
        public VehicleParameterDTO VehicleFasecolda { get; set; }

        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
