//-----------------------------------------------------------------------
// <copyright file="VehicleBodyViewModel.cs" company="Sistran">
// Copyright (c) Sistran. All rights reserved.
// </copyright>
// <author>Andres Gonzalez</author>
//-----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo para las Carrocería de vehículo
    /// </summary>
    public class VehicleBodyViewModel
    {
        /// <summary>
        /// Obtiene o establece Id de la Carrocería de vehículo
        /// </summary>
        [Display(Name = "LabelVehicleBodyCode", ResourceType = typeof(App_GlobalResources.Language))]
        public int? BodyCode { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion corta de Carrocería de vehículo
        /// </summary>
        [Display(Name = "LabelBodyVehicleShortDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15)]
        [Required(ErrorMessageResourceName = "ErrorRequiredBodyVehicleShortDescription", ErrorMessageResourceType = typeof(App_GlobalResources.Language))]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Obtiene o establece Usos asociados ala Carrocería de vehículo
        /// </summary>
        public List<int> VehicleUses { get; set; }

        /// <summary>
        /// Obtiene o establece Estado del registro de parametrizacion
        /// </summary>
        public int State { get; set; }
    }
}