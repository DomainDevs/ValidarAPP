using System;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class InsuredObjectViewModel
    {
        /// <summary>
        /// Id de Temporal
        /// </summary>
        public int TemporalId { get; set; }

        /// <summary>
        /// Id de Riesgo
        /// </summary>
        public int RiskId { get; set; }

        /// <summary>
        /// Id Grupo de Cobertura
        /// </summary>
        public int CoverageGroup { get; set; }

        /// <summary>
        /// Id de cobertura
        /// </summary>     
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentCoverage")]
        public int CoverageId { get; set; }
        /// <summary>
        /// Id de Producto
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Id del objeto de la cobertura
        /// </summary>     
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentCoverage")]
        public int InsuredObjectId { get; set; }


        /// <summary>
        ///Obtiene o Setea La fecha desde poliza
        /// </summary>
        /// <value>
        /// fecha desde poliza
        /// </value>
        public string CurrentFromPolicy { get; set; }

        /// <summary>
        /// Obtiene o Setea La fecha hasta poliza
        /// </summary>
        /// <value>
        /// Tfecha hasta poliza
        /// </value>
        public string CurrentToPolicy { get; set; }
        /// <summary>

        /// <summary>
        /// Valor declarado
        /// </summary>          
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Display(Name = "LabelDeclaredValue", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDeclaredValue")]
        public decimal DeclaredValue { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Fecha Desde Vigencia
        /// </summary>     
        [Display(Name = "LabelFrom", ResourceType = typeof(App_GlobalResources.Language))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Fecha Hasta Vigencia
        /// </summary>  
        [Display(Name = "LabelTo", ResourceType = typeof(App_GlobalResources.Language))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Tipo de Calculo
        /// </summary>  
        [Display(Name = "LabelCalculeType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentCalculeType")]
        public int CalculationTypeId { get; set; }

      

        /// <summary>
        /// Valor de la Tasa
        /// </summary>  
        [DisplayFormat(DataFormatString = "{{{0:### ##}}}")]
        public decimal? RateCoverage { get; set; }

        /// <summary>
        /// Tipo de Tasa
        /// </summary>  
        [Display(Name = "LabelRateType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorRateType")]
        public int RateTypeId { get; set; }

        ///// <summary>
        ///// Tipo de Tasa InsuredObject
        ///// </summary>  
        //[Display(Name = "LabelRateType", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorRateType")]
        //public int RateTypeObjectId { get; set; }

        /// <summary>
        /// Valor de la Suma
        /// </summary>          
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Display(Name = "LabelLimitAmount", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentControlLimitAmount")]
        public decimal LimitAmount { get; set; }
        /// <summary>
        /// Valor de la Suma
        /// </summary>          
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Display(Name = "LabelSubLimitAmount", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentControlSubLimitAmount")]
        public decimal SubLimitAmount { get; set; }

        /// <summary>
        /// Valor de la Suma
        /// </summary>          
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Display(Name = "LabelLimitOccurrence", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentControlLimitOccurrenceAmount")]
        public decimal LimitOccurrenceAmount { get; set; }
        /// <summary>
        /// Valor de la Suma
        /// </summary>          
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Display(Name = "LabelLimitClaimant", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentControlLimitClaimantAmount")]
        public decimal LimitClaimantAmount { get; set; }

        /// <summary>
        /// Valor de la Suma
        /// </summary>          
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Display(Name = "LabelExcessLimit", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentControlExcessLimit")]
        public decimal ExcessLimit { get; set; }

        /// <summary>
        /// Valor de la Suma
        /// </summary>          
        
        public int MaxLiabilitySurety { get; set; }
        /// <summary>
        /// Valor de la Prima
        /// </summary>  
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Display(Name = "LabelPremium", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentControlPremiumAmount")]
        public decimal PremiumAmount { get; set; }

        /// <summary>
        /// PrefixId
        /// </summary>
        public int PrefixId { get; set; }

        /// <summary>
        /// Gets or sets the coverage mode.
        /// </summary>
        /// <value>
        /// The coverage mode.
        /// </value>
        public int CoverageMode { get; set; }

        /// <summary>
        /// Gets or sets the deductible identifier.
        /// </summary>
        /// <value>
        /// The deductible identifier.
        /// </value>
        public int DeductibleId { get; set; }

        /// <summary>
        /// Gets or sets the index of the coverage.
        /// </summary>
        /// <value>
        /// The index of the coverage.
        /// </value>
        public int CoverageIndex { get; set; }

        /// <summary>
        /// Prima del riesgo
        /// </summary>
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal premiumRisk { get; set; }

        /// <summary>
        /// Suma del riesgo
        /// </summary>
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal TotalSumInsured { get; set; }

        /// <summary>
        /// Es visible
        /// </summary>
        public bool IsVisible { get; set; }
        /// <summary>
        /// esta obligatorio
        /// </summary>
        public bool IsMandatory { get; set; }
        /// <summary>
        /// Seleccionada
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Subramo
        /// </summary>
        public int SubLineBusinessId { get; set; }

        /// <summary>
        /// Id Ramo Tecnico
        /// </summary>    
        [Display(Name = "LabelLineBusiness", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int LineBusinessId { get; set; }

        /// <summary>
        /// Gets or sets the FirstRiskType identifier.
        /// </summary>
        /// <value>
        /// The FirstRiskType identifier.
        /// </value>
        [Display(Name = "LabelFirstRiskType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentControlFirstRiskType")]
        public int FirstRiskType { get; set; }

        public int EndorsementType { get; set; }

        /// <summary>
        ///Obtiene o Setea si se calcula el año bisisesto
        /// </summary>
        /// <value>
        /// <c>true</c>Si este caso es año bisiesto; de otra manera, <c>false</c>.
        /// </value>
        public Boolean IsLeapYear { get; set; }

        /// <summary>
        ///Obtiene o Setea el asegurado
        /// </summary>
        public int HolderId { get; set; }

        /// <summary>
        ///Obtiene o Setea valor del objeto
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Porcentaje de Prima en Deposito
        /// </summary>
        public decimal DepositPremiumPercentage { get; set; }

        /// <summary>
        /// Tasa
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// Declarativo
        /// </summary>
        public bool IsDeclaration { get; set; }


        /// <summary>
        ///  periodos de ajustes
        /// </summary>
        public int? BillingPeriodId { get; set; }

        /// <summary>
        ///  periodos de declaración.
        /// </summary>
        public int? DeclarationPeriodId { get; set; }


    }
}