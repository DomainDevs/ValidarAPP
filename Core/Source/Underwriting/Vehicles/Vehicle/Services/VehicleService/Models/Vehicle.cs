using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VEM = Sistran.Core.Application.Vehicles.Models;

namespace Sistran.Core.Application.Vehicles.VehicleServices.Models
{
    /// <summary>
    /// Vehículo - Riesgo
    /// </summary>
    [DataContract]
    public class Vehicle : BaseVehicle
    {

        public Vehicle()
        {
            Risk = new Risk();
        }

        [DataMember]
        public Risk Risk { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        [DataMember]
        public Color Color { get; set; }

        /// <summary>
        /// Uso
        /// </summary>
        [DataMember]
        public Use Use { get; set; }

        /// <summary>
        /// Accesorios
        /// </summary>
        [DataMember]
        public List<Accessory> Accesories { get; set; }
        
        /// <summary>
        /// Versión
        /// </summary>
        [DataMember]
        public VEM.Version Version { get; set; }

        /// <summary>
        /// Modelo
        /// </summary>
        [DataMember]
        public Model Model { get; set; }

        /// <summary>
        /// Marca
        /// </summary>
        [DataMember]
        public Make Make { get; set; }
    }
}
