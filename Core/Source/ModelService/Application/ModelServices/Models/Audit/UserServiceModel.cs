using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.Audit
{
    /// <summary>
    /// Usuario
    /// </summary>
    [DataContract]
    public class UserServiceModel
    {
        
        /// <summary>
        ///Identificador del Asuario
        /// </summary>
        /// <value>
        /// Identificador
        /// </value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Nombre Usuario
        /// </summary>
        /// <value>
        /// Nombre Usuario
        /// </value>
        [DataMember]
        public string Description { get; set; }

    }
}
