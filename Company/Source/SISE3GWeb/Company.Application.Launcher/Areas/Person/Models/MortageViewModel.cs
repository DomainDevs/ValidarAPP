using System;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Models
{
    public class MortageViewModel
    {
        /// <summary>
        /// Sucursal
        /// </summary>
        public int Office { get; set; }

        /// <summary>
        /// Cerrada
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Id de la contragarantía
        /// </summary>
        public int GuaranteeId { get; set; }

        /// <summary>
        /// Tipo de contragarantía
        /// </summary>
        public int GuaranteeTypeCode { get; set; }

        /// <summary>
        /// Estado de la contragarantía
        /// </summary>
        public int GuaranteeStatus { get; set; }

        /// <summary>
        /// País
        /// </summary>
        [Display(Name = "LabelCountry", ResourceType = typeof(App_GlobalResources.Language))]
        [Required]
        public int Country { get; set; }

        /// <summary>
        /// Departamento
        /// </summary>
        [Display(Name = "LabelState", ResourceType = typeof(App_GlobalResources.Language))]
        [Required]
        public int State { get; set; }

        /// <summary>
        /// Municipio
        /// </summary>
        [Display(Name = "LabelCity", ResourceType = typeof(App_GlobalResources.Language))]
        [Required]
        public int City { get; set; }

        /// <summary>
        /// Dirección
        /// </summary>
        [Display(Name = "LabelAddresses", ResourceType = typeof(App_GlobalResources.Language))]
        public string Address { get; set; }

        /// <summary>
        /// Tipo de bien
        /// </summary>
        [Display(Name = "LabelMortageType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required]
        public int MortageType { get; set; }

        /// <summary>
        /// Número de escritura
        /// </summary>
        [Display(Name = "LabelDeedNumber", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string DeedNumber { get; set; }

        /// <summary>
        /// Valor avalúo
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(Name = "LabelValuationValue", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal ValuationValue { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>
        [Display(Name = "LabelCoin", ResourceType = typeof(App_GlobalResources.Language))]
        [Required]
        public int Currency { get; set; }

        /// <summary>
        /// Fecha avalúo
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "LabelValuationDate", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime? ValuationDate { get; set; }

        /// <summary>
        /// Nombres - Apellidos del Perito
        /// </summary>
        [Display(Name = "LabelAdjusterNames", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(60, ErrorMessageResourceName = "ErrorMaxlengthCode", ErrorMessageResourceType = typeof(App_GlobalResources.Language))]
        public string AdjusterNames { get; set; }

        /// <summary>
        /// Área construida
        /// </summary>
        [Display(Name = "LabelBuiltArea", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal? BuiltArea { get; set; }

        /// <summary>
        /// Área medida
        /// </summary>
        [Display(Name = "LabelMeasureArea", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal? MeasureArea { get; set; }

        /// <summary>
        /// Unidad de medida
        /// </summary>
        [Display(Name = "LabelUnitOfMeasure", ResourceType = typeof(App_GlobalResources.Language))]
        public int UnitOfMeasure { get; set; }

        /// <summary>
        /// Compañía aseguradora
        /// </summary>
        [Display(Name = "LabelCompanyInsured", ResourceType = typeof(App_GlobalResources.Language))]
        public string Company { get; set; }

        /// <summary>
        /// Número de póliza
        /// </summary>
        [Display(Name = "LabelNroPolicy", ResourceType = typeof(App_GlobalResources.Language))]
        public string NumberPolicy { get; set; }

        /// <summary>
        /// Valor asegurado
        /// </summary>
        [Display(Name = "LabelInsuredValue", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal? InsuredValue { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [Display(Name = "LabelObservation", ResourceType = typeof(App_GlobalResources.Language))]
        public string Observation { get; set; }
    }
}
