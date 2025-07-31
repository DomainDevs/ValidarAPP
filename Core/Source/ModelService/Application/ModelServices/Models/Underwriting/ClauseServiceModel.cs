// -----------------------------------------------------------------------
// <copyright file="ClauseServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo para Clausula
    /// </summary>
    [DataContract]
    public class ClauseServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Nombre
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece Titulo
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Obtiene o establece Clausula
        /// </summary>
        [DataMember]
        public string ClauseText { get; set; }

        /// <summary>
        /// Obtiene o establece Fecha Inicio
        /// </summary>
        [DataMember]
        public DateTime InputStartDate { get; set; }

        /// <summary>
        /// Obtiene o establece Fecha Vencimiento
        /// </summary>
        [DataMember]
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Obtiene o establece Nivel De Condición
        /// </summary>
        [DataMember]
        public ConditionLevelServiceModel ConditionLevelServiceQueryModel { get; set; }

        /// <summary>
        /// Obtiene o establece modelo clauseLevel
        /// </summary>
        [DataMember]
        public ClauseLevelServiceModel ClauseLevelServiceModel { get; set; }

        /// <summary>
        /// Obtiene o establece Modelo cobertura
        /// </summary>
        [DataMember]
        public CoverageClauseServiceModel CoverageServiceQueryModel { get; set; }

        /// <summary>
        /// Obtiene o establece modelo tipo de riesgo
        /// </summary>
        [DataMember]
        public RiskTypeServiceModel RiskTypeServiceQueryModel { get; set; }

        /// <summary>
        /// Obtiene o establece modelo ramo comercial
        /// </summary>
        [DataMember]
        public PrefixServiceQueryModel PrefixServiceQueryModel { get; set; }

        [DataMember]
        public LineBusinessServiceQueryModel LineBusinessServiceQueryModel { get; set; }
    }
}
