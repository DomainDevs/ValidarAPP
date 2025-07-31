
namespace Sistran.Core.Application.ModelServices.Enums
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    [Flags]
    public enum RateType
    {
        /// <summary>
        /// Constante porcentaje
        /// </summary>
        [EnumMember]
        Percentage = 1,

        /// <summary>
        /// Constante pormilaje
        /// </summary>        
        [EnumMember]
        Permilage = 2,

        /// <summary>
        /// Constante valor fijo
        /// </summary>        
        [EnumMember]
        FixedValue = 3
    }
}
