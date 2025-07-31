using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Authorization.Server.Models
{
    public class LoginModel
    {
        /// <summary>
        /// Obtiene o establece el Nombre de usuario
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Obtiene o establece la contraseña del usuario
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Obtiene o establece la contraseña del usuario
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si quiere recordar contraseña
        /// </summary>
        //[Display(Name = "Recordar?")]
        //public bool RememberMe { get; set; }

        ///// <summary>
        ///// Obtiene o establece id user
        ///// </summary>
        //public int Id { get; set; }

        ///// <summary>
        ///// Obtiene o establece Mostrar contraseña
        ///// </summary>
        //public int LockPassword { get; set; }

        ///// <summary>
        ///// Obtiene o establece un valor que indica si esta en sesion
        ///// </summary>
        //public bool InSession { get; set; }

        ///// <summary>
        ///// Obtiene o establece Intentos de sesión
        ///// </summary>
        //public int SessionAttempts { get; set; }

        ///// <summary>
        ///// Obtiene o establece Lista de grupos de usuario
        ///// </summary>
        //public List<CompanyUserGroup> UserGroup { get; set; }

        ///// <summary>
        ///// Obtiene o establece Conexiones
        ///// </summary>
        //public ConcurrentBag<string> Conections { get; set; }

   // }
}
}