using System;
using System.ComponentModel.DataAnnotations;
using Sistran.Core.Framework.UIF.Web.Helpers;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class RiskVehicleModelsView: RiskModelView
    {
        /// <summary>
        /// Id de Riesgo
        /// </summary>
        public int? RiskId { get; set; }
        
        /// <summary>
        /// Placa
        /// </summary>
        [Display(Name = "LabelPlate", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")] 
        [DataNotationValidation(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorValidateLicensePlate")]
        [StringLength(15)]
        public string LicensePlate { get; set; }

        /// <summary>
        /// Placa
        /// </summary>
        [Display(Name = "LabelPlate", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        [StringLength(15)]
        public string LicensePlateConfirmation { get; set; }

        /// <summary>
        /// Código fasecolda
        /// </summary>
        [Display(Name = "LabelFasecoldaCode", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [StringLength(8)]
        public string FasecoldaCode { get; set; }

        /// <summary>
        /// Id marca
        /// </summary>
        [Display(Name = "LabelMake", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Range(1, int.MaxValue, ErrorMessage = "Error")]
        public int Make { get; set; }

        /// <summary>
        /// Descripcion Marca
        /// </summary>
        [Display(Name = "LabelMake", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string MakeDescription { get; set; }

        /// <summary>
        /// Id modelo
        /// </summary>
        [Display(Name = "LabelModel", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Model { get; set; }

        /// <summary>
        /// Descripcion modelo
        /// </summary>
        [Display(Name = "LabelModel", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string ModelDescription { get; set; }

        /// <summary>
        /// Id version
        /// </summary>
        [Display(Name = "LabelVersion", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Version { get; set; }

        /// <summary>
        /// Descripcion version
        /// </summary>
        [Display(Name = "LabelVersion", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string VersionDescription { get; set; }

        /// <summary>
        /// Id tipo
        /// </summary>
        [Display(Name = "LabelType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Type { get; set; }

        /// <summary>
        /// Id uso
        /// </summary>
        [Display(Name = "LabelUse", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Use { get; set; }

        /// <summary>
        /// Año
        /// </summary>
        [Display(Name = "LabelYear", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Year { get; set; }

        /// <summary>
        /// Es nuevo?
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// Motor
        /// </summary>
        [Display(Name = "LabelEngine", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        [StringLength(50)]
        public string Engine { get; set; }

        /// <summary>
        /// Motor
        /// </summary>
        [Display(Name = "LabelEngineConfirmation", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        [StringLength(50)]
        public string EngineConfirmation { get; set; }

        /// <summary>
        /// Chasis
        /// </summary>
        [Display(Name = "LabelChassis", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        [StringLength(50)]
        public string Chassis { get; set; }

        /// <summary>
        /// Chasis
        /// </summary>
        [Display(Name = "LabelChassisConfirmation", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        [StringLength(50)]
        public string ChassisConfirmation { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        [Display(Name = "LabelColor", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Color { get; set; }

        /// <summary>
        /// Zona de tarifación
        /// </summary>
        [Display(Name = "LabelRateZone", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int RatingZone { get; set; }

        /// <summary>
        /// Precio
        /// </summary>
        [Display(Name = "LabelVehiclePrice", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [StringLength(18)]
        public string Price { get; set; }

        /// <summary>
        /// Precio
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [StringLength(18)]
        public string StandardVehiclePrice { get; set; }

        /// <summary>
        /// Tasa
        /// </summary>
        [Display(Name = "LabelFlatRate", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^\d+((\.\d+)*)+(\,\d{1,4})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [StringLength(18)]
        public string Rate { get; set; }

        /// <summary>
        /// Valor accesorios
        /// </summary>
        [RegularExpression(@"^\-?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [StringLength(18)]
        public string PriceAccesories { get; set; }

        /// <summary>
        /// Limite RC
        /// </summary>
        [Display(Name = "LabelLimitRC", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int LimitRC { get; set; }

        /// <summary>
        /// Grupo de coberturas
        /// </summary>
        [Display(Name = "LabelGroupCoverage", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int GroupCoverage { get; set; }

        /// <summary>
        /// Valor asegurado
        /// </summary>
        [Display(Name = "LabelTotalSumInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public string AmountInsured { get; set; }

        /// <summary>
        /// Prima
        /// </summary>
        [Display(Name = "LabelPremiumRisk", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Premium { get; set; }

        /// <summary>
        /// Id asegurado
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int InsuredId { get; set; }

        /// <summary>
        /// Nombre asegurado
        /// </summary>
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(136)]
        public string InsuredName { get; set; }

        /// <summary>
        /// Id Detalle Asegurado
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        public int? InsuredDetailId { get; set; }

        /// <summary>
        /// Id Dirección Asegurado
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int InsuredAddressId { get; set; }

        /// <summary>
        /// Id Teléfono Asegurado
        /// </summary>
        public int? InsuredPhoneId { get; set; }

        /// <summary>
        /// Id Corre Asegurado
        /// </summary>
        public int? InsuredEmailId { get; set; }
        
        /// <summary>
        /// Tipo de cliente Tomador
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int InsuredCustomerType { get; set; }
        
        /// <summary>
        /// Numero de documento Asegurado
        /// </summary>
        public string InsuredDocumentNumber { get; set; }

        /// <summary>
        /// Fecha de Nacimiento del Asegurado
        /// </summary>
        public DateTime? InsuredBirthDate { get; set; }

        /// <summary>
        /// Genero del Asegurado
        /// </summary>
        public string InsuredGender { get; set; }
        
        /// <summary>
        /// Titulo pantalla
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Precio original
        /// </summary>
        public decimal OriginalPrice { get; set; }
        
        /// <summary>
        /// Id de Riesgo
        /// </summary>
        public int? OriginalRiskId { get; set; }

        /// <summary>
        /// Id de Riesgo
        /// </summary>
        public int? ServiceType { get; set; }

        /// <summary>
        /// Id de Riesgo
        /// </summary>
        public string ServiceTypeDescription { get; set; }
        /// <summary>
        /// Status Riesgo
        /// </summary>
        public int Status { get; set; }

    }

}