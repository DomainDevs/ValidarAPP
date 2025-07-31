// -----------------------------------------------------------------------
// <copyright file="BusinessTypeViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>"@Etriana</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// ModelView. Modelo de la vista del Tipo de Negocios.
    /// </summary>    
    public class BusinessTypeViewModel
    {
        /// <summary>
        /// Obtiene o establece el BUSINESS_TYPE_CD del Tipo de Negocios
        /// </summary>
        [Range(0, 255, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Error BUSINESS_TYPE_CD_BusinessType")]        
        public int BUSINESS_TYPE_CD { get; set; }

        /// <summary>
        /// Obtiene o establece la SMALL_DESCRIPTION corta del Tipo de Negocios.
        /// </summary>
        [Display(Name = "SMALL_DESCRIPTION", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string SMALL_DESCRIPTION { get; set; }     
    }
}