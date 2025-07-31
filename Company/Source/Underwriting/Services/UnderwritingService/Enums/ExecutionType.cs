using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Enums
{
    [Flags]
    public enum ExecutionType
    {
        /// <summary>
        /// Prima
        /// </summary>
        [EnumMember]
        TasaUnica = 1,
        /// <summary>
        /// Gastos
        /// </summary>
        [EnumMember]
        ReglasNegocio = 2
    }
}

