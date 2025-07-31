// -----------------------------------------------------------------------
// <copyright file="CoverageServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    [DataContract]
    public class CoverageServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el identificador de la cobertura
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción de la cobertura
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el id del ramo tecnico
        /// </summary>
        [DataMember]
        public LineBusinessServiceQueryModel LineBusiness { get; set; }

        /// <summary>
        /// Obtiene o establece el id del subramo tecnico
        /// </summary>
        [DataMember]
        public SubLineBusinessServiceQueryModel SubLineBusiness { get; set; }

        /// <summary>
        /// Obtiene o establece el id del amparo
        /// </summary>
        [DataMember]
        public PerilServiceQueryModel Peril { get; set; }

        /// <summary>
        /// Obtiene o establece el id del objeto del seguro
        /// </summary>
        [DataMember]
        public InsuredObjectServiceQueryModel InsuredObject { get; set; }

        /// <summary>
        /// Obtiene o establece el id del nivel del influencia
        /// </summary>
        [DataMember]
        public int? CompositionTypeId { get; set; }

        /// <summary>
        /// Obtiene o establece si la cobertura es principal
        /// </summary>
        [DataMember]
        public bool IsPrincipal { get; set; }

        /// <summary>
        /// Obtiene o establece la relacion con la CoCoverage Principal
        /// </summary>
        [DataMember]
        public CoCoverageServiceModel CoCoverageServiceModel { get; set; }

        /// <summary>
        /// Obtiene o establece la relacion con las CoCoverage Hijos
        /// </summary>
        [DataMember]
        public List<CoCoverageServiceModel> CoCoverageServiceModels { get; set; }

        /// <summary>
        /// Obtiene o establece las clausulas relacionados con la cobertura
        /// </summary>
        [DataMember]
        public ClausesServiceQueryModel ClausesServiceQueryModel { get; set; }

        /// <summary>
        /// Obtiene o establece los deducibles relacionados con la cobertura
        /// </summary>
        [DataMember]
        public DeductiblesServiceQueryModel DeductiblesServiceQueryModel { get; set; }

        /// <summary>
        /// Obtiene o establece los tipos de detalle relacionados con la cobertura
        /// </summary>
        [DataMember]
        public DetailTypesServiceQueryModel DetailTypesServiceQueryModel { get; set; }

        /// <summary>
        /// Obtiene o establece la covertura de homologacion de 2G
        /// </summary>
        [DataMember]
        public Coverage2GServiceModel Homologation2G { get; set; }
    }
}
