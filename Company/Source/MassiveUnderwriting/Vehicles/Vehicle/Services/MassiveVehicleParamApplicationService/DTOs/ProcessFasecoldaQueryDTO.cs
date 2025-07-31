

using Sistran.Company.Application.Utilities.DTO;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.DTOs
{
    [DataContract]
    public class ProcessFasecoldaQueryDTO
    {
        [DataMember]
        public ProcessFasecoldaDTO ProcessFasecolda { get; set; }

        [DataMember]
        public ErrorDTO Error { get; set; }
    }
}
