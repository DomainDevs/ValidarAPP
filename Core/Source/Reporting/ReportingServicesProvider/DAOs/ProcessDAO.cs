// Sistran Core
using Sistran.Core.Application.ReportingServices.Models;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using procedure = Sistran.Core.Framework.DAF.Engine.StoredProcedure;

using System;
using System.Collections;
using System.Collections.Generic;
using Sistran.Co.Application.Data;
using System.Data;
using Sistran.Core.Application.ReportingServices.Provider.Helpers;

namespace Sistran.Core.Application.ReportingServices.Provider.DAOs
{
    public class ProcessDAO
    {
        #region Instance Variables

        #region Interfaz

        /// <summary>
        /// Declaración del contexto y del dataFacadeManager
        /// </summary>
        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Interfaz

        #region DAOs

        private readonly MassiveReportDAO _massiveReportDAO = new MassiveReportDAO();

        #endregion DAOs

        #endregion Instance Variables

        #region Public Methods

        #region Report

        /// <summary>
        /// GenerateReport
        /// </summary>
        /// <param name="report"></param>
        /// <param name="massiveReport"></param>
        public void GenerateReport(Report report, MassiveReport massiveReport)
        {
            MassiveReport saveMassiveReport = new MassiveReport();

            try
            {
                // Graba la cabecera - tabla de proceso
                saveMassiveReport = _massiveReportDAO.SaveMassiveReport(massiveReport);

                int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                report.StoredProcedure.ProcedureParameters.Add(new Parameter
                {
                    Id = parameterNumber + 1,
                    Description = "@MASSIVE_REPORT_ID",
                    IsFormula = false,
                    //SMT-1914 Inicio
                    Value = saveMassiveReport.Id
                    //SMT-1914 Fin
                });
                report.StoredProcedure.ProcedureParameters.Add(new Parameter
                {
                    Id = parameterNumber + 2,
                    Description = "@EXECUTE",
                    IsFormula = false,
                    //SMT-1914 Inicio
                    Value = 1
                    //SMT-1914 Fin
                });

                GetDataReportByProcedure(report);
            }
            catch (BusinessException exception)
            {
                List<string> errorList = new List<string>();
                errorList.Add(exception.Message);
                if (exception.InnerException != null && exception.InnerException.Message != null)
                {
                    errorList.Add(exception.InnerException.Message);
                }
                massiveReport.EndDate = DateTime.Now;
                massiveReport.Id = saveMassiveReport.Id;
                massiveReport.Success = false;
                massiveReport.UrlFile = errorList[0];

                _massiveReportDAO.UpdateMassiveReport(massiveReport);
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetTotalRecords
        /// </summary>
        /// <param name="report"></param>
        /// <returns>int</returns>
        public int GetTotalRecords(Report report)
        {
            int recordCount = 0;
            try
            {
                // Ejecución de procedimiento de consulta de datos
                DataTable reports = GetDataReportByProcedure(report);
                foreach (DataRow item in reports.Rows)
                {
                    recordCount = Convert.ToInt32(item[0]);
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return recordCount;
        }

        #endregion

        #region Procedures

        /// <summary>
        /// GetDataReportByProcedure
        /// </summary>
        /// <param name="report"></param>
        /// <returns>ArrayList</returns>
        private DataTable GetDataReportByProcedure(Report report)
        {
            DataTable collections = null;

            string procedure = report.StoredProcedure.ProcedureName;
            List<Parameter> parameters = report.StoredProcedure.ProcedureParameters;

            try
            {

                if (report.IsAsync)
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
                else
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
                         collections = dynamicDataAccess.ExecuteSPDataTable(procedure, procedureParameters);
                    }
                }

                return collections;
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
