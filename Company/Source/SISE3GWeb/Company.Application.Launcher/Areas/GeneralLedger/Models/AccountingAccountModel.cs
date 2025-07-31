using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AccountingAccountModel
    {
        /// <summary>
        /// Id de la cuenta contable
        /// </summary>
        public int AccountingAccountId { get; set; }

        /// <summary>
        /// Id de la cuenta contable padre
        /// </summary>
        public int AccountingAccountParentId { get; set; }

        /// <summary>
        /// Numero de la cuenta contable
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingAccount")]
        public string AccountingAccountNumber { get; set; }

        /// <summary>
        /// Nombre de la cuenta contable
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingAccountName")]
        public string AccountingAccountName { get; set; }

        /// <summary>
        /// Id de sucursal
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "Branch")]
        public int BranchId { get; set; }

        /// <summary>
        /// Id de naturaleza contable
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingNatureId")]
        public int AccountingNatureId { get; set; }

        /// <summary>
        /// Id de moneda
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(Global), Name = "Currency")]
        public int CurrencyId { get; set; }

        /// <summary>
        /// Requiere análisis?
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "Analysis")]
        public int RequireAnalysis { get; set; }

        /// <summary>
        /// Código por defecto de Analisis
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "AnalysisCode")]
        public int AnalysisId { get; set; }

        /// <summary>
        /// Requiere centro de Costos
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "CostCenter")]
        public int RequireCostCenter { get; set; }

        /// <summary>
        /// centro de Costos 
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "CostCenter")]
        public List<int> CostCenters { get; set; }

        /// <summary>
        /// Prefixes : Ramos 
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "Prefixes")]
        public int PrefixId { get; set; }

        /// <summary>
        /// Comments: Observaciones de la cuenta
        /// </summary>        
        [Display(ResourceType = typeof(Global), Name = "Observations")]
        public string Comments { get; set; }

        /// <summary>
        /// AccountingAccountApplication: Tipo de Aplicacion de Cuenta
        /// </summary>
        public int AccountingAccountApplication { get; set; }

        /// <summary>
        /// AccountingAccountType: Tipo de Cuenta
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "AccountType")]
        public int AccountingAccountType { get; set; }

        /// <summary>
        /// FullName: Nombre completo de la cuenta "Número de cuenta - Nombre de la cuenta"
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Id Reclasificacion
        /// </summary>
        public bool IsReclassify { get; set; }

        /// <summary>
        /// Id Reevaluo
        /// </summary>
        public bool IsRevalue { get; set; }


        /// <summary>
        /// ReevaluePositive: Número de Reevaluo Positivo
        /// </summary>

        [Display(ResourceType = typeof(Global), Name = "AccountingPlanApplicationPositive")]
        public string ReevaluePositive { get; set; }

        /// <summary>
        /// ReevaluePositive: Número de Reevaluo Negativo
        /// </summary>

        [Display(ResourceType = typeof(Global), Name = "AccountingPlanApplicationNegative")]
        public string ReevalueNegative { get; set; }

        /// <summary>
        /// ReevaluePositive: Cuenta a Reclasificar
        /// </summary>
        [Display(ResourceType = typeof(Global), Name = "AccountingPlanAccounttoreclassify")]
        public string AccountClassify { get; set; }

    }
}