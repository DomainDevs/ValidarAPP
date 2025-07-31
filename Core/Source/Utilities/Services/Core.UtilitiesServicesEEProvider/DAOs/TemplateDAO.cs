using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.UtilitiesServicesEEProvider.Assemblers;
using Sistran.Core.UtilitiesServicesEEProvider.Resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs
{
    public class TemplateDAO
    {
        private List<int> fieldsIdsAllowsZero;
        private List<int> fieldsIdsAllowsSpecialCharacter;
        private List<int> fieldsTextTypeAllowsNumericOnly;
        private List<ValidationRegularExpression> validateRegularExpressions;

        /// <summary>
        /// Obtiene la lista de plantillas
        /// </summary>
        /// <returns>Lista de plantillas consultadas</returns>
        public List<Template> GetTemplates()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.Template)));
            return ModelAssembler.CreateTemplates(businessCollection);
        }

        /// <summary>
        /// Obtiene la lista de campos
        /// </summary>
        /// <returns>Lista de campos consultadas</returns>
        public List<Field> GetFields()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.Field)));
            return ModelAssembler.CreateFields(businessCollection);
        }

        /// <summary>
        /// Realiza los procesos del CRUD para las Plantillas
        /// </summary>
        /// <param name="listAdded"> Lista de templates(plantillas) para ser agregadas</param>
        /// <param name="listEdited">Lista de templates(plantillas) para ser modificadas</param>
        /// <param name="listDeleted">Lista de templates(plantillas) para ser eliminadas</param>
        /// <returns>Respuesta con el total de procesos realizados </returns>
        public ParametrizationResponse<Template> SaveTemplates(List<Template> templatesAdded, List<Template> templatesEdited, List<Template> templatesDeleted)
        {
            ParametrizationResponse<Template> returnTemplates = new ParametrizationResponse<Template>();
            using (Context.Current)
            {
                #region Agregar Plantillas
                if (templatesAdded != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (Template item in templatesAdded)
                            {
                                PrimaryKey key = COMMEN.Template.CreatePrimaryKey(item.Id);
                                COMMEN.Template entityFile = new COMMEN.Template(item.Id);
                                entityFile = (COMMEN.Template)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                entityFile = EntityAssembler.CreateTemplate(item);

                                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityFile);
                            }
                            transaction.Complete();
                            returnTemplates.TotalAdded = templatesAdded.Count;
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnTemplates.ErrorAdded = "ErrorSaveTemplatesAdded";
                        }
                    }
                }

                #endregion

                #region Modificar Plantillas
                if (templatesEdited != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (var item in templatesEdited)
                            {
                                PrimaryKey key = COMMEN.Template.CreatePrimaryKey(item.Id);
                                COMMEN.Template templateEntity = (COMMEN.Template)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                templateEntity.Description = item.Description;
                                templateEntity.PropertyName = item.PropertyName;

                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(templateEntity);
                            }
                            transaction.Complete();
                            returnTemplates.TotalModify = templatesEdited.Count;
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnTemplates.ErrorModify = "ErrorSaveTemplatesEdited";
                        }
                    }
                }
                #endregion

                #region Borrar Plantillas
                if (templatesDeleted != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                            filter.Property(COMMEN.Template.Properties.Id).In().ListValue();
                            templatesDeleted.ForEach(x => filter.Constant(x.Id));
                            filter.EndList();
                            DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(COMMEN.Template), filter.GetPredicate());
                            transaction.Complete();
                            returnTemplates.TotalDeleted = templatesDeleted.Count;
                        }
                        catch (ForeignKeyException)
                        {
                            transaction.Dispose();
                            returnTemplates.ErrorDeleted = "ErrorSaveTemplatesRelated";
                        }
                        catch (RelatedObjectException)
                        {
                            transaction.Dispose();
                            returnTemplates.ErrorDeleted = "ErrorSaveTemplatesRelated";
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnTemplates.ErrorDeleted = "ErrorSaveTemplatesDeleted";
                        }
                    }
                }

                #endregion
                returnTemplates.ReturnedList = GetTemplates();
            }
            return returnTemplates;
        }

        /// <summary>
        /// Obtener Valor De Un Campo
        /// </summary>
        /// <typeparam name="T">Tipo De Dato</typeparam>
        /// <param name="field">Campo</param>
        /// <returns>Valor</returns>
        public object GetValueByField<T>(Field field)
        {
            if (field != null)
            {
                if (!string.IsNullOrEmpty(field.Value))
                {
                    try
                    {
                        return (T)Convert.ChangeType(field.Value, typeof(T));
                    }
                    catch (FormatException ex)
                    {
                        if (typeof(T).Name.Contains("Int"))
                        {
                            return default(T);
                        }
                        throw ex;
                    }
                }
                else
                {
                    if (typeof(T).Name == "String")
                    {
                        return "";
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
            else
            {
                if (typeof(T).Name == "String")
                {
                    return "";
                }
                else
                {
                    return default(T);
                }
            }
        }




        /// <summary>
        /// Validar Archivo
        /// </summary>
        /// <param name="file">Archivo</param>
        /// <param name="userName">Nombre Usuario</param>
        /// <returns>Archivo</returns>
        public File ValidateFile(File file, string userName)
        {
            ConcurrentBag<Template> existingTemplates = new ConcurrentBag<Template>();

            ParallelHelper.ForEach(file.Templates, (template) =>
            {
                DataTable dataTemplate = GetHeaderTemplateByFileNameTemplateNameUserName(file.Name, template.Description, userName, template.Rows.Last().Fields.Count);

                if (dataTemplate != null)
                {

                    if (dataTemplate.Rows.Count > template.Rows.Count
                        && (!string.IsNullOrEmpty(dataTemplate.Rows[template.Rows.Count][0].ToString())
                        //Se agrega esta validación ya que para colectivas el primer campo de la primera columna puede 
                        //que venga nulo ya que es el id del asegurado
                        || !string.IsNullOrEmpty(dataTemplate.Rows[template.Rows.Count][1].ToString())))
                    {
                        if (dataTemplate.Columns.Count >= template.Rows.Last().Fields.Count)
                        {
                            StringBuilder stringBuilder = new StringBuilder();
                            int index = 0;

                            foreach (Field field in template.Rows.Last().Fields)
                            {
                                if (field.Description.ToUpper().RemoveAccent().RemoveMandatoryString() != dataTemplate.Rows[template.Rows.Count - 1][index].ToString().ToUpper().RemoveAccent().RemoveMandatoryString())
                                //if (EncodingManager.RemoveAccent(field.Description.ToUpper()) != EncodingManager.RemoveAccent(dataTemplate.Rows[template.Rows.Count - 1][index].ToString().ToUpper()))
                                {
                                    template.HasError = true;
                                    stringBuilder.Append(string.Format(Errors.ErrorFieldNotMatch, field.Description, template.Description)).Append(", ");
                                }

                                index++;
                            }

                            if (template.HasError)
                            {
                                template.ErrorDescription = stringBuilder.ToString().Substring(0, stringBuilder.Length - 2);
                            }

                            existingTemplates.Add(template);
                        }
                        else
                        {
                            template.HasError = true;
                            template.ErrorDescription = string.Format(Errors.ErrorTemplateNotMatch, template.Description);
                            existingTemplates.Add(template);
                        }
                    }
                    else if (template.IsMandatory)
                    {
                        template.HasError = true;
                        template.ErrorDescription = string.Format(Errors.ErrorTemplateIsMandatoryData, template.Description);
                        existingTemplates.Add(template);
                    }
                }
                else if (template.IsMandatory)
                {
                    template.HasError = true;
                    template.ErrorDescription = string.Format(Errors.ErrorTemplateIsMandatory, template.Description);
                    existingTemplates.Add(template);
                }
            });

            if (existingTemplates.Any(x => x.HasError))
            {
                string formatedError = string.Join(", ", existingTemplates.Where(x => x.HasError).Select(y => y.ErrorDescription));
                formatedError = Errors.ErrorFileLoaded + " " + formatedError;

                int maxLength = formatedError.Length;
                if (maxLength > 300)
                {
                    formatedError = formatedError.Substring(0, 300) + "...";
                }
                throw new BusinessException(formatedError);
            }

            file.Templates = existingTemplates.ToList();
            return file;
        }

        /// <summary>
        /// Validar Datos Archivo
        /// </summary>
        /// <param name="file">Archivo</param>
        /// <param name="userName">Nombre Usuario</param>
        /// <returns>Archivo</returns>
        public File ValidateDataFile(File file, string userName)
        {
            ParallelHelper.ForEach(file.Templates, (template) =>
            {
                DataTable dataTemplate = GetDataTemplateByFileNameTemplateNameUserName(file.Name, template.Description, userName, template.Rows.Last().Fields.First().Description);
                template.Rows = ValidateRows(dataTemplate, template.Rows.Last().Fields, template.Description);
                ValidateIdentifierTemplate(template);
            });

            return file;
        }

        /// <summary>
        /// Validar Datos Plantilla
        /// </summary>
        /// <param name="fileName">Nombre Archivo</param>
        /// <param name="userName">Nombre Usuario</param>
        /// <param name="template">Plantilla</param>
        /// <returns>Plantilla</returns>
        public Template ValidateDataTemplate(string fileName, string userName, Template template)
        {
            DataTable dataTemplate = GetDataTemplateByFileNameTemplateNameUserName(fileName, template.Description, userName, template.Rows.Last().Fields.First().Description);
            template.Rows = ValidateRows(dataTemplate, template.Rows.Last().Fields, template.Description);

            return template;
        }
        /// <summary>
        /// Valida Identificadores Plantilla
        /// </summary>
        /// <param name="template">Plantilla</param>
        private void ValidateIdentifierTemplate(Template template)
        {
            if (template != null & template.IsPrincipal)
            {
                var emptyIdentifiers = template?.Rows?.Where(x => x?.Fields?.Where(z => z.PropertyName != null && z.PropertyName == FieldPropertyName.Identificator && string.IsNullOrEmpty(z.Value)).Count() > 0);
                if (emptyIdentifiers.Count() > 1)
                {
                    foreach (var item in emptyIdentifiers)
                    {
                        item.HasError = true;
                        item.ErrorDescription += string.Format(Errors.ErrorFieldIsMandatory + KeySettings.ReportErrorSeparatorMessage(), item.Fields.First(u => u.PropertyName == FieldPropertyName.Identificator).Value, template.Description);
                    }
                }

                var identifiers = template?.Rows?.Where(r => !r.HasError).SelectMany(x => x?.Fields?.Where(z => z.PropertyName != null && z.PropertyName == FieldPropertyName.Identificator));

                if (identifiers != null && identifiers.Count() != 0)
                {


                    var consolidatedIdentifiers = template?.Rows?.Where(r => !r.HasError).SelectMany(x => x?.Fields?.Where(z => z.PropertyName != null && z.PropertyName == FieldPropertyName.Identificator)).GroupBy(x => x.Value).Where(z => z.Count() > 1).Select(x => new { Id = Convert.ToInt32(x.Key), Total = x.Key.Count() });
                    if (consolidatedIdentifiers != null && consolidatedIdentifiers.Count() > 0)
                    {
                        ParallelHelper.ForEach(template.Rows, row =>
                        {

                            var identifier = consolidatedIdentifiers.FirstOrDefault(z => z.Id == Convert.ToDecimal(row.Fields.First(y => y.PropertyName == FieldPropertyName.Identificator).Value))?.Id;
                            if (row.Fields != null && identifier.HasValue)
                            {
                                row.HasError = true;
                                row.ErrorDescription += String.Format("{0} : {1}", Errors.ErrorDuplicateIdentifiers, row.Fields.First(u => u.PropertyName == FieldPropertyName.Identificator).Value);
                            }

                        });
                    }
                }
            }
        }

        /// <summary>
        /// Obtener Datos De Plantillas Relacionados Por Identificador
        /// </summary>
        /// <param name="templates">Plantillas</param>
        /// <returns>Datos</returns>
        public List<File> GetDataTemplates(List<Template> templates)
        {
            List<File> files = new List<File>();
            Template templatePrincipal = templates.First(x => x.IsPrincipal);

            foreach (Row row in templatePrincipal.Rows)
            {
                if (row.HasError)
                {
                    File file = new File
                    {
                        Id = (int)GetValueByField<int>(row.Fields.First(x => x.PropertyName == FieldPropertyName.Identificator)),
                        Templates = new List<Template>()
                    };

                    file.Templates.Add(new Template
                    {
                        PropertyName = templatePrincipal.PropertyName,
                        IsPrincipal = templatePrincipal.IsPrincipal,
                        Description = templatePrincipal.Description,
                        Rows = new List<Row>()
                    });

                    file.Templates[0].Rows.Add(row);
                    files.Add(file);
                }
                else
                {
                    File file = new File
                    {
                        Id = (int)GetValueByField<int>(row.Fields.First(x => x.PropertyName == FieldPropertyName.Identificator)),
                        Templates = new List<Template>()
                    };

                    file.Templates.Add(new Template
                    {
                        PropertyName = templatePrincipal.PropertyName,
                        IsPrincipal = templatePrincipal.IsPrincipal,
                        Description = templatePrincipal.Description,
                        Rows = new List<Row>()
                    });

                    foreach (Template template in templates.Where(x => !x.IsPrincipal))
                    {
                        Template newTemplate = new Template
                        {
                            PropertyName = template.PropertyName,
                            Description = template.Description
                        };

                        if (template.Rows.Any(x => x.Fields.Exists(y => y.PropertyName == FieldPropertyName.Identificator)))
                        {
                            newTemplate.Rows = template.Rows.Where(x => x.Fields.First(y => y.PropertyName == FieldPropertyName.Identificator).Value == file.Id.ToString()).ToList();
                        }
                        else // Funciona sólo para la póliza de colectivas, que no tiene un identificador al ser única. JSCL - 17/11/2017
                        {
                            newTemplate.Rows = new List<Row>() { template.Rows.FirstOrDefault() };
                        }

                        if (newTemplate.Rows.Count > 0)
                        {
                            file.Templates.Add(newTemplate);
                        }
                    }

                    file.Templates[0].Rows.Add(row);
                    files.Add(file);
                }
            }

            return files;
        }

        /// <summary>
        /// Obtener Encabezados De Una Plantilla
        /// </summary>
        /// <param name="fileName">Nombre Archivo</param>
        /// <param name="templateName">Nombre Plantilla</param>
        /// <param name="userName">Nombre Usuario</param>
        /// <param name="countHeaders">Cantidad Encabezados</param>
        /// <returns>Encabezados</returns>
        private DataTable GetHeaderTemplateByFileNameTemplateNameUserName(string fileName, string templateName, string userName, int countHeaders)
        {
            OleDbConnection oleDbConnection = new OleDbConnection();

            try
            {
                string filePath = ConfigurationManager.AppSettings["ExternalFolderFiles"] + @"\" + userName + @"\" + fileName;
                oleDbConnection.ConnectionString = string.Format(ConfigurationManager.ConnectionStrings["ExcelConnection"].ConnectionString, filePath);
                oleDbConnection.Open();

                OleDbDataAdapter oleDbDataAdapter = null;
                StringBuilder command = new StringBuilder();
                countHeaders++;
                command.Append("Select Top " + countHeaders + " * From [" + templateName + "$]");

                oleDbDataAdapter = new OleDbDataAdapter(command.ToString(), oleDbConnection);

                DataTable dataTemplate = new DataTable(templateName);

                oleDbDataAdapter.Fill(dataTemplate);

                return dataTemplate;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                oleDbConnection.Close();
                oleDbConnection.Dispose();
            }
        }

        /// <summary>
        /// Obtener Datos De Una Plantilla
        /// </summary>
        /// <param name="fileName">Nombre Archivo</param>
        /// <param name="templateName">Nombre Plantilla</param>
        /// <param name="userName">Nombre Usuario</param>
        /// <param name="firstPropertyName">Primer Campo</param>
        /// <returns>Datos</returns>
        private DataTable GetDataTemplateByFileNameTemplateNameUserName(string fileName, string templateName, string userName, string firstPropertyName)
        {
            if (String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(templateName))
            {
                throw new ArgumentException(Errors.ErrorTemplateName);
            }

            OleDbConnection oleDbConnection = new OleDbConnection();

            try
            {
                string filePath = ConfigurationManager.AppSettings["ExternalFolderFiles"] + @"\" + userName + @"\" + fileName;
                oleDbConnection.ConnectionString = string.Format(ConfigurationManager.ConnectionStrings["ExcelConnection"].ConnectionString, filePath);
                oleDbConnection.Open();

                OleDbDataAdapter oleDbDataAdapter = null;

                // Se agrega este parámetro para el caso del cargue de colectivas que puede tener la primera columna nula
                string collectiveFilter = "OR [F2] IS NOT NULL";
                string command = string.Format("Select * From [{0}$] WHERE [F1] IS NOT NULL {1}", templateName, collectiveFilter);

                oleDbDataAdapter = new OleDbDataAdapter(command, oleDbConnection);

                DataTable dataTemplate = new DataTable(templateName);
                try
                {
                    oleDbDataAdapter.Fill(dataTemplate);
                    if (dataTemplate != null && dataTemplate.Rows.Count > 0)
                    {
                        bool deleteHeaders = true;

                        while (deleteHeaders)
                        {
                            if (dataTemplate.Rows.Count == 0)
                            {
                                deleteHeaders = false;
                            }
                            else if (dataTemplate.Rows.Count > 0 && dataTemplate.Rows[0][0].ToString().ToUpper() == firstPropertyName.ToUpper())
                            {
                                deleteHeaders = false;
                            }
                            if (dataTemplate.Rows.Count > 0)
                                dataTemplate.Rows.RemoveAt(0);

                        }
                    }

                    return dataTemplate;
                }
                catch (Exception e)
                {
                    throw new Exception(StringHelper.ConcatenateString(Errors.ErrorTempleteNotExist, ": ", templateName));
                }

            }
            catch (Exception e)
            {
                throw new Exception(StringHelper.ConcatenateString("Error In UtilitesService TemplateDAO: ", templateName, "InnerMessage:", e.Message));
            }
            finally
            {
                oleDbConnection.Close();
                oleDbConnection.Dispose();
            }
        }

        /// <summary>
        /// Validar Filas
        /// </summary>
        /// <param name="dataTable">Datos</param>
        /// <param name="fields">Campos</param>
        /// <returns>Filas</returns>
        private List<Row> ValidateRows(DataTable dataTable, List<Field> fields, string templateDescription)
        {
            List<Row> rows = new List<Row>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Row row = new Row
                {
                    Number = i,
                    Fields = new List<Field>()
                };

                for (int j = 0; j < fields.Count; j++)
                {
                    Field field = new Field
                    {
                        FieldType = fields[j].FieldType,
                        Description = fields[j].Description,
                        IsMandatory = fields[j].IsMandatory,
                        PropertyName = fields[j].PropertyName,
                        PropertyLength = fields[j].PropertyLength,
                        ColumnSpan = fields[j].ColumnSpan,
                        Id = fields[j].Id,
                        IsEnabled = fields[j].IsEnabled,
                        Order = fields[j].Order,
                        RowPosition = fields[j].RowPosition,
                        SmallDescription = fields[j].SmallDescription,
                        Value = dataTable.Rows[i][j].ToString().Trim()
                    };

                    ValidateField(field, row, templateDescription);

                    row.Fields.Add(field);
                }

                rows.Add(row);
            }

            return rows;
        }

        /// <summary>
        /// Validar Campo
        /// </summary>
        /// <param name="field">Campo</param>
        /// <param name="row">Fila</param>
        private void ValidateField(Field field, Row row, string templateDescription)
        {
            if (!string.IsNullOrEmpty(field.Value))
            {
                switch (field.FieldType)
                {
                    case FieldType.Int8:
                        ValidateInt8(field, row, templateDescription);
                        break;
                    case FieldType.Int16:
                        ValidateInt16(field, row, templateDescription);
                        break;
                    case FieldType.Int32:
                        ValidateInt32(field, row, templateDescription);
                        break;
                    case FieldType.Int64:
                        ValidateInt64(field, row, templateDescription);
                        break;
                    case FieldType.Decimal:
                        ValidateDecimal(field, row, templateDescription);
                        break;
                    case FieldType.String:
                        field.Value = Regex.Replace(field.Value, @"[^\u0000-\u00ff]", " ");
                        ValidateString(field, row, templateDescription);
                        break;
                    case FieldType.DateTime:
                        ValidateDateTime(field, row, templateDescription);
                        break;
                    case FieldType.Boolean:
                        ValidateBoolean(field, row, templateDescription);
                        break;
                }
            }
            else if (field.IsMandatory)
            {
                row.HasError = true;
                row.ErrorDescription += string.Format(Errors.ErrorFieldIsMandatory + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription);
            }
        }

        private void ValidateInt8(Field field, Row row, string templateDescription)
        {
            if (!(field.Value == "0" && FieldsIdsAllowsZero.Contains(field.Id)))
            {
                Int16 intValue = 0;
                Int16.TryParse(field.Value, out intValue);

                if (intValue == 0)
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorOnlyAllowsInt8 + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription);
                }
                else if (intValue < 0)
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorNegativeValuesAreNotAccepted + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription);
                }
                else if ((field.PropertyLength != null && Convert.ToInt16(field.PropertyLength) < field.Value.Length) || field.Value.Length > 255)
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorLengthOfDigits + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription, field.PropertyLength);
                }
            }
        }

        private void ValidateInt16(Field field, Row row, string templateDescription)
        {
            if (!(field.Value == "0" && FieldsIdsAllowsZero.Contains(field.Id)))
            {
                Int16 intValue = 0;
                Int16.TryParse(field.Value, out intValue);

                if (intValue == 0)
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorOnlyAllowsInt16 + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription);
                }
                else if (intValue < 0)
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorNegativeValuesAreNotAccepted + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription);
                }
                else if (field.PropertyLength != null && Convert.ToInt32(field.PropertyLength) < field.Value.Length)
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorLengthOfDigits + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription, field.PropertyLength);
                }
            }
        }

        /// <summary>
        /// Validar Enteros 32 Bits
        /// </summary>
        /// <param name="field">Valor</param>
        private void ValidateInt32(Field field, Row row, string templateDescription)
        {
            if (!(field.Value == "0" && FieldsIdsAllowsZero.Contains(field.Id)))
            {
                Int32 intValue = 0;
                Int32.TryParse(field.Value, out intValue);

                if (intValue == 0)
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorOnlyAllowsInt32 + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription);
                }
                else if (intValue < 0)
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorNegativeValuesAreNotAccepted + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription);
                }
                else if (field.PropertyLength != null  && Convert.ToInt32(field.PropertyLength) < field.Value.Length)
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorLengthOfDigits + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription, field.PropertyLength);
                }
            }
        }

        /// <summary>
        /// Validar Enteros 64 Bits
        /// </summary>
        /// <param name="field">Valor</param>
        private void ValidateInt64(Field field, Row row, string templateDescription)
        {
            if (!(field.Value == "0" && FieldsIdsAllowsZero.Contains(field.Id)))
            {
                Int64 intValue = 0;
                Int64.TryParse(field.Value, out intValue);

                if (intValue == 0)
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorOnlyAllowsInt64 + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription);
                }
                else if (intValue < 0)
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorNegativeValuesAreNotAccepted + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription);
                }
                else if (field.PropertyLength != null && Convert.ToInt64(field.PropertyLength) < field.Value.Length)
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorLengthOfDigits + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription, field.PropertyLength);
                }
            }
        }

        /// <summary>
        /// Validar Decimales
        /// </summary>
        /// <param name="field">Valor</param>
        private void ValidateDecimal(Field field, Row row, string templateDescription)
        {
            if (field.Value != "0")
            {
                string[] sizeParameter = field.PropertyLength.Split(',');
                decimal decimalValue = 0;

                if (!decimal.TryParse(field.Value, out decimalValue))
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorOnlyAllowsDecimal + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription);
                }
                else if (decimalValue < 0)
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorNegativeValuesAreNotAccepted + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription);
                }
                else
                {
                    if (field.PropertyLength != null)
                    {

                        string[] sizeProperty = field.Value.Split(',');

                        int sizeParameterLeft = Convert.ToInt32(sizeParameter[0]);
                        int sizeParameterRigth = 0;
                        int sizePropertyRigth = 0;

                        if (sizeParameter.Length == 2)
                        {
                            sizeParameterRigth = Convert.ToInt32(sizeParameter[1]);
                            sizeParameterLeft -= sizeParameterRigth;
                        }

                        if (sizeProperty.Length == 2)
                        {
                            sizePropertyRigth = sizeProperty[1].Length;
                        }

                        if (sizeProperty[0].Length > sizeParameterLeft || sizePropertyRigth > sizeParameterRigth)
                        {
                            row.HasError = true;
                            row.ErrorDescription += string.Format(Errors.ErrorOnlyAllowsDecimal + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription, sizeParameterLeft, sizeParameterRigth);
                        }
                    }
                    else
                    {
                        row.HasError = true;
                        row.ErrorDescription += string.Format(Errors.ErrorDecimalPropertyLength + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription);
                    }
                }
            }
        }

        /// <summary>
        /// Validar Textos
        /// </summary>
        /// <param name="field">Valor</param>
        private void ValidateString(Field field, Row row, string templateDescription)
        {
            if (FieldsTextTypeAllowsNumericOnly.Contains(field.Id))
            {
                ValidateInt64(field, row, templateDescription);
            }
            else if (!(FieldsIdsAllowsSpecialCharacter.Contains(field.Id)))
            {
                Match match = Regex.Match(field.Value, ValidateRegularExpressions.Find(x => x.Id == (int)RegularExpression.SpecialCharacter).ParameterValue, RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorDoesntAllowSpecialCharacters, field.Description, templateDescription);
                }
            }
            if (!string.IsNullOrEmpty(field.PropertyLength))
            {
                if (field.Value.Length > Convert.ToInt32(field.PropertyLength))
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorStringLength + KeySettings.ReportErrorSeparatorMessage(), field.Description, templateDescription);
                }
            }
        }

        /// <summary>
        /// Validar Fechas
        /// </summary>
        /// <param name="field">Valor</param>
        private void ValidateDateTime(Field field, Row row, string templateDescription)
        {

            Match match = Regex.Match(field.Value, ValidateRegularExpressions.Find(x => x.Id == (int)RegularExpression.date).ParameterValue, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                row.HasError = true;
                row.ErrorDescription += string.Format(Errors.ErrorOnlyAllowsDateTime, field.Description, templateDescription);
            }
            else
            {
                DateTime dateTimeValue = DateTime.MinValue;
                DateTime.TryParse(field.Value, out dateTimeValue);

                if (dateTimeValue == DateTime.MinValue || dateTimeValue > DateTime.MaxValue)
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorOnlyAllowsDateTime, field.Description, templateDescription);
                }
            }

        }

        /// <summary>
        /// Validar Boleanos
        /// </summary>
        /// <param name="field">Value</param>
        private void ValidateBoolean(Field field, Row row, string templateDescription)
        {
            string[] booleanYesValues = new string[] { "SI", "YES", "TRUE" };
            string[] booleanNotValues = new string[] { "NO", "NOT", "FALSE" };

            if (!booleanYesValues.Contains(field.Value.ToUpper()) && !booleanNotValues.Contains(field.Value.ToUpper()))
            {
                row.HasError = true;
                row.ErrorDescription += string.Format(Errors.ErrorOnlyAllowsBoolean, field.Description, templateDescription);
            }
            else if (booleanYesValues.Contains(field.Value.ToUpper()))
            {
                field.Value = "True";
            }
            else if (booleanNotValues.Contains(field.Value.ToUpper()))
            {
                field.Value = "False";
            }
        }
        public List<int> FieldsIdsAllowsZero
        {
            get
            {
                if (fieldsIdsAllowsZero == null || fieldsIdsAllowsZero.Count == 0)
                {
                    COMMEN.Parameter parameter = ManagerDAO.GetParameterByParameterId((int)UtilitiesServices.Enums.ExtendedParametersTypes.FieldAllowZero);
                    if (parameter != null && !string.IsNullOrEmpty(parameter.TextParameter))
                    {
                        fieldsIdsAllowsZero = parameter.TextParameter.Split('|').Select(x => int.Parse(x)).ToList();
                    }
                    else
                    {
                        fieldsIdsAllowsZero = new List<int>();
                    }
                }

                return fieldsIdsAllowsZero;
            }
        }

        public List<int> FieldsIdsAllowsSpecialCharacter
        {
            get
            {
                if (fieldsIdsAllowsSpecialCharacter == null || fieldsIdsAllowsSpecialCharacter.Count == 0)
                {
                    COMMEN.Parameter parameter = ManagerDAO.GetParameterByParameterId((int)UtilitiesServices.Enums.ExtendedParametersTypes.FieldAllowSpecialCharacter);
                    if (parameter != null && !string.IsNullOrEmpty(parameter.TextParameter))
                    {
                        fieldsIdsAllowsSpecialCharacter = parameter.TextParameter.Split('|').Select(x => int.Parse(x)).ToList();
                    }
                    else
                    {
                        fieldsIdsAllowsSpecialCharacter = new List<int>();
                    }
                }

                return fieldsIdsAllowsSpecialCharacter;
            }
        }

        public List<int> FieldsTextTypeAllowsNumericOnly
        {
            get
            {
                if (fieldsTextTypeAllowsNumericOnly == null || fieldsTextTypeAllowsNumericOnly.Count == 0)
                {
                    COMMEN.Parameter parameter = ManagerDAO.GetParameterByParameterId((int)UtilitiesServices.Enums.ExtendedParametersTypes.FieldTextTypeAllowNumericOnly);
                    if (parameter != null && !string.IsNullOrEmpty(parameter.TextParameter))
                    {
                        fieldsTextTypeAllowsNumericOnly = parameter.TextParameter.Split('|').Select(x => int.Parse(x)).ToList();
                    }
                    else
                    {
                        fieldsTextTypeAllowsNumericOnly = new List<int>();
                    }
                }

                return fieldsTextTypeAllowsNumericOnly;
            }
        }



        public List<ValidationRegularExpression> ValidateRegularExpressions
        {
            get
            {
                if (validateRegularExpressions == null || validateRegularExpressions.Count == 0)
                {
                    validateRegularExpressions = new ValidationDAO().GetAllValidationRegularExpressions();
                }
                return validateRegularExpressions;
            }

        }
        public Template GetTextFieldsByFileNameUserName(string fileName, string userName, Template template, int typeFile)
        {
            DataTable dataTemplate = new DataTable();
            switch ((UtilitiesServices.Enums.TypeFile)typeFile)
            {
                case UtilitiesServices.Enums.TypeFile.Valores:
                    dataTemplate = GetDataTextPriceFieldsByFileNameUserName(fileName, userName);
                    break;
                case UtilitiesServices.Enums.TypeFile.Codigos:
                    dataTemplate = GetDataTextCodeFieldsByFileNameUserName(fileName, userName);
                    break;
            }

            template.Rows = ValidateRows(dataTemplate, template.Rows.Last().Fields, template.Description);
            return template;
        }

        public DataTable GetDataTextPriceFieldsByFileNameUserName(string fileName, string userName)
        {
            if (String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException(Errors.ErrorTemplateName);
            }

            string filePath = ConfigurationManager.AppSettings["ExternalFolderFiles"] + @"\" + userName + @"\" + fileName;

            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            try
            {
                string[] Colums = new string[3] { "Codigo", "Modelo", "Valor" };

                DataTable dataTable = new DataTable("Valores");

                foreach (var item in Colums)
                {
                    DataColumn colum = new DataColumn(item);
                    colum.DataType = System.Type.GetType("System.String");
                    dataTable.Columns.Add(colum);
                }

                string line;
                DataColumn identificador = new DataColumn("Identificador");
                identificador.DataType = System.Type.GetType("System.Int32");
                dataTable.Columns.Add(identificador);

                int Identificador = 1;
                file.ReadLine();

                while ((line = file.ReadLine()) != null)
                {
                    DataRow row = dataTable.NewRow();

                    if (!String.IsNullOrEmpty(line))
                    {
                        string[] values = line.Split('|');
                        int index = 0;
                        foreach (string column in Colums)
                        {
                            try
                            {
                                row[column] = values[index];
                                index++;
                            }
                            catch (Exception)
                            {
                                row[column] = Errors.ErrorStructure;
                            }
                        }

                        row["Identificador"] = Identificador;

                        dataTable.Rows.Add(row);
                        Identificador++;
                    }
                }

                file.Close();

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDataTextCodeFieldsByFileNameUserName(string fileName, string userName)
        {
            if (String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException(Errors.ErrorTemplateName);
            }

            string filePath = ConfigurationManager.AppSettings["ExternalFolderFiles"] + @"\" + userName + @"\" + fileName;

            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            try
            {
                string[] Colums = new string[27] { "Novedad", "Marca", "Clase", "Codigo", "HomologoCodigo", "Referencia1", "Referencia2", "Referencia3", "Peso", "IdServicio", "Servicio", "Bcpp", "Importado", "Potencia", "TipoCaja", "Cilindraje", "Nacionalidad", "CapacidadPasajeros", "CapacidadCarga", "Puertas", "AireAcondicionado", "Ejes", "Estado", "Combustible", "Transmision", "Um", "PesoCategoria" };

                DataTable dataTable = new DataTable("Codigos");

                foreach (var item in Colums)
                {
                    DataColumn colum = new DataColumn(item);
                    colum.DataType = System.Type.GetType("System.String");
                    dataTable.Columns.Add(colum);
                }

                String line;
                DataColumn identificador = new DataColumn("Identificador");
                identificador.DataType = System.Type.GetType("System.Int32");
                dataTable.Columns.Add(identificador);
                int Identificador = 1;
                file.ReadLine();

                while ((line = file.ReadLine()) != null)
                {
                    DataRow row = dataTable.NewRow();

                    if (!String.IsNullOrEmpty(line))
                    {
                        string[] values = line.Split('|');
                        int index = 0;
                        foreach (string column in Colums)
                        {
                            try
                            {
                                row[column] = values[index];
                                index++;
                            }
                            catch (Exception)
                            {
                                row[column] = Errors.ErrorStructure;
                            }
                        }

                        row["Identificador"] = Identificador;

                        dataTable.Rows.Add(row);
                        Identificador++;
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}