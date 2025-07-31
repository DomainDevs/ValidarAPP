using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Tipo de Proveedor declinado (motivo de baja de un proveedor)
    /// </summary>
    [DataContract]
    public class ProviderDeclinedType : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Tipo de descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Descripcion abreviada
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

    }
}
