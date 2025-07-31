using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.ModificationService.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Vehicle.ModificationService.EEProvider.Assemblers;
using Sistran.Company.Application.Vehicle.ModificationService.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.Models;
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
using Sistran.Core.Framework;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using VSMO = Sistran.Company.Application.Vehicles.VehicleServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;


namespace Sistran.Company.Application.Vehicle.ModificationService.EEProvider.DAOs
{
    public class VehicleInclutionDAO
    {
        string templateName = "";
        private static int coverIdAccesoryNoORig = 0;
        private static int coverIdAccesoryORig = 0;

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
                    DelegateService.massiveService.UpdateMassiveLoad(collectiveEmission);
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

                VehicleInclutionValidationDAO vehicleInclutionValidationDAO = new VehicleInclutionValidationDAO();
                List<Validation> validations = vehicleInclutionValidationDAO.GetValidationsByFiles(validatedFiles, collectiveEmission, policy.Product.Id, companyPolicy.PolicyType.Id, policyRow, policy.CurrentFrom, policy.CurrentTo, companyPolicy.Branch != null && companyPolicy.Branch.SalePoints != null && companyPolicy.Branch.SalePoints.Any() ? companyPolicy.Branch.SalePoints[0].Id : 0);
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
                companyPolicy.SubMassiveProcessType = SubMassiveProcessType.Inclusion;
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
                collectiveEmission.Status = MassiveLoadStatus.Validated;
                collectiveEmission.HasError = true;

