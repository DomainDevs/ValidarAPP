using System;
using System.Runtime.Serialization;
namespace Sistran.Company.Application.UnderwritingServices.Enums
{
    [Flags]
    public enum ComponentTypePayerPayment
    {
        /// <summary>
        /// Prima
        /// </summary>
        [EnumMember]
        Premium = 1,
        /// <summary>
        /// Gastos
        /// </summary>
        [EnumMember]
        Expenses = 5,
        /// <summary>
        /// Impuestos
        /// </summary>
        [EnumMember]
        Taxes = 10
    }
}
