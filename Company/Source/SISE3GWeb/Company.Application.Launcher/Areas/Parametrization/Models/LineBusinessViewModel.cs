// -----------------------------------------------------------------------
// <copyright file="TechnicalBranchViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Méndez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo del Ramo Técnico
    /// </summary>
    [DataContract]
    public class LineBusinessViewModel
    {
        /// <summary>
        /// Obtiene o establece el Identificador del Ramo Técnico
        /// </summary>
        [Display(Name = "LabelTechnicalBranchCode", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Range(1, 999, ErrorMessageResourceName = "ErrorRangeProductId", ErrorMessageResourceType = typeof(App_GlobalResources.Language))]
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el Descripcion del largo del Ramo tecnico
        /// </summary>
        [Display(Name = "LabelDescriptionLong", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorLabelLength")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public string LongDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el Descripcioon corta del ramo tecnico
        /// </summary>
        [Display(Name = "LabelDescriptionShort", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorLabelLength")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la Abreviatura
        /// </summary>
        [Display(Name = "LabelAbbreviation", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(3, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorLabelLength")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        [DataMember]
        public string TyniDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de Riesgo cubierto
        /// </summary>
        [Display(Name = "MessageRiskType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public int RiskTypeId { get; set; }

        /// <summary>
        /// Obtiene o establece la Lista de Tipos de riesgo para el ramo tecnico
        /// </summary>
        [DataMember]
        public List<int> CoveredRiskTypes { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripcion de ramo tecnico
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es actualización
        /// </summary>
        [DataMember]
        public bool Update { get; set; }
    }
}