using SpreadsheetLight;

using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

//Sistran Core
using Sistran.Core.Application.ReportingServices.Models;
using Sistran.Core.Application.ReportingServices.Models.Formats;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Procedure = Sistran.Core.Framework.DAF.Engine.StoredProcedure;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Sistran.Co.Application.Data;
using Sistran.Core.Application.ReportingServices.Provider.Helpers;

namespace Sistran.Core.Application.ReportingServices.Provider.DAOs
{
    public class StructureDAO
    {
        #region Instance Variables

        #region Interfaz

        /// <summary>
        /// Declaración del contexto y del DataFacadeManager
        /// </summary>
        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Interfaz

        #region DAOs

        private readonly FormatDetailDAO _formatDetailDAO = new FormatDetailDAO();
        private readonly MassiveReportDAO _massiveReportDAO = new MassiveReportDAO();
        
        #endregion DAOs

        #endregion Instance Variables

        #region Public Methods

        #region Structure

        /// <summary>
        /// GenerateStructureReport
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public void GenerateStructureReport(Report report)
        {
            MassiveReport massiveReport = new MassiveReport();
            string sharedReport = ConfigurationManager.AppSettings["SharedReportFolder"].ToString();
            string sharedPortable = ConfigurationManager.AppSettings["SharedPortableDocumentFolder"].ToString();
            string virtualPath = ConfigurationManager.AppSettings["VirtualPath"].ToString();
            string exportFileName = "";
            string virtualFileName = "";

            int index = 0;
            int indexPageNumber = 0;
            double recordsCount = 0;
            int pageSize = 1;
            int pageSizeProcedure = 0;
            double rowsPage = 0;
            int massiveReportId = 0;
            double row = 0;
            double resultNewPage = -1;
            bool isAsync = report.IsAsync;

            try
            {
                // Se toman los parámetros y se calcula el paginado para la consulta
                foreach (Parameter parameter in report.StoredProcedure.ProcedureParameters)
                {
                    if (parameter.Description == "@MASSIVE_REPORT_ID")
                    {
                        massiveReportId = Convert.ToInt32(parameter.Value);
                    }
                    else if (parameter.Description == "@RECORD_COUNT")
                    {
                        recordsCount = Convert.ToDouble(parameter.Value);
                    }
                    else if (parameter.Description == "@PAGE_SIZE")
                    {
                        pageSizeProcedure = Convert.ToInt32(parameter.Value);
                    }
                    else if (parameter.Description == "@PAGE_NUMBER")
                    {
                        indexPageNumber = 1;//index;
                    }
                    index++;
                }

                pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSizeExcel"]);

                // Si la cantidad de registros es mayor al total por página
                if (recordsCount > pageSizeProcedure)
                {
                    rowsPage = recordsCount / pageSizeProcedure;
                    row = Math.Floor(rowsPage);
                    resultNewPage = rowsPage - row;
                }
                else
                {
                    row = 1;
                }

                if (report.IsAsync)
                {
                    // Se Actualiza 
                    massiveReport.Description = report.Description;
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = massiveReportId;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = true;
                    massiveReport.RecordsNumber = -2;
                    massiveReport.UrlFile = report.Description;
                    massiveReport.UserId = report.UserId;
                    massiveReport.RecordsProcessed = 1;

                    massiveReport = _massiveReportDAO.UpdateMassiveReport(massiveReport);
                }
                else
                {
                    // Se inserta 
                    massiveReport.Description = report.Description;
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.EndDate = DateTime.Now;
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.UrlFile = report.Name;
                    massiveReport.UserId = report.UserId;

                    massiveReport = _massiveReportDAO.SaveMassiveReport(massiveReport);
                }

                // Tipo de archivo 
                Format format = new Format();

                var fileType = report.ExportType == ExportTypes.ExcelTemplate ? FileTypes.ExcelTemplate : FileTypes.Text;
                format.FileType = report.ExportType == ExportTypes.Excel ? FileTypes.Excel : fileType;

                exportFileName = this.GenerateExportFileName(sharedPortable, report.Name, true, report.ExportType);

                if (format.FileType == FileTypes.ExcelTemplate)
                {
                    string source = sharedReport + "/" + report.Name;
                    string destination = exportFileName;
                    File.Copy(source, destination, true);
                }

                // Carpeta virtual de reportes generados
                virtualFileName = exportFileName.Replace(sharedPortable, "");

                int length = virtualPath.Length;

                string charVirtualPath = virtualPath.Substring((length - 1));

                if (charVirtualPath != "/")
                {
                    virtualPath = virtualPath + "/";
                }

                virtualFileName = virtualPath + virtualFileName;

                // Se obtiene el formato de la estructura

                List<FormatDetail> formatDetails = GetFormatTypeByFormatId(Convert.ToInt32(report.Format.Id));

                // Se envía al procedimiento la sección del formato (cabecera, detalle, sumario)/
                ArrayList reportHeaders = new ArrayList();
                ArrayList reportCollections = new ArrayList();

                // Se agrega el parámetro a ProcedureParameters
                List<Parameter> procedureParameters = report.StoredProcedure.ProcedureParameters;
                int parameterNumber = procedureParameters.Count;

                report.StoredProcedure.ProcedureParameters.Add(new Parameter
                {
                    Id = parameterNumber + 1,
                    Description = "@SECTION",
                    IsFormula = false,
                    Value = null,
                    DbType = typeof(Int32)
                });

                #region valida campo dinámico
                // Se revisa si uno de los campos contiene la palabra reservada como indicador de dinámico
                int dynamicCount = 0;
                string wordReserve = "";

                if (report.Parameters != null)
                {
                    foreach (Parameter parameter in report.Parameters)
                    {
                        if (parameter.Description == "@WORD_RESERVE")
                        {
                            wordReserve = (parameter.Value == null) ? "" : parameter.Value.ToString();
                        }
                    }
                    List<FormatField> formatFields = new List<FormatField>();
                    foreach (FormatDetail formatDetail in formatDetails)
                    {
                        if (formatDetail.FormatType == FormatTypes.Detail)
                        {
                            formatFields = formatDetail.Fields;
                        }
                    }

                    dynamicCount = formatFields.Count(sl => sl.Description == wordReserve);

                    if (dynamicCount > 0)
                    {
                        report.StoredProcedure.ProcedureParameters.Add(new Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@FORMAT_ID",
                            IsFormula = false,
                            Value = report.Format.Id
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Parameter
                        {
                            Id = parameterNumber + 3,
                            Description = "@WORD_RESERVE",
                            IsFormula = false,
                            Value = wordReserve
                        });

                        report.StoredProcedure.ProcedureParameters[parameterNumber].Value = 3;
                        CreateStructureReportByProcedure(report.StoredProcedure.ProcedureName, report.StoredProcedure.ProcedureParameters);

                        _dataFacadeManager.GetDataFacade().ClearObjectCache();
                        // Se obtiene de nuevo los campos
                        formatDetails = GetFormatTypeByFormatId(Convert.ToInt32(report.Format.Id));
                    }
                }

                #endregion

                report.IsAsync = false;
                foreach (FormatDetail formatDetail in formatDetails)
                {
                    if (formatDetail.FormatType == FormatTypes.Head)
                    {
                        report.StoredProcedure.ProcedureParameters[parameterNumber].Value = 1;
                        reportHeaders = CreateStructureReportByProcedure(report.StoredProcedure.ProcedureName, report.StoredProcedure.ProcedureParameters);
                    }
                    else if (formatDetail.FormatType == FormatTypes.Detail)
                    {
                        report.StoredProcedure.ProcedureParameters[parameterNumber].Value = 2;
                        // Solo procedimientos asíncronos
                        int page;

                        for (int i = 1; i <= row; i++)
                        {
                            page = i;
                            report.StoredProcedure.ProcedureParameters[indexPageNumber].Value = page;

                            ArrayList reportDetails = GetDataReportByProcedure(report);
                            reportCollections.Add(reportDetails);

                            if (i == row && resultNewPage > 0)
                            {
                                page = ++i;
                                report.StoredProcedure.ProcedureParameters[indexPageNumber].Value = page;
                                reportDetails = GetDataReportByProcedure(report);
                                reportCollections.Add(reportDetails);
                            }
                        }
                    }
                }
                report.IsAsync = isAsync;
                CreateStructureSpreadsheet(reportHeaders, reportCollections, format, formatDetails, exportFileName, pageSize, report.IsAsync, massiveReportId);

                // Se actualiza 
                massiveReport.RecordsNumber = -1;
                massiveReport.UrlFile = virtualFileName;
                _massiveReportDAO.UpdateMassiveReport(massiveReport);
            }
            catch (BusinessException exception)
            {

                if (report.IsAsync)
                {
                    // Se actualiza 
                    massiveReport.UrlFile = exception.Message;
                    massiveReport.UserId = report.UserId;
                    _massiveReportDAO.UpdateMassiveReport(massiveReport);
                }
            }
        }

