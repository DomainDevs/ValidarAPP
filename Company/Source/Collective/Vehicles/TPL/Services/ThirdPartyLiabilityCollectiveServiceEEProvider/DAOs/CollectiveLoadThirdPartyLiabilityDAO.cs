using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Company.Application.Vehicles.TPLCollectiveServices.EEProvider.Assemblers;
using Sistran.Company.Application.Vehicles.TPLCollectiveServices.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.TPLCollectiveServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Framework;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using VEMO = Sistran.Core.Application.Vehicles.Models;
using AUTHMO = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicles.TPLCollectiveServices.EEProvider.DAOs
{
    using Core.Application.AuthorizationPoliciesServices.Models;
    using Core.Application.UniqueUserServices.Models;
    using Sistran.Company.Application.Vehicles.VehicleServices.Models;
    using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
    using TypePolicies = Core.Application.AuthorizationPoliciesServices.Enums.TypePolicies;
    public class CollectiveLoadThirdPartyLiabilityDAO
    {
        string templateName = "";
        private static List<Use> uses = new List<Use>();
        //private static List<DocumentType> documentTypes = new List<DocumentType>();
        private static List<Color> colors = new List<Color>();
        private static List<Deductible> deductibles = new List<Deductible>();
        private static List<Core.Application.Vehicles.Models.Type> types = new List<Core.Application.Vehicles.Models.Type>();
        private static List<Currency> currency = new List<Currency>();
        private static List<PaymentMethod> paymentMethod = new List<PaymentMethod>();
        private static List<PolicyType> policiyType = new List<PolicyType>();

        ConcurrentBag<Row> concurrentRows = new ConcurrentBag<Row>();

        public CollectiveEmission CreateCollectiveLoad(CollectiveEmission collectiveLoad)
        {
            ValidateFile(collectiveLoad);
            collectiveLoad.Status = MassiveLoadStatus.Validating;
            collectiveLoad = DelegateService.collectiveService.CreateCollectiveEmission(collectiveLoad);
            if (collectiveLoad == null)
            {
                return null;
            }
            TP.Task.Run(() => ValidateData(collectiveLoad));
            return collectiveLoad;
        }

        private void ValidateData(CollectiveEmission collectiveEmission)
        {
            try
            {
                var file = collectiveEmission?.File;
                if (file != null)
                {
                    file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).IsPrincipal = true;
                    file = DelegateService.utilitiesService.ValidateDataFile(file, collectiveEmission.User.AccountName);
                    Template emissionTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.EmissionThirdPartyLiability);
                    templateName = file?.Templates?.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.EmissionThirdPartyLiability).Description;

                    emissionTemplate = DelegateService.utilitiesService.ValidateDataTemplate(file.Name, collectiveEmission.User.AccountName, emissionTemplate);

                    if (emissionTemplate.Rows[0].HasError)
                    {
                        collectiveEmission.Status = MassiveLoadStatus.Validated;
                        collectiveEmission.HasError = true;

                        string formatedError = emissionTemplate.Rows[0].ErrorDescription.Replace("|", ",");

                        int maxLength = formatedError.Length;
                        if (maxLength > 300)
                        {
                            formatedError = formatedError.Substring(0, 300) + "...";
                        }

                        if (string.IsNullOrEmpty(templateName))
                        {
                            collectiveEmission.ErrorDescription += $"{Errors.ErrorCreatingLoad} {formatedError}";
                        }
                        else
                        {
                            collectiveEmission.ErrorDescription += string.Format(Errors.ErrorInTemplate, templateName, formatedError);
                        }
                        DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                        return;
                    }

                    Row emissionRow = emissionTemplate.Rows.First();
                    CollectiveThirdPartyLiabilityValidationDAO collectiveLoadVehicleValidationDAO = new CollectiveThirdPartyLiabilityValidationDAO();
                    List<Validation> validations = collectiveLoadVehicleValidationDAO.GetValidationsByEmissionTemplate(file, emissionRow, collectiveEmission);

                    string emissionFormatedError = string.Empty;
                    if (validations != null && validations.Count > 0)
                    {
                        Validation validation = validations.Find(x => x.Id == file.Id);
                        emissionFormatedError = validation.ErrorMessage.Replace("|", ",");
                    }
                    if (emissionRow.ErrorDescription != null)
                    {
                        if (emissionFormatedError != string.Empty)
                        {
                            emissionFormatedError += "," + emissionRow.ErrorDescription;
                        }
                        else
                        {
                            emissionFormatedError += emissionRow.ErrorDescription;
                        }
                    }
                    if (emissionFormatedError != string.Empty)
                    {
                        collectiveEmission.Status = MassiveLoadStatus.Validated;
                        collectiveEmission.HasError = true;

                        int maxLength = emissionFormatedError.Length;
                        if (maxLength > 300)
                        {
                            emissionFormatedError = emissionFormatedError.Substring(0, 300) + "...";
                        }

                        if (string.IsNullOrEmpty(templateName))
                        {
                            collectiveEmission.ErrorDescription += $"{Errors.ErrorCreateMassiveLoad} {emissionFormatedError}";
                        }
                        else
                        {
                            collectiveEmission.ErrorDescription += string.Format(Errors.ErrorInTemplate, templateName, emissionFormatedError);
                        }
                        DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                        return;
                    }

                    Template additionalItermediariesTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.AdditionalIntermediaries);
                    Template coInsurancedAceptedTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.CoinsuranceAccepted);
                    Template coInsurancedAssignedTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.CoinsuranceAssigned);
                    Template companyAcceptCoInsuranceAgentTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == CompanyTemplatePropertyName.CoinsuranceAcceptedAgency);
                    Template templateClauses = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.Clauses);
                    file.Templates.Remove(emissionTemplate);

                    collectiveEmission.TotalRows = file.Templates.First(p => p.IsPrincipal).Rows.Count;
                    DelegateService.massiveService.UpdateMassiveLoad(collectiveEmission);
                    collectiveEmission.File.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows = DelegateService.massiveService.GetMassivePlatesValidation(collectiveEmission.File.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows);
                    List<File> validatedFiles = DelegateService.utilitiesService.GetDataTemplates(file.Templates);
                    if (coInsurancedAceptedTemplate != null)
                    {
                        validatedFiles[0].Templates.Add(coInsurancedAceptedTemplate);
                    }

                    if (additionalItermediariesTemplate != null)
                    {
                        validatedFiles[0].Templates.Add(additionalItermediariesTemplate);
                    }

                    int productId = (int)DelegateService.utilitiesService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
                    int policyType = (int)DelegateService.utilitiesService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyType));
                    long correlativePolicy = (long)DelegateService.utilitiesService.GetValueByField<long>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCoCorrelativePolicyNumber));
                    int branchId = (int)DelegateService.utilitiesService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                    DateTime currentTo = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentTo));
                    DateTime currentFrom = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
                    collectiveEmission.Branch.Id = branchId;
                    collectiveEmission.Product = DelegateService.productService.GetProductById(productId);
                    if (collectiveEmission.Product != null)
                    {
                        if (!collectiveEmission.Product.IsCollective)
                        {
                            throw new ValidationException(Errors.ErrorProductIsNotCollective);
                        }
                    }
                    int agentCode = (int)DelegateService.utilitiesService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                    int agentType = (int)DelegateService.utilitiesService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                    collectiveEmission.Agency = DelegateService.underwritingService.GetAgencyByAgentCodeAgentTypeCode(agentCode, agentType);
                    if (collectiveEmission.Agency == null)
                    {
                        throw new ValidationException(Errors.ErrorAgent);
                    }
                    validations = collectiveLoadVehicleValidationDAO.GetValidationsByFiles(validatedFiles, collectiveEmission, productId, policyType, correlativePolicy, currentTo, currentFrom, emissionRow);
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
                    if (companyAcceptCoInsuranceAgentTemplate != null)
                    {
                        lstTemplatesPolicy.Add(companyAcceptCoInsuranceAgentTemplate);
                    }
                    if (templateClauses != null)
                    {
                        lstTemplatesPolicy.Add(templateClauses);
                    }
                    CreateModels(collectiveEmission, validatedFiles, lstTemplatesPolicy);
                    DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveEmission.Id);
                }
                else
                {
                    throw new Exception(Errors.ErrorFileNotExist);
                }
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

        private void ValidateFile(CollectiveEmission collectiveLoad)
        {
            FileProcessValue fileProcessValue = new FileProcessValue()
            {
                Key1 = (int)FileProcessType.CollectiveEmission,
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

        private void CreateModels(CollectiveEmission collectiveLoad, List<File> files, List<Template> emissionTemplate)
        {
            if (collectiveLoad != null && files != null && emissionTemplate != null)
            {
                List<FilterIndividual> filterIndividuals = new List<FilterIndividual>();
                List<CompanyClause> companyRiskClauses = new List<CompanyClause>();
                List<CompanyClause> companyClauses = new List<CompanyClause>();
                List<Clause> clauses = new List<Clause>();

                List<Clause> riskClauses = new List<Clause>();

                CompanyPolicy policy = new CompanyPolicy();
                CacheListForThirdPartyLiability cacheListForVehicle = new CacheListForThirdPartyLiability();
                Row emissionRow = emissionTemplate.Find(t => t.PropertyName == TemplatePropertyName.EmissionThirdPartyLiability).Rows.First();
                if (emissionRow != null && !emissionRow.HasError && files.Exists(x => x.Templates.First(z => z.PropertyName == TemplatePropertyName.RiskDetail).Rows.Any(t => !t.HasError)))
                {
                    files.ForEach(x => x.Templates.Add(emissionTemplate.Find(t => t.PropertyName == TemplatePropertyName.EmissionThirdPartyLiability)));
                    filterIndividuals = DelegateService.massiveService.GetFilterIndividuals(collectiveLoad.User.UserId, collectiveLoad.Branch.Id, files, TemplatePropertyName.EmissionThirdPartyLiability);
                    if (filterIndividuals != null && filterIndividuals.Count > 0)
                    {
                        if (collectiveLoad.IsAutomatic)
                        {
                            var emissionFile = new File
                            {
                                Templates = emissionTemplate
                            };
                            templateName = "";
                            policy = DelegateService.collectiveService.CreateCompanyPolicy(collectiveLoad, emissionFile, TemplatePropertyName.EmissionThirdPartyLiability, filterIndividuals, files.Count);
                            templateName = emissionTemplate.Find(t => t.PropertyName == TemplatePropertyName.EmissionThirdPartyLiability).Description;
                            collectiveLoad.TemporalId = policy.Id;

                            //Validación sarlaft
                            FilterIndividual indiv = filterIndividuals.Where(i => (i.IndividualType == IndividualType.Person && i.Person.IndividualId == policy.Holder.IndividualId) || (i.IndividualType == IndividualType.Company && i.Company.IndividualId == policy.Holder.IndividualId)).FirstOrDefault();

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
                                pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(collectiveLoad.TemporalId);
                            }
                            policy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                        }

                        policy.Endorsement.TicketDate = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.TicketDate));
                        policy.Endorsement.TicketNumber = (int)DelegateService.utilitiesService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.TicketNumber));
                    }
                    policy.Summary = new CompanySummary
                    {
                        RiskCount = files.Count
                    };

                    riskClauses = DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.Risk, (int)CoveredRiskType.Vehicle);
                    clauses = DelegateService.underwritingService.GetClausesByEmissionLevel(EmissionLevel.Coverage);

                    foreach (Clause riskClause in riskClauses)
                    {
                        companyRiskClauses.Add(this.MappCompanyClause(riskClause));
                    }

                    foreach (Clause clause in clauses)
                    {
                        companyClauses.Add(MappCompanyClause(clause));
                    }
                }
                else
                {
                    files.ForEach(x => x.Templates.Add(emissionTemplate.Find(t => t.PropertyName == TemplatePropertyName.EmissionThirdPartyLiability)));
                }

                ParallelHelper.ForEach(files, file =>
                {
                    CreateModel(collectiveLoad, file, filterIndividuals, policy, companyRiskClauses, companyClauses);
                });
            }
        }


        private void CreateModel(CollectiveEmission collectiveLoad, File file, List<FilterIndividual> filterIndividuals, CompanyPolicy policy, List<CompanyClause> riskClauses, List<CompanyClause> coverageClauses)
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
                collectiveEmissionRow.MassiveLoadId = collectiveLoad.Id;
                collectiveEmissionRow.RowNumber = file.Id;
                collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Validation;
                collectiveEmissionRow.HasError = hasError;
                collectiveEmissionRow.HasEvents = false;
                collectiveEmissionRow.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                collectiveEmissionRow.SerializedRow = COMUT.JsonHelper.SerializeObjectToJson(file);
                DelegateService.collectiveService.CreateCollectiveEmissionRow(collectiveEmissionRow);

                if (!hasError)
                {
                    Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows.First();
                    templateName = file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).Description;

                    DateTime riskCurrentFrom = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
                    if (riskCurrentFrom != default(DateTime))
                    {
                        bool riskCurrentFromIsInPolicyVigency = policy.CurrentFrom <= riskCurrentFrom && policy.CurrentTo > riskCurrentFrom;
                        if (!riskCurrentFromIsInPolicyVigency)
                        {
                            collectiveEmissionRow.HasError = true;
                            collectiveEmissionRow.Observations += Errors.ErrorRiskCurrentFromDateOutOfPolicyRange + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }
                    else
                    {
                        riskCurrentFrom = policy.CurrentFrom;
                    }
                    int compareFromResult = DateTime.Compare(policy.CurrentFrom, policy.CurrentTo);
                    if (compareFromResult >= 0)
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += Errors.ErrorToDatePolicy + KeySettings.ReportErrorSeparatorMessage();
                    }
                    RatingZone rating = DelegateService.collectiveService.CreateRatingZone(row, policy.Prefix.Id);


                    CompanyTplRisk companyTplRisk = new CompanyTplRisk
                    {
                        Risk = new CompanyRisk
                        {
                            Status = RiskStatusType.Original,
                            CoveredRiskType = CoveredRiskType.Vehicle,
                            MainInsured = DelegateService.massiveService.CreateInsured(row, policy.Holder, filterIndividuals),
                            GroupCoverage = DelegateService.collectiveService.CreateGroupCoverage(row, policy.Product.Id),
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
                            Policy = policy,
                            Beneficiaries = new List<CompanyBeneficiary>()
                        },
                        Deductible = new CompanyDeductible(),
                        ServiceType = new CompanyServiceType
                        {
                            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskServiceType))
                        }
                    };
                    DelegateService.underwritingService.GetLimitsRcByPrefixIdProductIdPolicyTypeId(policy.Prefix.Id, policy.Product.Id, policy.PolicyType.Id);
                    companyTplRisk.Risk.Beneficiaries.Add(DelegateService.massiveService.CreateBeneficiary(row, companyTplRisk.Risk.MainInsured, filterIndividuals));

                    string fasecoldaCode = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskFasecolda)).ToString();
                    int yearVehicle = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskYear));
                    companyTplRisk.Year = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskYear));

                    companyTplRisk.Make = new CompanyMake();
                    companyTplRisk.Make.Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskMakeCode));
                    companyTplRisk.Version = new CompanyVersion();
                    companyTplRisk.Model = new CompanyModel();
                    companyTplRisk.Shuttle = new CompanyShuttle();
                    companyTplRisk.Shuttle.Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskShuttleCode));

                    companyTplRisk.RePoweredVehicle = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskRepotentiatedVehicle));
                    if (companyTplRisk.RePoweredVehicle)
                    {
                        if (string.IsNullOrEmpty(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RepoweringYear).Value))
                        {
                            collectiveEmissionRow.HasError = true;
                            collectiveEmissionRow.Observations += Errors.ErrorYearRePowered + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }
                    companyTplRisk.Use = new VehicleServices.Models.CompanyUse
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(
                            row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskUse))
                    };
                    if (companyTplRisk.Make.Id > 0)
                    {
                        VEMO.Version version = new VEMO.Version();

                        if (companyTplRisk.Year.ToString().Length == 4)
                        {
                            version = DelegateService.tplService.GetVersionsByMakeIdYear(companyTplRisk.Make.Id, companyTplRisk.Year);


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
                        else
                        {
                            collectiveEmissionRow.HasError = true;
                            collectiveEmissionRow.Observations += Errors.ErrorYearFormat;
                        }
                    }

                    companyTplRisk.Rate = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRate));
                    if (companyTplRisk.Rate > 100)
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += "Tasa no puede ser mayor a 100";
                    }

                    companyTplRisk.Risk.IsFacultative = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIsFacultative));
                    companyTplRisk.Risk.IsRetention = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskOnehundredRetention));

                    if (companyTplRisk.Risk.IsFacultative == true && companyTplRisk.Risk.IsRetention == true)
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations += string.Format(Errors.ValidateExcludingFacultativeFields, row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIsFacultative).Description, row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskOnehundredRetention).Description);
                    }
                    companyTplRisk.Rate = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRate));
                    companyTplRisk.IsNew = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskIsNew));
                    companyTplRisk.LicensePlate = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLicensePlate)).ToString().ToUpper();
                    companyTplRisk.EngineSerial = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskEngine)).ToString().ToUpper();
                    companyTplRisk.ChassisSerial = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskChassis)).ToString().ToUpper();


                    if (policy.Product.IsFlatRate)
                    {
                        if (string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskRate).Value))
                        {
                            collectiveEmissionRow.HasError = true;
                            collectiveEmissionRow.Observations += Errors.ErrorProductIsFlateRate + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }


                    companyTplRisk.Color = new CompanyColor
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<Int32>(
                            row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskColor))
                    };

                    companyTplRisk.Risk.Text = new CompanyText
                    {
                        TextBody = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText)).ToString()
                    };

                    companyTplRisk.Risk.Coverages = DelegateService.collectiveService.CreateCoverages(policy.Product.Id, companyTplRisk.Risk.GroupCoverage.Id, policy.Prefix.Id);
                    if (companyTplRisk?.Risk?.Coverages != null && companyTplRisk.Risk.Coverages.Any())
                    {
                        companyTplRisk.Risk.Coverages.AsParallel().ForAll(x =>
                        {
                            x.EndorsementType = policy.Endorsement.EndorsementType;
                            x.CurrentFrom = riskCurrentFrom;
                            x.CurrentTo = policy.CurrentTo;
                        });

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
                        else
                        {
                            companyTplRisk.Risk.Coverages = companyTplRisk.Risk.Coverages.Where(x => x.IsMandatory || x.IsSelected).ToList();
                        }

                        companyTplRisk.Risk.Coverages = DelegateService.massiveService.CreateDeductibles(companyTplRisk.Risk.Coverages, file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Deductible));

                        Template templateAdditionalBeneficiaries = file.Templates.Find(x => x.PropertyName == TemplatePropertyName.AdditionalBeneficiaries);

                        if (templateAdditionalBeneficiaries != null)
                        {
                            string errorAdditionalBeneficiaries = string.Empty;
                            List<CompanyBeneficiary> companyBeneficiaries = DelegateService.massiveService.GetBeneficiariesAdditional(file, templateAdditionalBeneficiaries, filterIndividuals, companyTplRisk.Risk.Beneficiaries, ref errorAdditionalBeneficiaries);
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
                            List<CompanyClause> companyClauses = new List<CompanyClause>();
                            List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();
                            DelegateService.massiveService.GetClausesByTemplate(templateClauses, ref companyClauses, ref companyCoverages, riskClauses, coverageClauses, ref errorClause);
                            if (string.IsNullOrEmpty(errorClause))
                            {
                                if (companyClauses.Count > 0)
                                {
                                    companyTplRisk.Risk.Clauses = companyClauses.Distinct().ToList();
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
                            List<DynamicConcept> dynamicConcepts = DelegateService.massiveService.GetDynamicConceptsByTemplate(policy.Product.CoveredRisk.ScriptId, templateScript, ref errorScript);

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
                        string pedingOperationJsoinIsnotNull = COMUT.JsonHelper.SerializeObjectToJson(companyTplRisk);
                        PendingOperation pendingOperation = new PendingOperation
                        {
                            ParentId = policy.Id,
                            UserId = policy.UserId,
                            IsMassive = companyTplRisk.Risk.Policy.PolicyOrigin != PolicyOrigin.Collective ? true : false,
                            Operation = pedingOperationJsoinIsnotNull
                        };
                        if (Settings.UseReplicatedDatabase())
                        {
                            string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}", COMUT.JsonHelper.SerializeObjectToJson(pendingOperation), (char)007, COMUT.JsonHelper.SerializeObjectToJson(collectiveEmissionRow), (char)007, nameof(CollectiveEmissionRow));
                            QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "CreatePendingOperationQuee");
                        }
                        else
                        {
                            pendingOperation = DelegateService.utilitiesService.CreatePendingOperation(pendingOperation);
                            collectiveEmissionRow.Risk.RiskId = pendingOperation.Id;
                            DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                        }
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorCoveragesNotFound);
                    }
                }
                else
                {
                    DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveLoad.Id);
                    return;
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

        #region Reportes
        /// <summary>
        /// Genera el archivo de reporte del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveLoad">massiveLoad</param>
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
            this.LoadList(collectiveEmission);
            SubCoveredRiskType subCoveredRiskType = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(collectiveEmission.Prefix.Id, (int)collectiveEmission.CoveredRiskType);
            List<CollectiveEmissionRow> collectiveEmissionRows = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadIdCollectiveLoadProcessStatus(collectiveEmission.Id, null, null, null);
            List<int> excludedRisk = collectiveEmissionRows.Where(p => p.HasError == true).Select(p => p.Risk.RiskId).ToList();
            collectiveEmissionRows = collectiveEmissionRows.Where(p => p.HasError != true && p.Status == processStatus).ToList();
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.MassiveReport;
            fileProcessValue.Key4 = collectiveEmission.Prefix.Id;
            fileProcessValue.Key5 = (int)subCoveredRiskType;

            File file = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);
            if (file == null && !collectiveEmissionRows.Any())
            {
                return "";
            }

            file.Templates[0].Rows[0].Fields.ForEach(u => u.Value = "");
            string serializeFields = COMUT.JsonHelper.SerializeObjectToJson(file.Templates[0].Rows[0].Fields);


            file.FileType = FileType.CSV;

            Policy policy = new Policy
            {
                Id = collectiveEmission.TemporalId,
                Endorsement = new Endorsement { EndorsementType = (EndorsementType)endorsementType, Id = collectiveEmission.EndorsementId ?? 0 },
                DocumentNumber = collectiveEmission.DocumentNumber != null ? collectiveEmission.DocumentNumber.GetValueOrDefault() : 0
            };

            CompanyPolicy companyPolicy = DelegateService.massiveService.GetCompanyPolicyByMassiveLoadStatusPolicy(collectiveEmission.Status.Value, policy);
            List<Field> fields = DelegateService.massiveService.GetFields(serializeFields, companyPolicy);
            fields.Find(u => u.PropertyName == FieldPropertyName.LoadId).Value = collectiveEmission.Id.ToString();
            serializeFields = COMUT.JsonHelper.SerializeObjectToJson(fields);

            return this.FillVehicleFields(file, collectiveEmission, collectiveEmissionRows, serializeFields, excludedRisk, companyPolicy);

        }

        private string FillVehicleFields(File file, CollectiveEmission collectiveEmission, List<CollectiveEmissionRow> collectiveEmissionRows, string serializeFields, List<int> excludedRisks, CompanyPolicy companyPolicy)
        {
            string filePath = "";
            try
            {
                string key = Guid.NewGuid().ToString();
                int bulkExcel = Convert.ToInt32(DelegateService.commonService.GetKeyApplication("MaxMassiveExcelRow"));
                //int goodExpNumPrinter = DelegateService.vehicleService.GetGoodExpNumPrinter();
                if (companyPolicy != null)
                {
                    List<CompanyTplRisk> companyTplRisks = GetCompanyVehicle(collectiveEmission, companyPolicy.Id);
                    companyTplRisks = companyTplRisks.Where(p => excludedRisks.Any(s => s == p.Risk.Id) == false).ToList();
                    List<int> assistanceCoverageIds = new List<int>();
                    //DelegateService.underwritingService.GetAssistanceCoveragesIds(CompanyParameterType.AssistanceVehicle);
                    List<CompanyCoverage> coverageAsistances = companyTplRisks.Where(v => v.Risk.Coverages.Any(x => assistanceCoverageIds.Exists(y => y == x.Id))).Select(v => v.Risk.Coverages.Where(x => assistanceCoverageIds.Exists(y => y == x.Id))).SelectMany(i => i).Distinct().ToList();
                    //var assistancePremium = coverageAsistances.Select(a => a.PremiumAmount).Sum();

                    TP.Parallel.ForEach(companyTplRisks, companyTplRisk =>
                    {
                        List<Field> fields = JsonConvert.DeserializeObject<List<Field>>(serializeFields);
                        fields.Find(u => u.PropertyName == FieldPropertyName.Identificator).Value = collectiveEmissionRows.FirstOrDefault(x => x.Risk.Id == companyTplRisk.Risk.Id).RowNumber.ToString();
                        fields = DelegateService.massiveService.FillInsuredFields(fields, companyTplRisk.Risk.MainInsured);
                        if (companyTplRisk.Risk.Status == RiskStatusType.Excluded)
                        {
                            fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = companyTplRisk.Risk.Coverages[0].EndorsementSublimitAmount.ToString();
                        }
                        else
                        {
                            //fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = companyTplRisk.Price.ToString();
                        }
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskNumber).Value = companyTplRisk.Risk.Number.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskRatingZoneDescription).Value = companyTplRisk.Risk.RatingZone != null ? companyTplRisk.Risk.RatingZone.Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskVehicleTypeDescription).Value = (types.Count > 0) ? types.FirstOrDefault(u => u.Id == companyTplRisk.Version.Type.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskMakeDescription).Value = companyTplRisk.Make.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskVehicleDescription).Value = companyTplRisk.Model.Description + " " + companyTplRisk.Version.Description;//pendiente
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskColorDescription).Value = colors.FirstOrDefault(u => u.Id == companyTplRisk.Color.Id).Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskModel).Value = companyTplRisk.Year.ToString();
                        //fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskFasecolda).Value = companyTplRisk.Fasecolda.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskLicensePlate).Value = companyTplRisk.LicensePlate;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskEngineDescription).Value = companyTplRisk.EngineSerial;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskChassisDescription).Value = companyTplRisk.ChassisSerial;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskServiceTypeDescription).Value = (uses.Count > 0 && companyTplRisk.Use.Id > 0) ? uses.FirstOrDefault(u => u.Id == companyTplRisk.Use.Id).Description : "";
                        //fields.Find(u => u.PropertyName == FieldPropertyName.RiskLimitRcDescription).Value = companyTplRisk.Risk.LimitRc.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskRate).Value = companyTplRisk.Rate.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.AccesoriesDescription).Value = CreateAccessories(companyTplRisk.Accesories);
                        //fields.Find(u => u.PropertyName == FieldPropertyName.TotalAccesories).Value = companyTplRisk.PriceAccesories.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskPrice).Value = companyTplRisk.Risk.AmountInsured.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.BeneficiaryData).Value = DelegateService.massiveService.CreateBeneficiaries(companyTplRisk.Risk.Beneficiaries);
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(companyTplRisk.Risk.Clauses);
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyCurrencyDescription).Value = companyPolicy.ExchangeRate != null ? currency.FirstOrDefault(l => l.Id == companyPolicy.ExchangeRate.Currency.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.MethodOfPaymentDescription).Value = companyPolicy.PaymentPlan != null ? paymentMethod.FirstOrDefault(l => l.Id == companyPolicy.PaymentPlan.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyType).Value = companyPolicy.PolicyType != null && policiyType.Count > 0 ? policiyType.FirstOrDefault(l => l.Id == companyPolicy.PolicyType.Id && l.Prefix.Id == companyPolicy.Prefix.Id).Description : "";

                        //if (companyTplRisk.GoodExperienceYear != null)
                        //{
                        //    if (companyTplRisk.GoodExperienceYear.GoodExpNumPrinter < 0)
                        //    {
                        //        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = "0";
                        //    }
                        //    else
                        //    {
                        //        //if (companyTplRisk.GoodExperienceYear.GoodExpNumPrinter >= goodExpNumPrinter)
                        //        //{
                        //        //    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = String.Format("{0} o más", goodExpNumPrinter);
                        //        //}
                        //        //else
                        //        //{
                        //        //    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = companyTplRisk.GoodExperienceYear.GoodExpNumPrinter.ToString();
                        //        //}
                        //    }
                        //}
                        //Asistencia 
                        if (companyTplRisk.Risk.Coverages.Any(x => assistanceCoverageIds.Exists(y => y == x.Id)))
                        {
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyPremium).Value =
                                (companyPolicy.Summary.Premium /*- assistancePremium*/).ToString("F2");
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyExpensesWithAssistance).Value =
                                (companyPolicy.Summary.Expenses /*+ assistancePremium*/).ToString("F2");
                        }

                        string serializeFields1 = COMUT.JsonHelper.SerializeObjectToJson(fields);
                        foreach (CompanyCoverage coverage in companyTplRisk.Risk.Coverages.OrderByDescending(u => u.Number))
                        {
                            List<Field> fieldsC = JsonConvert.DeserializeObject<List<Field>>(serializeFields1);

                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageDescription).Value = coverage.Description;
                            if (companyTplRisk.Risk.Status == RiskStatusType.Excluded)
                            {
                                fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageInsuredValue).Value = coverage.EndorsementSublimitAmount.ToString();
                            }
                            else
                            {
                                fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageInsuredValue).Value = coverage.DeclaredAmount.ToString();
                            }
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageRate).Value = coverage.Rate.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoveragePremium).Value = coverage.PremiumAmount.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.DeductibleDescription).Value = (coverage.Deductible != null && coverage.Deductible.Description != null) ? coverage.Deductible.Description : "";
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(coverage.Clauses);

                            //Fecha de vigencia del riesgo
                            //fieldsC.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskCurrentFrom).Value = coverage.CurrentFrom.ToString();
                            concurrentRows.Add(new Row
                            {
                                Fields = fieldsC,
                                Number = collectiveEmissionRows.FirstOrDefault(x => x.Risk.Id == companyTplRisk.Risk.Id).RowNumber
                            });

                        }
                        if (concurrentRows.Count >= bulkExcel || companyTplRisks.Count == 0)
                        {
                            file.Templates[0].Rows = concurrentRows.ToList().OrderBy(x => x.Number).ToList();
                            file.Name = "Reporte Colectivas Autos_" + key + "_" + collectiveEmission.Id;
                            filePath = DelegateService.utilitiesService.GenerateFile(file);
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
        private List<CompanyTplRisk> GetCompanyVehicle(CollectiveEmission collectiveEmission, int tempId)
        {
            int endorsementId = collectiveEmission.EndorsementId ?? 0;
            List<CompanyTplRisk> companyTplRisks = new List<CompanyTplRisk>();
            switch (collectiveEmission.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    List<PendingOperation> pendingOperations = new List<PendingOperation>();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperations = DelegateService.utilitiesService.GetPendingOperationsByParentId(tempId);
                    }
                    else
                    {
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(tempId);
                    }
                    foreach (PendingOperation item in pendingOperations)
                    {
                        companyTplRisks.Add(JsonConvert.DeserializeObject<CompanyTplRisk>(item.Operation));
                    }

                    break;
                case MassiveLoadStatus.Issued:
                    if (Settings.UseReplicatedDatabase())
                    {
                        DelegateService.pendingOperationEntityService.GetRiskByEndorsementDocumentNumber(endorsementId).ForEach(x => companyTplRisks.Add(JsonConvert.DeserializeObject<CompanyTplRisk>(x)));
                    }
                    else
                    {
                        DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(endorsementId).ForEach(x => companyTplRisks.Add(JsonConvert.DeserializeObject<CompanyTplRisk>(x)));
                    }
                    break;
            }
            return companyTplRisks;

        }


        private void LoadList(CollectiveEmission collectiveEmission)
        {
            if (uses.Count == 0)
            {
                uses = new List<Use>();//DelegateService.vehicleService.GetUses();
            }

            if (colors.Count == 0)
            {
                colors = DelegateService.tplService.GetColors();
            }
            if (types.Count == 0)
            {
                types = DelegateService.tplService.GetTypes();
            }

            //documentTypes = (List<DocumentType>)DelegateService.massiveService.GetCacheList("documentTypes_", "Report");
            deductibles = (List<Deductible>)DelegateService.massiveService.GetCacheList("deductibles_", "Report");
            currency = DelegateService.commonService.GetCurrencies();
            paymentMethod = DelegateService.commonService.GetPaymentMethods();
            policiyType = DelegateService.underwritingService.GetPolicyTypeAll();
        }


        private CompanyClause MappCompanyClause(Clause clause)
        {
            CompanyClause companyClause = new CompanyClause
            {
                Id = clause.Id,
                IsMandatory = clause.IsMandatory,
                Name = clause.Name,
                Text = clause.Text,
                Title = clause.Title
            };
            return companyClause;
        }
        #endregion

        public void SaveTemporalTpl(string[] objectsToSave)
        {
            CompanyTplRisk companyTpl = new CompanyTplRisk();
            CollectiveEmissionRow collectiveEmissionRow = new CollectiveEmissionRow();
            PendingOperation pendingOperationRisk = new PendingOperation();
            PendingOperation policyPendingOperation = new PendingOperation();
            CompanyPolicy companyPolicy = new CompanyPolicy();

            try
            {
                pendingOperationRisk = JsonConvert.DeserializeObject<PendingOperation>(objectsToSave[0].Trim());
                companyTpl = JsonConvert.DeserializeObject<CompanyTplRisk>(pendingOperationRisk.Operation);

                policyPendingOperation = DelegateService.utilitiesService.GetPendingOperationById(companyTpl.Risk.Policy.Id);
                companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(policyPendingOperation.Operation);

                companyPolicy.Id = companyTpl.Risk.Policy.Id;
                companyPolicy.Endorsement = companyTpl.Risk.Policy.Endorsement;
                companyTpl.Risk.Policy = companyPolicy;

                collectiveEmissionRow = JsonConvert.DeserializeObject<CollectiveEmissionRow>(objectsToSave[1].Trim());
                int hierarchy = int.Parse(objectsToSave[4]);
                List<int> ruleToValidateRisk = JsonConvert.DeserializeObject<List<int>>(objectsToSave[5]);
                List<int> ruleToValidateCoverage = JsonConvert.DeserializeObject<List<int>>(objectsToSave[6]);

                companyTpl.Risk.InfringementPolicies = DelegateService.tplService.ValidateAuthorizationPoliciesMassive(companyTpl, hierarchy, ruleToValidateRisk, ruleToValidateCoverage);
                //Validar Metodo
                DelegateService.tplService.CreateThirdPartyLiabilityTemporal(companyTpl, false);

                collectiveEmissionRow.Risk.Description = companyTpl.LicensePlate;
                collectiveEmissionRow.Risk.Id = companyTpl.Risk.Id;
                collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                collectiveEmissionRow.Premium = companyTpl.Risk.Premium;

                if (companyTpl.Risk.InfringementPolicies.Any(x => x.Type == TypePolicies.Restrictive))
                {
                    throw new Exception(string.Format(Errors.PoliciesRestrictive, string.Join("|", companyTpl.Risk.InfringementPolicies.Where(x => x.Type == TypePolicies.Restrictive).Select(x => x.Message).ToList())));
                }

                collectiveEmissionRow.HasEvents = companyTpl.Risk.InfringementPolicies.Any(x => x.Type == TypePolicies.Authorization);
            }
            catch (Exception e)
            {
                collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                collectiveEmissionRow.HasError = true;
                collectiveEmissionRow.Observations = e.Message;
            }
            finally
            {
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                if (DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveEmissionRow.MassiveLoadId, false))
                {
                    CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveEmissionRow.MassiveLoadId, false, false, false);

                    try
                    {
                        List<PendingOperation> riskPendingOperations = DelegateService.utilitiesService.GetPendingOperationsByParentId(companyTpl.Risk.Policy.Id);

                        if (riskPendingOperations != null && riskPendingOperations.Any() && collectiveEmission.IsAutomatic)
                        {
                            List<AuthorizationRequest> authorizationRequests = new List<AuthorizationRequest>();
                            List<CompanyRisk> risks = new List<CompanyRisk>();

                            foreach (PendingOperation risk in riskPendingOperations)
                            {
                                CompanyVehicle vehicle = JsonConvert.DeserializeObject<CompanyVehicle>(risk.Operation);
                                risks.Add(vehicle.Risk);
                                authorizationRequests.AddRange(DelegateService.collectiveService.ValidateAuthorizationPoliciesRisk(vehicle.Risk.InfringementPolicies, collectiveEmission, companyPolicy.Id, vehicle.Risk.Id));
                            }
                            /*VALIDACIÓN TOTALRISK EXCLUSION INCLUSIÓN*/
                            if (collectiveEmission.LoadType.Id == (int)SubMassiveProcessType.Exclusion || collectiveEmission.LoadType.Id == (int)SubMassiveProcessType.Inclusion)
                            {
                                if (collectiveEmission.LoadType.Id == (int)SubMassiveProcessType.Exclusion)
                                {
                                    int totalRiskInExclude = risks.Count;
                                    int ResultTotalRisk = TotalRiskPolicyId(companyPolicy.Endorsement.PolicyId, totalRiskInExclude);
                                    if (ResultTotalRisk > 0)
                                    {
                                        companyPolicy.TotalRisk = ResultTotalRisk;
                                    }
                                }
                                else
                                {
                                    int totalRiskInInclude = risks.Count;
                                    int ResultTotalRisk = TotalRiskPolicyIdInclution(companyPolicy.Endorsement.PolicyId, totalRiskInInclude);
                                    if (ResultTotalRisk > 0)
                                    {
                                        companyPolicy.TotalRisk = ResultTotalRisk;
                                    }
                                }

                            }
                            /***********/
                            /*EXCLUSION*/
                            if (collectiveEmission.LoadType.Id == (int)SubMassiveProcessType.Exclusion)
                            {
                                //Número de Días
                                companyPolicy.Endorsement.EndorsementDays = companyPolicy.CurrentTo.Subtract(companyPolicy.CurrentFrom).Days;

                                companyPolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponentsByCompanyPolicy(companyPolicy, risks);
                                companyPolicy.Summary = DelegateService.underwritingService.CalculateSummaryByCompanyPolicy(companyPolicy, risks);
                                companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(new QuotaFilterDTO { PlanId = companyPolicy.PaymentPlan.Id, CurrentFrom = companyPolicy.CurrentFrom, IssueDate = companyPolicy.IssueDate, ComponentValueDTO= ModelAssembler.CreateCompanyComponentValueDTO(companyPolicy.Summary) });
                                companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissionsByCompanyPolicy(companyPolicy, risks);
                            }
                            else
                            {
                                companyPolicy = DelegateService.underwritingService.CalculatePolicyAmounts(companyPolicy, risks);
                            }
                            var userId = JsonConvert.DeserializeObject<User>(objectsToSave[3].Trim()).UserId;

                            companyPolicy.User = new CompanyPolicyUser { UserId = userId };
                            companyPolicy.UserId = userId;

                            companyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyPolicy);

                            policyPendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                            policyPendingOperation.UserId = userId;
                            DelegateService.utilitiesService.UpdatePendingOperation(policyPendingOperation);

                            companyPolicy = DelegateService.underwritingService.SaveTableTemporal(companyPolicy);
                            collectiveEmission.Premium = companyPolicy.Summary.FullPremium;
                            collectiveEmission.Commiss = companyPolicy.Summary.FullPremium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;

                            if (companyPolicy.InfringementPolicies.Any(x => x.Type == TypePolicies.Restrictive))
                            {
                                throw new Exception(string.Format(Errors.PoliciesRestrictive + "</br>", string.Join("</br>", companyPolicy.InfringementPolicies.Where(x => x.Type == TypePolicies.Restrictive).Select(x => "*" + x.Message).ToList())));
                            }

                            collectiveEmission.HasEvents = companyPolicy.InfringementPolicies.Any(x => x.Type == TypePolicies.Authorization);
                            collectiveEmission.User = JsonConvert.DeserializeObject<User>(objectsToSave[3].Trim());
                            collectiveEmission.Id = int.Parse(objectsToSave[2].Trim());

                            collectiveEmission.Status = MassiveLoadStatus.Tariffed;
                            DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);

                            authorizationRequests.AddRange(DelegateService.collectiveService.ValidateAuthorizationPoliciesPolicy(companyTpl.Risk.Policy.InfringementPolicies, collectiveEmission, companyTpl.Risk.Policy.Id));
                            if (authorizationRequests.Any())
                            {
                                DelegateService.AuthorizationPoliciesService.CreateMassiveAutorizationRequest(authorizationRequests);
                            }
                        }
                        else
                        {
                            collectiveEmission.Status = MassiveLoadStatus.Tariffed;
                            DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                        }
                    }
                    catch (Exception e)
                    {
                        collectiveEmission.HasError = true;
                        collectiveEmission.ErrorDescription = e.Message;
                        DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                    }
                }
                DataFacadeManager.Dispose();
            }
        }
        private int TotalRiskPolicyId(int policyId, int totalriskExclude)
        {
            int totalRiskInPolicyId = DelegateService.underwritingService.GetCurrentRiskNumByPolicyId(policyId);
            int totalRisk = totalRiskInPolicyId - totalriskExclude;
            return totalRisk;
        }

        private int TotalRiskPolicyIdInclution(int policyId, int totalriskInclude)
        {
            int totalRiskInPolicyId = DelegateService.underwritingService.GetCurrentRiskNumByPolicyId(policyId);
            int totalRisk = totalRiskInPolicyId + totalriskInclude;
            return totalRisk;
        }
    }
}