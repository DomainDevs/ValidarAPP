using Newtonsoft.Json;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistran.Company.Application.Location.CollectivePropertyRenewalService.Models;
using Sistran.Company.Application.Location.CollectivePropertyRenewalServiceEEProvider.Resources;
using Sistran.Company.Application.Massive.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.UniquePersonServices.Models;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Helper;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.CollectivePropertyRenewalServiceEEProvider.DAOs
{
    public class RenewalDAO
    {
        public CollectiveEmission CreateCollectiveRenewal(CollectiveEmission collectiveEmission)
        {
            ValidateFile(collectiveEmission);
            collectiveEmission.Status = MassiveLoadStatus.Validating;
            collectiveEmission = DelegateService.collectiveService.CreateCollectiveEmission(collectiveEmission);
            if (collectiveEmission == null)
            {
                return null;
            }
            TP.Task.Run(() => ValidateData(collectiveEmission));
            return collectiveEmission;
        }

        private void ValidateFile(CollectiveEmission collectiveLoad)
        {
            FileProcessValue fileProcessValue = new FileProcessValue
            {
                Key1 = (int)FileProcessType.CollectiveRenewal,
                Key2 = (int)EndorsementType.Renewal,
                Key4 = collectiveLoad.Prefix.Id,
                Key5 = (int)SubCoveredRiskType.Property
            };
            string fileName = collectiveLoad.File.Name;
            collectiveLoad.File = DelegateService.commonService.GetFileByFileProcessValue(fileProcessValue);

            if (collectiveLoad.File != null)
            {
                collectiveLoad.File.Name = fileName;
                Template riskDetailTemplate = collectiveLoad.File.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail);
                collectiveLoad.File = DelegateService.commonService.ValidateFile(collectiveLoad.File, collectiveLoad.User.AccountName);

                if (!collectiveLoad.File.Templates.Exists(x => x.PropertyName == TemplatePropertyName.RiskDetail))
                {
                    riskDetailTemplate.Rows.RemoveRange(0, riskDetailTemplate.Rows.Count - 1);
                    collectiveLoad.File.Templates.Add(riskDetailTemplate);
                }
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
                var file = collectiveEmission.File;
                Template policyTemplate = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Renewal);
                file.Templates.Remove(policyTemplate);
                policyTemplate = DelegateService.commonService.ValidateDataTemplate(collectiveEmission.File.Name, collectiveEmission.User.AccountName, policyTemplate);
                RenewalValidationDAO renewalValidationDAO = new RenewalValidationDAO();
                List<Validation> emissionValidations = renewalValidationDAO.GetValidationsByEmissionTemplate(policyTemplate);
                if (emissionValidations.Count > 0)
                {
                    collectiveEmission.HasError = true;
                    collectiveEmission.ErrorDescription = string.Join(",", emissionValidations.Select(x => x.ErrorMessage));
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                    return;
                }

                List<File> validatedFiles = new List<File>();

                if (file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows[0].Fields[0].Value != null)
                {
                    file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).IsPrincipal = true;
                    file = DelegateService.commonService.ValidateDataFile(file, collectiveEmission.User.AccountName);
                    validatedFiles = DelegateService.commonService.GetDataTemplates(file.Templates);
                }

                Row rowRenewal = policyTemplate.Rows.First();
                decimal policyId = (decimal)DelegateService.commonService.GetValueByField<decimal>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumber));
                int branchId = (int)DelegateService.commonService.GetValueByField<int>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                int prefixId = (int)DelegateService.commonService.GetValueByField<int>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyPrefixCode));
                DateTime currentTo = (DateTime)DelegateService.commonService.GetValueByField<DateTime>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentTo));
                string policyText = (string)DelegateService.commonService.GetValueByField<string>(rowRenewal.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyText));
                Policy policy = DelegateService.underwritingService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyId);
                if (policy == null)
                {
                    collectiveEmission.HasError = true;
                    collectiveEmission.ErrorDescription = Errors.PolicyNotFound;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                    return;
                }
                policy.Clauses = DelegateService.massiveService.GetClauses(file.Templates.Find(p => p.PropertyName == TemplatePropertyName.Clauses), EmissionLevel.General);
                if (validatedFiles.Any())
                {
                    List<Validation> validations = renewalValidationDAO.GetValidationsByFiles(validatedFiles, collectiveEmission, policy.Product.Id);
                    if (validations.Count > 0)
                    {
                        foreach (File validatedFile in validatedFiles)
                        {
                            List<Validation> fileValidations = validations.Where(x => x.Id == validatedFile.Id).ToList();
                            if (fileValidations.Any())
                            {
                                validatedFile.Templates[0].Rows[0].HasError = true;
                                validatedFile.Templates[0].Rows[0].ErrorDescription = string.Join(",", fileValidations.Select(x => x.ErrorMessage));
                            }
                        }
                    }
                }
                policy.CurrentTo = currentTo;
                policy.Text = new Text
                {
                    TextBody = policyText
                };
                CreateModels(collectiveEmission, validatedFiles, policy, policyTemplate);
                DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveEmission.Id);

            }
            catch (Exception ex)
            {
                collectiveEmission.Status = MassiveLoadStatus.Validated;
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = ex.Message;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void CreateModel(CollectiveEmission collectiveEmission, List<File> files, CompanyPolicy companyPolicy, CompanyPropertyRisk companyPropertyRisk, CacheListForProperty cacheListForProperty, Template templatePolicy)
        {
            CollectiveEmissionRow collectiveEmissionRow = new CollectiveEmissionRow
            {
                Risk = new Risk()
            };

            try
            {
                bool hasError = false;
                List<string> errorList = new List<string>();
                File file = new File
                {
                    Templates = new List<Template>()
                };

                file.Templates.Add(templatePolicy);

                if (files.Exists(x => x.Templates.Exists(y => y.PropertyName == TemplatePropertyName.RiskDetail)) && files.Exists(x => x.Id == companyPropertyRisk.Number))
                {
                    file.Templates.Add(files.First(x => x.Id == companyPropertyRisk.Number).Templates.First(y => y.PropertyName == TemplatePropertyName.RiskDetail));
                }
                else
                {
                    file.Templates.Add(collectiveEmission.File.Templates.First(y => y.PropertyName == TemplatePropertyName.RiskDetail));
                }

                hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription))
                            .Select(r => r.ErrorDescription))).ToList();

                collectiveEmissionRow.MassiveLoadId = collectiveEmission.Id;
                collectiveEmissionRow.RowNumber = companyPropertyRisk.Number;
                collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Validation;
                collectiveEmissionRow.HasError = hasError;
                collectiveEmissionRow.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                collectiveEmissionRow.SerializedRow = JsonConvert.SerializeObject(file);
                DelegateService.collectiveService.CreateCollectiveEmissionRow(collectiveEmissionRow);
                if (hasError)
                {
                    return;
                }

                companyPropertyRisk.CompanyRisk.CompanyCoverages.ForEach(x =>
                {
                    x.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                    x.CurrentFrom = companyPolicy.CurrentFrom;
                    x.CurrentTo = companyPolicy.CurrentTo;
                    x.CoverStatus = CoverageStatusType.Original;
                });

                if (file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows.Count > 0)
                {
                    Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows.First();
                    companyPropertyRisk.CompanyRisk.Status = RiskStatusType.Modified;
                    companyPropertyRisk.Text.TextBody = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText));
                    decimal riskRateTri = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRateTRI));

                    companyPropertyRisk.CompanyRisk.CompanyInsured.ClaimIncurred = new ClaimIncurred();
                    decimal? accidentRate = DelegateService.uniquePersonService.GetSinisterPercentageByInsuredId(companyPropertyRisk.CompanyRisk.CompanyInsured.InsuredId).AccidentRate;

                    if (accidentRate == null)
                    {
                        if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskSinisterPercentage).Value))
                        {
                            companyPropertyRisk.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskSinisterPercentage));
                            if (companyPropertyRisk.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate < 0 || companyPropertyRisk.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate > 100)
                            {
                                collectiveEmissionRow.HasError = true;
                                collectiveEmissionRow.Observations += string.Format(Errors.ErrorAccidentRateValue) + KeySettings.ReportErrorSeparatorMessage();
                            }
                        }
                        else
                        {
                            collectiveEmissionRow.HasError = true;
                            collectiveEmissionRow.Observations += string.Format(Errors.AccidentRateMandatory) + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }
                    else
                    {
                        companyPropertyRisk.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate = accidentRate.Value;
                    }

                    ModifyCoverages(companyPropertyRisk, file.Templates.Find(x => x.PropertyName == TemplatePropertyName.ModifyCoverages));
                    ModifyInsuredObjects(companyPropertyRisk, file.Templates.Find(x => x.PropertyName == TemplatePropertyName.InsuredObjects), riskRateTri);

                    Template templateClauses = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.Clauses);
                    companyPropertyRisk.Clauses = DelegateService.massiveService.GetClauses(templateClauses, EmissionLevel.Risk);
                    companyPropertyRisk.CompanyRisk.CompanyCoverages.ForEach(p => p.Clauses = DelegateService.massiveService.GetClausesByCoverageId(templateClauses, p.Id));
                }


                if (Settings.ImplementWebServices())
                {
                    if (companyPolicy.CompanyProduct.IsScore.GetValueOrDefault())
                    {
                        companyPropertyRisk.CompanyRisk.CompanyInsured.ScoreCredit = cacheListForProperty.FilterIndividuals.Find(x => x.InsuredCode == companyPropertyRisk.CompanyRisk.CompanyInsured.InsuredId).ScoreCredit;
                    }
                }
                else
                {
                    companyPropertyRisk.CompanyRisk.CompanyInsured.ScoreCredit = DelegateService.externalProxyService.GetScoreDefault();
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
                    var queue = new Core.Framework.Queues.BaseQueueFactory().CreateQueue("CreatePendingOperationQuee", routingKey: "CreatePendingOperationQuee", serialization: "JSON");
                    queue.PutOnQueue(pendingOperationJson);
                }

                if (Settings.ImplementWebServices() &&
                    (companyPolicy.CompanyProduct.IsScore.GetValueOrDefault() && companyPropertyRisk.CompanyRisk.CompanyInsured.ScoreCredit == null))
                {
                    CheckExternalServices(cacheListForProperty, companyPolicy, companyPropertyRisk, collectiveEmission);
                }

            }
            catch (Exception ex)
            {
                collectiveEmissionRow.HasError = true;
                collectiveEmissionRow.Observations = ex.Message;
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
            }
        }

        private void CheckExternalServices(CacheListForProperty cacheListForProperty, CompanyPolicy companyPolicy, CompanyPropertyRisk companyProperty, CollectiveEmission collectiveEmission)
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
                DelegateService.externalProxyService.CheckExternalServices(filterIndividual.Person.IdentificationDocument, surname, filterIndividual.InsuredCode.Value, licencePlate, collectiveEmission.Id, 1, (int)SubCoveredRiskType.Property, collectiveEmission.User.UserId, scoreAlreadyQueried, simitAlreadyQueried, requireScore, requireSimit, requireFasecolda);
            }
        }

        private void CreateModels(CollectiveEmission collectiveLoad, List<File> files, Policy policy, Template policyTemplate)
        {
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetTemporalPolicyByPolicyIdEndorsementId(policy.Id, policy.Endorsement.Id);
            companyPolicy.Id = policy.Id;
            policy.Product.IsCollective = companyPolicy.CompanyProduct.IsCollective;
            collectiveLoad.Product = policy.Product;
            collectiveLoad.Branch = policy.Branch;
            collectiveLoad.Agency = companyPolicy.Agencies.FirstOrDefault();
            DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
            companyPolicy.CurrentFrom = companyPolicy.Endorsement.CurrentTo;
            companyPolicy.CurrentTo = policy.CurrentTo;
            companyPolicy.Endorsement.EndorsementType = EndorsementType.Renewal;
            companyPolicy.Endorsement.IsUnderIdenticalConditions = true;
            companyPolicy.Endorsement.IsMassive = true;
            companyPolicy.Endorsement.EndorsementDays = (companyPolicy.CurrentTo - companyPolicy.CurrentFrom).Days;
            companyPolicy.TemporalType = TemporalType.Endorsement;
            if (companyPolicy.Text == null)
            {
                companyPolicy.Text = new Text();
            }
            companyPolicy.Text.TextBody = policy.Text.TextBody;
            companyPolicy.UserId = collectiveLoad.User.UserId;
            companyPolicy.CompanyProduct = DelegateService.underwritingService.GetCompanyProductByProductIdPrefixId(companyPolicy.CompanyProduct.Id, companyPolicy.Prefix.Id);

            List<CompanyPropertyRisk> propertyRisks = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyPolicy.Id);

            if (propertyRisks.Count == 0)
            {
                throw new ValidationException(Errors.ErrorWithOutPropertyRisks);
            }

            if (!companyPolicy.CompanyProduct.IsCollective && propertyRisks.Count == 1)
            {
                collectiveLoad.HasError = true;
                collectiveLoad.ErrorDescription = Errors.PolicyIsNotCollective;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
                return;
            }

            CacheListForProperty cacheListForPropety = new CacheListForProperty();
            cacheListForPropety.Alliances = DelegateService.uniquePersonService.GetAlliances();
            cacheListForPropety.InsuredForScoreList = new List<int>();
            List<FilterIndividual> filterIndividuals = DelegateService.massiveService.GetDataFilterIndividualRenewalWithPropertyNames(files, TemplatePropertyName.Renewal, FieldPropertyName.PolicyNumber, FieldPropertyName.PolicyPrefixCode).Where(p => p.IsCLintonList == false).ToList();
            List<FilterIndividual> individualWithError = new List<FilterIndividual>();

            individualWithError.AddRange(filterIndividuals.Where(i => i.IsCLintonList == true));
            individualWithError.AddRange(filterIndividuals.Where(i => !string.IsNullOrEmpty(i.Error)));

            filterIndividuals.RemoveAll(i => i.IsCLintonList == true);
            filterIndividuals.RemoveAll(i => !string.IsNullOrEmpty(i.Error));

            if (Settings.UseReplicatedDatabase())
            {
                filterIndividuals = DelegateService.externalProxyMirrorService.GetMassiveScoresCreditByLastValid(filterIndividuals, companyPolicy.Prefix.Id, companyPolicy.UserId);
            }
            else
            {
                filterIndividuals = DelegateService.externalProxyService.GetMassiveScoresCreditByLastValid(filterIndividuals, companyPolicy.Prefix.Id, companyPolicy.UserId);
            }

            cacheListForPropety.FilterIndividuals = filterIndividuals;
            cacheListForPropety.FilterIndividuals.AddRange(individualWithError);

            if (Settings.ImplementWebServices())
            {
                //Vaidación Externos 
                Alliance validateAlliance = new Alliance();
                if (companyPolicy.Alliance != null)
                {
                    validateAlliance = cacheListForPropety.Alliances.Find(x => x.Id == companyPolicy.Alliance.Id);
                }

                companyPolicy.CompanyProduct.IsScore = DelegateService.externalProxyService.ValidateApplyScoreCreditByProduct(companyPolicy.CompanyProduct, validateAlliance, companyPolicy.Prefix.Id);
                //Vaidación Externos 
            }

            int totalRisks = propertyRisks.Count;
            companyPolicy.Summary = new Summary
            {
                RiskCount = totalRisks
            };
            collectiveLoad.TotalRows = totalRisks;

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
            collectiveLoad.TemporalId = companyPolicy.Id;
            DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);

            ParallelHelper.ForEach(propertyRisks, propertyRisk =>
            {
                CreateModel(collectiveLoad, files, companyPolicy, propertyRisk, cacheListForPropety, policyTemplate);
            });
        }

        private void ModifyInsuredObjects(CompanyPropertyRisk companyPropertyRisk, Template insuredObjectTemplate, decimal RiskRateTri)
        {
            if (insuredObjectTemplate != null)
            {
                foreach (Row row in insuredObjectTemplate.Rows)
                {
                    int id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                    CompanyInsuredObject companyInsuredObject = companyPropertyRisk.CompanyRisk.CompanyCoverages.Find(c => c.CompanyInsuredObject.Id == id).CompanyInsuredObject;
                    int amount = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectSumAssured));
                    if (amount > 0)
                    {
                        companyInsuredObject.Amount = amount;
                    }
                    decimal rateTri = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RiskRateTRI));
                    if (rateTri > 0)
                    {
                        companyInsuredObject.RateTRI = rateTri;
                    }
                    decimal percentageVariableIndex = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.FirstOrDefault(x => x.PropertyName == CompanyFieldPropertyName.RiskPercentageVariableIndex));
                    if (percentageVariableIndex > 0)
                    {
                        companyInsuredObject.PercentageVariableIndex = percentageVariableIndex;
                    }
                    int recoupmentPeriodId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == CompanyFieldPropertyName.RecoupmentPeriodId));
                    if (recoupmentPeriodId > 0)
                    {
                        companyInsuredObject.RecoupmentPeriod.Id = recoupmentPeriodId;
                    }
                }
                if (RiskRateTri > 0)
                {
                    companyPropertyRisk.CompanyRisk.CompanyCoverages.ForEach(x => x.CompanyInsuredObject.RateTRI = RiskRateTri);
                }
            }
        }

        private void ModifyCoverages(CompanyPropertyRisk companyPropertyRisk, Template modiffiedCoveragesTemplate)
        {
            if (modiffiedCoveragesTemplate != null)
            {
                foreach (Row row in modiffiedCoveragesTemplate.Rows)
                {
                    int id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                    int insuredObjectId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                    int deductibleId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                    CompanyCoverage companyCoverage = companyPropertyRisk.CompanyRisk.CompanyCoverages.Find(c => c.Id == id && c.CompanyInsuredObject.Id == insuredObjectId && c.Deductible.Id == deductibleId);
                    if (companyCoverage != null)
                    {
                        decimal rate = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.CoverageRate));
                        companyCoverage.Rate = rate;
                        companyCoverage.CoverStatus = CoverageStatusType.Modified;
                    }
                    else
                    {
                        CompanyCoverage newCompanyCoverage = new CompanyCoverage()
                        {
                            Id = id,
                            CompanyInsuredObject = new CompanyInsuredObject()
                            {
                                Id = insuredObjectId
                            },
                            Deductible = new Deductible()
                            {
                                Id = deductibleId
                            },
                            CoverStatus = CoverageStatusType.Included
                        };
                        companyPropertyRisk.CompanyRisk.CompanyCoverages.Add(newCompanyCoverage);
                    }
                }
            }
        }
    }
}