using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Usuario de la Persona
    /// </summary>
    [DataContract]
    public class UserPerson : Extension
    {
        /// <summary>
        /// Obtiene o establece el identificador del usuario.
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Obtiene o establece el atributo para la propiedad Código de la tabla Person
        /// </summary>
        [DataMember]
        public int PersonId { get; set; }

        /// <summary>
        /// Obtiene o establece el atributo para la propiedad Fecha de Expiración.
        /// </summary>
        [DataMember]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Obtiene o establece el atributo para la propiedad Apellido.
        /// </summary>
        [DataMember]
        public string Surname { get; set; }

        /// <summary>
        /// Obtiene o establece el atributo para la propiedad Nombre.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece el atributo para la propiedad Apellido Materno.
        /// </summary>
        [DataMember]
        public string MotherLastName { get; set; }
    }
}
