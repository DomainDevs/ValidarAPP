using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.Enums;
using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{

    /// <summary>
    /// Direcciones
    /// </summary>
    [DataContract]
    public class Address : BaseAddress
    {
        /// <summary>
        /// Ciudad
        /// </summary>
        [DataMember]
        public City City { get; set; }

        /// <summary>
        /// Zona de la dirección
        /// </summary>
        [DataMember]
        public AddressZoneType AddressZone { get; set; }

        /// <summary>
        /// Tipo de Direccion
        /// </summary>
        [DataMember]
        public AddressType AddressType { get; set; }

    }
}
