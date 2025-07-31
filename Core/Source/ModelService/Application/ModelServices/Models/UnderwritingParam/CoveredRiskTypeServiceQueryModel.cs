// -----------------------------------------------------------------------
// <copyright file="CoveredRiskTypeServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Alberto Sánchez Lesmes</author>
// -----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    /// <summary>
    /// Modelo de servicio de los tipos de riesgo cubierto.
    /// </summary>
    [DataContract]
    public class CoveredRiskTypeServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece Id del tipo de riesgo cubierto.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripción del tipo de riesgo cubierto.
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
