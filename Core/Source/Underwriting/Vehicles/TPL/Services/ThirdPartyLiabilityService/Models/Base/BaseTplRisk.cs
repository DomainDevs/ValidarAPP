using Sistran.Core.Application.Extensions;
using Sistran.Core.Application.TaxServices;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.Models.Base
{
    [DataContract]
    public class BaseTplRisk : Extension
    {
        /// <summary>
        /// Placa
        /// </summary>
        [DataMember]
        public string LicensePlate { get; set; }

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
        /// Año
        /// </summary>
        [DataMember]
        public int Year { get; set; }

        /// <summary>
        /// Tasa
        /// </summary>
        [DataMember]
        public decimal Rate { get; set; }

        /// <summary>
        /// Tipo Tasa
        /// </summary>
        [DataMember]
        public RateType? RateType { get; set; }

        /// <summary>
        /// Cantidad de Pasajeros
        /// </summary>
        [DataMember]
        public int PassengerQuantity { get; set; }

    }
}
