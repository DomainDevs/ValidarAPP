using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.SecurityServices.Models.Base
{
    [DataContract]
    public class BaseUser : Extension
    {
        /// <summary>
        /// Obtiene o setea el Identificador del Usuario
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o setea el Identificador de la persona
        /// </summary>
        /// <value>
        /// Identificador persona
        /// </value>
        public int PersonId { get; set; }
        /// <summary>
        /// Obtiene o setea el Alias del Usuario 
        /// </summary>
        /// <param name="Nick"></param>
        /// <returns></returns>
        [DataMember]
        public string Nick { get; set; }


        /// <summary>
        /// Status 
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        [DataMember]
        public bool Status { get; set; }
    }
}
