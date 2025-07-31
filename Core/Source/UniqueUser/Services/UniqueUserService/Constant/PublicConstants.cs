using System;

namespace Sistran.Core.Application.EEProvider.Constant
{
    /// <summary>
    /// Clase que contiene las constantes referidas al componente  
    /// Sistran.Core.Application.UniqueUser.Actions.
    /// </summary>
    [Serializable()]
    public class PublicConstants
    {
        /// <summary>
        /// Constante del Modulo asociado al Administrador de Usuario
        /// Unico.
        /// </summary>
        public static readonly int ADMIN_UU_MODULEID = 1;

        /// <summary>
        /// Constante del Submodulo asociado al Administrador de Usuario
        /// Unico.
        /// </summary>
        public static readonly int ADMIN_UU_SUBMODULEID = 1;

        /// <summary>
        ///	Constante que refiere al Nickname Administrador de Usuario Unico 
        /// </summary>
        public static readonly string ADMIN_UU_NICK = "fantasma";

        /// <summary>
        /// Código de autenticacion.
        /// </summary>
        public enum AUTHENTICATION_CODE : byte
        {
            Integrated = 1,
            Standard = 2
        }

    }
}
