using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.Models.Base
{
    /// <summary>
    /// Versión
    /// </summary>
    [DataContract]
    public class BaseVersion : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Versión
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// propeidad cantidad Pasajeros
        /// </summary>
        [DataMember]
        public int PassengerQuantity { get; set; }

        /// <summary>
        /// propiedad IsImported
        /// </summary>
        [DataMember]
        public bool IsImported { get; set; }

        /// <summary>
        /// propiedad DoorQuantity
        /// </summary>
        [DataMember]
        public int? DoorQuantity { get; set; }

        /// <summary>
        /// Propiedad Weight
        /// </summary>
        [DataMember]
        public int? Weight { get; set; }

        /// <summary>
        /// Propiedad WeightCategory
        /// </summary>
        [DataMember]
        public int? WeightCategory { get; set; }

        /// <summary>
        /// propiedad TonsQuantity
        /// </summary>
        [DataMember]
        public int? TonsQuantity { get; set; }

        /// <summary>
        /// propiedad NewVehiclePrice
        /// </summary>
        [DataMember]
        public decimal? NewVehiclePrice { get; set; }
        /// <summary>
        /// Es importado
        /// </summary>
        [DataMember]
        public int? IaVehicleVersion { get; set; }

        /// <summary>
        /// Es importado
        /// </summary>
        [DataMember]
        public int? PartialLossBase { get; set; }

        /// <summary>
        /// Es importado
        /// </summary>
        [DataMember]
        public bool AirConditioning { get; set; }

        /// <summary>
        /// Es importado
        /// </summary>
        [DataMember]
        public int? VehicleAxleQuantity { get; set; }

        /// <summary>
        /// Es importado
        /// </summary>
        [DataMember]
        public bool LastModel { get; set; }

        /// <summary>
        /// Tipo vehiculo
        /// </summary>
        [DataMember]
        public string Novelty { get; set; }

        /// <summary>
        /// Estatus
        /// </summary>
        [DataMember]
        public string Status { get; set; }

    }
}
