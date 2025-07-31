using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class EntryTypeAccountingModel
    {
        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        [Required]
        public int EntryTypeAccountingId { get; set; }

        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        [Required]
        public int EntryTypeCd { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string Description { get; set; }


        /// <summary>
        /// Enumeración para Naturaleza Contable
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingNatureId")]
        public int AccountingNatureId { get; set; }

        /// <summary>
        ///  Naturaleza Contable Description
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingNatureId")]
        public string AccountingNatureDescription { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Currency")]
        public int CurrencyId { get; set; }

        /// <summary>
        /// Moneda Descripcion
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "Currency")]
        public string CurrencyDescription { get; set; }

        /// <summary>
        /// Cambio
        /// </summary>
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Id de la Cuenta contable
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingAccount")]
        public int AccountingAccountId { get; set; }

        /// <summary>
        /// Numero de la Cuenta contable
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "AccountingAccount")]
        public string AccountingAccountNumber { get; set; }

        /// <summary>
        ///  Cuenta contable
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingAccount")]
        public string AccountingAccountDescription { get; set; }

        /// <summary>
        /// Id de la Tipo de Movimiento
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "MovementType")]
        public int AccountingMovementTypeId { get; set; }

        /// <summary>
        ///  Tipo de Movimiento
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "MovementType")]
        public string AccountingMovementTypeDescription { get; set; }

        /// <summary>
        ///  Analisis
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Analysis")]
        public int AnalysisId { get; set; }


        /// <summary>
        /// Descricion  Analisis
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "Analysis")]
        public string AnalysisDescription { get; set; }

        /// <summary>
        /// Id de Centro de Costos
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "CostCenter")]
        public int CostCenterId { get; set; }

        /// <summary>
        ///  Centro de Costos
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "CostCenter")]
        public string CostCenterDescription { get; set; }

        /// <summary>
        ///  Concepto Pago
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Concept")]
        public int PaymentConceptCd { get; set; }
        
        /// <summary>
        /// Descricion  Concepto Pago
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "Concept")]
        public string PaymentConceptDescription { get; set; }

        /// <summary>
        /// Número + Nombre de cuenta, usado para autocomplete
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "Concept")]
        public string AccountingAccountNumberName { get; set; }
    }
}