// -----------------------------------------------------------------------
// <copyright file="LegalRepresentativeSingViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Printing.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Contiene las propiedades de Firma Representante Legal
    /// </summary>
    public class LegalRepresentativeSingViewModel
    {
        /// <summary>
        /// Obtiene o establece el CiaCode de Firma Representante Legal.
        /// </summary>
        [Display(Name = "LabelCompanyType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public decimal CiaCode { get; set; }

        /// <summary>
        /// Obtiene o establece el CiaDescription de Firma Representante Legal.
        /// </summary>
        public string CiaDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el BranchTypeCode de Firma Representante Legal.
        /// </summary>
        [Display(Name = "LabelBranchType", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public decimal BranchTypeCode { get; set; }

        /// <summary>
        /// Obtiene o establece el BranchTypeDescription de Firma Representante Legal.
        /// </summary>
        public string BranchTypeDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el CurrentFrom de Firma Representante Legal.
        /// </summary>
        [Display(Name = "LabelDateCurrentFrom", ResourceType = typeof(App_GlobalResources.Language))]
        //[DisplayFormat(DataFormatString = "dd/mm/yyyy")]
        //[Range(typeof(DateTime), "1/1/1910", "1/1/2020", ErrorMessageResourceName = "ErrorRangeDateTimeCurrentFrom", ErrorMessageResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Obtiene o establece el LegalRepresentative de Firma Representante Legal.
        /// </summary>
        [Display(Name = "LabelLegalRepresentative", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(200, ErrorMessageResourceName = "ErrorStringLengthLegalRepresentative", ErrorMessageResourceType = typeof(App_GlobalResources.Language))]
        public string LegalRepresentative { get; set; }

        /// <summary>
        /// Obtiene o establece el PathSignatureImg de Firma Representante Legal.
        /// </summary>
        public string PathSignatureImg { get; set; }

        /// <summary>
        /// Obtiene o establece el SignatureImg de Firma Representante Legal.
        /// </summary>
        public string SignatureImg { get; set; }

        /// <summary>
        /// Obtiene o establece el UserId de Firma Representante Legal.
        /// </summary>
        public string UserId { get; set; }
    }
}