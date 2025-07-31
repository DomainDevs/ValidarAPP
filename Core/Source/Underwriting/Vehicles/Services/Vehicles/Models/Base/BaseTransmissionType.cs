using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.Models.Base
{
    /// <summary>
    /// Tipo de transmision
    /// </summary>
    [DataContract]
    public class BaseTransmissionType:Extension
    {
        /// <summary>
        /// Identificador de tipo de transmision
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion tipo de transmision
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
