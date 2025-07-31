using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.PrintingService.NASE.Clases
{
    public class ExecutePrinterReporRequestCo
    {
        public ExecutePrinterReporRequestCo()
        {

        }

        public int AsyncProcessId { get; set; }
        public int BranchId { get; set; }
        public string CodeBar { get; set; }
        public int CopiesQuantity { get; set; }
        public int EndorsementId { get; set; }
        public bool ExportToExcel { get; set; }
        public int IdPv { get; set; }
        public int IntermediaryId { get; set; }
        public bool IsAsynchronousProcess { get; set; }
        public string LicensePlate { get; set; }
        public int PolicyId { get; set; }
        public int PrefixNum { get; set; }
        public int PremiumBalance { get; set; }
        public bool PrintFromCurrentFromDate { get; set; }
        public int PrintProccesid { get; set; }
        public string ProcessFromDate { get; set; }
        public string ProcessToDate { get; set; }
        public int QuotationId { get; set; }
        public int RangeMaxValue { get; set; }
        public int RangeMinValue { get; set; }
        public int RequestId { get; set; }
        public bool ShowPremium { get; set; }
        public int TempNum { get; set; }
        public int TypeReport { get; set; }
        public string User { get; set; }
        public bool WithFormatCollect { get; set; }
        public bool CurrentFromFirst { get; set; }
        public bool EndorsementText { get; set; }
        public bool TempAuthorization { get; set; }

    }
}
