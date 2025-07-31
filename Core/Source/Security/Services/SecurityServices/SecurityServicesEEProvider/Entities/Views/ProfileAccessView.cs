using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.SecurityServices.EEProvider.Entities.Views
{

    /// <summary>
    /// Definición de la vista que vincula User con todos los accesos y 
    /// entidades relacionadas.
    /// </summary>
    [Serializable()]
    public class ProfileAccessView : BusinessView
    {
        /// <summary>
        /// Constructor de instancias de vista.
        /// </summary>
        public ProfileAccessView()
        { }

        /// <summary>
        /// Colección de elementos de la entidad <see 
        /// cref="Sistran.Core.UniqueAccess.Entities.Access">Access
        /// </see>
        /// </summary>
        /// <value>
        /// Colección de Access.
        /// </value>
        public BusinessCollection Access
        {
            get
            {
                return this["Access"];
            }
        }

       
        /// <summary>
        /// Colección de elementos de la entidad <see 
        /// cref="Sistran.Core.Application.UniqueUser.Entities.AccessProfile">AccessProfile
        /// </see>
        /// </summary>
        /// <value>
        /// Colección de AccessProfile.
        /// </value>
        public BusinessCollection AccessProfile
        {
            get
            {
                return this["AccessProfile"];
            }
        }
        
    }
}
