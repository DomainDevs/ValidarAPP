using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Especialidad
    /// </summary>
    [DataContract]
    public class Speciality : Extension
    {

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int? Id { get; set; }
        /// <summary>
        /// Tipo de Especialidad
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Abreviatura de tipo de Especialidad
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Opcion predeterminada
        /// </summary>
        [DataMember]
        public bool? IsDefault { get; set; }
    }
}
