using Sistran.Company.Application.PrintingServices.Models;
using Sistran.Core.Application.PrintingServices;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace Sistran.Company.Application.PrintingServices
{
    [ServiceContract]
    public interface IPrintingService : IPrintingServiceCore
    {
        /// <summary>
        /// Generar reporte de un póliza de individual
        /// </summary>
        /// <param name="filterReport">Filtro</param>
        /// <returns>Ruta Reporte</returns>
        [OperationContract]
        string GenerateReport(CompanyFilterReport companyFilterReport);

        [OperationContract]
        string GenerateReportMassive(List<CompanyFilterReport> companyFilterReports, int massiveLoadId);

        [OperationContract]
        string GenerateReportMassiveJetForm(List<PrintingInfo> lstPrintingInfos, string urlPdf, int printingType, int userId, int totalRisks, int massiveLoadId, bool collectFormat, string[] cuotas);

        [OperationContract]
        string GenerateReportCollective(List<CompanyFilterReport> companyFilterReports, int massiveLoadId);

        [OperationContract]
        string GenerateReportJetForm(PrintingInfo printingInfo);

        [OperationContract]
        TemporaryInfo GetTemporaryByTempID(int tempID);

        [OperationContract]
        List<QuotationInfo> GetQuotationByQuotationID(int quotationID, int branchID, int PrefixID);

        [OperationContract]
        PolicyInfo GetRisksByEndorsementId(int endorsementId);

        [OperationContract]
        DataTable GetRisksByPolicyId(int branchId, int prefixId, int policyNumber);

        [OperationContract]
        CompanyPrinting SavePrintingProcess(string urlPdf, int printingType, int userId, int totalRisks, int massiveLoadId);

        [OperationContract]
        CompanyPrinting UpdatePrintingProcess(CompanyPrinting companyPrinting);

        [OperationContract]
        CompanyPrinting GetPrintingProcess(int processId);

        [OperationContract]
        TemporaryInfo GetTemporaryByOpetarionId(int operationId, bool tempAutho = false);

        [OperationContract]
        List<CompanyPrintingLog> ValidatePrintingByPrintingId(int printingProcessId);

        [OperationContract]
        string GenerateZipFile(List<CompanyPrintingLog> companyPrintingLogs);

        [OperationContract]
        bool MergeFiles(string destinationFile, string[] sourceFiles);

        [OperationContract]
        string GenerateReportNase(PrintingInfo printingInfo);

        [OperationContract]
        string PrintCounterGuarantee(int guaranteeID, int individualID);
    }
}
