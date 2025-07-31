// -----------------------------------------------------------------------
// <copyright file="FasecoldaViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Eder Ramirez</author>
// -----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    /// <summary>
    /// Modelo de Parametrizacion de Fasecolda
    /// </summary>
    public class FasecoldaViewModel
    {

        /// <summary>
        /// Marca del vehiculo
        /// </summary>
        public MakeViewModel makeVehicle { get; set; }

        /// <summary>
        /// Modelo del Vehiculo
        /// </summary>
        public ModelViewModel modelVehicle { get; set; }

        /// <summary>
        /// Version del vehiculo
        /// </summary>
        public VersionViewModel versionVehicle { get; set; }


        /// <summary>
        /// Codigo de Marca de Vehiculo
        /// </summary>
        [Display(Name = "LabelBrandCodeFasecolda", ResourceType = typeof(App_GlobalResources.Language))]
        [MaxLength(3, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMax3Numbers")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string MakeVehicleCode { get; set; }

        /// <summary>
        /// Codigo de Modelo de Vehiculo
        /// </summary>
        [Display(Name = "LabelModelCodeFasecolda", ResourceType = typeof(App_GlobalResources.Language))]
        [MaxLength(5, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMax5Numbers")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string ModelVehicleCode { get; set; }

        /// <summary>
        /// Estado del registro de parametrizacion
        /// </summary>
        public int State { get; set; }
    }
}