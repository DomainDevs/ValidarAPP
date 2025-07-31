// -----------------------------------------------------------------------
// <copyright file="DelegationViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// -----------------------------------------------------------------------



namespace Sistran.Core.Framework.UIF.Web.Areas.ParamAuthorizationPolicies.Models
{
    using Sistran.Company.Application.ModelServices.Enums;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="BaseRejectionCausesViewModel" />
    /// </summary>
    public class BaseRejectionCausesViewModel
    {
        /// <summary>
        /// Obtiene o establece Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripcion
        /// </summary>
        [StringLength(30, ErrorMessage = "Maximo 30 Caracteres")]
        [Display(Name = "LabelDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FieldRequired")]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece grupo de politica
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "OptionSelect")]
        public BaseGroupPoliciesViewModel  GroupPolicies  { get; set;}

        public StatusTypeService StatusTypeService { get; set; }
    }
}