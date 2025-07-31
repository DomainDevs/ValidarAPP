// -----------------------------------------------------------------------
// <copyright file="ScoreTypeDoc.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Contiene el modelo de datos para el tipo de documento datacrédito
    /// </summary>
    [DataContract]
    public class ScoreTypeDoc
    {
        /// <summary>
        /// Obtiene o establece la Key para el tipo de documento datacrédito
        /// </summary>
        [DataMember]
        public int IdCardTypeScore { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del tipo de documento datacrédito
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción corta del tipo de documento datacrédito
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la key de la asociación entre tipo de documento datacrédito y tipo de documento SISE 3g
        /// </summary>
        [DataMember]
        public int IdScore3g { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de documento SISE 3g asociado
        /// </summary>
        [DataMember]
        public int IdCardTypeCode { get; set; }
    }
}
