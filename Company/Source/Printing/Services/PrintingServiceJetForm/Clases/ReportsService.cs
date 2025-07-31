using Sistran.Company.Application.PrintingServices.Clases.Quotation;
using Sistran.Company.Application.PrintingServices.Clases.Temp;
using Sistran.Company.Application.PrintingServices.Enums;
using Sistran.Company.Application.PrintingServices.Models;
using Sistran.Company.Application.PrintingServices.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.PrintingService.JetForm.Clases
{
    public class ReportsService : IReportsService
    {
        private Reporte report;
        private IReporte _report;

        /// <summary>
        /// Genera el reporte para una Poliza o un Temporario.
        /// </summary>
        /// <param name="xmlRequest">Xml enviado desde el cliente</param>
        /// <returns>Ruta del reporte</returns>
        public string GenerateReport(string xmlRequest)
        {
            string strReportData = string.Empty;
            DataSet dsOutput = new DataSet("StreamOut");
            StringReader sr = new StringReader(xmlRequest);
            dsOutput.ReadXml(sr);

            //Tipo de proceso
            bool IsCollective = Convert.ToBoolean(dsOutput.Tables["PolicyPrinting"].Rows[0]["IsAsynchronousProcess"].ToString());//Proceso Asincrono
            bool IsMassive = Convert.ToBoolean(dsOutput.Tables["MigratedPolicies"].Rows[0]["ExportToExcel"].ToString());//Se utiliza propiedad para proceso de masivos

            //Datos de la cantidad de archivos a imprimir
            int[] RangeValues = new int[2];
            RangeValues[0] = Convert.ToInt32(dsOutput.Tables["PolicyPrinting"].Rows[0]["RangeMinValue"].ToString());
            RangeValues[1] = Convert.ToInt32(dsOutput.Tables["PolicyPrinting"].Rows[0]["RangeMaxValue"].ToString());

            report = new Reporte(dsOutput);
            report.GenerationCompleted += new Reporte.ReportCompletedHandler(reporte_GenerationCompleted);

            report.OperationId = Convert.ToInt32(dsOutput.Tables["GroupRequest"].Rows[0]["RequestId"].ToString());

            //Tipo de reporte solicitado de la solicitud agrupadora.
            int typeReport = Convert.ToInt32(dsOutput.Tables["PolicyPrinting"].Rows[0]["TypeReport"]);

            this._report = selectPrefixToPrint();

            if (this._report != null)
            {
                this.report.iReporte = this._report;
                this.report.print(IsMassive, IsCollective);
                dsOutput.Tables["PendingPrintData"].Rows[0]["FilePath"] = this.report.iReporte.PdfFilePath;
                strReportData = dsOutput.GetXml();
            }
            else
            {
                strReportData = RptFields.ERR_MSG_GENERIC_ERROR;
            }


            return strReportData;
        }

        /// <summary>
        /// Select the prefix type to print
        /// </summary>
        /// <returns>Report Interface</returns>
        private IReporte selectPrefixToPrint()
        {
            try
            {
                int prefixTypeNum = Convert.ToInt32(report.DsPolicyRisks.Tables[2].Rows[0]["PREFIX_TYPE_CD"].ToString());

                switch (prefixTypeNum)
                {
                    case ((int)PrefixTypeCode.AUTOS):
                        if (this.report.PolicyData.PolicyId != 0)
                            return new Autos();
                        else if (this.report.PolicyData.TempNum != 0 && report.PolicyData.QuotationId.Equals(0))
                            return new TmpAutos();
                        else
                            return new QuoAutos();
                    case ((int)PrefixTypeCode.JUDICIAL_SURETY):
                        if (this.report.PolicyData.PolicyId != 0)
                            return new CaucionJudicial();
                        else if (this.report.PolicyData.TempNum != 0 && report.PolicyData.QuotationId.Equals(0))
                            return new TmpCaucionJudicial();
                        else
                            return new QuoCaucionJudicial();
                    case ((int)PrefixTypeCode.SURETY):
                        if (this.report.PolicyData.PolicyId != 0)
                            return new Cumplimiento();
                        else if (this.report.PolicyData.TempNum != 0 && report.PolicyData.QuotationId.Equals(0))
                            return new TmpCumplimiento();
                        else
                            return new QuoCumplimiento();
                    case ((int)PrefixTypeCode.GENERALS):
                        if (this.report.PolicyData.PolicyId != 0)
                            return new Generales();
                        else if (this.report.PolicyData.TempNum != 0 && report.PolicyData.QuotationId.Equals(0))
                            return new TmpGenerales();
                        else
                            return new QuoGenerales();
                    case ((int)PrefixTypeCode.MANAGE):
                        if (report.PolicyData.PolicyId != 0)
                            return new Manejo();
                        else if (report.PolicyData.TempNum != 0 && report.PolicyData.QuotationId.Equals(0))
                            return new TmpManejo();
                        else
                            return new QuoManejo();
                    /*case ((int)PrefixTypeCode.MODULAR):
                        if (this.report.PolicyData.PolicyId != 0)
                            return new Modular();
                        else if (this.report.PolicyData.TempNum != 0 && report.PolicyData.QuotationId.Equals(0))
                            return new TmpModular();
                        else
                            return new QuoModular();*/
                    case ((int)PrefixTypeCode.TRANSPORTS):
                        if (this.report.PolicyData.PolicyId != 0)
                            return new Transportes();
                        else if (this.report.PolicyData.TempNum != 0 && report.PolicyData.QuotationId.Equals(0))
                            return new TmpTransportes();
                        else
                            return new QuoTransportes();
                    case ((int)PrefixTypeCode.LIABILITY):
                        if (this.report.PolicyData.PolicyId != 0)
                            return new ResponsabilidadCivil();
                        else if (this.report.PolicyData.TempNum != 0 && report.PolicyData.QuotationId.Equals(0))
                            return new TmpResponsabilidadCivil();
                        else
                            return new QuoResponsabilidadCivil();
                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Manejo del evento fin de tarea de un hilo.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Evento</param>
        private void reporte_GenerationCompleted(object sender, EventArgs e)
        {
            //lock (this)//Bloqueo del recurso de código para manejo de concurrencia
            //{
            //    NameValue[] spParam = new NameValue[9];
            //    spParam[0] = new NameValue("ASYNC_PROCESS_ID", report.AsyncProcessId);
            //    spParam[1] = new NameValue("PRINT_PROCESS_ID", report.PrintProcessId);
            //    spParam[2] = new NameValue("DESCRIPTION", "PRINTER");
            //    spParam[3] = new NameValue("FILE_STATUS", report.Status);
            //    spParam[4] = new NameValue("HAS_ERROR", report.HasError);
            //    spParam[5] = new NameValue("ERROR_DESCRIPTION", report.ErrorDescription);
            //    spParam[6] = new NameValue("FILE_NAME", report.Ramo.FileName);
            //    spParam[7] = new NameValue("FILE_PATH", report.Ramo.PdfFilePath);
            //    spParam[8] = new NameValue("FILE_SIZE", report.FileSize);

            //    //Actualiza el estado del reporte y sus datos adicionales
            //    ReportServiceHelper.getData("REPORT.CO_PENDING_PRINT_FILE_STATUS", spParam);
            //}
        }

        private string GenerateDATReport(DataSet source, int branchId, string PDFfile, string DATFile, string PDFPath, string DATPath, string templateName, string printerName)
        {
            return string.Empty;
        }
    }
}
