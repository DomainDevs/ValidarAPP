// Sistran Core
using Sistran.Core.Application.ReportingServices.Provider.Helpers;
using Sistran.Core.Application.ReportingServices.Models;
using procedure = Sistran.Core.Framework.DAF.Engine.StoredProcedure;

//Sistran FWK
using Sistran.Core.Framework.BAF;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Sistran.Co.Application.Data;

namespace Sistran.Core.Application.ReportingServices.Provider.DAOs
{
    public class ReportDAO
    {
        #region Instance Variables

        #region DAOs

        private readonly MassiveReportDAO _massiveReportDAO = new MassiveReportDAO();

        #endregion DAOs

        #endregion Instance Variables

        #region Public Methods

        #region Report

        /// <summary>
        /// GenerateReportFile
        /// </summary>
        /// <param name="report"></param>
        public void GenerateReportFile(Report report)
        {
            MassiveReport massiveReport = new MassiveReport();
            string exportFileName = "";
            string virtualFileName = "";
            string serverName = ConfigurationManager.AppSettings["ODBCReport"].ToString();
            string connection = ConfigurationManager.ConnectionStrings["conexionActivaDAF"].ConnectionString;
            string[] connections = connection.Split(';');
            string databaseName = connections[1].Substring(connections[1].IndexOf('=') + 1);
            string userName = connections[2].Substring(connections[2].IndexOf('=') + 1);
            string password = connections[3].Substring(connections[3].IndexOf('=') + 1);
            string sharedReport = ConfigurationManager.AppSettings["SharedReportFolder"].ToString();
            string sharedPortable = ConfigurationManager.AppSettings["SharedPortableDocumentFolder"].ToString();
            string virtualPath = ConfigurationManager.AppSettings["VirtualPath"].ToString();

            string procedureName = report.StoredProcedure.ProcedureName;
            bool isAsync = report.IsAsync;
            string filter = report.Filter;
            List<Parameter> parameters = report.Parameters;
            List<Parameter> procedureParameters = report.StoredProcedure.ProcedureParameters;
            
            ExportTypes exportType = report.ExportType;

            try
            {
                if (isAsync)
                {
                    // Se inserta 
                    massiveReport.Description = report.Description;
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.UrlFile = report.Name;
                    massiveReport.UserId = report.UserId;

                    massiveReport = _massiveReportDAO.SaveMassiveReport(massiveReport);
                }

                ReportHelper reportHelper = new ReportHelper();

                exportFileName = this.GenerateExportFileName(sharedPortable, report.Name, true, exportType);

                // CARPETA VIRTUAL DE REPORTES GENERADOS
                virtualFileName = exportFileName.Replace(sharedPortable, "");

                virtualFileName = virtualPath + virtualFileName;

                using (reportHelper)
                {
                    GetReportByProcedureParameters(procedureName, procedureParameters);

                    reportHelper.InitialiseCrystalReport(sharedReport + report.Name);

                    reportHelper.SetDataSourceConnection(serverName.Trim(), databaseName.Trim(), userName.Trim(), password.Trim());

                    reportHelper.SetRecordSelectionFormula(filter);

                    foreach (Parameter parameter in parameters)
                    {
                        if (parameter.IsFormula)
                        {
                            reportHelper.SetParameter(parameter.Description, parameter.Value);
                        }
                        else
                        {
                            reportHelper.SetFormulaField(parameter.Description, parameter.Value);
                        }
                    }

                    reportHelper.ExportCrystalReport(report.ExportType, exportFileName);

                    if (isAsync)
                    {
                        // Se actualiza 
                        massiveReport.EndDate = DateTime.Now;
                        massiveReport.Success = true;
                        massiveReport.UrlFile = virtualFileName;
                        massiveReport.UserId = report.UserId;

                        _massiveReportDAO.UpdateMassiveReport(massiveReport);
                    }
                }
            }
            catch (BusinessException exception)
            {
                if (isAsync)
                {
                    // Se actualiza 
                    massiveReport.EndDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.UrlFile = exception.Message;
                    massiveReport.UserId = report.UserId;

                    _massiveReportDAO.UpdateMassiveReport(massiveReport);
                }
                else
                {
                    throw new BusinessException(exception.Message);
                }
            }
        }

        #endregion

        #region ExportFile

        /// <summary>
        /// GenerateExportFileName
        /// </summary>
        /// <param name="exportFolder"></param>
        /// <param name="exportFileName"></param>
        /// <param name="includeTimestampOnFile"></param>
        /// <param name="isPdf"></param>
        /// <returns>string</returns>
        private string GenerateExportFileName(string exportFolder, string exportFileName, bool includeTimestampOnFile, ExportTypes exportType)
        {
            string dateTime = string.Format("{0:yyyyMMddhhmmss}", DateTime.Now);
            string extension = ".pdf";

            if (exportType == ExportTypes.PDF)
            {
                extension = ".xls";
            }


            if (!Directory.Exists(exportFolder))
            {
                Directory.CreateDirectory(exportFolder);
            }
            string tempFileName = exportFileName;
            int index = exportFileName.LastIndexOf(".");
            if (index <= 0)
            {
                tempFileName = string.Concat(exportFolder, tempFileName);
            }
            else
            {
                tempFileName = exportFileName.Substring(0, index);
                if (!includeTimestampOnFile)
                {
                    tempFileName = string.Concat(exportFolder, tempFileName, extension);
                }
                else
                {
                    string[] folders = new string[] { exportFolder, tempFileName, "_", dateTime, extension };
                    tempFileName = string.Concat(folders);
                }
            }

            return tempFileName;
        }

        #endregion

        #region Procedures

        /// <summary>
        /// GetReportByProcedureParameters
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="parameters"></param>
        private void GetReportByProcedureParameters(string procedure, List<Parameter> parameters)
        {
            try
            {
                var procedureParameters = new NameValue[parameters.Count];
                var index = 0;
                foreach (Parameter parameter in parameters)
                {
                    if (parameters[index].Value == null || parameters[index].Value.ToString() == "null")
                    {
                        procedureParameters[index] = new NameValue(parameters[index].Description, DBNull.Value, DBTypeHelper.GetDbType(parameters[index]));
                    }
                    else
                    {
                        procedureParameters[index] = new NameValue(parameters[index].Description, parameters[index].Value);
                    }

                    index++;
                }
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    dynamicDataAccess.ExecuteSPDataTable(procedure, procedureParameters);
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #endregion Public Methods

    }
}
