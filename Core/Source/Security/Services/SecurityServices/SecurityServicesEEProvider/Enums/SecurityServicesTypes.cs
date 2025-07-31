using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.SecurityServices.EEProvider.Enums
{
    /// <summary>
    /// Zonas
    /// </summary>
    [Flags]
    public enum AccessObjectsType
    {
        /// <summary>
        /// Formulario Web
        /// </summary>
        [EnumMember]
        WebForm = 1,

        /// <summary>
        /// menu Opciones
        /// </summary>
        [EnumMember]
        Menu = 2,

        /// <summary>
        /// pagina Java
        /// </summary>
        [EnumMember]
        JavaPage = 3,

        /// <summary>
        /// menu Java
        /// </summary>
        [EnumMember]
        Javamenu = 4,

        /// <summary>
        /// Control
        /// </summary>
        [EnumMember]
        Control = 5,

        /// <summary>
        /// Vistas Mvc
        /// </summary>
        [EnumMember]
        ViewMvc = 6,

        /// <summary>
        /// menu Opciones Release 2
        /// </summary>
        [EnumMember]
        MenuR2 = 7
    }
}
