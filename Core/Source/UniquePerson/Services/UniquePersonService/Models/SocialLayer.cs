using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Informacion Social
    /// </summary>
    [DataContract]
    public class SocialLayer : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Tipo de nivel Educativo
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Abreviatura de tipo de email
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
