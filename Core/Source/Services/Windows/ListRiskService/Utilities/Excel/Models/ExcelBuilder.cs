using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Linq;
using Sistran.Core.Services.UtilitiesServices.Models;
using Utilities.Excel.Assemblers;
using System.Configuration;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DOS = DocumentFormat.OpenXml.Spreadsheet;
using System;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Utilities.Excel.Helpers;
using System.Text.RegularExpressions;

namespace Utilities.Excel.Models
{
    class ExcelBuilder
    {

        public string CreateCSVFile(Sistran.Core.Services.UtilitiesServices.Models.File file, string path, string separator)
        {
            string fileName = path + "\\" + file.Name + ".csv";
            

            using (System.IO.FileStream csvFile = new System.IO.FileStream(fileName, System.IO.FileMode.Append, System.IO.FileAccess.Write))
            {

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(csvFile, Encoding.UTF8))
                {
                    foreach (Template template in file.Templates)
                    {
                        foreach (Field field in template.Rows[0].Fields)
                        {
                            sw.Write(field.Description + separator);
                        }
                        sw.Write("\r\n");
                        foreach (Row row in template.Rows)
                        {
                            foreach (Field field in row.Fields)
                            {
                                sw.Write(field.Value + separator);
                            }
                            sw.Write("\r\n");
                        }
                    }
                    sw.Close();

                }
                csvFile.Close();

            }


            return fileName;
        }
        public string CreateExcelFile(File file, string path)
        {
            try
            {
                string fileName = path + "\\" + file.Name + ".xlsx";

                using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new DOS.Workbook();
                    UInt32 sheetId = 1;

                    document.WorkbookPart.Workbook.Sheets = new DOS.Sheets();
                    DOS.Sheets sheets = document.WorkbookPart.Workbook.GetFirstChild<DOS.Sheets>();


                    WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                    stylePart.Stylesheet = FileHelper.GetStylesExcel();
                    stylePart.Stylesheet.Save();

                    foreach (Template template in file.Templates)
                    {

                        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        DOS.Sheet sheet = new DOS.Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = sheetId, Name = template.Description };
                        sheets.Append(sheet);
                        sheetId++;
                        worksheetPart.Worksheet = new DOS.Worksheet();

                        DOS.SheetData sheetData = new DOS.SheetData();
                        worksheetPart.Worksheet = new DOS.Worksheet(sheetData);

                        DOS.MergeCells mergeCells = new DOS.MergeCells();

                        workbookPart.Workbook.Save();
                        int headersCount = template.Rows.Where(r => r.Fields != null && r.Fields.Count > 0).Max(r => r.Fields.Max(x => x.RowPosition));
                        int rowsCount = 0;
                        DOS.Row[] rowsHead = new DOS.Row[headersCount];

                        foreach (Row row in template.Rows.Where(r => r.Fields != null))
                        {
                            if (rowsCount < headersCount)
                            {
                                int columnSpan = 1;
                                foreach (Field field in row.Fields)
                                {
                                    if (rowsHead[field.RowPosition - 1] == null)
                                    {
                                        rowsHead[field.RowPosition - 1] = new DOS.Row();
                                    }

                                    DOS.Cell cell = FileHelper.ConstructCell(field.Description, DOS.CellValues.String, field.RowPosition, columnSpan - 1, 2);
                                    rowsHead[field.RowPosition - 1].Append(cell);
                                    mergeCells.Append(FileHelper.MergeCell(field.RowPosition, (columnSpan - 1), (field.RowPosition), (columnSpan + field.ColumnSpan - 2)));
                                    columnSpan += field.ColumnSpan;
                                }
                                rowsCount++;
                            }
                        }
                        worksheetPart.Worksheet.InsertAfter(mergeCells, worksheetPart.Worksheet.Elements<DOS.SheetData>().First());
                        rowsHead.ToList().ForEach(x => sheetData.AppendChild(x));

                        foreach (Row row in template.Rows.Where(r => r.Fields != null))
                        {
                            string someValue = string.Join("", row.Fields.Select(x => x.Value));
                            if (!string.IsNullOrEmpty(someValue))
                            {
                                DOS.Row fileRow = new DOS.Row();
                                foreach (Field field in row.Fields)
                                {
                                    if (field.Value != null)
                                    {
                                        switch (field.FieldType)
                                        {
                                            case FieldType.Int32:
                                            case FieldType.Int8:
                                                fileRow.Append(FileHelper.ConstructCell(field.Value, DOS.CellValues.Number));
                                                break;
                                            case FieldType.Int64:
                                                fileRow.Append(FileHelper.ConstructCell(field.Value, DOS.CellValues.Number));
                                                break;
                                            case FieldType.Decimal:
                                                fileRow.Append(FileHelper.ConstructCell(field.Value, DOS.CellValues.Number));
                                                break;
                                            case FieldType.String:
                                                field.Value = Regex.Replace(field.Value, @"[\u001a-\u001f]", "");
                                                fileRow.Append(FileHelper.ConstructCell(field.Value, DOS.CellValues.String));
                                                break;
                                            case FieldType.DateTime:
                                                fileRow.Append(FileHelper.ConstructCell(field.Value, DOS.CellValues.String));
                                                break;
                                            case FieldType.Boolean:
                                                fileRow.Append(FileHelper.ConstructCell(field.Value, DOS.CellValues.String));
                                                break;
                                            case FieldType.Int16:
                                                fileRow.Append(FileHelper.ConstructCell(field.Value.ToString(), DOS.CellValues.Number));
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        fileRow.Append(FileHelper.ConstructCell(String.Empty, DOS.CellValues.String));
                                    }
                                }

                                sheetData.AppendChild(fileRow);
                            }
                        }
                        worksheetPart.Worksheet.Save();
                    }

                    document.WorkbookPart.Workbook.Save();
                    document.Close();
                }

                return fileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
