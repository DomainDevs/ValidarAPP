using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports
{
    [KnownType("AutomaticAmortizationModel")]
    public class AutomaticAmortizationModel
    {
        [Required]
        public int ProcessAmortization { get; set; }
        [Required]
        public DateTime ProcessDate { get; set; }
        [Required]
        public int UserCode { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public decimal TotalDebits { get; set; }
        [Required]
        public decimal TotalCredits { get; set; }

        //detalle
        [Required]
        public int BranchId { get; set; }
        [Required]
        public string BranchName { get; set; }
        [Required]
        public int PrefixId { get; set; }
        [Required]
        public string PrefixName { get; set; }

        [Required]
        public int PolicyId { get; set; }
        [Required]
        public string Policy { get; set; }
        [Required]
        public int Endorsement { get; set; }
        [Required]
        public string Insured { get; set; }

        [Required]
        public string Payer { get; set; }
        [Required]
        public string PrincipalAgent { get; set; }
        [Required]
        public int CurrencyId { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public decimal Change { get; set; }
        [Required]
        public decimal LocalAmount { get; set; }
        [Required]
        public string ApplicationReceiptNumber { get; set; }
        [Required]
        public string EntryNumber { get; set; }
        [Required]
        public int ProcessNumber { get; set; }
    }
}