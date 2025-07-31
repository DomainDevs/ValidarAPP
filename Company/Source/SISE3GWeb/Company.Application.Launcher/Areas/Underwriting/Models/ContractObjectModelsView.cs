using System;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class ContractObjectModelsView
    {
        [Key]
        /// <summary>
        /// Identificador
        /// </summary>       
        public int? Id { get; set; }

        /// <summary>
        /// Fecha de emisión
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Solicitud Agrupadora
        /// </summary>
        public int? Request { get; set; }

        /// <summary>
        /// Solicitud Agrupadora
        /// </summary>
        public string RequestDescription { get; set; }

        /// <summary>
        /// Grupo de Facturacion
        /// </summary>
        public int? BillingGroup { get; set; }

        /// <summary>
        /// Grupo de Facturacion
        /// </summary>
        public string BillingGroupDescription { get; set; }

        /// <summary>
        /// Id Tomador
        /// </summary>        
        [Display(Name = "LabelHolder", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMessageHolderInvalid")]
        public int HolderId { get; set; }

        /// <summary>
        /// Numero de documento tomador
        /// </summary>
        public string HolderIdentificationDocument { get; set; }

        /// <summary>
        /// Tipo de documento
        /// </summary>
        public int? HolderDocumentType { get; set; }

        /// <summary>
        /// Nombre tomador
        /// </summary>
        [Display(Name = "LabelHolder", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(136)]
        public string HolderName { get; set; }

        /// <summary>
        /// Tipo de cliente Tomador
        /// </summary>        
        [Display(Name = "LabelCustomerType", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int HolderCustomerType { get; set; }

        /// <summary>
        /// Id detalle Tomador
        /// </summary>
        public int? HolderDetailId { get; set; }

        /// <summary>
        /// Id direccion tomador
        /// </summary>
        [Display(Name = "LabelAddress", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int HolderAddressId { get; set; }

        /// <summary>
        /// Id Telefono Tomador
        /// </summary>
        public int? HolderPhoneId { get; set; }

        /// <summary>
        /// Id Correo Tomador
        /// </summary>
        public int? HolderEmailId { get; set; }

        /// <summary>
        /// Id estado tomador
        /// </summary>
        [Display(Name = "LabelState", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int HolderStateId { get; set; }

        /// <summary>
        /// Id pais tomador
        /// </summary>
        [Display(Name = "LabelCountry", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int HolderCountryId { get; set; }

        /// <summary>
        /// Id actividad economica tomador
        /// </summary>
        [Display(Name = "EconomicActivity", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int HolderEconomicActivityId { get; set; }

        /// <summary>
        /// Id medio de pago
        /// </summary>
        [Display(Name = "LabelPaymentMethod", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int HolderPaymentMethodId { get; set; }

        /// <summary>
        /// Id pago
        /// </summary>
        [Display(Name = "LabelPaymentPlan", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int HolderPaymentIdId { get; set; }

        /// <summary>
        /// Fecha de nacimiento
        /// </summary>
        [Display(Name = "LabelHolderBirthDate", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime? HolderBirthDate { get; set; }

        /// <summary>
        /// Genero del Tomador
        /// </summary>
        public string HolderGender { get; set; }

        /// <summary>
        /// Genero del Tomador
        /// </summary>
        public int? HolderIndividualType { get; set; }

        /// <summary>
        /// Id agente
        /// </summary>
        [Display(Name = "LabelAgentPrincipal", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AgentId { get; set; }

        /// <summary>
        /// Nombre agente
        /// </summary>
        [Display(Name = "LabelAgentPrincipal", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string AgentName { get; set; }

        /// <summary>
        /// Id Agencia
        /// </summary>
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AgencyId { get; set; }


        /// <summary>
        /// Codigo Agencia
        /// </summary>
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AgentCode { get; set; }

        /// <summary>
        /// Nombre Agencia
        /// </summary>
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string AgencyName { get; set; }

        /// <summary>
        /// Sucursal agencia
        /// </summary>
        [Display(Name = "LabelAgentAgency", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AgencyBranchId { get; set; }

        /// <summary>
        /// AgentType 
        /// </summary>       
        public int AgentType { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        [Display(Name = "LabelBranch", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int BranchId { get; set; }

        /// <summary>
        /// Nombre Sucursal
        /// </summary>
        [Display(Name = "LabelBranch", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string BranchName { get; set; }

        /// <summary>
        /// Punto de venta
        /// </summary>
        [Display(Name = "LabelSalesPoint", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? SalePoint { get; set; }

        /// <summary>
        /// Ramo comercial
        /// </summary>
        [Display(Name = "LabelPrefixCommercial", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int PrefixId { get; set; }

        /// <summary>
        /// Nombre Ramo comercial
        /// </summary>
        [Display(Name = "LabelPrefixCommercial", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string PrefixName { get; set; }

        /// <summary>
        /// Producto
        /// </summary>
        [Display(Name = "LabelCommercialProduct", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int ProductId { get; set; }

        /// <summary>
        /// Nombre Producto
        /// </summary>
        [Display(Name = "LabelCommercialProduct", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string ProductName { get; set; }

        /// <summary>
        /// Tipo de póliza
        /// </summary>
        [Display(Name = "LabelPolicyType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int PolicyType { get; set; }

        /// <summary>
        /// Automatica o Especifica
        /// </summary>
        public bool? IsFloating { get; set; }

        /// <summary>
        /// Vigencia inicial
        /// </summary>
        [Display(Name = "LabelFrom", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Vigencia final
        /// </summary>
        [Display(Name = "LabelTo", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>
        [Display(Name = "LabelMoney", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Currency { get; set; }

        /// <summary>
        /// Cambio
        /// </summary>
        [Display(Name = "LabelChange", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(10)]
        public string ExchangeRate { get; set; }

        /// <summary>
        /// Tipo de temporal
        /// </summary>
        [Display(Name = "LabelTemporalType", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(1, 4, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int TemporalType { get; set; }


        /// <summary>
        /// Tipo de endoso
        /// </summary>
        [Display(Name = "LabelEndorsementType", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(1, Int16.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int EndorsementType { get; set; }

        /// <summary>
        /// Hora
        /// </summary>        
        public string TimeHour { get; set; }

        /// <summary>
        /// Tipo de riesgo del producto
        /// </summary>
        public int? CoveredRiskType { get; set; }

        /// <summary>
        /// Titulo pantalla
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the quotation version.
        /// </summary>
        /// <value>
        /// The quotation version.
        /// </value>
        public int? QuotationVersion { get; set; }

        /// <summary>
        /// Gets or sets the quotation identifier.
        /// </summary>
        /// <value>
        /// The quotation identifier.
        /// </value>
        public int? QuotationId { get; set; }

        /// <summary>
        /// Fecha de baja de tomador
        /// </summary>
        public DateTime? HolderDeclinedDate { get; set; }

        /// <summary>
        /// Id de asegurado de tomador
        /// </summary>
        public int HolderInsuredId { get; set; }

        /// <summary>
        /// cantidad de riesgos
        /// </summary>
        public int RisksQuantity { get; set; }

        /// <summary>
        /// cantidad de riesgos
        /// </summary>
        public int? JustificationSarlaft { get; set; }

        /// <summary>
        /// cantidad de riesgos
        /// </summary>
        public int? NumberRadication { get; set; }

        /// <summary>
        /// Numero de radiacación
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentTicketNumber")]
        public int? TicketNumber { get; set; }

        /// <summary>
        /// Fecha de radicación
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentTicketDate")]
        public DateTime? TicketDate { get; set; }

        /// <summary>
        /// Días de validez
        /// </summary>
        public int? DaysValidity { get; set; }

        /// <summary>
        /// Calculo de prima minima
        /// </summary>
        public bool? CalculateMinPremium { get; set; }

        /// <summary>
        /// Calculo de prima minima
        /// </summary>
        public decimal FinancingRate { get; set; }

        /// <summary>
        /// Calculo de prima minima
        /// </summary>
        public int StatePay { get; set; }

        /// <summary>
        /// Calculo de prima minima
        /// </summary>
        public DateTime ValidSince { get; set; }

        /// <summary>
        /// Calculo de prima minima
        /// </summary>
        public DateTime ValidityUntil { get; set; }

        /// <summary>
        /// Calculo de prima minima
        /// </summary>
        public int InsuredCode { get; set; }

        /// <summary>
        /// Calculo de prima minima
        /// </summary>
        public string InsuredName { get; set; }

        /// <summary>
        /// Calculo de prima minima
        /// </summary>
        public int Quota { get; set; }

        /// <summary>
        /// Calculo de prima minima
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Calculo de prima minima
        /// </summary>
        public decimal Percentage { get; set; }

        /// <summary>
        /// Calculo de prima minima
        /// </summary>
        public decimal ValueFinance { get; set; }

        /// <summary>
        /// Calculo de prima minima
        /// </summary>
        public decimal ValueFinanceTwo { get; set; }

        /// <summary>
        /// Calculo de prima minima
        /// </summary>
        public decimal MinimumValue { get; set; }

        /// <summary>
        /// Calculo de prima minima
        /// </summary>
        public int IndividualId { get; set; }


        /// <summary>
        /// Cuerpo
        /// </summary>      
        public string TextBody { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>      
        public string Observations { get; set; }

        /// <summary>
        /// Texto Precatalogado
        /// </summary>      
        public string TextPrecataloged { get; set; }
    }
}