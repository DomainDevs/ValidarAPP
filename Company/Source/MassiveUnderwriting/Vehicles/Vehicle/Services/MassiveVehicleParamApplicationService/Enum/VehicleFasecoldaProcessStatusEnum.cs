using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.Enum
{
    [Flags]
    public enum VehicleFasecoldaProcessStatusEnum
    {
        [EnumMember]
        Validando = 1,
        [EnumMember]
        ValidadoConExito = 2,
        [EnumMember]
        ValidadoConErrores = 3,
        [EnumMember]
        Cargando = 4,
        [EnumMember]
        Cargado = 5,
        [EnumMember]
        Procesando = 6,
        [EnumMember]
        Procesado = 7

    }
}
