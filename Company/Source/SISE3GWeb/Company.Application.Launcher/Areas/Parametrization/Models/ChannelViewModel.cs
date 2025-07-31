// -----------------------------------------------------------------------
// <copyright file="ChannelViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;
    using Sistran.Core.Services.UtilitiesServices.Models;

    /// <summary>
    /// Contiene las propiedades de canal
    /// </summary>
    public class ChannelViewModel
    {
        /// <summary>
        /// Obtiene o establece el SourceCode del ServiceQuotationSource.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el Description del ServiceQuotationSource.
        /// </summary>
        [Display(Name = "LabelDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el Comments del ServiceQuotationSource.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si IsEnabled del ServiceQuotationSource esta habilitado o no.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Obtiene o establece el DetailDescription del ServiceQuotationSource.
        /// </summary>
        [StringLength(50)]
        [Display(Name = "LabelDescriptionProduct", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string DetailDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el IsScore del ServiceQuotationSource.
        /// </summary>
        public bool? IsScore { get; set; }

        /// <summary>
        /// Obtiene o establece el IsFine del ServiceQuotationSource.
        /// </summary>
        public bool? IsFine { get; set; }

        /// <summary>
        /// Obtiene o establece el ValuesDefault del ServiceQuotationSource.
        /// </summary>
        public ValuesDefault ValuesDefault { get; set; }
    }
}