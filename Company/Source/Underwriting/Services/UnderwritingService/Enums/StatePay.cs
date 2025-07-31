using System;
using System.Runtime.Serialization;


namespace Sistran.Company.Application.UnderwritingServices.Enums
{
    [Flags]
    public enum StatePay
    {
        /// <summary>
        /// Prima
        /// </summary>
        [EnumMember]
        Inicia = 1,
        /// <summary>
        /// Gastos
        /// </summary>
        [EnumMember]
        Otro = 2
    }
}
