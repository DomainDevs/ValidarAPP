// -----------------------------------------------------------------------
// <copyright file="InfringementGroupViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using Application.EntityServices.Enums;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo de vista para grupo de infracciones
    /// </summary>
    public class InfringementGroupViewModel
    {
        public int InfringementGroupCode { get; set; }

        [Display(Name = "LabelInfringementGroupDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(60, ErrorMessageResourceName = "ErrorStringLengthInfringementGroupDescription", ErrorMessageResourceType = typeof(App_GlobalResources.Language))]
        public string Description { get; set; }

        [Display(Name = "LabelDaysCountPeriodOne", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Range(0, 9999, ErrorMessageResourceName = "ErrorRangeInfrigement", ErrorMessageResourceType = typeof(App_GlobalResources.Language))]
        public int InfrigementOneYear { get; set; }

        [Display(Name = "LabelDaysCountPeriodTwo", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Range(0, 9999, ErrorMessageResourceName = "ErrorRangeInfrigement", ErrorMessageResourceType = typeof(App_GlobalResources.Language))]
        public int InfrigementThreeYear { get; set; }

        public bool IsActive { get; set; }

        /// <summary>
        ///  Obtiene o establece el Estado de item - (CRUD)
        /// </summary>
        public StatusTypeService StatusTypeService { get; set; }
    }
}