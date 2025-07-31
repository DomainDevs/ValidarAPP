using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Sistran.Co.Application.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sistran.Company.PrintingService.NASE.Clases
{
    public class ReportServiceHelper
    {
        /// <summary>
        /// Valor constante que se envía al reporte como referencia 2
        /// </summary>
        public static Int64 reference2 = 9999999998;
        /// <summary>
        /// Elimina los archivos pdf que ya no se utilizan.
        /// </summary>
        /// <param name="rutas">Rutas de los archivos que ya no se utilizan.</param>
        public static void deleteFiles(ArrayList rutas)
        {
            for (int i = 1; i < rutas.Count; i++)
            {
                deleteFile(((Paths)rutas[i]).FilePath);
            }
        }

        /// <summary>
        /// Elimina un archivo del Sistema de Archivos.
        /// </summary>
        /// <param name="strPath">Ruta del archivo.</param>
        private static void deleteFile(string strPath)
        {
            FileInfo fiPdf = new FileInfo(strPath);//Se  instancia un objeto FileInfo con el archivo.

            if (fiPdf.Exists)//Valida si existe archivo.
            {
                fiPdf.Delete();
            }
        }
        /// <summary>
        /// Concatena los archivos generados.
        /// </summary>
        /// <param name="rutas">Rutas de los archivos generados</param>
        public static void joinPdfFiles(ArrayList rutas)
        {
            try
            {
                int i = 0;
                int file = 1;
                int pages = 0;
                int rotation = 0;
                string[] finalPdfPaths = new string[rutas.Count];

                foreach (Paths ruta in rutas)
                {
                    finalPdfPaths[i] = ruta.FilePath;
                    i++;
                }

                string[] pdfPaths = redimString(finalPdfPaths);
                PdfReader reader = new PdfReader(pdfPaths[file]);
                pages = reader.NumberOfPages;

                Document document = new Document(reader.GetPageSizeWithRotation(1));
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfPaths[0], FileMode.Create));
                document.Open();

                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                while (file < pdfPaths.Length)
                {
                    int index = 0;
                    while (index < pages)
                    {
                        index++;
                        document.SetPageSize(reader.GetPageSizeWithRotation(index));
                        document.NewPage();
                        page = writer.GetImportedPage(reader, index);
                        rotation = reader.GetPageRotation(index);
                        if (rotation == 90 || rotation == 270)
                        {
                            cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(index).Height);
                        }
                        else
                        {
                            cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                        }
                    }
                    file++;
                    if (file < pdfPaths.Length)
                    {
                        reader = new PdfReader(pdfPaths[file]);
                        pages = reader.NumberOfPages;
                    }
                }
                document.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // << TODO: Autor: Miguel López; Fecha: 21/10/2010; Asunto: Se sobrecarga este método para que reciba como parámetros los
        //                                                          márgenes del documento PDF. Asimismo se asignan atributos como la barra de
        //                                                          herramientas invisible y la escala real al momento de imprimir. Esto aplica para
        //                                                          la impresión de carnet RC Pasajeros.

        /// <summary>
        /// Concatena los archivos generados.
        /// </summary>
        /// <param name="rutas">Rutas de los archivos generados</param>
        /// <param name="top">Margen superior</param>
        /// <param name="bottom">Margen inferior</param>
        /// <param name="left">Margen izquierdo</param>
        /// <param name="right">Margen derecho</param>
        public static void joinPdfFiles(ArrayList rutas, int top, int bottom, int left, int right)
        {
            try
            {
                int i = 0;
                int file = 1;
                int pages = 0;
                int rotation = 0;
                string[] finalPdfPaths = new string[rutas.Count];

                foreach (Paths ruta in rutas)
                {
                    finalPdfPaths[i] = ruta.FilePath;
                    i++;
                }

                string[] pdfPaths = redimString(finalPdfPaths);
                PdfReader reader = new PdfReader(pdfPaths[file]);
                pages = reader.NumberOfPages;

                Document document = new Document(PageSize.LETTER, (float)left, (float)right, (float)top, (float)bottom);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfPaths[0], FileMode.Create));
                writer.AddViewerPreference(PdfName.HIDEMENUBAR, new PdfBoolean(true));
                writer.AddViewerPreference(PdfName.HIDETOOLBAR, new PdfBoolean(true));
                writer.SetMargins((float)left, (float)right, (float)top, (float)bottom);
                writer.AddViewerPreference(PdfName.PRINTSCALING, PdfName.NONE);

                document.Open();

                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                while (file < pdfPaths.Length)
                {
                    int index = 0;
                    while (index < pages)
                    {
                        index++;
                        document.SetPageSize(reader.GetPageSizeWithRotation(index));
                        document.NewPage();
                        page = writer.GetImportedPage(reader, index);
                        rotation = reader.GetPageRotation(index);
                        if (rotation == 90 || rotation == 270)
                        {
                            cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(index).Height);
                        }
                        else
                        {
                            cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                        }
                    }
                    file++;
                    if (file < pdfPaths.Length)
                    {
                        reader = new PdfReader(pdfPaths[file]);

                        pages = reader.NumberOfPages;
                    }
                }
                document.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* Autor: Miguel López, Fecha: 21/10/2010 >>*/

        /// <summary>
        /// Cambia la configuración de las conexiones de un reporte.
        /// </summary>
        /// <param name="collDSConnections">Colección de DataSourceConnections.</param>
        /// <param name="strDataConn">Datos para la conexión a la BD.</param>
        public static void setConnections(DataSourceConnections collDSConnections, string[] strDataConn)
        {
            //Se recorre cada objeto IConnectionInfo de la colección.
            foreach (IConnectionInfo tmpIConn in collDSConnections)
            {
                //Se configura la conexión.
                tmpIConn.SetConnection(strDataConn[2], strDataConn[3], strDataConn[0], strDataConn[1]);

                //Se configura la propiedad Data Source.
                tmpIConn.LogonProperties.Set("Data Source", strDataConn[2]);

                //Se configura la propiedad Initial Catalog.
                tmpIConn.LogonProperties.Set("Initial Catalog", strDataConn[3]);
            }
        }

        /// <summary>
        /// Ejecuta un Stored Procedure a la base de datos definida en el web config.
        /// </summary>
        /// <param name="storedProcedure">Nombre del Stored Procedure.</param>
        /// <param name="parametros">Collección de tipo NameValue que contiene los parametros para ejecutar el Stored Procedure.</param>
        /// <returns></returns>
        public static SerialDataSet getData(string storedProcedure, params NameValue[] parametros)
        {
            DynamicDataAccess pdb = new DynamicDataAccess();
            return pdb.ExecuteSPSerDataSet(storedProcedure, parametros);
        }

        /// <summary>
        /// Adiciona marca de agua del mensaje especificado al archivo pdf seleccionado.
        /// </summary>
        /// <param name="pdfName">Ruta completa del archivo</param>
        /// <param name="stringToWriteToPdf">Mensaje a imprimir como Marca de Agua</param>
        /// <returns></returns>
        public static byte[] setWaterMark(String pdfName, String stringToWriteToPdf)
        {
            int R = Convert.ToInt32(ConfigurationSettings.AppSettings["Red"]);
            int G = Convert.ToInt32(ConfigurationSettings.AppSettings["Green"]);
            int B = Convert.ToInt32(ConfigurationSettings.AppSettings["Blue"]);
            int FontSize = Convert.ToInt32(ConfigurationSettings.AppSettings["FontSize"]);

            PdfReader reader = new PdfReader(pdfName);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfStamper pdfStamper = new PdfStamper(reader, memoryStream);

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    Rectangle pageSize = reader.GetPageSizeWithRotation(i);
                    PdfContentByte pdfPageContents = pdfStamper.GetUnderContent(i);
                    pdfPageContents.BeginText();
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, Encoding.ASCII.EncodingName, false);
                    pdfPageContents.SetFontAndSize(baseFont, FontSize);
                    pdfPageContents.SetRGBColorFill(R, G, B);
                    float textAngle = (float)getHypotenuseAngleInDegreesFrom(pageSize.Height, pageSize.Width);
                    pdfPageContents.ShowTextAligned(PdfContentByte.ALIGN_CENTER, stringToWriteToPdf, pageSize.Width / 2, pageSize.Height / 2, textAngle);
                    pdfPageContents.EndText();
                }
                pdfStamper.FormFlattening = true;
                pdfStamper.Close();
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Calcula el angulo de inclinación de la marca de agua en el archivo a imprimir
        /// </summary>
        /// <param name="opposite">Ángulo opuesto</param>
        /// <param name="adjacent">Ángulo adyacente</param>
        /// <returns></returns>
        public static double getHypotenuseAngleInDegreesFrom(double opposite, double adjacent)
        {
            double radians = Math.Atan2(opposite, adjacent); // Get Radians for Atan2
            double angle = radians * (180 / Math.PI); // Change back to degrees
            return angle;
        }

        /// <summary>
        /// Verifica y/o Crea el directorio donde quedan los informes del usuario
        /// </summary>
        /// <param name="UserReportPath"></param>
        public static void validateDirectory(string UserReportPath)
        {
            DirectoryInfo DIR = new DirectoryInfo(UserReportPath);
            if (!DIR.Exists)
            {
                DIR.Create();//si no existe, se crea.
            }
        }

        /// <summary>
        /// Cuenta los riesgos que se solicitaron para impresión.
        /// </summary>
        /// <param name="dsPolicyRisks">DataSet con el total de riesgos</param>
        /// <param name="firstRisk">Número del primer riesgo</param>
        /// <param name="lastRisk">Número del último riesgo</param>
        /// <returns></returns>
        public static int[] getCountPrintableRisks(DataSet dsPolicyRisks, int firstRisk, int lastRisk)
        {
            int[] printables = new int[(lastRisk - firstRisk) + 1];
            int riskNum = 0;
            int printableRisk = 0;
            for (int risk = 0; risk < dsPolicyRisks.Tables[0].Rows.Count; risk++)
            {
                riskNum = Convert.ToInt32(dsPolicyRisks.Tables[0].Rows[risk][0]);
                if ((firstRisk <= riskNum && lastRisk >= riskNum))
                {
                    printables[printableRisk] = Convert.ToInt32(dsPolicyRisks.Tables[0].Rows[risk][0]);
                    printableRisk++;
                }
            }
            return printables;
        }

        /// <summary>
        /// Redimensiona un arreglo de enteros
        /// </summary>
        /// <param name="array">Arreglo origen</param>
        /// <returns></returns>
        public static int[] redimInt(int[] array)
        {
            int count = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != null)
                {
                    count++;
                }
            }
            if (array == null) return new int[count];
            int[] tmp = new int[count];
            array.CopyTo(tmp, 0);
            return tmp;
        }

        /// <summary>
        /// Redimensiona un arreglo de strings
        /// </summary>
        /// <param name="array">Arreglo origen</param>
        /// <returns></returns>
        public static string[] redimString(string[] array)
        {
            int count = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != null)
                {
                    count++;
                }
            }
            if (array == null) return new string[count];
            string[] tmp = new string[count];
            int index = 0;
            foreach (string str in array)
            {
                if (str != null)
                {
                    tmp[index] = str;
                    index++;
                }
            }
            return tmp;
        }

        /// <summary>
        /// Adiciona un arreglo a otro
        /// </summary>
        /// <param name="array1">Primer arreglo</param>
        /// <param name="array2">Segundo arreglo</param>
        /// <returns>Suma de los arreglos</returns>
        public static string[] addArray(string[] array1, string[] array2)
        {
            int count = array1.Length + array2.Length;
            string[] newArray = new string[count];

            array1.CopyTo(newArray, 0);
            array2.CopyTo(newArray, array1.Length);

            return newArray;
        }

        /// <summary>
        /// Agrega una posición a un arreglo de string
        /// </summary>
        /// <param name="array">Arreglo principal</param>
        /// <param name="addedString">Cadena a agregar</param>
        /// <returns>Nuevo arreglo</returns>
        public static string[] addString(string[] array, string addedString)
        {
            ArrayList arrayList = new ArrayList();

            foreach (string str in array)
            {
                arrayList.Add(str);
            }

            arrayList.Add(addedString);

            string[] newArray = new string[arrayList.Count];

            for (int i = 0; i < arrayList.Count; i++)
            {
                newArray[i] = arrayList[i].ToString();
            }

            return newArray;
        }

        /// <summary>
        /// Evalua una cadena dada e indica si es o no un valor númerico.
        /// </summary>
        /// <param name="strNumber">Número en formato string</param>
        /// <returns>Verdadero si es número, Falso de lo contrario.</returns>
        public static bool isNumeric(string strNumber)
        {
            char[] ca = strNumber.ToCharArray();
            for (int i = 0; i < ca.Length; i++)
            {
                if (ca[i] > 57 || ca[i] < 48)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Genera el reporte a partir de el archivo RPT correspondiente.
        /// </summary>
        /// <param name="strReportParameters">
        /// strReportParameters[0] - 
        /// strReportParameters[1] - 
        /// </param>
        /// <param name="strDataConn">
        /// strDataConn[0] - 
        /// strDataConn[1] - 
        /// </param>
        /// <param name="strPaths">
        /// strPaths[0] - Ruta del reporte a cargar
        /// strPaths[1] - Ruta del pdf generado
        /// </param>
        /// <param name="WaterMark">
        /// Marca de agua.
        /// </param>
        public static void loadReportFile(string[] strReportParameters, string[] strDataConn, string[] strPaths, string WaterMark, ExportFormatType formatToExport)
        {
            ReportDocument document = new ReportDocument();
            string sampleReportPath;

            try
            {
                int index = 0;

                sampleReportPath = strPaths[0];
                document.Load(sampleReportPath);
                ReportServiceHelper.setConnections(document.DataSourceConnections, strDataConn);

                /*
                 * Setea datos de conexión de cada subreporte
                 * Verifica la conectividad a la BD de cada subreporte
                 */

                foreach (ReportDocument subRpt in document.Subreports)
                {
                    ReportServiceHelper.setConnections(document.Subreports[subRpt.Name].DataSourceConnections, strDataConn);
                    document.Subreports[subRpt.Name].VerifyDatabase();
                }

                document.VerifyDatabase();

                /*
                 * Setea parametros del reporte principal
                 */

                foreach (ParameterField param in document.ParameterFields)
                {
                    if (param.ReportName == document.Name)
                    {
                        document.SetParameterValue(param.Name, strReportParameters[index]);
                        index++;
                    }
                }

                strPaths = ReportServiceHelper.redimString(strPaths);// Borra las pocisiones en blanco

                /*
                 * Guarda reportes apartir del primero. 
                 * No toma en cuenta la ruta del Archivo rpt.
                 */
                for (index = 1; index < strPaths.Length; index++)
                {
                    document.ExportToDisk(formatToExport, strPaths[index]);
                    if ((WaterMark != null) && (WaterMark.Length > 0))
                    {
                        byte[] fileBytes = setWaterMark(strPaths[index], WaterMark);
                        FileStream fileStream = new FileStream(strPaths[index], FileMode.Create);
                        fileStream.Write(fileBytes, 0, fileBytes.Length);
                        fileStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (document != null)
                {
                    document.Close();
                    document.Dispose();
                }
            }
        }

        /// <summary>
        /// Crea el código de barras a partir del número de póliza y IdPv dados.
        /// </summary>
        /// <param name="strPolicyNumber">Número de la póliza</param>
        /// <param name="strIdPv">Id PV</param>
        /// <param name="strBeneficiaryDocument">Documento del Beneficiario</param>
        /// <returns>Código de barras</returns>
        public static string[] getBarCode128(string[] rptParams, int idRef)
        {
            string strBarcode = string.Empty;
            string strBarcodeA = string.Empty;
            string[] resultBarCode = new string[2];
            string strBeneficiaryDocument = "";
            int processid = Convert.ToInt32(rptParams[0].ToString());
            int risknum = Convert.ToInt32(rptParams[4].ToString());
            DataSet dataBarCode;
            dataBarCode = loadDataBarCode(processid, risknum);
            strBeneficiaryDocument = dataBarCode.Tables[0].Rows[0]["POLICY_HOLDER_DOC"].ToString();
            strBarcode = "(";
            strBarcode += ((int)ReportEnum.BarCode.CONSTANT).ToString();
            strBarcode += ")";
            strBarcode += ((int)ReportEnum.BarCode.COUNTRY_CD).ToString();
            strBarcode += ((int)ReportEnum.BarCode.COMPANY_CD).ToString();
            strBarcode += ((int)ReportEnum.BarCode.SERVICE_CD).ToString().PadLeft(3, '0');
            strBarcode += ((int)ReportEnum.BarCode.CONVENTION).ToString();
            strBarcode += "(";
            strBarcode += ((int)ReportEnum.BarCode.FIELD_ID).ToString();
            strBarcode += ")";
            if ((idRef.ToString().Length % 2) == 1)
            {
                strBarcode += ((int)ReportEnum.BarCode.PARITY).ToString() + idRef.ToString();
            }
            else
            {
                strBarcode += idRef.ToString();
            }
            strBarcode += "(";
            strBarcode += ((int)ReportEnum.BarCode.FIELD_ID).ToString();
            strBarcode += ")";
            //07/07/2011 GC
            //<<Problemas Codigo Barras cuando trae letras en la cedula Modificacion para quitar caracteres no numericos
            strBarcodeA = strBarcode;
            strBarcode += strBeneficiaryDocument.ToString().PadLeft(10, '0');
            resultBarCode[0] = strBarcode;
            strBarcode = strBarcodeA + CleanInput(strBeneficiaryDocument).PadLeft(10, '0');
            strBarcode = strBarcode.Replace("(", "");
            strBarcode = strBarcode.Replace(")", "");
            resultBarCode[1] = uccEan128(strBarcode);

            return resultBarCode;
        }

        /// <summary>
        /// Da un estimado el tamaño del archivo a generar.
        /// </summary>
        /// <param name="range">Cantidad de riesgos o pólizas a imprimir</param>
        /// <returns>Tamaño apróximado del archivo a generar</returns>
        public static decimal calculateFileSize(int range)
        {
            decimal averageRiskFileSize = Convert.ToDecimal(ConfigurationSettings.AppSettings["AverageRiskFileSize"]);
            return range * averageRiskFileSize;
        }

        /// <summary>
        /// Cuenta los riesgos existentes para determinar el tipo de poliza.
        /// </summary>
        /// <param name="policyData">Datos de la póliza</param>
        /// <returns>Tabla de riesgos</returns>
        public static DataSet getRiskCount(Policy policyData)
        {
            DataSet dsPolicyRisks = new DataSet();
            NameValue[] strParameters = new NameValue[3];
            strParameters[0] = new NameValue("POLICY_ID", Convert.ToInt32(policyData.PolicyId));
            strParameters[1] = new NameValue("ENDORSEMENT_ID", Convert.ToInt32(policyData.EndorsementId));
            strParameters[2] = new NameValue("TEMP_ID", Convert.ToInt32(policyData.TempNum));
            dsPolicyRisks = getData("REPORT.CO_GET_RISK_COUNT", strParameters);
            return dsPolicyRisks;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="risknum"></param>
        /// <returns></returns>
        public static DataSet loadDataBarCode(int processId, int risknum)
        {
            NameValue[] spParam = new NameValue[2];
            spParam[0] = new NameValue("PROCESS_ID", processId);
            spParam[1] = new NameValue("RISK_NUM", risknum);

            //Actualiza el estado del reporte y sus datos adicionales
            DataSet dataReportFC = ReportServiceHelper.getData("REPORT.CO_FORMAT_COLLECT_BARCODE", spParam);

            return dataReportFC;
        }

        /// <summary>
        /// Función que convierte cadena numérica en formato EAN 128
        /// para representar en código de barras
        /// </summary>
        /// <param name="strToEncode">Recibe cadena numérica con datos de endoso y número
        /// id usuario</param>
        /// <returns>Cadena convertida en formato EAN 128</returns>
        public static string uccEan128(string strToEncode)
        {
            string charSet = "";
            int i = 0;
            string charToEncode = "";
            string codeBarstr = "";
            strToEncode = asciiToChar(strToEncode);
            strToEncode = strToEncode.ToUpper();

            if (strToEncode.Substring(0, 1) != (Convert.ToChar(247)).ToString())
            {
                strToEncode = (Convert.ToChar(247)).ToString() + strToEncode;
            }

            charSet = strToEncode.Substring(0, 1);
            for (i = 1; i < strToEncode.Length; i++)
            {
                charToEncode = strToEncode.Substring(i, 1);
                if ((Convert.ToInt32(charToEncode[0]) >= 48 && Convert.ToInt32(charToEncode[0]) <= 57) ||
                    (Convert.ToInt32(charToEncode[0]) >= 65 && Convert.ToInt32(charToEncode[0]) <= 90) ||
                    (charToEncode == (Convert.ToChar(247)).ToString()))
                {
                    charSet = charSet + charToEncode;
                }
            }
            codeBarstr = code128Auto(charSet);
            return code128Auto(charSet);
        }

        /// <summary>
        /// Función que convierte la cadena numérica a ascii
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns>Cadena caracteres ascii</returns>
        public static string asciiToChar(string strInput)
        {
            string tempAscii2Char = "";
            int i = 0;
            string strTemp = "";
            int nPos = 0;
            int nValue = 0;

            i = 1;
            nPos = (strInput.ToUpper().IndexOf(("&#").ToUpper(), i - 1) + 1);
            while (nPos > 0)
            {
                tempAscii2Char = tempAscii2Char + strInput.Substring(0, nPos - 1);

                strInput = strInput.Substring(strInput.Length - (strInput.Length - nPos + 1));
                i = 3;
                strTemp = "";
                while ((i <= strInput.Length) && (strTemp.Length < 3))
                {
                    strTemp = strTemp + strInput.Substring(i - 1, 1);
                    i = i + 1;
                }
                nValue = 0;
                if (strTemp != "")
                {
                    nValue = Convert.ToInt32(strTemp);
                }
                if (nValue >= 0 && nValue < 128)
                {
                    tempAscii2Char = tempAscii2Char + (Convert.ToChar(nValue)).ToString();
                }
                else if (nValue > 127 & nValue < 256)
                {
                    tempAscii2Char = tempAscii2Char + (Convert.ToChar(nValue)).ToString();
                }
                else
                {
                    tempAscii2Char = tempAscii2Char + strInput.Substring(0, i - 1);
                }
                if (i <= strInput.Length && strInput.Substring(i - 1, 1) == ";")
                {
                    i = i + 1;
                }
                strInput = strInput.Substring(strInput.Length - (strInput.Length - i + 1));
                nPos = (strInput.ToUpper().IndexOf(("&#").ToUpper(), 0) + 1);
            }
            if (strInput.Length > 0)
            {
                tempAscii2Char = tempAscii2Char + strInput;
            }
            return tempAscii2Char;
        }

        /// <summary>
        /// Genera cadena de caracteres válidos para conversión
        /// </summary>
        /// <returns>Cadena de caracteres código 128</returns>
        public static string Code128cCharset()
        {
            string functionReturnValue = null;
            int i = 0;
            for (i = 0; i <= 9; i++)
            {
                functionReturnValue = functionReturnValue + Convert.ToChar(i + (int)(Convert.ToChar(48)));
            }
            for (i = 245; i <= 247; i++)
            {
                functionReturnValue = functionReturnValue + Convert.ToChar(i);
            }
            return functionReturnValue;
        }

        /// <summary>
        /// Convierte cadena de entrada en formato 128 para generar código de barras
        /// </summary>
        /// <param name="strToEncode"></param>
        /// <returns>Cadena codificada 128</returns>
        public static string code128Auto(string strToEncode)
        {
            int i = 0;
            string charToEncode = null;
            int charPos = 0;
            int checkSum = 0;
            string checkDigit = null;
            string AcharSet = null;
            string BcharSet = null;
            string CcharSet = null;
            string mappingSet = null;
            string curCharSet = null;
            string dataCode128 = "";
            int iFunction1 = 0;
            int strLen = 0;
            int charVal = 0;
            int weight = 0;

            if (strToEncode == string.Empty)
            {
                return charToEncode;
            }

            CcharSet = Code128cCharset();
            mappingSet = code128MappingSet();
            strToEncode = asciiToChar(strToEncode);
            strLen = strToEncode.Length;
            ///
            iFunction1 = strLen - 15;
            ///
            charVal = (int)(Convert.ToChar(strToEncode.Substring(0, 1)));
            if (charVal <= 31) curCharSet = AcharSet;
            if (charVal >= 32 & charVal <= 126) curCharSet = BcharSet;
            if (charVal == 242) curCharSet = BcharSet;
            if (charVal == 247) curCharSet = CcharSet;
            if (strLen > 4) curCharSet = CcharSet;

            dataCode128 = dataCode128 + Convert.ToChar(250);

            for (i = 0; i < strLen; i++)
            {
                charToEncode = strToEncode.Substring(i, 1);
                charVal = (int)(Convert.ToChar(charToEncode));

                if ((charVal == 242))
                {
                    if (curCharSet == CcharSet)
                    {
                        dataCode128 = dataCode128 + Convert.ToChar(249);
                        curCharSet = BcharSet;
                    }
                    dataCode128 = dataCode128 + Convert.ToChar(242);
                    i = i + 1;
                    charToEncode = strToEncode.Substring(i, 1);
                    charVal = (int)(Convert.ToChar(charToEncode));
                }

                if ((charVal == 247))
                {
                    dataCode128 = dataCode128 + Convert.ToChar(247);
                }
                else if ((i < strLen - 2) || ((i < strLen) && (curCharSet == CcharSet)))
                {
                    if (curCharSet != CcharSet)
                    {
                        dataCode128 = dataCode128 + Convert.ToChar(244);
                        curCharSet = CcharSet;
                    }
                    charToEncode = strToEncode.Substring(i, 2);
                    charVal = Convert.ToInt32(charToEncode);
                    dataCode128 = dataCode128 + mappingSet.Substring(charVal, 1);
                    ///Estas instrucciones se utilizan para incluir
                    ///el caracter de función igual al generado al inicio de la trama
                    ///que permite la lectura correcta del código de barras
                    if (i == iFunction1 - 1)
                    {
                        dataCode128 = dataCode128 + Convert.ToChar(247);
                    }
                    ///****************************************************************
                    i = i + 1;
                }
                else if ((((i <= strLen) & (charVal < 31)) | ((curCharSet == AcharSet) & (charVal > 32 & charVal < 96))))
                {
                    if (curCharSet != AcharSet)
                    {
                        dataCode128 = dataCode128 + Convert.ToChar(246);
                        curCharSet = AcharSet;
                    }
                    charPos = curCharSet.IndexOf(charToEncode);
                    dataCode128 = dataCode128 + mappingSet.Substring(charPos, 1);
                }
                else if ((i <= strLen) & (charVal > 31 & charVal < 127))
                {
                    if (curCharSet != BcharSet)
                    {
                        dataCode128 = dataCode128 + Convert.ToChar(245);
                        curCharSet = BcharSet;
                    }
                    charPos = curCharSet.IndexOf(charToEncode);
                    dataCode128 = dataCode128 + mappingSet.Substring(charPos, 1);
                }
            }

            strLen = dataCode128.Length;
            for (i = 0; i < strLen; i++)
            {
                charVal = (int)(Convert.ToChar(dataCode128.Substring(i, 1)));
                if (charVal == 252)
                {
                    charVal = 0;
                }
                else if (charVal <= 126)
                {
                    charVal = charVal - 32;
                }
                else if (charVal >= 240)
                {
                    charVal = charVal - 145;
                }
                if (i > 1)
                {
                    weight = i;
                }
                else
                {
                    weight = 1;
                }
                checkSum = checkSum + charVal * weight;
            }
            checkSum = checkSum % 103;
            checkDigit = mappingSet.Substring(checkSum, 1);
            dataCode128 = dataCode128 + checkDigit + Convert.ToChar(251);
            return dataCode128;
        }

        /// <summary>
        /// Cadena de caracteres 128
        /// </summary>
        /// <returns></returns>
        public static string code128MappingSet()
        {
            string tempcode128MappingSet = null;
            int i = 0;
            tempcode128MappingSet = Convert.ToChar(252).ToString();
            for (i = 33; i <= 126; i++)
            {
                tempcode128MappingSet = tempcode128MappingSet + ((char)(i)).ToString();
            }
            for (i = 240; i <= 251; i++)
            {
                tempcode128MappingSet = tempcode128MappingSet + ((char)(i)).ToString();
            }
            return tempcode128MappingSet;
        }

        /// <summary>
        /// Obtiene información de la carátula de la póliza colectiva.
        /// </summary>
        /// <param name="policyData">Datos de la póliza</param>
        /// <returns>Tabla de datos carátula colectiva</returns>
        public static DataSet getDataCollective(Policy policyData)
        {
            DataSet dsPolicyData = new DataSet();
            NameValue[] strParameters = new NameValue[1];
            strParameters[0] = new NameValue("PROCESS_ID", policyData.ProcessId);
            dsPolicyData = getData("REPORT.CO_DATA_POLICY_COLLECTIVE", strParameters);
            return dsPolicyData;
        }

        /// <summary>
        /// Obtiene información de la carátula de la póliza colectiva.
        /// </summary>
        /// <param name="policyData">Datos de la póliza</param>
        /// <returns>Tabla de datos carátula colectiva</returns>
        public static DataSet getDataOperationAutho(int operationId)
        {
            DataSet dsPolicyData = new DataSet();
            NameValue[] strParameters = new NameValue[1];
            strParameters[0] = new NameValue("@OPERATION_ID", operationId);
            dsPolicyData = getData("REPORT.GET_DATA_OPERATION_AUTHO", strParameters);
            return dsPolicyData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsId"></param>
        /// <returns></returns>
        public static int findIdpv2g(int policyId, int endorsId)
        {
            DataSet ds;
            NameValue[] parametersp;
            DynamicDataAccess pdb;

            parametersp = new NameValue[2];

            //Parametros  de Envio  al  SP  
            parametersp[0] = new NameValue("POLICY_ID", policyId);
            parametersp[1] = new NameValue("ENDORSEMENT_ID", endorsId);

            pdb = new DynamicDataAccess(ConfigurationSettings.AppSettings["Sise2GDB"]);
            ds = pdb.ExecuteSPDataSet("CO_GET_ID_PV_2G", parametersp);
            pdb.Dispose();

            pdb = null;

            //if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            return Convert.ToInt32(ds.Tables[0].Rows[0].ToString());

        }
        public static String CleanInput(string strIn)
        {
            // Remplaza caracteres invalidos en el string
            return Regex.Replace(strIn, @"[^0-9]", "");
        }

        public static void PrintCounterGuarantees()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public static void LoadReportFileCounterGuarantees(string[] strReportParameters, string[] strDataConn, string[] strPaths, string WaterMark, ExportFormatType formatToExport)
        //{
        //    ReportDocument document = new ReportDocument();
        //    string sampleReportPath;

        //    try
        //    {
        //        int index = 0;

        //        sampleReportPath = strPaths[0];
        //        document.Load(sampleReportPath);
        //        ReportServiceHelper.setConnections(document.DataSourceConnections, strDataConn);

        //        /*
        //         * Setea datos de conexión de cada subreporte
        //         * Verifica la conectividad a la BD de cada subreporte
        //         */

        //        foreach (ReportDocument subRpt in document.Subreports)
        //        {
        //            ReportServiceHelper.setConnections(document.Subreports[subRpt.Name].DataSourceConnections, strDataConn);
        //            document.Subreports[subRpt.Name].VerifyDatabase();
        //        }

        //        document.VerifyDatabase();

        //        /*
        //         * Setea parametros del reporte principal
        //         */

        //        foreach (ParameterField param in document.ParameterFields)
        //        {
        //            if (param.ReportName == document.Name)
        //            {
        //                document.SetParameterValue(param.Name, strReportParameters[index]);
        //                index++;
        //            }
        //        }

        //        strPaths = ReportServiceHelper.redimString(strPaths);// Borra las pocisiones en blanco

        //        /*
        //         * Guarda reportes apartir del primero. 
        //         * No toma en cuenta la ruta del Archivo rpt.
        //         */
        //        for (index = 1; index < strPaths.Length; index++)
        //        {
        //            document.ExportToDisk(formatToExport, strPaths[index]);
        //            if ((WaterMark != null) && (WaterMark.Length > 0))
        //            {
        //                byte[] fileBytes = setWaterMark(strPaths[index], WaterMark);
        //                FileStream fileStream = new FileStream(strPaths[index], FileMode.Create);
        //                fileStream.Write(fileBytes, 0, fileBytes.Length);
        //                fileStream.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (document != null)
        //        {
        //            document.Close();
        //            document.Dispose();
        //        }
        //    }
        //}
    }
}
