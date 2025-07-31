// -----------------------------------------------------------------------
// <copyright file="ParamClauseLevel.cs" company="SISTRAN">
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
    /// Modelo para niveles de clausula
    /// </summary>
    [DataContract]
    public class BaseParamClauseLevel: Extension
    {
        /// <summary>
        /// Obtiene o establece nivel de clausula
        /// </summary>
        [DataMember]
        public int ClauseLevelId { get; set; }

        /// <summary>
        /// Obtiene o establece codigo clausula
        /// </summary>
        [DataMember]
        public int ClauseId { get; set; }

        /// <summary>
        /// Obtiene o establece codigo condicion del nivel
        /// </summary>
        [DataMember]
        public int? ConditionLevelId { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es obligatorio
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }
    }
}
