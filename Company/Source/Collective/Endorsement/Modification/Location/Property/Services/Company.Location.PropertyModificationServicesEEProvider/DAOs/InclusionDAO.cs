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
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.Location.PropertyModificationService.EEProvider.Resources;
using Sistran.Company.Application.Location.PropertyModificationService.Models;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Application.Utilities.Configuration;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.PropertyModificationService.EEProvider.DAOs
{
    public class InclusionDAO
    {
        public CollectiveEmission CreateCollectiveInclution(CollectiveEmission collectiveEmission)
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
                Key1 = (int)FileProcessType.CollectiveInclusion,
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
                File file = collectiveEmission.File;
                file = DelegateService.commonService.ValidateDataFile(file, collectiveEmission.User.AccountName);
                Template policyTemplate = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Policy);
                file.Templates.Remove(policyTemplate);
                collectiveEmission.TotalRows = file.Templates.First(p => p.IsPrincipal).Rows.Count;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                List<File> validatedFiles = DelegateService.commonService.GetDataTemplates(collectiveEmission.File.Templates);
                Row policyRow = policyTemplate.Rows.First();
                if (policyRow.HasError)
                {
                    collectiveEmission.HasError = true;
                    collectiveEmission.ErrorDescription = policyRow.ErrorDescription;
                    DelegateService.massiveService.UpdateMassiveLoad(collectiveEmission);
                    return;
                }
                int branchId = (int)DelegateService.commonService.GetValueByField<int>(policyRow.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                decimal policyNum = (decimal)DelegateService.commonService.GetValueByField<decimal>(policyRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumber));
                int prefixId = (int)DelegateService.commonService.GetValueByField<int>(policyRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyPrefixCode));
                Policy policy = DelegateService.underwritingService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNum);
                if (policy == null)
                {
                    collectiveEmission.HasError = true;
                    collectiveEmission.ErrorDescription = Errors.PolicyNotFound;
                    DelegateService.massiveService.UpdateMassiveLoad(collectiveEmission);
                    return;
                }
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetTemporalPolicyByPolicyIdEndorsementId(policy.Id, policy.Endorsement.Id);
                InclusionValidationDAO inclusionValidationDAO = new InclusionValidationDAO();
                List<Validation> validations = inclusionValidationDAO.GetValidationsByFiles(validatedFiles, collectiveEmission, policy.Product.Id, companyPolicy.PolicyType.Id, policyRow, policy.CurrentFrom, policy.CurrentTo);
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
                collectiveEmission.Branch.Id = branchId;
                collectiveEmission.Product.Id = companyPolicy.CompanyProduct.Id;
                collectiveEmission.DocumentNumber = policyNum;
                collectiveEmission.Agency = companyPolicy.Agencies.FirstOrDefault();
                companyPolicy.CompanyProduct = DelegateService.underwritingService.GetCompanyProductByProductIdPrefixId(collectiveEmission.Product.Id, companyPolicy.Prefix.Id);
                companyPolicy.CurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                companyPolicy.CurrentTo = companyPolicy.Endorsement.CurrentTo;
                companyPolicy.Summary = new Summary
                {
                    RiskCount = collectiveEmission.TotalRows
                };
                companyPolicy.Endorsement.EndorsementType = EndorsementType.Modification;
                companyPolicy.Endorsement.IsUnderIdenticalConditions = true;
                companyPolicy.Endorsement.IsMassive = true;
                companyPolicy.TemporalType = TemporalType.Endorsement;
                if (companyPolicy.Text == null)
                {
                    companyPolicy.Text = new Text();
                }
                companyPolicy.Text.TextBody = policyRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyText)?.Value;
                companyPolicy.UserId = collectiveEmission.User.UserId;
                companyPolicy.Clauses = DelegateService.massiveService.GetClauses(collectiveEmission.File.Templates.Find(p => p.PropertyName == TemplatePropertyName.Clauses), EmissionLevel.General);
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                validatedFiles.ForEach(x => x.Templates.Add(policyTemplate));
                CreateModels(collectiveEmission, validatedFiles, companyPolicy, policyRow);
            }
            catch (Exception ex)
            {
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription += StringHelper.ConcatenateString(Errors.ErrorValidatingFile + " " + ex.Message, "|");
            }
            finally
            {
                collectiveEmission.Status = MassiveLoadStatus.Validated;
                DelegateService.massiveService.UpdateMassiveLoad(collectiveEmission);
                DataFacadeManager.Dispose();
            }
        }

        private void CreateModels(CollectiveEmission collectiveEmission, List<File> files, CompanyPolicy policy, Row policyRow)
        {
            List<FilterIndividual> filterIndividuals = new List<FilterIndividual>();
            CacheListForProperty cacheListForProperty = new CacheListForProperty();
            if (!policyRow.HasError && files.Exists(x => x.Templates.First(z => z.PropertyName == TemplatePropertyName.RiskDetail).Rows.Any(t => !t.HasError)))
            {
                PendingOperation pendingOperation = new PendingOperation
                {
                    Operation = JsonConvert.SerializeObject(policy),
                    UserId = policy.UserId
                };
                if (!Settings.UseReplicatedDatabase())
                {
                    pendingOperation = DelegateService.commonService.CreatePendingOperation(pendingOperation);
                }
                else
                {
                    pendingOperation = DelegateService.pendingOperationEntityService.CreatePendingOperation(pendingOperation);
                }
                policy.Id = pendingOperation.Id;
                collectiveEmission.TemporalId = policy.Id;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                filterIndividuals = DelegateService.massiveService.GetFilterIndividualsForCollective(policyRow, files, collectiveEmission.User.UserId, collectiveEmission.Branch.Id, FieldPropertyName.PolicyNumber, FieldPropertyName.PrefixCode);
                List<FilterIndividual> individualWithError = new List<FilterIndividual>();

                individualWithError.AddRange(filterIndividuals.Where(i => i.IsCLintonList == true));
                individualWithError.AddRange(filterIndividuals.Where(i => !string.IsNullOrEmpty(i.Error)));

                filterIndividuals.RemoveAll(i => i.IsCLintonList == true);
                filterIndividuals.RemoveAll(i => !string.IsNullOrEmpty(i.Error));
                cacheListForProperty.FilterIndividuals = filterIndividuals;

                if (Settings.UseReplicatedDatabase())
                {
                    filterIndividuals = DelegateService.externalProxyMirrorService.GetMassiveScoresCreditByLastValid(cacheListForProperty.FilterIndividuals, collectiveEmission.Prefix.Id, collectiveEmission.User.UserId);
                }
                else
                {
                    filterIndividuals = DelegateService.externalProxyService.GetMassiveScoresCreditByLastValid(cacheListForProperty.FilterIndividuals, collectiveEmission.Prefix.Id, collectiveEmission.User.UserId);
                }

                cacheListForProperty.FilterIndividuals.AddRange(individualWithError);
                cacheListForProperty.Alliances = DelegateService.uniquePersonService.GetAlliances();
                cacheListForProperty.InsuredForScoreList = new List<int>();

                if (Settings.ImplementWebServices())
                {
                    //Vaidación Externos 
                    Alliance validateAlliance = new Alliance();
                    if (policy.Alliance != null)
                    {
                        validateAlliance = cacheListForProperty.Alliances.Find(x => x.Id == policy.Alliance.Id);
                    }
                    policy.CompanyProduct.IsScore = DelegateService.externalProxyService.ValidateApplyScoreCreditByProduct(policy.CompanyProduct, validateAlliance, policy.Prefix.Id);
                    //Vaidación Externos 
                }
            }

            ParallelHelper.ForEach(files, file =>
            {
                CreateModel(collectiveEmission, file, cacheListForProperty, policy);
            });
        }

        private void CreateModel(CollectiveEmission collectiveEmission, File file, CacheListForProperty cacheListForProperty, CompanyPolicy companyPolicy)
        {
            string templateName = "";
            string propertyName = "";
            CollectiveEmissionRow collectiveEmissionRow = new CollectiveEmissionRow();
            try
            {
                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription).Distinct())).ToList();
                collectiveEmissionRow.MassiveLoadId = collectiveEmission.Id;
                collectiveEmissionRow.RowNumber = file.Id;
                collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Validation;
                collectiveEmissionRow.HasError = hasError;
                collectiveEmissionRow.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(),
                    errorList.Where(x => !string.IsNullOrEmpty(x)));
                collectiveEmissionRow.SerializedRow = JsonConvert.SerializeObject(file);
                DelegateService.collectiveService.CreateCollectiveEmissionRow(collectiveEmissionRow);
                if (hasError)
                {
                    return;
                }
                List<FilterIndividual> filtersIndividuals = cacheListForProperty.FilterIndividuals.Cast<FilterIndividual>().ToList();
                templateName = TemplatePropertyName.RiskDetail;
                Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows.First();

                CompanyPropertyRisk companyProperty = new CompanyPropertyRisk
                {
                    Status = RiskStatusType.Included,
                    CoveredRiskType = CoveredRiskType.Location,
                    CompanyRisk = new CompanyRisk()
                    {
                        CompanyInsured = DelegateService.massiveService.CreateInsured(row, companyPolicy.Holder, cacheListForProperty.FilterIndividuals)
                    },
                    Beneficiaries = new List<Beneficiary>(),
                    GroupCoverage = DelegateService.collectiveService.CreateGroupCoverage(row, companyPolicy.CompanyProduct.Id)
                };

                if (Settings.ImplementWebServices())
                {
                    //Vaidación Externos 
                    if (companyPolicy.CompanyProduct.IsScore.GetValueOrDefault())
                    {
                        companyProperty.CompanyRisk.CompanyInsured.ScoreCredit = cacheListForProperty.FilterIndividuals.Find(x => x.InsuredCode == companyProperty.CompanyRisk.CompanyInsured.InsuredId).ScoreCredit;
                    }
                    //Vaidación Externos 
                }
                else
                {
                    companyProperty.CompanyRisk.CompanyInsured.ScoreCredit = DelegateService.externalProxyService.GetScoreDefault();
                }
                companyProperty.Beneficiaries.Add(DelegateService.massiveService.CreateBeneficiary(row, companyProperty.CompanyRisk.CompanyInsured, cacheListForProperty.FilterIndividuals));

                string fullAddress = DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskAddress)).ToString();
                companyProperty.FullAddress = DelegateService.uniquePersonService.NormalizeAddress(fullAddress);
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
                    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskUseProperty))
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

                propertyName = FieldPropertyName.RiskConstructionYear;
                companyProperty.ConstructionYear = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskConstructionYear));

                int minYearAllowed = DelegateService.commonService.GetExtendedParameterByParameterId((int)CompanyParameterType.MinYearAllowed).NumberParameter.GetValueOrDefault();
                if (companyProperty.ConstructionYear > DateTime.Now.Year)
                {
                    collectiveEmissionRow.HasError = true;
                    collectiveEmissionRow.Observations = Errors.ErrorTheYearOfConstructionCanNotBeGreaterThanTheCurrent + KeySettings.ReportErrorSeparatorMessage();
                }
                else if (companyProperty.ConstructionYear < minYearAllowed)
                {
                    collectiveEmissionRow.HasError = true;
                    collectiveEmissionRow.Observations = Errors.ErrorTheYearOfConstructionCanNotBeLessThan + " " + minYearAllowed + KeySettings.ReportErrorSeparatorMessage();
                }

                propertyName = FieldPropertyName.RiskFloorNumber;
                companyProperty.FloorNumber = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskFloorNumber));
                int maxFloorNumber = (int)DelegateService.commonService.GetExtendedParameterByParameterId((int)CompanyParameterType.MaxFloorNumber).NumberParameter;
                int minFloorNumber = (int)DelegateService.commonService.GetExtendedParameterByParameterId((int)CompanyParameterType.MinFloorNumber).NumberParameter;

                if (companyProperty.FloorNumber > maxFloorNumber)
                {
                    collectiveEmissionRow.HasError = true;
                    collectiveEmissionRow.Observations = Errors.ErrorTheNumberOfFloorsCanNotBeGreaterThan + " " + maxFloorNumber + KeySettings.ReportErrorSeparatorMessage();
                }
                else if (companyProperty.FloorNumber < minFloorNumber)
                {
                    collectiveEmissionRow.HasError = true;
                    collectiveEmissionRow.Observations = Errors.ErrorTheNumberOfFloorsCanNotBeLessThan + " " + minFloorNumber + KeySettings.ReportErrorSeparatorMessage();
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
                //Plantillas Adicionales
                List<Beneficiary> beneficiaries = DelegateService.massiveService.CreateAdditionalBeneficiaries(file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.AdditionalBeneficiaries), cacheListForProperty.FilterIndividuals);
                decimal beneficiariesParticipation = beneficiaries.Sum(x => x.Participation);
                if (beneficiariesParticipation < 100)
                {
                    companyProperty.Beneficiaries[0].Participation -= beneficiariesParticipation;
                    companyProperty.Beneficiaries.AddRange(beneficiaries);
                }
                else
                {
                    throw new ValidationException(StringHelper.ConcatenateString("ErrorParticipationBeneficiary|", row.Number.ToString()));
                }
                decimal principalRateTri = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRateTRI));
                List<CompanyInsuredObject> insuredObjects = CreateInsuredObjects(file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.InsuredObjects), principalRateTri, collectiveEmissionRow);
                companyProperty.CompanyRisk.CompanyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdsGroupCoverageIdProductId(
                        insuredObjects.Select(x => x.Id).ToList(), companyProperty.GroupCoverage.Id, companyPolicy.CompanyProduct.Id, true);
                DateTime riskCurrentTo = (DateTime)DelegateService.commonService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
                companyProperty.CompanyRisk.CompanyCoverages.ForEach(x =>
                {
                    x.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                    x.CurrentFrom = riskCurrentTo;
                    x.CurrentTo = companyPolicy.CurrentTo;
                    x.CoverStatus = CoverageStatusType.Included;
                    CompanyInsuredObject insuredObject;
                    if (x.CompanyInsuredObject == null || (insuredObject = insuredObjects.FirstOrDefault(y => y.Id == x.CompanyInsuredObject.Id)) == null)
                    {
                        throw new ValidationException(StringHelper.ConcatenateString("MissingInsuredObjectForCoverage", x.Id.ToString()));
                    }
                    int compareFromResult = DateTime.Compare(companyPolicy.CurrentFrom, companyPolicy.CurrentTo);
                    if (compareFromResult >= 0)
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations = Errors.ErrorToDatePolicy + KeySettings.ReportErrorSeparatorMessage();
                    }
                    x.CompanyInsuredObject = insuredObject;
                });
                //Deducibles
                collectiveEmissionRow = CreateDeductibles(companyProperty.CompanyRisk.CompanyCoverages, file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.ModifyCoverages), insuredObjects, collectiveEmissionRow);

                templateName = TemplatePropertyName.Clauses;
                Template templateClauses = file.Templates.Find(p => p.PropertyName == templateName);
                companyProperty.Clauses = DelegateService.massiveService.GetClauses(templateClauses, EmissionLevel.Risk);
                companyProperty.CompanyRisk.CompanyCoverages.ForEach(p => p.Clauses = DelegateService.massiveService.GetClausesByCoverageId(templateClauses, p.Id));

                PendingOperation pendingOperationRisk = new PendingOperation
                {
                    ParentId = companyPolicy.Id,
                    UserId = companyPolicy.UserId,
                    Operation = JsonConvert.SerializeObject(companyProperty)
                };
                if (Settings.UseReplicatedDatabase())
                {

                    string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}", JsonConvert.SerializeObject(pendingOperationRisk), (char)007, JsonConvert.SerializeObject(collectiveEmissionRow), (char)007, nameof(CollectiveEmissionRow));
                    QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "CreatePendingOperationQuee");

                }
                else
                {
                    pendingOperationRisk = DelegateService.commonService.CreatePendingOperation(pendingOperationRisk);
                    collectiveEmissionRow.Risk = new Risk()
                    {
                        Id = pendingOperationRisk.Id
                    };
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                }
                if (Settings.ImplementWebServices() &&
                (companyPolicy.CompanyProduct.IsScore.GetValueOrDefault() && companyProperty.CompanyRisk.CompanyInsured.ScoreCredit == null))
                {
                    CheckExternalServices(cacheListForProperty, companyPolicy, companyProperty, collectiveEmission, file, row);
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
                        collectiveEmissionRow.Observations += Errors.ErrorRecoupmentPeriod + " " + itemInsuredObject.Id + "/ ";
                    }
                }
                if (itemInsuredObject.RequiredPercentageVariableIndex)
                {
                    if (insuredObjects.Find(x => x.Id == itemInsuredObject.Id).PercentageVariableIndex == 0)
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += Errors.ErrorPercentageVariableIndex + " " + itemInsuredObject.Id + "/ ";
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
                    coverages.Add(coverage);
                }
            }
            return collectiveEmissionRow;
        }
    }
}