        /// <summary>
        /// ToExcelIndexCol
        /// </summary>
        /// <param name="n"></param>
        /// <param name="res"></param>
        public void ToExcelIndexCol(int n, ref string res)
        {
            if (n != 0)            
            {
                int r = n % 26;
                n = n / 26;
                if (r == 0)
                {
                    ToExcelIndexCol(n - 1, ref res);
                }
                else
                {
                    ToExcelIndexCol(n, ref res);
                }
                if (r == 0)
                {
                    res += "Z";
                    if (n == 1)
                        return;
                }

                switch (r)
                {
                    case 1:
                        res += "A"; break;
                    case 2:
                        res += "B"; break;
                    case 3:
                        res += "C"; break;
                    case 4:
                        res += "D"; break;
                    case 5:
                        res += "E"; break;
                    case 6:
                        res += "F"; break;
                    case 7:
                        res += "G"; break;
                    case 8:
                        res += "H"; break;
                    case 9:
                        res += "I"; break;
                    case 10:
                        res += "J"; break;
                    case 11:
                        res += "K"; break;
                    case 12:
                        res += "L"; break;
                    case 13:
                        res += "M"; break;
                    case 14:
                        res += "N"; break;
                    case 15:
                        res += "O"; break;
                    case 16:
                        res += "P"; break;
                    case 17:
                        res += "Q"; break;
                    case 18:
                        res += "R"; break;
                    case 19:
                        res += "S"; break;
                    case 20:
                        res += "T"; break;
                    case 21:
                        res += "U"; break;
                    case 22:
                        res += "V"; break;
                    case 23:
                        res += "W"; break;
                    case 24:
                        res += "X"; break;
                    case 25:
                        res += "Y"; break;
                    case 26:
                        res += "Z"; break;
                }
            }
        }

