namespace Sistran.Core.Application.AuditServices.EEProvider.DAOs
{
    using Sistran.Core.Application.AuditServices.EEProvider.Helpers;
    using Sistran.Core.Application.AuditServices.EEProvider.Resources;
    using Sistran.Core.Application.Utilities.Constants;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ENUMUT = Utilities.Enums;
    using UTMO = Utilities.Error;

    /// <summary>
    /// Clase para la generación de los archivos de excel.
    /// </summary>
    public class AuditFileDAO
    {
        public object UtilitiesServiceEEProviderCore { get; private set; }

        #region Generacion Archivos Auditoria
        /// <summary>
        /// Generates the file to audits.
        /// </summary>
        /// <param name="auditList">The audit list.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToAudits(List<Models.Audit> auditList, string fileName)
        {
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue
                {
                    Key1 = (int)FileProcessType.LogAudit
                };
                File file = Task.Run(() =>
                {
                    FileDAO utilFileDAO = new FileDAO();
                    var result = utilFileDAO.GetFileByFileProcessValue(fileProcessValue);
                    DataFacadeManager.Dispose();
                    return result;
                }
                ).Result;
                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    var rows = GetRowsAudit(auditList, file);
                    if (rows != null)
                    {
                        file.Templates[0].Rows = rows;
                    }
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                    FileDAO utilFileDAO = new FileDAO();
                    var result = utilFileDAO.GenerateFile(file);
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(result);
                }
                else
                {
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(string.Empty);
                }
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>
                {
                    Errors.ErrorDownloadExcel
                };
                return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }

        }
        #endregion

        /// <summary>
        /// Gets the rows audit.
        /// </summary>
        /// <param name="auditList">The audit list.</param>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        private List<Row> GetRowsAudit(List<Models.Audit> auditList, File file)
        {
            if (auditList != null && auditList.Count > 0 && file != null)
            {
                ConcurrentBag<Row> rows = new ConcurrentBag<Row>();
                var template = file.Templates.Where(x => x.PropertyName == TemplatePropertyName.LogAudit).FirstOrDefault();
                if (template != null)
                {
                    var registerDate =
                        template?.Rows[0].Fields.Where(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom).Select(x => new Field
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
                        }).FirstOrDefault();

                    var accounName = template?.Rows[0].Fields.Where(x => x.PropertyName == FieldPropertyName.UserName).Select(x => new Field
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
                    }).FirstOrDefault();

                    var typeTransaction = template?.Rows[0].Fields.Where(x => x.PropertyName == FieldPropertyName.TypeTransaction).Select(x => new Field
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
                    }).FirstOrDefault();

                    var objectName = template?.Rows[0].Fields.Where(x => x.PropertyName == FieldPropertyName.ObjectName).Select(x => new Field
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
                    }).FirstOrDefault();

                    var propertyName = template?.Rows[0].Fields.Where(x => x.PropertyName == FieldPropertyName.PropertyName).Select(x => new Field
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
                    }).FirstOrDefault();

                    var valueBefore = template?.Rows[0].Fields.Where(x => x.PropertyName == FieldPropertyName.ValueBefore).Select(x => new Field
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
                    }).FirstOrDefault();

                    var valueAfter = template?.Rows[0].Fields.Where(x => x.PropertyName == FieldPropertyName.ValueAfter).Select(x => new Field
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
                    }).FirstOrDefault();

                    template.Rows[0].Fields.Add(accounName);
                    object obj = new object();
                    Parallel.ForEach(auditList, new ParallelOptions { MaxDegreeOfParallelism = 1 }, audit =>
                    {

                        Field registerDateColum = null;
                        Field accountNameColum = null;
                        Field typeTransactionField = null;
                        Field objectNameField = null;

                        lock (obj)
                        {
                            registerDateColum = CloneHelper.CloneField(registerDate);
                            accountNameColum = CloneHelper.CloneField(accounName);
                            typeTransactionField = CloneHelper.CloneField(typeTransaction);
                            objectNameField = CloneHelper.CloneField(objectName);
                        }
                        registerDateColum.Value = audit.RegisterDate.ToString();
                        accountNameColum.Value = audit?.User?.Description;
                        typeTransactionField.Value = audit?.ActionType.ToString();
                        objectNameField.Value = audit?.ObjectName;
                        Parallel.ForEach(audit.Changes, new ParallelOptions { MaxDegreeOfParallelism = 1 }, change =>
                        {
                            Field propertyNameField = null;
                            Field valueBeforeField = null;
                            Field valueAfterField = null;
                            lock (obj)
                            {
                                propertyNameField = CloneHelper.CloneField(propertyName);
                                valueBeforeField = CloneHelper.CloneField(valueBefore);
                                valueAfterField = CloneHelper.CloneField(valueAfter);
                            }
                            var fields = new List<Field>();
                            propertyNameField.Value = change.Id;
                            valueBeforeField.Value = change.ValueBefore;
                            valueAfterField.Value = change.ValueAfter;
                            fields.Add(registerDateColum);
                            fields.Add(accountNameColum);
                            fields.Add(typeTransactionField);
                            fields.Add(objectNameField);
                            fields.Add(propertyNameField);
                            fields.Add(valueBeforeField);
                            fields.Add(valueAfterField);
                            rows.Add(new Row
                            {
                                Fields = fields
                            });
                        });
                    });

                    return rows.ToList();
                }
                else
                {
                    throw new Exception(Errors.TempleteNotExist);
                }
            }
            else
            {
                return null;
            }

        }

    }
}
