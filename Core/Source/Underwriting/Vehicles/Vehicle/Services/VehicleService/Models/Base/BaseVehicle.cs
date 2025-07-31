using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.VehicleServices.Models.Base
{
    [DataContract]
    public class BaseVehicle : Extension
    {
        /// <summary>
        /// Año
        /// </summary>
        [DataMember]
        public int Year { get; set; }

        /// <summary>
        /// Precio Nuevo
        /// </summary>
        [DataMember]
        public decimal NewPrice { get; set; }

        /// <summary>
        /// Precio
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }

        /// <summary>
        /// Placa
        /// </summary>
        [DataMember]
        public string LicensePlate { get; set; }

        /// <summary>
        /// Es Nuevo?
        /// </summary>
        [DataMember]
        public bool IsNew { get; set; }

        /// <summary>
        /// Serial del Motor
        /// </summary>
        [DataMember]
        public string EngineSerial { get; set; }

        /// <summary>
        /// Serial del Chasis
        /// </summary>
        [DataMember]
        public string ChassisSerial { get; set; }

        /// <summary>
        /// Tasa Simple
        /// </summary>
        [DataMember]
        public decimal SingleRate { get; set; }

        /// <summary>
        /// Tasa
        /// </summary>
        [DataMember]
        public decimal Rate { get; set; }

        /// <summary>
        /// Precio Accesorios
        /// </summary>
        [DataMember]
        public decimal PriceAccesories { get; set; }


        /// <summary>
        /// Tipo de Carga
        /// </summary>
        [DataMember]
        public int LoadTypeCode { get; set; }

        /// <summary>
        /// Cantidad Trailers
        /// </summary>
        [DataMember]
        public int TrailersQuantity { get; set; }

        /// <summary>
        /// Cantidad Pasajeros
        /// </summary>
        [DataMember]
        public int PassengerQuantity { get; set; }

        /// <summary>
        /// Precio Estandar
        /// </summary>
        [DataMember]
        public decimal StandardVehiclePrice { get; set; }

        /// <summary>
        /// Es Camión?
        /// </summary>
        [DataMember]
        public bool IsTruck { get; set; }

        /// <summary>
        /// Es Importado?
        /// </summary>
        [DataMember]
        public bool IsImported { get; set; }

        /// <summary>
        /// Precio Original
        /// </summary>
        [DataMember]
        public decimal OriginalPrice { get; set; }
    }
}
