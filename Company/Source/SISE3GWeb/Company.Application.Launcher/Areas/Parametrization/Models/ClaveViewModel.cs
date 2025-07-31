// -----------------------------------------------------------------------
// <copyright file="ClaveViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Wilfrido Heredia Carrera</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;
    /// <summary>
    /// Modelo de de la clave o balo arternativo de la entidad Min_premium_relation.
    /// </summary>   
    public class ClaveViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")] 
        public int Id { get; set; }
        /// <summary>
        /// Obtiene o establece la Descripción 
        /// </summary>
        public string Description { get; set; }
    }
}