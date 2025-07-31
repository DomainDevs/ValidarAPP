// -----------------------------------------------------------------------
// <copyright file="EndoTypeViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Eder Camio Ramirez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;
    /// <summary>
    /// Modelo de los tipos de endoso.
    /// </summary>   
    public class EndoTypeViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la version del vehiculo
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")] 
        public int Id { get; set; }
        /// <summary>
        /// Obtiene o establece la Descripción corta de la version.
        /// </summary>
        public string Description { get; set; }
    }
}