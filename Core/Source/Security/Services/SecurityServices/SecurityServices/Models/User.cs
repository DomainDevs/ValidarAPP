using Sistran.Core.Application.SecurityServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.SecurityServices.Models
{
    /// <summary>
    /// Usuario
    /// </summary>
    [DataContract]
    public class User : BaseUser
    {

        /// <summary>
        /// Obtiene o setea una lista de Perfiles
        /// </summary>
        /// <value>
        /// Perfil
        /// </value>
        public List<Profile> Profiles { get; set; }

        /// <summary>
        /// AccessObjects 
        /// </summary>
        /// <param name="List<AccessObject>"></param>
        /// <returns>List<AccessObject></returns>
        [DataMember]
        public List<OperationObject> OperationObjects { get; set; }
    }
}
