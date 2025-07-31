// -----------------------------------------------------------------------
// <copyright file="ParametroViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase con el modelo (MVC) de Varible
    /// </summary>
    public class ParametroViewModel
    {
        /// <summary>
        /// Obtiene o establece valor del parametro
        /// </summary>
        [Display(Name = "Valor")]
        [Range(0, 9999, ErrorMessage = "Valor númerico Mayor a 0 y menor a 9999.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public int Value { get; set; }

        /// <summary>
        /// Obtiene o establece descripcion del parametro
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(60)]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el id del parametro
        /// </summary>
        public int ParameterId { get; set; }

        /// <summary>
        /// Obtiene o establece Estado (creado, modificado o eliminado)
        /// </summary>
        [DataMember]
        public string Status { get; set; }
    }
}