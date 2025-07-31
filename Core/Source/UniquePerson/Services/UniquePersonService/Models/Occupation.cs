using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Ocupacion
    /// </summary>
    [DataContract]
    public class Occupation : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Tipo de Ocupacion
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Abreviatura de tipo de Ocupacion
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

    }
}
