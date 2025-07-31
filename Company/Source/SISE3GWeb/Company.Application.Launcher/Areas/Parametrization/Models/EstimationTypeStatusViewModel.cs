// -----------------------------------------------------------------------
// <copyright file="DiscountViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using Application.ModelServices.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Controlador para descuentos
    /// </summary>
    public class EstimationTypeStatusViewModel
    {
        /// <summary>
        /// Gets or sets Obtiene identificador
        /// </summary>
        public int Id { get; set; }
               
        [DataMember]
        [Display(Name = "LabelDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(100)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]

        /// <summary>
        /// Gets or sets Descripcion de
        /// </summary>
        public string Description { get; set; }
              
        [DataMember]
        [Display(Name = "LabelAbbreviation", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(100)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]

        /// <summary>
        /// Gets or sets Se obtiene Abreviatura
        /// </summary>
        public string InternalCode { get; set; }

        /// <summary>
        /// Gets or sets Obtiene identificador
        /// </summary>
        public int EstimationTypeCode { get; set; }

    }
}