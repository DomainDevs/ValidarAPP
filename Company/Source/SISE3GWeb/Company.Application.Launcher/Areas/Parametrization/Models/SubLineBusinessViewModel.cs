// -----------------------------------------------------------------------
// <copyright file="TechnicalSubBranchViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sistran.Core.Application.EntityServices.Enums;
   
    /// <summary>
    /// ModelView de SubRamoTecnico
    /// </summary>
    public class SubLineBusinessViewModel
    {
        /// <summary>
        /// Obtiene o establece Id de SubRamo Tecnico
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion del largo del SubRamo tecnico
        /// </summary>
        [Display(Name = "LabelDescriptionLong", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion del corto del SubRamo tecnico
        /// </summary>
        [Display(Name = "LabelDescriptionShort", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece Nombre SubRamo Técnico
        /// </summary>
        [Display(Name = "NameTechnicalSubBranch", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(50)]
        public string LineBusinessDescription { get; set; }

        /// <summary>
        /// Obtiene o establece Id de Ramo Tecnico
        /// </summary>
        public int LineBusinessId { get; set; }

        /// <summary>
        /// Obtiene o establece estado
        /// </summary>
        public StatusTypeService? StatusTypeService { get; set; }
    }
}