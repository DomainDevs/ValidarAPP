using Newtonsoft.Json;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.Massive.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Locations.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Sistran.Company.Application.Location.PropertyCollectiveService.EEProvider.Resources;
using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.Location.PropertyCollectiveService.Models;
using Sistran.Core.Application.Utilities.Configuration;
using System.Diagnostics;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.PropertyCollectiveService.EEProvider.DAOs
{
    public class CollectiveEmissionPropertyDAO
    {
        string templateName = "";
        private static List<DocumentType> documentTypes = new List<DocumentType>();
        private static List<Deductible> deductibles = new List<Deductible>();
        private static List<CompanyIrregularHeight> irregularHeights = new List<CompanyIrregularHeight>();
        private static List<CompanyIrregularPlant> irregularPlants = new List<CompanyIrregularPlant>();
        private static List<CompanyLevelZone> levelzones = new List<CompanyLevelZone>();
        private static List<CompanyLocation> locations = new List<CompanyLocation>();
        private static List<CompanyReinforcedStructureType> reinforcedStructureTypes = new List<CompanyReinforcedStructureType>();
        private static List<CompanyRepair> companyRepairs = new List<CompanyRepair>();
        private static List<CompanyStructureType> companyStructureTypes = new List<CompanyStructureType>();
        private static List<RiskUse> riskUse = new List<RiskUse>();
        private static List<Core.Application.Locations.Models.RiskType> riskTypes = new List<Core.Application.Locations.Models.RiskType>();
        private static List<ConstructionType> constructionTypes = new List<ConstructionType>();
        private static List<LineBusiness> lineBusiness = new List<LineBusiness>();
        private static List<SubLineBusiness> subLineBusiness = new List<SubLineBusiness>();
        private static List<CompanyInsuredObject> insuredObjects = new List<CompanyInsuredObject>();
        private static List<City> cities = new List<City>();
        private static List<State> states = new List<State>();
        private List<CompanyDamage> damages = null;
        private List<RatingZone> ratingZones = new List<RatingZone>();
        private List<CompanyMicroZone> companyMicroZones = new List<CompanyMicroZone>();
        private static List<Currency> currency = new List<Currency>();
        private static List<PaymentMethod> paymentMethod = new List<PaymentMethod>();
        private static List<PolicyType> policiyType = new List<PolicyType>();
        ConcurrentBag<Row> concurrentRows = new ConcurrentBag<Row>();

        public CollectiveEmission CreateCollectiveEmission(CollectiveEmission CollectiveEmission)
        {
            ValidateFile(CollectiveEmission);
            CollectiveEmission.Status = MassiveLoadStatus.Validating;
            CollectiveEmission = DelegateService.collectiveService.CreateCollectiveEmission(CollectiveEmission);
            if (CollectiveEmission == null)
            {
                return null;
            }
            TP.Task.Run(() => ValidateData(CollectiveEmission));
            return CollectiveEmission;
        }

        private void ValidateFile(CollectiveEmission collectiveEmission)
        {
            FileProcessValue fileProcessValue = new FileProcessValue()
            {
                Key1 = (int)FileProcessType.CollectiveEmission,
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
                var file = collectiveEmission.File;
                file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).IsPrincipal = true;
                file = DelegateService.commonService.ValidateDataFile(file, collectiveEmission.User.AccountName);
                Template emissionTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.EmissionProperty);

                emissionTemplate = DelegateService.commonService.ValidateDataTemplate(file.Name, collectiveEmission.User.AccountName, emissionTemplate);

                if (emissionTemplate.Rows[0].HasError)
                {
                    collectiveEmission.Status = MassiveLoadStatus.Validated;
                    collectiveEmission.HasError = true;

                    string formatedError = emissionTemplate.Rows[0].ErrorDescription.Replace("|", ",");
                    formatedError = formatedError.Remove(formatedError.Trim().Length - 1);

                    int maxLength = formatedError.Length;
                    if (maxLength > 300)
                    {
                        formatedError = formatedError.Substring(0, 300) + "...";
                    }

                    if (string.IsNullOrEmpty(templateName))
                    {
                        collectiveEmission.ErrorDescription += $"{Errors.ErrorCreateMassiveLoad} {formatedError}";
                    }
                    else
                    {
                        collectiveEmission.ErrorDescription += string.Format(Errors.ErrorInTemplate, templateName, formatedError);
                    }

                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                    return;
                }

                Template additionalItermediariesTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.AdditionalIntermediaries);
                Template coInsurancedAceptedTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.CoinsuranceAccepted);
                Template coInsurancedAssignedTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.CoinsuranceAssigned);
                file.Templates.Remove(emissionTemplate);
                collectiveEmission.TotalRows = file.Templates.First(p => p.IsPrincipal).Rows.Count;
                DelegateService.massiveService.UpdateMassiveLoad(collectiveEmission);
                List<File> validatedFiles = DelegateService.commonService.GetDataTemplates(file.Templates);
                Row emissionRow = emissionTemplate.Rows.First();

                CollectiveEmissionPropertyValidationDAO collectiveEmissionPropertyValidationDAO = new CollectiveEmissionPropertyValidationDAO();
                List<Validation> validations = collectiveEmissionPropertyValidationDAO.GetValidationsByEmissionTemplate(file, emissionRow, collectiveEmission);

                if (validations.Count > 0)
                {
                    Validation validation = validations.Find(x => x.Id == file.Id);

                    collectiveEmission.Status = MassiveLoadStatus.Validated;
                    collectiveEmission.HasError = true;

                    string formatedError = validation.ErrorMessage.Replace("|", ",");

                    int maxLength = formatedError.Length;
                    if (maxLength > 300)
                    {
                        formatedError = formatedError.Substring(0, 300) + "...";
                    }

                    if (string.IsNullOrEmpty(templateName))
                    {
                        collectiveEmission.ErrorDescription += $"{Errors.ErrorCreateMassiveLoad} {formatedError}";
                    }
                    else
                    {
                        collectiveEmission.ErrorDescription += string.Format(Errors.ErrorInTemplate, templateName, formatedError);
                    }
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                    return;
                }

                int productId = (int)DelegateService.commonService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
                int branchId = (int)DelegateService.commonService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                collectiveEmission.Branch.Id = branchId;
                collectiveEmission.Product = DelegateService.underwritingService.GetProductById(productId);
                if (!collectiveEmission.Product.IsCollective)
                {
                    throw new ValidationException(Errors.ErrorProductIsNotCollective);
                }

                validations = collectiveEmissionPropertyValidationDAO.GetValidationsByFiles(validatedFiles, collectiveEmission, productId, emissionRow);
                if (validations.Count > 0)
                {
                    int policyValidationsId = validatedFiles.Count + 1;
                    var policyValidations = validations.Where(x => x.Id == policyValidationsId).ToList();
                    string policyValidationsMessage = "";
                    if (policyValidations.Any())
                    {
                        policyValidationsMessage = string.Join(",", policyValidations.Select(x => x.ErrorMessage)) + "|";
                    }
                    validations.RemoveAll(x => x.Id == policyValidationsId);
                    foreach (File validatedFile in validatedFiles)
                    {
                        List<Validation> fileValidations = validations.Where(x => x.Id == validatedFile.Id).ToList();
                        string riskErrors = "";
                        if (fileValidations.Any())
                        {
                            riskErrors = string.Join(",", fileValidations.Select(x => x.ErrorMessage));
                        }
                        string error = policyValidationsMessage + riskErrors;
                        if (!string.IsNullOrEmpty(error))
                        {
                            validatedFile.Templates[0].Rows[0].HasError = true;
                            validatedFile.Templates[0].Rows[0].ErrorDescription = error;
                        }
                    }
                }
                List<Template> lstTemplatesPolicy = new List<Template>();
                if (emissionTemplate != null)
                {
                    lstTemplatesPolicy.Add(emissionTemplate);
                }
                if (coInsurancedAceptedTemplate != null)
                {
                    lstTemplatesPolicy.Add(coInsurancedAceptedTemplate);
                }
                if (additionalItermediariesTemplate != null)
                {
                    lstTemplatesPolicy.Add(additionalItermediariesTemplate);
                }
                if (coInsurancedAssignedTemplate != null)
                {
                    lstTemplatesPolicy.Add(coInsurancedAssignedTemplate);
                }
                CreateModels(collectiveEmission, validatedFiles, lstTemplatesPolicy);

                DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveEmission.Id);
            }
            catch (Exception ex)
            {
                collectiveEmission.Status = MassiveLoadStatus.Validated;
                collectiveEmission.HasError = true;

                if (string.IsNullOrEmpty(templateName))
                {
                    collectiveEmission.ErrorDescription += Errors.ErrorCreateMassiveLoad + " : " + ex.Message;
                }
                else
                {
                    string[] messages = ex.Message.Split('|');
                    collectiveEmission.ErrorDescription += string.Format(Errors.ErrorInTemplate, templateName, messages[0]);
                }

                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void CreateModels(CollectiveEmission collectiveLoad, List<File> files, List<Template> emissionTemplate)
        {
            CacheListForProperty cacheListForPropety = new CacheListForProperty();
            CompanyPolicy policy = new CompanyPolicy();
            Row emissionRow = emissionTemplate.Find(t => t.PropertyName == TemplatePropertyName.EmissionProperty).Rows.First();
            if (!emissionRow.HasError && files.Exists(x => x.Templates.First(z => z.PropertyName == TemplatePropertyName.RiskDetail).Rows.Any(t => !t.HasError)))
            {
                files.ForEach(x => x.Templates.Add(emissionTemplate.Find(t => t.PropertyName == TemplatePropertyName.EmissionProperty)));
                List<FilterIndividual> filterIndividuals = DelegateService.massiveService.GetFilterIndividuals(collectiveLoad.User.UserId, collectiveLoad.Branch.Id, files, TemplatePropertyName.EmissionProperty);
                if (filterIndividuals != null && filterIndividuals.Count > 0)
                {
                    if (collectiveLoad.IsAutomatic)
                    {
                        int agentCode = (int)DelegateService.commonService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                        int agentType = (int)DelegateService.commonService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                        collectiveLoad.Agency = DelegateService.uniquePersonService.GetAgencyByAgentCodeAgentTypeCode(agentCode, agentType);
                        var emissionFile = new File
                        {
                            Templates = emissionTemplate
                        };
                        policy = DelegateService.collectiveService.CreateCompanyPolicy(collectiveLoad, emissionFile, TemplatePropertyName.EmissionProperty, filterIndividuals, files.Count);
                        collectiveLoad.TemporalId = policy.Id;
                        DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
                    }
                    else
                    {
                        PendingOperation pendingOperation;
                        if (Settings.UseReplicatedDatabase())
                        {
                            pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveLoad.TemporalId);
                        }
                        else
                        {
                            pendingOperation = DelegateService.commonService.GetPendingOperationById(collectiveLoad.TemporalId);
                        }
                        policy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    }
                }

                cacheListForPropety.Alliances = DelegateService.uniquePersonService.GetAlliances();
                cacheListForPropety.InsuredForScoreList = new List<int>();
                cacheListForPropety.FilterIndividuals = filterIndividuals;

                if (!emissionRow.HasError && files.Exists(x => x.Templates.First(z => z.PropertyName == TemplatePropertyName.RiskDetail).Rows.Any(t => !t.HasError)))
                {
                    if (filterIndividuals != null && filterIndividuals.Count > 0)
                    {
                        if (Settings.ImplementWebServices())
                        {
                            //Vaidación Externos 
                            Alliance validateAlliance = new Alliance();
                            if (policy.Alliance != null)
                            {
                                validateAlliance = cacheListForPropety.Alliances.Find(x => x.Id == policy.Alliance.Id);
                            }

                            policy.CompanyProduct.IsScore = DelegateService.externalProxyService.ValidateApplyScoreCreditByProduct(policy.CompanyProduct, validateAlliance, policy.Prefix.Id);
                            //Vaidación Externos 
                        }
                    }
                }
            }
            else
            {
                files.ForEach(x => x.Templates.Add(emissionTemplate.Find(t => t.PropertyName == TemplatePropertyName.EmissionProperty)));
            }
            policy.Summary = new Summary
            {
                RiskCount = files.Count
            };

            List<Clause> riskClauses = DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.Risk, (int)CoveredRiskType.Location);
            List<Clause> coverageClauses = DelegateService.underwritingService.GetClausesByEmissionLevel(EmissionLevel.Coverage);

            ParallelHelper.ForEach(files, file =>
            {
                CreateModel(collectiveLoad, file, cacheListForPropety, policy, riskClauses, coverageClauses);
            });
        }

        private void CreateModel(CollectiveEmission collectiveEmission, File file, CacheListForProperty cacheListForProperty, CompanyPolicy companyPolicy, List<Clause> riskClauses, List<Clause> coverageClauses)
        {
            CollectiveEmissionRow collectiveEmissionRow = new CollectiveEmissionRow
            {
                Risk = new Risk()
            };

            try
            {
                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription))
                    .Select(r => r.ErrorDescription).Distinct())).ToList();
                collectiveEmissionRow.MassiveLoadId = collectiveEmission.Id;
                collectiveEmissionRow.RowNumber = file.Id;
                collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Validation;
                collectiveEmissionRow.HasError = hasError;
                collectiveEmissionRow.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                collectiveEmissionRow.SerializedRow = JsonConvert.SerializeObject(file);

                DelegateService.collectiveService.CreateCollectiveEmissionRow(collectiveEmissionRow);

                if (!hasError)
                {
                    Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows.First();
                    templateName = file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).Description;

                    var riskCurrentFrom = (DateTime)DelegateService.commonService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
                    if (riskCurrentFrom != default(DateTime))
                    {
                        var riskCurrentFromIsInPolicyVigency = companyPolicy.CurrentFrom <= riskCurrentFrom && companyPolicy.CurrentTo > riskCurrentFrom;
                        if (!riskCurrentFromIsInPolicyVigency)
                        {
                            collectiveEmissionRow.HasError = true;
                            collectiveEmissionRow.Observations += Errors.ErrorRiskCurrentFromDateOutOfPolicyRange + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }
                    else
                    {
                        riskCurrentFrom = companyPolicy.CurrentFrom;
                    }

                    CompanyPropertyRisk companyProperty = new CompanyPropertyRisk
                    {
                        Status = RiskStatusType.Original,
                        CoveredRiskType = CoveredRiskType.Location,
                        CompanyRisk = new CompanyRisk()
                    };
                    companyProperty.CompanyRisk.CompanyInsured = DelegateService.massiveService.CreateInsured(row, companyPolicy.Holder, cacheListForProperty.FilterIndividuals);
                    companyProperty.GroupCoverage = DelegateService.collectiveService.CreateGroupCoverage(row, companyPolicy.CompanyProduct.Id);
                    companyProperty.Beneficiaries = new List<Beneficiary>();
                    companyProperty.Beneficiaries.Add(DelegateService.massiveService.CreateBeneficiary(row, companyProperty.CompanyRisk.CompanyInsured, cacheListForProperty.FilterIndividuals));

                    List<Beneficiary> beneficiaries = DelegateService.massiveService.CreateAdditionalBeneficiaries(file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.AdditionalBeneficiaries), cacheListForProperty.FilterIndividuals);
                    companyProperty.Beneficiaries.AddRange(beneficiaries);
                    if (companyProperty.Beneficiaries.GroupBy(b => b.IndividualId, ben => ben).Select(b => b.First()).ToList().Count != companyProperty.Beneficiaries.Count)
                    {
                        throw new ValidationException(Errors.ErrorBeneficiariesAdditionalDuplicated);
                    }

                    decimal beneficiariesParticipation = beneficiaries.Sum(x => x.Participation);
                    if (beneficiariesParticipation < 100)
                    {
                        companyProperty.Beneficiaries[0].Participation -= beneficiariesParticipation;
                    }
                    else
                    {
                        throw new ValidationException(Errors.ErrorParticipationBeneficiary + row.Number);
                    }
                    var principalRateTri = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRateTRI));
                    List<CompanyInsuredObject> tempInsuredObjects = CreateInsuredObjects(file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.InsuredObjects), principalRateTri, collectiveEmissionRow);
                    companyProperty.CompanyRisk.CompanyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdsGroupCoverageIdProductId(
                        tempInsuredObjects.Select(x => x.Id).ToList(), companyProperty.GroupCoverage.Id, companyPolicy.CompanyProduct.Id, true);
                    companyProperty.CompanyRisk.CompanyCoverages.ForEach(x =>
                    {
                        x.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                        x.CurrentFrom = riskCurrentFrom;
                        x.CurrentTo = companyPolicy.CurrentTo;
                        CompanyInsuredObject insuredObject;
                        if (x.CompanyInsuredObject == null || (insuredObject = tempInsuredObjects.FirstOrDefault(y => y.Id == x.CompanyInsuredObject.Id)) == null)
                        {
                            throw new ValidationException(Errors.ErrorMissingInsuredObjectForCoverage + x.Id);
                        }
                        int compareFromResult = DateTime.Compare(companyPolicy.CurrentFrom, companyPolicy.CurrentTo);
                        if (compareFromResult >= 0)
                        {
                            collectiveEmissionRow.HasError = true;
                            collectiveEmissionRow.Observations += Errors.ErrorToDatePolicy + KeySettings.ReportErrorSeparatorMessage();
                        }
                        x.CompanyInsuredObject = insuredObject;
                    });

                    if (Settings.ImplementWebServices())
                    {
                        if (companyPolicy.CompanyProduct.IsScore.GetValueOrDefault())
                        {
                            FilterIndividual filterIndividual = cacheListForProperty.FilterIndividuals.Find(x => x.InsuredCode == companyProperty.CompanyRisk.CompanyInsured.InsuredId && x.IsCLintonList != true);
                            if (filterIndividual != null)
                            {
                                companyProperty.CompanyRisk.CompanyInsured.ScoreCredit = filterIndividual.ScoreCredit;
                            }
                        }
                    }
                    else
                    {
                        companyProperty.CompanyRisk.CompanyInsured.ScoreCredit = DelegateService.externalProxyService.GetScoreDefault();
                    }

                    //Deducibles
                    collectiveEmissionRow = CreateDeductibles(companyProperty.CompanyRisk.CompanyCoverages, file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.ModifyCoverages), insuredObjects, collectiveEmissionRow);
                    companyProperty.FullAddress = DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskAddress)).ToString();
                    companyProperty.NomenclatureAddress = new Core.Application.Locations.Models.NomenclatureAddress
                    {
                        Type = new RouteType
                        {
                            Id = 1
                        },
                    };
                    companyProperty.City = new City()
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCity))
                    };
                    companyProperty.City.State = new State
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskState))
                    };
                    companyProperty.City.State.Country = new Country
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCountry))
                    };

                    companyProperty.RatingZone = DelegateService.commonService.GetRatingZonesByPrefixIdCountryIdStateIdCityId(companyPolicy.Prefix.Id, companyProperty.City.State.Country.Id, companyProperty.City.State.Id, companyProperty.City.Id);

                    if (companyProperty.RatingZone == null)
                    {
                        companyProperty.RatingZone = new RatingZone
                        {
                            Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRatingZone))
                        };
                    }

                    companyProperty.RiskActivity = new RiskActivity
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskActivity))
                    };
                    companyProperty.RiskUse = new RiskUse
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskUseCode))
                    };
                    companyProperty.IsDeclarative = (bool)DelegateService.commonService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskIsFacultative));

                    companyProperty.ConstructionType = new ConstructionType
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCOfConstruction))
                    };
                    companyProperty.PML = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskEML));
                    companyProperty.Square = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskBlock));
                    companyProperty.Latitude = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLatitude));
                    companyProperty.Longitude = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLongitude));
                    companyProperty.ConstructionYear = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskConstructionYear));
                    companyProperty.FloorNumber = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskFloorNumber));

                    companyProperty.ConstructionYear = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskConstructionYear));

                    int minYearAllowed = DelegateService.commonService.GetExtendedParameterByParameterId((int)CompanyParameterType.MinYearAllowed).NumberParameter.GetValueOrDefault();

                    if (companyProperty.ConstructionYear > DateTime.Now.Year)
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += Errors.ErrorTheYearOfConstructionCanNotBeGreaterThanTheCurrent + KeySettings.ReportErrorSeparatorMessage();
                    }
                    else if (companyProperty.ConstructionYear < minYearAllowed)
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += Errors.ErrorTheYearOfConstructionCanNotBeLessThan + " " + minYearAllowed + KeySettings.ReportErrorSeparatorMessage();
                    }

                    companyProperty.FloorNumber = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskFloorNumber));
                    int maxFloorNumber = (int)DelegateService.commonService.GetExtendedParameterByParameterId((int)CompanyParameterType.MaxFloorNumber).NumberParameter;
                    int minFloorNumber = (int)DelegateService.commonService.GetExtendedParameterByParameterId((int)CompanyParameterType.MinFloorNumber).NumberParameter;

                    if (companyProperty.FloorNumber > maxFloorNumber)
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += Errors.ErrorTheNumberOfFloorsCanNotBeGreaterThan + " " + maxFloorNumber + KeySettings.ReportErrorSeparatorMessage();
                    }
                    else if (companyProperty.FloorNumber < minFloorNumber)
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += Errors.ErrorTheNumberOfFloorsCanNotBeLessThan + " " + minFloorNumber + KeySettings.ReportErrorSeparatorMessage();
                    }

                    //company
                    companyProperty.CompanyLocation = new CompanyLocation
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskLocation))
                    };
                    companyProperty.CompanyLocation.CompanyDistrict = new CompanyDistrict
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskNeighborhood))
                    };
                    companyProperty.CompanyLevelZone = new CompanyLevelZone
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskLevelZone))
                    };
                    companyProperty.IsResidential = (bool)DelegateService.commonService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIsResidential));
                    companyProperty.IsOutCommunity = (bool)DelegateService.commonService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIsOutCommunity));
                    companyProperty.CompanyRiskTypeEarthquake = new CompanyRiskTypeEarthquake
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskTypeCode))
                    };
                    companyProperty.CompanyRisk.CompanyAssistanceType = new CompanyAssistanceType
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskAsistType))
                    };
                    companyProperty.CompanyIrregularHeight = new CompanyIrregularHeight
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIrregularHeight))
                    };
                    companyProperty.CompanyIrregularPlant = new CompanyIrregularPlant
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIrregularPlant))
                    };
                    companyProperty.CompanyDamage = new CompanyDamage
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskPreviousDamage))
                    };
                    companyProperty.CompanyRepair = new CompanyRepair
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskRepairedCode))
                    };
                    companyProperty.CompanyStructureType = new CompanyStructureType
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskStructureCode))
                    };
                    companyProperty.CompanyReinforcedStructureType = new CompanyReinforcedStructureType
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskReinforcedStructureType))
                    };
                    companyProperty.CompanyUseHouse = new CompanyUseHouse
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskUseProperty))
                    };
                    companyProperty.CompanyHouseType = new CompanyHouseType
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskTypeOfProperty))
                    };
                    companyProperty.CompanyMicroZone = new CompanyMicroZone
                    {
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskMicrozoningZone))
                    };

                    companyProperty.CompanyRisk.CompanyInsured.ClaimIncurred = new ClaimIncurred();
                    decimal? accidentRate = DelegateService.uniquePersonService.GetSinisterPercentageByInsuredId(companyProperty.CompanyRisk.CompanyInsured.InsuredId).AccidentRate;

                    if (accidentRate == null)
                    {
                        if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskSinisterPercentage).Value))
                        {
                            companyProperty.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskSinisterPercentage));
                            if (companyProperty.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate < 0 || companyProperty.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate > 100)
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
                        companyProperty.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate = accidentRate.Value;
                    }

                    companyProperty.Text = new Text
                    {
                        TextBody = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText))
                    };

                    Template templateClauses = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.Clauses);

                    if (templateClauses != null)
                    {
                        templateName = templateClauses.Description;
                        companyProperty.Clauses = new List<Clause>();
                        companyProperty.CompanyRisk.CompanyCoverages.ForEach(c => c.Clauses = new List<Clause>());

                        foreach (Row clausesRow in templateClauses.Rows)
                        {
                            int levelCode = (int)DelegateService.commonService.GetValueByField<int>(clausesRow.Fields.Find(p => p.PropertyName == FieldPropertyName.LevelCode));
                            int clauseCode = (int)DelegateService.commonService.GetValueByField<int>(clausesRow.Fields.Find(p => p.PropertyName == FieldPropertyName.ClauseCode));
                            if (levelCode == (int)EmissionLevel.Risk)
                            {
                                if (riskClauses.Any(c => c.Id == clauseCode))
                                {
                                    companyProperty.Clauses.Add(riskClauses.First(c => c.Id == clauseCode));
                                }
                                else
                                {
                                    collectiveEmissionRow.HasError = true;
                                    collectiveEmissionRow.Observations += Errors.ErrorClauseNotFound + KeySettings.ReportErrorSeparatorMessage();
                                }
                            }
                            else if (levelCode == (int)EmissionLevel.Coverage)
                            {
                                if (coverageClauses.Any(a => a.Id == clauseCode))
                                {
                                    Clause clauseToAdd = coverageClauses.First(c => c.Id == clauseCode);

                                    if (companyProperty.CompanyRisk.CompanyCoverages.Any(c => c.Id == clauseToAdd.ConditionLevel.ConditionValue))
                                    {
                                        companyProperty.CompanyRisk.CompanyCoverages.First(c => c.Id == clauseToAdd.ConditionLevel.ConditionValue).Clauses.Add(clauseToAdd);
                                    }
                                    else
                                    {
                                        collectiveEmissionRow.HasError = true;
                                        collectiveEmissionRow.Observations += string.Format(Errors.ClauseCoverageNotPresentOnRisk, clauseCode) + KeySettings.ReportErrorSeparatorMessage();
                                    }

                                }
                                else
                                {
                                    collectiveEmissionRow.HasError = true;
                                    collectiveEmissionRow.Observations += string.Format(Errors.ClauseNotRelatedToCoverage, clauseCode) + KeySettings.ReportErrorSeparatorMessage();
                                }
                            }
                        }
                    }

                    if (collectiveEmissionRow.HasError.HasValue && collectiveEmissionRow.HasError.Value)
                    {
                        DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                        return;
                    }

                    templateName = "";

                    PendingOperation pendingOperation = new PendingOperation
                    {
                        ParentId = companyPolicy.Id,
                        UserId = companyPolicy.UserId,
                        Operation = JsonConvert.SerializeObject(companyProperty)
                    };
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.commonService.CreatePendingOperation(pendingOperation);
                        collectiveEmissionRow.Risk.RiskId = pendingOperation.Id;
                        DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                    }
                    else
                    {
                        string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}", JsonConvert.SerializeObject(pendingOperation), (char)007, JsonConvert.SerializeObject(collectiveEmissionRow), (char)007, nameof(CollectiveEmissionRow));
                        var queue = new Core.Framework.Queues.BaseQueueFactory().CreateQueue("CreatePendingOperationQuee", routingKey: "CreatePendingOperationQuee", serialization: "JSON");
                        queue.PutOnQueue(pendingOperationJson);
                    }

                    if (Settings.ImplementWebServices() &&
                        (companyPolicy.CompanyProduct.IsScore.GetValueOrDefault() && companyProperty.CompanyRisk.CompanyInsured.ScoreCredit == null))
                    {
                        CheckExternalServices(cacheListForProperty, companyPolicy, companyProperty, collectiveEmission, file, row);
                    }
                }
            }
            catch (Exception ex)
            {
                collectiveEmissionRow.HasError = true;

                if (string.IsNullOrEmpty(templateName))
                {
                    collectiveEmissionRow.Observations += Errors.ErrorCreateRisk;
                    EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
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

        private void CheckExternalServices(CacheListForProperty cacheListForProperty, CompanyPolicy companyPolicy, CompanyPropertyRisk companyProperty, CollectiveEmission collectiveEmission, File file, Row row)
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
                DelegateService.externalProxyService.CheckExternalServices(filterIndividual.Person.IdentificationDocument, surname, filterIndividual.InsuredCode.Value, licencePlate, collectiveEmission.Id, row.Number, (int)SubCoveredRiskType.Property, collectiveEmission.User.UserId, scoreAlreadyQueried, simitAlreadyQueried, requireScore, requireSimit, requireFasecolda);
            }
        }

        private List<CompanyInsuredObject> CreateInsuredObjects(Template insuredObjectTemplate, decimal principalRateTri, CollectiveEmissionRow collectiveEmissionRow)
        {

            if (insuredObjectTemplate == null)
            {
                throw new ValidationException(Errors.InsuranceObjectsAreMandatory);
            }
            List<CompanyInsuredObject> insuredObjects = new List<CompanyInsuredObject>();
            int id = 0, recoupmentPeriodId = 0;
            decimal rateTri = 0, percentageVariableIndex = 0, amount = 0;
            foreach (Row row in insuredObjectTemplate.Rows)
            {
                id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                amount = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectSumAssured));
                if (principalRateTri > 0)
                {
                    rateTri = principalRateTri;
                }
                else
                {
                    rateTri = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RiskRateTRI));
                }
                percentageVariableIndex = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.FirstOrDefault(x => x.PropertyName == CompanyFieldPropertyName.RiskPercentageVariableIndex));
                recoupmentPeriodId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == CompanyFieldPropertyName.RecoupmentPeriodId));
                if (id <= 0 || amount <= 0)
                {
                    continue;
                }
                var insuredObject = new CompanyInsuredObject
                {
                    Id = id,
                    Amount = amount,
                    RateTRI = rateTri,
                    PercentageVariableIndex = percentageVariableIndex
                };

                if (recoupmentPeriodId != 0)
                {
                    insuredObject.RecoupmentPeriod = new RecoupmentPeriod
                    {
                        Id = recoupmentPeriodId
                    };
                }
                insuredObjects.Add(insuredObject);
            }
            List<CompanyInsuredObject> companyInsuredObject = DelegateService.underwritingService.GetRecoupmentPeriodAndPercentageVariableIndex(insuredObjects);
            foreach (CompanyInsuredObject itemInsuredObject in companyInsuredObject)
            {
                if (itemInsuredObject.RecoupmentPeriod.Enable)
                {
                    if (insuredObjects.Find(x => x.Id == itemInsuredObject.Id).RecoupmentPeriod == null)
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += Errors.ErrorRecoupmentPeriod + " " + itemInsuredObject.Id + KeySettings.ReportErrorSeparatorMessage();
                    }
                }
                if (itemInsuredObject.RequiredPercentageVariableIndex)
                {
                    if (insuredObjects.Find(x => x.Id == itemInsuredObject.Id).PercentageVariableIndex == 0)
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += Errors.ErrorPercentageVariableIndex + " " + itemInsuredObject.Id + KeySettings.ReportErrorSeparatorMessage();
                    }
                }
            }
            if (!insuredObjects.Any())
            {
                throw new ValidationException(Errors.NoInsuredObjectsFound);
            }
            return insuredObjects;
        }

        private CollectiveEmissionRow CreateDeductibles(List<CompanyCoverage> coverages, Template deductiblesTemplate, List<CompanyInsuredObject> insuredObject, CollectiveEmissionRow collectiveEmissionRow)
        {
            if (deductiblesTemplate != null)
            {
                templateName = deductiblesTemplate.Description;
                foreach (Row row in deductiblesTemplate.Rows)
                {
                    int insuredObjectCode = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                    if (!insuredObject.Exists(c => c.Id == insuredObjectCode))
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations = Errors.TheInsuranceObjectInTheModificationIsNotRelatedToTheRisk + KeySettings.ReportErrorSeparatorMessage();
                        return collectiveEmissionRow;
                    }
                    int coverageId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                    int deductibleId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.DeductibleCodeDeductible));
                    if (coverageId <= 0 || deductibleId <= 0)
                    {
                        continue;
                    }
                    CompanyCoverage coverage;
                    if ((coverage = coverages.Find(c => c.Id == coverageId)) != null)
                    {
                        coverage.Deductible = new Deductible
                        {
                            Id = deductibleId
                        };
                    }
                }
            }
            return collectiveEmissionRow;
        }

        #region Reportes

        /// <summary>
        /// Genera el archivo de reporte del proceso de colectivas de hogar
        /// </summary>
        /// <param name="collectiveEmission"></param>
        /// <param name="endorsementType"></param>
        /// <returns>string</returns>
        public string GenerateReportToCollectiveLoad(CollectiveEmission collectiveEmission, int endorsementType)
        {
            CollectiveLoadProcessStatus processStatus = CollectiveLoadProcessStatus.Validation;
            switch (collectiveEmission.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    processStatus = CollectiveLoadProcessStatus.Events;
                    break;
                case MassiveLoadStatus.Issued:
                    processStatus = CollectiveLoadProcessStatus.Finalized;
                    break;
            }

            DelegateService.massiveService.LoadReportCacheList();
            LoadList(collectiveEmission);
            SubCoveredRiskType subCoveredRiskType = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(collectiveEmission.Prefix.Id, (int)collectiveEmission.CoveredRiskType);
            List<CollectiveEmissionRow> collectiveEmissionRows = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadIdCollectiveLoadProcessStatus(collectiveEmission.Id, null, null, null);
            List<int> excludedRisk = collectiveEmissionRows.Where(p => p.HasError == true).Select(p => p.Risk.RiskId).ToList();
            collectiveEmissionRows = collectiveEmissionRows.Where(p => p.HasError != true && p.Status == processStatus).ToList();
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.MassiveReport;
            fileProcessValue.Key4 = collectiveEmission.Prefix.Id;
            fileProcessValue.Key5 = (int)subCoveredRiskType;
            File file = DelegateService.commonService.GetFileByFileProcessValue(fileProcessValue);

            if (file == null || !collectiveEmissionRows.Any())
            {
                return "";
            }

            file.Templates[0].Rows[0].Fields.ForEach(u => u.Value = "");
            string serializeFields = JsonConvert.SerializeObject(file.Templates[0].Rows[0].Fields);
            //string key = Guid.NewGuid().ToString();
            //file.Name = "Reporte Colectiva Hogar_" + key + "_" + collectiveEmission.Id;

            file.FileType = FileType.CSV;

            Policy policy = new Policy();
            policy.Id = collectiveEmission.TemporalId;
            policy.Endorsement = new Endorsement { EndorsementType = (EndorsementType)endorsementType, Id = collectiveEmission.EndorsementId ?? 0 };
            policy.DocumentNumber = collectiveEmission.DocumentNumber != null ? collectiveEmission.DocumentNumber.GetValueOrDefault() : 0;

            CompanyPolicy companyPolicy = DelegateService.massiveService.GetCompanyPolicyByMassiveLoadStatusPolicy(collectiveEmission.Status.Value, policy);
            List<Field> fields = DelegateService.massiveService.GetFields(serializeFields, companyPolicy);
            fields.Find(u => u.PropertyName == FieldPropertyName.LoadId).Value = collectiveEmission.Id.ToString();
            serializeFields = JsonConvert.SerializeObject(fields);

            return FillPropertyFields(file, collectiveEmission, collectiveEmissionRows, serializeFields, excludedRisk, companyPolicy);

        }

        private string FillPropertyFields(File file, CollectiveEmission collectiveEmission, List<CollectiveEmissionRow> collectiveEmissionRows, string serializeFields, List<int> excludedRisk, CompanyPolicy companyPolicy)
        {
            string filePath = "";
            try
            {
                string key = Guid.NewGuid().ToString();
                int bulkExcel = Convert.ToInt32(DelegateService.commonService.GetKeyApplication("MaxMassiveExcelRow"));
                if (companyPolicy != null)
                {

                    List<CompanyPropertyRisk> companyPropertyRisks = GetCompanyPropertyRisk(collectiveEmission, companyPolicy.Id);
                    companyPropertyRisks = companyPropertyRisks.Where(p => excludedRisk.Any(s => s == p.Id) == false).ToList();

                    TP.Parallel.ForEach(companyPropertyRisks, property =>
                    {
                        List<Field> fields = JsonConvert.DeserializeObject<List<Field>>(serializeFields);
                        fields = CreateInsured(fields, property);

                        fields.Find(u => u.PropertyName == FieldPropertyName.Identificator).Value = collectiveEmissionRows.FirstOrDefault(x => x.Risk.Id == property.Id).RowNumber.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = property.AmountInsured.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskNumber).Value = property.Number.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskAddress).Value = CreateAddress(property);
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskFloorNumber).Value = property.FloorNumber.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskConstructionYear).Value = property.ConstructionYear.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskLongitude).Value = property.Longitude.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskLatitude).Value = property.Latitude.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.BeneficiaryData).Value = DelegateService.massiveService.CreateBeneficiaries(property.Beneficiaries);
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskUseDescription).Value = (property.RiskUse != null && property.RiskUse.Id > 0) ? riskUse.FirstOrDefault(u => u.Id == property.RiskUse.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskTypeDescription).Value = (property.CompanyRiskTypeEarthquake != null && property.CompanyRiskTypeEarthquake.Id > 0) ? riskTypes.FirstOrDefault(u => u.Id == property.CompanyRiskTypeEarthquake.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(property.Clauses);

                        fields.Find(u => u.PropertyName == FieldPropertyName.RaitingZoneDescription).Value = (property.RatingZone != null && property.RatingZone.Id > 0) ? ratingZones.FirstOrDefault(u => u.Id == property.RatingZone.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.Observations).Value = "";

                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.StructureDescription).Value = (property.CompanyStructureType != null && property.CompanyStructureType.Id > 0) ? companyStructureTypes.FirstOrDefault(u => u.TypeCD == property.CompanyStructureType.Id).Description : ""; ;
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskIrregularHeightDescription).Value = (property.CompanyIrregularHeight != null && property.CompanyIrregularHeight.Id > 0) ? irregularHeights.FirstOrDefault(u => u.Id == property.CompanyIrregularHeight.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskIrregularPlantDescription).Value = (property.CompanyIrregularPlant != null && property.CompanyIrregularPlant.Id > 0) ? irregularPlants.FirstOrDefault(u => u.Id == property.CompanyIrregularPlant.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskRepairedDescripcion).Value = (property.CompanyRepair != null && property.CompanyRepair.Id > 0) ? companyRepairs.FirstOrDefault(u => u.Id == property.CompanyRepair.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskReinforcedStructureDescription).Value = (property.CompanyReinforcedStructureType != null && property.CompanyReinforcedStructureType.Id > 0) ? reinforcedStructureTypes.FirstOrDefault(u => u.Id == property.CompanyReinforcedStructureType.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskPreviousDamageDescription).Value = (property.CompanyDamage != null && property.CompanyDamage.Id > 0) ? damages.FirstOrDefault(u => u.Id == property.CompanyDamage.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskLocationDescription).Value = "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskNeighborhoodDescription).Value = "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskCOfConstructionDescription).Value = (property.ConstructionType != null && property.ConstructionType.Id > 0) ? constructionTypes.FirstOrDefault(u => u.Id == property.ConstructionType.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskNeighborhoodDescription).Value = "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskMicrozoningZone).Value = (property.CompanyMicroZone != null && companyMicroZones.Count > 0 && property.CompanyMicroZone.Id > 0) ? companyMicroZones.FirstOrDefault(u => u.Id == property.CompanyMicroZone.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskSinisterPercentage).Value = property.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyCurrencyDescription).Value = companyPolicy.ExchangeRate != null ? currency.FirstOrDefault(l => l.Id == companyPolicy.ExchangeRate.Currency.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.MethodOfPaymentDescription).Value = companyPolicy.PaymentPlan != null ? paymentMethod.FirstOrDefault(l => l.Id == companyPolicy.PaymentPlan.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyType).Value = companyPolicy.PolicyType != null ? policiyType.FirstOrDefault(l => l.Id == companyPolicy.PolicyType.Id && l.Prefix.Id == companyPolicy.Prefix.Id).Description : "";

                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValue).Value = "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValueRC).Value = "";

                        //Asistencia
                        List<int> assistanceCoveragesIds = DelegateService.underwritingService.GetAssistanceCoveragesIds(CompanyParameterType.AssistanceProperty);
                        List<CompanyCoverage> coveragesAssistance = property.CompanyRisk.CompanyCoverages.Where(u => assistanceCoveragesIds.Exists(id => id == u.Id)).ToList();
                        decimal assistancePremium = coveragesAssistance.Sum(x => x.PremiumAmount);
                        if (assistancePremium > 0)
                        {
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyPremium).Value =
                                (companyPolicy.Summary.Premium - assistancePremium).ToString("F2");
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyExpensesWithAssistance).Value =
                                (companyPolicy.Summary.Expenses + assistancePremium).ToString("F2");
                        }

                        string serializeFields1 = JsonConvert.SerializeObject(fields);
                        decimal valueRc = 0, insuredValue = 0;
                        valueRc = property.CompanyRisk.CompanyCoverages.Where(u => u.Description != null && u.Description.Contains("R.C.E")).Sum(u => u.LimitAmount);
                        insuredValue = (companyPolicy.Summary.AmountInsured - valueRc);

                        foreach (CompanyCoverage coverage in property.CompanyRisk.CompanyCoverages.OrderByDescending(u => u.Number))
                        {

                            List<Field> fieldsC = JsonConvert.DeserializeObject<List<Field>>(serializeFields1);
                            fieldsC.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValue).Value = insuredValue.ToString();
                            fieldsC.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValueRC).Value = valueRc.ToString();
                            if (coverage.SubLineBusiness != null && coverage.SubLineBusiness.LineBusiness != null)
                            {
                                fieldsC.Find(u => u.PropertyName == FieldPropertyName.LineBusinessDescripcion).Value = lineBusiness.DefaultIfEmpty(new LineBusiness { Description = "" }).FirstOrDefault(u => u.Id == coverage.SubLineBusiness.LineBusiness.Id).Description;
                                fieldsC.Find(u => u.PropertyName == FieldPropertyName.SubLineBusinessDescripcion).Value = subLineBusiness.DefaultIfEmpty(new SubLineBusiness { Description = "" }).FirstOrDefault(u => u.Id == coverage.SubLineBusiness.Id).Description;
                            }
                            else
                            {
                                fieldsC.Find(u => u.PropertyName == FieldPropertyName.LineBusinessDescripcion).Value = "";
                                fieldsC.Find(u => u.PropertyName == FieldPropertyName.SubLineBusinessDescripcion).Value = "";

                            }
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.InsuredObjectDescription).Value = insuredObjects.DefaultIfEmpty(new CompanyInsuredObject { Description = "" }).FirstOrDefault(u => u.Id == coverage.CompanyInsuredObject.Id).Description;
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageDescription).Value = coverage.Description;
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageInsuredValue).Value = coverage.DeclaredAmount.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageRate).Value = coverage.Rate.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoveragePremium).Value = coverage.PremiumAmount.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.DeductibleDescription).Value = (coverage.Deductible != null && coverage.Deductible.Description != null) ? coverage.Deductible.Description : "";
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(coverage.Clauses);

                            //Fecha de vigencia del riesgo
                            fieldsC.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskCurrentFrom).Value = coverage.CurrentFrom.ToString();
                            concurrentRows.Add(new Row
                            {
                                Fields = fieldsC,
                                Number = collectiveEmissionRows.FirstOrDefault(x => x.Risk.Id == property.Id).RowNumber
                            });
                        }
                        if (concurrentRows.Count >= bulkExcel || companyPropertyRisks.Count == 0)
                        {
                            file.Templates[0].Rows = concurrentRows.ToList().OrderBy(x => x.Number).ToList();
                            file.Name = "Reporte Colectiva Hogar_" + key + "_" + collectiveEmission.Id;
                            filePath = DelegateService.commonService.GenerateFile(file);
                            concurrentRows = new ConcurrentBag<Row>();
                        }
                    });
                }
                return filePath;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                DataFacadeManager.Dispose();
            }

        }
        private List<CompanyPropertyRisk> GetCompanyPropertyRisk(CollectiveEmission collectiveEmission, int tempId)
        {
            List<CompanyPropertyRisk> companyProperties = new List<CompanyPropertyRisk>();
            int endorsementId = collectiveEmission.EndorsementId ?? 0;
            switch (collectiveEmission.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    List<PendingOperation> pendingOperations;
                    if (Settings.UseReplicatedDatabase())
                    {
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(tempId);
                    }
                    else
                    {
                        pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(tempId);
                    }
                    foreach (PendingOperation item in pendingOperations)
                    {
                        companyProperties.Add(JsonConvert.DeserializeObject<CompanyPropertyRisk>(item.Operation));
                    }
                    break;
                case MassiveLoadStatus.Issued:
                    //companyProperties = DelegateService.propertyService.GetCompanyPropertyByPrefixBranchDocumentNumberEndorsementType(collectiveEmission.Prefix.Id, collectiveEmission.Branch.Id, collectiveEmission.DocumentNumber.Value, EndorsementType.Emission);
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* without Replicated Database */
                        DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(endorsementId).ForEach(x => companyProperties.Add(JsonConvert.DeserializeObject<CompanyPropertyRisk>(x)));
                        /* without Replicated Database */
                    }
                    else
                    {
                        DelegateService.pendingOperationEntityService.GetRiskByEndorsementDocumentNumber(endorsementId).ForEach(x => companyProperties.Add(JsonConvert.DeserializeObject<CompanyPropertyRisk>(x)));
                    }
                    break;
            }
            return companyProperties;

        }
        private void LoadList(CollectiveEmission massiveEmission)
        {
            documentTypes = (List<DocumentType>)DelegateService.massiveService.GetCacheList("documentTypes_", "Report");
            deductibles = (List<Deductible>)DelegateService.massiveService.GetCacheList("deductibles_", "Report");
            irregularHeights = DelegateService.propertyService.GetCompanyIrregularHeights();
            irregularPlants = DelegateService.propertyService.GetCompanyIrregularPlants();
            levelzones = new List<CompanyLevelZone>();
            locations = new List<CompanyLocation>();
            reinforcedStructureTypes = DelegateService.propertyService.GetCompanyReinforcedStructureTypes();
            companyRepairs = DelegateService.propertyService.GetCompanyRepairs();
            companyStructureTypes = DelegateService.propertyService.GetCompanyStructureTypes();
            riskUse = DelegateService.propertyService.GetRiskUses();
            constructionTypes = DelegateService.propertyService.GetConstructionTypes();
            lineBusiness = DelegateService.commonService.GetLinesBusiness();
            subLineBusiness = DelegateService.commonService.GetSubLineBusiness();
            insuredObjects = DelegateService.underwritingService.GetCompanyInsuredObjects();
            cities = DelegateService.commonService.GetCities();
            states = DelegateService.commonService.GetStates();
            riskTypes = DelegateService.propertyService.GetRiskTypes();
            ratingZones = DelegateService.commonService.GetRatingZones();
            companyMicroZones = DelegateService.propertyService.GetCompanyMicroZones();
            damages = new List<CompanyDamage>();
            damages = DelegateService.propertyService.GetCompanyDamages();
            currency = DelegateService.commonService.GetCurrencies();
            paymentMethod = DelegateService.commonService.GetPaymentMethods();
            policiyType = DelegateService.commonService.GetPolicyTypeAll();
        }

        private List<Field> CreateInsured(List<Field> fields, CompanyPropertyRisk property)
        {
            CompanyInsured mainInsured = property.CompanyRisk?.CompanyInsured;
            CompanyName companyName = mainInsured?.CompanyName;
            if (mainInsured != null)
            {
                fields.Find(u => u.PropertyName == FieldPropertyName.InsuredDocument).Value = documentTypes.DefaultIfEmpty(new DocumentType { SmallDescription = "CC" }).First(u => u.Id == mainInsured.IdentificationDocument.DocumentType.Id).SmallDescription + ". " + mainInsured.IdentificationDocument.Number;
                fields.Find(u => u.PropertyName == FieldPropertyName.InsuredDescription).Value = mainInsured.Name;
                fields.Find(u => u.PropertyName == FieldPropertyName.InsuredPhoneDescription).Value = (companyName?.Phone != null) ? companyName.Phone.Description : "";
            }
            string address = companyName.Address.Description.ToUpper() ?? "";
            if (companyName != null)
            {
                if (companyName.Address.City != null && companyName.Address.City.State != null)
                {
                    address += "| " + states.FirstOrDefault(u => u.Id == companyName.Address.City.State.Id).Description;
                    address += "| " + cities.FirstOrDefault(u => u.Id == companyName.Address.City.Id && u.State.Id == companyName.Address.City.State.Id).Description;
                    //address = StringHelper.ConcatenateString(companyName.Address?.Description ?? "", "|", companyName.Address?.City?.State?.Description ?? "", "|", companyName.Address?.City?.Description ?? "");
                }
            }
            fields.Find(u => u.PropertyName == FieldPropertyName.InsuredAddressDescription).Value = address;
            return fields;

        }

        private string CreateAddress(CompanyPropertyRisk propertyRisk)
        {
            string address = "";
            address += propertyRisk.FullAddress;

            if (propertyRisk.City != null && propertyRisk.City.State != null)
            {
                address += StringHelper.ConcatenateString(" | ", states.DefaultIfEmpty(new State { Description = "" }).FirstOrDefault(u => u.Id == propertyRisk.City.State.Id).Description,
                    " | ", cities.DefaultIfEmpty(new City { Description = "" }).FirstOrDefault(u => u.Id == propertyRisk.City.Id && u.State.Id == propertyRisk.City.State.Id).Description);
            }

            return address;

        }
        #endregion
    }
}
