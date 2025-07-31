using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;

using Sistran.Core.Application.ReportingServices.Models;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Application.ReportingServices.Provider.Helpers
{
    public class ReportHelper : IDisposable
    {
        #region Enums

        /// <summary>
        /// ExportType
        /// </summary>
        enum ExportType
        {
            EditableRTF,
            PortableDocFormat,
            CharacterSeparatedValues,
            CrystalReport,
            Excel,
            ExcelRecord,
            RichText,
            TabSeperatedText,
            Text,
            WordForWindows,
        }

        #endregion Enums

        #region Constants

        #endregion Constants

        #region Instance Variables

        private ReportDocument _reportDocument;
        private ExportOptions _exportOptions;
        private DiskFileDestinationOptions _diskFileDestinationOptions;

        #endregion Instance Variables

        #region Constructors

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Close();
                //  Free other state (managed objects).
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }


        // Disposable types implement a finalizer.
        ~ReportHelper()
        {
            Dispose(false);
        }

        /// <summary>
        /// ReportHelper
        /// </summary>
        public ReportHelper()
        {
            this.Initialize();
        }

        /// <summary>
        /// ReportHelper
        /// </summary>
        /// <param name="fileFullName"></param>
        public ReportHelper(string fileFullName)
        {
            this.Initialize();
            this.InitialiseCrystalReport(fileFullName);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// InitialiseCrystalReport
        /// </summary>
        /// <param name="filePath"></param>
        public void InitialiseCrystalReport(string filePath)
        {
            _reportDocument.Load(filePath);
            _reportDocument.PrintOptions.PaperSize = PaperSize.PaperA4;
        }

        /// <summary>
        /// InitialiseCrystalReport
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="collections"></param>
        public void InitialiseCrystalReport(string filePath, List<object> collections)
        {
            _reportDocument.Load(filePath);
            _reportDocument.SetDataSource(collections);
            _reportDocument.PrintOptions.PaperSize = PaperSize.PaperA4;
        }

        /// <summary>
        /// GetReport
        /// </summary>
        /// <returns></returns>
        public ReportDocument GetReport()
        {
            return _reportDocument;
        }

        /// <summary>
        /// ExportTOFile
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileType"></param>
        public void ExportTOFile(string filePath, ExportFormatType fileType)
        {
            _exportOptions.ExportFormatType = fileType;
            _diskFileDestinationOptions.DiskFileName = filePath;
            _exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        }

        /// <summary>
        /// ExportTOFileCSV
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileType"></param>
        public void ExportTOFileCSV(string filePath, ExportFormatType fileType)
        {
            _exportOptions.ExportFormatType = fileType;
            CharacterSeparatedValuesFormatOptions csvOptions = new CharacterSeparatedValuesFormatOptions();
            csvOptions.ReportSectionsOption = CsvExportSectionsOption.DoNotExport;
            _diskFileDestinationOptions.DiskFileName = filePath;
            _exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        }

        /// <summary>
        /// PrintAllDocuments
        /// </summary>
        public void PrintAllDocuments()
        {
            _reportDocument.PrintToPrinter(1, true, 0, 0);
        }

        /// <summary>
        /// PrintDocument
        /// </summary>
        /// <param name="noOfCopies"></param>
        /// <param name="defaultCollate"></param>
        /// <param name="pageFrom"></param>
        /// <param name="pageTo"></param>
        public void PrintDocument(int noOfCopies, bool defaultCollate, Int32 pageFrom, Int32 pageTo)
        {
            _reportDocument.PrintToPrinter(noOfCopies, defaultCollate, pageFrom, pageTo);
        }

        /// <summary>
        /// ExportReport
        /// </summary>
        public void ExportReport()
        {
            _reportDocument.Export(_exportOptions);
        }

        /// <summary>
        /// ExportCrystalReport
        /// </summary>
        /// <param name="exportType"></param>
        /// <param name="filePath"></param>
        public void ExportCrystalReport(ExportTypes exportType, string filePath)
        {
            if (_reportDocument.IsLoaded)
            {
                switch (exportType)
                {
                    case ExportTypes.PDF: //"5": //ExportFormatType.PortableDocFormat.ToString():
                        // Or "PDF"
                        this.ExportTOFile(filePath, ExportFormatType.PortableDocFormat);
                        break;
                    case ExportTypes.Excel: //"4": //ExportFormatType.Excel.ToString 'Or "XLS"
                        this.ExportTOFile(filePath, ExportFormatType.Excel);
                        break;
                    /*
                    Case ExportFormatType.EditableRTF.ToString 'Or "RTF"
                        ExportTOFile(filePath, ExportFormatType.EditableRTF)
                    Case ExportFormatType.CrystalReport.ToString 'Or "RPT"
                        ExportTOFile(filePath, ExportFormatType.CrystalReport)
                    Case ExportFormatType.Excel.ToString 'Or "XLS"
                        ExportTOFile(filePath, ExportFormatType.Excel)
                    Case ExportFormatType.ExcelRecord.ToString 'Or "XLSR"
                        ExportTOFile(filePath, ExportFormatType.ExcelRecord)
                    Case ExportFormatType.HTML32.ToString
                        MsgBox("RTF")
                    Case ExportFormatType.HTML40.ToString
                        MsgBox("RTF")
                    Case ExportFormatType.NoFormat.ToString
                        ExportTOFile(filePath, ExportFormatType.NoFormat)
                    Case ExportFormatType.RichText.ToString
                        ExportTOFile(filePath, ExportFormatType.RichText)
                    Case ExportFormatType.TabSeperatedText.ToString
                        ExportTOFile(filePath, ExportFormatType.TabSeperatedText)
                     Case ExportFormatType.Text.ToString
                        ExportTOFile(filePath, ExportFormatType.Text)
                     Case ExportFormatType.WordForWindows.ToString
                        ExportTOFile(filePath, ExportFormatType.WordForWindows)
                    */
                }
            }

            _exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;

            _exportOptions.ExportDestinationOptions = _diskFileDestinationOptions;

            this.ExportReport();
        }


        /// <summary>
        /// SetReportDocument
        /// </summary>
        /// <param name="report"></param>
        public void SetReportDocument(ReportDocument report)
        {
            _reportDocument = report;
        }

        /// <summary>
        /// SetDataSourceConnection
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="dataBase"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void SetDataSourceConnection(string serverName, string dataBase, string userName, string password)
        {
            Logon(_reportDocument, serverName, dataBase, false, userName, password);
        }

        /// <summary>
        /// SetDataSourceConnection
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="dataBase"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void SetDataSourceConnection(string serverName, string dataBase, string userName, string password, bool isObject)
        {
            // Connection String for SubReports
            for (int i = 0; (i <= (_reportDocument.DataSourceConnections.Count - 1)); i++)
            {
                _reportDocument.DataSourceConnections[i].SetConnection(serverName, dataBase, userName, password);
            }

            _reportDocument.SetDatabaseLogon(userName, password, serverName, dataBase);

            ConnectionInfo connectionIfo = new ConnectionInfo();
            connectionIfo.ServerName = serverName;
            connectionIfo.DatabaseName = dataBase;
            connectionIfo.UserID = userName;
            connectionIfo.Password = password;

            SetDBLogonForReport(connectionIfo, _reportDocument);
        }

        /// <summary>
        /// SetDataBaseConnection
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="dataBase"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void SetDataBaseConnection(string serverName, string dataBase, string userName, string password)
        {
            Sections sections;
            ReportDocument subreportDocument;
            SubreportObject subreportObject;
            ReportObjects reportObjects;
            ConnectionInfo connectionInfo;
            Database database;
            Tables tables;
            TableLogOnInfo tableLogOnInfo;

            database = _reportDocument.Database;
            tables = database.Tables;
            connectionInfo = new ConnectionInfo();
            connectionInfo.ServerName = serverName;
            connectionInfo.DatabaseName = dataBase;
            connectionInfo.UserID = userName;
            connectionInfo.Password = password;
            foreach (Table table in tables)
            {
                tableLogOnInfo = table.LogOnInfo;
                tableLogOnInfo.ConnectionInfo = connectionInfo;
                table.ApplyLogOnInfo(tableLogOnInfo);
                table.Location = string.Format("{0}.dbo.{1}", dataBase, table.Location.Substring(table.Location.LastIndexOf(".") + 1));
            }
            // THIS STUFF HERE IS FOR REPORTS HAVING SUBREPORTS 
            // set the sections object to the current report's section 
            sections = _reportDocument.ReportDefinition.Sections;
            // loop through all the sections to find all the report objects 
            foreach (Section section in sections)
            {
                reportObjects = section.ReportObjects;
                // loop through all the report objects in there to find all subreports 
                foreach (ReportObject reportObject in reportObjects)
                {
                    if (reportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        subreportObject = (SubreportObject)reportObject;
                        // open the subreport object and logon as for the general report 
                        subreportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                        database = subreportDocument.Database;
                        tables = database.Tables;
                        foreach (Table table in tables)
                        {
                            tableLogOnInfo = table.LogOnInfo;
                            tableLogOnInfo.ConnectionInfo = connectionInfo;
                            table.ApplyLogOnInfo(tableLogOnInfo);
                            table.Location = string.Format("{0}.dbo.{1}", dataBase, table.Location.Substring(table.Location.LastIndexOf(".") + 1));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// SetParameter
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void SetParameter(string paramName, object paramValue)
        {
            _reportDocument.SetParameterValue(paramName, paramValue);
        }

        /// <summary>
        /// SetFormulaField
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void SetFormulaField(string paramName, object paramValue)
        {
            _reportDocument.DataDefinition.FormulaFields[paramName].Text = "'" + paramValue + "'";
        }

        /// <summary>
        /// SetRecordSelectionFormula
        /// </summary>
        /// <param name="paramValue"></param>
        public void SetRecordSelectionFormula(string paramValue)
        {
            _reportDocument.DataDefinition.RecordSelectionFormula = paramValue;
            _reportDocument.Refresh();
        }

        /// <summary>
        /// SetParameterForSubReport
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <param name="subReport"></param>
        public void SetParameterForSubReport(string paramName, object paramValue, string subReport)
        {
            _reportDocument.OpenSubreport(subReport);
            _reportDocument.SetParameterValue(paramName, paramValue, subReport);
        }

        /// <summary>
        /// SetPrinterSettingsDuplexDefaultOption
        /// </summary>
        public void SetPrinterSettingsDuplexDefaultOption()
        {
            _reportDocument.PrintOptions.PrinterDuplex = PrinterDuplex.Default;
        }

        /// <summary>
        /// SetPrinterSettingsDuplexHorizontal
        /// </summary>
        public void SetPrinterSettingsDuplexHorizontal()
        {
            _reportDocument.PrintOptions.PrinterDuplex = PrinterDuplex.Horizontal;
        }

        /// <summary>
        /// SetPrinterSettingsDuplexVertical
        /// </summary>
        public void SetPrinterSettingsDuplexVertical()
        {
            _reportDocument.PrintOptions.PrinterDuplex = PrinterDuplex.Vertical;
        }

        /// <summary>
        /// SetPrinterSettingsSimpleOption
        /// </summary>
        public void SetPrinterSettingsSimpleOption()
        {
            _reportDocument.PrintOptions.PrinterDuplex = PrinterDuplex.Simplex;
        }

        /// <summary>
        /// SetPrinter
        /// </summary>
        /// <param name="printerPath"></param>
        /// <returns></returns>
        public bool SetPrinter(string printerPath)
        {
            bool result = true;
            _reportDocument.PrintOptions.PrinterName = printerPath;
            return result;
        }

        /// <summary>
        /// Close
        /// </summary>
        public void Close()
        {
            _reportDocument.Close();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Initialize
        /// </summary>
        private void Initialize()
        {
            _reportDocument = new ReportDocument();
            _exportOptions = new ExportOptions();
            _diskFileDestinationOptions = new DiskFileDestinationOptions();
        }

        /// <summary>
        /// SetDBLogonForReport
        /// </summary>
        /// <param name="connectionInfo"></param>
        /// <param name="reportDocument"></param>
        private void SetDBLogonForReport(ConnectionInfo connectionInfo, ReportDocument reportDocument)
        {
            Tables tables = reportDocument.Database.Tables;

            foreach (Table table in tables)
            {
                TableLogOnInfo tableLogonInfo = table.LogOnInfo;
                tableLogonInfo.ConnectionInfo = connectionInfo;
                table.ApplyLogOnInfo(tableLogonInfo);
            }

            if (!reportDocument.IsSubreport)
            {
                foreach (ReportDocument report in reportDocument.Subreports)
                {
                    SetDBLogonForReport(connectionInfo, report);
                }
            }
        }

        /// <summary>
        /// Logon
        /// </summary>
        /// <param name="report"></param>
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <param name="integratedSecurity"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool Logon(ReportDocument report, string server, string database, bool integratedSecurity, string user, string password)
        {
            report.SetDatabaseLogon(user, password, server, database);

            ConnectionInfo connectionInfo = new ConnectionInfo();
            SubreportObject subObj;

            connectionInfo.ServerName = server;
            connectionInfo.DatabaseName = database;
            if (integratedSecurity)
            {
                connectionInfo.IntegratedSecurity = true;
            }
            else
            {
                connectionInfo.IntegratedSecurity = false;
                connectionInfo.UserID = user;
                connectionInfo.Password = password;
            }
            connectionInfo.Type = ConnectionInfoType.SQL;

            if (!ApplyLogon(report, connectionInfo))
            {
                return false;
            }

            for (int i = 0; i < report.ReportDefinition.ReportObjects.Count; i++)
            {
                if (report.ReportDefinition.ReportObjects[i].Kind == ReportObjectKind.SubreportObject)
                {
                    subObj = (SubreportObject)report.ReportDefinition.ReportObjects[i];
                    if (!ApplyLogon(report.OpenSubreport(subObj.SubreportName), connectionInfo, report))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// ApplyLogon
        /// </summary>
        /// <param name="report"></param>
        /// <param name="connectionInfo"></param>
        /// <param name="fatherReport"></param>
        /// <returns></returns>
        private bool ApplyLogon(ReportDocument report, ConnectionInfo connectionInfo, ReportDocument fatherReport = null)
        {
            TableLogOnInfo tableLogOnInfo;

            // for each table apply connection info
            for (int i = 0; i < report.Database.Tables.Count; i++)
            {
                tableLogOnInfo = report.Database.Tables[i].LogOnInfo;
                tableLogOnInfo.ConnectionInfo = connectionInfo;
                tableLogOnInfo.ReportName = report.Name;
                tableLogOnInfo.TableName = report.Database.Tables[i].Name;
                report.Database.Tables[i].ApplyLogOnInfo(tableLogOnInfo);
                report.Database.Tables[i].Location = report.Database.Tables[i].Location;

                // check if logon was successful
                // if TestConnectivity returns false, check
                // logon credentials
                if (TestConnectivity(report, i))
                {
                    if (!IsAliasTable(report.Database.Tables[i].Name, report, fatherReport))
                    {
                        // drop fully qualified table location
                        if (report.Database.Tables[i].Location.IndexOf(".") > -1)
                        {
                            report.Database.Tables[i].Location = report.Database.Tables[i].Location.Substring(report.Database.Tables[i].Location.LastIndexOf(".") + 1);
                        }

                        report.Database.Tables[i].Location = connectionInfo.DatabaseName + ".dbo." +
                        report.Database.Tables[i].Location.Substring(report.Database.Tables[i].Location.LastIndexOf(".") + 1);
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// TestConnectivity
        /// </summary>
        /// <param name="report"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private static bool TestConnectivity(ReportDocument report, int i)
        {
            bool test = false;
            try
            {
                test = report.Database.Tables[i].TestConnectivity();
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                try
                {
                    test = report.Database.Tables[i].TestConnectivity();
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    try
                    {
                        test = report.Database.Tables[i].TestConnectivity();
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                        test = report.Database.Tables[i].TestConnectivity();
                    }
                }
            }

            return test;
        }

        /// <summary>
        /// IsAliasTable
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="report"></param>
        /// <param name="fatherReport"></param>
        /// <returns></returns>
        private bool IsAliasTable(string tableName, ReportDocument report, ReportDocument fatherReport)
        {
            bool isAlias = false;
            if (!report.IsSubreport)
            {
                CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument reportClientDocument = report.ReportClientDocument;
                foreach (CrystalDecisions.ReportAppServer.DataDefModel.Table table in reportClientDocument.DatabaseController.Database.Tables)
                {
                    isAlias = (table.Name != table.Alias && table.Alias == tableName);
                    if (isAlias)
                    {
                        break;
                    }
                }
            }
            else
            {
                CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument reportClientDocument = fatherReport.ReportClientDocument;
                CrystalDecisions.ReportAppServer.Controllers.SubreportClientDocument subReportClientDocument = reportClientDocument.SubreportController.GetSubreport(report.Name);
                foreach (CrystalDecisions.ReportAppServer.DataDefModel.Table table in subReportClientDocument.DatabaseController.Database.Tables)
                {
                    isAlias = (table.Name != table.Alias && table.Alias == tableName);
                    if (isAlias)
                    {
                        break;
                    }
                }
            }

            return isAlias;
        }

        /// <summary>
        /// ApplyLoginInfo
        /// </summary>
        /// <param name="document"></param>
        /// <param name="serverName"></param>
        /// <param name="dbName"></param>
        /// <param name="useTrustedConnection"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        private void ApplyLoginInfo(ReportDocument document, string serverName, string dbName, bool useTrustedConnection, string userName = "", string password = "")
        {
            TableLogOnInfo info = null;
            var message = "";

            try
            {
                #region Credentials

                // Define credentials
                info = new TableLogOnInfo();
                info.ConnectionInfo.AllowCustomConnection = true;
                info.ConnectionInfo.ServerName = serverName;
                info.ConnectionInfo.DatabaseName = dbName;

                // Set the userid/password for the report if we are not using integrated security
                if (useTrustedConnection)
                {
                    info.ConnectionInfo.IntegratedSecurity = true;
                }
                else
                {
                    info.ConnectionInfo.Password = password;
                    info.ConnectionInfo.UserID = userName;
                }

                #endregion

                #region Apply to connections, tables and sub-reports

                // Main connection?
                document.SetDatabaseLogon(info.ConnectionInfo.UserID,
                    info.ConnectionInfo.Password,
                    info.ConnectionInfo.ServerName,
                    info.ConnectionInfo.DatabaseName,
                    false);

                // Other connections?
                foreach (IConnectionInfo connection in document.DataSourceConnections)
                {
                    connection.SetConnection(serverName, dbName, useTrustedConnection);
                    connection.SetLogon(userName, password);
                    connection.LogonProperties.Set("Data Source", serverName);
                    connection.LogonProperties.Set("Initial Catalog", dbName);
                    // Point to the SQL Server Native Client 11.0 driver
                    connection.LogonProperties.Set("Driver", "{SQL Server Native Client 11.0}");
                }

                // Only do this to the main report (can't do it to sub reports)
                if (!document.IsSubreport)
                {
                    // Apply to subreports
                    foreach (ReportDocument rd in document.Subreports)
                    {
                        ApplyLoginInfo(rd, serverName, dbName, useTrustedConnection, userName, password);
                    }
                }

                // Apply to tables
                foreach (CrystalDecisions.CrystalReports.Engine.Table table in document.Database.Tables)
                {
                    TableLogOnInfo tableLogOnInfo = table.LogOnInfo;
                    tableLogOnInfo.ConnectionInfo = info.ConnectionInfo;
                    table.ApplyLogOnInfo(tableLogOnInfo);

                    if (!table.TestConnectivity())
                    {
                        message = "Failed to apply log in info for Crystal Report";
                    }
                }

                #endregion

                try
                {
                    // Break it all down
                    document.VerifyDatabase();
                }
                catch (LogOnException excLogon)
                {
                    message = excLogon.Message;
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException("Failed to apply login information to the report - " + ex.Message);
            }
        }

        #endregion Private Methods

    }
}
