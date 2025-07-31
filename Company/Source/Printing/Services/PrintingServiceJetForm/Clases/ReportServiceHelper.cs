using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.PrintingServices.Clases;
using Sistran.Company.Application.PrintingServices.Enums;
using Sistran.Company.Application.PrintingServices.Models;

namespace Sistran.Company.PrintingService.JetForm.Clases
{
    public class ReportServiceHelper
    {
        /// <summary>
        /// Constante que define el número de caracteres por línea en un archivo de reporte
        /// </summary>
        //TODO: Autor: John Ruiz; Fecha 21/10/2010; Asunto: se cambia la cantidad de caracteres que caben por linea; Compañia 2;
        const int CHARS_FOR_RPT_LINE = 102;

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

        public static IRisk selectCoveredRiskTypeToPrint(int coverRiskTypeCd, int prefixCd)
        {
            try
            {
                switch (coverRiskTypeCd)
                {
                    case ((int)RiskType.AUTO):
                        if (prefixCd == (int)PrefixCode.RCV)
                            return new RiskRCPassengers();
                        else
                            return new RiskAuto();
                    case ((int)RiskType.UBICACION):
                        return new RiskLocation();
                    case ((int)RiskType.FIANZA):
                        return new RiskSurety();
                    case ((int)RiskType.TRANSPORTE):
                        return new RiskTransport();
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                return pdb.ExecuteSPSerDataSet(storedProcedure, parametros);
            }
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
        /// Crea el código de barras a partir del número de póliza y IdPv dados.
        /// </summary>
        /// <param name="strPolicyNumber">Número de la póliza</param>
        /// <param name="strIdPv">Id PV</param>
        /// <param name="strBeneficiaryDocument">Documento del Beneficiario</param>
        /// <returns>Código de barras</returns>
        public static string[] getBarCode128(string[] rptParams, int IdPv)
        {
            string strBarcode = string.Empty;
            string[] resultBarCode = new string[2];
            //string strVerificationChar = "\\" + ((int)BarCode.VERIFICATION_CHR).ToString();
            //string strVerificationChar = ((int)BarCode.VERIFICATION_CHR).ToString();
            string strBeneficiaryDocument = "";
            int processid = Convert.ToInt32(rptParams[0].ToString());
            int risknum = Convert.ToInt32(rptParams[4].ToString());
            DataSet dataBarCode;
            dataBarCode = loadDataBarCode128(processid, risknum);
            strBeneficiaryDocument = dataBarCode.Tables[0].Rows[0]["POLICY_HOLDER_DOC"].ToString();
            //strBarcode = Convert.ToChar(134).ToString();
            strBarcode += ((int)BarCode.CONSTANT).ToString();
            strBarcode += ((int)BarCode.COUNTRY_CD).ToString();
            strBarcode += ((int)BarCode.COMPANY_CD).ToString();
            strBarcode += ((int)BarCode.SERVICE_CD).ToString().PadLeft(3, '0');
            strBarcode += ((int)BarCode.CONVENTION).ToString();
            strBarcode += ((int)BarCode.FIELD_ID).ToString();

            if ((IdPv.ToString().Length % 2) == 1)
            {
                strBarcode += ((int)BarCode.PARITY).ToString() + IdPv;
            }
            else
            {
                strBarcode += IdPv;
            }

            //strBarcode += strVerificationChar;
            strBarcode += ((int)BarCode.FIELD_ID).ToString();
            //strBarcode += Convert.ToChar(134).ToString();
            if ((strBeneficiaryDocument.Length % 2) == 1)
            {
                strBarcode += ((int)BarCode.PARITY).ToString() + strBeneficiaryDocument;
            }
            else
            {
                strBarcode += strBeneficiaryDocument;
            }

            //resultBarCode[0] = strBarcode.Replace("\\1","");
            resultBarCode[0] = strBarcode;
            resultBarCode[1] = buildBarCode128(strBarcode);

            return resultBarCode;
        }

        /// <summary>
        /// Da un estimado el tamaño del archivo a generar.
        /// </summary>
        /// <param name="range">Cantidad de riesgos o pólizas a imprimir</param>
        /// <returns>Tamaño apróximado del archivo a generar</returns>
        public static decimal calculateFileSize(int range)
        {
            decimal averageRiskFileSize = 190;
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
        /// Arma la trama con encabezado y final para enviar al reporte e imprimir el código de barras
        /// </summary>
        /// <param name="stringToCode"></param>
        /// <param name="tableB"></param>
        /// <returns></returns>
        public static string buildBarCode128(string stringToCode)
        {
            int i, j;
            Int32 lnString = stringToCode.Length;
            string codeBar128 = "";
            //string[] cb128aux = 
            Int32 valueMin = 0;
            int dummy = 0;
            int checksum = 0;
            bool tableB = true;
            // Create an ASCII encoding.
            Int32 charAscii = 0;
            if (lnString == 0)
            {
                return codeBar128;
            }

            for (i = 0; i < lnString; i++)
            {
                charAscii = (int)(Convert.ToChar(stringToCode.Substring(i, 1).ToString()));
                if ((charAscii < 32) || (charAscii > 126))
                //if ((charAscii != 203) && (charAscii <= 32) && (charAscii >= 126))
                {
                    i = 0;
                    return codeBar128;
                }
            }
            if (i > 0)
            {
                i = 0;
                while (i < lnString)
                {
                    if (tableB)
                    {

                        if ((i == 0) || ((i + 3) == (lnString - 1)))
                        {
                            valueMin = 4;
                        }
                        else
                        {
                            valueMin = 6;
                        }
                        valueMin = valueMin - 1;
                        if ((i + valueMin) <= lnString)
                        {
                            while (valueMin >= 0)
                            {
                                j = i + valueMin;
                                if (((int)(Convert.ToChar(stringToCode.Substring(j, 1))) < 48) || ((int)(Convert.ToChar(stringToCode.Substring(j, 1))) > 57))
                                {
                                    return codeBar128;
                                }
                                else
                                {
                                    valueMin = valueMin - 1;
                                }
                            }
                        }
                        if (valueMin < 0)
                        {
                            if (i == 0)
                            {
                                codeBar128 = Convert.ToChar(210).ToString();
                            }
                            else
                            {
                                codeBar128 = codeBar128 + Convert.ToChar(204).ToString();
                            }
                            tableB = false;
                        }
                        else
                        {
                            if (i == 0)
                            {
                                codeBar128 = Convert.ToChar(209).ToString();
                            }
                        }
                    }
                    if (!tableB)
                    {
                        valueMin = 2;
                        valueMin = valueMin - 1;
                        if ((i + valueMin) < lnString)
                        {
                            while (valueMin >= 0)
                            {
                                j = i + valueMin;
                                if (((int)(Convert.ToChar(stringToCode.Substring(j, 1))) < 48) || ((int)(Convert.ToChar(stringToCode.Substring(j, 1))) > 57))
                                {
                                    return codeBar128;
                                }
                                else
                                {
                                    valueMin = valueMin - 1;
                                }
                            }
                        }
                        if (valueMin < 0)
                        {
                            //dummy = (int)(Convert.ToChar(stringToCode.Substring(i,2).ToString()));
                            dummy = Convert.ToInt32(stringToCode.Substring(i, 2).ToString());

                            if (dummy < 95)
                            {
                                dummy = dummy + 32;
                            }
                            else
                            {
                                dummy = dummy + 105;
                            }
                            codeBar128 = codeBar128 + Convert.ToChar(dummy).ToString();
                            i = i + 2;
                        }
                        else
                        {
                            codeBar128 = codeBar128 + Convert.ToChar(205).ToString();
                            tableB = true;
                        }
                    }
                    if (tableB)
                    {
                        codeBar128 = codeBar128 + Convert.ToChar(stringToCode.Substring(i, 1).ToString());
                        i = i + 1;
                    }

                }
                for (i = 0; i < codeBar128.Length; i++)
                {
                    dummy = (int)(Convert.ToChar(codeBar128.Substring(i, 1).ToString()));
                    if (dummy < 127)
                    {
                        dummy = dummy - 32;
                    }
                    else
                    {
                        dummy = dummy - 105;
                    }
                    if (i == 0)
                    {
                        checksum = dummy;
                    }
                    checksum = (checksum + i * dummy) % 103;
                }

                if (checksum < 95)
                {
                    checksum = checksum + 32;
                }
                else
                {
                    checksum = checksum + 105;
                }
                codeBar128 = codeBar128 + Convert.ToChar(checksum) + Convert.ToChar(211);
            }

            return codeBar128;

        }

        public static DataSet loadDataBarCode128(int processId, int risknum)
        {
            NameValue[] spParam = new NameValue[2];
            spParam[0] = new NameValue("PROCESS_ID", processId);
            spParam[1] = new NameValue("RISK_NUM", risknum);

            //Actualiza el estado del reporte y sus datos adicionales
            DataSet dataReportFC = ReportServiceHelper.getData("REPORT.CO_FORMAT_COLLECT_BARCODE", spParam);

            return dataReportFC;
        }

        /// <summary>
        /// Escribe los Items de la lista en forma de archivo .dat, en la ruta dada, con el nombre de archivo especificado.
        /// </summary>
        /// <param name="fileName">Nombre del archivo a escribir</param>
        /// <param name="localPath">Ruta local donde debe quedar el archivo</param>
        /// <param name="fileLines">Lineas del archivo a escribir</param>
        /// <returns>Ruta completa donde quedó el archivo</returns>
        public static string WriteFile(string fileName, string localPath, ArrayList fileLines)
        {
            string strPath = localPath + fileName;
            verifyDirPath(Path.GetDirectoryName(strPath));

            System.IO.StreamWriter writer = new StreamWriter(strPath, false, System.Text.Encoding.Default);

            foreach (DatRecord obj in fileLines)
            {
                if (obj.FieldName != string.Empty && obj.FieldName != null)
                {
                    writer.WriteLine(obj.FieldName);
                }

                if (obj.FieldValue != string.Empty && obj.FieldValue != null)
                {
                    writer.WriteLine(obj.FieldValue);
                }
            }
            writer.Close();

            return strPath;
        }

        /// <summary>
        /// Verifica existencia directorio dado. Sino existe lo crea.
        /// </summary>
        /// <param name="path"></param>
        public static void verifyDirPath(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            if (!dirInfo.Exists)
                dirInfo.Create();
        }

        /// <summary>
        /// Completa con '$***' los valores de moneda para la correcta tabulación
        /// </summary>
        /// <param name="value">Valor a completar</param>
        /// <param name="len">Longitud a completar</param>
        /// <param name="currencySymbol">Simbolo de la moneda</param>
        /// <returns>Valor completo</returns>
        public static string completeCurrency(string value, int len, string currencySymbol, string spaceChar)
        {
            for (int i = currencySymbol.Length; i <= (len - value.Trim().Length); i++)
            {
                currencySymbol += spaceChar;
            }
            return String.Format(currencySymbol + "{0:#,##0.00}", Convert.ToDecimal(value));
        }

        /// <summary>
        /// Completa con espación la columna
        /// </summary>
        /// <param name="value">Valor a completar</param>
        /// <param name="len">Longitud a completar</param>
        /// <returns>Valor completo</returns>
        public static string completeColumn(string value, int len)
        {
            string column = string.Empty;

            for (int i = 0; i <= (len - value.Length); i++)
            {
                column += " ";
            }

            return column += value;
        }

        public static string completeColumnCoverages(string value, int len)
        {
            string column = string.Empty;

            if (value.Length > len)
            {

                value = value.Remove(len);
                return value;
            }

            for (int i = 0; i <= (len - value.Length); i++)
            {
                column += " ";
            }

            return value + column;
        }

        /// <summary>
        /// Devuelve un string con un numero formateado en 
        /// formato de moneda
        /// </summary>        
        /// <param name="value">nuemro a formatear</param>
        /// <param name="value">CultureInfo correspondiente 
        /// al string que se envia como primer parametro</param>
        /// <returns>numero con el formato de moneda correcto</returns>
        public static string formatMoney(string value, CultureInfo culture)
        {
            try
            {
                return string.Format("{0:#,##0.00}", Convert.ToDecimal(value, culture));
            }
            catch (Exception)
            {
                return value;
            }
        }

        /// <summary>
        /// Devuelve un string con un numero formateado en
        /// forma porcentual
        /// </summary>        
        /// <param name="value">nuemro a formatear</param>
        /// <returns>numero con el formato porcentual correcto</returns>
        public static string formatPercentage(string value)
        {
            try
            {
                return string.Format("{0:#,##0.00%}", Convert.ToDecimal(value) / 100);
            }
            catch (Exception)
            {
                return value;
            }
        }

        /// <summary>
        /// Centra el texto de la cadena dada según la longitud deseada
        /// </summary>
        /// <param name="value">Valor que se desea centrar</param>
        /// <param name="len">Longitud de la cadena</param>
        /// <returns>Cadena completa con espacios</returns>
        public static string centerAlign(string value, int len)
        {
            value = value.Trim().TrimStart().TrimEnd();

            if (value.Length > len)
            {
                return value.Remove(len);
            }
            else
            {
                int space = (len - value.Length) / 2;
                value = value.PadLeft(space + value.Length);
                value = value.PadRight(space + value.Length);
                return value;
            }
        }

        /// <summary>
        /// Alinea a la derecha el texto de la cadena dada según la longitud deseada
        /// </summary>
        /// <param name="value">Valor que se desea centrar</param>
        /// <param name="len">Longitud de la cadena</param>
        /// <returns>Cadena completa con espacios</returns>
        public static string rightAlign(string value, int len)
        {
            value = value.Trim().TrimStart().TrimEnd();

            if (value.Length > len)
            {
                return value.Remove(len);
            }
            else
            {
                return value.PadLeft(len);
            }
        }

        /// <summary>
        /// Alinea a la izquierda el texto de la cadena dada según 
        /// la longitud deseada
        /// </summary>
        /// <param name="value">Valor que se desea centrar</param>
        /// <param name="len">Longitud de la cadena</param>
        /// <returns>Cadena completa con espacios</returns>
        public static string leftAlign(string value, int len)
        {
            value = value.Trim().TrimStart().TrimEnd();

            if (value.Length > len)
            {
                return value.Remove(len);
            }
            else
            {
                return value.PadRight(len);
            }
        }

        /// <summary>
        /// Devuelve un string con el NIT correctamente Formateado        
        /// </summary>
        /// <param name="value">string numero de NIT sin formato</param>
        /// <returns>Número de NIT con el formato correcto</returns>
        public static string formatNIT(String nit)
        {
            if (nit.CompareTo(string.Empty) == 0)
                return string.Empty;

            if (!nit.Contains("-"))
            {
                string pattern = "{0}-{1}";//Co.Previsora.Application.Reports.Services.Provider.Resources.RptFields.LBL_TRYBUTARY_NUMBER_FORMAT;
                string checkDigit = string.Empty;
                string docNumber = nit;
                checkDigit = nit.Substring(nit.Length - 1);
                if ((!nit.Contains(".")) && (!nit.Contains(",")))
                {
                    int value;
                    if (int.TryParse(nit.Substring(0, nit.Length - 1), out value))
                        docNumber = string.Format(new CultureInfo("es-AR"), "{0:0,0}", Convert.ToInt64(value));
                }
                return string.Format(pattern, docNumber, checkDigit);
            }
            else
                return nit;

            return string.Empty;
        }

        public static string unicode_iso8859(string text)
        {
            System.Text.Encoding iso = System.Text.Encoding.GetEncoding("iso8859-1");
            text = Regex.Replace(text, @"[/']", " ", RegexOptions.None);
            byte[] isoByte = iso.GetBytes(text);
            return iso.GetString(isoByte);
        }

        /// <summary>
        /// Devuelve el número de líneas que contiene un string 
        /// para imprimir en un archivo de reportes
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>Número de líneas contenidas</returns>
        public static int countLinesFor(string value)
        {
            return value.Length / CHARS_FOR_RPT_LINE + 1;
        }

        public static decimal validateValue(string value, CultureInfo culture)
        {
            try
            {
                return Convert.ToDecimal(value, culture);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// Devuelve un string con un numero formateado en 
        /// formato de cédula
        /// </summary>        
        /// <param name="value">numero a formatear</param>
        /// <param name="value">CultureInfo correspondiente 
        /// al string que se envia como primer parametro</param>
        /// <returns>numero con el formato de cédula correcto</returns>
        //TODO: Autor: César Giraldo; Fecha 05/08/2013; Asunto: se crea procedimiento para formatear como cédula AE016;
        public static string formatCedula(string value, CultureInfo culture)
        {
            try
            {
                //return Int64.Parse(value).ToString();
                return string.Format("{0:#,###,###,##0}", Convert.ToInt64(value, culture));
            }
            catch (Exception)
            {
                return value;
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

        public static void MergeFiles(string outPutFilePath, string[] filesPath)
        {
            List<PdfReader> readerList = new List<PdfReader>();
            foreach (string filePath in filesPath)
            {
                PdfReader pdfReader = new PdfReader(filePath);
                readerList.Add(pdfReader);
            }

            //Define a new output document and its size, type
            Document document = new Document(PageSize.A4, 0, 0, 0, 0);
            //Create blank output pdf file and get the stream to write on it.
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outPutFilePath, FileMode.Create));
            document.Open();

            foreach (PdfReader reader in readerList)
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                    document.Add(iTextSharp.text.Image.GetInstance(page));
                }
            }
            document.Close();
        }

        public static DataSet GetTemporaryByTempId(int tempId)
        {
            NameValue[] paramsp = new NameValue[1];
            paramsp[0] = new NameValue("TEMP_ID", Convert.ToInt32(tempId));
            return ReportServiceHelper.getData("REPORT.GET_TEMPORARY", paramsp);
        }

        public static DataSet GetQuotationByQuotationId(int quotationId, int brachId, int prefixId)
        {
            NameValue[] paramsp = new NameValue[3];
            paramsp[0] = new NameValue("@QUOTATION_ID", Convert.ToInt32(quotationId));
            paramsp[1] = new NameValue("@BRANCH_CD", Convert.ToInt32(brachId));
            paramsp[2] = new NameValue("@PREFIX_CD", Convert.ToInt32(prefixId));
            return ReportServiceHelper.getData("REPORT.GET_QUOTATION", paramsp);
        }

        public static DataSet GetIDsforMassivePrinter(int paramReport, int cantidadReservar)
        {
            NameValue[] paramsp = new NameValue[2];
            paramsp[0] = new NameValue("@PARAM_ID", paramReport);
            paramsp[1] = new NameValue("@RESERVE_QUANTITY", cantidadReservar);
            return ReportServiceHelper.getData("REPORT.RESERVE_ID_FOR_PRINTING", paramsp);
        }

        public static DataSet GetRisksForPrintingByPolicyId(int policyNum, int branchId, int prefixId)
        {
            NameValue[] paramsp = new NameValue[3];
            paramsp[0] = new NameValue("@DOCUMENT_NUM", policyNum);
            paramsp[1] = new NameValue("@BRANCH_CD", branchId);
            paramsp[2] = new NameValue("@PREFIX_CD", prefixId);
            return ReportServiceHelper.getData("REPORT.CO_FIND_ENDORSEMENT", paramsp);
        }

        public static DataSet GetRisksByEndorsementId(int endorsementId)
        {
            NameValue[] paramsp = new NameValue[1];
            paramsp[0] = new NameValue("@ENDORSEMENT_ID", endorsementId);
            return ReportServiceHelper.getData("REPORT.CO_FIND_RISKS", paramsp);
        }

        public static DataSet GetTemporaryByOperationId(int operationId)
        {
            NameValue[] paramsp = new NameValue[1];
            paramsp[0] = new NameValue("@OPERATION_ID", operationId);
            return ReportServiceHelper.getData("REPORT.CIA_GET_TEMPORARY_FOR_PRINTING", paramsp);
        }

        public static DataSet GetPrintingByPrintingId(int printingId)
        {
            NameValue[] paramsp = new NameValue[1];
            paramsp[0] = new NameValue("@PRINTING_ID", printingId);
            return ReportServiceHelper.getData("REPORT.CIA_CO_FIND_PRINTING_PROCESS", paramsp);
        }

        public static DataSet GetPrintingLogByPrintingId(int printingId)
        {
            NameValue[] paramsp = new NameValue[1];
            paramsp[0] = new NameValue("@PRINTING_ID", printingId);
            return ReportServiceHelper.getData("REPORT.CIA_CO_FIND_PRINTING_PROCESS_LOG", paramsp);
        }

        public static DataSet SavePrinting(CompanyPrinting companyPrinting, int typeProcess)
        {
            NameValue[] paramsp = new NameValue[11];
            paramsp[0] = new NameValue("@PRINTING_ID", companyPrinting.Id);
            paramsp[1] = new NameValue("@PRINTING_TYPE_ID", companyPrinting.PrintingTypeId);
            paramsp[2] = new NameValue("@KEY_ID", companyPrinting.KeyId);
            paramsp[3] = new NameValue("@URL_FILE", companyPrinting.UrlFile);
            paramsp[4] = new NameValue("@TOTAL", companyPrinting.Total);
            paramsp[5] = new NameValue("@BEGIN_DATE", companyPrinting.BeginDate);
            paramsp[6] = new NameValue("@FINISH_DATE", companyPrinting.FinishDate);
            paramsp[7] = new NameValue("@USER_ID", companyPrinting.UserId);
            paramsp[8] = new NameValue("@HAS_ERROR", companyPrinting.HasError);
            paramsp[9] = new NameValue("@URL_FILE_ERROR", companyPrinting.UrlFileError);
            paramsp[10] = new NameValue("@OPERATIONCODE", typeProcess);
            return ReportServiceHelper.getData("REPORT.CIA_CO_UPDATE_PRINTING_PROCESS", paramsp);
        }

        public static DataSet SavePrintingLog(CompanyPrintingLog companyPrintingLog, int typeProcess)
        {
            NameValue[] paramsp = new NameValue[5];
            paramsp[0] = new NameValue("@ID", companyPrintingLog.Id);
            paramsp[1] = new NameValue("@PRINTING_ID", companyPrintingLog.PrintingId);
            paramsp[2] = new NameValue("@URL_FILE", companyPrintingLog.UrlFile);
            paramsp[3] = new NameValue("@STATUS_CD", companyPrintingLog.StatusCd);
            paramsp[4] = new NameValue("@OPERATIONCODE", typeProcess);
            return ReportServiceHelper.getData("REPORT.CIA_CO_UPDATE_PRINTING_LOG", paramsp);
        }
    }
}
