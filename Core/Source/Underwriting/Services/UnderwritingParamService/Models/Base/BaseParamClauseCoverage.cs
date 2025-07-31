// -----------------------------------------------------------------------
// <copyright file="ParamClauseCoverage.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.UnderwritingServices.Models;

    /// <summary>
    /// Modelo de coberturas por clausula
    /// </summary>
    [DataContract]
    public class BaseParamClauseCoverage: Extension
    {
        /// <summary>
        /// Obtiene o establece Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

    }
}
