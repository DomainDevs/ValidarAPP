// -----------------------------------------------------------------------
// <copyright file="FileDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs
{
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Helper;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using DOS = DocumentFormat.OpenXml.Spreadsheet;
    using UtilComm = Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    using UTVIEW = Sistran.Core.Application.UtilitiesServicesEEProvider.Entities.Views;

    /// <summary>
    /// Clase para la generación de los archivos de excel.
    /// </summary>
    public class FileDAO
    {
		/// <summary>
        /// Genera archivo excel de tipos de riesgo cubierto.
        /// </summary>
        /// <param name="coveredRiskTypesList">Lista de tipos de riesgo cubierto</param>
        /// <param name="fileName">Url del archivo</param>
        /// <returns>Url del archivo generado.</returns>
        public string GenerateFileToCoveredRiskTypes(List<CoveredRiskTypeServiceModel> coveredRiskTypesList, string fileName)
        {
            UtilComm.FileDAO commonFileDAO = new UtilComm.FileDAO();
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ParametrizationCeoveredRiskType;

            File file = commonFileDAO.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (CoveredRiskTypeServiceModel coveredRiskType in coveredRiskTypesList)
                {
                    var fields = file.Templates[0].Rows[0].Fields.Select(x => new Field
                    {
                        ColumnSpan = x.ColumnSpan,
                        Description = x.Description,
                        FieldType = x.FieldType,
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        IsMandatory = x.IsMandatory,
                        Order = x.Order,
                        RowPosition = x.RowPosition,
                        SmallDescription = x.SmallDescription
                    }).ToList();

                    fields[0].Value = coveredRiskType.Id.ToString();
                    fields[1].Value = coveredRiskType.SmallDescription;                    

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                return commonFileDAO.GenerateFile(file);
            }
            else
            {
                return string.Empty;
            }
        }

        #region AllyCoverage
        /// <summary>
        /// Genera archivo excel de tipos de riesgo cubierto.
        /// </summary>
        /// <param name="coveredRiskTypesList">Lista de tipos de riesgo cubierto</param>
        /// <param name="fileName">Url del archivo</param>
        /// <returns>Url del archivo generado.</returns>
        public string GenerateFileToAllyCoverage(List<ParamQueryCoverage> li_ally_coverage, string fileName)
        {
            UtilComm.FileDAO commonFileDAO = new UtilComm.FileDAO();
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ParametrizationAllyCoverage;

            File file = commonFileDAO.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (ParamQueryCoverage allyC_item in li_ally_coverage)
                {
                    var fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(x => new Field
                    {
                        ColumnSpan = x.ColumnSpan,
                        Description = x.Description,
                        FieldType = x.FieldType,
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        IsMandatory = x.IsMandatory,
                        Order = x.Order,
                        RowPosition = x.RowPosition,
                        SmallDescription = x.SmallDescription
                    }).ToList();

                    fields[0].Value = allyC_item.AllyCoverage.Description;
                    fields[1].Value = allyC_item.CoveragePrincipal.Description;
                    fields[2].Value = allyC_item.CoveragePercentage.ToString();
                    

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                return commonFileDAO.GenerateFile(file);
            }
            else
            {
                return string.Empty;
            }
        }

		//Se comenta debido a que llama librería en solución que no sigue la arquitectura del desarrollo
        //public string GetFileAllyCoverage(List<ParamQueryCoverage> li_ally_coverage, string fileName)
        //{
        //    try
        //    {
        //        FileProcessValue fileProcessValue = new FileProcessValue();
        //        fileProcessValue.Key1 = (int)FileProcessType.ParametrizationCoCoverageValue;

        //        File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

        //        if (file != null && file.IsEnabled)
        //        {
        //            file.Name = fileName;
        //            List<Row> rows = new List<Row>();

        //            foreach (ParamQueryCoverage queryCoverage in li_ally_coverage)
        //            {
        //                var fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(x => new Field
        //                {
        //                    ColumnSpan = x.ColumnSpan,
        //                    Description = x.Description,
        //                    FieldType = x.FieldType,
        //                    Id = x.Id,
        //                    IsEnabled = x.IsEnabled,
        //                    IsMandatory = x.IsMandatory,
        //                    Order = x.Order,
        //                    RowPosition = x.RowPosition,
        //                    SmallDescription = x.SmallDescription
        //                }).ToList();

        //                fields[0].Value = queryCoverage.CoveragePrincipal.Description;//coCoverage.Prefix.Description.ToString();
        //                fields[1].Value = queryCoverage.AllyCoverage.Description;//Coverage.Description.ToString();
        //                fields[2].Value = string.Format("{0} %", queryCoverage.CoveragePercentage);//coCoverage.Percentage.ToString();

        //                rows.Add(new Row
        //                {
        //                    Fields = fields
        //                });
        //            }

        //            file.Templates[0].Rows = rows;
        //            file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
        //            return DelegateService.utilitiesServiceCore.GenerateFile(file);
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}
        #endregion

        /// <summary>
        /// Obtener Archivo Por Filtros
        /// </summary>
        /// <param name="fileProcessValue">Valores del proceso</param>
        /// <returns>Archivo a exportar</returns>
        public File GetFileByFileProcessValue(FileProcessValue fileProcessValue)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.FileProcessValue.Properties.Key1, typeof(COMMEN.FileProcessValue).Name).Equal().Constant(fileProcessValue.Key1);

            if (fileProcessValue.Key2 > 0)
            {
                filter.And();
                filter.Property(COMMEN.FileProcessValue.Properties.Key2, typeof(COMMEN.FileProcessValue).Name).Equal().Constant(fileProcessValue.Key2);
            }

            if (fileProcessValue.Key3 > 0)
            {
                filter.And();
                filter.Property(COMMEN.FileProcessValue.Properties.Key3, typeof(COMMEN.FileProcessValue).Name).Equal().Constant(fileProcessValue.Key3);
            }

            if (fileProcessValue.Key4 > 0)
            {
                filter.And();
                filter.Property(COMMEN.FileProcessValue.Properties.Key4, typeof(COMMEN.FileProcessValue).Name).Equal().Constant(fileProcessValue.Key4);
            }

            if (fileProcessValue.Key5 > 0)
            {
                filter.And();
                filter.Property(COMMEN.FileProcessValue.Properties.Key5, typeof(COMMEN.FileProcessValue).Name).Equal().Constant(fileProcessValue.Key5);
            }

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.FileProcessValue), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                return this.GetFileByFileId(businessCollection.Cast<COMMEN.FileProcessValue>().First().FileId);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener Archivo Por Identificador
        /// </summary>
        /// <param name="fileId">Identificador de archivo</param>
        /// <returns>Archivo a exportar</returns>
        public File GetFileByFileId(int fileId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            UTVIEW.FileFieldView fileFieldView = new UTVIEW.FileFieldView();
            ViewBuilder builder = new ViewBuilder("FileFieldView");

            filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.File.Properties.Id, typeof(COMMEN.File).Name);
            filter.Equal();
            filter.Constant(fileId);

            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, fileFieldView);

            if (fileFieldView.Files.Count > 0)
            {
                File file = ModelAssembler.CreateFile(fileFieldView.Files.Cast<COMMEN.File>().First());

                if (file.IsEnabled)
                {
                    file.Templates = new List<Template>();

                    List<COMMEN.FileTemplate> fileTemplates = fileFieldView.FileTemplates.Cast<COMMEN.FileTemplate>().ToList();
                    foreach (COMMEN.FileTemplate entityFileTemplate in fileTemplates.OrderBy(x => x.Order))
                    {
                        if (entityFileTemplate.IsEnabled)
                        {
                            COMMEN.Template entityTemplate = fileFieldView.Template.Cast<COMMEN.Template>().First(x => x.Id == entityFileTemplate.TemplateId);
                            Template template = new Template
                            {
                                Id = entityFileTemplate.Id,
                                TemplateId = entityTemplate.Id,
                                IsMandatory = entityFileTemplate.IsMandatory,
                                Rows = new List<Row>(),
                                Order = entityFileTemplate.Order,
                                IsPrincipal = entityFileTemplate.IsPrincipal,
                                Description = entityFileTemplate.Description,
                                PropertyName = entityTemplate.PropertyName
                            };
                            List<int> templeteFile = fileFieldView.FileTemplateFields.Cast<COMMEN.FileTemplateField>().ToList().Where(x => x.FileTemplateId == template.Id).Select(y => y.FieldId).ToList();
                            List<COMMEN.Field> entityFields = fileFieldView.Fields.Cast<COMMEN.Field>().ToList().Where(x => templeteFile.Contains(x.Id)).ToList();
                            int maxRow = fileFieldView.FileTemplateFields.Cast<COMMEN.FileTemplateField>().Where(z => z.FileTemplateId == template.Id).Max(x => x.RowPosition);

                            for (int i = 1; i <= maxRow; i++)
                            {
                                List<Field> fields = new List<Field>();

                                foreach (COMMEN.FileTemplateField entityFileTemplateField in fileFieldView.FileTemplateFields.Cast<COMMEN.FileTemplateField>().OrderBy(x => x.Order).Where(x => x.FileTemplateId == template.Id && x.RowPosition == i))
                                {
                                    if (entityFileTemplateField.IsEnabled)
                                    {
                                        fields.Add(ModelAssembler.CreateField(entityFileTemplateField, entityFields.First(x => x.Id == entityFileTemplateField.FieldId)));
                                    }
                                }

                                template.Rows.Add(new Row
                                {
                                    Number = i,
                                    Fields = fields
                                });
                            }

                            file.Templates.Add(template);
                        }
                    }
                }

                return file;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Crear Archivo
        /// </summary>
        /// <param name="file">Información del archivo</param>
        /// <returns>Ruta Archivo</returns>
        public string GenerateFile(File file)
        {
            string urlFile = string.Empty;

            switch (file.FileType)
            {
                case FileType.Excel:
                    urlFile = this.CreateExcelFile(file);
                    break;
                case FileType.CSV:
                    urlFile = this.CreateCSVFile(file);
                    break;
            }

            return urlFile;
        }

        /// <summary>
        /// Crear Archivo Excel
        /// </summary>
        /// <param name="file">Información del archivo</param>
        /// <returns>Información a exportar</returns>
        public string CreateExcelFile(File file)
        {
            try
            {
                string fileName = ConfigurationManager.AppSettings["ReportExportPath"] + "\\" + file.Name + ".xlsx";

                using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new DOS.Workbook();
                    uint sheetId = 1;

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
                                    mergeCells.Append(FileHelper.MergeCell(field.RowPosition, columnSpan - 1, field.RowPosition, columnSpan + field.ColumnSpan - 2));
                                    columnSpan += field.ColumnSpan;
                                }

                                rowsCount++;
                            }
                        }

                        worksheetPart.Worksheet.InsertAfter(mergeCells, worksheetPart.Worksheet.Elements<DOS.SheetData>().First());
                        rowsHead.ToList().ForEach(x => sheetData.AppendChild(x));

                        foreach (Row row in template.Rows.Where(r => r.Fields != null))
                        {
                            string someValue = string.Join(string.Empty, row.Fields.Select(x => x.Value));
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
                                                fileRow.Append(FileHelper.ConstructCell(field.Value.ToString(), DOS.CellValues.Number));
                                                break;
                                            case FieldType.Int64:
                                                fileRow.Append(FileHelper.ConstructCell(field.Value.ToString(), DOS.CellValues.Number));
                                                break;
                                            case FieldType.Decimal:
                                                fileRow.Append(FileHelper.ConstructCell(field.Value.ToString(), DOS.CellValues.Number));
                                                break;
                                            case FieldType.String:
                                                fileRow.Append(FileHelper.ConstructCell(field.Value.ToString(), DOS.CellValues.String));
                                                break;
                                            case FieldType.DateTime:
                                                fileRow.Append(FileHelper.ConstructCell(field.Value.ToString(), DOS.CellValues.Date));
                                                break;
                                            case FieldType.Boolean:
                                                fileRow.Append(FileHelper.ConstructCell(field.Value.ToString(), DOS.CellValues.Boolean));
                                                break;
                                            case FieldType.Int8:
                                                fileRow.Append(FileHelper.ConstructCell(field.Value.ToString(), DOS.CellValues.Number));
                                                break;
                                            case FieldType.Int16:
                                                fileRow.Append(FileHelper.ConstructCell(field.Value.ToString(), DOS.CellValues.Number));
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        fileRow.Append(FileHelper.ConstructCell(string.Empty, DOS.CellValues.String));
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

        /// <summary>
        /// Crear Archivo CSV
        /// </summary>
        /// <param name="file">Información del archivo</param>
        /// <returns>Información a exportar</returns>
        private string CreateCSVFile(File file)
        {
            string fileName = ConfigurationManager.AppSettings["ReportExportPath"] + "\\" + file.Name + ".csv";
            string separator = ConfigurationManager.AppSettings["ReportListSeparator"];

            using (System.IO.FileStream csvFile = new System.IO.FileStream(fileName, System.IO.FileMode.Append, System.IO.FileAccess.Write))
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(csvFile, Encoding.UTF8))
                {
                    foreach (Field field in file.Templates[0].Rows[0].Fields)
                    {
                        sw.Write(field.Description + separator);
                    }

                    sw.Write("\r\n");
                    foreach (Row row in file.Templates[0].Rows)
                    {
                        foreach (Field field in row.Fields)
                        {
                            sw.Write(field.Value + separator);
                        }

                        sw.Write("\r\n");
                    }

                    sw.Close();
                }

                csvFile.Close();
            }

            return fileName;
        }
    }
}
