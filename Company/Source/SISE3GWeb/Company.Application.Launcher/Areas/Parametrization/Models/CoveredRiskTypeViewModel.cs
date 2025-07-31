// -----------------------------------------------------------------------
// <copyright file="CoveredRiskTypeViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo de las propiedades del tipo de riesgo cubierto.
    /// </summary>    
    public class CoveredRiskTypeViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id del tipo de riesgo cubierto
        /// </summary>
        [Range(0, 255, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorIdCoveredRiskType")]        
        public int Id { get; set; }
                
        /// <summary>
        /// Obtiene o establece la Descripción corta del tipo de riesgo cubierto.
        /// </summary>
        [Display(Name = "LabelDescriptionShort", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string ShortDescription { get; set; }     
    }
}