        #endregion

        #endregion Public Methods

        #region Private Methods

        #region Structure

        /// <summary>
        /// styleStructure
        /// </summary>
        /// <param name="styleName"></param>
        /// <param name="sheetDocument"></param>
        /// <param name="font"></param>
        /// <returns>SLStyle</returns>
        private SLStyle styleStructure(string styleName, SLDocument sheetDocument, SLFont font)
        {
            SLStyle sLStyle = sheetDocument.CreateStyle();

            switch (styleName)
            {
                case "titleStyle":
                    sLStyle.SetFont("Arial", 20);
                    sLStyle.Font.Bold = true;
                    sLStyle.SetHorizontalAlignment(OpenXml.HorizontalAlignmentValues.Left);
                    break;
                case "detailHeader":
                    sLStyle.Font = font;
                    sLStyle.SetHorizontalAlignment(OpenXml.HorizontalAlignmentValues.Left);
                    break;
                case "headerStyle":
                    sLStyle.Font = font;
                    sLStyle.Fill.SetPattern(OpenXml.PatternValues.Solid, System.Drawing.Color.LightSteelBlue, System.Drawing.Color.Lavender);
                    sLStyle.SetBottomBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    sLStyle.SetTopBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    sLStyle.SetRightBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    sLStyle.SetLeftBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    break;
                case "numberStyle":
                    sLStyle.FormatCode = "$ #,##0.00"; 
                    sLStyle.SetBottomBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    sLStyle.SetTopBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    sLStyle.SetRightBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    sLStyle.SetLeftBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    break;
                case "datetimeStyle":
                    sLStyle.FormatCode = "dd/MM/yyyy";
                    sLStyle.SetBottomBorder(OpenXml.BorderStyleValues.Double, System.Drawing.Color.Black);
                    sLStyle.SetTopBorder(OpenXml.BorderStyleValues.Double, System.Drawing.Color.Black);
                    sLStyle.SetRightBorder(OpenXml.BorderStyleValues.Double, System.Drawing.Color.Black);
                    sLStyle.SetLeftBorder(OpenXml.BorderStyleValues.Double, System.Drawing.Color.Black);
                    break;
                case "stringStyle":
                    sLStyle.SetBottomBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    sLStyle.SetTopBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    sLStyle.SetRightBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    sLStyle.SetLeftBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    break;
                case "summaryStyle":
                    sLStyle.Font = font;
                    sLStyle.SetBottomBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    sLStyle.SetTopBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    sLStyle.SetRightBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    sLStyle.SetLeftBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    break;
                case "summaryNumberStyle":
                    sLStyle.FormatCode = "$ #,##0.00"; 
                    sLStyle.Font = font;
                    sLStyle.SetBottomBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    sLStyle.SetTopBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    sLStyle.SetRightBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    sLStyle.SetLeftBorder(OpenXml.BorderStyleValues.Double, SLThemeColorIndexValues.Accent1Color);
                    break;
                default:
                    sLStyle.SetFont("Arial", 21);
                    sLStyle.Font.Bold = true;
                    sLStyle.SetHorizontalAlignment(OpenXml.HorizontalAlignmentValues.Left);
                    break;
            }

            return sLStyle;
        }

