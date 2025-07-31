// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    [KnownType("LedgerEntryModel")]
    public class LedgerEntryModel
    {
        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        public int LedgerEntryId { get; set; }

        /// <summary>
        /// Compañía Contable
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingCompany")]
        public int AccountingCompanyId { get; set; }

        /// <summary>
        /// Tipo de movimiento contable
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingMovementType")]
        public int AccountingMovementTypeId { get; set; }

        /// <summary>
        /// Módulo contable
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingModule")]
        public int AccountingModuleId { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Branch")]
        public int BranchId { get; set; }

        /// <summary>
        /// Punto de Venta
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "SalePoint")]
        public int SalePointId { get; set; }

        /// <summary>
        /// Destino
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Destination")]
        public int EntryDestinationId { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "MovementDescription")]
        public string Description { get; set; }

        /// <summary>
        /// Fecha de operación
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "OperationDate")]
        [DataType(DataType.Date)]
        public string Date { get; set; }

        /// <summary>
        /// Listado de ítems
        /// </summary>
        public List<LedgerEntryItemModel> LedgerEntryItems { get; set; }

    }

    [KnownType("LedgerEntryItemModel")]
    public class LedgerEntryItemModel
    {
        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        public int LedgerEntryItemId { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Currency")]
        public int CurrencyId { get; set; }

        /// <summary>
        /// Id de la Cuenta contable
        /// </summary>
        [Required]
        public int AccountingAccountId { get; set; }

        /// <summary>
        /// Número de cuenta contable
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingAccount")]
        public string AccountingAccountNumber { get; set; }

        /// <summary>
        /// Conciliación bancaria
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "BankConciliation")]
        public int BankReconciliationId { get; set; }

        /// <summary>
        /// Fecha de Comprobante
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "DateVoucher")]
        public string ReceiptDate { get; set; }

        /// <summary>
        /// Número de Comprobante
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "VoucherNumber")]
        public int ReceiptNumber { get; set; }

        /// <summary>
        /// Enumeración para Naturaleza Contable
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingNatureId")]
        public int AccountingNatureId { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "MovementDescription")]
        public string Description { get; set; }

        /// <summary>
        /// Tipo de Cambio
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "ExchangeRate")]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Importe
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Importe en moneda local
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "LocalAmount")]
        public decimal LocalAmount { get; set; }

        /// <summary>
        /// IndividualId de Individuo o Compañia
        /// </summary>
        public int IndividualId { get; set; }

        /// <summary>
        /// Número de Documento de Individuo o Compañia
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "DocumentNumber")]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Asiento tipo
        /// </summary>
        public int EntryTypeId { get; set; }

        /// <summary>
        /// Listado de Centros de Costos
        /// </summary>
        public List<CostCenterEntryModel> CostCenters { get; set; }

        /// <summary>
        /// Listado de Análisis
        /// </summary>
        public List<AnalysisModel> Analysis { get; set; }

        /// <summary>
        /// Listado de Postfechados
        /// </summary>
        public List<PostDatedModel> Postdated { get; set; }
    }
}