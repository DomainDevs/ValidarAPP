using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Motivos de Baja Asegurado
    /// </summary>
    [DataContract]
    public class InsuredDeclinedType : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Descripcion del motivo de baja del asegurado
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Abreviatura del motivo de baja del asegurado
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
