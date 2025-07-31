using System;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class RiskThirdPartyLiabilityViewModel
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
        /// Grupo de coberturas
        /// </summary>
        [Display(Name = "LabelGroupCoverage", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int GroupCoverage { get; set; }

        /// <summary>
        /// Placa
        /// </summary>
        [Display(Name = "LabelPlate", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[a-zA-Z]{3}[0-9]{3}|[a-zA-Z]{3}[0-9]{2}[a-zA-Z]{1}|[rRsS]{1}[0-9]{5}|[a-zA-z]{2}[0-9]{4}$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorValidateLicensePlate")]
        [StringLength(15)]
        public string LicensePlate { get; set; }

        /// <summary>
        /// Motor
        /// </summary>
        [Display(Name = "LabelEngine", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        [StringLength(50)]
        public string Engine { get; set; }

        /// <summary>
        /// Chasis
        /// </summary>
        [Display(Name = "LabelChassis", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        [StringLength(50)]
        public string Chassis { get; set; }

        /// <summary>
        /// Id marca
        /// </summary>
        [Display(Name = "LabelMake", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Make { get; set; }

        /// <summary>
        /// Id modelo
        /// </summary>
        //[Display(Name = "Model", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        //[Range(1, int.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Model { get; set; }

        /// <summary>
        /// Id Version
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Descripcion Marca
        /// </summary>
        [Display(Name = "LabelMake", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string MakeDescription { get; set; }

        /// <summary>
        /// Id tipo
        /// </summary>
        public int TypeVehicle { get; set; }

      
        /// <summary>
        /// Año
        /// </summary>
        //[Display(Name = "LabelYear", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        //[Range(1, int.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Year { get; set; }

        /// <summary>
        /// Trayecto
        /// </summary>
        [Display(Name = "LabelShuttle", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Shuttle { get; set; }

        /// <summary>
        /// Trayecto
        /// </summary>
        [Display(Name = "LabelService", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int ServiceType { get; set; }

        /// <summary>
        /// Deducible
        /// </summary>
        [Display(Name = "LabelDeductible", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Deductible { get; set; }

        /// <summary>
        /// Zona de tarifación
        /// </summary>
        [Display(Name = "LabelRateZone", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int RatingZone { get; set; }

        /// <summary>
        /// Cantidad de pasajeros
        /// </summary>
        [Display(Name = "LabelPassengerQuantity", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int PassengerQuantity { get; set; }

        /// <summary>
        /// Tipo de Tasa Id
        /// </summary>
        [Display(Name = "LabelRateType", ResourceType = typeof(App_GlobalResources.Language))]
        public int? RateType { get; set; }

        /// <summary>
        /// Tasa
        /// </summary>
        [Display(Name = "LabelRate", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"^\d+((\.\d+)*)+(\,\d{1,4})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [StringLength(18)]
        public string Rate { get; set; }

        /// <summary>
        /// Id asegurado
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int InsuredId { get; set; }

        /// <summary>
        /// Documento asegurado
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string InsuredDocumentNumber { get; set; }

        /// <summary>
        /// Nombre asegurado
        /// </summary>
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(100)]
        public string InsuredName { get; set; }

        /// <summary>
        /// Tipo de cliente Tomador
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int InsuredCustomerType { get; set; }

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
        /// Fecha de Nacimiento del Asegurado
        /// </summary>
        public DateTime? InsuredBirthDate { get; set; }

        /// <summary>
        /// Genero del Asegurado
        /// </summary>
        public string InsuredGender { get; set; }

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
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        //[RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public string Premium { get; set; }
        
        /// <summary>
        /// Titulo
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Id de Riesgo
        /// </summary>
        public int? OriginalRiskId { get; set; }


        [Display(Name = "LabelTransportedGoods", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int TypeCargoId { get; set; }


        public string TypeCargodescription { get; set; }

        public int Tons { get; set; }

        public int TrailerQuantity { get; set; }
       
        public string PhoneNumber { get; set; }

        public bool IsRetention { get; set; }
        public bool IsFacultative { get; set; }
        /// <summary>
        /// capacidad galones tanque  
        /// </summary>
        [Display(Name = "LabelGallonTankCapacity", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]

        public string GallonTankCapacity { get; set; }

        public bool RePoweredVehicle { get; set; }
        /// <summary>
        /// Año Repotenciado
        /// </summary>
        [Display(Name = "LabelRepoweringYear", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Range(4, int.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string RepoweringYear { get; set; }
        /// <summary>
        /// Año Modelo
        /// </summary>
        [Display(Name = "LabelYearModel", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Range(4, int.MaxValue, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string YearModel { get; set; }

    }
}