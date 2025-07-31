// -----------------------------------------------------------------------
// <copyright file="VehicleVersionYearViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;
    using Sistran.Core.Application.EntityServices.Enums;

    /// <summary>
    /// Valor por año del vehículo
    /// </summary>
    public class VehicleVersionYearViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id de valor por año del vehiculo
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el Id de la marca del vehiculo
        /// </summary>
        public int MakeId { get; set; }

        /// <summary>
        /// Obtiene o establece el Id del modelo del vehiculo
        /// </summary>
        public int ModelId { get; set; }

        /// <summary>
        /// Obtiene o establece el Id de la versión del vehiculo
        /// </summary>
        public int VersionId { get; set; }

        /// <summary>
        /// Obtiene o establece el Año del vehiculo
        /// </summary>
        [Display(Name = "LabelYear", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(1000, 9999, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Year { get; set; }

        /// <summary>
        /// Obtiene o establece el Id de la moneda usada para el precio de vehiculo
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Obtiene o establece el Precio del vehiculo
        /// </summary>
        [Display(Name = "LabelVehiclePrice", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(0, 9999999999999.99, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public decimal Price { get; set; }

        /// <summary>
        ///  Obtiene o establece el Estado de item - (CRUD)
        /// </summary>
        public StatusTypeService Status { get; set; }

        /// <summary>
        /// Obtiene o establece el Código Fasecolda para agilizar o facilitar las busquedas avanzadas
        /// </summary>
        [Display(Name = "LabelFasecoldaCode", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(0, 99999999, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]        
        public decimal FasecoldaCode { get; set; }
    }
}