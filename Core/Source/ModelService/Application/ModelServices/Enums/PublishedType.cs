
namespace Sistran.Core.Application.ModelServices.Enums
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    [Flags]
    public enum PublishedType
    {
        /// <summary>
        /// Constante porcentaje
        /// </summary>
        [EnumMember]
        Todas = 1,

        /// <summary>
        /// Constante pormilaje
        /// </summary>        
        [EnumMember]
        Publicada = 2,

        /// <summary>
        /// Constante valor fijo
        /// </summary>        
        [EnumMember]
        SinPublicar = 3
    }
}
