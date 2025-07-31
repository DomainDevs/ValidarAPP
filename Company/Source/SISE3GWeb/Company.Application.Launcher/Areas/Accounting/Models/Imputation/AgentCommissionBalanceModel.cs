using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation
{
    [KnownType("AgentCommissionBalanceModel")]
    public class AgentCommissionBalanceModel
    {
        public int AgentCommissionBalanceCode { get; set; }
        public int UserId { get; set; }
        public int AgentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public int Status { get; set; }
        public int BranchCode { get; set; }
        public int CompanyCode { get; set; }
    }
}