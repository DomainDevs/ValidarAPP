using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.CurrentAccount
{
    [KnownType("CommissionBalanceModel")]
    public class CommissionBalanceModel
    {
        public int AgentId { get; set; }
        public int AgentTypeCode { get; set; } //
        public string AgentTypeDescription { get; set; } //
        public string AgentName { get; set; }
        public string DocumentNumAgent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RegisterDate { get; set; } //FECHA DE PROCESO
        public int UserId { get; set; }
        public int BranchCode { get; set; }
        public string Branch { get; set; }

        //Detalle
        public int InsuredId { get; set; }
        public string DocumentNumInsured { get; set; }
        public string InsuredName { get; set; }
        public int PrefixId { get; set; }
        public string PrefixDescription { get; set; }
        public int LineBusinessCode { get; set; }
        public string LineBusinessDescription { get; set; }
        public int PolicyId { get; set; }
        public string DocumentNumPolicy { get; set; }
        public int EndorsementId { get; set; }
        public string DocumentNumEndorsement { get; set; }
        public int CommissionTypeCode { get; set; }
        public string CommissionTypeDescription { get; set; }  //pend
        public decimal CommissionPercentage { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal CommissionDiscounted { get; set; }
        public decimal CommissionTax { get; set; }
        public decimal CommissionRetention { get; set; }
        public decimal CommissionBalance { get; set; }
        public decimal ParticipationPercentage { get; set; }
        public int BrokerCheckingAccountId { get; set; }
        public string BrokerCheckingAccountDescription { get; set; }
        public int AccountingNature { get; set; }
        public string AccountingNatureDescription { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyDescription { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal Amount { get; set; }
    }
}