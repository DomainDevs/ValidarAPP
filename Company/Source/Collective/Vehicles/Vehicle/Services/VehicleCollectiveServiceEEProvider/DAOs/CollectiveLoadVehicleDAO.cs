using Newtonsoft.Json;
using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.VehicleCollectiveServices.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.VehicleCollectiveServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
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
using VSMO = Sistran.Company.Application.Vehicles.VehicleServices.Models;
using AUTHMO = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicles.VehicleCollectiveServices.EEProvider.DAOs
{
    using Core.Application.AuthorizationPoliciesServices.Models;
    using Core.Application.UniqueUserServices.Models;
    using Sistran.Company.Application.UnderwritingServices.Assembler;
    using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
    using TypePolicies = Core.Application.AuthorizationPoliciesServices.Enums.TypePolicies;

    public class CollectiveLoadVehicleDAO
    {
        private string templateName = "";
        private static List<Use> uses = new List<Use>();
        //private static List<DocumentType> documentTypes = new List<DocumentType>();
        private static List<Color> colors = new List<Color>();
        private static List<Deductible> deductibles = new List<Deductible>();
        private static List<Accessory> accesoriesList = new List<Accessory>();
        private static List<Core.Application.Vehicles.Models.Type> types = new List<Core.Application.Vehicles.Models.Type>();
        private static List<Currency> currency = new List<Currency>();
        private static List<PaymentMethod> paymentMethod = new List<PaymentMethod>();
        private static List<PolicyType> policiyType = new List<PolicyType>();
        private static int coverIdAccesoryNoORig = 0;
        private static int coverIdAccesoryORig = 0;
        private ConcurrentBag<Row> concurrentRows = new ConcurrentBag<Row>();

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
                File file = collectiveEmission?.File;
                if (file != null)
                {

                    file.Templates.First(x => x.PropertyName == TemplatePropertyName.RiskDetail).IsPrincipal = true;
                    file = DelegateService.utilitiesService.ValidateDataFile(file, collectiveEmission.User.AccountName);
                    Template emissionTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.EmisionAutos);
                    templateName = file?.Templates?.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.EmisionAutos).Description;

                    emissionTemplate = DelegateService.utilitiesService.ValidateDataTemplate(file.Name, collectiveEmission.User.AccountName, emissionTemplate);

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
                    collectiveEmission.Product.Id = (int)DelegateService.utilitiesService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));

                    collectiveEmission.Branch.Id = (int)DelegateService.utilitiesService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));

                    CollectiveVehicleValidationDAO collectiveLoadVehicleValidationDAO = new CollectiveVehicleValidationDAO();
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

                    collectiveEmission.Product = DelegateService.productService.GetProductById(collectiveEmission.Product.Id);
                    if (collectiveEmission.Product != null)
                    {
                        if (!collectiveEmission.Product.IsCollective && !collectiveEmission.Product.IsMassive)
                        {
                            throw new ValidationException(Errors.ErrorProductType);
                        }
                        else if (!collectiveEmission.Product.IsCollective && collectiveEmission.Product.IsMassive)
                        {
                            throw new ValidationException(Errors.ErrorProductIsNotCollective);
                        }
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


                    if (additionalItermediariesTemplate != null)
                    {
                        validatedFiles[0].Templates.Add(additionalItermediariesTemplate);
                    }

                    int policyType = (int)DelegateService.utilitiesService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyType));
                    DateTime currentTo = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentTo));
                    DateTime currentFrom = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
                    int agentCode = (int)DelegateService.utilitiesService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                    int agentType = (int)DelegateService.utilitiesService.GetValueByField<int>(emissionRow.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));

                    collectiveEmission.Agency = DelegateService.underwritingService.GetAgencyByAgentCodeAgentTypeCode(agentCode, agentType);
                    validations = collectiveLoadVehicleValidationDAO.GetValidationsByFiles(validatedFiles, collectiveEmission, collectiveEmission.Product.Id, policyType, currentTo, currentFrom);
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
                CacheListForVehicle cacheListForVehicle = new CacheListForVehicle();
                Row emissionRow = emissionTemplate.Find(t => t.PropertyName == TemplatePropertyName.EmisionAutos).Rows.First();
                if (emissionRow != null && !emissionRow.HasError && files.Exists(x => x.Templates.First(z => z.PropertyName == TemplatePropertyName.RiskDetail).Rows.Any(t => !t.HasError)))
                {
                    files.ForEach(x => x.Templates.Add(emissionTemplate.Find(t => t.PropertyName == TemplatePropertyName.EmisionAutos)));
                    filterIndividuals = DelegateService.massiveService.GetFilterIndividuals(collectiveLoad.User.UserId, collectiveLoad.Branch.Id, files, TemplatePropertyName.EmisionAutos);
                    if (filterIndividuals != null && filterIndividuals.Count > 0)
                    {
                        if (collectiveLoad.IsAutomatic)
                        {
                            File emissionFile = new File
                            {
                                Templates = emissionTemplate
                            };
                            templateName = "";
                            policy = DelegateService.collectiveService.CreateCompanyPolicy(collectiveLoad, emissionFile, TemplatePropertyName.EmisionAutos, filterIndividuals, files.Count);
                            templateName = emissionTemplate.Find(t => t.PropertyName == TemplatePropertyName.EmisionAutos).Description;
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
                    files.ForEach(x => x.Templates.Add(emissionTemplate.Find(t => t.PropertyName == TemplatePropertyName.EmisionAutos)));
                }

                coverIdAccesoryNoORig = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.NonOriginalAccessories).NumberParameter.Value;
                coverIdAccesoryORig = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.OriginalAccessories).NumberParameter.Value;

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

                    LimitRc limitRc = DelegateService.collectiveService.CreateLimitRc(row, policy.Prefix.Id, policy.Product.Id, policy.PolicyType.Id);

                    CompanyVehicle companyVehicle = new CompanyVehicle
                    {
                        Risk = new CompanyRisk
                        {
                            Status = RiskStatusType.Original,
                            CoveredRiskType = CoveredRiskType.Vehicle,
                            MainInsured = DelegateService.massiveService.CreateInsured(row, policy.Holder, filterIndividuals),
                            LimitRc = new CompanyLimitRc
                            {
                                Id = limitRc.Id,
                                LimitSum = limitRc.LimitSum,
                                Description = limitRc.Description
                            },
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
                        ServiceType = new CompanyServiceType
                        {
                            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskServiceType))
                        },
                        GoodExperienceYear = new VSMO.GoodExperienceYear()
                    };

                    companyVehicle.Risk.Beneficiaries.Add(DelegateService.massiveService.CreateBeneficiary(row, companyVehicle.Risk.MainInsured, filterIndividuals));
                    //Agrega Clausulas Obligatorias a nivel de riesgo 
                    companyVehicle.Risk.Clauses = DelegateService.massiveService.GetClausesObligatory(EmissionLevel.Risk, policy.Prefix.Id, null);


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
                                collectiveEmissionRow.Observations += Errors.ErrorModelNotFound + KeySettings.ReportErrorSeparatorMessage();
                            }

                            if (companyVehicle.Version.Type.Id != (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskVehicleType)))
                            {
                                collectiveEmissionRow.HasError = true;
                                collectiveEmissionRow.Observations += Errors.ErrorFasecoldaVehicleType + KeySettings.ReportErrorSeparatorMessage();
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
                            collectiveEmissionRow.Observations += Errors.ErrorFasecoldaNotFound + KeySettings.ReportErrorSeparatorMessage();

                            bool fasecoldaHasLettersOrCharacters = fasecoldaCode.Any(x => !char.IsNumber(x));

                            if (fasecoldaHasLettersOrCharacters)
                            {
                                collectiveEmissionRow.HasError = true;
                                collectiveEmissionRow.Observations += Errors.FasecoldaCodeWithCharacters + KeySettings.ReportErrorSeparatorMessage();
                            }
                        }
                    }

                    companyVehicle.Year = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskYear));

                    companyVehicle.Use = new CompanyUse
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(
                            row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskUse))
                    };

                    companyVehicle.OriginalPrice = companyVehicle.Price;

                    if (companyVehicle.Make != null && companyVehicle.Model != null && companyVehicle.Version != null)
                    {
                        companyVehicle.NewPrice = DelegateService.vehicleService.GetYearsByMakeIdModelIdVersionId(companyVehicle.Make.Id, companyVehicle.Model.Id, companyVehicle.Version.Id).Last().Price;

                        companyVehicle.Version.Fuel = new CompanyFuel
                        {
                            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskFuel))
                        };
                    }
                    companyVehicle.Rate = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRate));
                    companyVehicle.IsNew = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskIsNew));
                    companyVehicle.LicensePlate = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLicensePlate)).ToString().ToUpper();
                    companyVehicle.EngineSerial = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskEngine)).ToString().ToUpper();
                    companyVehicle.ChassisSerial = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskChassis)).ToString().ToUpper();


                    if (policy.Product.IsFlatRate)
                    {
                        if (string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskRate).Value))
                        {
                            collectiveEmissionRow.HasError = true;
                            collectiveEmissionRow.Observations += Errors.ErrorProductIsFlateRate + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }


                    companyVehicle.Color = new CompanyColor
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(
                            row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskColor))
                    };

                    companyVehicle.Risk.Text = new CompanyText
                    {
                        TextBody = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText)).ToString()
                    };

                    companyVehicle.Risk.Coverages = DelegateService.collectiveService.CreateCoverages(policy.Product.Id, companyVehicle.Risk.GroupCoverage.Id, policy.Prefix.Id);
                    if (companyVehicle?.Risk?.Coverages != null && companyVehicle.Risk.Coverages.Any())
                    {
                        companyVehicle.Risk.Coverages.AsParallel().ForAll(x =>
                        {
                            x.EndorsementType = policy.Endorsement.EndorsementType;
                            x.CurrentFrom = riskCurrentFrom;
                            x.CurrentTo = policy.CurrentTo;


                        });
                        companyVehicle.Risk.Coverages[0].Clauses.AddRange(DelegateService.massiveService.GetClausesObligatory(EmissionLevel.Coverage, policy.Prefix.Id, null));

                        // Template Accesorios 
                        Template tempalteAccesory = file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Accesories);
                        if (tempalteAccesory != null)
                        {
                            string errorAccesory = string.Empty;
                            List<CompanyAccessory> companyAccessories = DelegateService.massiveService.GetAccesorysByTemplate(tempalteAccesory, policy, companyVehicle, coverIdAccesoryNoORig, coverIdAccesoryNoORig, ref errorAccesory);
                            if (string.IsNullOrEmpty(errorAccesory))
                            {
                                companyVehicle.Accesories = companyAccessories;
                            }
                            else
                            {
                                collectiveEmissionRow.HasError = true;
                                collectiveEmissionRow.Observations += errorAccesory + KeySettings.ReportErrorSeparatorMessage();
                            }
                        }
                        // Template Coberturas Adicionales
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

                        companyVehicle.Risk.Coverages = DelegateService.massiveService.CreateDeductibles(companyVehicle.Risk.Coverages, file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Deductible));


                        //Template de Beneficiarios.
                        Template templateAdditionalBeneficiaries = file.Templates.Find(x => x.PropertyName == TemplatePropertyName.AdditionalBeneficiaries);

                        if (templateAdditionalBeneficiaries != null)
                        {
                            string errorAdditionalBeneficiaries = string.Empty;
                            List<CompanyBeneficiary> companyBeneficiaries = DelegateService.massiveService.GetBeneficiariesAdditional(file, templateAdditionalBeneficiaries, filterIndividuals, companyVehicle.Risk.Beneficiaries, ref errorAdditionalBeneficiaries);
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
                        //templateName = file.Templates.First(x => x.PropertyName == TemplatePropertyName.EmisionAutos).Description;

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
                                    companyVehicle.Risk.Clauses = companyClauses.Distinct().ToList();
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

                        // Tempalte de Guiones 
                        Template templateScript = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.DinamicConcepts);

                        if (policy.Product.CoveredRisk.ScriptId.HasValue && policy.Product.CoveredRisk.ScriptId > 0)
                        {
                            if (templateScript == null)
                            {
                                collectiveEmissionRow.HasError = true;
                                collectiveEmissionRow.Observations += string.Format(Errors.TemplateScriptRequired, Errors.TemplateScript, policy.Product.Id);
                            }
                        }
                        if (templateScript != null)
                        {
                            string errorScript = string.Empty;
                            List<DynamicConcept> dynamicConcepts = DelegateService.massiveService.GetDynamicConceptsByTemplate(policy.Product.CoveredRisk.ScriptId, templateScript, ref errorScript);

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
                        PendingOperation pendingOperation = new PendingOperation
                        {
                            ParentId = policy.Id,
                            UserId = policy.UserId,
                            IsMassive = companyVehicle.Risk.Policy.PolicyOrigin != PolicyOrigin.Individual ? true : false,
                            Operation = pendingOperationJsonIsnotNull
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

        //private void CheckExternalServices(CacheListForVehicle cacheListForVehicle, CompanyPolicy companyPolicy, CompanyVehicle companyVehicle, CollectiveEmission collectiveLoad, File file, Row row)
        //{
        //    //VehicleFilterIndividual vehicleFilterIndividual = cacheListForVehicle.VehicleFilterIndividuals.Find(x => x.InsuredCode == companyVehicle.Risk.CompanyInsured.InsuredId);
        //    VehicleFilterIndividual vehicleFilterIndividual = new VehicleFilterIndividual();
        //    bool scoreAlreadyQueried = false; bool simitAlreadyQueried = false; bool requireScore = false; bool requireSimit = false; bool requireFasecolda = false;
        //    string licencePlate = string.Empty; string surname = string.Empty;
        //    IdentificationDocument identificationDocument = new IdentificationDocument();

        //    //if (vehicleFilterIndividual.IndividualTypes == IndividualTypes.LegalPerson)
        //    //{
        //    //    surname = vehicleFilterIndividual.Company.Name;
        //    //    identificationDocument = vehicleFilterIndividual.Company.IdentificationDocument;
        //    //}
        //    //else if (vehicleFilterIndividual.IndividualTypes == IndividualTypes.Person)
        //    //{
        //    //    surname = vehicleFilterIndividual.Person.Surname;
        //    //    identificationDocument = vehicleFilterIndividual.Person.IdentificationDocument;
        //    //}

        //    if (companyPolicy.Product.IsScore.GetValueOrDefault() /* && companyVehicle.Risk.CompanyInsured.ScoreCredit == null*/)
        //    {
        //        requireScore = true;

        //        /*
        //                        if (!cacheListForVehicle.InsuredForScoreList.Contains(companyVehicle.Risk.CompanyInsured.InsuredId))
        //                        {
        //                            cacheListForVehicle.InsuredForScoreList.Add(companyVehicle.Risk.CompanyInsured.InsuredId);
        //                        }
        //                        else
        //                        {
        //                            scoreAlreadyQueried = true;
        //                        }*/
        //    }
        //    //if (companyPolicy.Product.IsFine.GetValueOrDefault() && companyVehicle.ListInfringementCount == null)
        //    //{
        //    //    requireSimit = true;
        //    //    if (!cacheListForVehicle.InsuredForSimitList.Contains(companyVehicle.Risk.CompanyInsured.InsuredId))
        //    //    {
        //    //        cacheListForVehicle.InsuredForSimitList.Add(companyVehicle.Risk.CompanyInsured.InsuredId);
        //    //    }
        //    //    else
        //    //    {
        //    //        simitAlreadyQueried = true;
        //    //    }
        //    //}
        //    if (companyPolicy.Product.IsFasecolda.GetValueOrDefault())
        //    {
        //        requireFasecolda = true;
        //        licencePlate = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.RiskLicensePlate));
        //    }

        //    if (requireScore || requireSimit || requireFasecolda)
        //    {
        //        //DelegateService.externalProxyService.CheckExternalServices(identificationDocument, surname, vehicleFilterIndividual.InsuredCode.Value, licencePlate, collectiveLoad.Id, file.Id, (int)SubCoveredRiskType.Vehicle, collectiveLoad.User.UserId, scoreAlreadyQueried, simitAlreadyQueried, requireScore, requireSimit, requireFasecolda);
        //    }
        //}

        /// <summary>
        /// Crear Accesorios
        /// </summary>
        /// <param name="accesoriesTemplate">Plantilla Accesorios</param>
        /// <returns>Accesorios</returns>
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

                    CompanyAccessory accessory = new CompanyAccessory
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AccesoriesAccessoryId)),
                        IsOriginal = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AccesoriesIsOriginal))
                    };

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
            FileProcessValue fileProcessValue = new FileProcessValue
            {
                Key1 = (int)FileProcessType.MassiveReport,
                Key4 = collectiveEmission.Prefix.Id,
                Key5 = (int)subCoveredRiskType
            };

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
                    List<CompanyVehicle> companyVehicles = this.GetCompanyVehicle(collectiveEmission, companyPolicy.Id);
                    companyVehicles = companyVehicles.Where(p => excludedRisks.Any(s => s == p.Risk.Id) == false && p.Risk.Id != 0).ToList();
                    List<int> assistanceCoverageIds = new List<int>();
                    //DelegateService.underwritingService.GetAssistanceCoveragesIds(CompanyParameterType.AssistanceVehicle);
                    List<CompanyCoverage> coverageAsistances = companyVehicles.Where(v => v.Risk.Coverages.Any(x => assistanceCoverageIds.Exists(y => y == x.Id))).Select(v => v.Risk.Coverages.Where(x => assistanceCoverageIds.Exists(y => y == x.Id))).SelectMany(i => i).Distinct().ToList();
                    //var assistancePremium = coverageAsistances.Select(a => a.PremiumAmount).Sum();

                    TP.Parallel.ForEach(companyVehicles, vehicle =>
                    {
                        List<Field> fields = JsonConvert.DeserializeObject<List<Field>>(serializeFields);
                        fields.Find(u => u.PropertyName == FieldPropertyName.Identificator).Value = collectiveEmissionRows.FirstOrDefault(x => x.Risk.Id == vehicle.Risk.Id).RowNumber.ToString();
                        fields = DelegateService.massiveService.FillInsuredFields(fields, vehicle.Risk.MainInsured);
                        if (vehicle.Risk.Status == RiskStatusType.Excluded)
                        {
                            fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = vehicle.Risk.Coverages[0].EndorsementSublimitAmount.ToString();
                        }
                        else
                        {
                            fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = vehicle.Price.ToString();
                        }
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskNumber).Value = vehicle.Risk.Number.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskRatingZoneDescription).Value = vehicle.Risk.RatingZone != null ? vehicle.Risk.RatingZone.Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskVehicleTypeDescription).Value = (types.Count > 0) ? types.FirstOrDefault(u => u.Id == vehicle.Version.Type.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskMakeDescription).Value = vehicle.Make.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskVehicleDescription).Value = vehicle.Model.Description + " " + vehicle.Version.Description;//pendiente
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskColorDescription).Value = colors.FirstOrDefault(u => u.Id == vehicle.Color.Id).Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskModel).Value = vehicle.Year.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskLicensePlate).Value = vehicle.LicensePlate;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskEngineDescription).Value = vehicle.EngineSerial;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskChassisDescription).Value = vehicle.ChassisSerial;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskServiceTypeDescription).Value = (uses.Count > 0 && vehicle.Use.Id > 0) ? uses.FirstOrDefault(u => u.Id == vehicle.Use.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskLimitRcDescription).Value = vehicle.Risk.LimitRc.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskRate).Value = vehicle.Rate.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.AccesoriesDescription).Value = this.CreateAccessories(vehicle.Accesories);
                        fields.Find(u => u.PropertyName == FieldPropertyName.TotalAccesories).Value = vehicle.PriceAccesories.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskPrice).Value = vehicle.Risk.AmountInsured.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.BeneficiaryData).Value = DelegateService.massiveService.CreateBeneficiaries(vehicle.Risk.Beneficiaries);
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(vehicle.Risk.Clauses);
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyCurrencyDescription).Value = companyPolicy.ExchangeRate != null ? currency.FirstOrDefault(l => l.Id == companyPolicy.ExchangeRate.Currency.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.MethodOfPaymentDescription).Value = companyPolicy.Holder.PaymentMethod != null ? paymentMethod.FirstOrDefault(l => l.Id == companyPolicy.Holder.PaymentMethod.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyType).Value = companyPolicy.PolicyType != null && policiyType.Count > 0 ? policiyType.FirstOrDefault(l => l.Id == companyPolicy.PolicyType.Id && l.Prefix.Id == companyPolicy.Prefix.Id).Description : "";

                        if (vehicle.GoodExperienceYear != null)
                        {
                            if (vehicle.GoodExperienceYear.GoodExpNumPrinter < 0)
                            {
                                fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = "0";
                            }
                            else
                            {
                                //if (vehicle.GoodExperienceYear.GoodExpNumPrinter >= goodExpNumPrinter)
                                //{
                                //    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = String.Format("{0} o más", goodExpNumPrinter);
                                //}
                                //else
                                //{
                                //    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = vehicle.GoodExperienceYear.GoodExpNumPrinter.ToString();
                                //}
                            }
                        }
                        //Asistencia 
                        if (vehicle.Risk.Coverages.Any(x => assistanceCoverageIds.Exists(y => y == x.Id)))
                        {
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyPremium).Value =
                                (companyPolicy.Summary.Premium /*- assistancePremium*/).ToString("F2");
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyExpensesWithAssistance).Value =
                                (companyPolicy.Summary.Expenses /*+ assistancePremium*/).ToString("F2");
                        }

                        string serializeFields1 = COMUT.JsonHelper.SerializeObjectToJson(fields);
                        foreach (CompanyCoverage coverage in vehicle.Risk.Coverages.OrderByDescending(u => u.Number))
                        {
                            List<Field> fieldsC = JsonConvert.DeserializeObject<List<Field>>(serializeFields1);

                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageDescription).Value = coverage.Description;
                            if (vehicle.Risk.Status == RiskStatusType.Excluded)
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
                            this.concurrentRows.Add(new Row
                            {
                                Fields = fieldsC,
                                Number = collectiveEmissionRows.FirstOrDefault(x => x.Risk.Id == vehicle.Risk.Id).RowNumber
                            });

                        }
                        if (this.concurrentRows.Count >= bulkExcel || companyVehicles.Count == 0)
                        {
                            file.Templates[0].Rows = this.concurrentRows.ToList().OrderBy(x => x.Number).ToList();
                            file.Name = "Reporte Colectivas Autos_" + key + "_" + collectiveEmission.Id;
                            filePath = DelegateService.utilitiesService.GenerateFile(file);
                            this.concurrentRows = new ConcurrentBag<Row>();
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
        private List<CompanyVehicle> GetCompanyVehicle(CollectiveEmission collectiveEmission, int tempId)
        {
            int endorsementId = collectiveEmission.EndorsementId ?? 0;
            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();
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
                        companyVehicles.Add(JsonConvert.DeserializeObject<CompanyVehicle>(item.Operation));
                    }

                    break;
                case MassiveLoadStatus.Issued:
                    if (Settings.UseReplicatedDatabase())
                    {
                        DelegateService.pendingOperationEntityService.GetRiskByEndorsementDocumentNumber(endorsementId).ForEach(x => companyVehicles.Add(JsonConvert.DeserializeObject<CompanyVehicle>(x)));
                    }
                    else
                    {
                        DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(endorsementId).ForEach(x => companyVehicles.Add(JsonConvert.DeserializeObject<CompanyVehicle>(x)));
                    }
                    break;
            }
            return companyVehicles;

        }
        private string CreateAccessories(List<CompanyAccessory> accessories)
        {
            string value = "";
            if (accessories != null)
            {
                foreach (Accessory accessory in accessories)
                {
                    value += (accesoriesList.Count > 0 ? accesoriesList.FirstOrDefault(u => u.Id == accessory.Id).Description : "") + " " + Convert.ToInt64(accessory.Amount) + " | ";
                }
            }

            return value;
        }

        private void LoadList(CollectiveEmission collectiveEmission)
        {
            if (uses.Count == 0)
            {
                uses = DelegateService.vehicleService.GetUses();
            }

            if (colors.Count == 0)
            {
                colors = DelegateService.vehicleService.GetColors();
            }
            if (types.Count == 0)
            {
                types = DelegateService.vehicleService.GetTypes();
            }
            if (accesoriesList.Count == 0)
            {
                accesoriesList = DelegateService.vehicleService.GetAccessories();
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


        public void SaveTemporalVehicle(string[] objectsToSave)
        {
            CompanyVehicle companyVehicle = new CompanyVehicle();
            CollectiveEmissionRow collectiveEmissionRow = new CollectiveEmissionRow();
            PendingOperation pendingOperationRisk = new PendingOperation();
            PendingOperation policyPendingOperation = new PendingOperation();
            CompanyPolicy companyPolicy = new CompanyPolicy();

            try
            {
                pendingOperationRisk = JsonConvert.DeserializeObject<PendingOperation>(objectsToSave[0].Trim());
                companyVehicle = JsonConvert.DeserializeObject<CompanyVehicle>(pendingOperationRisk.Operation);

                policyPendingOperation = DelegateService.utilitiesService.GetPendingOperationById(companyVehicle.Risk.Policy.Id);
                companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(policyPendingOperation.Operation);

                companyPolicy.Id = companyVehicle.Risk.Policy.Id;
                companyPolicy.Endorsement = companyVehicle.Risk.Policy.Endorsement;
                companyVehicle.Risk.Policy = companyPolicy;

                collectiveEmissionRow = JsonConvert.DeserializeObject<CollectiveEmissionRow>(objectsToSave[1].Trim());
                int hierarchy = int.Parse(objectsToSave[4]);
                List<int> ruleToValidateRisk = JsonConvert.DeserializeObject<List<int>>(objectsToSave[5]);
                List<int> ruleToValidateCoverage = JsonConvert.DeserializeObject<List<int>>(objectsToSave[6]);

                companyVehicle.Risk.InfringementPolicies = DelegateService.vehicleService.ValidateAuthorizationPoliciesMassive(companyVehicle, hierarchy, ruleToValidateRisk, ruleToValidateCoverage);
                DelegateService.vehicleService.CompanySaveCompanyVehicleTemporal(companyVehicle);

                collectiveEmissionRow.Risk.Description = companyVehicle.LicensePlate;
                collectiveEmissionRow.Risk.Id = companyVehicle.Risk.Id;
                collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                collectiveEmissionRow.Premium = companyVehicle.Risk.Premium;

                if (companyVehicle.Risk.InfringementPolicies.Any(x => x.Type == TypePolicies.Restrictive))
                {
                    throw new Exception(string.Format(Errors.PoliciesRestrictive, string.Join("|", companyVehicle.Risk.InfringementPolicies.Where(x => x.Type == TypePolicies.Restrictive).Select(x => x.Message).ToList())));
                }

                collectiveEmissionRow.HasEvents = companyVehicle.Risk.InfringementPolicies.Any(x => x.Type == TypePolicies.Authorization);
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
                        List<PendingOperation> riskPendingOperations = DelegateService.utilitiesService.GetPendingOperationsByParentId(companyVehicle.Risk.Policy.Id);

                        if (riskPendingOperations != null && riskPendingOperations.Any() && collectiveEmission.IsAutomatic)
                        {
                            List<AuthorizationRequest> authorizationRequests = new List<AuthorizationRequest>();
                            List<CompanyRisk> risks = new List<CompanyRisk>();
                            //List<AUTHMO.UserGroupModel> userGroup = new List<AUTHMO.UserGroupModel>();
                            //userGroup = DelegateService.uniqueUserService.GetUsersGroupByUserId(collectiveEmission.User.UserId).Select(x => new AUTHMO.UserGroupModel { UserId = collectiveEmission.User.UserId, GroupId = x.GroupId }).ToList();

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
                                companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(new QuotaFilterDTO { PlanId = companyPolicy.PaymentPlan.Id, CurrentFrom = companyPolicy.CurrentFrom, IssueDate = companyPolicy.IssueDate, ComponentValueDTO = Sistran.Company.Application.Vehicles.VehicleCollectiveServices.EEProvider.Assemblers.ModelAssembler.CreateCompanyComponentValueDTO(companyPolicy.Summary) });
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

                            authorizationRequests.AddRange(DelegateService.collectiveService.ValidateAuthorizationPoliciesPolicy(companyVehicle.Risk.Policy.InfringementPolicies, collectiveEmission, companyVehicle.Risk.Policy.Id));
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