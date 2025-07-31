using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sistran.Company.Application.UniquePersonListRiskApplicationServices.Enum;
using Sistran.Company.Application.UniquePersonListRiskBusinessService;
using Sistran.Company.Application.UniquePersonListRiskBusinessService.Model;
using Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.Business;
using Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.DAO;
using Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.Resources;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider
{
    public class UniquePersonListRiskBusinessServiceEEProvider : IUniquePersonListRiskBusinessService
    {


        #region Proccess
        public CompanyListRiskLoad CreateAsyncronousProcess(CompanyListRiskLoad listRiskLoad)
        {
            try
            {
                ListRiskLoadBusiness listRiskLoadBusiness = new ListRiskLoadBusiness();
                return listRiskLoadBusiness.CreateAsyncronousProcess(listRiskLoad);
            }
            catch (Exception ex)
            {
                throw new Exception(Errors.ErrorCreateListRiskLoad);
            }
        }

        private CompanyListRiskLoad UpdateProcessListRisk(CompanyListRiskLoad listRiskLoad)
        {
            ListRiskLoadBusiness listRiskLoadBusiness = new ListRiskLoadBusiness();
            return listRiskLoadBusiness.UpdateProcessListRisk(listRiskLoad);
        }

        public CompanyListRiskLoad GetMassiveListRiskByProcessId(int processId)
        {
            ListRiskLoadDAO listRiskLoadDAO = new ListRiskLoadDAO();
            return listRiskLoadDAO.GetLoadMassiveListRiskLoad(processId);
        }

        private void CreateProcessListRiskLoadRow(CompanyListRiskRow listRiskRowLoad)
        {
            ListRiskLoadBusiness listRiskLoadBusiness = new ListRiskLoadBusiness();
            listRiskLoadBusiness.CreateProcessListRiskLoadRow(listRiskRowLoad);
        }


        #endregion

        #region Validate Data

        string templateName = "";
        public CompanyListRiskLoad GenerateLoadListRisk(CompanyListRiskLoad listRiskLoad)
        {
            ValidateFile(listRiskLoad);
            listRiskLoad.ProcessStatus = ProcessStatusEnum.Validando;
            listRiskLoad.EndDate = null;

            listRiskLoad = CreateAsyncronousProcess(listRiskLoad);

            if (listRiskLoad != null)
            {

                Task.Run(() => ValidateData(listRiskLoad));

            }
            else
            {
                return null;
            }


            return listRiskLoad;
        }

        private void ValidateFile(CompanyListRiskLoad listRiskLoad)
        {
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue();

                switch (listRiskLoad.RiskListType)
                {
                    case (int)RiskListTypeEnum.OWN:
                        fileProcessValue.Key1 = (int)Sistran.Core.Services.UtilitiesServices.Enums.FileProcessType.ListRisk;
                        fileProcessValue.Key4 = 2;
                        fileProcessValue.Key5 = (int)SubCoveredRiskType.Transport;
                        break;
                    case (int)RiskListTypeEnum.OFAC:
                        fileProcessValue.Key1 = (int)Sistran.Core.Services.UtilitiesServices.Enums.FileProcessType.ListRisk;
                        fileProcessValue.Key4 = 3;
                        fileProcessValue.Key5 = (int)SubCoveredRiskType.Transport;
                        break;
                    default:
                        break;
                }

                string fileName = listRiskLoad.File.Name;
                listRiskLoad.File = Delegate.utilitiesService.GetFileByFileProcessValue(fileProcessValue);
                if (listRiskLoad.File != null)
                {
                    listRiskLoad.File.Name = fileName;

                    listRiskLoad.File = Delegate.utilitiesService.ValidateFile(listRiskLoad.File, listRiskLoad.User.AccountName);
                }
                else
                {
                    throw new ValidationException(Errors.ErrorFileParametrizationNotFound);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
            }
        }

        private void ValidateData(CompanyListRiskLoad listRiskLoad)
        {
            try
            {
                File file = listRiskLoad?.File;

                if (!file.Templates.Any(x => x.PropertyName == TemplatePropertyName.ListRisk))
                {
                    listRiskLoad.ProcessStatus = ProcessStatusEnum.ValidadoConErrores;
                    listRiskLoad.Error_Description = Errors.ErrorListWithoutRecords;
                    UpdateProcessListRisk(listRiskLoad);
                    throw new Exception(Errors.ErrorListWithoutRecords);
                }
                if (file.Templates.Any(x => x.HasError == true))
                {
                    listRiskLoad.Error_Description = file.Templates.Where(x => x.HasError == true).FirstOrDefault().ErrorDescription;
                    listRiskLoad.ProcessStatus = ProcessStatusEnum.ValidadoConErrores;
                    UpdateProcessListRisk(listRiskLoad);
                    throw new Exception(Errors.ErrorInTemplate);
                }

                if (file != null)
                {
                    Template listRiskTemplate = new Template();

                    file.Name = listRiskLoad.File.Name;
                    listRiskTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.ListRisk);
                    listRiskTemplate.Description = file?.Templates?.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.ListRisk).Description;

                    listRiskTemplate = Delegate.utilitiesService.ValidateDataTemplate(file.Name, listRiskLoad.User.AccountName, listRiskTemplate);
                    templateName = listRiskTemplate.Description;
                    listRiskLoad.TotalRows = listRiskTemplate.Rows.Count;
                    UpdateProcessListRisk(listRiskLoad);

                    listRiskLoad.ProcessStatus = ProcessStatusEnum.ValidadoConExito;
                    listRiskLoad.HasError = false;

                    List<Row> filteredRows = new List<Row>();

                    string description = listRiskLoad.RiskListType == (int)RiskListTypeEnum.OWN ? "Numero de documento" : listRiskLoad.RiskListType == (int)RiskListTypeEnum.OFAC ? "ent_num" : string.Empty;

                    foreach (Row row in listRiskTemplate.Rows)
                    {
                        if (row.HasError)
                        {
                            listRiskLoad.ProcessStatus = ProcessStatusEnum.ValidadoConErrores;
                            listRiskLoad.HasError = true;

                            string formatedError = row.ErrorDescription.Replace("|", ",");
                            formatedError = formatedError.Remove(formatedError.Trim().Length - 1);

                            int maxLength = formatedError.Length;
                            if (maxLength > 300)
                            {
                                formatedError = formatedError.Substring(0, 300) + "...";
                            }

                            if (string.IsNullOrEmpty(templateName))
                            {
                                listRiskLoad.Error_Description += $"{Errors.ErrorCreatingLoad} {formatedError}";
                            }
                            else
                            {
                                listRiskLoad.Error_Description += string.Format(Errors.ErrorInTemplate, templateName, formatedError);
                            }

                            UpdateProcessListRisk(listRiskLoad);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(description))
                            {
                                if (listRiskTemplate.Rows.Count(x => x.Fields.FirstOrDefault(z => z.Description == description)?.Value == row.Fields.FirstOrDefault(z => z.Description == description)?.Value) == 1)
                                {
                                    filteredRows.Add(row);
                                }
                                else if (filteredRows.Count(x => x.Fields.FirstOrDefault(z => z.Description == description)?.Value == row.Fields.FirstOrDefault(z => z.Description == description)?.Value) == 0)
                                {
                                    filteredRows.Add(row);
                                }
                            }
                        }
                    }
                    listRiskTemplate.Rows = filteredRows;
                    listRiskLoad.TotalRows = listRiskTemplate.Rows.Count;
                    UpdateProcessListRisk(listRiskLoad);
                    Row emissionRow = listRiskTemplate.Rows.First();
                    FileValidationDAO ListRiskValidationDAO = new FileValidationDAO();
                    List<Validation> validations = ListRiskValidationDAO.GetValidationsByListRiskTemplate(file, emissionRow, listRiskLoad);

                    if (validations != null && validations.Count > 0)
                    {
                        Validation validation = validations.Find(x => x.Id == file.Id);
                        listRiskLoad.ProcessStatus = ProcessStatusEnum.ValidadoConErrores;
                        listRiskLoad.HasError = true;
                        string formatedError = validation.ErrorMessage.Replace("|", ",");
                        int maxLength = formatedError.Length;
                        if (maxLength > 300)
                        {
                            formatedError = formatedError.Substring(0, 300) + "...";
                        }


                        if (string.IsNullOrEmpty(templateName))
                        {
                            listRiskLoad.Error_Description += $"{Errors.ErrorCreateListRiskLoad} {formatedError}";
                        }
                        else
                        {
                            listRiskLoad.Error_Description += string.Format(Errors.ErrorInTemplate, templateName, formatedError);
                        }
                        UpdateProcessListRisk(listRiskLoad);
                    }


                    try
                    {
                        foreach (var item in file.Templates[0].Rows)
                        {
                            item.Fields.Add(new Field { PropertyName = "Identifier", Value = "1" });

                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                    List<File> validatedFiles = Delegate.utilitiesService.GetDataTemplates(file.Templates);

                    UpdateProcessListRisk(listRiskLoad);

                    List<Template> lstTemplates = new List<Template>();
                    if (listRiskTemplate != null)
                    {
                        lstTemplates.Add(listRiskTemplate);
                    }

                    listRiskLoad.ProcessStatus = ProcessStatusEnum.Cargando;
                    UpdateProcessListRisk(listRiskLoad);


                    CreateModelsListRisk(listRiskLoad, validatedFiles, lstTemplates);
                }
                else
                {
                    throw new Exception(Errors.ErrorFileNotExist);
                }
            }
            catch (Exception ex)
            {
                listRiskLoad.ProcessStatus = ProcessStatusEnum.ValidadoConErrores;
                listRiskLoad.HasError = true;

                if (string.IsNullOrEmpty(templateName))
                {
                    listRiskLoad.Error_Description += Errors.ErrorInTemplate + " : " + ex.Message;
                }
                else
                {
                    string[] messages = ex.Message.Split('|');
                    listRiskLoad.Error_Description += string.Format(Errors.ErrorInTemplate, templateName, messages[0]);
                }

                UpdateProcessListRisk(listRiskLoad);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void CreateModelsListRisk(CompanyListRiskLoad listRiskLoad, List<File> files, List<Template> templates)
        {
            if (listRiskLoad != null && files != null && templates != null)
            {
                switch (listRiskLoad.RiskListType)
                {
                    case (int)RiskListTypeEnum.OWN:
                        ParallelHelper.ForEach(files, file => { CreateModelOwnList(listRiskLoad, file); });
                        break;
                    case (int)RiskListTypeEnum.OFAC:
                        ParallelHelper.ForEach(files, file => { 
                            CreateModelOfacList(listRiskLoad, file); 
                            });
                        break;
                    default:
                        break;
                }


                listRiskLoad.ProcessStatus = ProcessStatusEnum.Cargado;
                UpdateProcessListRisk(listRiskLoad);
            }
        }

        private void CreateModelOwnList(CompanyListRiskLoad listRiskLoad, File file)
        {
            CompanyListRiskRow companyListRiskRow = new CompanyListRiskRow();

            try
            {
                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription).Distinct())).ToList();
                companyListRiskRow.ProcessId = listRiskLoad.ProcessId;
                companyListRiskRow.ProcessMassiveLoadId = listRiskLoad.Id;
                companyListRiskRow.RowNumber = file.Id;
                companyListRiskRow.HasError = hasError;
                companyListRiskRow.IdCardNo = Delegate.utilitiesService.GetValueByField<string>(file.Templates[0].Rows[0].Fields.Find(x => x.PropertyName == FieldPropertyName.RiskListDocumentNumberField)).ToString();
                companyListRiskRow.Error_Description = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                companyListRiskRow.SerializedRow = JsonConvert.SerializeObject(file);

                CreateProcessListRiskLoadRow(companyListRiskRow);

                if (!hasError)
                {
                    Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.ListRisk).Rows.First();
                    templateName = file.Templates.First(x => x.PropertyName == TemplatePropertyName.ListRisk).Description;

                    string document = Delegate.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskListDocumentNumberField)).ToString();
                    string name = Delegate.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskListNameField)).ToString();
                    string lastName = Delegate.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskListLastNameField)).ToString();
                    DateTime assigmentDate = Convert.ToDateTime(Delegate.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskListAssignmentDate)));

                    if (!string.IsNullOrEmpty(document))
                    {
                        CompanyListRisk listRisk = new CompanyListRisk
                        {
                            DocumentNumber = document,
                            Name = name,
                            LastName = lastName,
                            Alias = string.Empty,
                            ListRiskId = listRiskLoad.ListRiskId,
                            CreatedUser = listRiskLoad.User.AccountName,
                            ProcessId = listRiskLoad.ProcessId,
                            ListRiskDescription = listRiskLoad.ListRiskDescription,
                            ListRiskType = (int)RiskListTypeEnum.OWN,
                            ListRiskTypeDescription = listRiskLoad.ListRiskDescription,
                            AssignmentDate = assigmentDate

                        };
                        string pendingOperationJson = string.Format("{0}", JsonHelper.SerializeObjectToJson(listRisk));
                        QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "ListRiskQueue");
                    }
                    else
                    {
                        companyListRiskRow.HasError = true;
                        companyListRiskRow.Error_Description += Errors.ErrorListRiskNotFound + KeySettings.ReportErrorSeparatorMessage();
                        CreateProcessListRiskLoadRow(companyListRiskRow);
                    }
                    UpdateProcessListRisk(listRiskLoad);
                }
                else
                {
                    EventLog.WriteEntry("Error en CreateModel", companyListRiskRow.Error_Description);
                }
            }
            catch (Exception ex)
            {
                listRiskLoad.HasError = true;

                if (string.IsNullOrEmpty(templateName))
                {
                    listRiskLoad.Error_Description += Errors.ErrorCreateModelListRisk;
                    EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                }
                else
                {
                    string[] messages = ex.Message.Split('|');
                    EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                    listRiskLoad.Error_Description += string.Format(Errors.ErrorInTemplate, templateName, messages[0]);
                }

                UpdateProcessListRisk(listRiskLoad);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void CreateModelOfacList(CompanyListRiskLoad listRiskLoad, File file)
        {
            CompanyListRiskRow companyListRiskRow = new CompanyListRiskRow();

            try
            {
                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription).Distinct())).ToList();
                companyListRiskRow.ProcessId = listRiskLoad.ProcessId;
                companyListRiskRow.ProcessMassiveLoadId = listRiskLoad.Id;
                companyListRiskRow.RowNumber = file.Id;
                companyListRiskRow.HasError = hasError;
                companyListRiskRow.IdCardNo = Delegate.utilitiesService.GetValueByField<string>(file.Templates[0].Rows[0].Fields.Find(x => x.PropertyName == FieldPropertyName.OFACRiskListEntNumField)).ToString();
                companyListRiskRow.Error_Description = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                companyListRiskRow.SerializedRow = JsonConvert.SerializeObject(file);

                CreateProcessListRiskLoadRow(companyListRiskRow);

                if (!hasError)
                {
                    Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.ListRisk).Rows.First();
                    templateName = file.Templates.First(x => x.PropertyName == TemplatePropertyName.ListRisk).Description;

                    string EntNum = Delegate.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.OFACRiskListEntNumField)).ToString();
                    string SDNName = Delegate.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.OFACRiskListSDNNameField)).ToString();
                    string SDNType = Delegate.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.OFACRiskListSDNTypeField)).ToString();
                    string Program = Delegate.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.OFACRiskListProgramField)).ToString();
                    string Title = Delegate.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.OFACRiskListTitleField)).ToString();
                    string CallSign = Delegate.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.OFACRiskListCallSignField)).ToString();
                    string VessType = Delegate.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.OFACRiskListVessTypeField)).ToString();
                    string Tonnage = Delegate.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.OFACRiskListTonnageField)).ToString();
                    string GRT = Delegate.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.OFACRiskListGRTField)).ToString();
                    string VessFlag = Delegate.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.OFACRiskListVessownerField)).ToString();
                    string VessOwner = Delegate.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.OFACRiskListVessownerField)).ToString();
                    string Remarks = Delegate.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.OFACRiskListRemarksField)).ToString();
                    if (!string.IsNullOrEmpty(EntNum) && !string.IsNullOrEmpty(SDNName))
                    {
                        CompanyListRiskOfac listRisk = new CompanyListRiskOfac
                        {
                            EntNum = EntNum,
                            SDNName = SDNName,
                            SDNType = SDNType,
                            Program = Program,
                            Title = Title,
                            CallSign = CallSign,
                            VessType = VessType,
                            Tonnage = Tonnage,
                            GRT = GRT,
                            VessFlag = VessFlag,
                            VessOwner = VessOwner,
                            Remarks = Remarks,
                            ProcessId = listRiskLoad.ProcessId,
                            ListRiskDescription = listRiskLoad.ListRiskDescription,
                            ListRiskType = (int)RiskListTypeEnum.OFAC,
                            CreatedUser = listRiskLoad.User.AccountName,
                            ListRiskId = listRiskLoad.ListRiskId,
                            ListRiskTypeDescription = listRiskLoad.ListRiskDescription
                        };
                        string pendingOperationJson = string.Format("{0}", JsonHelper.SerializeObjectToJson(listRisk));
                        QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "ListRiskOfacQueue");
                        //listRiskLoad.TotalRows = listRiskLoad.TotalRows + 1;
                    }
                    else
                    {
                        companyListRiskRow.HasError = true;
                        companyListRiskRow.Error_Description += Errors.ErrorListRiskNotFound + KeySettings.ReportErrorSeparatorMessage();
                        CreateProcessListRiskLoadRow(companyListRiskRow);
                    }
                    UpdateProcessListRisk(listRiskLoad);
                }
                else
                {
                    EventLog.WriteEntry("Error en CreateModel", companyListRiskRow.Error_Description);
                }
            }
            catch (Exception ex)
            {
                listRiskLoad.HasError = true;

                if (string.IsNullOrEmpty(templateName))
                {
                    listRiskLoad.Error_Description += Errors.ErrorCreateModelListRisk;
                    EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                }
                else
                {
                    string[] messages = ex.Message.Split('|');
                    EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                    listRiskLoad.Error_Description += string.Format(Errors.ErrorInTemplate, templateName, messages[0]);
                }

                UpdateProcessListRisk(listRiskLoad);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        public void CreateListRiskTemporal(string businessCollection)
        {
            ListRiskLoadBusiness listRiskLoadBusiness = new ListRiskLoadBusiness();
            listRiskLoadBusiness.CreateListRiskTemporal(businessCollection);
        }

        public void CreateListRiskOfacTemporal(string businessCollection)
        {
            ListRiskLoadBusiness listRiskLoadBusiness = new ListRiskLoadBusiness();
            listRiskLoadBusiness.CreateListRiskOfacTemporal(businessCollection);
        }
        #endregion

        #region Record
        public CompanyListRiskLoad RecordListRisk(CompanyListRiskLoad listRiskLoad)
        {
            listRiskLoad.ProcessStatus = ProcessStatusEnum.Procesando;
            UpdateProcessListRisk(listRiskLoad);

            Task.Run(() => RecordPendigOpeartions(listRiskLoad));

            return listRiskLoad;
        }

        public void RecordPendigOpeartions(CompanyListRiskLoad listRiskLoad)
        {
            CompanyListRiskLoad companyListRiskLoad = new CompanyListRiskLoad();
            ListRiskLoadBusiness listRiskLoadBusiness = new ListRiskLoadBusiness();
            companyListRiskLoad = listRiskLoadBusiness.GetMassiveListRiskByProcessId(listRiskLoad.ProcessId, false);
            listRiskLoad.TotalRows = companyListRiskLoad.Rows.Count;
            UpdateProcessListRisk(listRiskLoad);

            ParallelHelper.ForEach(companyListRiskLoad.Rows, listRiskRow =>
            {
                RecordPendingOperation(listRiskRow, listRiskLoad);
            });
            listRiskLoad.ProcessStatus = ProcessStatusEnum.Procesado;
            UpdateProcessListRisk(listRiskLoad);
        }

        public void RecordPendingOperation(CompanyListRiskRow companyListRiskRow, CompanyListRiskLoad listRiskLoad)
        {
            ListRiskBusiness listRiskBusiness = new ListRiskBusiness();

            var listRiskObject = listRiskBusiness.GetListRiskPersonTemporalByDocumentNumber(companyListRiskRow.IdCardNo, listRiskLoad.ProcessId);

            switch (listRiskLoad.RiskListType)
            {
                case (int)RiskListTypeEnum.OWN:
                    CompanyListRisk listRiskPerson = JsonConvert.DeserializeObject<CompanyListRisk>(listRiskObject);
                    string listRiskPersonJson = string.Format("{0}", JsonConvert.SerializeObject(listRiskPerson));
                    QueueHelper.PutOnQueueJsonByQueue(listRiskPersonJson, "RecordListRiskQueue");
                    break;
                case (int)RiskListTypeEnum.OFAC:
                    CompanyListRiskOfac listRiskPersonOfac = JsonConvert.DeserializeObject<CompanyListRiskOfac>(listRiskObject);
                    string listRiskPersonOfacJson = string.Format("{0}", JsonConvert.SerializeObject(listRiskPersonOfac));
                    QueueHelper.PutOnQueueJsonByQueue(listRiskPersonOfacJson, "RecordListRiskOfacQueue");
                    break;
                default:
                    break;
            }


        }

        public void IssueRecordListRisk(string businessCollection)
        {
            ListRiskLoadBusiness listRiskLoadBusiness = new ListRiskLoadBusiness();
            listRiskLoadBusiness.IssueRecordListRisk(businessCollection);
        }

        public void IssueRecordListRiskOfac(string businessCollection)
        {
            ListRiskLoadBusiness listRiskLoadBusiness = new ListRiskLoadBusiness();
            listRiskLoadBusiness.IssueRecordListRiskOfac(businessCollection);
        }

        public void IssueRecordListRiskOnu(string businessCollection)
        {
            ListRiskLoadBusiness listRiskLoadBusiness = new ListRiskLoadBusiness();
            listRiskLoadBusiness.IssueRecordListRiskOnu(businessCollection);
        }
        #endregion

        #region Others

        public CompanyListRiskPerson CreateListRiskPersonBusiness(CompanyListRiskPerson listRisk)
        {
            ListRiskBusiness business = new ListRiskBusiness();
            return business.CreateListRiskPerson(listRisk);
        }

        public List<IdentityCardTypes> GetIdentityCardTypes()
        {
            ListRiskBusiness business = new ListRiskBusiness();
            return business.GetIdentityCardTypes();
        }
        public List<RiskListModel> GetListRisk()
        {
            ListRiskBusiness business = new ListRiskBusiness();
            return business.GetListRisk();
        }

        public List<CompanyListRiskPerson> GetListRiskPersonByDocumentNumber(string documentNumber)
        {
            ListRiskBusiness business = new ListRiskBusiness();
            return business.GetListRiskPersonByDocumentNumber(documentNumber);
        }

        public List<CompanyListRiskPerson> GetListRiskPersonList(int documentNumber, string name, string surname, string nickName, int listRiskId)
        {
            ListRiskBusiness business = new ListRiskBusiness();
            return business.GetListRiskPersonList(documentNumber, name, surname, nickName, listRiskId);
        }

        public CompanyListRiskStatus GetStatusByProcessId(int processId)
        {
            ListRiskLoadBusiness listRiskLoad = new ListRiskLoadBusiness();
            return listRiskLoad.GetStatusByProcessId(processId);
        }

        public string GetErrorExcelProcessListRisk(int loadProcessId)
        {
            ListRiskLoadBusiness listRiskLoad = new ListRiskLoadBusiness();
            return listRiskLoad.GetErrorExcelProcessListRisk(loadProcessId);
        }

        public CompanyListRiskModel GetAssignedListMantenance(string documentNumber, int? listRiskType)
        {
            ListRiskLoadBusiness listRiskLoad = new ListRiskLoadBusiness();
            return listRiskLoad.GetAssignedListMantenance(documentNumber, listRiskType);
        }

        public bool InitialProcessFile()
        {
            ListRiskLoadBusiness listRiskLoad = new ListRiskLoadBusiness();
            return listRiskLoad.IntialProcessFile();
        }

        public int GenerateListRiskProcessRequest(bool matchProcess, bool OnuProcess, bool ofacProcess, string searchValue, bool isMasive)
        {
            ListRiskLoadDAO listRiskLoadDAO = new ListRiskLoadDAO();
            return listRiskLoadDAO.GenerateListRiskProcessRequest(matchProcess, OnuProcess, ofacProcess, searchValue, isMasive);
        }

        public RiskListMatch SearchProcess(string searchValue)
        {

            ListRiskLoadDAO listRiskLoadDAO = new ListRiskLoadDAO();
            return listRiskLoadDAO.SearchProcess(searchValue);
        }

        #endregion
    }
}
