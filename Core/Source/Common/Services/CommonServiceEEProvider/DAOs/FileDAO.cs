using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.CommonServices.EEProvider.Entities.Views;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DOS = DocumentFormat.OpenXml.Spreadsheet;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using System.Diagnostics;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.Transactions;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using System.Text;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UtilitiesServicesEEProvider.Entities.Views;
using Sistran.Core.Application.UtilitiesServicesEEProvider.Helper;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
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
                            List<Field> fields = new List<Field>();

                            foreach (COMMEN.FileTemplateField entityFileTemplateField in fileFieldView.FileTemplateFields.Cast<COMMEN.FileTemplateField>().Where(x => x.FileTemplateId == template.Id && x.RowPosition == i))
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
                    catch (System.Exception)
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
                                    int status = int.Parse(itemField.parametrizationStatus.ToString()) == 0 ? int.Parse(ParametrizationStatus.Original.ToString()) : int.Parse(itemField.parametrizationStatus.ToString());
                                    ParametrizationStatus itemStatus = (ParametrizationStatus)status;

                                    if (itemStatus == ParametrizationStatus.Create)
                                    {
                                        CreateFileTemplateField(itemField, itemTemplate.Id);
                                    }
                                    if (itemStatus == ParametrizationStatus.Update)
                                    {
                                        UpdateFileTemplateField(itemField, itemTemplate.Id);
                                    }
                                    if (itemStatus == ParametrizationStatus.Delete)
                                    {
                                        DeleteFileTemplateField(itemField);
                                    }
                                    if (lastRow != itemField.RowPosition && itemStatus != ParametrizationStatus.Delete)
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

                            int statusTemplate = int.Parse(itemTemplate.parametrizationStatus.ToString()) == 0 ? int.Parse(ParametrizationStatus.Original.ToString()) : int.Parse(itemTemplate.parametrizationStatus.ToString());
                            ParametrizationStatus itemStatusTemplate = (ParametrizationStatus)statusTemplate;

                            if (itemStatusTemplate == ParametrizationStatus.Create)
                            {
                                columnError = columnError + CreateFileTemplate(itemTemplate, file.Id);
                            }
                            if (itemStatusTemplate == ParametrizationStatus.Update)
                            {
                                columnError = columnError + UpdateFileTemplate(itemTemplate, file.Id);
                            }
                            if (itemStatusTemplate == ParametrizationStatus.Delete)
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
                    catch (System.Exception)
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
                int status = int.Parse(item.parametrizationStatus.ToString()) == 0 ? int.Parse(ParametrizationStatus.Original.ToString()) : int.Parse(item.parametrizationStatus.ToString());
                ParametrizationStatus itemStatus = (ParametrizationStatus)status;

                if (itemStatus == ParametrizationStatus.Create)
                {
                    CreateFileTemplateField(item, item.Id);
                }
                if (itemStatus == ParametrizationStatus.Update)
                {
                    UpdateFileTemplateField(item, item.Id);
                }
                if (itemStatus == ParametrizationStatus.Delete)
                {
                    DeleteFileTemplateField(item);
                }
                if (lastRow != item.RowPosition && itemStatus != ParametrizationStatus.Delete)
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
                if (!System.IO.Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["ReportExportPath"]))
                {
                    System.IO.DirectoryInfo directoryInfo = System.IO.Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["ReportExportPath"]);
                }

                string fileName = ConfigurationManager.AppSettings["ReportExportPath"] + "\\" + file.Name + ".xlsx";

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
                        worksheetPart.Worksheet = new DOS.Worksheet();

                        DOS.SheetData sheetData = new DOS.SheetData();
                        worksheetPart.Worksheet = new DOS.Worksheet(sheetData);

                        DOS.MergeCells mergeCells = new DOS.MergeCells();
                        
                        workbookPart.Workbook.Save();
                        int headersCount = template.Rows.Where(r=> r.Fields != null && r.Fields.Count > 0).Max(r => r.Fields.Max(x => x.RowPosition));
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
                     
                        foreach (Row row in template.Rows.Where(r=> r.Fields != null))
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
                                            case FieldType.String:
                                            case FieldType.DateTime:
                                            case FieldType.Decimal:
                                            case FieldType.Int8:
                                            case FieldType.Int16:
                                            case FieldType.Int32:
                                            case FieldType.Int64:
                                                fileRow.Append(FileHelper.ConstructCell(field.Value.ToString(), DOS.CellValues.String));
                                                break;
                                            case FieldType.Boolean:
                                                string fieldBool = string.Empty;
                                                fieldBool = field.Value.ToString().ToLower() == "true" ? "SI" : "NO";
                                                fileRow.Append(FileHelper.ConstructCell(fieldBool, DOS.CellValues.String));
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
                        sheetId++;
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
            if (!System.IO.Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["ReportExportPath"]))
            {
                System.IO.DirectoryInfo directoryInfo = System.IO.Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["ReportExportPath"]);
            }

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

        /// <summary>
        /// Genera archivo excel subramo técnico
        /// </summary>
        /// <param name="subLinebusiness"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToSubLinebusiness(List<SubLineBusiness> subLinebusiness, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ParametrizationSubLineBusiness;

            File file = GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (SubLineBusiness sublinebusiness in subLinebusiness)
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

                    fields[0].Value = sublinebusiness.LineBusinessId.ToString();
                    fields[1].Value = sublinebusiness.LineBusinessDescription;
                    fields[2].Value = sublinebusiness.Id.ToString();
                    fields[3].Value = sublinebusiness.Description;
                    fields[4].Value = sublinebusiness.SmallDescription;

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                return GenerateFile(file);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Genera archivo excel ramo comercial
        /// </summary>
        /// <param name="Prefix"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToPrefix(List<Prefix> Prefix, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ParametrizationPrefix;

            File file = GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();
                foreach (Prefix Prefixes in Prefix)
                {
                    if (Prefixes.LineBusiness != null && Prefixes.LineBusiness.Count > 0)
                    {
                        foreach (LineBusiness linebusiness in Prefixes.LineBusiness)
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
                            fields[0].Value = Prefixes.Id.ToString();
                            fields[1].Value = Prefixes.Description;
                            fields[2].Value = Prefixes.SmallDescription;
                            fields[3].Value = Prefixes.TinyDescription;
                            fields[4].Value = Prefixes.PrefixType.Description;
                            fields[5].Value = linebusiness.Description;

                            rows.Add(new Row
                            {
                                Fields = fields,
                            });
                        }
                    }
                    else
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
                        fields[0].Value = Prefixes.Id.ToString();
                        fields[1].Value = Prefixes.Description;
                        fields[2].Value = Prefixes.SmallDescription;
                        fields[3].Value = Prefixes.TinyDescription;
                        fields[4].Value = Prefixes.PrefixType.Description;
                        fields[5].Value = " ";

                        rows.Add(new Row
                        {
                            Fields = fields,
                        });
                    }
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                return GenerateFile(file);
            }
            else
            {
                return "";
            }
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
        /// Genera archivo excel sucursales
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToBranch(List<Branch> branch, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ParametrizationBranch;

            File file = GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (Branch branchs in branch)
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

                    fields[0].Value = branchs.Id.ToString();
                    fields[1].Value = branchs.SmallDescription;
                    fields[2].Value = branchs.Description;

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                return GenerateFile(file);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Genera archivo excel ramo técnico
        /// </summary>
        /// <param name="linebusiness"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileLinebusiness(List<LineBusiness> linebusiness, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ParametrizationLineBusiness;

            File file = GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (LineBusiness LineBusiness in linebusiness)
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

                    fields[0].Value = LineBusiness.Id.ToString();
                    fields[1].Value = LineBusiness.Description;
                    fields[2].Value = LineBusiness.ShortDescription;
                    fields[3].Value = LineBusiness.TyniDescription;
                    fields[4].Value = LineBusiness.ReportLineBusiness.ToString();
                    //if (LineBusiness.ListLineBusinessCoveredrisktype != null && LineBusiness.ListLineBusinessCoveredrisktype.Count > 0)
                    //{
                    //    fields[5].Value = LineBusiness.ListLineBusinessCoveredrisktype.First().IdRiskType.ToString();
                    //}
                    //else
                    //{
                        fields[5].Value = " ";
                    //}
                    //if (LineBusiness.ListClausesLineBusiness != null)
                    //{
                    //    foreach (var item in LineBusiness.ListClausesLineBusiness)
                    //    {
                    //        fields[6].Value = item.DescriptionClauseByLineBusiness;
                    //    }
                    //}
                    //else
                    //{
                    //    fields[6].Value = " ";
                    //}
                    //if (LineBusiness.ListInsurectObjects != null)
                    //{
                    //    foreach (var item in LineBusiness.ListInsurectObjects)
                    //    {
                    //        fields[7].Value = item.ToString();
                    //    }
                    //}
                    //else
                    //{
                    //    fields[7].Value = " ";
                    //}
                    //if (LineBusiness.ListProtections != null)
                    //{
                    //    foreach (var item in LineBusiness.ListProtections)
                    //    {
                    //        fields[8].Value = item.IdPeril.ToString();
                    //    }
                    //}
                    //else
                    //{
                    //    fields[8].Value = " ";
                    //}

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }
                file.Templates[0].Rows = rows;
                List<Row> rowsClause = new List<Row>();

                foreach (LineBusiness LineBusiness in linebusiness)
                {
                    LineBusinessClausesView lbc = LineBusinessClausesByLineBusinessId(LineBusiness.Id);



                    foreach (var item in lbc.Clause.Cast<QUOEN.Clause>())
                    {

                        var fields = file.Templates[1].Rows[0].Fields.Select(x => new Field
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

                        fields[0].Value = LineBusiness.Id.ToString();
                        fields[1].Value = LineBusiness.Description;
                        fields[2].Value = item.ClauseId.ToString();
                        fields[3].Value = item.ClauseName;
                        fields[4].Value = item.ClauseTitle;

                        rowsClause.Add(new Row
                        {
                            Fields = fields
                        });
                    }
                }
                file.Templates[1].Rows = rowsClause;
                List<Row> rowsInsuredObject = new List<Row>();

                foreach (LineBusiness LineBusiness in linebusiness)
                {
                    LineBusinessInsuredObjectPerilView lb = LineBusinessInsuredObjectPerilByLineBusinessId(LineBusiness.Id);



                    foreach (var io in lb.InsuredObject.Cast<QUOEN.InsuredObject>())
                    {
                        foreach (var p in lb.Peril.Cast<QUOEN.Peril>())
                        {

                            var fields = file.Templates[2].Rows[0].Fields.Select(x => new Field
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


                            fields[0].Value = LineBusiness.Id.ToString();
                            fields[1].Value = LineBusiness.Description;
                            fields[2].Value = io.InsuredObjectId.ToString();
                            fields[3].Value = io.Description;
                            fields[4].Value = p.PerilCode.ToString();
                            fields[5].Value = p.Description;

                            rowsInsuredObject.Add(new Row
                            {
                                Fields = fields
                            });
                        }
                    }
                }
                file.Templates[2].Rows = rowsInsuredObject;

                file.Name = string.Format(fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy"));
                return GenerateFile(file);
            }
            else
            {
                return "";
            }
        }

        private LineBusinessClausesView LineBusinessClausesByLineBusinessId(int lineBusinessId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(COMMEN.LineBusiness.Properties.LineBusinessCode, typeof(COMMEN.LineBusiness).Name);
            filter.Equal();
            filter.Constant(lineBusinessId);
            filter.And();
            filter.Property(QUOEN.Clause.Properties.ConditionLevelCode, typeof(QUOEN.Clause).Name);
            filter.Equal();
            filter.Constant(4);//LineBusiness

            LineBusinessClausesView view = new LineBusinessClausesView();
            ViewBuilder builder = new ViewBuilder("LineBusinessClausesView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            return view;
        }

        private LineBusinessInsuredObjectPerilView LineBusinessInsuredObjectPerilByLineBusinessId(int lineBusinessId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(COMMEN.LineBusiness.Properties.LineBusinessCode, typeof(COMMEN.LineBusiness).Name);
            filter.Equal();
            filter.Constant(lineBusinessId);

            LineBusinessInsuredObjectPerilView view = new LineBusinessInsuredObjectPerilView();
            ViewBuilder builder = new ViewBuilder("LineBusinessInsuredObjectPerilView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            return view;
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
