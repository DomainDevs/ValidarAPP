// -----------------------------------------------------------------------
// <copyright file="DetailViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using Sistran.Core.Application.EntityServices.Enums;
    using System.ComponentModel.DataAnnotations;
    public class DetailViewModel
    {
        /// <summary>
        /// Tipo de detalle
        /// </summary>
        [Display(Name = "LabelDetailType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int DetailTypeId { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }

        /// <summary>
        /// Valor de la Suma
        /// </summary>            
        [Display(Name = "LabelSubLimitAmount", ResourceType = typeof(App_GlobalResources.Language))]       
        public decimal? SublimitAmt { get; set; }

        /// <summary>
        /// Tipo de Tasa
        /// </summary>  
        [Display(Name = "LabelRateType", ResourceType = typeof(App_GlobalResources.Language))]
        public int? RateTypeId { get; set; }


        /// <summary>
        /// Valor de la Tasa
        /// </summary>  
        [DisplayFormat(DataFormatString = "{{{0:### ##}}}")]
        [Display(Name = "LabelRate", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? Rate { get; set; }

        public bool Enabled { get; set; }
        public string EnabledDescription { get; set; }

        public string RateTypeDescription { get; set; }
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

       // public string Status { get; set; }
        public StatusTypeService Status { get; set; }
        public string TypeDescription { get; set; }
    }
}