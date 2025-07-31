using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Especialidad de proveedores
    /// </summary>
    [DataContract]
    public class ProviderSpeciality : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Id de especitialidad 
        /// </summary>
        [DataMember]
        public int SpecialityId { get; set; }

        /// <summary>
        /// Id de proveedor
        /// </summary>
        [DataMember]
        public int ProviderId { get; set; }

    }
}
