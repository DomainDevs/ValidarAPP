using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.CurrentAccount
{
    [Serializable]
    [KnownType("CoinsuranceCurrentAccountSummaryModel")]
    public class CoinsuranceCurrentAccountSummaryModel
    {
        [DataMember]
        public decimal TotalPremiumCharged { get; set; }
        [DataMember]
        public decimal TotalChangeColletion { get; set; }
        [DataMember]
        public decimal IssuancePremiumPaid { get; set; }
        [DataMember]
        public decimal PremiumPaid { get; set; }
        [DataMember]
        public decimal IssuanceManagementExpenses { get; set; }
        [DataMember]
        public decimal ManagementExpenses { get; set; }
        [DataMember]
        public decimal IssuanceIVA { get; set; }
        [DataMember]
        public decimal IVA { get; set; }
        [DataMember]
        public decimal IssuanceTotalDebitCredit { get; set; }
        [DataMember]
        public decimal TotalDebitCredit { get; set; }
    }
}