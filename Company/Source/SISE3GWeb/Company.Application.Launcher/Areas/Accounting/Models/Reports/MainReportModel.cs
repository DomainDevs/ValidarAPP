
using System;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports
{
    public class MainReportModel
    {
        //Cabecera
        public string Bill { get; set; }
        public string User { get; set; }
        public string Branch { get; set; }
        public int Status { get; set; }
        public string DateTransaction { get; set; }
        public decimal BillingTotal { get; set; }
        public string BillNumber { get; set; } //número de carátula para factura.
        public int WaterMark { get; set; }
        /*----------------------------------------------*/
        public string BillDescription { get; set; }
        public int CollectConcept { get; set; }
        public string CollectConceptDescription { get; set; }
        public string PayerName { get; set; }
        public string PayerId { get; set; }
        public string TechnicalTransaction { get; set; }
        public DateTime AccountingDate { get; set; }
    }
}