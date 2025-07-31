// -----------------------------------------------------------------------
// <copyright file="ParametrizationCoverageGroupRiskType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Collections.Generic;
    using System.Runtime.Serialization;    

    /// <summary>
    /// Grupos de cobertura (Modelo del negocio)
    /// </summary>
    [DataContract]
    public class BaseParametrizationCoverageGroupRiskType: Extension
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