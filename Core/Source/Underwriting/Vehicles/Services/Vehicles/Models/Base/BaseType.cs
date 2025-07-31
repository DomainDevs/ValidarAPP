using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.Models.Base
{
    /// <summary>
    /// Tipo de Vehiculo 
    /// </summary>
    [DataContract]
    public class BaseType:Extension
    {
        /// <summary>
        /// Identificador Tipo de Vehiculo 
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion Tipo de Vehiculo 
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Minima Descripcion Tipo de Vehiculo 
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
