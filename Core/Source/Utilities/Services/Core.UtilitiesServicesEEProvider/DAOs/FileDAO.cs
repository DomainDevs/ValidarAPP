using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UtilitiesServicesEEProvider.Entities.Views;
using Sistran.Core.Application.UtilitiesServicesEEProvider.Helper;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.UtilitiesServicesEEProvider.Assemblers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using COMMEN = Sistran.Core.Application.Common.Entities;
using DOS = DocumentFormat.OpenXml.Spreadsheet;

namespace Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs
{
    public class FileDAO
    {

        /// <summary>
        /// Obtiene la lista de sucursales
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        public List<File> GetFiles(string Description, FileType fileType)
        {
            FileFieldView fileFieldView = new FileFieldView();
            ViewBuilder builder = new ViewBuilder("FileFieldView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.File.Properties.Description, typeof(COMMEN.File).Name);
            filter.Like();
            filter.Constant('%' + Description + '%');
            filter.And();
            filter.Property(COMMEN.File.Properties.FileTypeId, typeof(COMMEN.File).Name);
            filter.Equal();
            filter.Constant(fileType);
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, fileFieldView);

            if (fileFieldView.Files.Count > 0)
            {
                List<File> files = new List<File>();
                foreach (BusinessObject item in fileFieldView.Files)
                {
                    File file = ModelAssembler.CreateFile((COMMEN.File)item);
                    file.Templates = new List<Template>();

                    List<COMMEN.FileTemplate> fileTemplates = fileFieldView.FileTemplates.Cast<COMMEN.FileTemplate>().Where(s => s.FileId == file.Id).ToList();
                    foreach (COMMEN.FileTemplate entityFileTemplate in fileTemplates.OrderBy(x => x.Order))
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
                            Description = entityTemplate.Description,
                            PropertyName = entityTemplate.PropertyName,
                            IsEnabled = entityFileTemplate.IsEnabled
                        };

                        List<COMMEN.Field> entityFields = fileFieldView.Fields.Cast<COMMEN.Field>().ToList();
                        int maxRow = fileFieldView.FileTemplateFields.Cast<COMMEN.FileTemplateField>().Max(x => x.RowPosition);

                        for (int i = 1; i <= maxRow; i++)
                        {
                            ConcurrentBag<Field> fields = new ConcurrentBag<Field>();

                            foreach (COMMEN.FileTemplateField entityFileTemplateField in fileFieldView.FileTemplateFields.Cast<COMMEN.FileTemplateField>().Where(x => x.FileTemplateId == template.Id && x.RowPosition == i))
                            {
                                fields.Add(ModelAssembler.CreateField(entityFileTemplateField, entityFields.First(x => x.Id == entityFileTemplateField.FieldId)));
                            }

                            template.Rows.Add(new Row
                            {
                                Number = i,
                                Fields = fields.ToList()
                            });
                        }

                        file.Templates.Add(template);

                    }
                    files.Add(file);
                }

