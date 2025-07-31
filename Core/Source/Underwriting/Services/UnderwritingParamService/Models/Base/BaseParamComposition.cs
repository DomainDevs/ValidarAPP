// -----------------------------------------------------------------------
// <copyright file="BaseParamComposition.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andrés Clavijo</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System.Runtime.Serialization;

    /// <summary>
    /// Niveles de influencia (modelo)
    /// </summary>
    public class BaseParamComposition : Extension
    {
        /// <summary>
        /// Obtiene o establece id
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Obtiene o establece descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Obtiene o establece descripcion corta
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
