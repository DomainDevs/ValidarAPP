using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.Enum
{
    [Flags]
    public enum VehicleFasecoldaStatusEnum
    {
        [EnumMember]
        EnProceso = 0,
        [EnumMember]
        Finalizado = 1
    }
}
