// -----------------------------------------------------------------------
// <copyright file="BranchAllianceViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    /// <summary>
    /// Modelo de vista de sucursal de aliado
    /// </summary>
    public class BranchAllianceViewModel
    {
        /// <summary>
        /// Gets or sets Identificador de sucursal
        /// </summary>
        [Display(Name = "LabelBranchCode", ResourceType = typeof(App_GlobalResources.Language))]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharactersBranchAlliance")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int BranchId { get; set; }

        /// <summary>
        /// Gets or sets Descripción de la sucursal
        /// </summary>
        [StringLength(50)]
        [Display(Name = "Description", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string BranchDescription { get; set; }

        /// <summary>
        /// Gets or sets Identificador del aliado
        /// </summary>
        [Display(Name = "LabelAllied", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int AllianceId { get; set; }

        /// <summary>
        /// Gets or sets Nombre del aliado
        /// </summary>
        public string AllianceName { get; set; }

        /// <summary>
        /// Gets or sets Ciudad de la sucursal
        /// </summary>
        [Display(Name = "LabelCity", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? CityCD { get; set; }

        /// <summary>
        /// Gets or sets Nombre de la Ciudad de la sucursal
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Gets or sets País de la sucursal
        /// </summary>
        [Display(Name = "LabelCountry", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? CountryCD { get; set; }

        /// <summary>
        /// Gets or sets Nombre des departamento de la sucursal
        /// </summary>        
        public string StateName { get; set; }

        /// <summary>
        /// Gets or sets Departamento de la sucursal
        /// </summary>
        [Display(Name = "LabelState", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? StateCD { get; set; }

        /// <summary>
        /// Gets or sets Nombre del país de la sucursal
        /// </summary>        
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets Estado.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Lista de puntos de venta
        /// </summary>
        public List<AllianceSalesPointsViewModel> SalesPoints { get; set; }
    }
}