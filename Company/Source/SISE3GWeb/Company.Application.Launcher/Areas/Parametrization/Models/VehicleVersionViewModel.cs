// -----------------------------------------------------------------------
// <copyright file="VersionViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gomez Hernandez</author>
// -----------------------------------------------------------------------
using Sistran.Core.Application.EntityServices.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    /// <summary>
    /// Modelo de las propiedades de Version .
    /// </summary>    
    public class VehicleVersionViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la version
        /// </summary>
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "CodeVersion")]
        public int Id { get; set; }
        /// <summary>
        /// Obtiene o establece la marca de la version
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Make")]
        public int VehicleMakeServiceQueryModel { get; set; }
        /// <summary>
        /// Obtiene o establece el modelo de la version
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Model")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int VehicleModelServiceQueryModel { get; set; }
        /// <summary>
        /// Obtiene o establece el descripcion de la version
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(50)]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "NameVersion")]
        public string Description { get; set; }
        /// <summary>
        /// Obtiene o establece la cilindrada del motor de la version
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "EngineQuantity")]
        [Range(typeof(decimal), "0", "9999", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        public int? EngineQuantity { get; set; }
        /// <summary>
        /// Obtiene o establece los caballos de fuerza de la version
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "HorsePower")]
        [Range(typeof(decimal), "0", "255", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        public int? HorsePower { get; set; }
        /// <summary>
        /// Obtiene o establece el paso de la version
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Weight")]
        [Range(typeof(decimal), "0", "32767", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        public int? Weight { get; set; }
        /// <summary>
        /// Obtiene o establece las toneladas de la version
        /// </summary>   
        /// 
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "TonsQuantity")]
        [Range(typeof(decimal), "0", "255", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        public int? TonsQuantity { get; set; }
        /// <summary>
        /// Obtiene o establece el cantidad de pasajeros de la version
        /// </summary>  
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "PassengerQuantity")]
        [Range(typeof(decimal), "0", "32767", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        public int? PassengerQuantity { get; set; }
        /// <summary>
        /// Obtiene o establece el tipo de combustible de la version
        /// </summary> 
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "FuelType")]
        public int? VehicleFuelServiceQueryModel { get; set; }
        /// <summary>
        /// Obtiene o establece el tipo de carroceria de la version
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Body")]
        public int VehicleBodyServiceQueryModel { get; set; }
        /// <summary>
        /// Obtiene o establece el tipo  de la version
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "VehicleType")]
        public int VehicleTypeServiceQueryModel { get; set; }
        /// <summary>
        /// Obtiene o establece el tipo de transmision de la version
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "TransmissionType")]
        public int? VehicleTransmissionTypeServiceQueryModel { get; set; }
        /// <summary>
        /// Obtiene o establece la velocidad maxima de la version
        /// </summary> 
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "TopSpeed")]
        [Range(typeof(decimal), "0", "255", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        public int? MaxSpeedQuantity { get; set; }
        /// <summary>
        /// Obtiene o establece la cantidad de puertas de la version
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "DoorQuantity")]
        [Range(typeof(decimal), "0", "255", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        public int? DoorQuantity { get; set; }
        /// <summary>
        /// Obtiene o establece el precio de la version
        /// </summary>  
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Price")]
        [Range(typeof(decimal), "0", "9999999999999999", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Obtiene o establece si es importado de la version
        /// </summary>  
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "IsImported")]
        public bool IsImported { get; set; }
        /// <summary>
        /// Obtiene o establece si es ultimo modelo de la version
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "LastModel")]
        public bool? LastModel { get; set; }
        /// <summary>
        /// Obtiene o establece si es ultimo modelo de la version
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.Language), Name = "Currency")]
        public int? Currency { get; set; }
        public StatusTypeService StatusTypeService { get; set; }
        /// <summary>
        /// Obtiene o establece la descripcion de la marca
        /// </summary>   
        public string DescriptionMake { get; set; }
        /// <summary>
        /// Obtiene o establece la descripcion del modelo
        /// </summary>   
        public string DescriptionModel { get; set; }

    }
}