using Sistran.Co.Application.Data;
using Sistran.Company.Application.PrintingServices.Clases;
using Sistran.Company.Application.PrintingServices.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTFORM = Sistran.Company.PrintingService.JetForm.Clases;

namespace Sistran.Company.PrintingService.JetForm.Clases
{
    public static class PrinterHelper
    {
        public static string getPrintedFileInfo(ReportPr report)
        {
            DataSet dsResponse;
            string reportPath;
            int[] riskRange = new int[2];
            riskRange[0] = report.RangeMinValue;
            riskRange[1] = report.RangeMaxValue;
            dsResponse = executeReportPrinter(report);
            reportPath = dsResponse.GetXml();
            return reportPath;
        }

        public static DataSet executeReportPrinter(ReportPr report)
        {
            ExecutePrinterReporRequestCo rqsPrintReport = new ExecutePrinterReporRequestCo();
            rqsPrintReport.AsyncProcessId = report.AsyncProcessId;
            rqsPrintReport.PrintProccesid = report.PrintProcessId;
            rqsPrintReport.QuotationId = report.QuotationId;
            rqsPrintReport.PolicyId = report.PolicyId;
            rqsPrintReport.EndorsementId = report.EndorsementId;
            rqsPrintReport.TypeReport = report.ReportType;
            rqsPrintReport.WithFormatCollect = report.WithFormatCollect;
            rqsPrintReport.CodeBar = report.CodeBar;
            rqsPrintReport.User = report.Username;
            rqsPrintReport.TempNum = report.TempId;
            rqsPrintReport.PrefixNum = report.PrefixId;
            rqsPrintReport.RangeMinValue = report.RangeMinValue;
            rqsPrintReport.RangeMaxValue = report.RangeMaxValue;
            rqsPrintReport.ProcessFromDate = report.ProcessFromDate;
            rqsPrintReport.ProcessToDate = report.ProcessToDate;
            rqsPrintReport.IntermediaryId = report.IndivualId;
            rqsPrintReport.BranchId = report.BranchId;
            rqsPrintReport.ExportToExcel = report.ExportToExcel;
            rqsPrintReport.IsAsynchronousProcess = report.PrintAsynchronously;
            rqsPrintReport.RequestId = report.RequestId;
            rqsPrintReport.LicensePlate = string.Empty;
            ExecutePrinterReporActionCo action = new ExecutePrinterReporActionCo();
            ExecutePrinterReporResponseCo rpsPrintReport = new ExecutePrinterReporResponseCo();
            rpsPrintReport = action.InternalProcess(rqsPrintReport);

            return rpsPrintReport.ReportPrinter;
        }
    }
}
