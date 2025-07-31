// -----------------------------------------------------------------------
// <copyright file="BranchViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo de objetos de seguro
    /// </summary>    
    public class BranchViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la Description 
        /// </summary>
        [StringLength(50)]
        [Display(Name = "LabelLongDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string LongDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la Description corta
        /// </summary>
        [StringLength(15)]
        [Display(Name = "LabelShortDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el status 
        /// </summary>
        public bool Is_issue { get; set; }

        /// <summary>
        /// Obtiene o establece el status 
        /// </summary>
        public int Status { get; set; }
    }
}