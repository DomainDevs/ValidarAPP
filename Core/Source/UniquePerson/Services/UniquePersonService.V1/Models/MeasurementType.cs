using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Tipo de Medida
    /// </summary>
    [DataContract]
    public class MeasurementType : Extension
    {
        /// <summary>
        /// Obtiene o Setea el Identificador
        /// </summary>
        /// <value>
        /// Identificador
        /// </value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o Setea Descripcion
        /// </summary>
        /// <value>
        /// Descripcion
        /// </value>
        [DataMember]
        public string Description { get; set; }
    }
}
