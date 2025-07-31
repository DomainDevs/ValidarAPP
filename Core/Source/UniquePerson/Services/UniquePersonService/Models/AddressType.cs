using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Tipos de Direccion
    /// </summary>
    [DataContract]
    public class AddressType : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Tipo de dirección
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
