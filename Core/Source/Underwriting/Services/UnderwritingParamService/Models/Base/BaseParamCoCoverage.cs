// -----------------------------------------------------------------------
// <copyright file="ParamCoCoverage.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de negocio de CoCoverage
    /// </summary>
    [DataContract]
    public class BaseParamCoCoverage: Extension
    {
        /// <summary>
        /// Obtiene o establece el identificador de la covertura a imprimirse
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cobertura debe imprimirse
        /// </summary>
        [DataMember]
        public bool IsImpression { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la cobertura a imprimir
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el valor asegurado a imprimir
        /// </summary>
        [DataMember]
        public string ImpressionValue { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cobertura esta incluida en cálculo de prima mínima
        /// </summary>
        [DataMember]
        public bool IsAccMinPremium { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cobertura es de asistencia
        /// </summary>
        [DataMember]
        public bool IsAssistance { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la impresion de cobertura es hijo
        /// </summary>
        [DataMember]
        public bool IsChild { get; set; }

        [DataMember]
        public bool IsSeriousOffer { get; set; }
    }
}
