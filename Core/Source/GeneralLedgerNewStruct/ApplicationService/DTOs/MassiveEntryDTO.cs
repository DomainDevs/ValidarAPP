using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    /// <summary>    
    ///     DTO para usarse en la carga masiva de asientos de mayor.
    /// </summary>
    [DataContract]
    public class MassiveEntryDTO 
    {
        /// <summary>
        ///     Identificador de tabla
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        // Campos de Cabecera.

        /// <summary>
        ///     Sucursal
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        ///     Pto de Venta
        /// </summary>
        [DataMember]
        public int SalePointId { get; set; }

        /// <summary>
        ///     Compañía Contable
        /// </summary>
        [DataMember]
        public int AccoutingCompanyId { get; set; }

        /// <summary>
        ///     Destino
        /// </summary>
        [DataMember]
        public int EntryDestinationId { get; set; }

        /// <summary>
        ///     Tipo de Comprobante
        /// </summary>
        [DataMember]
        public int AccountingMovementTypeId { get; set; }

        /// <summary>
        ///     Fecha de operación
        /// </summary>
        [DataMember]
        public DateTime OperationDate { get; set; }

        /// <summary>
        ///     Moneda
        /// </summary>
        [DataMember]
        public int CurrencyId { get; set; }

        /// <summary>
        ///     Tasa de cambio
        /// </summary>
        [DataMember]
        public decimal ExchangeRate { get; set; }

        //Campos de movimientos.

        /// <summary>
        ///     Campo para indentificar el número de orden del movimiento, esto se lo usa el momento de registrar errores.
        /// </summary>
        [DataMember]
        public int RowNumber { get; set; }

        /// <summary>
        ///     Código de cuenta contable
        /// </summary>
        [DataMember]
        public int AccountingAccountId { get; set; }

        /// <summary>
        ///     Naturaleza contable
        /// </summary>
        [DataMember]
        public int AccountingNature { get; set; }

        /// <summary>
        ///     Importe en moneda de emisión
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        ///     Id de abonador
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        ///     Descripción del movimiento
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Id de conciliación bancaria
        /// </summary>
        [DataMember]
        public int? BankReconciliationId { get; set; }

        /// <summary>
        ///     Número de recibo para conciliación
        /// </summary>
        [DataMember]
        public int ReceiptNumber { get; set; }

        /// <summary>
        ///     Fecha del comprobante
        /// </summary>
        [DataMember]
        public DateTime? ReceiptDate { get; set; }

        //Campos de Centros de costos

        /// <summary>
        ///     Centro de Costos
        /// </summary>
        [DataMember]
        public int? CostCenterId { get; set; }

        /// <summary>
        ///     Porcentaje del Centro de Costos
        /// </summary>
        [DataMember]
        public decimal? Percentage { get; set; }

        //campos de Análisis

        /// <summary>
        ///     Código de Análisis
        /// </summary>
        [DataMember]
        public int? AnalysisId { get; set; }

        /// <summary>
        ///     Concepto
        /// </summary>
        [DataMember]
        public int? ConceptId { get; set; }

        /// <summary>
        ///     Clave de Concepto
        /// </summary>
        [DataMember]
        public string ConceptKey { get; set; }

        /// <summary>
        ///     Descripción
        /// </summary>
        [DataMember]
        public string AnalysisDescription { get; set; }

        //campos de postfechados

        /// <summary>
        ///     Tipo de postfechado
        /// </summary>
        [DataMember]
        public int? PostdatedId { get; set; }

        /// <summary>
        ///     Moneda de postfechado
        /// </summary>
        [DataMember]
        public int? PostdatedCurrencyId { get; set; }

        /// <summary>
        ///     Tasa de cambio de postfechado
        /// </summary>
        [DataMember]
        public decimal? PostdatedExchangeRate { get; set; }

        /// <summary>
        ///     Numero de documento de postfechado
        /// </summary>
        [DataMember]
        public string PosdatedDocumentNumber { get; set; }

        /// <summary>
        ///     Importe de postfechados
        /// </summary>
        [DataMember]
        public decimal? PostdatedAmount { get; set; }
    }
}