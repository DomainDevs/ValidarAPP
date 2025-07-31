using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.Audit
{
    /// <summary>
    /// Entidades Sistema
    /// </summary>
    [DataContract]
    public class EntityServiceModel
    {
        /// <summary>
        ///Identificador de la entidad
        /// </summary>
        /// <value>
        /// Identificador
        /// </value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Nombre Entidad
        /// </summary>
        /// <value>
        /// Nombre Entidad
        /// </value>
        [DataMember]
        public string Description { get; set; }

    }
}
