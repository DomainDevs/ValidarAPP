
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class RiskSuretyModelsView
    {
        /// <summary>
        /// Id de Temporal
        /// </summary>
        public int TemporalId { get; set; }

        /// <summary>
        /// Id de Riesgo
        /// </summary>
        public int? RiskId { get; set; }

        /// <summary>
        /// Nombre Asegurado
        /// </summary>
        [Display(Name = "LabelInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(136)]
        public string InsuredName { get; set; }

        /// <summary>
        /// Id asegurado
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int InsuredId { get; set; }

        /// <summary>
        /// Id detalle asegurado
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? InsuredDetailId { get; set; }

        /// <summary>
        /// Direccion asegurado
        /// </summary>
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int InsuredAddressId { get; set; }

        /// <summary>
        /// Telefono asegurado
        /// </summary>
        public int? InsuredPhoneId { get; set; }

        /// <summary>
        /// Correo asegurado
        /// </summary>
        public int? InsuredEmailId { get; set; }

        /// <summary>
        /// Tipo de cliente Tomador
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        public int InsuredCustomerType { get; set; }
        /// <summary>
        /// Tipo de persona asegurado
        /// </summary>        
        public int InsuredIndividualType { get; set; }

        /// <summary>
        /// Fecha de Nacimiento del Asegurado
        /// </summary>
        public DateTime? InsuredBirthDate { get; set; }

        /// <summary>
        /// Genero del Asegurado
        /// </summary>
        public string InsuredGender { get; set; }

        /// <summary>
        /// Obtener o setear el Afianzado.
        /// </summary>
        /// <value>
        /// Id Afianzado
        /// </value>
        [Display(Name = "LabelSecure", ResourceType = typeof(App_GlobalResources.Language))]
        public int ContractorId { get; set; }

        /// <summary>
        /// Obtener o setear el CustomerType.
        /// </summary>
        /// <value>
        /// ContractorCustomerType
        /// </value>
        public int ContractorCustomerType { get; set; }
        /// <summary>
        /// Obtener o setear el IndividualType.
        /// </summary>
        /// <value>
        /// ContractorCustomerType
        /// </value>
        public int ContractorIndividualType { get; set; }

        /// <summary>
        /// Gets or sets the insured identifier.
        /// </summary>
        /// <value>
        /// Nombre afianzado
        /// </value>
        [Display(Name = "LabelSecure", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(136, ErrorMessageResourceName = "ErrorMaxlengthCode", ErrorMessageResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string ContractorName { get; set; }

        /// <summary>
        /// Id detalle afianzado
        /// </summary>        
        [Display(Name = "LabelSecure", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? ContractorDetailId { get; set; }

        /// <summary>
        /// Direccion asegurado
        /// </summary>
        [Display(Name = "LabelSecure", ResourceType = typeof(App_GlobalResources.Language))]
        public int ContractorAddressId { get; set; }
        /// <summary>
        /// ContractorAddressDescription
        /// </summary>
        [Display(Name = "LabelSecure", ResourceType = typeof(App_GlobalResources.Language))]
        public string ContractorAddressDescription { get; set; }
        /// <summary>
        /// ContractorAddressDescription
        /// </summary>
        [Display(Name = "LabelSecure", ResourceType = typeof(App_GlobalResources.Language))]
        public string ContractorPhoneDescription { get; set; }

        /// <summary>
        /// Obtener o setear el Cumulo
        /// </summary>
        /// <value>
        /// Cumulo
        /// </value>
        [Display(Name = "LabelAccumulation", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public Decimal Aggregate { get; set; }

        /// <summary>
        /// Gets or sets the contract value.
        /// </summary>
        /// <value>
        /// Valor del Contrato
        /// </value>        
        [Display(Name = "LabelContractValue", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public decimal ContractValue { get; set; }

        /// <summary>
        /// Gets or sets the contract number.
        /// </summary>
        /// <value>
        /// The contract number.
        [Display(Name = "LabelContractNumber", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(20, ErrorMessage = "Nro del Contrato Maximo 20 Caracteres")]
        public string ContractNumber { get; set; }

        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        /// <value>
        /// The class.
        /// </value>
        [Display(Name = "LabelClassofContract", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Class { get; set; }

        /// <summary>
        /// Obtener o setear el Cupo Operativo
        /// </summary>
        /// <value>
        /// Cupo Operativo
        /// </value>
        [Display(Name = "LabelOperationalCapacity", ResourceType = typeof(App_GlobalResources.Language))]
        //[Range(0.1, double.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "SecuredWithoutOperative")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public decimal OperatingQuota { get; set; }

        /// <summary>
        /// Cupo Disponible
        /// </summary>
        /// <value>
        /// Disponible
        /// </value>
        [Display(Name = "LabelAvailable", ResourceType = typeof(App_GlobalResources.Language))]
        //[Range(0.1, double.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorAvaliableOperationQuota")]        
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public decimal Available { get; set; }

        /// <summary>
        /// Cúpo Operativo Disponible  (Cs/UT/CF)
        /// </summary>
        /// <value>
        /// Disponible
        /// </value>
        [Display(Name = "LabelOperationalCapacity", ResourceType = typeof(App_GlobalResources.Language))]
        //[Range(0.1, double.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorAvaliableOperationQuota")]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public decimal AvailableOperatingQuota { get; set; }

        /// <summary>
        /// Obtiene o setea el tipo de contrato
        /// </summary>
        /// <value>
        /// tipo de contrato
        /// </value>
        [Display(Name = "LabelContractType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int ContractType { get; set; }

        /// <summary>
        /// Obtiene o setea el grupo de coberturas
        /// </summary>
        /// <value>
        /// Grupo De coberturas
        /// </value>
        [Display(Name = "LabelGroupCoverage", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int GroupCoverage { get; set; }

        /// <summary>
        /// Valor asegurado
        /// </summary>
        [Display(Name = "LabelTotalSumInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        //[RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public string AmountInsured { get; set; }

        /// <summary>
        /// Prima
        /// </summary>
        [Display(Name = "LabelPremiumRisk", ResourceType = typeof(App_GlobalResources.Language))]
        //[RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public string Premium { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RiskSuretyModelsView"/> is isfacultative.
        /// </summary>
        /// <value>
        ///   <c>true</c> if isfacultative; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsfacultativeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RiskSuretyModelsView"/> is isfacultative.
        /// </summary>
        /// <value>
        ///   <c>true</c> if isfacultative; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsRetention { get; set; }

        /// <summary>
        /// Obtiene o setea el numero de documento
        /// </summary>
        /// <value>
        /// numero de documento
        /// </value>
        public string ContractorIdentificationDocument { get; set; }

        /// <summary>
        /// Obtiene o setea el Id de Tipo documento
        /// </summary>
        /// <value>
        /// Tipo de documento
        /// </value>
        public int ContractorDocumentTypeId { get; set; }

        /// <summary>
        /// Obtiene o setea la descripción del Tipo documento
        /// </summary>
        /// <value>
        /// Tipo de documento
        /// </value>
        public string ContractorDocumentTypeDescription { get; set; }

        /// <summary>
        /// Obtiene o setea el numero de documento
        /// </summary>
        /// <value>
        /// numero de documento
        /// </value>
        public string InsuredIdentificationDocument { get; set; }

        /// <summary>
        /// Obtiene o setea el tipo de asociacion del asegurado
        /// </summary>
        /// <value>
        /// Tipo de asociacion
        /// </value>
        public int? InsuredAssociationTypeId { get; set; }

        /// <summary>
        /// Obtiene o setea el tipo de asociacion del afianzado
        /// </summary>
        /// <value>
        /// Tipo de asociacion
        /// </value>
        public int? ContractorAssociationTypeId { get; set; }

        /// <summary>
        /// Obtiene o setea el Id del tipo de documento del asegurado
        /// </summary>
        /// <value>
        /// numero de documento
        /// </value>
        public int InsuredDocumentTypeId { get; set; }

        /// <summary>
        /// Obtiene o setea la descripcion del tipo de documento del asegurado
        /// </summary>
        /// <value>
        /// numero de documento
        /// </value>
        public string InsuredDocumentTypeDescription { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
        /// <summary>
        /// TerminalUnitContract
        /// </summary>
        public DateTime TerminalUnitContract { get; set; }

        /// <summary>
        /// FinalDeliveryDate
        /// </summary>
        public DateTime FinalDeliveryDate { get; set; }

        /// <summary>
        /// ChkContractDate
        /// </summary>
        public bool ChkContractDate { get; set; }
        /// <summary>
        /// ChkContractFinalyDate
        /// </summary>
        public bool ChkContractFinalyDate { get; set; }

        /// <summary>
        /// Obtiene o setea el Codigo Asegurado del Afianzado
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int ContractorInsuredId { get; set; }


        /// <summary>
        /// Obtiene o setea país
        /// </summary>
        [Required]
        [Display(Name = "Country", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        public int CountryId { get; set; }

        /// <summary>
        /// Obtiene o setea departamento
        /// </summary>
        [Required]
        [Display(Name = "LabelDepartment", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        public int? StateId { get; set; }

        /// <summary>
        /// Obtiene o setea la ciudad
        /// </summary>
        [Required]
        [Display(Name = "City", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        public int? CityId { get; set; }

        /// <summary>
        /// Obtiene el nombre del país
        /// </summary>
        public string CountryDescription { get; set; }

        /// <summary>
        /// Obtiene el nombre del departamento
        /// </summary>
        public string StateDescription { get; set; }

        /// <summary>
        /// Obtiene el nombre del país
        /// </summary>
        public string CityDescription { get; set; }

        /// <summary>
        /// Obtiene si la covertura del riesgo es a nivel nacional
        /// </summary>
        /// <value>
        /// </value>
        public Boolean IsNational { get; set; }

        /// <summary>
        /// Obtiene si el objeto de contrato a nivel de riesgo
        /// </summary>
        /// <value>
        /// </value>
        public string ContractObject { get; set; }


    }
}