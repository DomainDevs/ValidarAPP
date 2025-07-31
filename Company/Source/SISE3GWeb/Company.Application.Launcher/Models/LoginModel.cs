// -----------------------------------------------------------------------
// <copyright file="LoginModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Desconocido</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Models
{
    using Sistran.Company.Application.UniqueUserServices.Models;
    using System.Collections.Concurrent;
   using System.Collections.Generic;
   using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo cuenta usuario
    /// </summary>
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
}
