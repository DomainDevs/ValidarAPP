using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.Enum;
using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.DTOs
{
    public class VehicleFasecoldaStatusTypeDTO
    {
        [DataMember]
        public int ProcessId { get; set;}

        [DataMember]
        public VehicleFasecoldaProcessTypeEnum ProcessType { get; set; }

        [DataMember]
        public VehicleFasecoldaProcessStatusEnum StatusType { get; set; }

        [DataMember]
        public DateTime BeginDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }
    }
}
