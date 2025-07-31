using System.ComponentModel.DataAnnotations;

// Sistran
using Sistran.Core.Framework.UIF.Web.Resources;


namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class PaymentConceptModel
    {
        public string Id { get; set; }

        [Required]
        [Display(ResourceType = typeof(Global), Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Código de impuesto")]
        public int TaxCd { get; set; }

        [Display(ResourceType = typeof(Global), Name = "Tax")]
        public string TaxDescription { get; set; }

        [Display(ResourceType = typeof(Global), Name = "ExchangeRate")]
        public string Rate { get; set; }

        [Required]
        public int GeneralLedgerCd { get; set; }

        [Required]
        [Display(Name = "Código Cuenta Contable")]
        public int AccountingAccountCd { get; set; }

        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingAccount")]
        public string AccountNumber { get; set; }

        [Required]
        [Display(ResourceType = typeof(Global), Name = "AccountingAccountName")]
        public string AccountingAccountDescription { get; set; }

        public int BranchId { get; set; }
    }
}