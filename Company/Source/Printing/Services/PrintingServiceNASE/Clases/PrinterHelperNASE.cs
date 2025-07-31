using Sistran.Company.Application.PrintingServices.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.PrintingService.NASE.Clases
{
    public static class PrinterHelperNASE
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
            rqsPrintReport.CurrentFromFirst = report.CurrentFromFirst;
            rqsPrintReport.EndorsementText = report.EndorsementText;
            rqsPrintReport.TempAuthorization = report.TempAuthorization;
            ExecutePrinterReporActionCo action = new ExecutePrinterReporActionCo();
            ExecutePrinterReporResponseCo rpsPrintReport = new ExecutePrinterReporResponseCo();
            rpsPrintReport = action.InternalProcess(rqsPrintReport);

            return rpsPrintReport.ReportPrinter;
        }
    }
}
