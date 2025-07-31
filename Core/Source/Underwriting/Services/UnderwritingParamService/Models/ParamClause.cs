// -----------------------------------------------------------------------
// <copyright file="ParamClause.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using System.Runtime.Serialization;
    /// <summary>
    /// Clausulas (modelo)
    /// </summary>
    public class ParamClause: BaseParamClause
    {
        /// <summary>
        /// Obtiene o establece clausulas
        /// </summary>
        [DataMember]
        public Clause Clause { get; set; }

        /// <summary>
        /// Obtiene o establece nivel de clausula
        /// </summary>
        [DataMember]
        public ParamClauseLevel ClauseLevel { get; set; }

        /// <summary>
        /// Obtiene o establece coberturas
        /// </summary>
        [DataMember]
        public ParamClauseCoverage ParamClauseCoverage { get; set; }
       
        /// <summary>
        /// Obtiene o establece ramo comercial
        /// </summary>
        [DataMember]
        public ParamClausePrefix ParamClausePrefix { get; set; }
        
        /// <summary>
        /// Obtiene o establece tipo de riesgo
        /// </summary>
        [DataMember]
        public RiskType RiskType { get; set; }

        /// <summary>
        /// Obtiene o establece textos
        /// </summary>
        [DataMember]
        public Text Text { get; set; }

        /// <summary>
        /// Obtiene o establece ramo tecnico
        /// </summary>
        [DataMember]
        public ParamClauseLineBusiness ParamClauseLineBusiness { get; set; }
    }
}
