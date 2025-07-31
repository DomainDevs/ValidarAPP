// -----------------------------------------------------------------------
// <copyright file="AssistanceTypeModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.ModelServices.Enums;

    /// <summary>
    /// Clase con el modelo (MVC) de tipos de asistencia 
    /// </summary>
    public class AssistanceTypeModel
    {
        /// <summary>
        /// Obtiene o establece Codigo tipo de asistencia
        /// </summary>
        [Range(0, 999, ErrorMessage = "Debe ser mayor a 0 y menor a 1000 ")]
        [Display(Name = "AssistanceCode", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public int AssistanceCode { get; set; }

        /// <summary>
        /// Obtiene o establece descripcion de asistencia
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(60)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si, estado de texto de asistencia
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Obtiene o establece estado de texto de asistencia
        /// </summary>
        public string EnabledDescription { get; set; }

        /// <summary>
        /// Obtiene o establece codigo de ramo comercial
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int PrefixCode { get; set; }

        /// <summary>
        /// Obtiene o establece descripcion del ramo comercial
        /// </summary>
        public string PrefixDescription { get; set; }

        /// <summary>
        /// Obtiene o establece ramo asociado al tipo de asistencia
        /// </summary>
        public Prefix Prefix { get; set; }

        /// <summary>
        /// Obtiene o establece Estado (creado, modificado o eliminado)
        /// </summary>
        [DataMember]
        public StatusTypeService Status { get; set; }
    }
}