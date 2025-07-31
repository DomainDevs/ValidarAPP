using System;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class FidelityCoverageViewModel
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
        /// Id de la medida de la prestación
        /// </summary>     
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentCoverage")]
        public int MeasurementBenefitId { get; set; }


        /// <summary>
        /// Valor declarado
        /// </summary>          
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
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
        public decimal? Rate { get; set; }

        /// <summary>
        /// Tipo de Tasa
        /// </summary>  
        public int RateType { get; set; }

        /// <summary>
        /// Tipo de Tasa InsuredObject
        /// </summary>  
        public int RateTypeObjectId { get; set; }
        
        /// <summary>
        /// Valor de la Suma
        /// </summary>    
      
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal LimitAmount { get; set; }
        /// <summary>
        /// Valor de la Suma
        /// </summary>          
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal SubLimitAmount { get; set; }

        /// <summary>
        /// Valor de la Suma
        /// </summary>          
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal LimitOccurrenceAmount { get; set; }        
        /// <summary>
        /// Valor de la Suma
        /// </summary>          
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal LimitClaimantAmount { get; set; }

        /// <summary>
        /// Valor de la Suma
        /// </summary>          
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal ExcessLimit { get; set; }
        
         /// <summary>
        /// Valor de la Suma
        /// </summary>          
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal MaxFidelity { get; set; }
        
        /// <summary>
        /// Valor de la Prima
        /// </summary>  
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
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

        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal premiumRisk { get; set; }

        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal TotalSumInsured { get; set; }
        
        /// <summary>
        /// Estatus cobertura
        /// </summary> 
        public int CoverStatus { get; set; }

        /// <summary>
        /// Descripcion de estatus de cobertura
        /// </summary> 
        public string CoverStatusName { get; set; }

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
        public int LineBusinessId { get; set; }

        /// <summary>
        /// Gets or sets the FirstRiskType identifier.
        /// </summary>
        /// <value>
        /// The FirstRiskType identifier.
        /// </value>
        public int FirstRiskType { get; set; }

        /// <summary>
        /// Id tomador
        /// </summary> 
        public int HolderId { get; set; }

        /// <summary>
        /// Titulo
        /// </summary> 
        public string Title { get; set; }

    }
}