                return files;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene la lista de archivos
        /// </summary>
        /// <returns>Lista de archivos consultadas</returns>
        public List<File> GetFileByDescription(string Description)
        {
            FileFieldView fileFieldView = new FileFieldView();
            ViewBuilder builder = new ViewBuilder("FileFieldView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.File.Properties.Description, typeof(COMMEN.File).Name);
            filter.Like();
            filter.Constant('%' + Description + '%');

            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, fileFieldView);

            if (fileFieldView.Files.Count > 0)
            {
                List<File> files = new List<File>();
                foreach (BusinessObject item in fileFieldView.Files)
                {
                    File file = ModelAssembler.CreateFile((COMMEN.File)item);
                    file.Templates = new List<Template>();

                    List<COMMEN.FileTemplate> fileTemplates = fileFieldView.FileTemplates.Cast<COMMEN.FileTemplate>().Where(s => s.FileId == file.Id).ToList();
                    foreach (COMMEN.FileTemplate entityFileTemplate in fileTemplates.OrderBy(x => x.Order))
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
                            Description = entityTemplate.Description,
                            PropertyName = entityTemplate.PropertyName,
                            IsEnabled = entityFileTemplate.IsEnabled
                        };
                        List<int> templeteFile = fileFieldView.FileTemplateFields.Cast<COMMEN.FileTemplateField>().ToList().Where(x => x.FileTemplateId == template.Id).Select(y => y.FieldId).ToList();
                        List<COMMEN.Field> entityFields = fileFieldView.Fields.Cast<COMMEN.Field>().ToList().Where(x => templeteFile.Contains(x.Id)).ToList();
                        int maxRow = fileFieldView.FileTemplateFields.Cast<COMMEN.FileTemplateField>().Where(z => z.FileTemplateId == template.Id).Max(x => x.RowPosition);

                        for (int i = 1; i <= maxRow; i++)
                        {
                            List<Field> fields = new List<Field>();

                            foreach (COMMEN.FileTemplateField entityFileTemplateField in fileFieldView.FileTemplateFields.Cast<COMMEN.FileTemplateField>().Where(x => x.FileTemplateId == template.Id && x.RowPosition == i).OrderBy(x => x.Order))
                            {
                                fields.Add(ModelAssembler.CreateField(entityFileTemplateField, entityFields.First(x => x.Id == entityFileTemplateField.FieldId)));
                            }

                            template.Rows.Add(new Row
                            {
                                Number = i,
                                Fields = fields
                            });
                        }

                        file.Templates.Add(template);

                    }
                    files.Add(file);
                }

