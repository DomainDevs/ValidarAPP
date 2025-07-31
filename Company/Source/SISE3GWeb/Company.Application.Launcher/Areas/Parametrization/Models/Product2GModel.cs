// -----------------------------------------------------------------------
// <copyright file="Product2GModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using Sistran.Core.Application.CommonService.Models;

    /// <summary>
    /// Clase con el modelo (MVC) de producto 2G
    /// </summary>
    public class Product2GModel
    {
        /// <summary>
        /// Obtiene o establece codigo de producto 2G
        /// </summary>
        [Display(Name = "CodeProductId", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(1, 999, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorRangeProductId")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public int ProductId { get; set; }

        /// <summary>
        /// Obtiene o establece Codigo de ramo comercial
        /// </summary>
        public int PrefixCode { get; set; }

        /// <summary>
        /// Obtiene o establece descripcion de ramo comercial
        /// </summary>
        public string PrefixDescription { get; set; }

        /// <summary>
        /// Obtiene o establece descripcion de producto 2G
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece estado (creado, modificado o eliminado)
        /// </summary>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        /// Obtiene o establece ramo asociado al tipo de asistencia
        /// </summary>
        public Prefix Prefix { get; set; }
    }
}