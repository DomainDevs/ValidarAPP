// -----------------------------------------------------------------------
// <copyright file="DelegationViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
using Sistran.Core.Application.ModelServices.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.ParamAuthorizationPolicies.Models
{
    /// <summary>
    /// Defines the <see cref="DelegationViewModel" />
    /// </summary>
    public class DelegationViewModel
    {   
            /// <summary>
            /// Obtiene o establece la Descripcion
            /// </summary>
            [StringLength(30, ErrorMessage = "Maximo 30 Caracteres")]
            [Display(Name = "LabelDescription", ResourceType = typeof(App_GlobalResources.Language))]
            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Required")]
            public string Description { get; set; }

            /// <summary>
            /// Obtiene o establece la jerarquia
            /// </summary>
            [Display(Name = "Hierarchy", ResourceType = typeof(App_GlobalResources.Language))]
            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Required")]
            public int Hierarchy { get; set; }

            /// <summary>
            /// Obtiene o establece un valor que indica si IsEnabled
            /// </summary>
            [Display(Name = "LabelAlliancePrintFormatIsEnable", ResourceType = typeof(App_GlobalResources.Language))]
            public bool IsEnabled { get; set; }

            /// <summary>
            /// Obtiene o establece un valor que indica si IsExclusionary
            /// </summary>
            [Display(Name = "Exclusion", ResourceType = typeof(App_GlobalResources.Language))]
            public bool IsExclusionary { get; set; }

            /// <summary>
            /// Obtiene o establece el modulo 
            /// </summary>
            [Display(Name = "LabelModule", ResourceType = typeof(App_GlobalResources.Language))]
            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Required")]
            public int Module { get; set; }

            /// <summary>
            /// Obtiene o establece el subModulo
            /// </summary>
            [Display(Name = "LabelSubmodule", ResourceType = typeof(App_GlobalResources.Language))]
            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Required")]
            public int SubModule { get; set; }

            /// <summary>
            /// Obtiene o establece estado
            /// </summary>
            public StatusTypeService StatusTypeService { get; set; }
        
    }
}