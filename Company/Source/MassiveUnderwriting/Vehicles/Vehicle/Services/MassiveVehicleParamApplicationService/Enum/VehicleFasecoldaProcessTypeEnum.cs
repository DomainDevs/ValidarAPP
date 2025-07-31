using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.Enum
{
    [Flags]
    public enum VehicleFasecoldaProcessTypeEnum
    {
        [EnumMember]
        Validado = 0,
        [EnumMember]
        Cargado = 1,
        [EnumMember]
        Procesado = 2
    }
}
