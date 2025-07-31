// -----------------------------------------------------------------------
// <copyright file="CoveredRiskTypeServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Alberto Sánchez Lesmes</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using System.Runtime.Serialization;
    using Sistran.Core.Application.ModelServices.Models.Param;

    /// <summary>
    /// Modelo de servicio de los tipos de riesgo cubierto.
    /// </summary>
    [DataContract]
    public class CoveredRiskTypeServiceRelationModel : ParametricServiceModel
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
