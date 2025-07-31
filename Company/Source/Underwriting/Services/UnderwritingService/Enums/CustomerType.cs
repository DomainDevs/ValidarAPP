using System;
using System.Runtime.Serialization;


namespace Sistran.Company.Application.UnderwritingServices.Enums
{

    [Flags]
    public enum CustomerType
    {
        /// <summary>
        /// Tipo de Cliente
        /// </summary>
        [EnumMember]
        Individual = 1,
        /// <summary>
        /// Constante Prospecto
        /// </summary>
        [EnumMember]
        Prospect = 2
    }

}
