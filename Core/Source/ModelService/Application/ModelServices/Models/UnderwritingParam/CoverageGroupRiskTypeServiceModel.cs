// -----------------------------------------------------------------------
// <copyright file="CoverageGroupRiskTypeServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de grupo de coberturas
    /// </summary>
    [DataContract]
    public class CoverageGroupRiskTypeServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Atributo para la propiedad IdCoverGroupRisk
        /// </summary> 
        [DataMember]
        public int IdCoverGroupRisk { get; set; }

        /// <summary>
        /// Atributo para la propiedad IdCoverageGroup
        /// </summary> 
        [DataMember]
        public int IdCoverageGroup { get; set; }

        /// <summary>
        /// Atributo para la propiedad IdCoverageRiskType
        /// </summary> 
        [DataMember]
        public CoveredRiskTypeServiceModel CoverageRiskType { get; set; }

        /// <summary>
        /// Atributo para la propiedad Description
        /// </summary> 
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Atributo para la propiedad SmallDescription
        /// </summary> 
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Atributo para la propiedad Enabled
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }
    }
}
