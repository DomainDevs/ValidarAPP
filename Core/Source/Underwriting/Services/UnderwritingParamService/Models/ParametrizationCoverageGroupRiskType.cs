// -----------------------------------------------------------------------
// <copyright file="ParametrizationCoverageGroupRiskType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using System.Runtime.Serialization;
    /// <summary>
    /// Grupos de cobertura (Modelo del negocio)
    /// </summary>
    [DataContract]
    public class ParametrizationCoverageGroupRiskType: BaseParametrizationCoverageGroupRiskType
    {
        
        /// <summary>
        /// Atributo para la propiedad IdCoverageRiskType
        /// </summary> 
        [DataMember]
        public ParamCoveredRiskType CoverageRiskType { get; set; }
              
    }
}