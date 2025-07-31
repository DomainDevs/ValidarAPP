using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.Reports
{
    [Serializable]
    [KnownType("MonthlyProcessSummaryModel")]
    public class MonthlyProcessSummaryModel
    {
        /// <summary>
        /// Código de la Sucursal
        /// </summary>
        [DataMember]
        public decimal TotalDebit { get; set; }

        /// <summary>
        /// Descripción de la Susursal
        /// </summary>
        [DataMember]
        public decimal TotalCredit { get; set; }
    }
}