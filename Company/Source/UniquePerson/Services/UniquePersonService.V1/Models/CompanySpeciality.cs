using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    /// <summary>
    /// Especialidad
    /// </summary>
    [DataContract]
    public class CompanySpeciality : BaseGeneric
    {

        /// <summary>
        /// Opcion predeterminada
        /// </summary>
        [DataMember]
        public bool? IsDefault { get; set; }
    }
}
