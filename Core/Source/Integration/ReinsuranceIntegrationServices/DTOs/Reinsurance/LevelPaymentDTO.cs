using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    /// <summary>
    /// Modelo para Pagos de Nivel de contrato
    /// </summary>
    [DataContract]
    public class LevelPaymentDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Level
        /// </summary>
        [DataMember]
        public LevelDTO Level { get; set; }

        /// <summary>
        /// Número 
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Fecha
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// Cantidad, Moneda
        /// </summary>
        [DataMember]
        public AmountDTO Amount { get; set; }
    }
}
