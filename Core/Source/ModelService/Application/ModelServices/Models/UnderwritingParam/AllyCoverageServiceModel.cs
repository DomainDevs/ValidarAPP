// -----------------------------------------------------------------------
// <copyright file="AllyCoverageServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Alberto Sánchez Lesmes</author>
// -----------------------------------------------------------------------
using Sistran.Core.Application.ModelServices.Models.Param;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    /// <summary>
    /// Cobertura Aliada
    /// </summary>
    [DataContract]
    public class AllyCoverageServiceModel: ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el id de la Cobertura Aliada
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        
        /// <summary>
        /// Obtiene o establece la descripcion de la Cobertura Aliada
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        
        /// <summary>
        /// Obtiene o establece el porcentaje de la Cobertura Aliada
        /// </summary>
        [DataMember]
        public decimal? AlliedCoveragePercentage { get; set; }
    }
}
