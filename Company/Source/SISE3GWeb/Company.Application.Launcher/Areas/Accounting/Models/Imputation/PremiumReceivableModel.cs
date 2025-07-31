using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation
{
    [KnownType("PremiumReceivableModel")]
    public class PremiumReceivableModel
    {
        public int ImputationId { get; set; }
        public bool IsDiscountedCommisson { get; set; }
        public List<PremiumReceivableItemModel> PremiumReceivableItems { get; set; }
        public List<CommissionDiscountedModel> CommissionsDiscounted { get; set; }
    }

    [KnownType("PremiumReceivableItemModel")]
    public class PremiumReceivableItemModel
    {
        public int PremiumReceivableItemId { get; set; }
        public int PolicyId { get; set; }
        public int EndorsementId { get; set; }
        public int PaymentNum { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal PaymentLocalAmount { get; set; }
        public int PayerId { get; set; }
        public decimal LocalAmount { get; set; }
        public int CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal Expenses { get; set; }
        public decimal Tax { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public DateTime RegisterDate { get; set; }
        public decimal DiscountedCommisson { get; set; }
        public int ItemId { get; set; }
        public bool NoExpenses { get; set; }
    }

    [KnownType("UsedDepositPremiumModel")]
    public class UsedDepositPremiumModel
    {
        public int PremiumReceivableItemId { get; set; }
        public List<UsedDepositPremiumAmountModel> UsedAmounts { get; set; }
    }

    [KnownType("UsedDepositPremiumAmountModel")]
    public class UsedDepositPremiumAmountModel
    {
        public int UsedDepositPremiumId { get; set; }
        public int DepositPremiumTrasactionId { get; set; }
        public decimal Amount { get; set; }
    }
    [KnownType("CommissionDiscountedModel")]
    public class CommissionDiscountedModel {

        [DataMember]
        public int DiscountedCommissionId { get; set; }
        [DataMember]
        public int ApplicationPremiumId { get; set; }
        [DataMember]
        public int AgentTypeCode { get; set; }
        [DataMember]
        public int AgentIndividualId { get; set; }
        [DataMember]
        public string AgentName { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public decimal BaseIncomeAmount { get; set; }
        [DataMember]
        public decimal BaseAmount { get; set; }
        [DataMember]
        public decimal CommissionPercentage { get; set; }
        [DataMember]
        public decimal AgentPercentageParticipation { get; set; }
        [DataMember]
        public int CommissionType { get; set; }
        [DataMember]
        public decimal CommissionDiscountIncomeAmount { get; set; }
        [DataMember]
        public decimal CommissionDiscountAmount { get; set; }
        [DataMember]
        public bool IsUsedCommission { get; set; }
        [DataMember]
        public int AgentAgencyId { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal LocalAmount { get; set; }
        [DataMember]
        public int AgentId { get; set; }
    }
}