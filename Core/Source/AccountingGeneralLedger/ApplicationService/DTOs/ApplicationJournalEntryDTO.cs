using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class ApplicationJournalEntryDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador de moneda
        /// </summary>
        [DataMember]
        public int CurrencyId { get; set; }

        /// <summary>
        /// Identificador contable
        /// </summary>
        [DataMember]
        public int AccountAccountingId { get; set; }

        /// <summary>
        /// Naturaleza contable
        /// </summary>
        [DataMember]
        public int AccountingNature { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Monto
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// Monto en moneda local
        /// </summary>
        [DataMember]
        public decimal LocalAmount { get; set; }

        /// <summary>
        /// Tasa de cambio
        /// </summary>
        [DataMember]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Identificador del individuo
        /// </summary>
        [DataMember]
        public int? IndividualId { get; set; }

        /// <summary>
        /// Identificador del objeto origen
        /// </summary>
        [DataMember]
        public int SourceCode { get; set; }

        /// <summary>
        /// Identificador del concepto contable
        /// </summary>
        [DataMember]
        public int? AccountingConceptId { get; set; }

        /// <summary>
        /// Identificador de la sucursal
        /// </summary>
        [DataMember]
        public int? BranchId { get; set; }

        /// <summary>
        /// Identificador del punto de venta
        /// </summary>
        [DataMember]
        public int? SalePointId { get; set; }

        /// <summary>
        /// Número de recibo
        /// </summary>
        [DataMember]
        public int? ReceiptNumber { get; set; }

        /// <summary>
        /// Fecha del recibo
        /// </summary>
        [DataMember]
        public DateTime? ReceiptDate { get; set; }

        /// <summary>
        /// Identificador de la reconciliación
        /// </summary>
        [DataMember]
        public int? ReconciliationCode { get; set; }

        /// <summary>
        /// Identificador de moneda
        /// </summary>
        [DataMember]
        public DateTime? ReconciliationDate { get; set; }

        /// <summary>
        /// Código del paquete de reglas a buscar
        /// </summary>
        [DataMember]
        public string PackageRuleCodeId { get; set; }

        /// <summary>
        /// Identificador de la cuenta puente
        /// </summary>
        [DataMember]
        public int BridgeAccountId { get; set; }

        /// <summary>
        /// centros de costos
        /// </summary>
        [DataMember]
        public List<CostCenterDTO> AccountingCostCenters { get; set; }

        /// <summary>
        /// Conceptos De analysis
        /// </summary>
        [DataMember]
        public List<AnalysisDTO> AccountingAnalysisCodes { get; set; }
}
}
