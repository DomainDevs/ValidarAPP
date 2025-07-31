// -----------------------------------------------------------------------
// <copyright file="SurchargeViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using Application.ModelServices.Enums;

    /// <summary>
    /// Controlador para descuentos
    /// </summary>
    public class SurchargeViewModel
    {
        /// <summary>
        /// Obtiene o establece identificador
        /// </summary>
        public int Id { get; set; }
                
        /// <summary>
        /// Obtiene o establece Descripcion 
        /// </summary>
        [DataMember]
        [Display(Name = "LabelDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }
        
        /// <summary>
        /// Obtiene o establece Se obtiene Abreviatura
        /// </summary>
        [DataMember]
        [Display(Name = "LabelAbbreviation", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(3)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]  
        public string TinyDescription { get; set; }

         /// <summary>
        /// Obtiene o establece Tipo de cálculo 
        /// </summary>
        [DataMember]
        public string CalculationType { get; set; }
        
        /// <summary>
        /// Obtiene o establece Tipo de tasa
        /// </summary>
        [DataMember]
        [Display(Name = "LabelRate", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,6})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Range(1, 100, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FileStringHundred")]
        public string Rate { get; set; }

        /// <summary>
        /// Obtiene o establece Tipo de tasa
        /// </summary>
        [DataMember]
        [Display(Name = "LabelRate", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,6})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Range(1, 1000, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FileStringMil")]
        public string RateM { get; set; }

        /// <summary>
        /// Obtiene o establece Tipo de tasa
        /// </summary>
        [DataMember]
        [Display(Name = "LabelRate", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,6})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Range(1, 10000000000000, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FileStringCero")]
        public string RateI { get; set; }

        /// <summary>
        /// Obtiene o establece Tipo 
        /// </summary>
        [DataMember]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? Type { get; set; }

        /// <summary>
        /// Obtiene o establece Tipo de tasa
        /// </summary>
        public string RateDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el Estado de item - (CRUD)
        /// </summary>
        public StatusTypeService StatusTypeService { get; set; }
    }
}