using Sistran.Core.Framework.UIF.Web.Constant;
using Sistran.Core.Framework.UIF.Web.Validator;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class CoverageModelsView
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
        /// Id de cobertura
        /// </summary>     
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentCoverage")]
        public int CoverageId { get; set; }

        /// <summary>
        /// Id Grupo de Cobertura
        /// </summary>
        public int CoverageGroup { get; set; }

        /// <summary>
        /// Id de Producto
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Id tipo de endoso poliza
        /// </summary>
        public int EndorsementType { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Fecha Desde Vigencia
        /// </summary>     
        [Display(Name = "LabelFrom", ResourceType = typeof(App_GlobalResources.Language))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string CurrentFrom { get; set; }

        /// <summary>
        /// Fecha Hasta Vigencia
        /// </summary>  
        [Display(Name = "LabelTo", ResourceType = typeof(App_GlobalResources.Language))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string CurrentTo { get; set; }

        /// <summary>
        /// Tipo de Calculo
        /// </summary>  
        [Display(Name = "LabelCalculeType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentCalculeType")]
        public int CalculationTypeId { get; set; }

        /// <summary>
        /// Valor de la Tasa
        /// </summary>  
        //[DisplayFormat(DataFormatString = "{{{0:### ##}}}")]
        public decimal? Rate { get; set; }

        /// <summary>
        /// Tipo de Tasa
        /// </summary>  
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentControlRateType")]
        public int RateType { get; set; }

        [Display(Name = "LabelLimitAmount", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentControlLimitAmount")]
        //[RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [MaxDecimal(ColumName = ValidatorProperty.DecimalPlaceRequired, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal LimitAmount { get; set; }

        [Display(Name = "LabelSubLimitAmount", ResourceType = typeof(App_GlobalResources.Language))]
        [MaxDecimal(ColumName = ValidatorProperty.DecimalPlaceRequired, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal SubLimitAmount { get; set; }

        /// <summary>
        /// Valor de la Prima
        /// </summary>  
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal PremiumAmount { get; set; }

        public int CoverStatus { get; set; }
        public string CoverStatusName { get; set; }
        public decimal? AccumulatedPremiumAmount { get; set; }
        public decimal? EndorsementLimitAmount { get; set; }
        public decimal? EndorsementSublimitAmount { get; set; }
        public bool IsDeclarative { get; set; }
        public bool IsMinPremiumDeposit { get; set; }

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
        public int? DeductibleId { get; set; }

        /// <summary>
        /// Gets or sets the deductible description.
        /// </summary>
        /// <value>
        /// The deductible identifier.
        /// </value>
        public string DeductibleDescription { get; set; }

        /// <summary>
        /// Gets or sets the index of the coverage.
        /// </summary>
        /// <value>
        /// The index of the coverage.
        /// </value>
        public int CoverageIndex { get; set; }

        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal PremiumRisk { get; set; }

        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal TotalSumInsured { get; set; }

        public bool IsVisible { get; set; }

        public bool IsMandatory { get; set; }

        public bool IsSelected { get; set; }

        public int SubLineBusinessId { get; set; }

        public int LineBusinessId { get; set; }

        public int EffectiveDays { get; set; }

        public string Title { get; set; }

        public decimal? FlatRatePorcentage { get; set; }

        /// <summary>
        /// Valor declarado
        /// </summary>          
        [Display(Name = "LabelDeclaredValue", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDeclaredValue")]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal DeclaredAmount { get; set; }

        [Display(Name = "LabelLimitClaimantAmount", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "LabelLimitClaimantAmount")]
        public decimal? LimitClaimantAmount { get; set; }

        [Display(Name = "LabelMaxLiabilitySurety", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxResponsability")]
        public decimal? MaxLiabilityAmount { get; set; }

        [Display(Name = "LabelLimitClaimant", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "LabelLimitClaimant")]
        public decimal? LimitOccurrenceAmount { get; set; }

        public decimal LimitEvent { get; set; }

        public string TypeCoverage { get; set; }

        
        public int DecimalPlaceRequired { get; set; }

        public string Text { get; set; }
    }
}