                if (string.IsNullOrEmpty(templateName))
                {
                    collectiveEmission.ErrorDescription += StringHelper.ConcatenateString(Errors.ErrorValidatingFile + " " + ex.Message, "|");
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

        private List<VehicleFilterIndividual> CreateVehicleFilterIndividual(List<FilterIndividual> filtersIndividuals, int prefixId, int userId)
        {
            if (filtersIndividuals.Count == 0)
                return new List<VehicleFilterIndividual>();

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

            List<VehicleFilterIndividual> vehicleFilterIndividuals = ModelAssembler.CreateVehicleFilterIndividuals(filtersIndividuals);

            //foreach (InsuredInfringementCount item in infringments)
            //{
            //    vehicleFilterIndividuals.Find(v => v.InsuredCode == item.InsuredId).InfringementCounts = item.InfringementsCounts;
            //}

            return vehicleFilterIndividuals;
        }

        private void CreateModels(CollectiveEmission collectiveLoad, List<File> files, CompanyPolicy policy, Row policyRow)
        {
            List<FilterIndividual> filterIndividuals = new List<FilterIndividual>();
            CacheListForVehicle cacheListForVehicle = new CacheListForVehicle();
            List<CompanyClause> companyRiskClauses = new List<CompanyClause>();
            List<CompanyClause> companyClauses = new List<CompanyClause>();
            List<Clause> clauses = new List<Clause>();
            List<Clause> riskClauses = new List<Clause>();

            policy = GetMinCurrentFromByFile(files, policy , TemplatePropertyName.RiskDetail);

            if (!policyRow.HasError && files.Exists(x => x.Templates.First(z => z.PropertyName == TemplatePropertyName.RiskDetail).Rows.Any(t => !t.HasError)))
            {
                PendingOperation pendingOperation = new PendingOperation
                {
                    IsMassive = policy.Endorsement.IsMassive.Value,
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
                cacheListForVehicle.VehicleFilterIndividuals = CreateVehicleFilterIndividual(filterIndividuals.ToList(), collectiveLoad.Prefix.Id, collectiveLoad.Prefix.Id);
                cacheListForVehicle.InsuredForScoreList = new List<int>();
                cacheListForVehicle.InsuredForSimitList = new List<int>();

                //Valida sarlaft sobre el tomador
                ValidateSarlaft(filterIndividuals, policy.Holder);
            }

            coverIdAccesoryNoORig = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.NonOriginalAccessories).NumberParameter.Value;
            coverIdAccesoryORig = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.OriginalAccessories).NumberParameter.Value;

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
                CreateModel(collectiveLoad, file, cacheListForVehicle, policy, companyRiskClauses, companyClauses);
            });
        }
        public CompanyPolicy GetMinCurrentFromByFile(List<File> files, CompanyPolicy policy , string templateName)
        {
            DateTime currentDateTime = DateTime.MinValue;

            foreach (var file in files)
            {
                Row row = file.Templates.First(x => x.PropertyName == templateName).Rows.First();
                DateTime currentFromRisk = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));

                if (policy.CurrentFrom <= currentFromRisk && currentFromRisk < policy.CurrentTo)
                {
                    if (currentDateTime == DateTime.MinValue)
                    {
                        currentDateTime = currentFromRisk;
                    }
                    else
                    {
                        if (currentDateTime > currentFromRisk)
                        {
                            currentDateTime = currentFromRisk;
                        }
                    }
                }
            }
            if (currentDateTime != DateTime.MinValue)
                policy.CurrentFrom = currentDateTime;

            return policy;
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

        private void CreateModel(CollectiveEmission collectiveEmission, File file, CacheListForVehicle cacheListForVehicle, CompanyPolicy companyPolicy, List<CompanyClause> companyRiskClauses, List<CompanyClause> companyClauses)
        {
            string propertyName = "";
            CollectiveEmissionRow collectiveEmissionRow = new CollectiveEmissionRow();

            try
            {
                Template templateEmision = file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail);
                templateName = templateEmision.Description;

                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription).Distinct())).ToList();
                collectiveEmissionRow.MassiveLoadId = collectiveEmission.Id;
                collectiveEmissionRow.RowNumber = file.Id;
                collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Validation;
                collectiveEmissionRow.HasError = hasError;
                collectiveEmissionRow.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(),
                    errorList.Where(x => !string.IsNullOrEmpty(x)));
                collectiveEmissionRow.SerializedRow = COMUT.JsonHelper.SerializeObjectToJson(file);

                if (hasError)
                {
                    collectiveEmissionRow.HasError = true;
                    DelegateService.collectiveService.CreateCollectiveEmissionRow(collectiveEmissionRow);
                    DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveEmissionRow.Id);
                    return;
                }
                else
                {
                    DelegateService.collectiveService.CreateCollectiveEmissionRow(collectiveEmissionRow);
                }

                Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows.First();
                RatingZone rating = DelegateService.collectiveService.CreateRatingZone(row, companyPolicy.Prefix.Id);
                List<FilterIndividual> filtersIndividuals = cacheListForVehicle.VehicleFilterIndividuals.Cast<FilterIndividual>().ToList();

                LimitRc limitRc = DelegateService.collectiveService.CreateLimitRc(row, companyPolicy.Prefix.Id, companyPolicy.Product.Id, companyPolicy.PolicyType.Id);

                CompanyVehicle companyVehicle = new CompanyVehicle
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
                            Description = limitRc.Description
                        },
                        Policy = companyPolicy,
                        CurrentFrom = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom)),
                        IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now)
                    },
                    ServiceType = new CompanyServiceType
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskServiceType))
                    },
                    GoodExperienceYear = new VSMO.GoodExperienceYear(),
                };

                CompanyClause companyClause = DelegateService.massiveService.GetClausesObligatory(EmissionLevel.Risk, companyVehicle.Risk.Policy.Prefix.Id, null).Find(x => x.Id != companyVehicle.Risk.Clauses[0].Id);
                if (companyClause != null)
                {
                    companyVehicle.Risk.Clauses.Add(companyClause);
                }
                if (companyPolicy.Product.IsFlatRate)
                {
                    if (string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskRate).Value))
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += Errors.ErrorProductIsFlateRate + KeySettings.ReportErrorSeparatorMessage();
                    }
                }
                companyVehicle.Risk.Beneficiaries.Add(DelegateService.massiveService.CreateBeneficiary(row, companyVehicle.Risk.MainInsured, filtersIndividuals));
                propertyName = CompanyFieldPropertyName.RiskFasecolda;
                string fasecoldaCode = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskFasecolda)).ToString();
                int yearVehicle = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskYear));
                if (!string.IsNullOrEmpty(fasecoldaCode))
                {
                    CompanyVehicle companyVehicleFasecolda = DelegateService.vehicleService.GetVehicleByFasecoldaCode(fasecoldaCode, yearVehicle);
                    if (companyVehicleFasecolda != null)
                    {
                        companyVehicle.Fasecolda = companyVehicleFasecolda.Fasecolda;
                        companyVehicle.Make = companyVehicleFasecolda.Make;
                        companyVehicle.Model = companyVehicleFasecolda.Model;
                        companyVehicle.Version = companyVehicleFasecolda.Version;
                        companyVehicle.StandardVehiclePrice = companyVehicleFasecolda.Price;

                        propertyName = FieldPropertyName.RiskPrice;
                        companyVehicle.Price = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskPrice));

                        if (companyVehicleFasecolda.Price > 0)
                        {
                            if (companyVehicle.Price == 0)
                            {
                                companyVehicle.Price = companyVehicleFasecolda.Price;
                            }
                        }
                        else
                        {
                            collectiveEmissionRow.HasError = true;
                            collectiveEmissionRow.Observations = Errors.ErrorModelNotFound + KeySettings.ReportErrorSeparatorMessage();
                        }

                        propertyName = FieldPropertyName.RiskPrice;
                        companyVehicle.Price = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskPrice));

                        if (companyVehicleFasecolda.Price > 0)
                        {
                            if (companyVehicle.Price == 0)
                            {
                                companyVehicle.Price = companyVehicleFasecolda.Price;
                            }
                        }
                        else
                        {
                            collectiveEmissionRow.HasError = true;
                            collectiveEmissionRow.Observations = Errors.ErrorModelNotFound + KeySettings.ReportErrorSeparatorMessage();
                            DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                            return;
                        }

                        companyVehicle.Version.Body.Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskBody));

                        if (companyVehicle.Version.Body.Id == 0)
                        {
                            if (companyVehicle.Version.Body != null)
                            {
                                if (companyVehicle.Version.Body.Id == DelegateService.commonService.GetParameterByParameterId((int)CompanyParameterType.WithOutBodyVehicle).NumberParameter.GetValueOrDefault())
                                {
                                    collectiveEmissionRow.HasError = true;
                                    collectiveEmissionRow.Observations += Errors.ErrorBodyWithOutBody + KeySettings.ReportErrorSeparatorMessage();
                                }
                            }
                            else
                            {
                                collectiveEmissionRow.HasError = true;
                                collectiveEmissionRow.Observations += Errors.ErrorBodyNotFound + KeySettings.ReportErrorSeparatorMessage();
                            }
                        }
                    }
                    else
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations = Errors.ErrorFasecoldaNotFound + KeySettings.ReportErrorSeparatorMessage();
                    }
                }
                propertyName = FieldPropertyName.RiskYear;
                companyVehicle.Year = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskYear));
                propertyName = FieldPropertyName.RiskUse;
                companyVehicle.Use = new CompanyUse
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskUse))
                };
                propertyName = FieldPropertyName.RiskPrice;

                companyVehicle.OriginalPrice = companyVehicle.Price;
                if (companyVehicle.Make != null && companyVehicle.Model != null && companyVehicle.Version != null)
                {
                    companyVehicle.NewPrice = DelegateService.vehicleService.GetYearsByMakeIdModelIdVersionId(companyVehicle.Make.Id, companyVehicle.Model.Id, companyVehicle.Version.Id).Last().Price;
                    companyVehicle.Version.Fuel = new CompanyFuel()
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskFuel))
                    };
                }
                propertyName = FieldPropertyName.RiskRateTRI;
                companyVehicle.Rate = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRate));
                propertyName = FieldPropertyName.RiskIsNew;
                companyVehicle.IsNew = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskIsNew));
                propertyName = FieldPropertyName.RiskLicensePlate;
                companyVehicle.LicensePlate = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLicensePlate)).ToString();
                propertyName = FieldPropertyName.RiskEngine;
                companyVehicle.EngineSerial = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskEngine)).ToString();
                propertyName = FieldPropertyName.RiskChassis;
                companyVehicle.ChassisSerial = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskChassis)).ToString();
                propertyName = FieldPropertyName.RiskColor;
                companyVehicle.Color = new CompanyColor
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<Int32>(
                        row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskColor))
                };

                propertyName = FieldPropertyName.RiskText;
                companyVehicle.Risk.Text = new CompanyText
                {
                    TextBody = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText)).ToString()
                };
                companyVehicle.Risk.Coverages = DelegateService.collectiveService.CreateCoverages(companyPolicy.Product.Id, companyVehicle.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);
                
                Template templateAdditionalCoverages = file.Templates.Find(p => p.PropertyName == CompanyTemplatePropertyName.AdditionalCoverages);

                if (templateAdditionalCoverages != null)
                {
                    companyVehicle.Risk.Coverages = DelegateService.massiveService.CreateAdditionalCoverages(companyVehicle.Risk.Coverages, companyVehicle.Risk.Coverages.Where(x => x.IsMandatory || x.IsSelected).ToList(), templateAdditionalCoverages.Rows);
                    if (templateAdditionalCoverages.Rows.Any(x => x.HasError))
                    {
                        collectiveEmissionRow.HasError = true;
                        errorList = templateAdditionalCoverages.Rows.Select(t => string.Join(",", t.ErrorDescription)).ToList();
                        collectiveEmissionRow.Observations += string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                    }
                }
                else
                {
                    companyVehicle.Risk.Coverages = companyVehicle.Risk.Coverages.Where(x => x.IsMandatory || x.IsSelected).ToList();
                }

                companyVehicle.Risk.Coverages.ForEach(x =>
                {
                    x.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                    x.CurrentFrom = (DateTime)companyVehicle.Risk.CurrentFrom;
                    x.CurrentTo = companyPolicy.CurrentTo;
                    int compareFromResult = DateTime.Compare(companyPolicy.CurrentFrom, companyPolicy.CurrentTo);
                    if (compareFromResult >= 0)
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations = Errors.ErrorToDatePolicy + KeySettings.ReportErrorSeparatorMessage();
                    }
                });
                propertyName = CompanyFieldPropertyName.BirthDateEldestSon;
                companyVehicle.BirthDateEldestson = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.BirthDateEldestSon));
                propertyName = FieldPropertyName.RiskFuel;

                //Plantillas Adicionales
                //Plantilla Accesoryes
                propertyName = "";
                Template tempalteAccesory = file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Accesories);
                if (tempalteAccesory != null)
                {
                    bool accesoriesNotAllowed = false;
                    List<CompanyCoverage> coveragesAccessories = DelegateService.underwritingService.GetCompanyCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, companyVehicle.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);
                    if (coveragesAccessories != null && companyVehicle?.Risk?.Coverages != null)
                    {
                        if (coverIdAccesoryNoORig != 0 && coverIdAccesoryORig != 0 && companyVehicle.Risk.Coverages.FirstOrDefault(x => x.Id == coverIdAccesoryNoORig || x.Id == coverIdAccesoryORig) == null)
                        {
                            collectiveEmissionRow.HasError = true;
                            accesoriesNotAllowed = true;
                            collectiveEmissionRow.Observations += Errors.ErrorCoverAccesory + " " + companyVehicle.Risk.GroupCoverage.Id + KeySettings.ReportErrorSeparatorMessage();
                        }

                        var coverageDuplicate = companyVehicle.Risk.Coverages.Where(x => coveragesAccessories.Select(y => y.Id).Contains(x.Id)).ToList();
                        var coverageAdd = coveragesAccessories.Where(x => !coverageDuplicate.Select(p => p.Id).Contains(x.Id)).ToList();
                        if (coverageAdd != null && coverageAdd.Any())
                        {
                            companyVehicle.Risk.Coverages.AddRange(coverageAdd);
                        }
                    }
                    if (accesoriesNotAllowed)
                    {
                        string errorScript = string.Empty;
                        List<CompanyAccessory> companyAccessories = DelegateService.massiveService.GetAccesorysByTemplate(tempalteAccesory, companyPolicy, companyVehicle, coverIdAccesoryNoORig, coverIdAccesoryNoORig, ref errorScript);
                        if (string.IsNullOrEmpty(errorScript))
                        {
                            companyVehicle.Accesories = companyAccessories;
                        }
                        else
                        {
                            collectiveEmissionRow.HasError = true;
                            collectiveEmissionRow.Observations += errorScript + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }
                }

                templateName = TemplatePropertyName.Deductible;
                companyVehicle.Risk.Coverages = DelegateService.massiveService.CreateDeductibles(companyVehicle.Risk.Coverages, file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Deductible));
                templateName = TemplatePropertyName.AdditionalBeneficiaries;

                //Template de Beneficiarios.
                Template templateAdditionalBeneficiaries = file.Templates.Find(x => x.PropertyName == TemplatePropertyName.AdditionalBeneficiaries);

                if (templateAdditionalBeneficiaries != null)
                {
                    string errorAdditionalBeneficiaries = string.Empty;
                    List<CompanyBeneficiary> companyBeneficiaries = DelegateService.massiveService.GetBeneficiariesAdditional(file, templateAdditionalBeneficiaries, filtersIndividuals, companyVehicle.Risk.Beneficiaries, ref errorAdditionalBeneficiaries);
                    if (string.IsNullOrEmpty(errorAdditionalBeneficiaries))
                    {
                        companyVehicle.Risk.Beneficiaries = companyBeneficiaries;
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
                    List<CompanyClause> companyClausesTemplate = new List<CompanyClause>();
                    List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();
                    DelegateService.massiveService.GetClausesByTemplate(templateClauses, ref companyClausesTemplate, ref companyCoverages, companyRiskClauses, companyClauses, ref errorClause);
                    if (string.IsNullOrEmpty(errorClause))
                    {
                        if (companyClausesTemplate.Count > 0)
                        {
                            companyVehicle.Risk.Clauses = companyClausesTemplate.Distinct().ToList();
                        }
                        if (companyCoverages.Count > 0)
                        {
                            companyVehicle.Risk.Coverages = companyCoverages;
                        }
                    }
                    else
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += errorClause + KeySettings.ReportErrorSeparatorMessage();
                    }
                }
                //Plantilla de Guiones
                Template templateScript = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.DinamicConcepts);

                if (companyPolicy.Product.CoveredRisk.ScriptId.HasValue && companyPolicy.Product.CoveredRisk.ScriptId > 0)
                {
                    if (templateScript == null)
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += string.Format(Errors.TemplateScriptRequired, Errors.TemplateScript, companyPolicy.Product.Id);
                    }
                }
                if (templateScript != null)
                {
                    string errorScript = string.Empty;
                    List<DynamicConcept> dynamicConcepts = DelegateService.massiveService.GetDynamicConceptsByTemplate(companyPolicy.Product.CoveredRisk.ScriptId, templateScript, ref errorScript);

                    if (string.IsNullOrEmpty(errorScript))
                    {
                        companyVehicle.Risk.DynamicProperties = dynamicConcepts;
                    }
                    else
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += errorScript + KeySettings.ReportErrorSeparatorMessage();
                    }
                }

                templateName = "";
                string pendingOperationJsonIsnotNull = COMUT.JsonHelper.SerializeObjectToJson(companyVehicle);
                PendingOperation pendingOperationRisk = new PendingOperation
                {
                    IsMassive = companyPolicy.Endorsement.IsMassive.Value,
                    ParentId = companyPolicy.Id,
                    UserId = companyPolicy.UserId,
                    Operation = pendingOperationJsonIsnotNull
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

        private List<CompanyAccessory> CreateAccesories(Template template)
        {
            List<CompanyAccessory> accesories = new List<CompanyAccessory>();

            if (template != null)
            {
                templateName = template.Description;
                int count = 1;

                foreach (Row row in template.Rows)
                {
                    row.Number = count;
                    count += 1;

                    CompanyAccessory accessory = new CompanyAccessory();

                    accessory.Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AccesoriesAccessoryId));
                    accessory.IsOriginal = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AccesoriesIsOriginal));

                    if (!accessory.IsOriginal)
                    {
                        accessory.Amount = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AccesoriesPrice));

                        if (accessory.Amount == 0)
                        {
                            row.HasError = true;
                            row.ErrorDescription = Errors.ErrorAccessoryAmount;
                        }
                    }

                    accesories.Add(accessory);
                }
            }

            return accesories;
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