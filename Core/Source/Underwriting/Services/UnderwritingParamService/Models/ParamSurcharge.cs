// -----------------------------------------------------------------------
// <copyright file="ParamSurcharge.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using ModelServices.Enums;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using System.Runtime.Serialization;
    /// <summary>
    /// Modelo de negocio recargos
    /// </summary>
    public class ParamSurcharge : Component
    {
        /// <summary>
        /// Obtiene o establece abreviatura
        /// </summary>
        [DataMember]
        public string TinyDescription { get; set; }

        /// <summary>
        /// Obtiene o establece tipo tasa
        /// </summary>
        [DataMember]
        public RateType Type { get; set; }

        /// <summary>
        /// Obtiene o establece tasa
        /// </summary>
        [DataMember]
        public decimal Rate { get; set; }
    }
}
