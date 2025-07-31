using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Tipo Asociacion
    /// </summary>
    [DataContract]
    public class AssociationType : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Tipo de asociación
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}