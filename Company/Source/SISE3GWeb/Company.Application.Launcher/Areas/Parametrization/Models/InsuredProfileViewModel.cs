// -----------------------------------------------------------------------
// <copyright file="InsuredProfileViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo de las propiedades del Perfil de Asegurado.
    /// </summary>    
    public class InsuredProfileViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id del perfil de asegurado
        /// </summary>
        [Range(0, 255, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorIdInsuredProfile")]        
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripción larga la  del perfil de asegurado
        /// </summary>
        [Display(Name = "LabelDescriptionLong", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string LongDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripción corta del perfil de asegurado.
        /// </summary>
        [Display(Name = "LabelDescriptionShort", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string ShortDescription { get; set; }     
    }
}