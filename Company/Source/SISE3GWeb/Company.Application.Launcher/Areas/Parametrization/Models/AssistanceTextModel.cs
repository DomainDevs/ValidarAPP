// -----------------------------------------------------------------------
// <copyright file="AssistanceTextModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using Sistran.Core.Application.UnderwritingServices.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;


    /// <summary>
    /// Clase con el modelo (MVC) de textos de asistencia 
    /// </summary>
    public class AssistanceTextModel
    {
        /// <summary>
        /// Obtiene o establece Codigo de texto de asistencia
        /// </summary>
        [Range(0, 999, ErrorMessage = "Debe ser mayor a 0 y menor a 1000 ")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public int AssistanceTextId { get; set; }

        /// <summary>
        /// Obtiene o establece Texto o descripcion de asistencia
        /// </summary>
        [Display(Name = "Textos")]
        [StringLength(60)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public string Text { get; set; }

        /// <summary>
        /// Obtiene o establece Codigo de clausula 3G
        /// </summary>
        public int ClauseCd3G { get; set; }

        /// <summary>
        /// Obtiene o establece Codigo de clausula 2G
        /// </summary>
        public int ClauseCd2G { get; set; }

        /// <summary>
        /// Obtiene o establece Nombre o descripcion de clausula 3G
        /// </summary>
        [Display(Name = "Cláusulas")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string ClauseDescription { get; set; }

        /// <summary>
        /// Obtiene o establece Codigo de ramo comercial
        /// </summary>
        public int PrefixCd { get; set; }

        /// <summary>
        /// Obtiene o establece Codigo de tipo de asistencia
        /// </summary>
        public int AssistanceCd { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si, Estado de texto de asistencia
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion del estado de texto de asistencia
        /// </summary>
        public string EnabledDescription { get; set; }

        /// <summary>
        /// Obtiene o establece Clausula asociada a texto de asistencia
        /// </summary>
        public Clause Clause { get; set; }

        /// <summary>
        /// Obtiene o establece Estado de la accion (creado, modificado o eliminado)
        /// </summary>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        /// Obtiene o establece titulo del modal
        /// </summary>
        [DataMember]
        public string TitleDescription { get; set; }
    }
}