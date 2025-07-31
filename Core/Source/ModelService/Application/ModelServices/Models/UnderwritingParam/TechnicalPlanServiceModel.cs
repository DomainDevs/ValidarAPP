// -----------------------------------------------------------------------
// <copyright file="TechnicalPlanServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Alberto Sánchez Lesmes</author>
// -----------------------------------------------------------------------
using Sistran.Core.Application.ModelServices.Models.Param;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    [DataContract]
    public class TechnicalPlanServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Id del Plan Técnico.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del Plan Técnico.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Obtiene o establece la descripción Corta del Plan Técnico.
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
        /// <summary>
        /// Obtiene o establece el tipo de riesgo.
        /// </summary>
        [DataMember]
        public CoveredRiskTypeServiceQueryModel CoveredRiskType { get; set; }
        /// <summary>
        /// Obtiene o establece la fecha de Creación del Plan Técnico.
        /// </summary>
        [DataMember]
        public System.DateTime CurrentFrom { get; set; }
        /// <summary>
        /// Obtiene o establece la fecha de Finalización del Plan Técnico.
        /// </summary>
        [DataMember]
        public System.DateTime? CurrentTo { get; set; }
        /// <summary>
        /// Obtiene o establece la Lista de Coberturas.
        /// </summary>
        [DataMember]
        public List<TechnicalPlanCoverageServiceRelationModel> TechnicalPlanCoverages { get; set; }
    }
}
