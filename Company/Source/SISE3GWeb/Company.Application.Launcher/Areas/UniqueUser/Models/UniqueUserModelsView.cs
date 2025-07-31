using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Configuration;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class UniqueUserModelsView
    {
        /// <summary>
        /// PersonId
        /// </summary>
        [Display(Name = "NameSurname", ResourceType = typeof(App_GlobalResources.Language))]
        public string UserId { get; set; }



        /// <summary>
        /// Nombre de Inicio
        /// </summary>
        [Display(Name = "LoginName", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(30)]
        [RegularExpression("^[A-Za-z0-9]*$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharactersandSpace")]
        public string AccountName { get; set; }

        ///// <summary>
        ///// PersonName
        ///// </summary>
        //[Display(Name = "NameSurname", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        //public string PersonName { get; set; }

        /// <summary>
        /// PersonId
        /// </summary>
        [Display(Name = "NameSurname", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string PersonId { get; set; }

        /// <summary>
        /// Perfil
        /// </summary>
        [Display(Name = "Profile", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string ProfileId { get; set; }


        /// <summary>
        /// Fecha de creación
        /// </summary>
        [Display(Name = "CreationDate", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Fecha de ultima modificación
        /// </summary>
        [Display(Name = "DateLastModifid", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public DateTime LastModificationDate { get; set; }

        /// <summary>
        /// Fecha de expiracion
        /// </summary>
        public DateTime DateExpirationUser { get; set; }

        public DateTime? LockDate { get; set; }

        /// <summary>
        /// Fecha Deshabilitado usuario
        /// </summary>      
        [Display(Name = "LabelDisabledDate", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime? DisableDate { get; set; }

        /// <summary>
        /// Fecha de expiracion contraseña
        /// </summary>
        public DateTime DateExpirationPassword { get; set; }


        [DataType(DataType.Password)]
        [RegularExpression(@"^[`~!@#$%^&*¿¡(\\)\""_|/+/\-=?;:',\\.{}[\] a-zA-Z0-9ñÑ]*$", ErrorMessage = "No se admiten caracteres especiales. ")]
        [Display(Name = "Password", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [RegularExpression(@"^[`~!@#$%^&*¿¡(\\)\""_|/+/\-=?;:',\\.{}[\] a-zA-Z0-9ñÑ]*$", ErrorMessage = "No se admiten caracteres especiales. ")]
        [Display(Name = "Confirmation", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [Compare("Password", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "PassConfirmationDontMatch")]
        public string Confirmation { get; set; }


        public List<ProfileModelsView> Profiles { get; set; }

        public UniqueUserLoginModelsView UniqueUsersLogin { get; set; }

        public int CreatedUserId { get; set; }

        public Application.UniqueUserServices.Enums.UniqueUserTypes.AuthenticationType AuthenticationType { get; set; }
        public int ModifiedUserId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public List<CoHierarchyAssociationModelsView> Hierarchies { get; set; }
        public List<Branch> Branch { get; set; }

        public List<IndividualRelationAppModelsView> IndividualsRelation { get; set; }
                
        public string Status { get; set; }

        public string Name { get; set; }

        public List<UserGroupModelsView> UserGroup { get; set; }
    }
}