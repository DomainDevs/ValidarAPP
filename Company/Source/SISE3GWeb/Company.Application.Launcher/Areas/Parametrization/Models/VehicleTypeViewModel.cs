//-----------------------------------------------------------------------
// <copyright file="VehicleTypeViewModel.cs" company="Sistran">
// Copyright (c) Sistran. All rights reserved.
// </copyright>
// <author>Julian Ospina</author>
//-----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo para los tipos de vehiculos
    /// </summary>
    public class VehicleTypeViewModel
    {
        /// <summary>
        /// Id del tipo de vehiculo
        /// </summary>
        [Display(Name = "LabelTypeVehicleCode", ResourceType = typeof(App_GlobalResources.Language))]
        public int? TypeCode { get; set; }

        /// <summary>
        /// Descripcion del tipo de vehiculo
        /// </summary>
        [Display(Name = "LabelTypeVehicleDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50)]
        [Required(ErrorMessageResourceName = "ErrorRequiredTypeVehicleDescription", ErrorMessageResourceType = typeof(App_GlobalResources.Language))]
        public string Description { get; set; }

        /// <summary>
        /// Descripcion corta del tipo de vehiculo
        /// </summary>
        [Display(Name = "LabelTypeVehicleShortDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15)]
        [Required(ErrorMessageResourceName = "ErrorRequiredTypeVehicleShortDescription", ErrorMessageResourceType = typeof(App_GlobalResources.Language))]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Indica si el tipo de vehiculo es un camion
        /// </summary>
        [Display(Name = "LabelTypeVehicleIsTruck", ResourceType = typeof(App_GlobalResources.Language))]
        public bool IsTruck { get; set; }

        /// <summary>
        /// Indica si el tipo de vehiculo se encuentra activo
        /// </summary>
        [Display(Name = "LabelTypeVehicleIsActive", ResourceType = typeof(App_GlobalResources.Language))]
        public bool IsActive { get; set; }

        /// <summary>
        /// Carrocerias asociadas al tipo de vehiculo
        /// </summary>
        public List<int> VehicleBodies { get; set; }

        /// <summary>
        /// Estado del registro de parametrizacion
        /// </summary>
        public int State { get; set; }
    }
}