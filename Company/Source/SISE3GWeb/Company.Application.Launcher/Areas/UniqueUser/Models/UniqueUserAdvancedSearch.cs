//-----------------------------------------------------------------------
// <copyright file="UniqueUserAdvancedSearch.cs" company="Sistran">
// Copyright (c) Sistran. All rights reserved.
// </copyright>
// <author>Julian Ospina</author>
//-----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo vista para la busqueda avanzada de usuarios
    /// </summary>
    public class UniqueUserAdvancedSearch
    {
        /// <summary>
        /// Obtiene o establece el nombre de usuario
        /// </summary>
        [Display(Name = "UserName", ResourceType = typeof(App_GlobalResources.Language))]
        [MaxLength(10)]
        [RegularExpression(@"^[ña-zÑA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        public string AccountName { get; set; }

        /// <summary>
        /// Obtiene o establece el Id de usuario
        /// </summary>
        [Display(Name = "LabelUserId", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(0, 99999, ErrorMessageResourceName = "ErrorUserIdRange", ErrorMessageResourceType = typeof(App_GlobalResources.Language))]
        public int? UserId { get; set; }

        /// <summary>
        /// Obtiene o establece el nro de identificacion de la persona asociada al usuario
        /// </summary>
        [Display(Name = "LabelIdentificationNumber", ResourceType = typeof(App_GlobalResources.Language))]
        public string IdentificationNumber { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de creacion del usuario
        /// </summary>
        [Display(Name = "CreationDate", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime? CreationDate { get; set; }
        
        /// <summary>
        /// Obtiene o establece el estado del usuario
        /// </summary>
        [Display(Name = "EnabledDisabled", ResourceType = typeof(App_GlobalResources.Language))]
        public int? Status { get; set; }
        
        /// <summary>
        /// Obtiene o establece la ultima fecha de modificacion del usuario
        /// </summary>
        [Display(Name = "DateLastModified", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime? LastModificationDate { get; set; }
    }
}