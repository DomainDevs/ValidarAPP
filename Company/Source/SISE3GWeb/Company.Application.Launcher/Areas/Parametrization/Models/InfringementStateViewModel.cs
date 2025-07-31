// -----------------------------------------------------------------------
// <copyright file="InfringementStateViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using Application.EntityServices.Enums;
    using System.ComponentModel.DataAnnotations;

    public class InfringementStateViewModel
    {
        public int InfringementStateCode { get; set; }

        [Display(Name = "LabelInfringementStateDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(30)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string InfringementStateDescription { get; set; }

        /// <summary>
        ///  Obtiene o establece el Estado de item - (CRUD)
        /// </summary>
        public StatusTypeService StatusTypeService { get; set; }
    }
}