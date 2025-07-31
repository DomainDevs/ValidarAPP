// -----------------------------------------------------------------------
// <copyright file="MakeViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Eder Camio Ramirez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo de las propiedades de Marca.
    /// </summary>    
    public class MakeViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la marca
        /// </summary>
        ///[Range(0, 255, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorIdCoveredRiskType")]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripción corta del tipo de Marca.
        /// </summary>
        //[Display(Name = "LabelDescriptionShort", ResourceType = typeof(App_GlobalResources.Language))]
        //[StringLength(15)]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }
    }
}