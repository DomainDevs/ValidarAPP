// -----------------------------------------------------------------------
// <copyright file="InfringementViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using Sistran.Core.Application.ModelServices.Enums;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo de vista para infracciones
    /// </summary>
    public class InfringementViewModel
    {
        [Display(Name = "LabelInfringementCode", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(10)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string InfringementCode { get; set; }

        [Display(Name = "LabelInfringementPreviousCode", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(0, 9999, ErrorMessage = "Debe ser mayor a 0 y menor a 10000 ")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public int? InfringementPreviousCode { get; set; }

        [Display(Name = "LabelInfringementDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(1000)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string InfringementDescription { get; set; }

        public int? InfringementGroupCode { get; set; }

        public string InfringementGroupDescription { get; set; }

        /// <summary>
        ///  Obtiene o establece el Estado de item - (CRUD)
        /// </summary>
        public StatusTypeService StatusTypeService { get; set; }
    }
}