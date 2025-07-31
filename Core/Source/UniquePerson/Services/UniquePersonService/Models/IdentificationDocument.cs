using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Tipo de Identificacion
    /// </summary>
    [DataContract]
    public class IdentificationDocument : BaseIdentificationDocument
    {
        /// <summary>
        /// Tipo de documento
        /// </summary>
        [DataMember]
        public DocumentType DocumentType { get; set; }

    }
}
