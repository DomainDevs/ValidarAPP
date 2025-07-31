// -----------------------------------------------------------------------
// <copyright file="ParamBusinessLine.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Méndez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.UnderwritingServices.Models;

    /// <summary>
    /// Ramo técnico
    /// </summary>
    [DataContract]
    public class BaseParamLineBusiness: Extension
    {
        /// <summary>
        /// Obtiene o establece la descripción del ramo técnico
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el id del ramo técnico
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción corta del ramo técnico
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la abreviatura del ramo técnico
        /// </summary>
        [DataMember]
        public string TinyDescription { get; set; }
        
    }
}
