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
    public class DiscountViewModel
    {
        /// <summary>
        /// Gets or sets Obtiene identificador
        /// </summary>
        public int Id { get; set; }
               
        [DataMember]
        [Display(Name = "LabelDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]

        /// <summary>
        /// Gets or sets Descripcion de
        /// </summary>
        public string Description { get; set; }
              
        [DataMember]
        [Display(Name = "LabelAbbreviation", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(3)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]

        /// <summary>
        /// Gets or sets Se obtiene Abreviatura
        /// </summary>
        public string TinyDescription { get; set; }

        /// <summary>
        /// Gets or sets Tipo de cálculo 
        /// </summary>
        [DataMember]
        public string CalculationType { get; set; }
                
        [DataMember]
        [Display(Name = "LabelRate", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,6})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Range(1, 100, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FileLengthHundred")]

        /// <summary>
        /// Gets or sets Tipo de tasa
        /// </summary>
        public string Rate { get; set; }

        [DataMember]
        [Display(Name = "LabelRate", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,6})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Range(1, 1000, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FileLengthMil")]

        /// <summary>
        /// Gets or sets Tipo de tasa
        /// </summary>
        public string RateM { get; set; }

        [DataMember]
        [Display(Name = "LabelRate", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,6})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Range(1, 10000000000000, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FileLengthZero")]

        /// <summary>
        /// Gets or sets Tipo de tasa
        /// </summary>
        public string RateI { get; set; }

        [DataMember]      
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]

        /// <summary>
        /// Gets or sets Tipo de tasa
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets Tipo de tasa
        /// </summary>
        public string RateDescription { get; set; }

        /// <summary>
        ///  Obtiene o establece el Estado de item - (CRUD)
        /// </summary>
        public StatusTypeService StatusTypeService { get; set; }
    }
}