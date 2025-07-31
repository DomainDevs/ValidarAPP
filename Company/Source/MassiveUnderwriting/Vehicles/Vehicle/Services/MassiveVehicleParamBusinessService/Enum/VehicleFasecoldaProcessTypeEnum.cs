using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.MassiveVehicleParamBusinessService.Enum
{
    [Flags]
    public enum VehicleFasecoldaProcessTypeEnum
    {
        [EnumMember]
        Cargado = 1,
        [EnumMember]
        Procesado = 2
    }
}
