// -----------------------------------------------------------------------
// <copyright file="CompanyPhoneType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UniquePersonService.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Contiene el modelo de datos para el tipo de teléfono
    /// </summary>
    [DataContract]
    public class CompanyPhoneType
    {
        /// <summary>
        /// Obtiene o establece la Key para el tipo de teléfono
        /// </summary>
        [DataMember]
        public int PhoneTypeCode { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del tipo de teléfono
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción corta del tipo de teléfono
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es celular
        /// </summary>
        [DataMember]
        public bool IsCellphone { get; set; }

        /// <summary>
        /// Obtiene o establece la expresión regular para validar el tipo de número
        /// </summary>
        [DataMember]
        public string RegExpression { get; set; }

        /// <summary>
        /// Obtiene o establece el mensaje de error cuando no cumpla con la expresión regular
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si tiene información asociada
        /// </summary>
        [DataMember]
        public bool IsForeing { get; set; }
    }
}
