using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Estado Civil
    /// </summary>
    [DataContract]
    public class MaritalStatus : Extension
    {
        /// <summary>
        /// Identificador del MaritalStatus
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion Estado Civil
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Descripcion Corta Estado Civil
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }

}
