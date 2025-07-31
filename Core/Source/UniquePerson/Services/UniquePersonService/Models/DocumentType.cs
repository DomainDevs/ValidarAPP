using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Tipo de Documento
    /// </summary>
    [DataContract]
    public class DocumentType : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Tipo de documento
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Abreviatura
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
