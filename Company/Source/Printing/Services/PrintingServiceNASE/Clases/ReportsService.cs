using Sistran.Co.Application.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sistran.Company.PrintingService.NASE.Clases
{
    public class ReportsService
    {
        private Report report;

        /// <summary>
        /// Genera el reporte para una Poliza o un Temporario.
        /// </summary>
        /// <param name="xmlRequest">Xml enviado desde el cliente</param>
        /// <returns>Ruta del reporte</returns>
        public string GenerateReport(string xmlRequest)
        {
            string strReportData;
            DataSet dsOutput = new DataSet("StreamOut");
            StringReader sr = new StringReader(xmlRequest);
            dsOutput.ReadXml(sr);

            //Tipo de proceso
            bool IsAsynchronousProcess = Convert.ToBoolean(dsOutput.Tables["PolicyPrinting"].Rows[0]["IsAsynchronousProcess"].ToString());//Proceso Asincrono

            //Datos de la cantidad de archivos a imprimir
            int[] RangeValues = new int[2];
            RangeValues[0] = Convert.ToInt32(dsOutput.Tables["PolicyPrinting"].Rows[0]["RangeMinValue"].ToString());//First
            RangeValues[1] = Convert.ToInt32(dsOutput.Tables["PolicyPrinting"].Rows[0]["RangeMaxValue"].ToString());//Last 
                                                                                                                    //<TODO: Edgar Piraneque; Compañía: 1; 14/12/2010; HD-2688 Ajuste impresión sustitución
            int diffRisks = 0;
            int prefixNum = Convert.ToInt32(dsOutput.Tables["PolicyPrinting"].Rows[0]["PrefixNum"]);
            if (prefixNum != Convert.ToInt32(ReportEnum.PrefixCode.LOCATION))
            {
                diffRisks = RangeValues[1] - RangeValues[0] + 1;
            }
            else
            {
                diffRisks = 1;
            }
            // Edgar Piraneque; 14/12/2010>
            //Datos de la conexión

            //TODO: Fecha: 09/08/2011 Autor: John Ruiz; Asunto: Se agrega funcionalidad para no especificar dos veces la cadena de conexion en el archivo de configuración
            DynamicDataAccess dda = new DynamicDataAccess();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(dda.DataBaseName);

            string[] strDataConn = new string[4];

            strDataConn[0] = builder.UserID;
            strDataConn[1] = builder.Password;
            strDataConn[2] = builder.DataSource;
            strDataConn[3] = builder.InitialCatalog;

            //strDataConn[0] = ConfigurationSettings.AppSettings["UserReport"];//user
            //strDataConn[1] = ConfigurationSettings.AppSettings["PasswordReport"];//password
            //strDataConn[2] = ConfigurationSettings.AppSettings["ServerReport"];//server
            //strDataConn[3] = ConfigurationSettings.AppSettings["DatabaseReport"];//database

            report = new Report(dsOutput, strDataConn);
            report.GenerationCompleted += new Report.ReportCompletedHandler(report_GenerationCompleted);

            //Tipo de reporte solicitado de la solicitud agrupadora.
            int typeReport = Convert.ToInt32(dsOutput.Tables["PolicyPrinting"].Rows[0]["TypeReport"]);


            if (IsAsynchronousProcess)
            {
                if (typeReport == (int)ReportEnum.ReportType.MASS_LOAD)
                {
                    Thread thrReport = new Thread(new ThreadStart(report.printMassiveLoadPolicies));
                    thrReport.Start();
                }
                else if ((typeReport == (int)ReportEnum.ReportType.COMPLETE_REQUEST) ||
                    (typeReport == (int)ReportEnum.ReportType.ONLY_REQUEST))
                {
                    Thread thrReport = new Thread(new ThreadStart(report.printGroupRequestPolicies));
                    thrReport.Start();

                }
                else
                {
                    Thread thrReport = new Thread(new ThreadStart(report.printGeneralPolicies));
                    thrReport.Start();
                }

                dsOutput.Tables["PendingPrintData"].Rows[0]["PrintProcessId"] = report.PrintProcessId;
                dsOutput.Tables["PendingPrintData"].Rows[0]["FileName"] = report.FileName;
                dsOutput.Tables["PendingPrintData"].Rows[0]["FileSize"] = report.FileSize;
                dsOutput.Tables["PendingPrintData"].Rows[0]["FilePath"] = report.FilePath;
                dsOutput.Tables["PendingPrintData"].Rows[0]["RangeMinValue"] = report.RangeMinValue;
                dsOutput.Tables["PendingPrintData"].Rows[0]["RangeMaxValue"] = report.RangeMaxValue;
                dsOutput.Tables["PendingPrintData"].Rows[0]["DocTypeDescription"] = report.DocTypeDescription;

                strReportData = dsOutput.GetXml();
            }
            else
            {
                if (typeReport == (int)ReportEnum.ReportType.MASS_LOAD)
                {
                    report.printMassiveLoadPolicies();
                }
                else if ((typeReport == (int)ReportEnum.ReportType.COMPLETE_REQUEST) ||
                    (typeReport == (int)ReportEnum.ReportType.ONLY_REQUEST))
                {
                    report.printGroupRequestPolicies();
                }

                //TODO:  <<Autor: Luisa Fernanda Ramirez; Fecha: 27/12/2010; Asunto: OT-0051 Renovacion de Autos Individuales. Compañía: 1 
                else if (typeReport == (int)ReportEnum.ReportType.MASSIVE_RENWAL)
                {
                    report.printGenerateRenewalTemplateAgent();
                }
                /* Autor: Luisa Fernanda Ramirez, Fecha: 27/12/2010 >>*/
                else
                {
                    //if (diffRisks == 1)
                    //{
                        report.printGeneralPolicies();
                    //}
                    //else
                    //{
                    //    report.printCollectivePolicies();
                    //}
                }

                dsOutput.Tables["PendingPrintData"].Rows[0]["FilePath"] = report.FilePath;
                strReportData = dsOutput.GetXml();
            }

            return strReportData;
        }

        /// <summary>
        /// Manejo del evento fin de tarea de un hilo.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Evento</param>
        private void report_GenerationCompleted(object sender, EventArgs e)
        {
            lock (this)//Bloqueo del recurso de código para manejo de concurrencia
            {
                NameValue[] spParam = new NameValue[9];
                spParam[0] = new NameValue("ASYNC_PROCESS_ID", report.AsyncProcessId);
                spParam[1] = new NameValue("PRINT_PROCESS_ID", report.PrintProcessId);
                spParam[2] = new NameValue("DESCRIPTION", ConfigurationSettings.AppSettings["ProcessDescription"]);
                spParam[3] = new NameValue("FILE_STATUS", report.Status);
                spParam[4] = new NameValue("HAS_ERROR", report.HasError);
                spParam[5] = new NameValue("ERROR_DESCRIPTION", report.ErrorDescription);
                spParam[6] = new NameValue("FILE_NAME", report.FileName);
                spParam[7] = new NameValue("FILE_PATH", report.FilePath);
                spParam[8] = new NameValue("FILE_SIZE", report.FileSize);

                //Actualiza el estado del reporte y sus datos adicionales
                ReportServiceHelper.getData("REPORT.CO_PENDING_PRINT_FILE_STATUS", spParam);
            }
        }
    }
}
