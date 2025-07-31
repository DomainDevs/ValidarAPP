// -----------------------------------------------------------------------
// <copyright file="CoverageGroupViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using Sistran.Core.Application.EntityServices.Enums;
    using System.ComponentModel.DataAnnotations;
    public class CoverageGroupViewModel
    {
        /// <summary>
        /// Descripción del grupo
        /// </summary>
        [Display(Name = "LabelNameCoverage", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }

        /// <summary>
        /// Descripcion Corta
        /// </summary>
        [Display(Name = "LabelAbbreviation", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Descripción del tipo
        /// </summary>
        public string RiskTypeCoverageDescription { get; set; }

        /// <summary>
        /// Id del tipo de riesgo
        /// </summary>
        [Display(Name = "RiskTypeCoverage", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int CoveredRiskTypeCode { get; set; }     
        public int CoverageGroupCode { get; set; }
        public int IdCoverGroupRisk { get; set; }
        public bool Enabled { get; set; }
        public string EnabledDescription { get; set; }

        public StatusTypeService Status { get; set; }

    }
}