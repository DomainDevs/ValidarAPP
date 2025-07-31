using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Contragarantias Documentacion
    /// </summary>
    [DataContract]
    public class GuaranteeRequiredDocument : Extension
    {
        /// <summary>
        /// Obtiene o Setea el Identificador
        /// </summary>
        /// <value>
        /// Identificador
        /// </value>
        [DataMember]
        public int GuaranteeCode { get; set; }

        /// <summary>
        /// Codigo Documento
        /// </summary>
        /// <value>
        /// Codigo Documento
        /// </value>
        [DataMember]
        public int DocumentCode { get; set; }

        /// <summary>
        /// Descripcion Documentacion requerida
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
