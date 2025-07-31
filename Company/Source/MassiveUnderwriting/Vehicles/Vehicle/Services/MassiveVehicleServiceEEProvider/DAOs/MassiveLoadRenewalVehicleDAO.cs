using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using System.Threading.Tasks;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Newtonsoft.Json;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.UnderwritingServices.Models;
using AutoMapper;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Framework;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider.Resources;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Vehicles.MassiveVehicleServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using System.Diagnostics;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider.Assemblers;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Company.Application.CommonServices.Enums;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Company.Application.Vehicles.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider.DAOs
{
    public class MassiveLoadRenewalVehicleDAO
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

                MassiveRenewalVehicleValidationDAO massiveRenewalVehicleValidationDAO = new MassiveRenewalVehicleValidationDAO();
                List<Validation> validations = massiveRenewalVehicleValidationDAO.GetValidationsByFiles(files, massiveRenewal);
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
                Key5 = (int)SubCoveredRiskType.Vehicle
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

                List<Clause> riskClauses = DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.Risk, (int)CoveredRiskType.Vehicle);
                List<Clause> coverageClauses = DelegateService.underwritingService.GetClausesByEmissionLevel(EmissionLevel.Coverage);

                List<CompanyClause> companyRiskClauses = new List<CompanyClause>();
                List<CompanyClause> companyClauses = new List<CompanyClause>();

                foreach (var riskClause in riskClauses)
                {
                    companyRiskClauses.Add(MappCompanyClause(riskClause));
                }

                foreach (var clause in coverageClauses)
                {
                    companyClauses.Add(MappCompanyClause(clause));
                }

                CacheListForVehicle cacheListForVehicle = new CacheListForVehicle();

                cacheListForVehicle.VehicleFilterIndividuals = CreateVehicleFilterIndividual(filtersIndividuals, massiveLoad.Prefix.Id, massiveLoad.User.UserId);

                cacheListForVehicle.InsuredForScoreList = new ConcurrentDictionary<int, int>();
                cacheListForVehicle.InsuredForSimitList = new ConcurrentDictionary<int, int>();

                ParallelHelper.ForEach(files, file =>
                {
                    CreateModel(massiveLoad, file, cacheListForVehicle, companyRiskClauses, companyClauses);
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

        public List<VehicleFilterIndividual> CreateVehicleFilterIndividual(List<FilterIndividual> filtersIndividuals, int prefixId, int userId)
        {
            List<FilterIndividual> individualWithError = new List<FilterIndividual>();

            individualWithError.AddRange(filtersIndividuals.Where(i => !string.IsNullOrEmpty(i.Error)));

            filtersIndividuals.RemoveAll(i => !string.IsNullOrEmpty(i.Error));

            filtersIndividuals.AddRange(individualWithError);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FilterIndividual, VehicleFilterIndividual>();

            });
            List<VehicleFilterIndividual> vehicleFilterIndividuals = config.CreateMapper().Map<List<FilterIndividual>, List<VehicleFilterIndividual>>(filtersIndividuals);

            return vehicleFilterIndividuals;
        }

        /// <summary>
        /// Valida el sarlaft en la row
        /// </summary>
        /// <param name="filterIndividuals"></param>
        /// <param name="row"></param>
        private void ValidateSarlaft(List<VehicleFilterIndividual> filterIndividuals, Row row, Holder holder, CompanyIssuanceInsured insured, ref string errorSarlaft)
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
            if (row.HasError)
            {
                errorSarlaft = row.ErrorDescription;
            }

            #endregion
        }

        private void CreateModel(MassiveRenewal massiveLoad, File file, CacheListForVehicle cacheListForVehicle, List<CompanyClause> riskClauses, List<CompanyClause> coverageClauses)
        {
            MassiveRenewalRow massiveRenewalRow = new MassiveRenewalRow();

            try
            {
                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Renewal).Rows.First();
                CompanyPolicy companyPolicy = null;
                List<CompanyVehicle> companyVehicles = null;
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
                    companyPolicy = DelegateService.massiveRenewal.CreateCompanyPolicy(file, TemplatePropertyName.Renewal, massiveLoad.User.UserId, massiveLoad.Prefix.Id);

                    //Cantidad maxima de dias permitidos 
                    int maxDaysRenewPolicy = DelegateService.commonService.GetExtendedParameterByParameterId((int)CompanyParameterType.MaxDaysRenewPolicy).NumberParameter.GetValueOrDefault();
                    DateTime dateRenovation = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentTo));
                    TimeSpan differenceDays = dateRenovation - companyPolicy.CurrentFrom;

                    if ((int)differenceDays.TotalDays > maxDaysRenewPolicy)
                    {
                        throw new ValidationException(string.Format(Errors.ErrorMaxDaysRenewPolicy, maxDaysRenewPolicy));
                    }
                    companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(companyPolicy.Endorsement.PolicyId);

                    if (companyVehicles == null || companyVehicles.Count == 0)
                    {
                        throw new ValidationException(Errors.ErrorRisksNotFound);
                    }

                    string errorSarlfat = string.Empty;
                    //Valida sarlaft
                    ValidateSarlaft(cacheListForVehicle.VehicleFilterIndividuals, row, companyPolicy.Holder, companyVehicles[0].Risk.MainInsured, ref errorSarlfat);
                    if (!string.IsNullOrEmpty(errorSarlfat))
                    {
                        massiveRenewalRow.HasError = true;
                        massiveRenewalRow.Observations += row.ErrorDescription + KeySettings.ReportErrorSeparatorMessage();
                    }
                    hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));

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

                    CompanyVehicle companyVehicleRisk = companyVehicles[0];

                    foreach (CompanyBeneficiary beneficiary in companyVehicleRisk.Risk.Beneficiaries)
                    {
                        ModelAssembler.CreateBeneficiary(beneficiary);
                        beneficiary.IdentificationDocument = DelegateService.vehicleService.GetIdentificationDocumentByIndividualIdCustomerType(beneficiary.IndividualId, (int)beneficiary.CustomerType);
                    }



                    //Validar Tomador activo en el sistema
                    FilterIndividual filterIndividualHolder = null;
                    filterIndividualHolder = validateActiveIndividual(companyPolicy.Holder, null, null);

                    if (filterIndividualHolder != null && filterIndividualHolder.DeclinedDate != null)
                    {
                        massiveRenewalRow.HasError = true;
                        massiveRenewalRow.Observations += Errors.InactiveTaker + KeySettings.ReportErrorSeparatorMessage();
                        filterIndividualHolder = null;
                    }


                    //Validar Asegurado activo en el sistema
                    FilterIndividual filterIndividualMainInsured = null;
                    filterIndividualMainInsured = validateActiveIndividual(null, companyVehicleRisk.Risk.MainInsured, null);

                    if (filterIndividualMainInsured != null && filterIndividualMainInsured.DeclinedDate != null)
                    {
                        massiveRenewalRow.HasError = true;
                        massiveRenewalRow.Observations += Errors.MainInsuredInactive + KeySettings.ReportErrorSeparatorMessage();
                        filterIndividualMainInsured = null;
                    }

                    //Validar beneficiarios activos en el sistema
                    if (companyVehicleRisk.Risk.Beneficiaries != null && companyVehicleRisk.Risk.Beneficiaries.Count > 0)
                    {
                        foreach (CompanyBeneficiary companyBeneficiary in companyVehicleRisk.Risk.Beneficiaries)
                        {
                            FilterIndividual filterIndividualBeneficiary = null;
                            filterIndividualBeneficiary = validateActiveIndividual(null, null, companyBeneficiary);

                            if (filterIndividualBeneficiary != null && filterIndividualBeneficiary.DeclinedDate != null)
                            {
                                massiveRenewalRow.HasError = true;
                                massiveRenewalRow.Observations += Errors.BeneficiaryInactive + KeySettings.ReportErrorSeparatorMessage();
                                filterIndividualBeneficiary = null;
                            }
                        }
                    }


                    //Validar individuos activos en el sistema
                    FilterIndividual validateActiveIndividual(Holder holder = null, CompanyIssuanceInsured companyIssuanceInsured = null, CompanyBeneficiary companyBeneficiary = null)
                    {
                        FilterIndividual filterIndividualTemp = null;
                        if (holder != null)
                        {
                            switch (holder.IndividualType)
                            {
                                case IndividualType.Person:
                                    filterIndividualTemp = cacheListForVehicle.VehicleFilterIndividuals.Find(p => p.IndividualType == IndividualType.Person && p.Person.IndividualId == holder.IndividualId);

                                    break;

                                case IndividualType.Company:
                                    filterIndividualTemp = cacheListForVehicle.VehicleFilterIndividuals.Find(p => p.IndividualType == IndividualType.Company && p.Company.IndividualId == holder.IndividualId);
                                    break;

                            }
                        }

                        if (companyIssuanceInsured != null)
                        {
                            switch (companyIssuanceInsured.IndividualType)
                            {
                                case IndividualType.Person:
                                    filterIndividualTemp = cacheListForVehicle.VehicleFilterIndividuals.Find(p => p.IndividualType == IndividualType.Person && p.Person.IndividualId == companyIssuanceInsured.IndividualId);

                                    break;

                                case IndividualType.Company:
                                    filterIndividualTemp = cacheListForVehicle.VehicleFilterIndividuals.Find(p => p.IndividualType == IndividualType.Company && p.Company.IndividualId == companyIssuanceInsured.IndividualId);
                                    break;

                            }
                        }

                        if (companyBeneficiary != null)
                        {
                            switch (companyBeneficiary.IndividualType)
                            {
                                case IndividualType.Person:
                                    filterIndividualTemp = cacheListForVehicle.VehicleFilterIndividuals.Find(p => p.IndividualType == IndividualType.Person && p.Person.IndividualId == companyBeneficiary.IndividualId);

                                    break;

                                case IndividualType.Company:
                                    filterIndividualTemp = cacheListForVehicle.VehicleFilterIndividuals.Find(p => p.IndividualType == IndividualType.Company && p.Company.IndividualId == companyBeneficiary.IndividualId);
                                    break;

                            }
                        }

                        return filterIndividualTemp;
                    }



                    int bodyVehicle = companyVehicleRisk.Version.Body.Id;
                    CompanyFuel fuel = companyVehicleRisk.Version.Fuel;
                    Vehicles.Models.CompanyType vehicleType = companyVehicleRisk.Version.Type;
                    companyVehicleRisk.Version = ModelAssembler.CreateCompanyVersion(DelegateService.vehicleService.GetVersionByVersionIdModelIdMakeId(companyVehicleRisk.Version.Id, companyVehicleRisk.Model.Id, companyVehicleRisk.Make.Id));
                    companyVehicleRisk.Version.Type = vehicleType;
                    companyVehicleRisk.Version.Body.Id = bodyVehicle;
                    companyVehicleRisk.Version.Fuel = fuel;
                    companyVehicleRisk.ActualDateMovement = DateTime.Now.Date;
                    companyVehicleRisk.Risk.IsPersisted = true;
                    companyVehicleRisk.Risk.Status = RiskStatusType.Original;
                    CompanyVehicle companyVehicleFasecolda = DelegateService.vehicleService.GetVehicleByFasecoldaCode(companyVehicleRisk.Fasecolda.Description, companyVehicleRisk.Year);
                    if (companyVehicleFasecolda != null)
                    {
                        companyVehicleRisk.Price = companyVehicleFasecolda.Price;
                        if (companyVehicleFasecolda.Price == 0)
                        {
                            throw new ValidationException(Errors.TheVehicleHasNoInsuredValueForTheModel + companyVehicleRisk.Year);
                        }
                    }
                    companyVehicleRisk.StandardVehiclePrice = companyVehicleRisk.Price;

                    foreach (CompanyCoverage coverage in companyVehicleRisk.Risk.Coverages)
                    {
                        // Ajuste temporal se deberían sacar los paquetes de reglas al recuperar las coberturas
                        Coverage coverageInfo = DelegateService.underwritingService.GetCoverageByCoverageIdProductIdGroupCoverageId(coverage.Id, companyPolicy.Product.Id, companyVehicleRisk.Risk.GroupCoverage.Id);
                        coverage.RuleSetId = coverageInfo.RuleSetId;
                        coverage.PosRuleSetId = coverageInfo.PosRuleSetId;
                        coverage.CurrentFrom = companyPolicy.CurrentFrom;
                        coverage.CurrentTo = companyPolicy.CurrentTo;
                        coverage.CoverStatus = CoverageStatusType.Original;
                        coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                        coverage.AccumulatedPremiumAmount = coverageInfo.AccumulatedPremiumAmount;
                        coverage.FlatRatePorcentage = coverageInfo.FlatRatePorcentage;
                        coverage.IsMandatory = coverageInfo.IsMandatory;
                        coverage.IsSelected = coverageInfo.IsSelected;
                    }

                    List<CompanyCoverage> companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, companyVehicleRisk.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);

                    companyCoverages.RemoveAll(u => companyVehicleRisk.Risk.Coverages.Select(z => z.Id).Contains(u.Id));
                    foreach (CompanyCoverage coverage in companyCoverages.Where(u => u.IsSelected))
                    {
                        coverage.CurrentFrom = companyPolicy.CurrentFrom;
                        coverage.CurrentTo = companyPolicy.CurrentTo;
                        coverage.CoverStatus = CoverageStatusType.Original;
                        coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                        companyVehicleRisk.Risk.Coverages.Add(coverage);
                    }

                    Template templateAdditionalCoverages = file.Templates.Find(p => p.PropertyName == CompanyTemplatePropertyName.AdditionalCoverages);

                    if (templateAdditionalCoverages != null)
                    {

                        companyVehicleRisk.Risk.Coverages = DelegateService.massiveService.CreateAdditionalCoverages(companyCoverages, companyVehicleRisk.Risk.Coverages.Where(x => x.IsMandatory || x.IsSelected).ToList(), templateAdditionalCoverages.Rows);
                        if (templateAdditionalCoverages.Rows.Any(x => x.HasError))
                        {
                            massiveRenewalRow.HasError = true;
                            errorList = templateAdditionalCoverages.Rows.Select(t => string.Join(",", t.ErrorDescription)).ToList();
                            massiveRenewalRow.Observations += string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                        }
                    }
                    else
                    {
                        companyVehicleRisk.Risk.Coverages = companyVehicleRisk.Risk.Coverages.Where(x => x.IsMandatory || x.IsSelected).ToList();
                    }

                    string textRisk = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText));
                    companyVehicleRisk.Risk.Text = new CompanyText { TextBody = textRisk };
                    companyVehicleRisk.Risk.Policy = companyPolicy;

                    decimal price = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskPrice));
                    if (price > 0)
                    {
                        companyVehicleRisk.Price = price;
                    }

                    CompanyClause clauses = DelegateService.massiveService.GetClausesObligatory(EmissionLevel.Risk, companyPolicy.Prefix.Id, null).Find(x => x.Id != companyVehicleRisk.Risk.Clauses[0].Id);
                    if (clauses != null)
                    {
                        companyVehicleRisk.Risk.Clauses.Add(clauses);
                    }

                    //Plantilla Deducibles
                    templateName = Errors.Deductible;
                    Template templeteDeductible = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.Deductible);
                    if (templeteDeductible != null)
                        companyVehicleRisk.Risk.Coverages = DelegateService.massiveService.CreateDeductibles(companyVehicleRisk.Risk.Coverages, templeteDeductible);
                    //Plantilla Accesorios
                    int coverageIdAccNoOriginal = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.NonOriginalAccessories).NumberParameter.Value;
                    int coverageIdAccOriginal = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.OriginalAccessories).NumberParameter.Value;

                    Template tempalteAccesory = file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Accesories);
                    if (tempalteAccesory != null)
                    {
                        string errorScript = string.Empty;
                        List<CompanyAccessory> companyAccessories = DelegateService.massiveService.GetAccesorysByTemplate(tempalteAccesory, companyPolicy, companyVehicleRisk, coverageIdAccNoOriginal, coverageIdAccOriginal, ref errorScript);
                        if (string.IsNullOrEmpty(errorScript))
                        {
                            companyVehicleRisk.Accesories = companyAccessories;
                        }
                        else
                        {
                            massiveRenewalRow.HasError = true;
                            massiveRenewalRow.Observations += errorScript + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }

                    Template templateClauses = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.Clauses);
                    companyCoverages.ForEach(p => p.Clauses = DelegateService.massiveService.GetClausesByCoverageId(templateClauses, p.Id));

                    if (templateClauses != null)
                    {
                        string errorClause = string.Empty;
                        List<CompanyClause> companyClauses = new List<CompanyClause>();
                        List<CompanyCoverage> companyCoverage = new List<CompanyCoverage>();
                        DelegateService.massiveService.GetClausesByTemplate(templateClauses, ref companyClauses, ref companyCoverage, riskClauses, coverageClauses, ref errorClause);
                        if (string.IsNullOrEmpty(errorClause))
                        {
                            if (companyClauses.Count > 0)
                            {
                                companyVehicleRisk.Risk.Clauses = companyClauses.Distinct().ToList();
                            }
                            if (companyCoverages.Count > 0)
                            {
                                companyVehicleRisk.Risk.Coverages = companyCoverages;
                            }
                        }
                        else
                        {
                            massiveRenewalRow.HasError = true;
                            massiveRenewalRow.Observations += errorClause + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }

                    Template templateScript = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.DinamicConcepts);

                    if (companyPolicy.Product.CoveredRisk.ScriptId.HasValue && companyPolicy.Product.CoveredRisk.ScriptId > 0)
                    {
                        if (templateScript == null)
                        {
                            massiveRenewalRow.HasError = true;
                            massiveRenewalRow.Observations += string.Format(Errors.TemplateScriptRequired, Errors.TemplateScript, companyPolicy.Product.Id);
                        }
                    }
                    if (templateScript != null)
                    {
                        string errorScript = string.Empty;
                        List<DynamicConcept> dynamicConcepts = DelegateService.massiveService.GetDynamicConceptsByTemplate(companyPolicy.Product.CoveredRisk.ScriptId, templateScript, ref errorScript);

                        if (string.IsNullOrEmpty(errorScript))
                        {
                            companyVehicleRisk.Risk.DynamicProperties = dynamicConcepts;
                        }
                        else
                        {
                            massiveRenewalRow.HasError = true;
                            massiveRenewalRow.Observations += errorScript + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }

                    if (massiveRenewalRow.HasError.HasValue && massiveRenewalRow.HasError.Value)
                    {
                        massiveRenewalRow.TemporalId = null;
                        massiveRenewalRow.Status = MassiveLoadProcessStatus.Validated;
                        DelegateService.massiveRenewal.UpdateMassiveRenewalRow(massiveRenewalRow);
                        return;
                    }

                    templateName = "";

                    string pendingOperationRiskJsonIsnotNull = COMUT.JsonHelper.SerializeObjectToJson(companyVehicleRisk);
                    string pendingOperationPolicyJsonIsnotNull = COMUT.JsonHelper.SerializeObjectToJson(companyPolicy);

                    PendingOperation policyPendingOperation = new PendingOperation
                    {
                        IsMassive = true,
                        Operation = pendingOperationPolicyJsonIsnotNull,
                        UserId = companyPolicy.UserId
                    };


                    PendingOperation pendingOperationRisk = new PendingOperation
                    {
                        IsMassive = true,
                        UserId = companyPolicy.UserId,
                        OperationName = "Temporal",
                        Operation = pendingOperationRiskJsonIsnotNull
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
                massiveRenewalRow.Status = MassiveLoadProcessStatus.Validated;
                massiveRenewalRow.HasError = true;
                massiveRenewalRow.TemporalId = null;

                if (string.IsNullOrEmpty(templateName))
                {
                    massiveRenewalRow.Observations += Errors.ErrorCreateRisk + " : " + ex.Message;
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

        /// <summary>
        /// Crear Accesorios
        /// </summary>
        /// <param name="accesoriesTemplate">Plantilla Accesorios</param>
        /// <returns>Accesorios</returns>
        private List<CompanyAccessory> CreateAccesories(List<CompanyAccessory> companyPolicyAccesories, Template template, MassiveRenewalRow massiveRenewalRow)
        {
            List<CompanyAccessory> accesories = new List<CompanyAccessory>();
            List<CompanyAccessory> totalAccessories = ModelAssembler.CreateAccesories(DelegateService.vehicleService.GetAccessories());

            if (template != null)
            {
                templateName = template.Description;

                foreach (Row row in template.Rows)
                {
                    CompanyAccessory accessory = new CompanyAccessory();

                    accessory.Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AccesoriesAccessoryId));
                    accessory.IsOriginal = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AccesoriesIsOriginal));
                    if (accessory.Id == 0)
                    {
                        throw new ValidationException(Errors.ValidateErrorAcessory + KeySettings.ReportErrorSeparatorMessage());
                    }
                    if (!accessory.IsOriginal)
                    {
                        accessory.Amount = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AccesoriesPrice));
                    }

                    CompanyAccessory realAccesory = totalAccessories.Find(p => p.Id == accessory.Id);

                    if (realAccesory == null)
                    {
                        throw new ValidationException(Errors.ErrorAccesoryIdNotFound);
                    }

                    accessory.Description = realAccesory.Description;
                    accessory.RateType = realAccesory.RateType;
                    accessory.Rate = realAccesory.Rate;

                    accesories.Add(accessory);
                }

                //Válida accesorios duplicados
                var consolidatedAccessory =
                   from a in template.Rows
                   group a by new
                   {
                       Id = Convert.ToInt32(a.Fields.First(x => x.PropertyName == FieldPropertyName.AccesoriesAccessoryId).Value)
                   }
                   into accesoriesId
                   where accesoriesId.Count() > 1
                   select new
                   {
                       Id = accesoriesId.Key.Id,
                       Total = accesoriesId.Count(),
                   };

                foreach (var item in consolidatedAccessory)
                {
                    if (accesories.Select(i => i.Id).Distinct().Contains(item.Id))
                    {
                        massiveRenewalRow.HasError = true;
                        massiveRenewalRow.Observations += Errors.ErrorAccesoriesDuplicated + item.Id + KeySettings.ReportErrorSeparatorMessage();
                    }
                }
            }
            foreach (CompanyAccessory accessoryCompany in accesories)
            {
                if (companyPolicyAccesories != null)
                {
                    if (!companyPolicyAccesories.Exists(p => p.Id == accessoryCompany.Id))
                    {
                        companyPolicyAccesories.Add(accessoryCompany);

                    }
                    else
                    {
                        companyPolicyAccesories.Find(p => p.Id == accessoryCompany.Id).Amount = accessoryCompany.Amount;
                    }
                }
                else
                {
                    companyPolicyAccesories = new List<CompanyAccessory>();
                    companyPolicyAccesories.Add(accessoryCompany);
                }
            }

            return companyPolicyAccesories;
        }
        #endregion

        #region Métodos y propiedades para la generación del reporte
        private static List<Use> uses = new List<Use>();
        // private static List<DocumentType> documentTypes = new List<DocumentType>();
        private static List<Color> colors = new List<Color>();
        private static List<Deductible> deductibles = new List<Deductible>();
        private static List<Accessory> accesoriesList = new List<Accessory>();
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
            
            int bulkExcel = Convert.ToInt32(DelegateService.commonService.GetKeyApplication("MaxMassiveExcelRow"));

            file.FileType = FileType.CSV;

            TP.Parallel.ForEach(massiveLoadProcesses,
                    (process) =>
                    {
                        FillVehicleFields(massiveRenewal, process, serializeFields);

                        if (concurrentRows.Count >= bulkExcel || massiveLoadProcesses.Count == 0)
                        {
                            file.Templates[0].Rows = concurrentRows.ToList().OrderBy(x => x.Number).ToList();
                            file.Name = "Reporte Autos_" + key + "_" + massiveRenewal.Id;
                            filePath = DelegateService.utilitiesService.GenerateFile(file);
                            concurrentRows = new ConcurrentBag<Row>();
                        }
                    });
            
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
                // int goodExpNumPrinter = DelegateService.vehicleService.GetGoodExpNumPrinter();

                CompanyPolicy companyPolicy = DelegateService.massiveService.GetCompanyPolicyByMassiveLoadStatusPolicy(massiveRenewal.Status.Value, proccess.Risk.Policy);
                if (companyPolicy == null)
                {
                    return;
                }
                List<Field> fields = DelegateService.massiveService.GetFields(serializeFields, companyPolicy);
                List<CompanyVehicle> vehicles = GetCompanyVehicle(massiveRenewal, proccess, companyPolicy.Id);
                foreach (CompanyVehicle vehicle in vehicles)
                {
                    fields.Find(u => u.PropertyName == FieldPropertyName.Identificator).Value = proccess.RowNumber.ToString();
                    fields.Find(u => u.PropertyName == FieldPropertyName.LoadId).Value = massiveRenewal.Id.ToString();
                    fields = DelegateService.massiveService.FillInsuredFields(fields, vehicle.Risk.MainInsured);
                    fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = vehicle.Price.ToString();

                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskNumber).Value = "1";// vehicle.Number.ToString();
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskRatingZoneDescription).Value = vehicle.Risk.RatingZone != null ? ratingZones.FirstOrDefault(r => r.Id == vehicle.Risk.RatingZone.Id).Description : "";
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskVehicleTypeDescription).Value = (types.Count > 0) ? types.FirstOrDefault(u => u.Id == vehicle.Version.Type.Id).Description : "";
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskMakeDescription).Value = vehicle.Make.Description;
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskVehicleDescription).Value = vehicle.Model.Description + " " + vehicle.Version.Description;//pendiente
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskColorDescription).Value = colors.First(u => u.Id == vehicle.Color.Id).Description;
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskModel).Value = vehicle.Year.ToString();
                    fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskFasecolda).Value = vehicle.Fasecolda.Description;
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskLicensePlate).Value = vehicle.LicensePlate;
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskEngineDescription).Value = vehicle.EngineSerial;
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskChassisDescription).Value = vehicle.ChassisSerial;
                    if (vehicle.Use != null && vehicle.Use.Id > 0)
                    {
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskServiceTypeDescription).Value = (uses.Count > 0) ? uses.First(u => u.Id == vehicle.Use.Id).Description : "";
                    }
                    companyPolicy.PaymentPlan = DelegateService.underwritingService.GetPaymentPlanByPaymentPlanId(companyPolicy.PaymentPlan.Id);
                    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyCurrencyDescription).Value = companyPolicy.ExchangeRate != null ? currency.FirstOrDefault(l => l.Id == companyPolicy.ExchangeRate.Currency.Id).Description : "";
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskLimitRcDescription).Value = vehicle.Risk.LimitRc != null ? limitRc.FirstOrDefault(l => l.Id == vehicle.Risk.LimitRc.Id).Description : "";
                    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyNumberDays).Value = companyPolicy.Endorsement.EndorsementDays == 0 ? companyPolicy.CurrentTo.Subtract(companyPolicy.CurrentFrom).Days.ToString() : "";
                    fields.Find(u => u.PropertyName == FieldPropertyName.MethodOfPaymentDescription).Value = companyPolicy.PaymentPlan.Description;
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskChassisDescription).Value = vehicle.ChassisSerial;
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskRate).Value = vehicle.Rate.ToString();
                    fields.Find(u => u.PropertyName == FieldPropertyName.AccesoriesDescription).Value = CreateAccessories(vehicle.Accesories);
                    fields.Find(u => u.PropertyName == FieldPropertyName.TotalAccesories).Value = vehicle.PriceAccesories.ToString();
                    fields.Find(u => u.PropertyName == FieldPropertyName.RiskPrice).Value = vehicle.Risk.AmountInsured.ToString();
                    List<Beneficiary> beneficiary = ModelAssembler.CreateBeneficiarys(vehicle.Risk.Beneficiaries);
                    fields.Find(u => u.PropertyName == FieldPropertyName.BeneficiaryData).Value = DelegateService.massiveService.CreateBeneficiaries(vehicle.Risk.Beneficiaries);
                    if (vehicle.Risk.Clauses != null)
                    {
                        List<Clause> clauses = ModelAssembler.CreateClauses(vehicle.Risk.Clauses);
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(vehicle.Risk.Clauses);
                    }

                    if (vehicle.GoodExperienceYear != null)
                    {
                        if (vehicle.GoodExperienceYear.GoodExpNumPrinter < 0)
                        {
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = "0";
                        }
                        //else
                        //{
                        //    if (vehicle.GoodExperienceYear.GoodExpNumPrinter >= goodExpNumPrinter)
                        //    {
                        //        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = String.Format("{0} o más", goodExpNumPrinter);
                        //    }
                        //    else
                        //    {
                        //        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = vehicle.GoodExperienceYear.GoodExpNumPrinter.ToString();
                        //    }
                        //}
                    }

                    //Asistencia
                    CompanyCoverage coverageAsistance = vehicle.Risk.Coverages.FirstOrDefault(u => u.Id == 9);
                    if (coverageAsistance != null)
                    {
                        var assistancePremium = coverageAsistance.PremiumAmount;
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyPremium).Value =
                            (companyPolicy.Summary.Premium - assistancePremium).ToString("F2");
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyExpensesWithAssistance).Value =
                            (companyPolicy.Summary.Expenses + assistancePremium).ToString("F2");
                    }

                    serializeFields = JsonConvert.SerializeObject(fields);
                    foreach (CompanyCoverage coverage in vehicle.Risk.Coverages.OrderByDescending(u => u.Number))
                    {
                        List<Field> fieldsC = JsonConvert.DeserializeObject<List<Field>>(serializeFields);

                        fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageDescription).Value = coverage.Description;
                        fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageInsuredValue).Value = coverage.EndorsementSublimitAmount.ToString();
                        fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageRate).Value = coverage.Rate.ToString();
                        fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoveragePremium).Value = coverage.PremiumAmount.ToString();
                        fieldsC.Find(u => u.PropertyName == FieldPropertyName.DeductibleDescription).Value = (coverage.Deductible != null && coverage.Deductible.Description != null) ? coverage.Deductible.Description : "";
                        List<Clause> clauses = ModelAssembler.CreateClauses(coverage.Clauses);
                        fieldsC.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(vehicle.Risk.Clauses);
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



        private List<CompanyVehicle> GetCompanyVehicle(MassiveRenewal massiveRenewal, MassiveRenewalRow proccess, int tempId)
        {
            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();
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
                        companyVehicles.Add(JsonConvert.DeserializeObject<CompanyVehicle>(item.Operation));
                    }

                    break;
                case MassiveLoadStatus.Issued:
                    var policy = proccess.Risk.Policy;
                    if (policy.Endorsement != null && policy.Endorsement.EndorsementType.HasValue)
                    {
                        //companyVehicles = DelegateService.vehicleService.GetVehiclesByPrefixBranchDocumentNumberEndorsementType(massiveRenewal.Prefix.Id, policy.Branch.Id, policy.DocumentNumber, policy.Endorsement.EndorsementType.Value);
                        if (!Settings.UseReplicatedDatabase())
                        {
                            /* without Replicated Database */
                            DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(policy.Endorsement.Id).ForEach(x => companyVehicles.Add(JsonConvert.DeserializeObject<CompanyVehicle>(x)));
                        }
                        else
                        {
                            DelegateService.pendingOperationEntityService.GetRiskByEndorsementDocumentNumber(policy.Endorsement.Id).ForEach(x => companyVehicles.Add(JsonConvert.DeserializeObject<CompanyVehicle>(x)));
                        }
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
                foreach (CompanyAccessory accessory in accessories)
                {
                    value += (accesoriesList.Count > 0 ? accesoriesList.FirstOrDefault(u => u.Id == accessory.Id).Description : "") + " " + Convert.ToInt64(accessory.Amount) + " | ";
                }
            }

            return value;
        }

        private void LoadList()
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
            List<DocumentType> documentTypes = (List<DocumentType>)DelegateService.massiveService.GetCacheList("documentTypes_", "Report");
            deductibles = (List<Deductible>)DelegateService.massiveService.GetCacheList("deductibles_", "Report");
            currency = DelegateService.commonService.GetCurrencies();
            limitRc = DelegateService.underwritingService.GetLimitsRc();
            ratingZones = DelegateService.underwritingService.GetRatingZones();
        }
        #endregion
    }
}