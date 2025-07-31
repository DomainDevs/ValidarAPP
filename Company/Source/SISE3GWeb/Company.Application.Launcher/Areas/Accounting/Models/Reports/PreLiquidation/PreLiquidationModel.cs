using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports.PreLiquidation
{
    [KnownType("PreLiquidationModel")]
    public class PreLiquidationModel
    {
        public int Id { get; set; }
        public DateTime DateTransaction { get; set; }
        public string PreLiquidationBranch { get; set; }
        public string PreLiquidationCompany { get; set; }
        public string UserName { get; set; }
        public string PayerDocumentNumberHeader { get; set; }
        public string PayerDocumentNameHeader { get; set; }
        public int TempImputationId { get; set; }

        public string Address { get; set; }//
        public string BranchPrefixPolicyEndorsement { get; set; }//
        public string CurrencyDescription { get; set; }//
        public string PayerDocumentNumberName { get; set; }//
        public decimal PaymentAmount { get; set; }//
        public DateTime PaymentExpirationDate { get; set; }//
        public int PaymentNumber { get; set; }//
        public List<PremiumsReceivableModel> premiumsReceivable { get; set; }

        //Movement Sumary
        public string DescriptionMovementSumary { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
    }

    [KnownType("PremiumsReceivableModel")]
    public class PremiumsReceivableModel
    {
        public int Id { get; set; }
        public string Address { get; set; }//
        public string BranchPrefixPolicyEndorsement { get; set; }//
        public string CurrencyDescription { get; set; }//
        public string PayerDocumentNumberName { get; set; }//
        public decimal PaymentAmount { get; set; }//
        public DateTime PaymentExpirationDate { get; set; }//
        public int PaymentNumber { get; set; }//
    }
}