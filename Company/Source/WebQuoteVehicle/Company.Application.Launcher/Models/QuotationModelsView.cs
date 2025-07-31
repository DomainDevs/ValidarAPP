using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Models
{
    public class QuotationModelsView
    {
        /// <summary>
        /// Título
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Id Temporal
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id Cotización
        /// </summary>
        public int? QuotationId { get; set; }

        /// <summary>
        /// Placa
        /// </summary>
        public string Plate { get; set; }

        /// <summary>
        /// Código Fasecolda
        /// </summary>
        public string FasecoldaCode { get; set; }

        /// <summary>
        /// Id Marca
        /// </summary>
        public int MakeId { get; set; }

        /// <summary>
        /// Descripción Marca
        /// </summary>
        public string MakeDescription { get; set; }

        /// <summary>
        /// Id Modelo
        /// </summary>
        public int ModelId { get; set; }

        /// <summary>
        /// Descripción Modelo
        /// </summary>
        public string ModelDescription { get; set; }

        /// <summary>
        /// Id Versión
        /// </summary>
        public int VersionId { get; set; }

        /// <summary>
        /// Descripción Versión
        /// </summary>
        public string VersionDescription { get; set; }

        /// <summary>
        /// Uso
        /// </summary>
        public int Use { get; set; }

        /// <summary>
        /// Tipo
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Año
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Suma Asegurada
        /// </summary>
        public decimal InsuredAmount { get; set; }

        /// <summary>
        /// Suma Accesorios
        /// </summary>
        public decimal AmountAccesories { get; set; }

        /// <summary>
        /// Zona de Tarifación
        /// </summary>
        public int RatingZone { get; set; }

        /// <summary>
        /// Id Cliente
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Tipo de Cliente
        /// </summary>
        public int CustomerType { get; set; }

        /// <summary>
        /// Tipo de Individuo
        /// </summary>
        public int IndividualType { get; set; }

        /// <summary>
        /// Tipo de Documento
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int DocumentType { get; set; }

        /// <summary>
        /// Documento
        /// </summary>
       [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Document { get; set; }

        /// <summary>
        /// Digito de Verificación
        /// </summary>
        public string VerifyDigit { get; set; }
        
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
        public string BirthDate { get; set; }

        /// <summary>
        /// Genero
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Dirección
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Teléfono
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Correo
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Descuento
        /// </summary>
        public int Discount { get; set; }

        /// <summary>
        /// Producto
        /// </summary>
        public int Product { get; set; }

        /// <summary>
        /// Grupo de Coberturas
        /// </summary>
        public int GroupCoverage { get; set; }

        /// <summary>
        /// RC en Exceso
        /// </summary>
        public int LimitRC { get; set; }

        /// <summary>
        /// Prima
        /// </summary>
        public decimal Premium { get; set; }

        /// <summary>
        /// IVA
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Gastos
        /// </summary>
        public decimal Expenses { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Ruta PDF
        /// </summary>
        public string UrlPDF { get; set; }

        /// <summary>
        /// Versión Cotización
        /// </summary>
        public int? QuotationVersion { get; set; }

        /// <summary>
        /// Id Temporal
        /// </summary>
        public int? TemporalId { get; set; }

        /// <summary>
        /// Id Vehiculo de Reemplazo
        /// </summary>
        public bool ReplacementVehicle { get; set; }

        /// <summary>
        /// Risk Number
        /// </summary>
        public int RiskNum { get; set; }
    }
}