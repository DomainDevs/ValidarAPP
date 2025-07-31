using Newtonsoft.Json;
using Sistran.Company.Application.Location.LiabilityModificationService.EEProvider.Resources;
using Sistran.Company.Application.Location.LiabilityServices.Models;
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

namespace Sistran.Company.Application.Location.LiabilityModificationService.EEProvider.DAOs
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
                Key5 = (int)SubCoveredRiskType.Liability
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
                collectiveEmission.File.Templates.First(p => p.PropertyName == TemplatePropertyName.RiskDetail).IsPrincipal = true;
                collectiveEmission.File.Templates.First(p => p.PropertyName == TemplatePropertyName.Policy).IsPrincipal = false;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                List<File> validatedFiles = DelegateService.commonService.GetDataTemplates(collectiveEmission.File.Templates);
                collectiveEmission.File.Templates.First(p => p.PropertyName == TemplatePropertyName.RiskDetail).IsPrincipal = false;
                collectiveEmission.File.Templates.First(p => p.PropertyName == TemplatePropertyName.Policy).IsPrincipal = true;
                InclusionValidationDAO inclusionValidationDAO = new InclusionValidationDAO();
                List<Validation> validationsPolicy = inclusionValidationDAO.GetValidationsByFilesPolicy(collectiveEmission.File, collectiveEmission);
                if (validationsPolicy.Count > 0)
                {
                    collectiveEmission.HasError = true;
                    collectiveEmission.ErrorDescription = string.Join(",", validationsPolicy.Select(x => x.ErrorMessage));
                    return;
                }
                Row rowEmision = policyTemplate.Rows.FirstOrDefault();
                int branchId = (int)DelegateService.commonService.GetValueByField<int>(rowEmision.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                decimal policyNum = (decimal)DelegateService.commonService.GetValueByField<decimal>(rowEmision.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumber));
                string policyText = (string)DelegateService.commonService.GetValueByField<string>(rowEmision.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyText));
                int prefixId = (int)DelegateService.commonService.GetValueByField<int>(rowEmision.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyPrefixCode));
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
                    collectiveEmission.Branch.Id = (int)DelegateService.commonService.GetValueByField<int>(rowEmision.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
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
                    companyPolicy.Text.TextBody = policyText;
                    companyPolicy.UserId = collectiveEmission.User.UserId;
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
                    //collectiveEmission.DocumentNumber = companyPolicy.DocumentNumber;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                    List<Validation> validations = inclusionValidationDAO.GetValidationsByFiles(validatedFiles, collectiveEmission, policy.Product.Id);
                    if (validations.Count > 0)
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
                    validatedFiles.ForEach(x => x.Templates.Add(policyTemplate));
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

        private void CreateModels(CollectiveEmission collectiveEmission, List<File> files, CompanyPolicy companyPolicy)
        {
            if (files[0].Templates.First(x => x.PropertyName == TemplatePropertyName.Policy).HasError)
            {
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = files[0].Templates.First(x => x.PropertyName == TemplatePropertyName.Policy).ErrorDescription;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            }
            else if (files[0].Templates.First(x => x.PropertyName == TemplatePropertyName.Policy).Rows[0].HasError)
            {
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = files[0].Templates.First(x => x.PropertyName == TemplatePropertyName.Policy).Rows[0].ErrorDescription;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            }
            else
            {
                List<FilterIndividual> filtersIndividuals = DelegateService.massiveService.GetFilterIndividuals(collectiveEmission.User.UserId, collectiveEmission.Branch.Id, files, TemplatePropertyName.Policy);
                companyPolicy.Summary = new Summary
                {
                    RiskCount = files.Count
                };

                ParallelHelper.ForEach(files, file =>
                {
                    string templateName = "";
                    string propertyName = "";
                    CollectiveEmissionRow collectiveEmissionRow = new CollectiveEmissionRow();
                    try
                    {
                        bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                        List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription))).ToList();
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
                            CompanyLiabilityRisk companyLiability = new CompanyLiabilityRisk
                            {
                                Status = RiskStatusType.Included,
                                CoveredRiskType = CoveredRiskType.Location,
                                CompanyRisk = new CompanyRisk()
                            };
                            companyLiability.CompanyRisk.CompanyInsured = DelegateService.massiveService.CreateInsured(row, companyPolicy.Holder, filtersIndividuals);
                            companyLiability.GroupCoverage = DelegateService.collectiveService.CreateGroupCoverage(row, companyPolicy.CompanyProduct.Id);
                            companyLiability.RatingZone = DelegateService.collectiveService.CreateRatingZone(row, companyPolicy.Prefix.Id);
                            companyLiability.Beneficiaries = new List<Beneficiary>();
                            companyLiability.Beneficiaries.Add(DelegateService.massiveService.CreateBeneficiary(row, companyLiability.CompanyRisk.CompanyInsured, filtersIndividuals));
                            List<Beneficiary> beneficiaries = DelegateService.massiveService.CreateAdditionalBeneficiaries(file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.AdditionalBeneficiaries), filtersIndividuals);
                            decimal beneficiariesParticipation = beneficiaries.Sum(x => x.Participation);
                            if (beneficiariesParticipation < 100)
                            {
                                companyLiability.Beneficiaries[0].Participation -= beneficiariesParticipation;
                                companyLiability.Beneficiaries.AddRange(beneficiaries);
                            }
                            else
                            {
                                throw new ValidationException(StringHelper.ConcatenateString("ErrorParticipationBeneficiary|", row.Number.ToString()));
                            }

                            var principalRateTri = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRateTRI));
                            List<CompanyInsuredObject> insuredObjects = CreateInsuredObjects(file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.InsuredObjects), principalRateTri);
                            companyLiability.CompanyRisk.CompanyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdsGroupCoverageIdProductId(
                                    insuredObjects.Select(x => x.Id).ToList(), companyLiability.GroupCoverage.Id, companyPolicy.CompanyProduct.Id, true);
                            DateTime riskCurrentTo = (DateTime)DelegateService.commonService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
                            companyLiability.CompanyRisk.CompanyCoverages.ForEach(x =>
                            {
                                x.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                                x.CurrentFrom = riskCurrentTo;
                                x.CurrentTo = companyPolicy.CurrentTo;
                                int compareFromResult = DateTime.Compare(companyPolicy.CurrentFrom, companyPolicy.CurrentTo);
                                if (compareFromResult >= 0)
                                {
                                    collectiveEmission.HasError = true;
                                    collectiveEmission.ErrorDescription = Errors.ErrorToDatePolicy;
                                }
                                x.CoverStatus = CoverageStatusType.Included;
                                CompanyInsuredObject insuredObject;
                                if (x.CompanyInsuredObject == null || (insuredObject = insuredObjects.FirstOrDefault(y => y.Id == x.CompanyInsuredObject.Id)) == null)
                                {
                                    throw new ValidationException(StringHelper.ConcatenateString("MissingInsuredObjectForCoverage", x.Id.ToString()));
                                }
                                x.CompanyInsuredObject = insuredObject;
                            });
                        //Deducibles
                        CreateDeductibles(companyLiability.CompanyRisk.CompanyCoverages, file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.ModifyCoverages));
                            companyLiability.FullAddress = DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskAddress)).ToString();
                            companyLiability.NomenclatureAddress = new Core.Application.Locations.Models.NomenclatureAddress
                            {
                                Type = new RouteType
                                {
                                    Id = 1
                                },
                            };
                            companyLiability.City = new City()
                            {
                                Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCity))
                            };
                            companyLiability.City.State = new State
                            {
                                Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskState))
                            };
                            companyLiability.City.State.Country = new Country
                            {
                                Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCountry))
                            };
                            companyLiability.RiskActivity = new RiskActivity
                            {
                                Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskActivity))
                            };
                            companyLiability.RiskUse = new RiskUse
                            {
                                Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskUseProperty))
                            };
                            companyLiability.IsDeclarative = (bool)DelegateService.commonService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskIsFacultative));
                            companyLiability.RatingZone = new RatingZone
                            {
                                Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRatingZone))
                            };
                            companyLiability.ConstructionType = new ConstructionType
                            {
                                Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCOfConstruction))
                            };
                            companyLiability.PML = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskEML));
                            companyLiability.Square = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskBlock));
                            companyLiability.Latitude = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLatitude));
                            companyLiability.Longitude = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLongitude));
                            companyLiability.ConstructionYear = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskConstructionYear));
                            companyLiability.FloorNumber = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskFloorNumber));
                        //company
                        //companyLiability.CompanyLocation = new CompanyLocation
                        //{
                        //    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskLocation))
                        //};
                        //companyLiability.CompanyLocation.CompanyDistrict = new CompanyDistrict
                        //{
                        //    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskNeighborhood))
                        //};
                        //companyLiability.CompanyLevelZone = new CompanyLevelZone
                        //{
                        //    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskLevelZone))
                        //};
                        //companyLiability.IsResidential = (bool)DelegateService.commonService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIsResidential));
                        //companyLiability.IsOutCommunity = (bool)DelegateService.commonService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIsOutCommunity));
                        //companyLiability.CompanyRiskTypeEarthquake = new CompanyRiskTypeEarthquake
                        //{
                        //    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskTypeCode))
                        //};
                        //companyLiability.CompanyAssistanceType = new CompanyAssistanceType
                        //{
                        //    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskAsistType))
                        //};
                        //companyLiability.CompanyRisk.AssistanceId = companyLiability.CompanyAssistanceType.Id;
                        //companyLiability.CompanyIrregularHeight = new CompanyIrregularHeight
                        //{
                        //    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIrregularHeight))
                        //};
                        //companyLiability.CompanyIrregularPlant = new CompanyIrregularPlant
                        //{
                        //    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIrregularPlant))
                        //};
                        //companyLiability.CompanyDamage = new CompanyDamage
                        //{
                        //    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskPreviousDamage))
                        //};
                        //companyLiability.CompanyRepair = new CompanyRepair
                        //{
                        //    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskRepairedCode))
                        //};
                        //companyLiability.CompanyStructureType = new CompanyStructureType
                        //{
                        //    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskStructureCode))
                        //};
                        //companyLiability.CompanyReinforcedStructureType = new CompanyReinforcedStructureType
                        //{
                        //    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskReinforcedStructureType))
                        //};

                        //companyLiability.CompanyUseHouse = new CompanyUseHouse
                        //{
                        //    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskUseLiability))
                        //};
                        //companyLiability.CompanyHouseType = new CompanyHouseType
                        //{
                        //    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskTypeOfLiability))
                        //};

                        //companyLiability.CompanyMicroZone = new CompanyMicroZone
                        //{
                        //    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskMicrozoningZone))
                        //};

                        //companyLiability.CompanyRisk.CompanyInsured.ClaimIncurred = new ClaimIncurred();
                        //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskSinisterPercentage).Value))
                        //{
                        //    propertyName = CompanyFieldPropertyName.RiskSinisterPercentage;

                        //    companyLiability.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskSinisterPercentage));
                        //}
                        //else
                        //{
                        //    companyLiability.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate = DelegateService.uniquePersonService.GetSinisterPercentageByInsuredId(companyLiability.CompanyRisk.CompanyInsured.InsuredId).AccidentRate;
                        //}
                        //companyLiability.Text = new Text
                        //{
                        //    TextBody = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText))
                        //};
                        PendingOperation pendingOperationRisk = new PendingOperation
                            {
                                ParentId = companyPolicy.Id,
                                UserId = companyPolicy.UserId,
                                Operation = JsonConvert.SerializeObject(companyLiability)
                            };
                            if (!Settings.UseReplicatedDatabase())
                            {
                                DelegateService.commonService.CreatePendingOperation(pendingOperationRisk);
                            }
                            else
                            {
                                DelegateService.pendingOperationEntityService.CreatePendingOperation(pendingOperationRisk);
                            }
                            if (collectiveEmissionRow.Risk == null)
                            {
                                collectiveEmissionRow.Risk = new Risk();
                            }
                            collectiveEmissionRow.Risk.RiskId = pendingOperationRisk.Id;
                        }
                        DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                    }
                    catch (Exception ex)
                    {
                        collectiveEmissionRow.HasError = true;
                        if (string.IsNullOrEmpty(templateName))
                        {
                            collectiveEmissionRow.Observations += Errors.ErrorCreateModel;
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
                });
            }
        }

        private List<CompanyInsuredObject> CreateInsuredObjects(Template insuredObjectTemplate, decimal principalRateTri)
        {
            if (insuredObjectTemplate == null)
            {
                throw new ValidationException("InsuredObjectTemplateNotFound");
            }
            List<CompanyInsuredObject> insuredObjects = new List<CompanyInsuredObject>();
            int id = 0, amount = 0, recoupmentPeriodId = 0;
            decimal rateTri = 0, percentageVariableIndex = 0;
            foreach (Row row in insuredObjectTemplate.Rows)
            {
                id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                amount = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectSumAssured));
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
            if (!insuredObjects.Any())
            {
                throw new ValidationException("NoInsuredObjectsFound");
            }
            return insuredObjects;
        }

        private void CreateDeductibles(List<CompanyCoverage> coverages, Template deductiblesTemplate)
        {
            if (deductiblesTemplate == null)
            {
                return;
            }
            foreach (Row row in deductiblesTemplate.Rows)
            {
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
    }
}