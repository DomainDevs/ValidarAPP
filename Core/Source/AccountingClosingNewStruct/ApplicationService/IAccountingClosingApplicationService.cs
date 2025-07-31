using Sistran.Core.Application.AccountingClosingServices.DTOs;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.AccountingClosingServices
{
    [ServiceContract]
    public interface IAccountingClosingApplicationService
    {
        /// <summary>
        /// GetStatus
        /// </summary>
        /// <param name="module"></param>
        /// <returns>AccountingClosingDTO</returns>
        [OperationContract]
        AccountingClosingDTO GetAccountingClosing(int module);
        [OperationContract]
        int ExecuteClosing(int closingTypeId, int year, int month, int userId);
        #region Actions
        /// <summary>
        /// GetStatus
        /// </summary>
        /// <param name="module"></param>
        /// <returns>JsonResult</returns>
        [OperationContract]
        List<object> GetStatus(int module);
        [OperationContract]
        int MonthlyClosureAsync(int module);
        [OperationContract]
        List<string> AccountClosure(int module, int userId, int day);
        #endregion Actions

        #region Issuance

        /// <summary>
        /// IssuanceClosureGeneration
        /// Ejecuta el cierre mensual de Emisión        
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="module"></param>
        /// <returns>int</returns>
        [OperationContract]
        int IssuanceClosureGeneration(int year, int month, int day, int module);

        /// <summary>
        /// GetIssuanceClosureReportParameters
        /// Método Creado solo para EE
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        [OperationContract]
        List<AccountingClosingReportDTO> GetIssuanceClosureReportParameters(DateTime startDate, DateTime endDate, int moduleId);

        /// <summary>
        /// IssuanceClosureEnding
        /// Realiza el proceso de Contabilización de Emisión
        /// Devuelve el Nro. de asiento generado        
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns>int</returns>
        [OperationContract]
        int IssuanceClosureEnding(int year, int month, int day);

        /// <summary>
        /// Metodo AccountClosure Logica de AccountingClosing 
        /// </summary>
        /// <param name="accountingDate"></param>
        /// <returns></returns>
        [OperationContract]
        void GenerateIssuanceModule(DateTime accountingDate, int module);
        [OperationContract]
        LedgerEntryDTO GenerateIssuanceEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId, int userId);
        [OperationContract]
        List<string> AccountClosureIssuance(DateTime accountingDate, int module, int userId, int day);
        
        #endregion Issuance

        #region Reinsurance

        /// <summary>
        /// ReinsuranceClosureGeneration
        /// Ejecuta el cierre mensual de Reaseguros        
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="module"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ReinsuranceClosureGeneration(int year, int month, int day, int module);

        /// <summary>
        /// GetReinsuranceClosureReport
        /// Obtiene los registros procesados del cierre mensual de Reaseguros        
        /// </summary>
        /// <returns>List<MonthlyProcessReportDTO/></returns>
        [OperationContract]
        List<AccountingClosingReportDTO> GetReinsuranceClosureReport();

        /// <summary>
        /// GetReinsuranceClosureReportRecords
        /// Obtiene el número de registros procesados del cierre mensual de Reaseguros
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        int GetReinsuranceClosureReportRecords();

        /// <summary>
        /// GetReinsurancePaginatedClosureReport
        /// Obtiene los registros paginados del cierre mensual de Reaseguros debido a los limitantes de enviar muchos registros de servicio a controlador
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="records"></param>
        /// <returns>List<AccountingClosingReportDto/></returns>
        [OperationContract]
        List<AccountingClosingReportDTO> GetReinsurancePaginatedClosureReport(int pageSize, int pageNumber, int records);

        /// <summary>        
        /// GetGeneratedRecordsCount
        /// Obtiene el número de registros de pre-cierre de contabilidad para cierres mensuales
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="processId"></param>
        /// <returns>int</returns>
        [OperationContract]
        int GetGeneratedRecordsCount(int moduleId, int processId);

        /// <summary>
        /// Obtener los datos de cierre de Reaseguros, agrupados por Cuenta Contable
        /// </summary>
        /// <param name="moduleId">Identificador del módulo</param>
        /// <param name="processId">Identificador del proceso</param>
        /// <returns>List<AccountingClosingReportDto/></returns>
        [OperationContract]
        List<AccountingClosingReportDTO> GetReinsuranceClosureReportByModuleIdAndProcessId(int moduleId, int processId);


        /// <summary>
        /// GetGeneratedClosureReportRecords
        /// Obtiene los registros paginados de pre-cierre de contabilidad para cierres mensuales
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="processId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="records"></param>
        /// <returns>List<AccountingClosingReportDto/></returns>
        [OperationContract]
        List<AccountingClosingReportDTO> GetGeneratedClosureReportRecords(int moduleId, int processId, int pageSize, int pageNumber, int records);

        /// <summary>        
        /// GetLogProcess
        /// Obtiene el Id del proceso.
        /// </summary>
        /// <param name="module"></param>
        [OperationContract]
        int GetLogProcessId(int module);

        /// <summary>        
        /// UpdateLogProcess
        /// Se publica el método para la actualización del estado del proceso.
        /// </summary>
        /// <param name="processId"></param>
        [OperationContract]
        void UpdateLogProcess(int processId);

        /// <summary>        
        /// SaveClosureTempEntryGeneration
        /// Graba registro en la tabla temporal para la generación de asientos de cierre.
        /// </summary>
        /// <param name="closureTempEntryGenerationDTO"></param>
        [OperationContract]
        void SaveClosureTempEntryGeneration(ClosureTempEntryGenerationDTO closureTempEntryGenerationDTO);

        /// <summary>
        /// ReinsuranceClosureEnding
        /// Realiza el proceso de Contabilización de Reaseguros
        /// Devuelve el Nro. de asiento generado        
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ReinsuranceClosureEnding(int year, int month);
        [OperationContract]
        List<string> AccountClosureReinsurance(DateTime accountingDate, int module, int userId);
        [OperationContract]
        void SaveTemporalEntryRecord(LedgerEntryDTO ledgerEntry, int temporalEntryNumber, int processId, int module, int userId);

        void GenerateReinsuranceEntry(List<AccountingClosingReportDTO> accountingClosings, int temporalEntryNumber, int processId, int moduleId, int userId);
        int ReinsuranceGenerationClosure(int year, int month, int day, int module, int userId);

        #endregion Reinsurance

        #region Claim

        /// <summary>
        /// ClaimClosureGeneration
        /// Realiza el proceso de cierre de siniestros        
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="module"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ClaimClosureGeneration(int year, int month, int module);

        /// <summary>
        /// GetClaimClosureReport
        /// Genera el reporte para proceso de precierre de siniestros        
        /// </summary>
        /// <returns>List<MonthlyProcessReportDTO/></returns>
        [OperationContract]
        List<AccountingClosingReportDTO> GetClaimClosureReport();

        /// <summary>
        /// ClaimClosureEnding 
        /// Realiza el proceso de Contabilización de Siniestros        
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ClaimClosureEnding(int year, int month);
        [OperationContract]
        List<string> AccountingClosureClaim(DateTime accountingDate, int module, int userId);
        #endregion Claim

        #region ClaimReserve

        /// <summary>
        /// ClaimReserveClosureGeneration
        /// Ejecuta el cierre mensual de reserva de siniestros        
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="module"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ClaimReserveClosureGeneration(int year, int month, int module);

        /// <summary>
        /// GetClaimReserveClosureReport
        /// Obtiene los registros procesados del cierre de reserva de siniestros        
        /// </summary>
        /// <returns>List<MonthlyProcessReportDTO/></returns>
        [OperationContract]
        List<AccountingClosingReportDTO> GetClaimReserveClosureReport();

        /// <summary>
        /// ClaimReserveClosureEnding        
        /// Realiza el proceso de Contabilización de Reserva de siniestros
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ClaimReserveClosureEnding(int year, int month);
        [OperationContract]
        LedgerEntryDTO GenerateClaimReserveEntry(List<AccountingClosingReportDTO> AccountingClosings, int moduleId, int userId);

        #endregion ClaimReserve

        #region RiskReserve
        [OperationContract]
        LedgerEntryDTO GenerateRiskReserveEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId, int userId);
        [OperationContract]
        List<string> AccountClosureRiskReserve(DateTime accountingDate, int module, int userId);

        #endregion

        #region ExpensesClousure


        /// <summary>
        /// ExpensesClousureReport
        /// Obtiene los registros procesados del cierre mensual de Ingresos y Egresos         
        /// </summary>
        /// <returns>List<MonthlyProcessReportDTO/></returns>
        [OperationContract]
        List<AccountingClosingReportDTO> ExpensesClousureReport();

        /// <summary>
        /// ExpensesClousureEnding
        /// Genera la Contabilización y devuelve el Nro. de asiento        
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ExpensesClousureEnding(int year, int month, int day);

        #endregion ExpensesClousure

        #region IBNR Reserves

        /// <summary>
        /// IbnrClosureGeneration
        /// Ejecuta el cierre mensual de reservas IBNR        
        /// </summary>
        /// <param name="year"></param>
        /// <param name="module"></param>
        /// <returns>int</returns>
        [OperationContract]
        int IbnrClosureGeneration(int year, int module);

        /// <summary>
        /// IbnrClosureReport
        /// Obtiene los registros procesados del cierre mensual reservas IBNR        
        /// </summary>
        /// <returns>List<MonthlyProcessReportDTO/></returns>
        [OperationContract]
        List<AccountingClosingReportDTO> IbnrClosureReport();

        /// <summary>
        /// IbnrClosureEnding 
        /// Genera la Contabilización y devuelve el Nro. de asiento        
        /// </summary>
        /// <param name="year"></param>
        /// <returns>int</returns>
        [OperationContract]
        int IbnrClosureEnding(int year);
        [OperationContract]
        List<string> AccountClosureIBNRReserves(DateTime accountingDate, int module, int userId);
        [OperationContract]
        LedgerEntryDTO GenerateIBNRReserveEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId, int userId);
        #endregion IBNR Reserves

        #region Prevision reserves
        [OperationContract]
        List<string> AccountClosurePrevisionReserves(DateTime accountingDate, int module, int userId);
        [OperationContract]
        LedgerEntryDTO GeneratePrevisionReserveEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId, int userId);
        #endregion

        #region RiskPreventionReserveClosure

        /// <summary>
        /// RiskPreventionReserveClosureGeneration
        /// Realiza la generación del cierre de Reserva de Riesgos de Previsión        
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="module"></param>
        /// <returns>int</returns>
        [OperationContract]
        int RiskPreventionReserveClosureGeneration(int year, int month, int module);

        /// <summary>
        /// GetRiskPreventionReserveClosureReport
        /// Obtiene los registros procesados del cierre de Reserva de Riesgos de Previsión        
        /// </summary>
        /// <returns>List<MonthlyProcessReportDTO/></returns>
        [OperationContract]
        List<AccountingClosingReportDTO> GetRiskPreventionReserveClosureReport();

        /// <summary>
        /// RiskPreventionReserveClosureEnding
        /// Realiza el proceso del cierre de Reserva de Riesgos de Previsión
        /// Devuelve el Nro. de asiento generado        
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>int</returns>
        [OperationContract]
        int RiskPreventionReserveClosureEnding(int year, int month);

        #endregion RiskPreventionReserveClosure

        #region ExpiredPremiums

        /// <summary>
        /// ExpiredPremiumsGeneration
        /// Ejecuta el cierre mensual de reservas para Primas Vencidas        
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="module"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ExpiredPremiumsGeneration(int year, int month, int module);

        /// <summary>
        /// ExpiredPremiumsReport
        /// Obtiene los registros procesados del cierre mensual reservas Primas Vencidas        
        /// </summary>
        /// <returns>List<MonthlyProcessReportDTO/></returns>
        [OperationContract]
        List<AccountingClosingReportDTO> ExpiredPremiumsReport();

        /// <summary>
        /// ProvisionExpiredPremiumsReport
        /// Obtiene los registros procesados del cierre mensual reservas Provisión de Primas Vencidas        
        /// </summary>
        /// <returns>List<MonthlyProcessReportDTO/></returns>
        [OperationContract]
        List<AccountingClosingReportDTO> ProvisionExpiredPremiumsReport();

        /// <summary>
        /// ExpiredPremiumsEnding
        /// Genera la Contabilización y devuelve el Nro. de asiento y el Nro. de asiento de provisión        
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns>int</returns>
        [OperationContract]
        int ExpiredPremiumsEnding(int year, int month, int day);
        [OperationContract]
        List<string> AccountClosureExpiredPremiums(DateTime accountingDate, int module, int userId, int day);
        [OperationContract]
        LedgerEntryDTO GenerateExpiredPremiumsEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId, int userId);
        [OperationContract]
        LedgerEntryDTO GeneratePrevisionExpiredPremiumsEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId, int userId);
        #endregion ExpiredPremiums

        #region CatastrophicRiskReserveClosure

        /// <summary>
        /// Realiza la generación del cierre de Reservas para Riesgos Catastróficos        
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="module"></param>
        /// <returns>int</returns>
        [OperationContract]
        int CatastrophicRiskReserveClosureGeneration(int year, int month, int module);

        /// <summary>
        /// Obtiene los registros procesados del cierre de Reservas para Riesgos Catastróficos        
        /// </summary>
        /// <returns>List<MonthlyProcessReportDTO/></returns>
        [OperationContract]
        List<AccountingClosingReportDTO> GetCatastrophicRiskReserveClosureReport();

        /// <summary>
        /// Realiza el proceso del cierre de Reservas para Riesgos Catastróficos
        /// Devuelve el Nro. de asiento generado        
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>int</returns>
        [OperationContract]
        int CatastrophicRiskReserveClosureEnding(int year, int month);
        [OperationContract]
        List<string> AccountClosureCatastrophicRiskReserve(DateTime accountingDate, int module, int userId);
        [OperationContract]
        LedgerEntryDTO GenerateCatastrophicRiskEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId, int userId);
        #endregion CatastrophicRiskReserveClosure

        #region IncomeAndExpenses

        /// <summary>
        /// GenerateIncomeAndExpensesEntry
        /// Genera los asientos para el cierre de ingresos y egresos.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        [OperationContract]
        LedgerEntryDTO GenerateIncomeAndExpensesEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId, int userId);
        [OperationContract]
        List<string> AccountClosureIncomeAndExpenses(DateTime accountingDate, int userId, int day);

        #endregion IncomeAndExpenses

        /// <summary>
        /// GetClosingDate: Obtiene Fecha de Cierre
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>DateTime</returns>
        [OperationContract]
        DateTime GetClosingDate(int moduleId);
        [OperationContract]
        List<ExchangeDifferenceReportDTO> GetExchangeDifferenceReportRecords();
        [OperationContract]
        bool CheckClosedModules(int year);
        #region ExchangeDifference

        /// <summary>
        /// GetExchangeDifference
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="rateDate"></param>
        /// <param name="localCurrencyId"></param>
        /// <returns></returns>        
        [OperationContract]
        int GetExchangeDifference(DateTime startDate, DateTime endDate, DateTime rateDate, int accountingYear, int localCurrencyId);

        [OperationContract]
        int GetExchangeDifferenceDate(string startDate, string endDate);

        //[OperationContract]
        //int GetExchangeDifference(string startDate, string endDate);

        [OperationContract]
        List<ExchangeDifferenceReportDTO> GenerateExchangeDifferenceReport();

        /// <summary>
        /// GetExchangeDifferenceRecords
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ExchangeDifferenceReportDTO> GetExchangeDifferenceRecords();

        /// <summary>
        /// PostExchangeDifferenceRecord
        /// </summary>
        /// <param name="exchangeDifferenceRecordId"></param>
        [OperationContract]
        void PostExchangeDifferenceRecord(int exchangeDifferenceRecordId);
        [OperationContract]
        int AccountExchangeDifferenceRecords(int userId);

        #endregion ExchangeDifference

        #region Reporing

        [OperationContract]
        List<MassiveReportDTO> GetMassiveReportsByUserIdReportName(int userId, string reportName);

        [OperationContract]
        string GenerateStructureReportMassive(int processId, string reportTypeDescription, int exportFormatType, decimal recordsNumber, int userId);

        [OperationContract]
        int GetProductionDetailReports(string year, string month, string day, int userId, int? process);

        [OperationContract]
        int GetCancellationRecordIssuanceReports(string year, string month, string day, int userId, int? process);
        [OperationContract]
        List<MonthlyProcessModelDTO> LoadMonthlyProcessReport(int module, int userId, string userName);
        [OperationContract]
        List<MonthlyProcessSummaryModelDTO> LoadMonthlyProcessReportSummaries(int module, int userId);
        [OperationContract]
        List<MonthlyProcessModelDTO> LoadPrintEntryReport(int entry, int module, int userId, string userName);
        [OperationContract]
        List<MonthlyProcessSummaryModelDTO> LoadSummaryEntryReport(int entry, int module, int userId);
        [OperationContract]
        List<AccountingClosingReportDTO> ReportExcel(int module, int userId);
        #endregion
    }
}
