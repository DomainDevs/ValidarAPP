using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AccountingAccountOrderModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int AccountingAccountOrderId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingAccountOrderDescription")]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "LedgerAccountNumber")]
        public string Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Module")]
        public int Module { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "SubModule")]
        public int SubModule { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "EndorsementType")]
        public int EndorsementTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "EndorsementType")]
        public string EndorsementTypeDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Nature")]
        public int AccountingNatureCd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "AccountingAccountOrderDescription")]
        public string ModuleDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "SubModule")]
        public string SubModuleDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "Nature")]
        public string AccountingNatureCdDescription { get; set; }

        /// <summary>
        /// Campo que se lo utiliza para guardar el id de la relación entre cuentas de orden para cargar en la tabla de relación.
        /// </summary>
        public int AccountOrderRelationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "BusinessType")]
        public int BusinessTypeCd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "BusinessType")]
        public string BusinessTypeDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "IsAssessValue")]
        public bool IsAssessValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "IsAssessValue")]
        public string IsAssessValueDescription { get; set; }

        /// <summary>
        /// Código de Moneda
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Currency")]
        public int CurrencyCd { get; set; }
        /// <summary>
        /// Descripción de moneda
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "Currency")]
        public string CurrencyDescription { get; set; }

        /// <summary>
        /// Código de Componente
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "Component")]
        public int ComponentCd { get; set; }

        /// <summary>
        /// Descripción de Componente
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = " Component")]
        public string ComponentDescription { get; set; }
    }
}