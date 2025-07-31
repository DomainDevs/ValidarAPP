using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Roles de la Personas
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UniquePersonService.Models.Person" />
    [DataContract]
    public class Rol
    {
        /// <summary>
        /// Id del Rol
        /// </summary>        
        [DataMember]
        public int  Id { get; set; }


        /// <summary>
        /// Id del SubRol
        /// </summary>        
        [DataMember]
        public int? SubRoleId { get; set; }
    }
}
