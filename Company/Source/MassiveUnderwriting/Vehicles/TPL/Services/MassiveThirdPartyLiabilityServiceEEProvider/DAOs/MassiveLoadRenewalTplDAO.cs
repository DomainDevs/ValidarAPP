using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Vehicles.MassiveThirdPartyLiabilityServices.EEProvider.Assemblers;
using Sistran.Company.Application.Vehicles.MassiveThirdPartyLiabilityServices.Models;
using Sistran.Company.Application.Vehicles.MassiveTPLServices.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
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
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicles.MassiveThirdPartyLiabilityServices.EEProvider.DAOs
{
    public class MassiveLoadRenewalTplDAO
    {
        string templateName = "";

        #region Métodos para la validación y el cargue 
        /// <summary>
        /// Tarifar Cargue Masivo
        /// </summary>
        /// <param name="massiveLoadId">Id Cargue Masivo</param>
        /// <returns>Cargue Masivo</returns>
        public MassiveRenewal CreateMassiveLoad(MassiveRenewal massiveLoad)
        {
            ValidateFile(massiveLoad);

            massiveLoad.Status = MassiveLoadStatus.Validating;
            massiveLoad = DelegateService.massiveRenewal.CreateMassiveRenewal(massiveLoad);

            try
            {
                if (massiveLoad != null)
                {
                    TP.Task.Run(() => ValidateData(massiveLoad));
                }
            }
            catch (Exception ex)
            {
                massiveLoad.HasError = true;
                massiveLoad.ErrorDescription = string.Format(Errors.ErrorValidatingFile, ex.Message);
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }

            return massiveLoad;
        }

        private void ValidateData(MassiveRenewal massiveRenewal)
        {
            try
            {
                massiveRenewal.File = DelegateService.utilitiesService.ValidateDataFile(massiveRenewal.File, massiveRenewal.User.AccountName);
                massiveRenewal.TotalRows = massiveRenewal.File.Templates.First(x => x.IsPrincipal).Rows.Count();

                DelegateService.massiveRenewal.UpdateMassiveRenewal(massiveRenewal);

                List<File> files = DelegateService.utilitiesService.GetDataTemplates(massiveRenewal.File.Templates);

                var template = massiveRenewal.File.Templates.First(x => x.PropertyName == TemplatePropertyName.Renewal);

                if (!massiveRenewal.File.Templates.Any(t => t.Rows.Any(r => r.HasError)))
                {
                    var consolidatedPolicies =
                                        from p in template.Rows
                                        group p by new
                                        {
                                            DocumentNumber = Convert.ToDecimal(p.Fields.First(x => x.PropertyName == FieldPropertyName.PolicyNumberRenewal).Value),
                                            Branch = Convert.ToDecimal(p.Fields.First(x => x.PropertyName == FieldPropertyName.BranchId).Value),
                                            Prefix = Convert.ToDecimal(p.Fields.First(x => x.PropertyName == FieldPropertyName.PrefixCode).Value)
                                        }
                                        into policies
                                        where policies.Count() > 1
                                        select new
                                        {
                                            DocumentNumber = policies.Key.DocumentNumber,
                                            Branch = policies.Key.Branch,
                                            Prefix = policies.Key.Prefix,
                                            Total = policies.Count()
                                        };

                    ParallelHelper.ForEach(template.Rows.ToList(), (rows) =>
                    {
                        var PolicyNumber = consolidatedPolicies.FirstOrDefault(z => z.DocumentNumber == Convert.ToDecimal(rows.Fields.First(y => y.PropertyName == FieldPropertyName.PolicyNumberRenewal).Value)
                                                && z.Branch == Convert.ToDecimal(rows.Fields.First(y => y.PropertyName == FieldPropertyName.BranchId).Value)
                                                && z.Prefix == Convert.ToDecimal(rows.Fields.First(y => y.PropertyName == FieldPropertyName.PrefixCode).Value))?.DocumentNumber;

                        if (rows.Fields != null && PolicyNumber.HasValue)
                        {
                            rows.HasError = true;
                            rows.ErrorDescription = String.Format("{0}", Errors.ErrorDuplicatePolicies, rows.Fields.First(u => u.PropertyName == FieldPropertyName.PolicyNumberRenewal).Value);
                        }
                    });
                }

                MassiveRenewalTplValidationDAO MassiveRenewalTplValidationDAO = new MassiveRenewalTplValidationDAO();
                List<Validation> validations = MassiveRenewalTplValidationDAO.GetValidationsByFiles(files, massiveRenewal);
                if (validations.Count > 0)
                {
                    Validation validation;
                    foreach (File file in files)
                    {
                        validation = validations.Find(x => x.Id == file.Id);
                        if (validation != null)
                        {
                            file.Templates[0].Rows[0].HasError = true;
                            file.Templates[0].Rows[0].ErrorDescription = validation.ErrorMessage;
                        }
                    }
                }
                CreateModels(massiveRenewal, files);
                DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(massiveRenewal.Id);
            }
            catch (Exception ex)
            {
                massiveRenewal.Status = MassiveLoadStatus.Validated;
                massiveRenewal.HasError = true;
                massiveRenewal.ErrorDescription = ex.Message;
                DelegateService.massiveRenewal.UpdateMassiveRenewal(massiveRenewal);
            }
        }


        /// <summary>
        /// Validar Archivo
        /// </summary>
        /// <param name="massiveLoad">Cargue Masivo</param>
        private void ValidateFile(MassiveRenewal massiveRenewal)
        {
            FileProcessValue fileProcessValue = new FileProcessValue()
            {
                Key1 = (int)FileProcessType.MassiveRenewal,
                Key2 = (int)EndorsementType.Renewal,
                Key4 = massiveRenewal.Prefix.Id,
                Key5 = (int)SubCoveredRiskType.ThirdPartyLiability
            };
            string fileName = massiveRenewal.File.Name;
            massiveRenewal.File = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);

            if (massiveRenewal.File != null)
            {
                massiveRenewal.File.Name = fileName;
                massiveRenewal.File = DelegateService.utilitiesService.ValidateFile(massiveRenewal.File, massiveRenewal.User.AccountName);
            }
            else
            {
                throw new ValidationException(Errors.ErrorFileParametrizationNotFound);
            }
        }

        /// <summary>
        /// Crear Modelos
        /// </summary>
        /// <param name="massiveLoad">Cargue Masivo</param>
        /// <param name="files">Datos Archivo</param>
        private void CreateModels(MassiveRenewal massiveLoad, List<File> files)
        {
            try
            {
                List<FilterIndividual> filtersIndividuals = DelegateService.massiveService.GetDataFilterIndividualRenewal(files, TemplatePropertyName.Renewal);

                CacheListForThirdPartyLiability cacheListForTpl = new CacheListForThirdPartyLiability();

                List<CompanyClause> companyCoverageClauses = new List<CompanyClause>();
                List<CompanyClause> companyRiskClauses = new List<CompanyClause>();
                List<Clause> coverageClauses = new List<Clause>();
                List<Clause> riskClauses = new List<Clause>();

                cacheListForTpl.VehicleFilterIndividuals = CreateVehicleFilterIndividual(filtersIndividuals, massiveLoad.Prefix.Id, massiveLoad.User.UserId);

                //cacheListForTpl.Alliances = DelegateService.underwritingService.GetAlliances();
                cacheListForTpl.InsuredForScoreList = new ConcurrentDictionary<int, int>();
                cacheListForTpl.InsuredForSimitList = new ConcurrentDictionary<int, int>();

                //List<int> packages = DataFacadeManager.GetPackageProcesses(files.Count(), "MaxThreadRenewal");
                //foreach (int package in packages)
                //{
                //    List<File> packageList = files.Take(package).ToList();
                //    files.RemoveRange(0, package);
                //    Parallel.ForEach(packageList,file=> {
                //        CreateModel(massiveLoad, file, cacheListForTpl);
                //    });
                //}
                riskClauses = DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.Risk, (int)CoveredRiskType.Vehicle);
                coverageClauses = DelegateService.underwritingService.GetClausesByEmissionLevel(EmissionLevel.Coverage);

                foreach (var riskClause in riskClauses)
                {
                    companyRiskClauses.Add(MappCompanyClause(riskClause));
                }

                foreach (var clause in coverageClauses)
                {
                    companyCoverageClauses.Add(MappCompanyClause(clause));
                }
                ParallelHelper.ForEach(files, file =>
                {
                    CreateModel(massiveLoad, file, cacheListForTpl, companyRiskClauses, companyCoverageClauses);
                });
            }
            catch (Exception ex)
            {
                massiveLoad.Status = MassiveLoadStatus.Validated;
                massiveLoad.HasError = true;
                massiveLoad.ErrorDescription = ex.Message;
                DelegateService.massiveRenewal.UpdateMassiveRenewal(massiveLoad);
            }
        }

        public List<ThirdPartyLiabilityFilterIndividual> CreateVehicleFilterIndividual(List<FilterIndividual> filtersIndividuals, int prefixId, int userId)
        {
            List<FilterIndividual> individualWithError = new List<FilterIndividual>();

            individualWithError.AddRange(filtersIndividuals.Where(i => !string.IsNullOrEmpty(i.Error)));

            filtersIndividuals.RemoveAll(i => !string.IsNullOrEmpty(i.Error));

            filtersIndividuals.AddRange(individualWithError);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FilterIndividual, ThirdPartyLiabilityFilterIndividual>();

            });
            List<ThirdPartyLiabilityFilterIndividual> vehicleFilterIndividuals = config.CreateMapper().Map<List<FilterIndividual>, List<ThirdPartyLiabilityFilterIndividual>>(filtersIndividuals);

            return vehicleFilterIndividuals;
        }

        /// <summary>
        /// Valida el sarlaft en la row
        /// </summary>
        /// <param name="filterIndividuals"></param>
        /// <param name="row"></param>
        private void ValidateSarlaft(List<ThirdPartyLiabilityFilterIndividual> filterIndividuals, Row row, Holder holder, CompanyIssuanceInsured insured)
        {
            #region holder

            //Validación sarlaft
            FilterIndividual indiv = filterIndividuals.Where(i => (i.IndividualType == IndividualType.Person && i.Person.IndividualId == holder.IndividualId) || (i.IndividualType == IndividualType.Company && i.Company.IndividualId == holder.IndividualId)).FirstOrDefault();

            if (indiv != null && !string.IsNullOrEmpty(indiv.SarlaftError))
            {
                if (indiv.SarlaftError == "ValidateSarlaftExpired")
                {
                    row.HasError = true;
                    row.ErrorDescription += (!string.IsNullOrEmpty(row.ErrorDescription) ? "|" : "") + Errors.ValidateSarlaftExpired;
                }
                else if (indiv.SarlaftError == "ValidateSarlaftOvercome")
                {
                    row.HasError = true;
                    row.ErrorDescription += (!string.IsNullOrEmpty(row.ErrorDescription) ? "|" : "") + Errors.ValidateSarlaftOvercome;
                }
                else if (indiv.SarlaftError == "ValidateSarlaftPending")
                {
                    row.HasError = true;
                    row.ErrorDescription += (!string.IsNullOrEmpty(row.ErrorDescription) ? "|" : "") + Errors.ValidateSarlaftPending;
                }
            }
            else
            {
                row.HasError = true;
                row.ErrorDescription += (!string.IsNullOrEmpty(row.ErrorDescription) ? "|" : "") + Errors.ValidateSarlaftExists;
            }

            #endregion

            #region insured

            if (!(bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(p => p.PropertyName == CompanyFieldPropertyName.InsuredEqualHolder)))
            {
                //Validación sarlaft
                indiv = filterIndividuals.Where(i => (i.IndividualType == IndividualType.Person && i.Person.IndividualId == insured.IndividualId) || (i.IndividualType == IndividualType.Company && i.Company.IndividualId == insured.IndividualId)).FirstOrDefault();

                if (indiv != null && !string.IsNullOrEmpty(indiv.SarlaftError))
                {
                    indiv.SarlaftError += "Insured";

                    if (indiv.SarlaftError == "ValidateSarlaftExpiredInsured")
                    {
                        row.HasError = true;
                        row.ErrorDescription += (!string.IsNullOrEmpty(row.ErrorDescription) ? "|" : "") + Errors.ValidateSarlaftExpiredInsured;
                    }
                    else if (indiv.SarlaftError == "ValidateSarlaftOvercomeInsured")
                    {
                        row.HasError = true;
                        row.ErrorDescription += (!string.IsNullOrEmpty(row.ErrorDescription) ? "|" : "") + Errors.ValidateSarlaftOvercomeInsured;
                    }
                    else if (indiv.SarlaftError == "ValidateSarlaftPendingInsured")
                    {
                        row.HasError = true;
                        row.ErrorDescription += (!string.IsNullOrEmpty(row.ErrorDescription) ? "|" : "") + Errors.ValidateSarlaftPendingInsured;
                    }
                }
                else
                {
                    row.HasError = true;
                    row.ErrorDescription += (!string.IsNullOrEmpty(row.ErrorDescription) ? "|" : "") + Errors.ValidateSarlaftExistsInsured;
                }
            }

            #endregion
        }

        private void CreateModel(MassiveRenewal massiveLoad, File file, CacheListForThirdPartyLiability cacheListForTpl, List<CompanyClause> companyRiskClauses, List<CompanyClause> companyCoverageClauses)
        {
            MassiveRenewalRow massiveRenewalRow = new MassiveRenewalRow();

            try
            {
                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Renewal).Rows.First();
                CompanyPolicy companyPolicy = null;
                List<CompanyTplRisk> companyTplRisks = null;

                if (!hasError)
                {
                    companyPolicy = DelegateService.massiveRenewal.CreateCompanyPolicy(file, TemplatePropertyName.Renewal, massiveLoad.User.UserId, massiveLoad.Prefix.Id);
                    companyTplRisks = DelegateService.tplService.GetCompanyThirdPartyLiabilitiesByPolicyId(companyPolicy.Endorsement.PolicyId);

                    if (companyTplRisks == null || companyTplRisks.Count == 0)
                    {
                        throw new ValidationException(Errors.ErrorRisksNotFound);
                    }

                    //Valida sarlaft
                    ValidateSarlaft(cacheListForTpl.VehicleFilterIndividuals, row, companyPolicy.Holder, companyTplRisks[0].Risk.MainInsured);
                    hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                }

                List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription).Distinct())).ToList();

                massiveRenewalRow.MassiveRenewalId = massiveLoad.Id;
                massiveRenewalRow.RowNumber = file.Id;
                massiveRenewalRow.Status = MassiveLoadProcessStatus.Validation;
                massiveRenewalRow.HasError = hasError;
                massiveRenewalRow.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                massiveRenewalRow.SerializedRow = JsonConvert.SerializeObject(file);
                DelegateService.massiveRenewal.CreateMassiveRenewalRow(massiveRenewalRow);

                if (!hasError)
                {
                    templateName = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Renewal).Description;
                    companyPolicy.IsPersisted = true;
                    massiveRenewalRow.TemporalId = companyPolicy.Id;
                    massiveRenewalRow.Risk = new Risk
                    {
                        Policy = new Policy
                        {
                            Id = companyPolicy.Id
                        }
                    };
                    templateName = Errors.TemplateRenewal;

                    CompanyTplRisk companyTplRiskRisk = companyTplRisks[0];
                    foreach (CompanyBeneficiary beneficiary in companyTplRiskRisk.Risk.Beneficiaries)
                    {
                        ModelAssembler.CreateBeneficiary(beneficiary);
                        //beneficiary.IdentificationDocument = DelegateService.tplService.GetIdentificationDocumentByIndividualIdCustomerType(beneficiary.IndividualId, (int)beneficiary.CustomerType);
                    }
                    int bodyVehicle = companyTplRiskRisk.Version.Body.Id;

                    companyTplRiskRisk.Version = ModelAssembler.CreateCompanyVersion(DelegateService.tplService.GetVersionByVersionIdModelIdMakeId(companyTplRiskRisk.Version.Id, companyTplRiskRisk.Model.Id, companyTplRiskRisk.Make.Id));
                    companyTplRiskRisk.Version.Body.Id = bodyVehicle;
                    //companyTplRiskRisk.ActualDateMovement = DateTime.Now.Date;
                    companyTplRiskRisk.Risk.IsPersisted = true;
                    companyTplRiskRisk.Risk.Status = RiskStatusType.Original;
                    //CompanyTplRisk companyTplRiskFasecolda = DelegateService.tplService.GetVehicleByFasecoldaCode(companyTplRiskRisk.Fasecolda.Description, companyTplRiskRisk.Year);
                    //if (companyTplRiskFasecolda != null)
                    //{
                    //    //companyTplRiskRisk.Price = companyTplRiskFasecolda.Price;
                    //    if (companyTplRiskFasecolda.Price == 0)
                    //    {
                    //        throw new ValidationException(Errors.TheVehicleHasNoInsuredValueForTheModel + companyTplRiskRisk.Year);
                    //    }
                    //}
                    //companyTplRiskRisk.StandardVehiclePrice = companyTplRiskRisk.Price;

                    foreach (CompanyCoverage coverage in companyTplRiskRisk.Risk.Coverages)
                    {
                        // Ajuste temporal se deberían sacar los paquetes de reglas al recuperar las coberturas
                        var coverageInfo = DelegateService.underwritingService.GetCoverageByCoverageIdProductIdGroupCoverageId(coverage.Id, companyPolicy.Product.Id, companyTplRiskRisk.Risk.GroupCoverage.Id);
                        coverage.RuleSetId = coverageInfo.RuleSetId;
                        coverage.PosRuleSetId = coverageInfo.PosRuleSetId;
                        coverage.CurrentFrom = companyPolicy.CurrentFrom;
                        coverage.CurrentTo = companyPolicy.CurrentTo;
                        coverage.CoverStatus = CoverageStatusType.Original;
                        coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                        coverage.AccumulatedPremiumAmount = 0;
                        coverage.FlatRatePorcentage = 0;
                        coverage.IsMandatory = coverageInfo.IsMandatory;
                        coverage.IsSelected = coverageInfo.IsSelected;
                    }

                    List<CompanyCoverage> companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, companyTplRiskRisk.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);

                    companyCoverages.RemoveAll(u => companyTplRiskRisk.Risk.Coverages.Select(z => z.Id).Contains(u.Id));
                    foreach (CompanyCoverage coverage in companyCoverages.Where(u => u.IsSelected))
                    {
                        coverage.CurrentFrom = companyPolicy.CurrentFrom;
                        coverage.CurrentTo = companyPolicy.CurrentTo;
                        coverage.CoverStatus = CoverageStatusType.Original;
                        coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                        companyTplRiskRisk.Risk.Coverages.Add(coverage);
                    }
                    companyTplRiskRisk.Risk.Coverages = companyTplRiskRisk.Risk.Coverages.Where(x => x.IsMandatory || x.IsSelected).ToList();
                    string textRisk = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText));
                    companyTplRiskRisk.Risk.Text = new CompanyText { TextBody = textRisk };
                    companyTplRiskRisk.Risk.Policy = companyPolicy;

                    Template templateAdditionalCoverages = file.Templates.Find(p => p.PropertyName == CompanyTemplatePropertyName.AdditionalCoverages);

                    if (templateAdditionalCoverages != null)
                    {

                        companyTplRiskRisk.Risk.Coverages = DelegateService.massiveService.CreateAdditionalCoverages(companyCoverages, companyTplRiskRisk.Risk.Coverages.Where(x => x.IsMandatory || x.IsSelected).ToList(), templateAdditionalCoverages.Rows);
                        if (templateAdditionalCoverages.Rows.Any(x => x.HasError))
                        {
                            massiveRenewalRow.HasError = true;
                            errorList = templateAdditionalCoverages.Rows.Select(t => string.Join(",", t.ErrorDescription)).ToList();
                            massiveRenewalRow.Observations += string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                        }
                    }
                    else
                    {
                        companyTplRiskRisk.Risk.Coverages = companyTplRiskRisk.Risk.Coverages.Where(x => x.IsMandatory || x.IsSelected).ToList();
                    }

                    //Plantilla Deducibles
                    templateName = Errors.Deductible;
                    Template templeteDeductible = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.Deductible);
                    if (templeteDeductible != null)
                    {
                        companyTplRiskRisk.Risk.Coverages = DelegateService.massiveService.CreateDeductibles(companyTplRiskRisk.Risk.Coverages, templeteDeductible);
                    }

                    Template templateClauses = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.Clauses);
                    companyCoverages.ForEach(p => p.Clauses = DelegateService.massiveService.GetClausesByCoverageId(templateClauses, p.Id));

                    if (templateClauses != null)
                    {
                        string errorClause = string.Empty;
                        List<CompanyClause> companyClause = new List<CompanyClause>();
                        List<CompanyCoverage> companyCoverage = new List<CompanyCoverage>();
                        DelegateService.massiveService.GetClausesByTemplate(templateClauses, ref companyClause, ref companyCoverage, companyRiskClauses, companyCoverageClauses, ref errorClause);
                        if (string.IsNullOrEmpty(errorClause))
                        {
                            if (companyClause.Count > 0)
                            {
                                companyTplRiskRisk.Risk.Clauses = companyClause.Distinct().ToList();
                            }
                            if (companyCoverage.Count > 0)
                            {
                                companyTplRiskRisk.Risk.Coverages = companyCoverages;
                            }
                        }
                        else
                        {
                            massiveRenewalRow.HasError = true;
                            massiveRenewalRow.Observations += errorClause + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }


                    Template templateScript = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.DinamicConcepts);

                    if (templateScript != null)
                    {
                        string errorScript = string.Empty;
                        List<DynamicConcept> dynamicConcepts = DelegateService.massiveService.GetDynamicConceptsByTemplate(companyPolicy.Product.CoveredRisk.ScriptId, templateScript, ref errorScript);

                        if (string.IsNullOrEmpty(errorScript))
                        {
                            companyTplRiskRisk.Risk.DynamicProperties = dynamicConcepts;
                        }
                        else
                        {
                            massiveRenewalRow.HasError = true;
                            massiveRenewalRow.Observations += errorScript + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }

                    templateName = "";

                    PendingOperation policyPendingOperation = new PendingOperation
                    {
                        Operation = JsonConvert.SerializeObject(companyPolicy),
                        UserId = companyPolicy.UserId
                    };


                    PendingOperation pendingOperationRisk = new PendingOperation
                    {
                        UserId = companyPolicy.UserId,
                        OperationName = "Temporal",
                        Operation = JsonConvert.SerializeObject(companyTplRiskRisk)
                    };



                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* Without Replicated Database */
                        policyPendingOperation = DelegateService.utilitiesService.CreatePendingOperation(policyPendingOperation);
                        pendingOperationRisk.ParentId = policyPendingOperation.Id;
                        pendingOperationRisk = DelegateService.utilitiesService.CreatePendingOperation(pendingOperationRisk);
                        massiveRenewalRow.Risk.Id = pendingOperationRisk.Id;
                        DelegateService.massiveRenewal.UpdateMassiveRenewalRow(massiveRenewalRow);
                    }
                    else
                    {
                        /* with Replicated Database */
                        string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(policyPendingOperation), (char)007, JsonConvert.SerializeObject(pendingOperationRisk), (char)007, JsonConvert.SerializeObject(massiveRenewalRow), (char)007, nameof(MassiveRenewalRow));
                        QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "CreatePendingOperationQuee");
                        /* with Replicated Database */
                    }
                }
                else
                {
                    massiveRenewalRow.Status = MassiveLoadProcessStatus.Validated;
                    DelegateService.massiveRenewal.UpdateMassiveRenewalRow(massiveRenewalRow);
                }
            }
            catch (Exception ex)
            {
                massiveRenewalRow.HasError = true;
                if (string.IsNullOrEmpty(templateName))
                {
                    massiveRenewalRow.Observations += Errors.ErrorCreateRisk;
                    EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                }
                else
                {
                    string[] messages = ex.Message.Split('|');
                    massiveRenewalRow.Observations += string.Format(Errors.ErrorInTemplate, templateName, messages[0]);
                }

                DelegateService.massiveRenewal.UpdateMassiveRenewalRow(massiveRenewalRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }


        #endregion

        #region Métodos y propiedades para la generación del reporte
        private static List<Use> uses = new List<Use>();
        private static List<Color> colors = new List<Color>();
        private static List<Deductible> deductibles = new List<Deductible>();
        private static List<Core.Application.Vehicles.Models.Type> types = new List<Core.Application.Vehicles.Models.Type>();
        private static List<Currency> currency = new List<Currency>();
        private static List<LimitRc> limitRc = new List<LimitRc>();
        private static List<RatingZone> ratingZones = new List<RatingZone>();

        ConcurrentBag<Row> concurrentRows = new ConcurrentBag<Row>();



        /// <summary>
        /// Genera el archivo de reporte del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveLoad">massiveLoad</param>
        /// <param name="massiveRenewal"></param>
        /// <returns>string</returns>
        public string GenerateReportToMassiveRenewal(MassiveRenewal massiveRenewal)
        {
            MassiveLoadProcessStatus processStatus = MassiveLoadProcessStatus.Validation;
            switch (massiveRenewal.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    processStatus = MassiveLoadProcessStatus.Events;
                    break;
                case MassiveLoadStatus.Issued:
                    processStatus = MassiveLoadProcessStatus.Finalized;
                    break;
            }
            DelegateService.massiveService.LoadReportCacheList();
            LoadList();
            List<MassiveRenewalRow> massiveLoadProcesses = DelegateService.massiveRenewal.GetMassiveRenewalRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveRenewal.Id, processStatus, false, null);
            SubCoveredRiskType subCoveredRiskType = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveRenewal.Prefix.Id, (int)massiveRenewal.CoveredRiskType);

            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.MassiveReport;
            fileProcessValue.Key4 = massiveRenewal.Prefix.Id;
            fileProcessValue.Key5 = (int)subCoveredRiskType;

            File file = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);
            if (file == null && !massiveLoadProcesses.Any())
            {
                return "";
            }
            //string serializeFields = JsonConvert.SerializeObject(file.Templates[0].Rows[0].Fields);
            //ParallelHelper.ForEach(massiveLoadProcesses, (processes) =>
            //{
            //    FillVehicleFields(massiveRenewal, processes, serializeFields);
            //});
            //file.Templates[0].Rows = concurrentRows.ToList();
            //file.Name = "Reporte Autos";
            //return DelegateService.commonService.GenerateFile(file);
            file.Templates[0].Rows[0].Fields.ForEach(u => u.Value = "");
            string serializeFields = JsonConvert.SerializeObject(file.Templates[0].Rows[0].Fields);
            string key = Guid.NewGuid().ToString();
            string filePath = "";
            List<int> packages = DataFacadeManager.GetPackageProcesses(massiveLoadProcesses.Count(), "MaxThreadMassive");
            int bulkExcel = Convert.ToInt32(DelegateService.commonService.GetKeyApplication("MaxMassiveExcelRow"));

            file.FileType = FileType.CSV;

            foreach (int package in packages)
            {
                List<MassiveRenewalRow> packageMassiveLoad = massiveLoadProcesses.Take(package).ToList();
                massiveLoadProcesses.RemoveRange(0, package);
                TP.Parallel.ForEach(packageMassiveLoad,
                    (process) =>
                    {
                        FillVehicleFields(massiveRenewal, process, serializeFields);
                    });

                if (concurrentRows.Count >= bulkExcel || massiveLoadProcesses.Count == 0)
                {
                    file.Templates[0].Rows = concurrentRows.ToList().OrderBy(x => x.Number).ToList();
                    file.Name = "Reporte Autos_" + key + "_" + massiveRenewal.Id;
                    filePath = DelegateService.utilitiesService.GenerateFile(file);
                    concurrentRows = new ConcurrentBag<Row>();
                }
            }
            return filePath;
        }
        private void FillVehicleFields(MassiveRenewal massiveRenewal, MassiveRenewalRow proccess, string serializeFields)
        {
            try
            {
                Policy policy = new Policy();
                policy = proccess.Risk.Policy;
                if (proccess.Risk.Policy.Endorsement != null)
                {
                    policy.Endorsement = new Endorsement { EndorsementType = EndorsementType.Renewal, Id = proccess.Risk.Policy.Endorsement.Id };
                }
                policy.Id = proccess.TemporalId.GetValueOrDefault();
                policy.Prefix = massiveRenewal.Prefix;
                // int goodExpNumPrinter = DelegateService.tplService.GetGoodExpNumPrinter();

                CompanyPolicy companyPolicy = DelegateService.massiveService.GetCompanyPolicyByMassiveLoadStatusPolicy(massiveRenewal.Status.Value, proccess.Risk.Policy);
                if (companyPolicy == null)
                {
                    return;
                }
                List<Field> fields = DelegateService.massiveService.GetFields(serializeFields, companyPolicy);
                List<CompanyTplRisk> tplRisks = GetCompanyTplRisk(massiveRenewal, proccess, companyPolicy.Id);
                foreach (CompanyTplRisk tplrisk in tplRisks)
                {
                    fields.Find(u => u.PropertyName == FieldPropertyName.Identificator).Value = proccess.RowNumber.ToString();
                    fields.Find(u => u.PropertyName == FieldPropertyName.LoadId).Value = massiveRenewal.Id.ToString();
                    fields = DelegateService.massiveService.FillInsuredFields(fields, tplrisk.Risk.MainInsured);
                    //fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = tplrisk.Price.ToString();

                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskNumber).Value = "1";// tplrisk.Number.ToString();
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskRatingZoneDescription).Value = tplrisk.Risk.RatingZone != null ? ratingZones.FirstOrDefault(r => r.Id == tplrisk.Risk.RatingZone.Id).Description : "";
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskVehicleTypeDescription).Value = (types.Count > 0) ? types.FirstOrDefault(u => u.Id == tplrisk.Version.Type.Id).Description : "";
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskMakeDescription).Value = tplrisk.Make.Description;
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskVehicleDescription).Value = tplrisk.Model.Description + " " + tplrisk.Version.Description;//pendiente
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskColorDescription).Value = colors.First(u => u.Id == tplrisk.Color.Id).Description;
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskModel).Value = tplrisk.Year.ToString();
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskLicensePlate).Value = tplrisk.LicensePlate;
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskEngineDescription).Value = tplrisk.EngineSerial;
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskChassisDescription).Value = tplrisk.ChassisSerial;
                    if (tplrisk.Use != null && tplrisk.Use.Id > 0)
                    {
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskServiceTypeDescription).Value = (uses.Count > 0) ? uses.First(u => u.Id == tplrisk.Use.Id).Description : "";
                    }
                    companyPolicy.PaymentPlan = DelegateService.underwritingService.GetPaymentPlanByPaymentPlanId(companyPolicy.PaymentPlan.Id);
                    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyCurrencyDescription).Value = companyPolicy.ExchangeRate != null ? currency.FirstOrDefault(l => l.Id == companyPolicy.ExchangeRate.Currency.Id).Description : "";
                    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyNumberDays).Value = companyPolicy.Endorsement.EndorsementDays == 0 ? companyPolicy.CurrentTo.Subtract(companyPolicy.CurrentFrom).Days.ToString() : "";
                    fields.Find(u => u.PropertyName == FieldPropertyName.MethodOfPaymentDescription).Value = companyPolicy.PaymentPlan.Description;
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskChassisDescription).Value = tplrisk.ChassisSerial;
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskRate).Value = tplrisk.Rate.ToString();
                    //fields.Find(u => u.PropertyName == FieldPropertyName.AccesoriesDescription).Value = CreateAccessories(tplrisk.Accesories);
                    //fields.Find(u => u.PropertyName == FieldPropertyName.TotalAccesories).Value = tplrisk.PriceAccesories.ToString();
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskPrice).Value = tplrisk.Risk.AmountInsured.ToString();
                    List<Beneficiary> beneficiary = ModelAssembler.CreateBeneficiarys(tplrisk.Risk.Beneficiaries);
                    fields.Find(u => u.PropertyName == FieldPropertyName.BeneficiaryData).Value = DelegateService.massiveService.CreateBeneficiaries(tplrisk.Risk.Beneficiaries);
                    if (tplrisk.Risk.Clauses != null)
                    {
                        List<Clause> clauses = ModelAssembler.CreateClauses(tplrisk.Risk.Clauses);
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(tplrisk.Risk.Clauses);
                    }

                    //if (tplrisk.GoodExperienceYear != null)
                    //{
                    //    if (tplrisk.GoodExperienceYear.GoodExpNumPrinter < 0)
                    //    {
                    //        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = "0";
                    //    }
                    //    //else
                    //    //{
                    //    //    if (tplrisk.GoodExperienceYear.GoodExpNumPrinter >= goodExpNumPrinter)
                    //    //    {
                    //    //        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = String.Format("{0} o más", goodExpNumPrinter);
                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = tplrisk.GoodExperienceYear.GoodExpNumPrinter.ToString();
                    //    //    }
                    //    //}
                    //}

                    //Asistencia
                    CompanyCoverage coverageAsistance = tplrisk.Risk.Coverages.FirstOrDefault(u => u.Id == 9);
                    if (coverageAsistance != null)
                    {
                        var assistancePremium = coverageAsistance.PremiumAmount;
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyPremium).Value =
                            (companyPolicy.Summary.Premium - assistancePremium).ToString("F2");
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyExpensesWithAssistance).Value =
                            (companyPolicy.Summary.Expenses + assistancePremium).ToString("F2");
                    }

                    serializeFields = JsonConvert.SerializeObject(fields);
                    foreach (CompanyCoverage coverage in tplrisk.Risk.Coverages.OrderByDescending(u => u.Number))
                    {
                        List<Field> fieldsC = JsonConvert.DeserializeObject<List<Field>>(serializeFields);

                        fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageDescription).Value = coverage.Description;
                        fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageInsuredValue).Value = coverage.EndorsementSublimitAmount.ToString();
                        fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageRate).Value = coverage.Rate.ToString();
                        fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoveragePremium).Value = coverage.PremiumAmount.ToString();
                        fieldsC.Find(u => u.PropertyName == FieldPropertyName.DeductibleDescription).Value = (coverage.Deductible != null && coverage.Deductible.Description != null) ? coverage.Deductible.Description : "";
                        List<Clause> clauses = ModelAssembler.CreateClauses(coverage.Clauses);
                        fieldsC.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(tplrisk.Risk.Clauses);
                        concurrentRows.Add(new Row
                        {
                            Number = proccess.RowNumber,
                            Fields = fieldsC
                        });

                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }



        private List<CompanyTplRisk> GetCompanyTplRisk(MassiveRenewal massiveRenewal, MassiveRenewalRow proccess, int tempId)
        {
            List<CompanyTplRisk> companyTplRisks = new List<CompanyTplRisk>();
            switch (massiveRenewal.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    List<PendingOperation> pendingOperations = new List<PendingOperation>();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* Without Replicated Database */
                        pendingOperations = DelegateService.utilitiesService.GetPendingOperationsByParentId(proccess.Risk.Policy.Id);
                        /* Without Replicated Database */
                    }
                    else
                    {
                        /* with Replicated Database */
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(proccess.Risk.Policy.Id);
                        /* with Replicated Database */
                    }

                    foreach (PendingOperation item in pendingOperations)
                    {
                        companyTplRisks.Add(JsonConvert.DeserializeObject<CompanyTplRisk>(item.Operation));
                    }

                    break;
                case MassiveLoadStatus.Issued:
                    var policy = proccess.Risk.Policy;
                    if (policy.Endorsement != null && policy.Endorsement.EndorsementType.HasValue)
                    {
                        //companyTplRisks = DelegateService.tplService.GetVehiclesByPrefixBranchDocumentNumberEndorsementType(massiveRenewal.Prefix.Id, policy.Branch.Id, policy.DocumentNumber, policy.Endorsement.EndorsementType.Value);
                        if (!Settings.UseReplicatedDatabase())
                        {
                            /* without Replicated Database */
                            DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(policy.Endorsement.Id).ForEach(x => companyTplRisks.Add(JsonConvert.DeserializeObject<CompanyTplRisk>(x)));
                        }
                        else
                        {
                            DelegateService.pendingOperationEntityService.GetRiskByEndorsementDocumentNumber(policy.Endorsement.Id).ForEach(x => companyTplRisks.Add(JsonConvert.DeserializeObject<CompanyTplRisk>(x)));
                        }
                    }
                    break;
            }
            return companyTplRisks;

        }


        private void LoadList()
        {
            if (uses.Count == 0)
            {
                uses = new List<Use>(); //DelegateService.tplService.GetUses();
            }

            if (colors.Count == 0)
            {
                colors = DelegateService.tplService.GetColors();
            }
            if (types.Count == 0)
            {
                types = DelegateService.tplService.GetTypes();
            }

            List<DocumentType> documentTypes = (List<DocumentType>)DelegateService.massiveService.GetCacheList("documentTypes_", "Report");
            deductibles = (List<Deductible>)DelegateService.massiveService.GetCacheList("deductibles_", "Report");
            currency = DelegateService.commonService.GetCurrencies();
            limitRc = DelegateService.underwritingService.GetLimitsRc();
            ratingZones = DelegateService.underwritingService.GetRatingZones();
        }
        #endregion

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