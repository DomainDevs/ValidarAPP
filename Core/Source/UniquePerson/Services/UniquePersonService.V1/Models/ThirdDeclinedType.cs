using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Tipo de tercero declinado (motivo de baja de un tercero)
    /// </summary>
    [DataContract]
    public class ThirdDeclinedType : Extension
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