                return files;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Obtener Archivo Por Identificador
        /// </summary>
        /// <param name="structureType">Identificador</param>
        /// <returns>Archivo</returns>
        public File GetFileByFileId(int fileId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            FileFieldView fileFieldView = new FileFieldView();
            ViewBuilder builder = new ViewBuilder("FileFieldView");

            filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.File.Properties.Id, typeof(COMMEN.File).Name);
            filter.Equal();
            filter.Constant(fileId);

            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, fileFieldView);
            }

            if (fileFieldView != null && fileFieldView.Files.Count > 0)
            {
                File file = ModelAssembler.CreateFile(fileFieldView.Files.Cast<COMMEN.File>().First());

                if (file.IsEnabled)
                {
                    file.Templates = new List<Template>();
                    ConcurrentBag<Template> templates = new ConcurrentBag<Template>();

                    List<COMMEN.FileTemplate> fileTemplates = fileFieldView.FileTemplates.Cast<COMMEN.FileTemplate>().ToList();
                    Parallel.ForEach(fileTemplates.OrderBy(x => x.Order), entityFileTemplate =>
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
                                ConcurrentBag<Field> fields = new ConcurrentBag<Field>();

                                Parallel.ForEach(fileFieldView.FileTemplateFields.Cast<COMMEN.FileTemplateField>().OrderBy(x => x.Order).Where(x => x.FileTemplateId == template.Id && x.RowPosition == i), entityFileTemplateField =>
                                {
                                    if (entityFileTemplateField.IsEnabled)
                                    {
                                        fields.Add(ModelAssembler.CreateField(entityFileTemplateField, entityFields.First(x => x.Id == entityFileTemplateField.FieldId)));
                                    }
                                });

                                template.Rows.Add(new Row
                                {
                                    Number = i,
                                    Fields = fields.OrderBy(x => x.Order).ToList()
                                });
                            }
                            templates.Add(template);
                        }
                    });

                    file.Templates = templates.OrderBy(x => x.Order).ToList();
                }

                return file;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Realiza los procesos del CRUD para el Archivo
        /// </summary>
        /// <param name="file">Archivo para crear</param>
        public string CreateFile(File file)
        {
            string returnResponse;
            int columnError = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        PrimaryKey key = COMMEN.File.CreatePrimaryKey(file.Id);
                        COMMEN.File entityFile = new COMMEN.File(file.Id);
                        entityFile = (COMMEN.File)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                        entityFile = EntityAssembler.CreateFile(file);
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(entityFile);


                        if (file.Templates != null)
                        {
                            foreach (Template item in file.Templates)
                            {
                                columnError = columnError + CreateFileTemplate(item, entityFile.Id);
                            }
                        }
                        if (columnError > 0)
                        {
                            transaction.Dispose();
                            returnResponse = "ColumnError";
                        }
                        else
                        {
                            transaction.Complete();
                            returnResponse = "Success";
                        }

                    }
                    catch (System.Exception ex)
                    {
                        transaction.Dispose();
                        returnResponse = "hasError";
                    }
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.CreateFile");
            return returnResponse;
        }

        /// <summary>
        /// Realiza los procesos del CRUD para el Archivo
        /// </summary>
        /// <param name="file">Archivo para actualizar</param>+
        public string UpdateFile(File file)
        {
            string returnResponse;
            int columnError = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {

                    try
                    {
                        PrimaryKey key = COMMEN.File.CreatePrimaryKey(file.Id);
                        COMMEN.File fileEntity = (COMMEN.File)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                        fileEntity.Description = file.Description;
                        fileEntity.SmallDescription = file.SmallDescription;
                        fileEntity.Observations = file.Observations;
                        fileEntity.FileTypeId = (int)file.FileType;
                        fileEntity.IsEnabled = file.IsEnabled;
                        DataFacadeManager.Instance.GetDataFacade().UpdateObject(fileEntity);

                        foreach (Template itemTemplate in file.Templates)
                        {
                            if (itemTemplate.parametrizationStatus == null)
                            {
                                int rowsNumber = 0;
                                int lastRow = 0;
                                foreach (Field itemField in itemTemplate.Rows[0].Fields)
                                {
                                    if (itemField.parametrizationStatus == ParametrizationStatus.Create)
                                    {
                                        CreateFileTemplateField(itemField, itemTemplate.Id);
                                    }
                                    if (itemField.parametrizationStatus == ParametrizationStatus.Update)
                                    {
                                        UpdateFileTemplateField(itemField, itemTemplate.Id);
                                    }
                                    if (itemField.parametrizationStatus == ParametrizationStatus.Delete)
                                    {
                                        DeleteFileTemplateField(itemField);
                                    }
                                    if (lastRow != itemField.RowPosition && itemField.parametrizationStatus != ParametrizationStatus.Delete)
                                    {
                                        rowsNumber++;
                                    }
                                    lastRow = itemField.RowPosition;
                                }
                                int rowBefore = 0;
                                for (int i = 1; i <= rowsNumber; i++)
                                {
                                    List<Field> tmpList = itemTemplate.Rows[0].Fields.Where(x => x.RowPosition == i).ToList();
                                    int ColumnNumber = tmpList.Sum(x => x.ColumnSpan);
                                    if (rowBefore != ColumnNumber && rowBefore != 0)
                                    {
                                        columnError++;
                                    }
                                    rowBefore = ColumnNumber;
                                }

                            }
                            if (itemTemplate.parametrizationStatus == ParametrizationStatus.Create)
                            {
                                columnError = columnError + CreateFileTemplate(itemTemplate, file.Id);
                            }
                            if (itemTemplate.parametrizationStatus == ParametrizationStatus.Update)
                            {
                                columnError = columnError + UpdateFileTemplate(itemTemplate, file.Id);
                            }
                            if (itemTemplate.parametrizationStatus == ParametrizationStatus.Delete)
                            {
                                DeleteFileTemplate(itemTemplate);
                            }
                        }
                        if (columnError > 0)
                        {
                            transaction.Dispose();
                            returnResponse = "ColumnError";
                        }
                        else
                        {
                            transaction.Complete();
                            returnResponse = "Success";
                        }

                    }
                    catch (System.Exception ex)
                    {
                        transaction.Dispose();
                        returnResponse = "hasError";
                    }
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.UpdateFile");
            return returnResponse;
        }

        private int CreateFileTemplate(Template template, int entityFileId)
        {
            //PrimaryKey key = COMMEN.FileTemplate.CreatePrimaryKey(template.Id);
            COMMEN.FileTemplate fileTemplateEntity = new COMMEN.FileTemplate(template.Id);
            //fileTemplateEntity = (COMMEN.FileTemplate)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            fileTemplateEntity = EntityAssembler.CreateFileTemplate(template, entityFileId);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(fileTemplateEntity);

            if (template.Rows != null)
            {
                if (template.Rows[0].Fields != null)
                {
                    int rowsNumber = 0;
                    int lastRow = 0;
                    foreach (Field item in template.Rows[0].Fields)
                    {
                        CreateFileTemplateField(item, fileTemplateEntity.Id);
                        if (lastRow != item.RowPosition)
                        {
                            rowsNumber++;
                        }
                        lastRow = item.RowPosition;
                    }
                    int ColumnNumber = template.Rows[0].Fields.Sum(x => x.ColumnSpan);
                    int result = ColumnNumber % rowsNumber;
                    if (result != 0)
                    {
                        return 1;
                    }
                }
            }
            return 0;
        }

        private int UpdateFileTemplate(Template template, int entityFileId)
        {
            PrimaryKey key = COMMEN.FileTemplate.CreatePrimaryKey(template.Id);
            COMMEN.FileTemplate fileTemplateEntity = (COMMEN.FileTemplate)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            fileTemplateEntity.FileId = entityFileId;
            fileTemplateEntity.IsMandatory = template.IsMandatory;
            fileTemplateEntity.IsEnabled = template.IsEnabled;
            fileTemplateEntity.Order = template.Order;
            fileTemplateEntity.IsPrincipal = template.IsPrincipal;
            fileTemplateEntity.TemplateId = template.TemplateId;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(fileTemplateEntity);

            int rowsNumber = 0;
            int lastRow = 0;
            foreach (Field item in template.Rows[0].Fields)
            {
                if (item.parametrizationStatus == ParametrizationStatus.Create)
                {
                    CreateFileTemplateField(item, item.Id);
                }
                if (item.parametrizationStatus == ParametrizationStatus.Update)
                {
                    UpdateFileTemplateField(item, item.Id);
                }
                if (item.parametrizationStatus == ParametrizationStatus.Delete)
                {
                    DeleteFileTemplateField(item);
                }
                if (lastRow != item.RowPosition && item.parametrizationStatus != ParametrizationStatus.Delete)
                {
                    rowsNumber++;
                }
                lastRow = item.RowPosition;
            }

            int rowBefore = 0;
            for (int i = 1; i <= rowsNumber; i++)
            {
                List<Field> tmpList = template.Rows[0].Fields.Where(x => x.RowPosition == i).ToList();
                int ColumnNumber = tmpList.Sum(x => x.ColumnSpan);
                if (rowBefore != ColumnNumber && rowBefore != 0)
                {
                    return 1;
                }
                rowBefore = ColumnNumber;
            }
            return 0;

        }

        private void DeleteFileTemplate(Template template)
        {
            foreach (Field item in template.Rows[0].Fields)
            {
                DeleteFileTemplateField(item);
            }
            PrimaryKey key = COMMEN.FileTemplate.CreatePrimaryKey(template.Id);
            COMMEN.FileTemplate fileTemplateEntity = (COMMEN.FileTemplate)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (fileTemplateEntity != null)
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(fileTemplateEntity);
            }
        }

        private void CreateFileTemplateField(Field field, int fileTemplateEntityId)
        {
            //PrimaryKey key = COMMEN.FileTemplateField.CreatePrimaryKey(field.TemplateFieldId);
            COMMEN.FileTemplateField fileTemplateFieldEntity = new COMMEN.FileTemplateField(field.TemplateFieldId);
            fileTemplateFieldEntity = EntityAssembler.CreateFileTemplateField(field, fileTemplateEntityId);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(fileTemplateFieldEntity);
        }

        private void UpdateFileTemplateField(Field field, int fileTemplateEntityId)
        {
            PrimaryKey key = COMMEN.FileTemplateField.CreatePrimaryKey(field.TemplateFieldId);
            COMMEN.FileTemplateField fileTemplateFieldEntity = (COMMEN.FileTemplateField)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            fileTemplateFieldEntity.FieldId = field.Id;
            fileTemplateFieldEntity.Order = field.Order;
            fileTemplateFieldEntity.ColumnSpan = field.ColumnSpan;
            fileTemplateFieldEntity.RowPosition = field.RowPosition;
            fileTemplateFieldEntity.IsMandatory = field.IsMandatory;
            fileTemplateFieldEntity.IsEnabled = field.IsEnabled;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(fileTemplateFieldEntity);
        }

        private void DeleteFileTemplateField(Field field)
        {
            PrimaryKey key = COMMEN.FileTemplateField.CreatePrimaryKey(field.TemplateFieldId);
            COMMEN.FileTemplateField fileTemplateFieldEntity = (COMMEN.FileTemplateField)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (fileTemplateFieldEntity != null)
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(fileTemplateFieldEntity);
            }
        }

        /// <summary>
        /// Crear Archivo
        /// </summary>
        /// <param name="file">Información</param>
        /// <returns>Ruta Archivo</returns>
        public string GenerateFile(File file)
        {
            string urlFile = "";

            switch (file.FileType)
            {
                case FileType.Excel:
                    urlFile = CreateExcelFile(file);
                    break;
                case FileType.CSV:
                    urlFile = CreateCSVFile(file);
                    break;
            }

            return urlFile;
        }

        /// <summary>
        /// Crear Archivo Excel
        /// </summary>
        /// <param name="file">Información</param>
        /// <returns>Información</returns>
        public string CreateExcelFile(File file)
        {
            try
            {
                string fileName = ConfigurationManager.AppSettings["ExternalFolderFiles"] + "\\" + file.Name + ".xlsx";

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
                                            ///<summary>
                                            ///Creación del caso FieldType.Int16, necesario para validar campos numéricos que no estaban 
                                            ///siendo tomados en cuenta al momento de generar el archivo Excel.
                                            ///</summary>
                                            ///<author>Diego Leon</author>
                                            ///<date>17/07/2018</date>
                                            ///<purpose>REQ_#079</purpose>
                                            ///<returns></returns>
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

        /// <summary>
        /// Crear Archivo CSV
        /// </summary>
        /// <param name="file">Información</param>
        /// <returns>Información</returns>
        private string CreateCSVFile(File file)
        {
            string fileName = ConfigurationManager.AppSettings["ExternalFolderFiles"] + "\\" + file.Name + ".csv";
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
        public FileProcessValue GetFileProcess(int typeLoadId, int riskTypeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.FileProcessValue.Properties.Key3, typeof(COMMEN.FileProcessValue).Name).Equal().Constant(typeLoadId);
            filter.And();
            filter.Property(COMMEN.FileProcessValue.Properties.Key5, typeof(COMMEN.FileProcessValue).Name).Equal().Constant(riskTypeId);
            //filter.Property(COMMEN.FileProcessValue.Properties.Key5, typeof(COMMEN.FileProcessValue).Name).IsNull();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.FileProcessValue), filter.GetPredicate()));

            return ModelAssembler.CreateFileProcessValue(businessCollection);
        }

        /// <summary>
        /// Obtener Archivo Por Filtros
        /// </summary>
        /// <param name="fileProcessValue">Filtros</param>
        /// <returns>Archivo</returns>
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
                return GetFileByFileId(businessCollection.Cast<COMMEN.FileProcessValue>().First().FileId);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener la información de la tabla FileProcessValue 
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>FileProcessValue</returns>
        public FileProcessValue GetFileProcessValue(int fileId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.FileProcessValue.Properties.FileId, typeof(COMMEN.FileProcessValue).Name).Equal().Constant(fileId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.FileProcessValue), filter.GetPredicate()));
            return ModelAssembler.CreateFileProcessValue(businessCollection);
        }

       
    }
}
