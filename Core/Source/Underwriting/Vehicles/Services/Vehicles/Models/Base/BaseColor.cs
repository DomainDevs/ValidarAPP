using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.Models.Base
{
    /// <summary>
    /// Color del Vehiculo
    /// </summary>
    [DataContract]
    public class BaseColor : Extension
    {
        /// <summary>
        /// Identificador Color del Vehiculo
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion Color del Vehiculo
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
