using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Tipo de Compañia
    /// </summary>
    [DataContract]
    public class CompanyType : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion Tipo de compañia
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
