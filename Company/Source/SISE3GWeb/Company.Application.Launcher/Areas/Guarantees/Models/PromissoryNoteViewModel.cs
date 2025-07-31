using System;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Guarantees.Models
{
    public class PromissoryNoteViewModel
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
        /// Número de documento
        /// </summary>
        [Display(Name = "LabelDocumentNumber", ResourceType = typeof(App_GlobalResources.Language))]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Fecha de constitución
        /// </summary>
        [Display(Name = "LabelConstitutionDate", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime ConstitutionDate { get; set; }

        /// <summary>
        /// Fecha de vencimiento
        /// </summary>
        [Display(Name = "LabelDueDate", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Valor nominal
        /// </summary>
        [Display(Name = "LabelNominalValue", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal NominalValue { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>

        [Display(Name = "LabelBranch", ResourceType = typeof(App_GlobalResources.Language))]
        [Required]
        public int Currency { get; set; }

        /// <summary>
        /// Tipo de pagaré
        /// </summary>
        [Display(Name = "LabelPromissoryNoteType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required]
        public int PromissoryNoteType { get; set; }

        /// <summary>
        /// Número firmantes
        /// </summary>
        [Display(Name = "LabelSignatoriesNumber", ResourceType = typeof(App_GlobalResources.Language))]
        public int SignatoriesNumber { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [Display(Name = "LabelObservations", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(254, ErrorMessageResourceName = "ErrorMaxlengthCode", ErrorMessageResourceType = typeof(App_GlobalResources.Language))]
        public string Observations { get; set; }
    }
}