// -----------------------------------------------------------------------
// <copyright file="CompanyPhoneTypeViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo de vista para el tipo teléfono
    /// </summary>
    public class CompanyPhoneTypeViewModel
    {
        /// <summary>
        /// Obtiene o establece Id del tipo teléfono
        /// </summary>
        public int PhoneTypeCode { get; set; }

        /// <summary>
        /// Obtiene o establece Descripción del tipo teléfono
        /// </summary>  
        [Display(Name = "LabelDescription", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        [StringLength(50)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece Descripción corta del tipo teléfono
        /// </summary>
        [Display(Name = "LabelDescriptionShort", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        [StringLength(15)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es celular
        /// </summary>
        [Display(Name = "LabelIsCellphone", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        public bool IsCellphone { get; set; }

        /// <summary>
        /// Obtiene o establece Expresión Regular del tipo teléfono
        /// </summary>
        [Display(Name = "LabelRegExpression", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        [StringLength(50)]
        public string RegExpression { get; set; }

        /// <summary>
        /// Obtiene o establece Mensaje de Error del tipo teléfono
        /// </summary>
        [Display(Name = "LabelErrorMessage", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        [StringLength(200)]
        public string ErrorMessage { get; set; }

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