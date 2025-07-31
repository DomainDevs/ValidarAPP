using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.AccountingClosing.Models
{
    [Serializable]
    [KnownType("MonthlyProcessSummaryModel")]
    public class MonthlyProcessSummaryModel
    {
        /// <summary>
        /// Total de debitos
        /// </summary>
        [DataMember]
        public decimal TotalDebit { get; set; }

        /// <summary>
        /// Total de creditos
        /// </summary>
        [DataMember]
        public decimal TotalCredit { get; set; }
        
    }
}