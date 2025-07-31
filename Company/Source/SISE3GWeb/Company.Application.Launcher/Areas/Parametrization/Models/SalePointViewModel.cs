// -----------------------------------------------------------------------
// <copyright file="BranchViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    //using Application.EntityServices.Enums;
    using System.ComponentModel.DataAnnotations;
    using Sistran.Core.Application.ModelServices.Enums;

    /// <summary>
    /// Modelo de objetos de seguro
    /// </summary>    
    public class SalePointViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id 
        /// </summary>
        [Display(Name = "LabelSalesPointCode", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
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
        /// Obtiene o establece la sucursal
        /// </summary>      
        [Display(Name = "ErrorDocumentBranch", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Branch { get; set; }

        /// <summary>
        /// Obtiene o establece el id de la sucursal
        /// </summary>       
        public int BranchId { get; set; }
        /// <summary>
        /// Obtiene o establece el status 
        /// </summary>
        public StatusTypeService Status { get; set; }

        /// <summary>
        /// Obtiene o establece el Enabled
        /// </summary>
        public bool Enabled { get; set; }
    }
}