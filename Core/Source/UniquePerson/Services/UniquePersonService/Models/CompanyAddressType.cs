// -----------------------------------------------------------------------
// <copyright file="CompanyAddressType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UniquePersonService.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Contiene el modelo de datos para el tipo de dirección
    /// </summary>
    [DataContract]
    public class CompanyAddressType
    {
        /// <summary>
        /// Obtiene o establece la Key para el tipo de dirección
        /// </summary>
        [DataMember]
        public int AddressTypeCode { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción corta del tipo de dirección
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la abreviación del tipo de dirección
        /// </summary>
        [DataMember]
        public string TinyDescription { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es correo electrónico
        /// </summary>
        [DataMember]
        public bool IsElectronicMail { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si tiene información asociada
        /// </summary>
        [DataMember]
        public bool IsForeing { get; set; }
    }
}
