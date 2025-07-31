using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    [DataContract]
    public class TempApplicationPremiumCommissDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador de la aplicación de primas
        /// </summary>
        [DataMember]
        public int TempApplicationPremiumId { get; set; }
        /// <summary>
        /// Tipo de comision
        /// </summary>
        [DataMember]
        public int CommissionType { get; set; }

        /// <summary>
        /// Identificador del tipo de agente
        /// </summary>
        [DataMember]
        public int AgentTypeId { get; set; }

        /// <summary>
        /// Identificador de agente
        /// </summary>
        [DataMember]
        public int AgentId { get; set; }

        /// <summary>
        /// Id del Intermediario
        /// </summary>
        [DataMember]
        public int AgentAgencyId { get; set; }
        /// <summary>
        /// Tipo de Monead
        /// </summary>
        [DataMember]
        public int CurrencyCode { get; set; }

        /// <summary>
        /// Tipo de cambio
        /// </summary>
        [DataMember]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Monto de la comisión
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }
        /// <summary>
        /// Monto de la comisión Local
        /// </summary>
        [DataMember]
        public decimal LocalAmount { get; set; }
        [DataMember]
        public decimal BaseAmount { get; set; }

        [DataMember]
        public decimal PremiumReceivableId { get; set; }

    }
}