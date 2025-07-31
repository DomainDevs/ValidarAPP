// -----------------------------------------------------------------------
// <copyright file="AllianceSalesPointsView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    /// <summary>
    /// Modelo de vista para puntos de venta aliados
    /// </summary>
    public class AllianceSalesPointsViewModel
    {
        /// <summary>
        /// Gets or sets Identificador del punto de venta
        /// </summary>
        [Display(Name = "LabelSalesPointCode", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int SalePointId { get; set; }

        /// <summary>
        /// Gets or sets Descripción del punto de venta
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string SalePointDescription { get; set; }

        /// <summary>
        /// Gets or sets Estado.
        /// </summary>
        public string Status { get; set; }
    }
}