        /// <summary>
        /// CreateStructureSpreadsheet
        /// </summary>
        /// <param name="reportHeaders"></param>
        /// <param name="reportDetails"></param>
        /// <param name="format"></param>
        /// <param name="formatDetails"></param>
        /// <param name="fileName"></param>
        /// <param name="pageSize"></param>
        /// <param name="isAsync"></param>
        private void CreateStructureSpreadsheet(ArrayList reportHeaders, ArrayList reportDetails, Format format,
                                                List<FormatDetail> formatDetails, string fileName, int pageSize,
                                                bool isAsync, int massiveReportId)
        {
            int iBegin = 0;
            int iEnd = 0;
            int cell = 0;
            var rowNumber = 0;
            var columnNumber = 0;
            bool createHeader = true;
            bool createSheet = false;
            bool titleDetail = true;
            string sheetName = "Sheet1";
            int sheetNumber = 2;
            int indexItem = 1;
            int saveItemLimited = 1;
            bool createSummary = true;
            int recordsProcessed = 1;

            int parameterSaveDisk = Convert.ToInt32(ConfigurationManager.AppSettings["ParameterSaveDisk"]);

            SLDocument sheetDocument = new SLDocument();

            if (format.FileType == FileTypes.ExcelTemplate)
            {
                using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    sheetDocument = new SLDocument(file, "Datos");  // Inicio el libro que ya había creado
                }
            }

            #region style

            SLFont font = sheetDocument.CreateFont();
            font.SetFont("Tahoma", 9);
            font.Bold = true;

            SLFont fontHeader = sheetDocument.CreateFont();
            fontHeader.SetFont("Arial", 12);

            SLStyle titleStyle = styleStructure("titleStyle", sheetDocument, font);
            SLStyle detailHeader = styleStructure("detailHeader", sheetDocument, fontHeader);
            SLStyle headerStyle = styleStructure("headerStyle", sheetDocument, font);
            SLStyle numberStyle = styleStructure("numberStyle", sheetDocument, font);
            SLStyle datetimeStyle = styleStructure("datetimeStyle", sheetDocument, font);
            SLStyle stringStyle = styleStructure("stringStyle", sheetDocument, font);
            SLStyle summaryStyle = styleStructure("summaryStyle", sheetDocument, font);
            SLStyle summaryNumberStyle = styleStructure("summaryNumberStyle", sheetDocument, font);
            #endregion

            List<FormatField> formatHeader = new List<FormatField>();
            List<FormatField> formatDetail = new List<FormatField>();
            List<FormatField> formatSummary = new List<FormatField>();

            // Separación de formatos cabecera, detalle, sumario
            foreach (FormatDetail detail in formatDetails)
            {
                if (detail.FormatType == FormatTypes.Head)
                {
                    formatHeader = detail.Fields.OrderBy(o => o.RowNumber).ToList();
                }
                else if (detail.FormatType == FormatTypes.Detail)
                {
                    formatDetail = detail.Fields;
                }
                else
                {
                    formatSummary = detail.Fields;
                }
            }

            int sumaryCount = formatSummary.Count;
            //////////////////////////////////////////////////////////////

