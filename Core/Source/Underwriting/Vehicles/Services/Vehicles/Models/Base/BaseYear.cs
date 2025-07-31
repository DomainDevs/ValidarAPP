using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.Models.Base
{
    /// <summary>
    /// Años de la versión del Vehiculo
    /// </summary>
    [DataContract]
    public class BaseYear:Extension
    {
        /// <summary>
        /// Año
        /// </summary>
        [DataMember]
        public int Description { get; set; }

        /// <summary>
        /// Precio
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }

    }
}
