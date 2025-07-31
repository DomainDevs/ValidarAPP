// -----------------------------------------------------------------------
// <copyright file="AllianceViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo de vista del aliado.
    /// </summary>
    public class AllianceViewModel
    {
        /// <summary>
        /// Gets or sets Código del aliado.
        /// </summary>
        public int AlliedCode { get; set; }

        /// <summary>
        /// Gets or sets Nombre del aliado.
        /// </summary>
        [Display(Name = "LabelDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether consultar DataCrédito
        /// </summary>
        public bool IsScore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether tiene multas de tránsito.
        /// </summary>
        public bool IsFine { get; set; }

        /// <summary>
        /// Gets or sets Etiqueta de servicios a consultar.
        /// </summary>
        public string ServicesDescription { get; set; }

        /// <summary>
        /// Gets or sets Estado.
        /// </summary>
        public string Status { get; set; }
    }
}