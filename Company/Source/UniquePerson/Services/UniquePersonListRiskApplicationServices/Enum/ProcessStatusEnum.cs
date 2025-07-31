using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonListRiskApplicationServices.Enum
{
    [Flags]
    public enum ProcessStatusEnum
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
        Procesado = 7,
        [EnumMember]
        SinCoincidencias = 9
    }
    public enum MatchingProcessStatusEnum
    {
        [Description("Cargando")]
        Cargando = 1,
        [Description("Cargado")]
        Cargado = 2,
        [Description("Procesando")]
        Procesando = 3,
        [Description("Procesado")]
        Procesado = 4,
        [Description("ConErrores")]
        ConErrores = 5,

    }
}
