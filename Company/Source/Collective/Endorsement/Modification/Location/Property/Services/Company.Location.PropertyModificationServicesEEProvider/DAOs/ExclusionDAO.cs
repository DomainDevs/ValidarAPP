using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.Location.PropertyModificationService.EEProvider.Resources;
using Sistran.Company.Application.Location.PropertyModificationService.Models;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.Massive.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using TP = Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Location.PropertyModificationService.EEProvider.DAOs
{
    public class ExclusionDAO
    {
        public CollectiveEmission CreateCollectiveExclusion(CollectiveEmission collectiveEmission)
        {
            ValidateFile(collectiveEmission);
            collectiveEmission.Status = MassiveLoadStatus.Validating;
            collectiveEmission = DelegateService.collectiveService.CreateCollectiveEmission(collectiveEmission);

            if (collectiveEmission != null)
            {
                TP.Task.Run(() => ValidateData(collectiveEmission));
            }
            return collectiveEmission;
        }

        private void ValidateFile(CollectiveEmission collectiveEmission)
        {
            FileProcessValue fileProcessValue = new FileProcessValue()
            {
                Key1 = (int)FileProcessType.CollectiveExclusion,
                Key2 = (int)EndorsementType.Modification,
                Key4 = collectiveEmission.Prefix.Id,
                Key5 = (int)SubCoveredRiskType.Property
            };

            string fileName = collectiveEmission.File.Name;
            collectiveEmission.File = DelegateService.commonService.GetFileByFileProcessValue(fileProcessValue);

            if (collectiveEmission.File != null)
            {
                collectiveEmission.File.Name = fileName;
                collectiveEmission.File = DelegateService.commonService.ValidateFile(collectiveEmission.File, collectiveEmission.User.AccountName);
            }
            else
            {
                throw new ValidationException(Errors.ErrorFileParametrizationNotFound);
            }
        }

        private void ValidateData(CollectiveEmission collectiveEmission)
        {
            try
            {
                collectiveEmission.File = DelegateService.commonService.ValidateDataFile(collectiveEmission.File, collectiveEmission.User.AccountName);
                collectiveEmission.TotalRows = collectiveEmission.File.Templates.First(p => p.IsPrincipal).Rows.Count;
                Template policyTemplate = collectiveEmission.File.Templates.First(x => x.PropertyName == TemplatePropertyName.Policy);
                collectiveEmission.File.Templates.First(p => p.PropertyName == TemplatePropertyName.RiskExclusion).IsPrincipal = true;
                collectiveEmission.File.Templates.First(p => p.PropertyName == TemplatePropertyName.Policy).IsPrincipal = false;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                List<File> validatedFiles = DelegateService.commonService.GetDataTemplates(collectiveEmission.File.Templates);
                collectiveEmission.File.Templates.First(p => p.PropertyName == TemplatePropertyName.RiskExclusion).IsPrincipal = false;
                collectiveEmission.File.Templates.First(p => p.PropertyName == TemplatePropertyName.Policy).IsPrincipal = true;
                ExclusionValidationDAO exclusionValidationDAO = new ExclusionValidationDAO();
                List<Validation> validationsPolicy = exclusionValidationDAO.GetValidationsByFilesPolicy(collectiveEmission.File, collectiveEmission);
                if (validationsPolicy.Count > 0)
                {
                    collectiveEmission.HasError = true;
                    collectiveEmission.ErrorDescription = string.Join(",", validationsPolicy.Select(x => x.ErrorMessage));
                    return;
                }
                Row rowExclusion = policyTemplate.Rows.First();

                int policyNum = (int)DelegateService.commonService.GetValueByField<int>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumber));
                int branchId = (int)DelegateService.commonService.GetValueByField<int>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                int prefixId = (int)DelegateService.commonService.GetValueByField<int>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyPrefixCode));
                string policyText = (string)DelegateService.commonService.GetValueByField<string>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyText));
                Policy policy = DelegateService.underwritingService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNum);
                if (policy == null)
                {
                    collectiveEmission.HasError = true;
                    collectiveEmission.ErrorDescription = "Póliza no existe";
                }
                else
                {
                    CompanyPolicy companyPolicy = DelegateService.underwritingService.GetTemporalPolicyByPolicyIdEndorsementId(policy.Id, policy.Endorsement.Id);
                    int policyType = companyPolicy.PolicyType.Id;
                    collectiveEmission.Branch.Id = branchId;
                    collectiveEmission.Product.Id = policy.Product.Id;
                    collectiveEmission.DocumentNumber = policyNum;
                    collectiveEmission.Branch.Id = (int)DelegateService.commonService.GetValueByField<int>(rowExclusion.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                    collectiveEmission.Agency = companyPolicy.Agencies.FirstOrDefault();
                    companyPolicy.CurrentFrom = companyPolicy.Endorsement.CurrentTo;
                    companyPolicy.Endorsement.PolicyId = policy.Id;
                    companyPolicy.Endorsement.EndorsementType = EndorsementType.Modification;
                    companyPolicy.Endorsement.IsUnderIdenticalConditions = true;
                    companyPolicy.Endorsement.IsMassive = true;
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.Text = new Text { TextBody = policyText };
                    companyPolicy.UserId = policy.UserId;
                    companyPolicy.Branch = policy.Branch;
                    companyPolicy.CompanyProduct = DelegateService.underwritingService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                    companyPolicy.Summary = new Summary
                    {
                        RiskCount = collectiveEmission.TotalRows
                    };
                    PendingOperation pendingOperation = new PendingOperation
                    {
                        Operation = JsonConvert.SerializeObject(companyPolicy),
                        UserId = companyPolicy.UserId
                    };
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.commonService.CreatePendingOperation(pendingOperation);
                    }
                    else
                    {
                        pendingOperation = DelegateService.pendingOperationEntityService.CreatePendingOperation(pendingOperation);
                    }
                    companyPolicy.Id = pendingOperation.Id;

                    collectiveEmission.TemporalId = companyPolicy.Id;
                    collectiveEmission.DocumentNumber = companyPolicy.DocumentNumber;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                    CreateModels(collectiveEmission, validatedFiles, companyPolicy);
                }
            }
            catch (Exception ex)
            {
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = StringHelper.ConcatenateString("Error validando archivo|", ex.Message);
            }
            finally
            {
                collectiveEmission.Status = MassiveLoadStatus.Validated;
                DelegateService.massiveService.UpdateMassiveLoad(collectiveEmission);
                DataFacadeManager.Dispose();
            }

        }

        private void CreateModels(CollectiveEmission collectiveLoad, List<File> files, CompanyPolicy companyPolicy)
        {
            List<int> packages = DataFacadeManager.GetPackageProcesses(files.Count, "MaxThreadMassive");
            List<CompanyPropertyRisk> companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyPolicy.Endorsement.PolicyId);
            foreach (int package in packages)
            {
                List<File> packageFiles = files.Take(package).ToList();
                files.RemoveRange(0, package);

                Parallel.ForEach(packageFiles, (file, state, riskNum) =>
                {
                    // Este indice por el empaquetamiento de los archivos en paralelo en teoría nunca supera 
                    // la capacidad de un Int32 con esta consideración deja la conversión explicita de long a int
                    CreateModel(collectiveLoad, file, companyPolicy, companyPropertyRisks);
                });
            }
        }

        private void CreateModel(CollectiveEmission collectiveEmission, File file, CompanyPolicy companyPolicy, List<CompanyPropertyRisk> companyPropertyRisks)
        {
            string templateName = "";
            string propertyName = "";
            CollectiveEmissionRow collectiveEmissionRow = new CollectiveEmissionRow();
            
            try
            {
                Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskExclusion).Rows.First();
                int RiskNum = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.Identificator));
                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription).Distinct())).ToList();

                collectiveEmissionRow.Risk = new Risk();
                collectiveEmissionRow.Risk.Policy = companyPolicy;

                collectiveEmissionRow.MassiveLoadId = collectiveEmission.Id;
                collectiveEmissionRow.RowNumber = RiskNum;
                collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Validation;
                collectiveEmissionRow.HasError = hasError;
                collectiveEmissionRow.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                collectiveEmissionRow.SerializedRow = JsonConvert.SerializeObject(file);
                DelegateService.collectiveService.CreateCollectiveEmissionRow(collectiveEmissionRow);
                if (!hasError)
                {
                    CompanyPropertyRisk companyPropertyRisk = companyPropertyRisks.Find(x => x.Number == RiskNum);
                    if (companyPropertyRisk == null)
                    {
                        throw new ValidationException(Errors.ErrorRiskNotAssociatedWithThePolicy + " " + RiskNum);
                    }
                    companyPropertyRisk.Text = new Text();

                    companyPropertyRisk.Text.TextBody = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText));

                    DateTime currentFrom = (DateTime)DelegateService.commonService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));

                    companyPropertyRisk.CompanyRisk.Status = RiskStatusType.Excluded;
                    if (companyPropertyRisk.CompanyRisk.CompanyCoverages != null)
                    {
                        companyPropertyRisk.CompanyRisk.CompanyCoverages.ForEach(c => c.CurrentFrom = currentFrom);
                    }

                    PendingOperation pendingOperationRisk = new PendingOperation
                    {
                        ParentId = companyPolicy.Id,
                        UserId = companyPolicy.UserId,
                        OperationName = "Temporal",
                        Operation = JsonConvert.SerializeObject(companyPropertyRisk)
                    };
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperationRisk = DelegateService.commonService.CreatePendingOperation(pendingOperationRisk);
                        collectiveEmissionRow.Risk.RiskId = pendingOperationRisk.Id;
                        DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                    }
                    else
                    {
                        string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}", JsonConvert.SerializeObject(pendingOperationRisk), (char)007, JsonConvert.SerializeObject(collectiveEmissionRow), (char)007, nameof(CollectiveEmissionRow));
                        QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "CreatePendingOperationQuee");
                        
                    }

                }
            }
            catch (Exception ex)
            {
                collectiveEmissionRow.HasError = true;
                if (string.IsNullOrEmpty(templateName))
                {
                    collectiveEmissionRow.Observations += Errors.ErrorCreateRisk;
                }
                else
                {
                    string[] messages = ex.Message.Split('|');
                    collectiveEmissionRow.Observations += string.Format(Errors.ErrorInTemplate, templateName, messages[0]);
                }
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void CheckExternalServices(CacheListForProperty cacheListForProperty, CompanyPolicy companyPolicy, CompanyPropertyRisk companyProperty, CollectiveEmission massiveEmission, File file, Row row)
        {
            FilterIndividual filterIndividual = cacheListForProperty.FilterIndividuals.Find(x => x.InsuredCode == companyProperty.CompanyRisk.CompanyInsured.InsuredId);
            bool scoreAlreadyQueried = false; bool simitAlreadyQueried = false; bool requireScore = false; bool requireSimit = false; bool requireFasecolda = false;
            string licencePlate = string.Empty; string surname = string.Empty;
            IdentificationDocument identificationDocument = new IdentificationDocument();

            if (filterIndividual.IndividualType == Core.Application.UniquePersonService.Enums.IndividualType.LegalPerson)
            {
                surname = filterIndividual.Company.Name;
                identificationDocument = filterIndividual.Company.IdentificationDocument;
            }
            else if (filterIndividual.IndividualType == Core.Application.UniquePersonService.Enums.IndividualType.Person)
            {
                surname = filterIndividual.Person.Surname;
                identificationDocument = filterIndividual.Person.IdentificationDocument;
            }

            if (companyPolicy.CompanyProduct.IsScore.GetValueOrDefault())
            {
                requireScore = true;


                if (!cacheListForProperty.InsuredForScoreList.Contains(companyProperty.CompanyRisk.CompanyInsured.InsuredId))
                {
                    cacheListForProperty.InsuredForScoreList.Add(companyProperty.CompanyRisk.CompanyInsured.InsuredId);
                }
                else
                {
                    scoreAlreadyQueried = true;
                }
            }

            if (requireScore)
            {
                DelegateService.externalProxyService.CheckExternalServices(filterIndividual.Person.IdentificationDocument, surname, filterIndividual.InsuredCode.Value, licencePlate, massiveEmission.Id, row.Number, (int)SubCoveredRiskType.Property, massiveEmission.User.UserId, scoreAlreadyQueried, simitAlreadyQueried, requireScore, requireSimit, requireFasecolda);
            }
        }
    }
}