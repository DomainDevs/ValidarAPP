// -----------------------------------------------------------------------
// <copyright file="ParamClauseCoverage.cs" company="SISTRAN">
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
    /// Modelo de coberturas por clausula
    /// </summary>
    [DataContract]
    public class ParamClauseCoverage: BaseParamClauseCoverage
    {
        /// <summary>
        /// Obtiene o establece modelo Peril
        /// </summary>
        [DataMember]
        public Peril Peril { get; set; }

        /// <summary>
        /// Obtiene o establece modelo InsuredObject
        /// </summary>
        [DataMember]
        public ParamClauseInsuredObject ParamClauseInsuredObject { get; set; }
    }
}
