namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// Comisión de aplicación de primas
    /// </summary>
    public class TempApplicationPremiumCommiss
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador de la aplicación de primas
        /// </summary>
        public int TempApplicationPremiumId { get; set; }
        /// <summary>
        /// Tipo de comision
        /// </summary>
        public int CommissionType { get; set; }

        /// <summary>
        /// Identificador del tipo de agente
        /// </summary>
        public int AgentTypeId { get; set; }

        /// <summary>
        /// Identificador de agente
        /// </summary>
        public int AgentId { get; set; }

        /// <summary>
        /// Id del Intermediario
        /// </summary>
        public int AgentAgencyId { get; set; }
        /// <summary>
        /// Tipo de Monead
        /// </summary>
        public int Currency { get; set; }

        /// <summary>
        /// Tipo de cambio
        /// </summary>
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Monto de la comisión
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Monto de la comisión Local
        /// </summary>
        public decimal LocalAmount { get; set; }
    }
}
