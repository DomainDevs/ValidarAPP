// -----------------------------------------------------------------------
// <copyright file="CompanyAddressTypeViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo de vista para el tipo dirección
    /// </summary>
    public class CompanyAddressTypeViewModel
    {
        /// <summary>
        /// Obtiene o establece Id del tipo dirección
        /// </summary>
        public int AddressTypeCode { get; set; }

        /// <summary>
        /// Obtiene o establece Descripción larga del tipo dirección
        /// </summary>  
        [Display(Name = "LabelDescription", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        [StringLength(15)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece Descripción corta del tipo dirección
        /// </summary>
        [Display(Name = "LabelAbbreviation", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        [StringLength(3)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        public string TinyDescription { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es correo electrónico
        /// </summary>
        [Display(Name = "LabelIsElectronicMail", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        public bool IsElectronicMail { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si tiene foráneos asociados
        /// </summary>
        public bool IsForeing { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si se puede eliminar
        /// </summary>
        public bool AllowDelete { get; set; }
    }
}