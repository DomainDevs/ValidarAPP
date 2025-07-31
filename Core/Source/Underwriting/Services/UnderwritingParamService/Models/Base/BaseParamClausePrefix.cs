// -----------------------------------------------------------------------
// <copyright file="ParamClausePrefix.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Modelo Ramo comercial por clausula
    /// </summary>
    [DataContract]
    public class BaseParamClausePrefix: Extension
    {
        /// <summary>
        /// Obtiene o establece descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece pequeña descripcion
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
