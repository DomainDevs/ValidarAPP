using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Especialidad
    /// </summary>
    [DataContract]
    public class Speciality : BaseGeneric
    {

        /// <summary>
        /// Opcion predeterminada
        /// </summary>
        [DataMember]
        public bool? IsDefault { get; set; }
    }
}
