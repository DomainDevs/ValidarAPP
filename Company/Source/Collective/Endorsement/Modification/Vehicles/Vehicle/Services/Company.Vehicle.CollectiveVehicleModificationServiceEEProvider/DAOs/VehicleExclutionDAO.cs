using Newtonsoft.Json;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Vehicle.ModificationService.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicle.ModificationService.EEProvider.DAOs
{
    public class VehicleExclutionDAO
    {
        public CollectiveEmission CreateVehicleExclution(CollectiveEmission collectiveLoad)
        {
            ValidateFile(collectiveLoad);
            collectiveLoad.Status = MassiveLoadStatus.Validating;
            collectiveLoad = DelegateService.collectiveService.CreateCollectiveEmission(collectiveLoad);

            if (collectiveLoad != null)
            {
                TP.Task.Run(() => ValidateData(collectiveLoad));
            }
            return collectiveLoad;
        }

        private void ValidateData(CollectiveEmission collectiveLoad)
        {
            try
            {
                VehicleInclutionDAO vehicleInclutionDAO = new VehicleInclutionDAO();
                collectiveLoad.File = DelegateService.utilitiesService.ValidateDataFile(collectiveLoad.File, collectiveLoad.User.AccountName);
                collectiveLoad.TotalRows = collectiveLoad.File.Templates.First(x => x.IsPrincipal).Rows.Count();
                Template policyTemplate = collectiveLoad.File.Templates.First(x => x.PropertyName == TemplatePropertyName.Policy);
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
                
                List<File> validatedFiles = ValidateRows(collectiveLoad.File.Templates, FieldPropertyName.RiskLicensePlate);
                VehicleExclutionValidationDAO vehicleExclutionValidationDAO = new VehicleExclutionValidationDAO();
                List<Validation> validationsPolicy = vehicleExclutionValidationDAO.GetValidationsByFilesPolicy(collectiveLoad.File, collectiveLoad);
                
                if (validationsPolicy.Count > 0)
                {
                    collectiveLoad.HasError = true;
                    collectiveLoad.ErrorDescription = string.Join(",", validationsPolicy.Select(x => x.ErrorMessage));
                    return;
                }
                
                Row rowExclusion = policyTemplate.Rows.FirstOrDefault();
                Int64 policyNum = (Int64)DelegateService.utilitiesService.GetValueByField<Int64>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumber));
                int branchId = (int)DelegateService.utilitiesService.GetValueByField<int>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                int prefixId = (int)DelegateService.utilitiesService.GetValueByField<int>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
                string policyText = (string)DelegateService.utilitiesService.GetValueByField<string>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyText));
                if (collectiveLoad.Prefix.Id != prefixId)
                {
                    collectiveLoad.HasError = true;
                    collectiveLoad.ErrorDescription = Errors.PolicyNotCorrespondPrefix;
                    DelegateService.massiveService.UpdateMassiveLoad(collectiveLoad);
                    return;
                }
                
                Policy policy = DelegateService.underwritingService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNum);
                
                if (policy == null)
                {
                    collectiveLoad.HasError = true;
                    collectiveLoad.ErrorDescription = Errors.PolicyNotFound;
                    DelegateService.massiveService.UpdateMassiveLoad(collectiveLoad);
                    return;
                }

                if (policy.Endorsement == null || !policy.Endorsement.EndorsementType.HasValue
                        || policy.Endorsement.EndorsementType.Value == EndorsementType.Cancellation
                        || policy.Endorsement.EndorsementType.Value == EndorsementType.AutomaticCancellation
                        || policy.Endorsement.EndorsementType.Value == EndorsementType.Nominative_cancellation)
                {
                    throw new ValidationException(Errors.NullOrCancelledPolicyEndorsement);
                }

                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetTemporalPolicyByPolicyIdEndorsementId(policy.Endorsement.PolicyId, policy.Endorsement.Id);

                int policyType = companyPolicy.PolicyType.Id;
                collectiveLoad.Branch.Id = branchId;
                collectiveLoad.Product.Id = policy.Product.Id;
                collectiveLoad.DocumentNumber = policyNum;
                collectiveLoad.Branch.Id = (int)DelegateService.utilitiesService.GetValueByField<int>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                collectiveLoad.Agency = companyPolicy.Agencies.FirstOrDefault();
                companyPolicy.Endorsement.EndorsementType = EndorsementType.Modification;
                companyPolicy.Endorsement.IsUnderIdenticalConditions = true;
                companyPolicy.Endorsement.IsMassive = true;
                companyPolicy.TemporalType = TemporalType.Endorsement;
                companyPolicy.Text = new CompanyText { TextBody = policyText };
                companyPolicy.UserId = policy.UserId;
                companyPolicy.SubMassiveProcessType = SubMassiveProcessType.Exclusion;
                companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                companyPolicy.Summary = new CompanySummary
                {
                    RiskCount = collectiveLoad.TotalRows
                };
                companyPolicy.Endorsement.TicketDate = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.TicketDate));
                companyPolicy.Endorsement.TicketNumber = (int)DelegateService.utilitiesService.GetValueByField<int>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.TicketNumber));


                companyPolicy = vehicleInclutionDAO.GetMinCurrentFromByFile(validatedFiles, companyPolicy, TemplatePropertyName.RiskExclusion);

                PendingOperation pendingOperation = new PendingOperation
                {
                    IsMassive = true,
                    Operation = COMUT.JsonHelper.SerializeObjectToJson(companyPolicy),
                    UserId = companyPolicy.UserId
                };
                if (!Settings.UseReplicatedDatabase())
                {
                    pendingOperation = DelegateService.utilitiesService.CreatePendingOperation(pendingOperation);
                }
                else
                {
                    pendingOperation = DelegateService.pendingOperationEntityService.CreatePendingOperation(pendingOperation);
                }
                companyPolicy.Id = pendingOperation.Id;

                collectiveLoad.TemporalId = companyPolicy.Id;
                collectiveLoad.DocumentNumber = companyPolicy.DocumentNumber;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);

                CreateModels(collectiveLoad, validatedFiles, companyPolicy);
                DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveLoad.Id);
            }
            catch (Exception ex)
            {
                collectiveLoad.HasError = true;
                collectiveLoad.ErrorDescription += StringHelper.ConcatenateString(Errors.ErrorValidatingFile + " " + ex.GetBaseException().Message, "|");
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void ValidateFile(CollectiveEmission collectiveLoad)
        {
            FileProcessValue fileProcessValue = new FileProcessValue()
            {
                Key1 = (int)FileProcessType.CollectiveExclusion,
                Key2 = (int)EndorsementType.Modification,
                Key4 = collectiveLoad.Prefix.Id,
                Key5 = (int)SubCoveredRiskType.Vehicle
            };

            string fileName = collectiveLoad.File.Name;
            collectiveLoad.File = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);

            if (collectiveLoad.File != null)
            {
                collectiveLoad.File.Name = fileName;
                collectiveLoad.File = DelegateService.utilitiesService.ValidateFile(collectiveLoad.File, collectiveLoad.User.AccountName);
            }
            else
            {
                throw new ValidationException(Errors.ErrorFileParametrizationNotFound);
            }
        }

        private void CreateModels(CollectiveEmission collectiveLoad, List<File> files, CompanyPolicy companyPolicy)
        {        
            List<CompanyVehicle> companyVehicleRisks = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(companyPolicy.Endorsement.PolicyId);
            if (companyVehicleRisks != null && companyVehicleRisks.Count > 0)
            {
                ValidatePolicyExcludeAllRisks(companyVehicleRisks, files);
                Parallel.ForEach(files, ParallelHelper.DebugParallelFor(), file =>
                 {
                     CreateModel(collectiveLoad, file, companyPolicy, companyVehicleRisks);
                 });
            }
            else
            {
                throw new ValidationException(Errors.ErrorRisksNotFound);
            }
        }

        private void ValidatePolicyExcludeAllRisks(List<CompanyVehicle> companyVehicleRisks, List<File> files)
        {

            List<string> currentRisks = companyVehicleRisks.Select(u => u.LicensePlate.ToUpper()).ToList();
            List<string> excludeRisks = new List<string>();
            Row row;
            foreach (var file in files)
            {
                row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskExclusion).Rows.First();
                string licensePlate = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLicensePlate));

                if (!string.IsNullOrEmpty(licensePlate))
                {
                    excludeRisks.Add(licensePlate.ToUpper());
                }
            }
            List<string> result = currentRisks.Except(excludeRisks).ToList();

            if (!result.Any())
            {
                throw new ValidationException(Errors.ErrorExcludeAllRisks);
            }
        }

        private void CreateModel(CollectiveEmission collectiveLoad, File file, CompanyPolicy companyPolicy, List<CompanyVehicle> companyVehicleRisks)
        {
            string templateName = "";
            CollectiveEmissionRow collectiveLoadProcess = new CollectiveEmissionRow();
            try
            {
                Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskExclusion).Rows.First();
                string licencePlate = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLicensePlate));
                string documentNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumber));
                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription).Distinct())).ToList();
                
                collectiveLoadProcess.MassiveLoadId = collectiveLoad.Id;
                collectiveLoadProcess.RowNumber = file.Id;
                collectiveLoadProcess.Status = CollectiveLoadProcessStatus.Validation;
                collectiveLoadProcess.HasError = hasError;
                collectiveLoadProcess.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                collectiveLoadProcess.SerializedRow = COMUT.JsonHelper.SerializeObjectToJson(file);
                DelegateService.collectiveService.CreateCollectiveEmissionRow(collectiveLoadProcess);
                
                if (!hasError)
                {
                    Template templateEmision = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Policy);
                    templateName = templateEmision.Description;
                    CompanyVehicle companyVehicleRisk = companyVehicleRisks.Find(x => x.LicensePlate == licencePlate.ToUpper());
                    if (companyVehicleRisk == null)
                    {
                        throw new ValidationException(Errors.ErrorRiskNotAssociatedWithThePolicy + " " + documentNumber);
                    }
                    companyVehicleRisk.Risk.Text = new CompanyText();
                    companyVehicleRisk.Risk.Text.TextBody = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText));


                    DateTime currentFrom = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));

                    companyVehicleRisk.Risk.Status = RiskStatusType.Excluded;
                    if (currentFrom != null)
                    {
                        if (currentFrom < companyPolicy.CurrentFrom || currentFrom > companyPolicy.CurrentTo)
                        {
                            throw new Exception(Errors.ErrorCurrentFrom);
                        }
                        companyVehicleRisk.Risk.Coverages.AsParallel().ForAll(x => x.CurrentFrom = currentFrom);
                    }
                    string pendingOperationJsonIsnotNull = COMUT.JsonHelper.SerializeObjectToJson(companyVehicleRisk);
                    PendingOperation pendingOperationRisk = new PendingOperation
                    {
                        ParentId = companyPolicy.Id,
                        UserId = companyPolicy.UserId,
                        IsMassive = companyPolicy.Endorsement.IsMassive.Value,
                        OperationName = "Temporal",
                        Operation = pendingOperationJsonIsnotNull,
                    };

                    if (Settings.UseReplicatedDatabase())
                    {
                        string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}", COMUT.JsonHelper.SerializeObjectToJson(pendingOperationRisk), (char)007, COMUT.JsonHelper.SerializeObjectToJson(collectiveLoadProcess), (char)007, nameof(CollectiveEmissionRow));
                        QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "CreatePendingOperationQuee");

                    }
                    else
                    {
                        pendingOperationRisk = DelegateService.utilitiesService.CreatePendingOperation(pendingOperationRisk);
                        collectiveLoadProcess.Risk = new Risk()
                        {
                            RiskId = pendingOperationRisk.Id
                        };
                        DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveLoadProcess);
                    }
                }
                else
                {
                    DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveLoad.Id);
                }
            }
            catch (Exception ex)
            {
                collectiveLoadProcess.HasError = true;
                if (string.IsNullOrEmpty(templateName))
                {
                    collectiveLoadProcess.Observations += Errors.ErrorCreateRisk;
                    EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                }
                else
                {
                    string[] messages = ex.GetBaseException().Message.Split('|');
                    collectiveLoadProcess.Observations += string.Format(Errors.ErrorInTemplate, templateName, messages[0]);
                }
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveLoadProcess);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        public List<File> ValidateRows(List<Template> templates, string fielPropertyName)
        {
            int rowCount = 1;
            List<File> files = new List<File>();
            Template templatePrincipal = templates.First(x => x.IsPrincipal);

            foreach (Row row in templatePrincipal.Rows)
            {
                File file = new File
                {
                    Templates = new List<Template>()
                };

                file.Id = rowCount;
                file.Name = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.First(x => x.PropertyName == (string.IsNullOrEmpty(fielPropertyName) ? FieldPropertyName.Identificator : fielPropertyName)));

                file.Templates.Add(new Template
                {

                    PropertyName = templatePrincipal.PropertyName,
                    IsPrincipal = templatePrincipal.IsPrincipal,
                    Rows = new List<Row>()
                });

                //DelegateService.commonService.ValidateDataRow(row);

                if (!row.HasError)
                {
                    foreach (Template template in templates.Where(x => !x.IsPrincipal))
                    {
                        file.Templates.Add(template);
                    }
                }

                file.Templates[0].Rows.Add(row);

                files.Add(file);
                rowCount++;
            }

            return files;
        }
    }
}