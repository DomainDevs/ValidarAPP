using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///     Carga datos de asientos para reportes de Asienttos de Diario y de Mayor
    /// </summary>
    [DataContract]
    public class EntryDTO 
    {
        /// <summary>
        ///     Id del Asiento
        /// </summary>
        [DataMember]
        public int EntryId { get; set; }

        /// <summary>
        ///     Id del  Tipo de Comprobante
        /// </summary>
        [DataMember]
        public int AccountingMovementTypeId { get; set; }

        /// <summary>
        ///     Descripción del  Tipo de Comprobante
        /// </summary>
        [DataMember]
        public string AccountingMovementTypeDescription { get; set; }

        /// <summary>
        ///     Código de Moneda
        /// </summary>
        [DataMember]
        public int CurrencyCd { get; set; }

        /// <summary>
        ///     Descripción de Moneda
        /// </summary>
        [DataMember]
        public string CurrencyDescription { get; set; }

        /// <summary>
        ///     Id del Centro de costos
        /// </summary>
        [DataMember]
        public int CostCenterId { get; set; }

        /// <summary>
        ///     Descripción del Centro de costos
        /// </summary>
        [DataMember]
        public string CostCenterDescription { get; set; }

        /// <summary>
        ///     Id de Modulo Contable
        /// </summary>
        [DataMember]
        public int AccountingModuleId { get; set; }

        /// <summary>
        ///     Descripción de Modulo Contable
        /// </summary>
        [DataMember]
        public string AccountingModuleDescription { get; set; }

        /// <summary>
        ///     Id de del centro de Analisis asociado al Asiento
        /// </summary>
        [DataMember]
        public int EntryAnalysisId { get; set; }

        /// <summary>
        ///     Id del Punto de venta
        /// </summary>
        [DataMember]
        public int SalePointCd { get; set; }

        /// <summary>
        ///     Descripción del Punto de venta
        /// </summary>
        [DataMember]
        public string SalePointDescription { get; set; }

        /// <summary>
        ///     Id de Sucursal
        /// </summary>
        [DataMember]
        public int BranchCd { get; set; }

        /// <summary>
        ///     Descripción de Sucursal
        /// </summary>
        [DataMember]
        public string BranchDescription { get; set; }

        /// <summary>
        ///     Id de la Cuenta Contable
        /// </summary>
        [DataMember]
        public Decimal AccountingAccountId { get; set; }

        /// <summary>
        ///     Descripción del Cuenta Contable
        /// </summary>
        [DataMember]
        public string AccountingAccountDescription { get; set; }

        /// <summary>
        ///     Numero de cuenta Contable
        /// </summary>
        [DataMember]
        public decimal AccountingNumber { get; set; }

        /// <summary>
        ///     Id de Conciliacion bancaria
        /// </summary>
        [DataMember]
        public int BankReconciliationId { get; set; }

        /// <summary>
        ///     Descripción de Conciliacion bancaria
        /// </summary>
        [DataMember]
        public string BankReconciliationDescription { get; set; }

        [DataMember]
        public int AccountingCompanyId { get; set; }

        /// <summary>
        ///     Descripción de Compañia Contable
        /// </summary>
        [DataMember]
        public string AccountingCompanyDescription { get; set; }

        [DataMember]
        public int PaymentMovementTypeCd { get; set; }

        /// <summary>
        ///     Descripción de Tipo de pago
        /// </summary>
        [DataMember]
        public string PaymentMovementTypeDescription { get; set; }

        /// <summary>
        ///     Id de Endoso
        /// </summary>
        [DataMember]
        public int EntryDestinationId { get; set; }

        /// <summary>
        ///     Descripción de EntryDestination
        /// </summary>
        [DataMember]
        public string EntryDestinationDescription { get; set; }

        /// <summary>
        ///     Fecha del Asiento
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        ///     Fecha del recibo
        /// </summary>
        [DataMember]
        public DateTime ReceiptDate { get; set; }

        /// <summary>
        ///     Número del recibo
        /// </summary>
        [DataMember]
        public int ReceiptNumber { get; set; }

        /// <summary>
        ///     Distintivo entre asiento diario y Mayor
        /// </summary>
        [DataMember]
        public bool IsDailyEntry { get; set; }

        /// <summary>
        ///     Código de de Naturaleza contable
        /// </summary>
        [DataMember]
        public int AccountingNature { get; set; }

        /// <summary>
        ///     Descripción de Naturaleza contable
        /// </summary>
        [DataMember]
        public string AccountingNatureDescription { get; set; }

        /// <summary>
        ///     Descripción de Asiento
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Valor del asiento
        /// </summary>
        [DataMember]
        public decimal AmountValue { get; set; }

        /// <summary>
        ///     Valor del asiento en moneda en monedad de emisión
        /// </summary>
        [DataMember]
        public decimal AmountLocalValue { get; set; }

        /// <summary>
        ///     Valor de tasa de Cambio de moneda
        /// </summary>
        [DataMember]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        ///     Numero de Asiento
        /// </summary>
        [DataMember]
        public int EntryNumber { get; set; }

        /// <summary>
        ///     Numero de Asiento como texto
        /// </summary>
        [DataMember]
        public string EntryNumberDescription { get; set; }

        //DailyEntry

        /// <summary>
        ///     Id de Asiento diario
        /// </summary>
        [DataMember]
        public int DailyEntryId { get; set; }

        /// <summary>
        ///     Id de Cabecera de Asiento Diario
        /// </summary>
        [DataMember]
        public int DailyEntryHeaderId { get; set; }

        /// <summary>
        ///     Nùmero de Transacción
        /// </summary>
        [DataMember]
        public int TransactionNumber { get; set; }

        /// <summary>
        ///     Id de Tipo de Imputación
        /// </summary>
        [DataMember]
        public int ImputationCode { get; set; }

        /// <summary>
        ///     Descripción de Cabecera de Asiento Diario
        /// </summary>
        [DataMember]
        public string DailyEntryHeaderDescription { get; set; }

        /// <summary>
        ///     Descripción de Tipo de Imputación
        /// </summary>
        [DataMember]
        public string ImputationTypeDescription { get; set; }

        /// <summary>
        ///     Número Filas del dto
        /// </summary>
        [DataMember]
        public int Rows { get; set; }
    }
}