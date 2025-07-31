// -----------------------------------------------------------------------
// <copyright file="InsurencesObjectsViewModel.cs" company="SISTRAN">
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
    public class InsurencesObjectsViewModel
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
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece la Description corta
        /// </summary>
        [StringLength(15)]
        [Display(Name = "LabelShortDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es declarativo
        /// </summary>
        public bool IsDeraclarative { get; set; }

        /// <summary>
        /// Obtiene o establece la Declarative Description 
        /// </summary>
        public string DeclarativeDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el status 
        /// </summary>
        public int Status { get; set; }
    }
}