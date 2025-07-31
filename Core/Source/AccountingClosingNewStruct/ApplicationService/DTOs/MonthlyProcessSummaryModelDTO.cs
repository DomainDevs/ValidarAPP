using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingClosingServices.DTOs
{
    public class MonthlyProcessSummaryModelDTO
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
