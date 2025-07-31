using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.Enum
{
    [Flags]
    public enum FileTypeFasecoldaEnum
    {
        [EnumMember]
        Valores = 1,
        [EnumMember]
        Codigo = 2
    }
}
