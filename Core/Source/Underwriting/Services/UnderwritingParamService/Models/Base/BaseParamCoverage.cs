// -----------------------------------------------------------------------
// <copyright file="ParamCoverage.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de negocio de cobertura
    /// </summary>
    [DataContract]
    public class BaseParamCoverage: Extension
    {
        /// <summary>
        /// Obtiene o establece el id de la cobertura
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        
        /// <summary>
        /// Obtiene o establece la descripción de la cobertura
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        
        /// <summary>
        /// Obtiene o establece el id del nivel del influencia
        /// </summary>
        [DataMember]
        public int? CompositionTypeId { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cobertura es principal
        /// </summary>
        [DataMember]
        public bool IsPrincipal { get; set; }

    }
}
