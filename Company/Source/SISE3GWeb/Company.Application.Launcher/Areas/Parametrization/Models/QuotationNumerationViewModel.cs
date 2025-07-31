// -----------------------------------------------------------------------
// <copyright file="QuotationNumerationViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Méndez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de vista para numeración de cotización
    /// </summary>
    public class QuotationNumerationViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la numeración
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el Id del ramo
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public int PrefixId { get; set; }

        /// <summary>
        /// Obtiene o establecela descripción del ramo
        /// </summary>
        public string PrefixDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el número de la última cotización
        /// </summary>
        [Display(Name = "LastQuotation", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorQuotationNumberRequired")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Range(0, 2147483647, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ValError")]
        [DataMember]
        public int LastQuotation { get; set; }

        /// <summary>
        /// Obtiene o establece el Id de la sucursal
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción de la sucursal
        /// </summary>
        public string BranchDescription { get; set; }
    }
}