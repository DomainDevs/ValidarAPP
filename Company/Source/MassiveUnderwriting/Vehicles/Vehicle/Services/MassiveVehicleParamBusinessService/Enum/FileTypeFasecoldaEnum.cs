using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.MassiveVehicleParamBusinessService.Enum
{
    [Flags]
    public enum FileTypeFasecoldaEnum
    {
        [EnumMember]
        Codigo = 1,
        [EnumMember]
        Valores = 2
    }
}
