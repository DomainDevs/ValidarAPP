using Sistran.Co.Application.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.PrintingService.NASE.Clases
{
    public class ExecutePrinterReporActionCo
    {
        public ExecutePrinterReporActionCo()
        {

        }

        public ExecutePrinterReporResponseCo InternalProcess(ExecutePrinterReporRequestCo request)
        {
            string strReportData = "";

            ReportsService reportsService = new ReportsService();

            strReportData = reportsService.GenerateReport(getXml(request));//metodo del WebService al que se le envia el XML

            ExecutePrinterReporResponseCo rspPrinter = new ExecutePrinterReporResponseCo();

            //Se asigna la ruta.
            rspPrinter.PathReportPolicy = "";
            rspPrinter.PathReportConvention = "";
            rspPrinter.ReportPrinter = convertResponseToDs(strReportData);

            return rspPrinter;
        }

        public string getXml(ExecutePrinterReporRequestCo request)
        {
            string iPath = ConfigurationSettings.AppSettings["PATHXSD"];
            int TypeReport = request.TypeReport;
            bool WithFormatCollect = request.WithFormatCollect;
            int PolicyId = request.PolicyId;
            int RequestId = request.RequestId;
            int EndorsementId = request.EndorsementId;
            int AsyncProcessId = request.AsyncProcessId;
            int PrintProccesid = request.PrintProccesid;
            string User = request.User;
            string CodeBar = request.CodeBar;
            int QuotationId = request.QuotationId;
            int TempNum = request.TempNum;
            int PrefixNum = request.PrefixNum;
            int RangeMinValue = request.RangeMinValue;
            int RangeMaxValue = request.RangeMaxValue;
            int IntermediaryId = request.IntermediaryId;
            int BranchId = request.BranchId;
            string ProcessFromDate = request.ProcessFromDate;
            string ProcessToDate = request.ProcessToDate;
            bool ExportToExcel = request.ExportToExcel;
            bool IsAsynchronousProcess = request.IsAsynchronousProcess;
            int IdPv = request.IdPv;
            bool CurrentFromFirst = request.CurrentFromFirst;
            bool EndorsementText = request.EndorsementText;
            bool TempAuthorization = request.TempAuthorization;
            //TODO:  <<Autor: Edgar Cervantes De Los Rios; Fecha: 02/08/2010; Asunto: HD 2508 - Se agrega checkBox para validar si en la impresión, la fecha desde debe ser la misma fecha de inicio de vigencia de la póliza. Esto solo aplica para cumplimiento. Compañía: 1 - CPT.
            bool PrintFromCurrentFromDate = request.PrintFromCurrentFromDate;
            /* Autor: Edgar Cervantes De Los Rios, Fecha: 02/08/2010 >>*/

            // << TODO: Edgar O. Piraneque E.; 05/11/2010; Se incluye propiedad para retornar el saldo del endoso en 2g 
            int premiumBalance = request.PremiumBalance;
            // Edgar O. Piraneque E.; 05/11/2010;>>

            //TODO:  <<Autor: Miguel López; Fecha: 06/10/2010; Asunto: Agregamos el parámetro de número de placa de un vehículo. Aplica únicamente para impresión de carnet RC Pasajeros
            string LicensePlate = request.LicensePlate;
            /* Autor: Miguel López, Fecha: 06/10/2010 >>*/

            //TODO:  <<Autor: Miguel López; Fecha: 07/10/2010; Asunto: Agregamos el parámetro que determina si debe mostrar el valor de la prima. Aplica únicamente para impresión de carnet RC Pasajeros
            bool ShowPremium = request.ShowPremium;
            /* Autor: Miguel López, Fecha: 07/10/2010 >>*/

            //TODO:  <<Autor: Miguel López; Fecha: 08/10/2010; Asunto: Agregamos el parámetro que almacena la cantidad de copias requeridas. Aplica únicamente para impresión de carnet RC Pasajeros
            int CopiesQuantity = request.CopiesQuantity;
            /* Autor: Miguel López, Fecha: 08/10/2010 >>*/

            DataSet dsInput = new DataSet("StreamIn");
            dsInput.ReadXmlSchema(iPath);

            //TODO:  <<Autor: Edgar Cervantes De Los Rios; Fecha: 02/08/2010; Asunto: HD 2508 - Se agrega checkBox para validar si en la impresión, la fecha desde debe ser la misma fecha de inicio de vigencia de la póliza. Esto solo aplica para cumplimiento. Compañía: 1 - CPT.
            dsInput.Tables["PolicyPrinting"].Rows.Add(PrintProccesid, TypeReport, PolicyId, EndorsementId, QuotationId, User, CodeBar, TempNum, PrefixNum, RangeMinValue, RangeMaxValue, WithFormatCollect, IsAsynchronousProcess, IdPv, PrintFromCurrentFromDate, premiumBalance, LicensePlate, ShowPremium, CopiesQuantity, CurrentFromFirst, EndorsementText, TempAuthorization);
            /* Autor: Edgar Cervantes De Los Rios, Fecha: 02/08/2010 >>*/
            dsInput.Tables["GroupRequest"].Rows.Add(RequestId);
            dsInput.Tables["MigratedPolicies"].Rows.Add(IntermediaryId, BranchId, ProcessFromDate, ProcessToDate, ExportToExcel);
            dsInput.Tables["PendingPrintData"].Rows.Add(AsyncProcessId, PrintProccesid, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            return dsInput.GetXml();
        }

        /// <summary>
        /// Convierte la respuesta del WebService el un DataSet.
        /// </summary>
        /// <param name="strReportData">Cadena devuelta por el WebService</param>
        /// <returns>SerialDataSet con los datos de la respuesta</returns>
        public SerialDataSet convertResponseToDs(string strReportData)
        {
            SerialDataSet dsOutput = new SerialDataSet();
            StringReader sr = new StringReader(strReportData);
            dsOutput.ReadXml(sr);
            return dsOutput;
        }
    }
}
