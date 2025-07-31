using Sistran.Company.Application.UniqueUserServices.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Models
{


    public class LoginModel
    {

        /// <summary>
        /// Obtiene o establece el Nombre de usuario
        /// </summary>
        [Required(ErrorMessage = "Campo requerido")]
        [RegularExpression("^[-_, @.A-Za-z0-9]*$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "NotRegExAllowed")]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        /// <summary>
        /// Obtiene o establece la contraseña del usuario
        /// </summary>
        [RegularExpression(@"^[`~!@#$%^&*¿¡(\\)\""_|/+/\-=?;:',\\.{}[\] a-zA-Z0-9ñÑ]*$", ErrorMessage = "No se admiten caracteres especiales. ")]
        [Required(ErrorMessage = "Campo requerido")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si quiere recordar contraseña
        /// </summary>
        [Display(Name = "Recordar?")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// Obtiene o establece id user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Mostrar contraseña
        /// </summary>
        public int LockPassword { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si esta en sesion
        /// </summary>
        public bool InSession { get; set; }

        /// <summary>
        /// Obtiene o establece Intentos de sesión
        /// </summary>
        public int SessionAttempts { get; set; }

        /// <summary>
        /// Obtiene o establece Lista de grupos de usuario
        /// </summary>
        public List<CompanyUserGroup> UserGroup { get; set; }

        /// <summary>
        /// Obtiene o establece Conexiones
        /// </summary>
        public ConcurrentBag<string> Conections { get; set; }

    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class SessionModel
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string UserName { get; set; }
        public int AgentId { get; set; }

        public int AgencyId { get; set; }
    }

    
}
