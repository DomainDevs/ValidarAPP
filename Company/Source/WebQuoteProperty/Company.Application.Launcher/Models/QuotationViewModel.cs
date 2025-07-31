using Sistran.Core.Framework.UIF.Web.App_GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Models
{
    public class QuotationViewModel
    {
        /// <summary>
        /// Id Temporal
        /// </summary>
        public int TemporalId { get; set; }

        /// <summary>
        /// Id Cotización
        /// </summary>
        public int QuotationId { get; set; }

        /// <summary>
        /// Versión Cotización
        /// </summary>
        public int QuotationVersion { get; set; }

        /// <summary>
        /// Dirección Completa
        /// </summary>
        [Required]
        [Display(Name = "LabelFullAddress", ResourceType = typeof(Language))]
        [MaxLength(180)]
        public string FullAddress { get; set; }

        /// <summary>
        /// Año De Construcción
        /// </summary>
        [Required]
        [Display(Name = "LabelConstructionYear", ResourceType = typeof(Language))]
        //[MaxLength(4)]
        public int ConstructionYear { get; set; }

        /// <summary>
        /// Id País
        /// </summary>
        [Required]
        [Display(Name = "LabelCountry", ResourceType = typeof(Language))]
        public int CountryId { get; set; }

        /// <summary>
        /// Id Departamento
        /// </summary>
        [Required]
        [Display(Name = "LabelState", ResourceType = typeof(Language))]
        public int StateId { get; set; }

        /// <summary>
        /// Id Ciudad
        /// </summary>
        [Required]
        [Display(Name = "LabelCity", ResourceType = typeof(Language))]
        public int CityId { get; set; }

        /// <summary>
        /// Id Cliente
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Tipo de Cliente
        /// </summary>
        public int CustomerTypeId { get; set; }

        /// <summary>
        /// Tipo de Individuo
        /// </summary>
        [Required]
        public int IndividualTypeId { get; set; }

        /// <summary>
        /// Tipo de Documento
        /// </summary>
        [Required]
        [Display(Name = "LabelDocumentType", ResourceType = typeof(Language))]
        public int DocumentTypeId { get; set; }

        /// <summary>
        /// Documento
        /// </summary>
        [Required]
        [Display(Name = "LabelDocumentNumber", ResourceType = typeof(Language))]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Razón Social
        /// </summary>
        public string TradeName { get; set; }

        /// <summary>
        /// Nombres
        /// </summary>
        public string Names { get; set; }

        /// <summary>
        /// Primer Apellido
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Segundo Apellido
        /// </summary>
        public string SecondSurname { get; set; }

        /// <summary>
        /// Fecha de Nacimiento
        /// </summary>
        [Required]
        public string BirthDate { get; set; }

        /// <summary>
        /// Genero
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Dirección
        /// </summary>
        public string CompanyAddress { get; set; }

        /// <summary>
        /// Teléfono
        /// </summary>
        public string CompanyPhone { get; set; }

        /// <summary>
        /// Correo
        /// </summary>
        public string CompanyEmail { get; set; }

        /// <summary>
        /// Dirección
        /// </summary>
        [MaxLength(180)]
        public string PersonAddress { get; set; }

        /// <summary>
        /// Teléfono
        /// </summary>
        public string PersonPhone { get; set; }

        /// <summary>
        /// Correo
        /// </summary>
        [MaxLength(60)]
        public string PersonEmail { get; set; }

        /// <summary>
        /// Producto
        /// </summary>
        [Required]
        [Display(Name = "LabelProduct", ResourceType = typeof(Language))]
        public int ProductId { get; set; }

        /// <summary>
        /// Grupo de Coberturas
        /// </summary>
        [Required]
        [Display(Name = "LabelGroupCoverages", ResourceType = typeof(Language))]
        public int CoverageGroupId { get; set; }
    }
}