// -----------------------------------------------------------------------
// <copyright file="DelegationViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// -----------------------------------------------------------------------


namespace Sistran.Core.Framework.UIF.Web.Areas.ParamAuthorizationPolicies.Models
{

    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="BaseGroupPoliciesViewModel" />
    /// </summary>
    public class BaseGroupPoliciesViewModel
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
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Required")]
        public string Description { get; set; }
    }
}