using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AccountingEntryModel
    {
        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        public int AccountingEntryId { get; set; }

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
        /// Compañía utilizado unicamente para Contabilidad
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Company")]
        public int CompanyId { get; set; }

        /// <summary>
        /// Enumeración para Naturaleza Contable
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingNatureId")]
        public int AccountingNatureId { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Currency")]
        public int CurrencyId { get; set; }

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
        /// Código y nombre del concepto contable
        /// </summary>
        
        [Display(ResourceType = typeof(Global), Name = "CodeConcept")]
        public int AccountingConceptId { get; set; }

        /// <summary>
        /// Id de la Cuenta contable
        /// </summary>
        [Required]
        public int AccountingAccountId { get; set; }

        /// <summary>
        /// Número y Nombre de la Cuenta contable
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingAccount")]
        public int AccountingAccountNumber { get; set; }

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
        /// Tipo de Comprobante
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "VoucherType")]
        public int AccountingMovementTypeId { get; set; }

        /// <summary>
        /// Conciliación bancaria
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "BankConciliation")]
        public int BankReconciliationId { get; set; }

        /// <summary>
        /// Tipo de Cambio
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "ExchangeRate")]
        public Decimal ExchangeRate { get; set; }

        /// <summary>
        /// Importe
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Amount")]
        public Decimal Amount { get; set; }

        /// <summary>
        /// Fecha de operación
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "OperationDate")]
        [DataType(DataType.Date)]
        public string Date { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "MovementDescription")]
        public string Description { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "MovementDescription")]
        public string DailyEntryDescription { get; set; }

        /// <summary>
        /// Destino
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Destination")]
        public int DestinationId { get; set; }

        /// <summary>
        /// Listado de Centros de Costos
        /// </summary>
        public List<CostCenterEntryModel> CostCenters { get; set; }

        /// <summary>
        /// Listado de Analisis
        /// </summary>
        public List<AnalysisModel> Analyses { get; set; }

        /// <summary>
        /// Listado de Postfechados
        /// </summary>
        public List<PostDatedModel> Postdated { get; set; }
    }
}