            if (formatDetail != null && format.FileType == FileTypes.Excel && reportDetails.Count > 0)
            {
                // Registros del paginado (consulta del sp) 
                foreach (ArrayList itemList in reportDetails)
                {
                    // Recorrido de detalle
                    foreach (Array item in itemList)
                    {
                        if (createSheet)
                        {
                            createSheet = false;
                            sheetName = "Sheet";
                            sheetName = sheetName + Convert.ToString(sheetNumber);
                            sheetDocument.AddWorksheet(sheetName);
                            sheetNumber++;
                        }

                        #region Header

                        if (createHeader)
                        {
                            createHeader = false;
                            createSummary = true;
                            indexItem = 1;

                            if (formatHeader != null)
                            {
                                // Creación de títulos 
                                int rownumber = 0;

                                foreach (FormatField formatField in formatHeader)
                                {
                                    if (rownumber != formatField.RowNumber)
                                    {
                                        List<FormatField> formatFieldHeader = formatHeader.Where(sl => sl.RowNumber == formatField.RowNumber).ToList();
                                        rownumber = formatField.RowNumber;
                                        rowNumber = formatFieldHeader[0].RowNumber;

                                        foreach (FormatField headerFormatField in formatFieldHeader)
                                        {
                                            bool headerinto = true;
                                            columnNumber = headerFormatField.ColumnNumber;
                                            if (headerFormatField.Align == null || headerFormatField.Align == "")
                                            {
                                                Array itemheader = (Array)reportHeaders[0];
                                                int order = 0;
                                                int columMerge = 0;
                                                order = headerFormatField.Order - 1;
                                                columMerge = columnNumber + 8;

                                                sheetDocument.SetCellValue(rowNumber, columnNumber, itemheader.GetValue(order).ToString());
                                                sheetDocument.SetCellStyle(rowNumber, columnNumber, titleStyle);
                                                sheetDocument.MergeWorksheetCells(rowNumber, columnNumber, rowNumber, columMerge);

                                                headerinto = false;
                                            }
                                            else
                                            {
                                                sheetDocument.SetCellValue(rowNumber, columnNumber, headerFormatField.Description);
                                                sheetDocument.SetColumnWidth(columnNumber, 25);
                                                fontHeader.Bold = true;
                                                sheetDocument.SetCellStyle(rowNumber, columnNumber, detailHeader);
                                            }

                                            //* Detalles
                                            if (headerinto)
                                            {
                                                columnNumber = columnNumber + 1;
                                                if (reportHeaders.Count > 0)
                                                {
                                                    Array itemheader = (Array)reportHeaders[0];
                                                    int order = 0;
                                                    order = headerFormatField.Order - 1;
                                                    sheetDocument.SetCellValue(rowNumber, columnNumber, itemheader.GetValue(order).ToString());
                                                    fontHeader.Bold = false;
                                                    sheetDocument.SetCellStyle(rowNumber, columnNumber, detailHeader);
                                                }
                                            }
                                        }
                                    }
                                }
                                rowNumber++;
                            }
                        }

                        #endregion

                        #region Detail

                        if (titleDetail)
                        {
                            titleDetail = false;
                            cell = 1;
                            rowNumber = formatDetail[0].RowNumber;

                            // Creación de títulos
                            foreach (FormatField formatFieldDetail in formatDetail)
                            {
                                sheetDocument.SetCellValue(rowNumber, cell, formatFieldDetail.Description);
                                sheetDocument.SetCellStyle(rowNumber, cell, headerStyle);
                                sheetDocument.SetColumnWidth(cell, 25);
                                cell++;
                            }
                            iBegin = rowNumber + 2;
                        }

                        rowNumber++; // Contador de filas, que se incrementa por cada detalle para iniciar en el Sumario
                        iEnd = rowNumber + 1;
                        int cellParam = 0;
                        if (isAsync)
                        {
                            cellParam = 3;
                        }

                        for (int i = 1; i < cell; i++)
                        {
                            if (i < item.Length)
                            {
                                // Si el valor es numérico 
                                Regex regex = new Regex("^(\\d|-)?(\\d|,)*\\.?\\d*$");
                                if (regex.IsMatch(item.GetValue(cellParam).ToString()))
                                {
                                    var cellValue = item.GetValue(cellParam) == DBNull.Value ? "0" : item.GetValue(cellParam).ToString();
                                    if (cellValue.IndexOf(',') > -1)
                                    {
                                        cellValue = cellValue.Replace(',', '.');
                                        sheetDocument.SetCellValueNumeric(rowNumber, i, cellValue);
                                        sheetDocument.SetCellStyle(rowNumber, i, numberStyle);
                                    }
                                    else
                                    {
                                        sheetDocument.SetCellValue(rowNumber, i, cellValue);
                                        sheetDocument.SetCellStyle(rowNumber, i, stringStyle);
                                    }
                                }
                                else // Si el valor es texto
                                {
                                    // Para cuando la fecha sea null
                                    if (item.GetValue(cellParam).ToString() != "01/01/1900")
                                    {
                                        sheetDocument.SetCellValue(rowNumber, i, item.GetValue(cellParam).ToString());
                                    }
                                    else
                                    {
                                        sheetDocument.SetCellValue(rowNumber, i, "");
                                    }

                                    sheetDocument.SetCellStyle(rowNumber, i, stringStyle);
                                }
                            }
                            cellParam++;
                        }

                        #endregion

                        #region Summary

                        // Número de registros que se grabaran en Disco para la liberación de memoria
                        if (saveItemLimited == parameterSaveDisk)
                        {
                            sheetDocument.SaveAs(fileName);
                            sheetDocument.Dispose();


                            // Se recupera el archivo para seguir escribiendo
                            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
                            {
                                sheetDocument = new SLDocument(file, sheetName);  // Inicio el libro que ya había creado
                            }

                            titleStyle = styleStructure("titleStyle", sheetDocument, font);
                            detailHeader = styleStructure("detailHeader", sheetDocument, fontHeader);
                            headerStyle = styleStructure("headerStyle", sheetDocument, font);
                            numberStyle = styleStructure("numberStyle", sheetDocument, font);
                            datetimeStyle = styleStructure("datetimeStyle", sheetDocument, font);
                            stringStyle = styleStructure("stringStyle", sheetDocument, font);
                            summaryStyle = styleStructure("summaryStyle", sheetDocument, font);
                            summaryNumberStyle = styleStructure("summaryNumberStyle", sheetDocument, font);

                            saveItemLimited = 1;
                        }

                        // Si llega al límite de registros por página graba el sumario y crea una nueva hoja
                        if (indexItem == pageSize)
                        {
                            createHeader = true;
                            titleDetail = true;
                            createSheet = true;

                            if (formatSummary != null)
                            {
                                cell = 0;
                                string col = "";
                                rowNumber = rowNumber + 1;
                                iBegin = iBegin - 1;
                                iEnd = iEnd - 1;

                                foreach (FormatField formatFieldSumary in formatSummary)
                                {
                                    columnNumber = formatFieldSumary.ColumnNumber;
                                    if (formatFieldSumary.Align == null || formatFieldSumary.Align == "")
                                    {
                                        sheetDocument.SetCellValue(rowNumber, columnNumber, formatFieldSumary.Description);
                                        sheetDocument.SetCellStyle(rowNumber, columnNumber, summaryStyle);
                                        sheetDocument.SetColumnWidth(columnNumber, 25);
                                    }
                                    else
                                    {
                                        col = "";
                                        // Obtiene el caracter de la fila del excel
                                        ToExcelIndexCol(formatFieldSumary.ColumnNumber, ref col);
                                        sheetDocument.SetCellValue(rowNumber, columnNumber, "=SUM(" + col + iBegin + ":" + col + iEnd + ")");
                                        sheetDocument.SetCellStyle(rowNumber, columnNumber, summaryNumberStyle);
                                    }
                                }
                            }
                            createSummary = false;
                        }
                        #endregion

                        // Se Actualiza 
                        MassiveReport massiveReport = new MassiveReport();
                        massiveReport.Id = massiveReportId;
                        massiveReport.RecordsNumber = -3;
                        massiveReport.RecordsProcessed = recordsProcessed;
                        massiveReport.EndDate = DateTime.Now;
                        _massiveReportDAO.UpdateMassiveReport(massiveReport);

                        indexItem++;
                        recordsProcessed++;
                        saveItemLimited++;
                    }
                }

                #region NewSheetSumary

                if (createSummary && sumaryCount > 0)
                {                    
                    string col = "";
                    rowNumber = rowNumber + 1;

                    iBegin = iBegin - 1;
                    iEnd = iEnd - 1;

                    foreach (FormatField formatFieldSummary in formatSummary)
                    {
                        columnNumber = formatFieldSummary.ColumnNumber;
                        if (formatFieldSummary.Align == null || formatFieldSummary.Align == "")
                        {
                            sheetDocument.SetCellValue(rowNumber, columnNumber, formatFieldSummary.Description);
                            sheetDocument.SetCellStyle(rowNumber, columnNumber, summaryStyle);
                            sheetDocument.SetColumnWidth(columnNumber, 25);
                        }
                        else
                        {
                            col = "";
                            // Obtiene el caracter de la fila del excel
                            if (formatFieldSummary.ColumnNumber > 0 && iEnd > 0)
                            {
                                ToExcelIndexCol(formatFieldSummary.ColumnNumber, ref col);
                                sheetDocument.SetCellValue(rowNumber, columnNumber, "=SUM(" + col + iBegin + ":" + col + iEnd + ")");
                                sheetDocument.SetCellStyle(rowNumber, columnNumber, summaryNumberStyle);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                #endregion
            }

            sheetDocument.SaveAs(fileName);
            sheetDocument.Dispose();
        }

        /// <summary>
        /// GetFormatTypeByFormatId
        /// Obtiene la lista de parametrización de plantillas
        /// </summary>
        /// <param name="formatId"></param>
        /// <returns>List<FormatDetail/></returns>
        private List<FormatDetail> GetFormatTypeByFormatId(int formatId)
        {
            Format format = new Format();
            format.Id = formatId;
            format.FileType = FileTypes.Text;

            List<FormatDetail> formatDetails = _formatDetailDAO.GetFormatDetailsByFormat(format);
            return formatDetails;
        }

        /// <summary>
        /// GetDataReportByProcedure
        /// </summary>
        /// <param name="report"></param>
        /// <returns>ArrayList</returns>
        private ArrayList GetDataReportByProcedure(Report report)
        {
            ArrayList collections = new ArrayList();
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
                    var procedureParameters = new Procedure.Param[parameters.Count];
                    var index = 0;
                    foreach (Parameter parameter in parameters)
                    {
                        if (parameters[index].Value == null || parameters[index].Value.ToString() == "null")
                        {
                            procedureParameters[index] = new Procedure.Param(parameters[index].Description, DBNull.Value);
                        }
                        else
                        {
                            procedureParameters[index] = new Procedure.Param(parameters[index].Description, parameters[index].Value);
                        }

                        index++;
                    }
                    collections = _dataFacadeManager.GetDataFacade().ExecuteSPReader(procedure, procedureParameters);
                }

                return collections;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Structure

        #region ExportFile

        /// <summary>
        /// GenerateExportFileName
        /// </summary>
        /// <param name="exportFolder"></param>
        /// <param name="exportFileName"></param>
        /// <param name="includeTimestampOnFile"></param>
        /// <param name="isPdf"></param>
        /// <returns>string</returns>
        private string GenerateExportFileName(string exportFolder, string exportFileName, bool includeTimestampOnFile, ExportTypes exportTypes)
        {
            string dateTime = string.Format("{0:yyyyMMddhhmmss}", DateTime.Now);
            string extension = ".pdf";

            if (exportTypes == ExportTypes.Excel)
            {
                extension = ".xlsx";
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
        /// CreateStructureReportByProcedure
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="parameters"></param>
        /// <returns>ArrayList</returns>
        private ArrayList CreateStructureReportByProcedure(string procedure, List<Parameter> parameters)
        {
            try
            {
                var procedureParameters = new Procedure.Param[parameters.Count];
                var index = 0;
                foreach (Parameter parameter in parameters)
                {
                    if (parameters[index].Value == null || parameters[index].Value.ToString() == "null")
                    {
                        procedureParameters[index] = new Procedure.Param(parameters[index].Description, DBNull.Value);
                    }
                    else
                    {
                        procedureParameters[index] = new Procedure.Param(parameters[index].Description, parameters[index].Value);
                    }

                    index++;
                }

                ArrayList collections = _dataFacadeManager.GetDataFacade().ExecuteSPReader(procedure, procedureParameters);

                return collections;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #endregion Private Methods



    }
}
