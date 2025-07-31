using Sistran.Company.Application.ModelServices.Enums;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MOS = Sistran.Core.Application.UnderwritingServices.Models;
namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    /// <summary>
    /// Modelo de productos
    /// </summary>
    public class ProductModelsView
    {

        /// <summary>
        /// Identificador
        /// </summary>
        [Display(Name = "LabelProduct", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Id { get; set; }

        /// <summary>
        /// Ramo comercial
        /// </summary>
        [Display(Name = "LabelPrefix", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int PrefixId { get; set; }

        [StringLength(50)]
        [Display(Name = "LabelDescriptionProduct", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }

        [Display(Name = "LabelDescriptionProductReduced", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string DescriptionReduced { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "LabelCurrentDate", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public DateTime CurrentDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "LabelDisabledDate", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime? DisabledDate { get; set; }

        /// <summary>
        /// Porcentaje de Comision
        /// </summary>   
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "LabelPercentageCommissionNormal", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public decimal Percentage { get; set; }

        /// <summary>
        /// Porcentaje de Comision Adicional
        /// </summary>   
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "LabelPercentageCommissionAditional", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? PercentageAdditional { get; set; }

        /// <summary>
        /// Tipo de póliza
        /// </summary>
        [Display(Name = "LabelPolicyType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int PolicyType { get; set; }

        /// <summary>
        /// Tipo de Riesgo
        /// </summary>
        [Display(Name = "LabelRiskType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int RiskType { get; set; }

        /// <summary>
        /// Maximo numero de riesgos
        /// </summary>
        [Display(Name = "LabelMaximumNumberRisk", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int MaximumNumberRisk { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>
        [Display(Name = "LabelMoney", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Currency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCollective { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsGreen { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRequest { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFlatRate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPremium { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsRcAdditional { get; set; }

        /// <summary>
        /// Gets or sets the pre rule set identifier.
        /// </summary>
        /// <value>
        /// The pre rule set identifier.
        /// </value>
        public int? PreRuleSetId { get; set; }


        /// <summary>
        /// Gets or sets the rule set identifier.
        /// </summary>
        /// <value>
        /// The rule set identifier.
        /// </value>
        public int? RuleSetId { get; set; }

        /// <summary>
        /// Gets or sets the rule set identifier.
        /// </summary>
        /// <value>
        /// The rule set identifier.
        /// </value>
        public int? ScriptId { get; set; }

        /// <summary>
        /// Lista  Monedas
        /// </summary>
        public List<CurrencyModelsView> Currencies { get; set; }

        /// <summary>
        /// Lista tipos de poliza
        /// </summary>
        public List<PolicyTypeModelsView> PolicyTypes { get; set; }

        /// <summary>
        /// Lista tipos de riesgo
        /// </summary>
        public List<RiskTypesModelsView> ProductCoveredRisks { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public List<ProductAssistanceTypeModelsView> AssistanceType { get; set; }

        /// <summary>
        /// Gets or sets Lista Planes de Financiacion del Producto
        /// </summary>
        /// <value>
        /// The financial plans.
        /// </value>
        public List<ProductFinancialPlanModelsView> FinancialPlan { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        [Display(Name = "LabelVersion", ResourceType = typeof(App_GlobalResources.Language))]
        public int VersionId { get; set; }

        #region Company Product
        ///// <summary>
        ///// Producto 2G
        ///// </summary>
        //[Display(Name = "LabelProduct2G", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? Product2G { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPoliticalProduct { get; set; }

        /// <summary>
        /// Obtiene o establece la propiedad IncentiveAmount
        /// </summary>
        [Range(typeof(decimal), "0", "1000000", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "LabelIncentiveValue", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public decimal IncentiveAmount { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si Habilitar Incentivo esta activo
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Obtiene o establece la propiedad IsScore indica DataCrédito
        /// </summary>
        public bool? IsScore { get; set; }

        /// <summary>
        /// Obtiene o establece la propiedad IsFine indica Multas de Transito
        /// </summary>
        public bool? IsFine { get; set; }

        /// <summary>
        /// Obtiene o establece la propiedad IsFasecolda 
        /// </summary>        
        public bool? IsFasecolda { get; set; }
        #endregion

        /// <summary>
        /// Obtiene o establece los dias de vigencia de la cotizacion
        /// </summary>
        [Display(Name = "LabelProductValidDaysTempQuote", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(0, 10000, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        public int? ValidDaysTempQuote { get; set; }

        /// <summary>
        /// Obtiene o establece los dias de vigencia de la temporal
        /// </summary>

        [Display(Name = "LabelProductValidDaysTempPolicy", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(0, 10000, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        public int? ValidDaysTempPolicy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ///
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "LabelSurchargeCommissionPercentage", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? SurchargeCommissionPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// 
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "LabelAdditDisCommissPercentage", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? AdditDisCommissPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// 
        [Range(typeof(decimal), "0", "100", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Display(Name = "LabelStdDiscountCommPercentage", ResourceType = typeof(App_GlobalResources.Language))]
        public decimal? StdDiscountCommPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? DecrementCommisionAdjustFactorPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsUse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? IncrementCommisionAdjustFactorPercentage { get; set; }

        /// <summary>
        /// Estado del objeto
        /// </summary>        
        public StatusTypeService StatusTypeService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Emision para interactivo
        /// </summary>
        public bool IsInteractive { get; set; }

        /// <summary>
        /// Emision para masivos
        /// </summary>
        public bool IsMassive { get; set; }


    }
}
