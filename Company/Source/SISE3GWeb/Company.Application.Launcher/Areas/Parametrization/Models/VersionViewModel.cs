// -----------------------------------------------------------------------
// <copyright file="VersionViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Eder Camio Ramirez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    /// <summary>
    /// Modelo de las propiedades de Version.
    /// </summary>    
    public class VersionViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la version del vehiculo
        /// </summary>
        ///[Range(0, 255, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorIdCoveredRiskType")]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripción corta de la version.
        /// </summary>
        //[Display(Name = "LabelDescriptionShort", ResourceType = typeof(App_GlobalResources.Language))]
        //[StringLength(15)]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }
    }
}