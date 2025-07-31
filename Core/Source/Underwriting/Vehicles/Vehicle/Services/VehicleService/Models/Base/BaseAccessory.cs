using Sistran.Core.Application.Extensions;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.Vehicles.VehicleServices.Models.Base
{
    [DataContract]
    public class BaseAccessory : Extension
    {
        /// <summary>
        /// Identificador Accesorio
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion del Accesorio
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Accesorio Original Si/No
        /// </summary>
        [DataMember]
        public bool IsOriginal { get; set; }

        /// <summary>
        /// Valor del Accesorio
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// Valor original del Accesorio
        /// </summary>
        [DataMember]
        public decimal OriginalAmount { get; set; }

        /// <summary>
        /// Marca del Accesorio
        /// </summary>
        [DataMember]
        public string Make { get; set; }

        /// <summary>
        /// Id del Accesorio relacionado
        /// </summary>
        [DataMember]
        public string AccessoryId { get; set; }

        /// <summary>
        /// Prima del Accesorio
        /// </summary>
        [DataMember]
        public decimal Premium { get; set; }

        /// <summary>
        /// Tipo Tasa
        /// </summary>
        [DataMember]
        public RateType? RateType { get; set; }

        /// <summary>
        /// Tasa|
        /// </summary>
        [DataMember]
        public decimal Rate { get; set; }

        /// <summary>
        /// Prima acumulada del Accesorio
        /// </summary>
        [DataMember]
        public decimal AccumulatedPremium { get; set; }

        /// <summary>
        /// Codigo del Accesorio
        /// </summary>
        [DataMember]
        public int RiskDetailId { get; set; }

        /// <summary>
        /// Estado del  Accesorio
        /// </summary>
        [DataMember]
        public int Status { get; set; }
    }
}
