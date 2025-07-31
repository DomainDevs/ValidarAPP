using System;
using System.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;
using Sistran.Core.Framework;
using System.Collections.Generic;
using Sistran.Core.Framework.Queues;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Company.Application.MassiveVehicleParamBusinessService;
using Sistran.Company.Application.MassiveVehicleParamBusinessService.Model;
using Sistran.Company.Application.MassiveVehicleParamBusinessServiceProvider.Dao;
using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.Enum;
using Sistran.Company.Application.MassiveVehicleParamBusinessServiceProvider.Assembler;
using Sistran.Company.Application.MassiveVehicleParamBusinessServiceProvider.Resources;
using Sistran.Company.Application.MassiveVehicleParamBusinessServiceProvider.Bussiness;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.MassiveVehicleParamBusinessServiceProvider
{
    public class CompanyMassiveVehicleParamBusinessServiceProvider : ICompanyMassiveVehicleParamBusinessService
    {
        string templateName = "";
        public CompanyProcessFasecolda GenerateLoadMassiveVehicleFasecolda(CompanyProcessFasecolda processFasecolda)
        {
            ValidateFile(processFasecolda);
            processFasecolda.StatusId = (int)VehicleFasecoldaProcessStatusEnum.Validando;
            processFasecolda.ProcessStatusType = new CompanyVehicleFasecoldaStatusType
            {
                StatusType = VehicleFasecoldaProcessStatusEnum.Validando
            };
            processFasecolda.EndDate = null;

            processFasecolda = CreateProcessFasecolda(processFasecolda);

            if (processFasecolda == null)
            {
                return null;
            }
            processFasecolda.ProcessStatus = VehicleFasecoldaStatusEnum.Finalizado;
            TP.Task.Run(() => ValidateData(processFasecolda));
            return processFasecolda;
        }

        private void ValidateFile(CompanyProcessFasecolda processFasecolda)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();

            switch ((int)processFasecolda.TypeFile)
            {
                case 1:
                    fileProcessValue.Key1 = (int)FileProcessType.LoadValuesFaseColda;
                    fileProcessValue.Key4 = (int)FileTypeFasecoldaEnum.Valores;
                    break;
                case 2:
                    fileProcessValue.Key1 = (int)FileProcessType.LoadCodesFaseColda;
                    fileProcessValue.Key4 = (int)FileTypeFasecoldaEnum.Codigo;
                    break;
            }

            fileProcessValue.Key5 = (int)SubCoveredRiskType.Vehicle;

            string fileName = processFasecolda.File.Name;
            processFasecolda.File = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);
            if (processFasecolda.File != null)
            {
                processFasecolda.File.Name = fileName;
                processFasecolda.File = DelegateService.utilitiesService.ValidateFile(processFasecolda.File, processFasecolda.User.AccountName);
            }
            else
            {
                throw new ValidationException(Errors.ErrorFileParametrizationNotFound);
            }
        }

        private void ValidateData(CompanyProcessFasecolda processFasecolda)
        {
            try
            {
                var file = processFasecolda?.File;
                if (file != null)
                {
                    FileProcessValue fileProcessValue = new FileProcessValue();

                    switch ((int)processFasecolda.TypeFile)
                    {
                        case 1:
                            fileProcessValue.Key1 = (int)FileProcessType.LoadValuesFaseColda;
                            break;
                        case 2:
                            fileProcessValue.Key1 = (int)FileProcessType.LoadCodesFaseColda;
                            break;
                    }

                    fileProcessValue.Key4 = (int)processFasecolda.TypeFile;
                    fileProcessValue.Key5 = (int)SubCoveredRiskType.Vehicle;

                    file = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);

                    Template faseColdaTemplate = new Template();

                    switch (processFasecolda.TypeFile)
                    {
                        case FileTypeFasecoldaEnum.Valores:
                            file.Name = processFasecolda.File.Name;
                            faseColdaTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.LoadValuesFaseColda);
                            faseColdaTemplate.Description = file?.Templates?.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.LoadValuesFaseColda).Description;
                            faseColdaTemplate = DelegateService.utilitiesService.GetTextFieldsByFileNameUserName(file.Name, processFasecolda.User.AccountName, faseColdaTemplate, (int)processFasecolda.TypeFile);
                            break;
                        case FileTypeFasecoldaEnum.Codigo:
                            file.Name = processFasecolda.File.Name;
                            faseColdaTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.LoadCodesFaseColda);
                            faseColdaTemplate.Description = file?.Templates?.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.LoadCodesFaseColda).Description;
                            faseColdaTemplate = DelegateService.utilitiesService.GetTextFieldsByFileNameUserName(file.Name, processFasecolda.User.AccountName, faseColdaTemplate, (int)processFasecolda.TypeFile);
                            break;
                    }


                    templateName = faseColdaTemplate.Description;

                    processFasecolda.Pendings = faseColdaTemplate.Rows.Count;
                    processFasecolda.TotalRows = faseColdaTemplate.Rows.Count;
                    processFasecolda.ProcessStatusType.StatusType = VehicleFasecoldaProcessStatusEnum.ValidadoConExito;
                    processFasecolda.HasError = false;

                    UpdateProcessFasecolda(processFasecolda);

                    foreach (Row row in faseColdaTemplate.Rows)
                    {
                        foreach (Field field in row.Fields)
                        {
                            if (field.Value.Contains("Error"))
                            {
                                row.HasError = true;
                                row.ErrorDescription = field.Value;
                            }
                        }

                        if (row.HasError)
                        {
                            processFasecolda.ProcessStatusType.StatusType = VehicleFasecoldaProcessStatusEnum.ValidadoConErrores;
                            processFasecolda.HasError = true;

                            string formatedError = row.ErrorDescription.Replace("|", ",");
                            formatedError = formatedError.Remove(formatedError.Trim().Length - 1);

                            int maxLength = formatedError.Length;
                            if (maxLength > 300)
                            {
                                formatedError = formatedError.Substring(0, 300) + "...";
                            }

                            if (string.IsNullOrEmpty(templateName))
                            {
                                processFasecolda.Error_Description += $"{Errors.ErrorCreatingLoad} {formatedError}";
                            }
                            else
                            {
                                processFasecolda.Error_Description += string.Format(Errors.ErrorInTemplate, templateName, formatedError);
                            }
                            processFasecolda.WithErrorsLoaded += 1;

                            UpdateProcessFasecolda(processFasecolda);
                        }
                    }

                    Row emissionRow = faseColdaTemplate.Rows.First();
                    FaseColdaValidationDAO faseColdaValidationDAO = new FaseColdaValidationDAO();
                    List<Validation> validations = faseColdaValidationDAO.GetValidationsByFaseColdaTemplate(file, emissionRow, processFasecolda);
                    if (validations != null && validations.Count > 0)
                    {
                        Validation validation = validations.Find(x => x.Id == file.Id);

                        processFasecolda.ProcessStatusType.StatusType = VehicleFasecoldaProcessStatusEnum.ValidadoConErrores;
                        processFasecolda.HasError = true;

                        string formatedError = validation.ErrorMessage.Replace("|", ",");

                        int maxLength = formatedError.Length;
                        if (maxLength > 300)
                        {
                            formatedError = formatedError.Substring(0, 300) + "...";
                        }

                        if (string.IsNullOrEmpty(templateName))
                        {
                            processFasecolda.Description += $"{Errors.ErrorCreateFacecoldaLoad} {formatedError}";
                        }
                        else
                        {
                            processFasecolda.Description += string.Format(Errors.ErrorInTemplate, templateName, formatedError);
                        }

                        UpdateProcessFasecolda(processFasecolda);
                    }
                    //Add Identifier Column 
                    Template templatePrincipal = file.Templates.First(x => x.IsPrincipal);
                    foreach (Row row in templatePrincipal.Rows)
                    {
                        int indexrow = templatePrincipal.Rows.IndexOf(row);
                        if (row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.Identificator) == null)
                        {
                            templatePrincipal.Rows[indexrow].Fields.Add(new Field { PropertyName = "Identifier", Value = (indexrow + 1).ToString() });
                        }
                        else
                        {
                            row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.Identificator).Value = (indexrow + 1).ToString();
                        }

                    }

                    List<File> validatedFiles = DelegateService.utilitiesService.GetDataTemplates(file.Templates);

                    validations = faseColdaValidationDAO.GetValidationsByFiles(validatedFiles, processFasecolda);

                    if (validations != null && validations.Count > 0)
                    {
                        Validation validation;

                        foreach (File validatedFile in validatedFiles)
                        {
                            validation = validations.Find(x => x.Id == validatedFile.Id);
                            if (validation != null)
                            {
                                validatedFile.Templates[0].Rows[0].HasError = true;
                                validatedFile.Templates[0].Rows[0].ErrorDescription = validation.ErrorMessage;
                            }
                        }
                    }

                    //UpdateProcessFasecolda(processFasecolda);

                    List<Template> lstTemplates = new List<Template>();
                    if (faseColdaTemplate != null)
                    {
                        lstTemplates.Add(faseColdaTemplate);
                    }

                    switch ((int)processFasecolda.TypeFile)
                    {
                        case 1:
                            FaseColdaLoadDAO.DeleteFasecoldaPrice();
                            CreateModelsFasecoldaValues(processFasecolda, validatedFiles, lstTemplates);
                            break;
                        case 2:
                            FaseColdaLoadDAO.DeleteFasecoldaCode();
                            CreateModelsFasecoldaCodes(processFasecolda, validatedFiles, lstTemplates);
                            break;
                    }

                    processFasecolda.ProcessStatusType.StatusType = VehicleFasecoldaProcessStatusEnum.Cargando;
                    UpdateProcessFasecolda(processFasecolda);
                }
                else
                {
                    throw new Exception(Errors.ErrorFileNotExist);
                }
            }
            catch (Exception ex)
            {
                processFasecolda.ProcessStatusType.StatusType = VehicleFasecoldaProcessStatusEnum.ValidadoConErrores;
                processFasecolda.HasError = true;

                if (string.IsNullOrEmpty(templateName))
                {
                    processFasecolda.Description += Errors.ErrorInTemplate + " : " + ex.Message;
                }
                else
                {
                    string[] messages = ex.Message.Split('|');
                    processFasecolda.Description += string.Format(Errors.ErrorInTemplate, templateName, messages[0]);
                }

                UpdateProcessFasecolda(processFasecolda);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void CreateModelsFasecoldaValues(CompanyProcessFasecolda processFasecolda, List<File> files, List<Template> templates)
        {
            if (processFasecolda != null && files != null && templates != null)
            {
                ParallelHelper.ForEach(files, file =>
                {
                    CreateModelValue(processFasecolda, file);
                });

                processFasecolda.ProcessStatusType.StatusType = VehicleFasecoldaProcessStatusEnum.Cargado;
                UpdateProcessFasecolda(processFasecolda);
            }
        }

        private void CreateModelsFasecoldaCodes(CompanyProcessFasecolda processFasecolda, List<File> files, List<Template> templates)
        {
            if (processFasecolda != null && files != null && templates != null)
            {
                //ParallelHelper.ForEach(files, file =>
                //{
                //    CreateModelCode(processFasecolda, file);
                //});
                foreach (File file in files) {
                    CreateModelCode(processFasecolda, file);
                }
                processFasecolda.ProcessStatusType.StatusType = VehicleFasecoldaProcessStatusEnum.Cargado;
                UpdateProcessFasecolda(processFasecolda);
            }
        }

        private void CreateModelValue(CompanyProcessFasecolda processFasecolda, File file)
        {
            CompanyMassiveVehicleFasecoldaRow vehicleFasecoldaRow = new CompanyMassiveVehicleFasecoldaRow { };

            try
            {
                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription).Distinct())).ToList();
                vehicleFasecoldaRow.ProcessId = processFasecolda.ProcessId;
                vehicleFasecoldaRow.ProcessMassiveLoadId = processFasecolda.Id;
                vehicleFasecoldaRow.RowNumber = file.Id;
                vehicleFasecoldaRow.HasError = hasError;
                vehicleFasecoldaRow.Error_Description = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                vehicleFasecoldaRow.SerializedRow = JsonConvert.SerializeObject(file);

                //CreateProcessFasecoldaRow(vehicleFasecoldaRow);

                if (!hasError)
                {
                    Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.LoadValuesFaseColda).Rows.First();
                    templateName = file.Templates.First(x => x.PropertyName == TemplatePropertyName.LoadValuesFaseColda).Description;

                    string codigo = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Codigo")).ToString();
                    string modelo = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Modelo")).ToString();
                    decimal valor = Convert.ToDecimal(DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == "Valor")));
                    CreateProcessFasecoldaRow(vehicleFasecoldaRow);
                    if (!string.IsNullOrEmpty(codigo) && !string.IsNullOrEmpty(modelo))
                    {
                        CompanyFasecoldaPrice fasecolda = new CompanyFasecoldaPrice
                        {
                            Codigo = codigo,
                            Modelo = modelo,
                            Valor = valor
                        };

                        IQueue queue = new BaseQueueFactory().CreateQueue("FasecoldaValueQueue", routingKey: "FasecoldaValueQueue", serialization: "JSON");
                        queue.PutOnQueue(JsonConvert.SerializeObject(fasecolda));
                       
                    }
                    else
                    {
                        vehicleFasecoldaRow.HasError = true;
                        vehicleFasecoldaRow.Error_Description += Errors.ErrorFasecoldaNotFound + KeySettings.ReportErrorSeparatorMessage();
                        CreateProcessFasecoldaRow(vehicleFasecoldaRow);
                    }
                }
                else
                {
                    throw new Exception(Errors.ErrorCreatePriceFasecolda);
                }
            }
            catch (Exception ex)
            {
                processFasecolda.HasError = true;
                processFasecolda.StatusId = 2;

                if (string.IsNullOrEmpty(templateName))
                {
                    processFasecolda.Error_Description += Errors.ErrorCreatePriceFasecolda;
                    EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                }
                else
                {
                    string[] messages = ex.Message.Split('|');
                    processFasecolda.Error_Description += string.Format(Errors.ErrorInTemplate, templateName, messages[0]);
                }

                UpdateProcessFasecolda(processFasecolda);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void CreateModelCode(CompanyProcessFasecolda processFasecolda, File file)
        {
            CompanyMassiveVehicleFasecoldaRow vehicleFasecoldaRow = new CompanyMassiveVehicleFasecoldaRow { };

            try
            {
                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription).Distinct())).ToList();
                vehicleFasecoldaRow.ProcessId = processFasecolda.ProcessId;
                vehicleFasecoldaRow.ProcessMassiveLoadId = processFasecolda.Id;
                vehicleFasecoldaRow.RowNumber = file.Id;
                vehicleFasecoldaRow.HasError = hasError;
                vehicleFasecoldaRow.Error_Description = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                vehicleFasecoldaRow.SerializedRow = JsonConvert.SerializeObject(file);

              //  CreateProcessFasecoldaRow(vehicleFasecoldaRow);

                if (!hasError)
                {
                    Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.LoadCodesFaseColda).Rows.First();
                    templateName = file.Templates.First(x => x.PropertyName == TemplatePropertyName.LoadCodesFaseColda).Description;

                    String serializeObject = ModelAssembler.CreateSerializeObjectCode(row);
                    CreateProcessFasecoldaRow(vehicleFasecoldaRow);
                    if (!string.IsNullOrEmpty(serializeObject))
                    {
                        IQueue queue = new BaseQueueFactory().CreateQueue("FasecoldaCodeQueue", routingKey: "FasecoldaCodeQueue", serialization: "JSON");
                        queue.PutOnQueue(serializeObject);
                    }
                    else
                    {
                        vehicleFasecoldaRow.HasError = true;
                        vehicleFasecoldaRow.Error_Description += Errors.ErrorFasecoldaNotFound + KeySettings.ReportErrorSeparatorMessage();
                        CreateProcessFasecoldaRow(vehicleFasecoldaRow);
                    }
                }
                else
                {
                    vehicleFasecoldaRow.HasError = true;
                    CreateProcessFasecoldaRow(vehicleFasecoldaRow);
                }
            }
            catch (Exception ex)
            {
                processFasecolda.HasError = true;
                processFasecolda.StatusId = 2;

                if (string.IsNullOrEmpty(templateName))
                {
                    processFasecolda.Error_Description += Errors.ErrorCreateCodeFasecolda;
                    EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                }
                else
                {
                    string[] messages = ex.Message.Split('|');
                    processFasecolda.Error_Description += string.Format(Errors.ErrorInTemplate, templateName, messages[0]);
                }

                UpdateProcessFasecolda(processFasecolda);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        public void CreateFasecoldaValue(string businessCollection)
        {
            try
            {
                VehicleFasecoldaBussiness fasecoldaBussiness = new VehicleFasecoldaBussiness();
                fasecoldaBussiness.CreateFasecoldaValue(businessCollection);
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorCreatePriceFasecolda);
            }
        }

        public void CreateFasecoldaCode(string businessCollection)
        {
            try
            {
                VehicleFasecoldaBussiness fasecoldaBussiness = new VehicleFasecoldaBussiness();
                fasecoldaBussiness.CreateFasecoldaCode(businessCollection);
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorCreateCodeFasecolda);
            }
        }

        public string GetErrorExcelProcessVehicleFasecolda(int loadProcessId)
        {
            try
            {
                FaseColdaProcessDAO faseColdaProcessDAO = new FaseColdaProcessDAO();
                return faseColdaProcessDAO.GetErrorExcelProcessVehicleFasecolda(loadProcessId);
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorGetFile);
            }
        }

        public List<CompanyProcessFasecoldaMassiveLoad> GetProcessMassiveVehiclefasecolda(int loadProcessId)
        {
            try
            {
                FaseColdaLoadDAO faseColdaProcessDAO = new FaseColdaLoadDAO();
                return faseColdaProcessDAO.GetProcessMassiveVehiclefasecolda(loadProcessId);
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorGetProcess);
            }
        }

        public CompanyProcessFasecolda CreateProcessFasecolda(CompanyProcessFasecolda processFasecolda)
        {
            try
            {
                FaseColdaLoadDAO faseColdaLoadDAO = new FaseColdaLoadDAO();
                return faseColdaLoadDAO.CreateProcessFasecolda(processFasecolda);
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorCreateProcessFasecolda);
            }
        }

        public CompanyProcessFasecolda UpdateProcessFasecolda(CompanyProcessFasecolda processFasecolda)
        {
            FaseColdaLoadDAO faseColdaLoadDAO = new FaseColdaLoadDAO();
            return faseColdaLoadDAO.UpdateProcessFasecolda(processFasecolda);
        }

        public void CreateProcessFasecoldaRow(CompanyMassiveVehicleFasecoldaRow vehicleFasecoldaRow)
        {
            FaseColdaProcessDAO faseColdaProcessDAO = new FaseColdaProcessDAO();
            faseColdaProcessDAO.CreateProcessFasecoldaRow(vehicleFasecoldaRow);
        }

        public CompanyProcessFasecolda GenerateProcessMassiveVehicleFasecolda(int processId)
        {
            try
            {
                FaseColdaProcessDAO faseColdaProcessDAO = new FaseColdaProcessDAO();
                return faseColdaProcessDAO.GenerateProcessMassiveVehicleFasecolda(processId);
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorGenerateProcessMassive);
            }
        }
    }
}
