using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Tipos de Individuo
    /// </summary>
    [DataContract]
    public class PersonIndividualType : Extension
    {
        /// <summary>
        /// Gets or sets the individual identifier.
        /// </summary>
        /// <value>
        /// The individual identifier.
        /// </value>
        [DataMember]
        public int IndividualId { get; set; }
        /// <summary>
        /// Gets or sets the person type code.
        /// </summary>
        /// <value>
        /// The person type code.
        /// </value>
        [DataMember]
        public int PersonTypeCode { get; set; }
    }
}
