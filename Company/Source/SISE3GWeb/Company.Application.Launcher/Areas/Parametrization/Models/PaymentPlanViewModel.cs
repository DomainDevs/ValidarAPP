// -----------------------------------------------------------------------
// <copyright file="PaymentPlanViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;

    /// <summary>
    /// ModelView de Plan de pago
    /// </summary>
    public class PaymentPlanViewModel 
    {
        /// <summary>
        /// Obtiene o establece el Id de Plan de Pago
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece descripcion de Plan de Pago
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(50)]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece descripcion abreviada
        /// </summary>
        [Display(Name = "LabelDescriptionShort", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(15)]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad de Cuotas
        /// </summary>
        [Display(Name = "QuantityQuota", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FieldRequired")]        
        public int Quantity { get; set; }
        
        /// <summary>
        /// Obtiene o establece la cantidad de días hasta el vencimiento de la primera cuota
        /// </summary>
        [Display(Name = "NumberDaysExpirationQuota", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FieldRequired")]
        [Range(0, 180, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "RangeAllow")]
        public int FirstPayQuantity { get; set; }
        
        /// <summary>
        /// Obtiene o establece la mínima cantidad de días entre el vencimiento de la última cuota y la fecha de fin de vigencia
        /// </summary>
        [Display(Name = "MinimumDaysExpirationQuota", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FieldRequired")]
        [Range(0, 180, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "RangeAllow")]
        public int LastPayQuantity { get; set; }
       
        /// <summary>
        /// Obtiene o establece un valor que indica si las cuotas son apartir de la fecha de emision
        /// </summary>
        public bool IsIssueDate { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si las cuotas son apartir de la mayor de ambas
        /// </summary>
        public bool IsGreaterDate { get; set; }

        /// <summary>
        /// Obtiene o establece Unidad de tiempo
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FieldRequired")]
        public int GapUnit { get; set; }
        
        /// <summary>
        ///  Obtiene o establece las Cuotas
        /// </summary>
        public List<QuotaServiceModel> QuotasServiceModel { get; set; }
        
        /// <summary>
        ///  Obtiene o establece el Estado de item - (CRUD)
        /// </summary>
        public StatusTypeService StatusTypeService { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica Financiacion
        /// </summary>
        public bool Financing { get; set; }
    }
}