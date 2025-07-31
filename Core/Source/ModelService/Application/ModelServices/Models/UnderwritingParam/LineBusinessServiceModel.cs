// -----------------------------------------------------------------------
// <copyright file="LineBusinessServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Fernando Méndez Cardoso</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio para ramo técnico
    /// </summary>
    [DataContract]
    public class LineBusinessServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece la descripción del ramo técnico
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el id del ramo técnico
        /// </summary>
        [DataMember]
        public int Id { get; set; }

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

        /// <summary>
        /// Obtiene o establece los amparos del ramo técnico
        /// </summary>
        [DataMember]
        public List<PerilServiceModel> Perils { get; set; }

        /// <summary>
        /// Obtiene o establece los objetos del seguro del ramo técnico
        /// </summary>
        [DataMember]
        public List<InsuredObjectServiceModel> InsuredObjects { get; set; }

        /// <summary>
        /// Obtiene o establece las clausulas del ramo técnico
        /// </summary>
        [DataMember]
        public List<ClauseServiceModel> Clauses { get; set; }

        /// <summary>
        /// Obtiene o establece los tipos de riesgo del ramo técnico
        /// </summary>
        [DataMember]
        public List<int> CoveredRiskTypes { get; set; }

        /// <summary>
        /// Obtiene o establece los tipos de riesgo del ramo técnico modelo
        /// </summary>
        [DataMember]
        public List<CoveredRiskTypeServiceRelationModel> CoveredRiskTypeServiceModel { get; set; }
    }
}
