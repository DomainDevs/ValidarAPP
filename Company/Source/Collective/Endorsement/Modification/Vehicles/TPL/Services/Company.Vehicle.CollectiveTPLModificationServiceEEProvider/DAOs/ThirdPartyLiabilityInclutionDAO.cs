using AutoMapper;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Vehicles.CollectiveTPLModificationService.Models;
using Sistran.Company.Application.Vehicles.CollectiveTPLModificationServiceEEProvider.Assemblers;
using Sistran.Company.Application.Vehicles.CollectiveTPLModificationServiceEEProvider.Resources;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Framework;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using VEMO = Sistran.Core.Application.Vehicles.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicles.CollectiveTPLModificationServiceEEProvider.DAOs
{
    public class ThirdPartyLiabilityInclutionDAO
    {
        string templateName = "";

        public CollectiveEmission CreateVehicleInclution(CollectiveEmission collectiveLoad)
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

        private void ValidateFile(CollectiveEmission collectiveLoad)
        {
            FileProcessValue fileProcessValue = new FileProcessValue()
            {
                Key1 = (int)FileProcessType.CollectiveInclusion,
                Key2 = (int)EndorsementType.Modification,
                Key4 = collectiveLoad.Prefix.Id,
                Key5 = (int)SubCoveredRiskType.ThirdPartyLiability
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

        private void ValidateData(CollectiveEmission collectiveEmission)
        {
            try
            {
                var file = collectiveEmission.File;
                file = DelegateService.utilitiesService.ValidateDataFile(file, collectiveEmission.User.AccountName);
                Template policyTemplate = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Policy);
                file.Templates.Remove(policyTemplate);
                collectiveEmission.TotalRows = file.Templates.First(p => p.IsPrincipal).Rows.Count;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                Template additionalCoveranges = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.AdditionalCoverages);
                List<File> validatedFiles = DelegateService.utilitiesService.GetDataTemplates(collectiveEmission.File.Templates);
         
                Row policyRow = policyTemplate.Rows.First();
                if (policyRow.HasError)
                {
                    collectiveEmission.HasError = true;
                    collectiveEmission.ErrorDescription = policyRow.ErrorDescription;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                    return;
                }
                int branchId = (int)DelegateService.utilitiesService.GetValueByField<int>(policyRow.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                decimal policyNum = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(policyRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumber));
                int prefixId = (int)DelegateService.utilitiesService.GetValueByField<int>(policyRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
                if (collectiveEmission.Prefix.Id != prefixId)
                {
                    collectiveEmission.HasError = true;
                    collectiveEmission.ErrorDescription = Errors.PolicyNotCorrespondPrefix;
                    DelegateService.massiveService.UpdateMassiveLoad(collectiveEmission);
                    return;
                }
                Policy policy = DelegateService.underwritingService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNum);
                if (policy == null)
                {
                    collectiveEmission.HasError = true;
                    collectiveEmission.ErrorDescription = Errors.PolicyNotFound;
                    DelegateService.massiveService.UpdateMassiveLoad(collectiveEmission);
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
                collectiveEmission.File.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows = DelegateService.massiveService.GetMassivePlatesValidation(collectiveEmission.File.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows);
                ThirdPartyLiabilityInclutionValidationDAO tplInclutionValidationDAO = new ThirdPartyLiabilityInclutionValidationDAO();
                List<Validation> validations = tplInclutionValidationDAO.GetValidationsByFiles(validatedFiles, collectiveEmission, policy.Product.Id, companyPolicy.PolicyType.Id, policyRow, policy.CurrentFrom, policy.CurrentTo);
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
                collectiveEmission.Product.Id = companyPolicy.Product.Id;
                collectiveEmission.DocumentNumber = policyNum;
                collectiveEmission.Agency = companyPolicy.Agencies.FirstOrDefault();
                companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(collectiveEmission.Product.Id, companyPolicy.Prefix.Id);
                companyPolicy.CurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                companyPolicy.CurrentTo = companyPolicy.Endorsement.CurrentTo;
                companyPolicy.Summary = new CompanySummary
                {
                    RiskCount = collectiveEmission.TotalRows
                };
                companyPolicy.Endorsement.EndorsementType = EndorsementType.Modification;
                companyPolicy.Endorsement.IsUnderIdenticalConditions = true;
                companyPolicy.Endorsement.IsMassive = true;
                companyPolicy.TemporalType = TemporalType.Endorsement;
                if (companyPolicy.Text == null)
                {
                    companyPolicy.Text = new CompanyText();
                }
                companyPolicy.Text.TextBody = policyRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyText)?.Value;
                companyPolicy.UserId = collectiveEmission.User.UserId;
                companyPolicy.Clauses = DelegateService.massiveService.GetClauses(collectiveEmission.File.Templates.Find(p => p.PropertyName == TemplatePropertyName.Clauses), EmissionLevel.General);
                companyPolicy.Endorsement.TicketDate = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(policyRow.Fields.Find(x => x.PropertyName == FieldPropertyName.TicketDate));
                companyPolicy.Endorsement.TicketNumber = (int)DelegateService.utilitiesService.GetValueByField<int>(policyRow.Fields.Find(x => x.PropertyName == FieldPropertyName.TicketNumber));
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                validatedFiles.ForEach(x => x.Templates.Add(policyTemplate));

                CreateModels(collectiveEmission, validatedFiles, companyPolicy, policyRow);
                DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveEmission.Id);
            }
            catch (Exception ex)
            {
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription += StringHelper.ConcatenateString(Errors.ErrorValidatingFile + " " + ex.Message, "|");
            }
            finally
            {
                DataFacadeManager.Dispose();
            }

        }

        private List<ThirdPartyLiabilityFilterIndividual> CreateThirdPartyLiabilityFilterIndividual(List<FilterIndividual> filtersIndividuals, int prefixId, int userId)
        {
            if (filtersIndividuals.Count == 0)
                return new List<ThirdPartyLiabilityFilterIndividual>();

            //List<InsuredInfringementCount> infringments = new List<InsuredInfringementCount>();
            List<FilterIndividual> individualWithError = new List<FilterIndividual>();

            individualWithError.AddRange(filtersIndividuals.Where(i => !string.IsNullOrEmpty(i.Error)));
            filtersIndividuals.RemoveAll(i => !string.IsNullOrEmpty(i.Error));

            //if (Settings.UseReplicatedDatabase())
            //{
            //    filtersIndividuals = DelegateService.externalProxyMirrorService.GetMassiveScoresCreditByLastValid(filtersIndividuals, prefixId, userId);
            //    infringments = DelegateService.externalProxyMirrorService.GetMassiveInfringementSimitLastValid(filtersIndividuals);
            //}
            //else
            //{
            //    filtersIndividuals = DelegateService.externalProxyService.GetMassiveScoresCreditByLastValid(filtersIndividuals, prefixId, userId);
            //    infringments = DelegateService.externalProxyService.GetMassiveInfringementSimitLastValid(filtersIndividuals);
            //}

            filtersIndividuals.AddRange(individualWithError);

            List<ThirdPartyLiabilityFilterIndividual> vehicleFilterIndividuals = ModelAssembler.CreateVehicleFilterIndividuals(filtersIndividuals);

            //foreach (InsuredInfringementCount item in infringments)
            //{
            //    vehicleFilterIndividuals.Find(v => v.InsuredCode == item.InsuredId).InfringementCounts = item.InfringementsCounts;
            //}

            return vehicleFilterIndividuals;
        }


        private void CreateModels(CollectiveEmission collectiveLoad, List<File> files, CompanyPolicy policy, Row policyRow)
        {
            List<FilterIndividual> filterIndividuals = new List<FilterIndividual>();
            CacheListForThirdPartyLiability cacheListForTpl = new CacheListForThirdPartyLiability();
            List<CompanyClause> companyClauses = new List<CompanyClause>();
            List<CompanyClause> companyRiskClauses = new List<CompanyClause>();
            List<Clause> clauses = new List<Clause>();

            List<Clause> riskClauses = new List<Clause>();

            if (!policyRow.HasError && files.Exists(x => x.Templates.First(z => z.PropertyName == TemplatePropertyName.RiskDetail).Rows.Any(t => !t.HasError)))
            {
                PendingOperation pendingOperation = new PendingOperation
                {
                    Operation = COMUT.JsonHelper.SerializeObjectToJson(policy),
                    UserId = policy.UserId
                };
                if (!Settings.UseReplicatedDatabase())
                {
                    pendingOperation = DelegateService.utilitiesService.CreatePendingOperation(pendingOperation);
                }
                else
                {
                    pendingOperation = DelegateService.pendingOperationEntityService.CreatePendingOperation(pendingOperation);
                }
                policy.Id = pendingOperation.Id;
                collectiveLoad.TemporalId = policy.Id;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
                filterIndividuals = DelegateService.massiveService.GetFilterIndividualsForCollective(policyRow, files, collectiveLoad.User.UserId, collectiveLoad.Branch.Id, FieldPropertyName.PolicyNumber, FieldPropertyName.PrefixCode);
                cacheListForTpl.TplFilterIndividuals = CreateThirdPartyLiabilityFilterIndividual(filterIndividuals.ToList(), collectiveLoad.Prefix.Id, collectiveLoad.Prefix.Id);
                cacheListForTpl.InsuredForScoreList = new List<int>();
                cacheListForTpl.InsuredForSimitList = new List<int>();

                //Valida sarlaft sobre el tomador
                ValidateSarlaft(filterIndividuals, policy.Holder);
            }
            riskClauses = DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.Risk, (int)CoveredRiskType.Vehicle);
            clauses = DelegateService.underwritingService.GetClausesByEmissionLevel(EmissionLevel.Coverage);

            foreach (var riskClause in riskClauses)
            {
                companyRiskClauses.Add(MappCompanyClause(riskClause));
            }

            foreach (var clause in clauses)
            {
                companyClauses.Add(MappCompanyClause(clause));
            }

            ParallelHelper.ForEach(files, file =>
            {
                CreateModel(collectiveLoad, file, cacheListForTpl, policy, companyRiskClauses, companyClauses);
            });
        }

        /// <summary>
        /// Valida el sarlaft en la row
        /// </summary>
        /// <param name="filterIndividuals"></param>
        /// <param name="row"></param>
        private void ValidateSarlaft(List<FilterIndividual> filterIndividuals, Holder holder)
        {
            #region holder

            //Validación sarlaft
            FilterIndividual indiv = filterIndividuals.Where(i => (i.IndividualType == IndividualType.Person && i.Person.IndividualId == holder.IndividualId) || (i.IndividualType == IndividualType.Company && i.Company.IndividualId == holder.IndividualId)).FirstOrDefault();

            if (indiv != null && !string.IsNullOrEmpty(indiv.SarlaftError))
            {
                if (indiv.SarlaftError == "ValidateSarlaftExpired")
                {
                    throw new ValidationException(Errors.ValidateSarlaftExpired);
                }
                else if (indiv.SarlaftError == "ValidateSarlaftOvercome")
                {
                    throw new ValidationException(Errors.ValidateSarlaftOvercome);
                }
                else if (indiv.SarlaftError == "ValidateSarlaftPending")
                {
                    throw new ValidationException(Errors.ValidateSarlaftPending);
                }
            }
            else
            {
                throw new ValidationException(Errors.ValidateSarlaftExists);
            }

            #endregion
        }

        private void CreateModel(CollectiveEmission collectiveEmission, File file, CacheListForThirdPartyLiability cacheListForTpl, CompanyPolicy companyPolicy, List<CompanyClause> companyRiskClauses, List<CompanyClause> companyClauses)
        {
            string propertyName = "";
            CollectiveEmissionRow collectiveEmissionRow = new CollectiveEmissionRow();

            try
            {
                templateName = TemplatePropertyName.RiskDetail;
                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription).Distinct())).ToList();
                collectiveEmissionRow.MassiveLoadId = collectiveEmission.Id;
                collectiveEmissionRow.RowNumber = file.Id;
                collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Validation;
                collectiveEmissionRow.HasError = hasError;
                collectiveEmissionRow.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(),
                    errorList.Where(x => !string.IsNullOrEmpty(x)));
                collectiveEmissionRow.SerializedRow = COMUT.JsonHelper.SerializeObjectToJson(file);
                DelegateService.collectiveService.CreateCollectiveEmissionRow(collectiveEmissionRow);
                if (hasError)
                {
                    DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveEmission.Id);
                    return;
                }

                templateName = TemplatePropertyName.EmissionThirdPartyLiability;

                Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows.First();
                RatingZone rating = DelegateService.collectiveService.CreateRatingZone(row, companyPolicy.Prefix.Id);
                List<FilterIndividual> filtersIndividuals = cacheListForTpl.TplFilterIndividuals.Cast<FilterIndividual>().ToList();

                LimitRc limitRc = new LimitRc();
                limitRc = DelegateService.underwritingService.GetLimitsRcByPrefixIdProductIdPolicyTypeId(companyPolicy.Prefix.Id, companyPolicy.Product.Id, companyPolicy.PolicyType.Id).FirstOrDefault(x => x.IsDefault == true);

                CompanyTplRisk companyTplRisk = new CompanyTplRisk
                {
                    Risk = new CompanyRisk
                    {
                        Status = RiskStatusType.Included,
                        CoveredRiskType = CoveredRiskType.Vehicle,
                        MainInsured = DelegateService.massiveService.CreateInsured(row, companyPolicy.Holder, filtersIndividuals),
                        GroupCoverage = DelegateService.collectiveService.CreateGroupCoverage(row, companyPolicy.Product.Id),
                        RatingZone = new CompanyRatingZone
                        {
                            Id = rating.Id,
                            Description = rating.Description,
                            Prefix = new CompanyPrefix
                            {
                                Id = rating.Prefix.Id
                            },
                            SmallDescription = rating.SmallDescription,
                        },
                        Beneficiaries = new List<CompanyBeneficiary>(),
                        LimitRc = new CompanyLimitRc
                        {
                            Id = limitRc.Id,
                            LimitSum = limitRc.LimitSum,
                        },
                        Policy = companyPolicy
                    },
                    Deductible = new CompanyDeductible(),
                    ServiceType = new CompanyServiceType
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskServiceType))
                    },
                    //GoodExperienceYear = new VSMO.GoodExperienceYear(),
                };

                if (companyPolicy.Product.IsFlatRate)
                {
                    if (string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskRate).Value))
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += Errors.ErrorProductIsFlateRate + KeySettings.ReportErrorSeparatorMessage();
                    }
                }
                companyTplRisk.Shuttle = new CompanyShuttle();
                companyTplRisk.Shuttle.Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskShuttleCode));
                companyTplRisk.Risk.Beneficiaries.Add(DelegateService.massiveService.CreateBeneficiary(row, companyTplRisk.Risk.MainInsured, filtersIndividuals));
                propertyName = FieldPropertyName.RiskYear;
                companyTplRisk.Year = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskYear));
                //Fasecolda
                companyTplRisk.Make = new CompanyMake();
                companyTplRisk.Make.Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskMakeCode));

                companyTplRisk.Risk.IsFacultative = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIsFacultative));
                companyTplRisk.Risk.IsRetention = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskOnehundredRetention));

                if (companyTplRisk.Risk.IsFacultative == true && companyTplRisk.Risk.IsRetention == true)
                {
                    collectiveEmissionRow.HasError = true;
                    collectiveEmissionRow.Observations += string.Format(Errors.ValidateExcludingFacultativeFields, row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIsFacultative).Description, row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskOnehundredRetention).Description);
                }

                companyTplRisk.Version = new CompanyVersion();
                companyTplRisk.Model = new CompanyModel();
                if (companyTplRisk.Make.Id > 0)
                {
                    VEMO.Version version = DelegateService.tplService.GetVersionsByMakeIdYear(companyTplRisk.Make.Id, companyTplRisk.Year);

                    if (version != null)
                    {

                        version = DelegateService.tplService.GetVersionByVersionIdModelIdMakeId(version.Id, version.Model.Id, version.Make.Id);
                        companyTplRisk.Version = ModelAssembler.CreateCompanyVersion(version);
                        var config = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<VEMO.Model, CompanyModel>();

                        });
                        companyTplRisk.Model = config.CreateMapper().Map<VEMO.Model, CompanyModel>(version.Model);

                    }
                }

                propertyName = FieldPropertyName.RiskUse;
                companyTplRisk.Use = new CompanyUse
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskUse))
                };
                propertyName = FieldPropertyName.RiskPrice;
                //companyTplRisk.OriginalPrice = companyTplRisk.Price;
                companyTplRisk.NewPrice = DelegateService.tplService.GetYearsByMakeIdModelIdVersionId(companyTplRisk.Make.Id, companyTplRisk.Model.Id, companyTplRisk.Version.Id).Last().Price;
                propertyName = FieldPropertyName.RiskRateTRI;
                companyTplRisk.Rate = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRate));
                propertyName = FieldPropertyName.RiskIsNew;
                companyTplRisk.IsNew = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskIsNew));
                propertyName = FieldPropertyName.RiskLicensePlate;
                companyTplRisk.LicensePlate = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLicensePlate)).ToString();
                propertyName = FieldPropertyName.RiskEngine;
                companyTplRisk.EngineSerial = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskEngine)).ToString();
                propertyName = FieldPropertyName.RiskChassis;
                companyTplRisk.ChassisSerial = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskChassis)).ToString();
                propertyName = FieldPropertyName.RiskColor;
                companyTplRisk.Color = new CompanyColor
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<Int32>(
                        row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskColor))
                };

                propertyName = FieldPropertyName.RiskText;
                companyTplRisk.Risk.Text = new CompanyText
                {
                    TextBody = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText)).ToString()
                };
                companyTplRisk.Risk.Coverages = DelegateService.collectiveService.CreateCoverages(companyPolicy.Product.Id, companyTplRisk.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);


                int primaryCoverageId = companyTplRisk.Risk.Coverages.Where(x => x.IsPrimary == true).FirstOrDefault().Id;

                List<Deductible> deductibles = DelegateService.underwritingService.GetDeductiblesByCoverageId(primaryCoverageId);

                int templateDeductibleId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskDeductibleCode));
                if (deductibles.Any(x => x.Id == templateDeductibleId))
                {
                    companyTplRisk.Deductible.Id = templateDeductibleId;
                }
                else
                {
                    collectiveEmissionRow.HasError = true;
                    collectiveEmissionRow.Observations += Errors.InvalidDeductible + KeySettings.ReportErrorSeparatorMessage();
                }



                Template templateAdditionalCoverages = file.Templates.Find(p => p.PropertyName == CompanyTemplatePropertyName.AdditionalCoverages);

                if (templateAdditionalCoverages != null)
                {
                    companyTplRisk.Risk.Coverages = DelegateService.massiveService.CreateAdditionalCoverages(companyTplRisk.Risk.Coverages, companyTplRisk.Risk.Coverages.Where(x => x.IsMandatory || x.IsSelected).ToList(), templateAdditionalCoverages.Rows);

                    if (templateAdditionalCoverages.Rows.Any(x => x.HasError))
                    {
                        collectiveEmissionRow.HasError = true;
                        errorList = templateAdditionalCoverages.Rows.Select(t => string.Join(",", t.ErrorDescription)).ToList();
                        collectiveEmissionRow.Observations += string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                    }
                }

                companyTplRisk.Risk.Coverages.ForEach(x =>
                {
                    x.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                    x.CurrentFrom = companyPolicy.CurrentFrom;
                    x.CurrentTo = companyPolicy.CurrentTo;
                    x.CoverStatus = CoverageStatusType.Included;
                    int compareFromResult = DateTime.Compare(companyPolicy.CurrentFrom, companyPolicy.CurrentTo);
                    if (compareFromResult >= 0)
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations = Errors.ErrorToDatePolicy + KeySettings.ReportErrorSeparatorMessage();
                    }
                });
                propertyName = CompanyFieldPropertyName.BirthDateEldestSon;
                //companyTplRisk.BirthDateEldestson = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.BirthDateEldestSon));
                propertyName = FieldPropertyName.RiskFuel;
                companyTplRisk.Version.Fuel = new CompanyFuel()
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskFuel))
                };

                templateName = TemplatePropertyName.Deductible;
                companyTplRisk.Risk.Coverages = DelegateService.massiveService.CreateDeductibles(companyTplRisk.Risk.Coverages, file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Deductible));

                // Template Beneficiarios Adicionales.
                Template templateAdditionalBeneficiaries = file.Templates.Find(x => x.PropertyName == TemplatePropertyName.AdditionalBeneficiaries);

                if (templateAdditionalBeneficiaries != null)
                {
                    string errorAdditionalBeneficiaries = string.Empty;
                    List<CompanyBeneficiary> companyBeneficiaries = DelegateService.massiveService.GetBeneficiariesAdditional(file, templateAdditionalBeneficiaries, filtersIndividuals, companyTplRisk.Risk.Beneficiaries, ref errorAdditionalBeneficiaries);
                    if (string.IsNullOrEmpty(errorAdditionalBeneficiaries))
                    {
                        companyTplRisk.Risk.Beneficiaries = companyBeneficiaries;
                    }
                    else
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += errorAdditionalBeneficiaries + KeySettings.ReportErrorSeparatorMessage();
                    }
                }


                Template templateClauses = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.Clauses);

                if (templateClauses != null)
                {
                    string errorClause = string.Empty;
                    List<CompanyClause> companyClause = new List<CompanyClause>();
                    List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();
                    DelegateService.massiveService.GetClausesByTemplate(templateClauses, ref companyClause, ref companyCoverages, companyRiskClauses, companyClauses, ref errorClause);
                    if (string.IsNullOrEmpty(errorClause))
                    {
                        if (companyClause.Count > 0)
                        {
                            companyTplRisk.Risk.Clauses = companyClause;
                        }
                        if (companyCoverages.Count > 0)
                        {
                            companyTplRisk.Risk.Coverages = companyCoverages;
                        }
                    }
                    else
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += errorClause + KeySettings.ReportErrorSeparatorMessage();
                    }
                }

                Template templateScript = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.DinamicConcepts);

                if (templateScript != null)
                {
                    string errorScript = string.Empty;
                    List<DynamicConcept> dynamicConcepts = DelegateService.massiveService.GetDynamicConceptsByTemplate(companyPolicy.Product.CoveredRisk.ScriptId, templateScript, ref errorScript);

                    if (string.IsNullOrEmpty(errorScript))
                    {
                        companyTplRisk.Risk.DynamicProperties = dynamicConcepts;
                    }
                    else
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += errorScript + KeySettings.ReportErrorSeparatorMessage();
                    }
                }

                templateName = "";

                PendingOperation pendingOperationRisk = new PendingOperation
                {
                    ParentId = companyPolicy.Id,
                    UserId = companyPolicy.UserId,
                    Operation = COMUT.JsonHelper.SerializeObjectToJson(companyTplRisk)
                };
                if (Settings.UseReplicatedDatabase())
                {
                    string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}", COMUT.JsonHelper.SerializeObjectToJson(pendingOperationRisk), (char)007, COMUT.JsonHelper.SerializeObjectToJson(collectiveEmissionRow), (char)007, nameof(CollectiveEmissionRow));
                    QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "CreatePendingOperationQuee");
                }
                else
                {
                    pendingOperationRisk = DelegateService.utilitiesService.CreatePendingOperation(pendingOperationRisk);
                    collectiveEmissionRow.Risk = new Risk()
                    {
                        RiskId = pendingOperationRisk.Id
                    };
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                }

                //companyTplRisk = DelegateService.tplService.CreateVehicleTemporal(companyTplRisk);
                //collectiveEmissionRow.Risk = new Risk()
                //{
                //    RiskId = companyTplRisk.Risk.Id
                //};
                //DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);

                //if (Settings.ImplementWebServices() &&
                //    (companyPolicy.Product.IsScore.GetValueOrDefault() && companyTplRisk.Risk.MainInsured.ScoreCredit == null
                //    || companyPolicy.Product.IsFine.GetValueOrDefault() && companyTplRisk.ListInfringementCount == null
                //     || companyPolicy.Product.IsFasecolda.GetValueOrDefault()))
                //{
                //    CheckExternalServices(cacheListForTpl, companyPolicy, companyTplRisk, collectiveEmission, file, row);
                //}
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

        private void CheckExternalServices(CacheListForThirdPartyLiability cacheListForTpl, CompanyPolicy companyPolicy, CompanyTplRisk companyTplRisk, CollectiveEmission collectiveLoad, File file, Row row)
        {
            //ThirdPartyLiabilityFilterIndividual vehicleFilterIndividual = cacheListForTpl.VehicleFilterIndividuals.Find(x => x.InsuredCode == companyTplRisk.CompanyRisk.CompanyInsured.InsuredId);
            ThirdPartyLiabilityFilterIndividual vehicleFilterIndividual = new ThirdPartyLiabilityFilterIndividual();
            bool scoreAlreadyQueried = false; bool simitAlreadyQueried = false; bool requireScore = false; bool requireSimit = false; bool requireFasecolda = false;
            string licencePlate = string.Empty; string surname = string.Empty;
            //IdentificationDocument identificationDocument = new IdentificationDocument();

            //if (vehicleFilterIndividual.IndividualType == Core.Application.UniquePersonService.Enums.IndividualType.LegalPerson)
            //{
            //    surname = vehicleFilterIndividual.Company.Name;
            //    identificationDocument = vehicleFilterIndividual.Company.IdentificationDocument;
            //}
            //else if (vehicleFilterIndividual.IndividualType == Core.Application.UniquePersonService.Enums.IndividualType.Person)
            //{
            //    surname = vehicleFilterIndividual.Person.Surname;
            //    identificationDocument = vehicleFilterIndividual.Person.IdentificationDocument;
            //}

            if (companyPolicy.Product.IsScore.GetValueOrDefault() && companyTplRisk.Risk.MainInsured.ScoreCredit == null)
            {
                requireScore = true;


                //if (!cacheListForTpl.InsuredForScoreList.Contains(companyTplRisk.CompanyRisk.CompanyInsured.InsuredId))
                //{
                //    cacheListForTpl.InsuredForScoreList.Add(companyTplRisk.CompanyRisk.CompanyInsured.InsuredId);
                //}
                //else
                //{
                //    scoreAlreadyQueried = true;
                //}
            }
            //if (companyPolicy.Product.IsFine.GetValueOrDefault() && companyTplRisk.ListInfringementCount == null)
            //{
            //    requireSimit = true;
            //    if (!cacheListForTpl.InsuredForSimitList.Contains(companyTplRisk.CompanyRisk.CompanyInsured.InsuredId))
            //    {
            //        cacheListForTpl.InsuredForSimitList.Add(companyTplRisk.CompanyRisk.CompanyInsured.InsuredId);
            //    }
            //    else
            //    {
            //        simitAlreadyQueried = true;
            //    }
            //}
            if (companyPolicy.Product.IsFasecolda.GetValueOrDefault())
            {
                requireFasecolda = true;
                licencePlate = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.RiskLicensePlate));
            }

            if (requireScore || requireSimit || requireFasecolda)
            {
                //DelegateService.externalProxyService.CheckExternalServices(identificationDocument, surname, vehicleFilterIndividual.InsuredCode.Value, licencePlate, collectiveLoad.Id, file.Id, (int)SubCoveredRiskType.Vehicle, collectiveLoad.User.UserId, scoreAlreadyQueried, simitAlreadyQueried, requireScore, requireSimit, requireFasecolda);
            }
        }

        private CompanyClause MappCompanyClause(Clause clause)
        {
            CompanyClause companyClause = new CompanyClause();
            companyClause.Id = clause.Id;
            companyClause.IsMandatory = clause.IsMandatory;
            companyClause.Name = clause.Name;
            companyClause.Text = clause.Text;
            companyClause.Title = clause.Title;
            return companyClause;
        }


    }
}