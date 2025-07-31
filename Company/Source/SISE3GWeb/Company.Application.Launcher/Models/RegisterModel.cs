// -----------------------------------------------------------------------
// <copyright file="RegisterModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jaime Trujillo</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// modelo de registro
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Obtiene o establece el User Name
        /// </summary>
        [Required]
        [RegularExpression("^[A-Za-z0-9]*$", ErrorMessage = "No se admiten caracteres especiales. ")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        /// <summary>
        /// Obtiene o establece la contraseña
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[`~!@#$%^&*¿¡(\\)\""_|/+/\-=?;:',\\.{}[\] a-zA-Z0-9ñÑ]*$", ErrorMessage = "No se admiten caracteres especiales. ")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Obtiene o establece la confirmacion de la contraseña
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[`~!@#$%^&*¿¡(\\)\""_|/+/\-=?;:',\\.{}[\] a-zA-Z0-9ñÑ]*$", ErrorMessage = "No se admiten caracteres especiales. ")]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Password", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "NewAndConfirmPasswordDontMatch")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Obtiene o establece Contraseña Actual
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[`~!@#$%^&*¿¡(\\)\""_|/+/\-=?;:',\\.{}[\] a-zA-Z0-9ñÑ]*$", ErrorMessage = "No se admiten caracteres especiales. ")]
        [Display(Name = "Actual password")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Obtiene o establece RegExpPassword
        /// </summary>
        public string RegExpPassword { get; set; }

        /// <summary>
        /// Obtiene o establece Condiciones de contraseña
        /// </summary>
        public string PasswordConditions { get; set; }
    }
}