// -----------------------------------------------------------------------
// <copyright file="ProductViewModel.cs" company="SISTRAN">
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
    public class ProductViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id del producto
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")] 
        public int Id { get; set; }
        /// <summary>
        /// Obtiene o establece la Descripción corta del producto
        /// </summary>
        public string Description { get; set; }
    }
}