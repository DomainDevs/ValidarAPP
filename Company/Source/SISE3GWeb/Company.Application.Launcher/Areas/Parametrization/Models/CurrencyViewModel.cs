// -----------------------------------------------------------------------
// <copyright file="CurrencyViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Wilfrido Heredia Carrera</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;
    /// <summary>
    /// Modelo de los moneda.
    /// </summary>   
    public class CurrencyViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la moneda
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")] 
        public int Id { get; set; }
        /// <summary>
        /// Obtiene o establece la Descripción corta de la moneda.
        /// </summary>
        public string Description { get; set; }
    }
}