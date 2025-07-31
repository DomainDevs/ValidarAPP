using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.MassiveVehicleParamBusinessService.Enum
{
    [Flags]
    public enum VehicleFasecoldaStatusEnum
    {
        [EnumMember]
        Finalizado = 1,
        [EnumMember]
        EnProceso = 2
    }
}
