using Sistran.Core.Application.ReinsuranceServices.DTOs;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.ReinsuranceServices
{
    [ServiceContract]
    public interface IReinsuranceReportsService
    {
        [OperationContract]
        string ClosureReport(int year, int month, int reportType, int userId);
        
        [OperationContract]
        decimal GetTotalRecordsMassiveReport(string dateFrom, string dateTo, int reportType);

        [OperationContract]
        string ReinsuranceReports(string dateFrom, string dateTo, int reportType, int userId);

        [OperationContract]
        List<MassiveReportDTO> GetMassiveReportProcess(string reportName, int userId);

        [OperationContract]
        string GenerateStructureReport(int processId, string reportTypeDescription, int exportFormatType, decimal recordsNumber, int userId);

        [OperationContract]
        string RecordReinsuranceEntry(int processId, int userId);

        [OperationContract]
        List<ReportTypeDTO> GetReportTypes();
    }
}
