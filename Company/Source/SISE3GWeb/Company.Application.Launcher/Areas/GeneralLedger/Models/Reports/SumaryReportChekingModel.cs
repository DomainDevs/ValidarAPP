using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.Reports
{
    [Serializable]
    [KnownType("SumaryReportChekingModel")]
    public class SumaryReportChekingModel
    {
        /// <summary>
        /// Total Activos
        /// </summary>
        [DataMember]
        public decimal TotalAssets { get; set; }

        /// <summary>
        /// Total Pasivos
        /// </summary>
        [DataMember]
        public decimal TotalLiabilities { get; set; }

        /// <summary>
        /// Patrimonio
        /// </summary>
        [DataMember]
        public decimal Equity { get; set; }

        /// <summary>
        /// Total Ingresos
        /// </summary>
        [DataMember]
        public decimal TotalIncome { get; set; }

        /// <summary>
        /// Total de Costo
        /// </summary>
        [DataMember]
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Total de Gastos
        /// </summary>
        [DataMember]
        public decimal TotalSpending { get; set; }


        /// <summary>
        /// Saldo de Debitos
        /// </summary>
        [DataMember]
        public decimal Debitbalance { get; set; }

        /// <summary>
        /// utilidades_retenidas
        /// </summary>
        [DataMember]
        public decimal Utilities { get; set; }

        /// <summary>
        /// Total_Patrimonio
        /// </summary>
        [DataMember]
        public decimal TotalEquity { get; set; }

        /// <summary>
        /// capital_contable
        /// </summary>
        [DataMember]
        public decimal Capital { get; set; }

        /// <summary>
        /// ingreso_bruto
        /// </summary>
        [DataMember]
        public decimal GrossIncome { get; set; }


        /// <summary>
        /// total_utilidad
        /// </summary>
        [DataMember]
        public decimal TotalUtilities { get; set; }
    }
}