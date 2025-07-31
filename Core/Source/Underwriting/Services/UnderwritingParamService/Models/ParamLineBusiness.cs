// -----------------------------------------------------------------------
// <copyright file="ParamBusinessLine.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Méndez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    /// <summary>
    /// Ramo técnico
    /// </summary>
    [DataContract]
    public class ParamLineBusiness: BaseParamLineBusiness
    {
        

        /// <summary>
        /// Obtiene o establece los amparos del ramo técnico
        /// </summary>
        [DataMember]
        public List<Peril> Perils { get; set; }

        /// <summary>
        /// Obtiene o establece los objetos del seguro del ramo técnico
        /// </summary>
        [DataMember]
        public List<ParamInsuredObjectDesc> InsuredObjects { get; set; }

        /// <summary>
        /// Obtiene o establece las clausulas del ramo técnico
        /// </summary>
        [DataMember]
        public List<Clause> Clauses { get; set; }

        /// <summary>
        /// Obtiene o establece los tipos de riesgo del ramo técnico
        /// </summary>
        [DataMember]
        public List<int> CoveredRiskTypes { get; set; }
    }
}
