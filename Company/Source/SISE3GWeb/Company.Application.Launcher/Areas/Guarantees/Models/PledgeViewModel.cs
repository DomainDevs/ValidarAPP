using System;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Guarantees.Models
{
    public class PledgeViewModel
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
        /// Placa
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(15, ErrorMessageResourceName = "ErrorMaxlengthCode", ErrorMessageResourceType = typeof(App_GlobalResources.Language))]
        [Display(Name = "LabelPlate", ResourceType = typeof(App_GlobalResources.Language))]
        public string Plate { get; set; }

        /// <summary>
        /// Motor
        /// </summary>
        [StringLength(50, ErrorMessageResourceName = "ErrorMaxlengthCode", ErrorMessageResourceType = typeof(App_GlobalResources.Language))]
        [Display(Name = "LabelEngine", ResourceType = typeof(App_GlobalResources.Language))]
        public string Engine { get; set; }

        //Longitud máxima 25 caracteres. No es un dato obligatorio.
        /// <summary>
        /// Chasis
        /// </summary>
        [Display(Name = "LabelChassis", ResourceType = typeof(App_GlobalResources.Language))]
        public string Chassis { get; set; }

        /// <summary>
        /// Fecha avalúo
        /// </summary>
        [Display(Name = "LabelValuationDate", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime? ValuationDate { get; set; }

        /// <summary>
        /// Compañía
        /// </summary>
        [Display(Name = "LabelCompanyInsured", ResourceType = typeof(App_GlobalResources.Language))]
        public string Company { get; set; }

        /// <summary>
        /// Número de póliza
        /// </summary>
        [Display(Name = "LabelNroPolicy", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(20)]
        public string NumberPolicy { get; set; }

        /// <summary>
        /// Valor asegurado
        /// </summary>
        [Display(Name = "LabelInsuredValue", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal? InsuredValue { get; set; }

        /// <summary>
        /// Valor avalúo
        /// </summary>
        [Display(Name = "LabelValuationValue", ResourceType = typeof(App_GlobalResources.Language))]        
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal? ValuationValue { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>
        [Display(Name = "LabelCurrency", ResourceType = typeof(App_GlobalResources.Language))]
        public int Currency { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [Display(Name = "LabelObservations", ResourceType = typeof(App_GlobalResources.Language))]
        public string Observations { get; set; }

    }
}