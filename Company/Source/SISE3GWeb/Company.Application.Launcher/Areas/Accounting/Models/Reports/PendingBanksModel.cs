using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports   
{
     [KnownType("PendingBanksModel")]
    public class PendingBanksModel
    {
        [Required]
        public int BranchCode { get; set; }
        [Required]
        public string BranchName { get; set; }
        [Required]
        public int BankId { get; set; }
        [Required]
        public string BankName { get; set; }
        [Required]
        public int AccountType { get; set; }
        [Required]
        public string AccountTypeName { get; set; }
        [Required]
        public string EndDate { get; set; }
        [Required]
        public int UserCode { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public int CurrencyId { get; set; }
        [Required]
        public string Currency { get; set; }
         [Required]
        public string AccountNumber { get; set; }
         

         //DETALLES
         //Conciliación
         [Required]
         public int ReconciliationNumber { get; set; }
         [Required]
         public string ReconciliationType { get; set; }
         [Required]
         public string ReconciliationDate { get; set; }
         
         //bancos
        [Required]
        public string Date { get; set; }
        [Required]
        public string VoucherNumber { get; set; }
        [Required]
        public int MovementCode { get; set; }
        [Required]
        public string Amount { get; set; }
        [Required]
        public string TotalAmount { get; set; }
        [Required]
        public string MovementDescription { get; set; }
    }
}