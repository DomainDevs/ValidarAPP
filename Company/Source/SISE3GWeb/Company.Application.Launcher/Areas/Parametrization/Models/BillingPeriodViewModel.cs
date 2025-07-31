// -----------------------------------------------------------------------
// <copyright file="BillingPeriodViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>"@Etriana</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// ModelView. Modelo de las Vista periodos de facturacion.
    /// </summary>    
    public class BillingPeriodViewModel
    {
        /// <summary>
        /// Obtiene o establece el BILLING_PERIOD_CD del periodo de facturacion
        /// </summary>
        [Range(0, 255, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "Error BILLING_PERIOD_CD_BillingPeriod")]        
        public int BILLING_PERIOD_CD { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripción corta del periodo de facturacion.
        /// </summary>
        [Display(Name = "DESCRIPTION", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string DESCRIPTION { get; set; }     
    }
}