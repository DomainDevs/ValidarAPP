using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Vehicles.MassiveThirdPartyLiabilityServices.EEProvider.Assemblers;
using Sistran.Company.Application.Vehicles.MassiveThirdPartyLiabilityServices.Models;
using Sistran.Company.Application.Vehicles.MassiveTPLServices.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
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
using VEMO = Sistran.Core.Application.Vehicles.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicles.MassiveThirdPartyLiabilityServices.EEProvider.DAOs
{
    public class MassiveLoadTplDAO
    {
        string templateName = "";
        private static List<Use> uses = new List<Use>();
        private static List<DocumentType> documentTypes = new List<DocumentType>();
        private static List<VEMO.Color> colors = new List<VEMO.Color>();
        private static List<Deductible> deductibles = new List<Deductible>();
        private static List<Core.Application.Vehicles.Models.Type> types = new List<Core.Application.Vehicles.Models.Type>();

        ConcurrentBag<Row> concurrentRows = new ConcurrentBag<Row>();

        /// <summary>
        /// Crear Cargue
        /// </summary>
        /// <param name="massiveEmission">Cargue</param>
        /// <returns>Cargue</returns>
        public MassiveEmission CreateMassiveEmission(MassiveEmission massiveEmission)
        {
            ValidateFile(massiveEmission);

            massiveEmission.Status = MassiveLoadStatus.Validating;
            massiveEmission = DelegateService.massiveUnderwritingService.CreateMassiveEmission(massiveEmission);
            try
            {

                if (massiveEmission != null)
                {
                    TP.Task.Run(() => ValidateData(massiveEmission));
                }
            }
            catch (Exception ex)
            {
                massiveEmission.HasError = true;
                massiveEmission.ErrorDescription = string.Format(Errors.ErrorValidatingFile, ex.Message);
                DelegateService.massiveService.UpdateMassiveLoad(massiveEmission);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
            return massiveEmission;
        }

        /// <summary>
        /// Validar Archivo
        /// </summary>
        /// <param name="massiveEmission">Cargue</param>
        private void ValidateFile(MassiveEmission massiveEmission)
        {
            FileProcessValue fileProcessValue = new FileProcessValue()
            {
                Key1 = (int)FileProcessType.MassiveEmission,
                Key3 = massiveEmission.LoadType.Id,
                Key4 = massiveEmission.Prefix.Id,
                Key5 = (int)SubCoveredRiskType.ThirdPartyLiability
            };

            string fileName = massiveEmission.File.Name;
            massiveEmission.File = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);

            if (massiveEmission.File != null)
            {
                massiveEmission.File.Name = fileName;
                massiveEmission.File = DelegateService.utilitiesService.ValidateFile(massiveEmission.File, massiveEmission.User.AccountName);
            }
            else
            {
                throw new ValidationException(Errors.ErrorFileParametrizationNotFound);
            }
        }

        /// <summary>
        /// Validar Archivo
        /// </summary>
        /// <param name="massiveEmission">Cargue</param>
        private void ValidateData(MassiveEmission massiveEmission)
        {
            try
            {
                massiveEmission.File = DelegateService.utilitiesService.ValidateDataFile(massiveEmission.File, massiveEmission.User.AccountName);
                massiveEmission.TotalRows = massiveEmission.File.Templates.First(x => x.IsPrincipal).Rows.Count();

                Row row = massiveEmission.File.Templates.First(x => x.IsPrincipal).Rows.First();
                int agentId = 0, agentTypeId = 0, productId = 0, requestId = 0, billingGroupId = 0;

                DelegateService.massiveService.UpdateMassiveLoad(massiveEmission);

                List<File> files = DelegateService.utilitiesService.GetDataTemplates(massiveEmission.File.Templates);

                massiveEmission.File.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.EmissionThirdPartyLiability).Rows = DelegateService.massiveService.GetMassivePlatesValidation(massiveEmission.File.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.EmissionThirdPartyLiability).Rows);

                MassiveVehicleValidationDAO massiveVehicleValidationDAO = new MassiveVehicleValidationDAO();

                List<Validation> validations = massiveVehicleValidationDAO.GetValidationsByFiles(files, massiveEmission, agentId, agentTypeId, productId, requestId, billingGroupId);

                Validation validation;

                foreach (File file in files)
                {

                    if (validations.Count > 0)
                    {
                        validation = validations.Find(x => x.Id == file.Id);
                        if (validation != null)
                        {
                            file.Templates[0].Rows[0].HasError = true;
                            file.Templates[0].Rows[0].ErrorDescription += validation.ErrorMessage;
                        }
                    }
                    //Validación Fecha Solicitud

                }

                CreateModels(massiveEmission, files);

                DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(massiveEmission.Id);
            }
            catch (Exception ex)
            {
                massiveEmission.Status = MassiveLoadStatus.Validated;
                massiveEmission.HasError = true;
                massiveEmission.ErrorDescription = string.Format(Errors.ErrorValidatingFile, ex.Message);
                DelegateService.massiveService.UpdateMassiveLoad(massiveEmission);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        /// <summary>
        /// Crear Modelos
        /// </summary>
        /// <param name="massiveLoad">Cargue</param>
        /// <param name="files">Datos</param>
        private void CreateModels(MassiveEmission massiveEmission, List<File> files)
        {
            List<FilterIndividual> filtersIndividuals = DelegateService.massiveService.GetFilterIndividuals(massiveEmission.User.UserId, massiveEmission.Branch.Id, files, TemplatePropertyName.EmissionThirdPartyLiability);

            List<Clause> riskClauses = DelegateService.underwritingService.GetClausesByEmissionLevel(EmissionLevel.Risk);
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


            ParallelHelper.ForEach(files, file =>
            {
                CreateModel(massiveEmission, file, filtersIndividuals, companyRiskClauses, companyClauses);
            });
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

        public List<ThirdPartyLiabilityFilterIndividual> CreateVehicleFilterIndividual(List<FilterIndividual> filtersIndividuals, int prefixId, int userId)
        {

            List<FilterIndividual> individualWithError = new List<FilterIndividual>();


            individualWithError.AddRange(filtersIndividuals.Where(i => !string.IsNullOrEmpty(i.Error)));


            filtersIndividuals.RemoveAll(i => !string.IsNullOrEmpty(i.Error));

            if (Settings.UseReplicatedDatabase())
            {
                //filtersIndividuals = DelegateService.externalProxyMirrorService.GetMassiveScoresCreditByLastValid(filtersIndividuals, prefixId, userId);
            }
            else
            {
                //filtersIndividuals = DelegateService.externalProxyService.GetMassiveScoresCreditByLastValid(filtersIndividuals, prefixId, userId);
            }
            filtersIndividuals.AddRange(individualWithError);
            //Mapper.CreateMap<FilterIndividual, TplFilterIndividual>();
            List<ThirdPartyLiabilityFilterIndividual> vehicleFilterIndividuals = Mapper.Map<List<FilterIndividual>, List<ThirdPartyLiabilityFilterIndividual>>(filtersIndividuals);



            return vehicleFilterIndividuals;
        }

        /// <summary>
        /// Valida el sarlaft en la row
        /// </summary>
        /// <param name="filterIndividuals"></param>
        /// <param name="row"></param>
        private void ValidateSarlaft(List<FilterIndividual> filterIndividuals, Row row, Holder holder, CompanyIssuanceInsured insured)
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

        /// <summary>
        /// Crear Modelo
        /// </summary>
        /// <param name="massiveEmission">Cargue</param>
        /// <param name="file">Datos</param>
        private void CreateModel(MassiveEmission massiveEmission, File file, List<FilterIndividual> filtersIndividuals, List<CompanyClause> riskClauses, List<CompanyClause> coverageClauses)
        {
            MassiveEmissionRow massiveEmissionRow = new MassiveEmissionRow();

            try
            {
                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.EmissionThirdPartyLiability).Rows.First();
                CompanyPolicy companyPolicy = null;

                List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription).Distinct())).ToList();
                massiveEmissionRow.MassiveLoadId = massiveEmission.Id;
                massiveEmissionRow.RowNumber = file.Id;
                massiveEmissionRow.Status = MassiveLoadProcessStatus.Validation;
                massiveEmissionRow.HasError = hasError;
                massiveEmissionRow.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                massiveEmissionRow.SerializedRow = JsonConvert.SerializeObject(file);
                DelegateService.massiveUnderwritingService.CreateMassiveEmissionRow(massiveEmissionRow);

                if (!hasError)
                {
                    templateName = file.Templates.First(x => x.PropertyName == TemplatePropertyName.EmissionThirdPartyLiability).Description;
                    companyPolicy = DelegateService.massiveUnderwritingService.CreateCompanyPolicy(massiveEmission, massiveEmissionRow, file, TemplatePropertyName.EmissionThirdPartyLiability, filtersIndividuals);
                    CompanyIssuanceInsured insured = DelegateService.massiveService.CreateInsured(row, companyPolicy.Holder, filtersIndividuals);

                    //Valida sarlaft
                    ValidateSarlaft(filtersIndividuals, row, companyPolicy.Holder, insured);
                    hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));

                    if (hasError)
                    {
                        massiveEmissionRow.HasError = hasError;
                        errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription).Distinct())).ToList();
                        massiveEmissionRow.Observations += string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                        DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
                    }
                }

                if (!hasError)
                {
                    massiveEmissionRow.Risk = new Risk
                    {
                        Policy = new Policy
                        {
                            Id = companyPolicy.Id
                        }
                    };

                    RatingZone rating = DelegateService.collectiveService.CreateRatingZone(row, companyPolicy.Prefix.Id);

                    CompanyTplRisk companyTplRisk = new CompanyTplRisk
                    {
                        //ActualDateMovement = DateTime.Now,
                        Risk = new CompanyRisk
                        {
                            Status = RiskStatusType.Original,
                            CoveredRiskType = CoveredRiskType.Vehicle,
                            MainInsured = DelegateService.massiveService.CreateInsured(row, companyPolicy.Holder, filtersIndividuals),
                            GroupCoverage = DelegateService.massiveUnderwritingService.CreateGroupCoverage(row, companyPolicy.Product.Id),
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
                            Policy = companyPolicy,
                            Beneficiaries = new List<CompanyBeneficiary>()
                        },
                        Deductible = new CompanyDeductible(),
                        ServiceType = new CompanyServiceType
                        {
                            Id = Convert.ToInt32(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskServiceType).Value)
                        }
                    };

                    if (companyPolicy.Product.IsFlatRate)
                    {
                        if (string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskRate).Value))
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += Errors.ErrorProductIsFlateRate + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }
                    companyTplRisk.Risk.Beneficiaries.Add(DelegateService.massiveService.CreateBeneficiary(row, companyTplRisk.Risk.MainInsured, filtersIndividuals));

                    companyTplRisk.PhoneNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskPhoneNumber));

                    companyTplRisk.Year = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskYear));

                    companyTplRisk.Shuttle = new CompanyShuttle();
                    companyTplRisk.Shuttle.Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskShuttleCode));

                    //Fasecolda
                    companyTplRisk.Make = new CompanyMake();
                    companyTplRisk.Make.Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskMakeCode));
                    companyTplRisk.Version = new CompanyVersion();
                    companyTplRisk.Model = new CompanyModel();
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
                                MapperConfiguration config = new MapperConfiguration(cfg =>
                                {
                                    cfg.CreateMap<VEMO.Model, CompanyModel>();

                                });
                                companyTplRisk.Model = config.CreateMapper().Map<VEMO.Model, CompanyModel>(version.Model);
                            }
                        }
                        else
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += Errors.ErrorYearFormat;
                        }
                    }

                    companyTplRisk.RePoweredVehicle = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskRepotentiatedVehicle));
                    if (companyTplRisk.RePoweredVehicle)
                    {
                        if (string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RepoweringYear).Value))
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += Errors.ErrorYearRePowered + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }
                    companyTplRisk.NewPrice = DelegateService.tplService.GetPriceByMakeIdModelIdVersionId(companyTplRisk.Make.Id, companyTplRisk.Model.Id, companyTplRisk.Version.Id, companyTplRisk.Year);

                    companyTplRisk.Use = new CompanyUse
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(
                            row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskUse))
                    };

                    companyTplRisk.Rate = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRate));
                    companyTplRisk.Risk.IsFacultative = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIsFacultative));
                    companyTplRisk.Risk.IsRetention = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskOnehundredRetention));

                    if (companyTplRisk.Risk.IsFacultative == true && companyTplRisk.Risk.IsRetention == true)
                    {
                        massiveEmissionRow.HasError = true;
                        massiveEmissionRow.Observations += string.Format(Errors.ValidateExcludingFacultativeFields, row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIsFacultative).Description, row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskOnehundredRetention).Description);
                    }

                    companyTplRisk.IsNew = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskIsNew));
                    companyTplRisk.LicensePlate = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLicensePlate)).ToString().ToUpper();
                    companyTplRisk.EngineSerial = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskEngine)).ToString().ToUpper();
                    companyTplRisk.ChassisSerial = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskChassis)).ToString().ToUpper();

                    companyTplRisk.Color = new CompanyColor
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<Int32>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskColor))

                    };

                    companyTplRisk.Risk.Text = new CompanyText
                    {
                        TextBody = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText)).ToString()
                    };

                    companyTplRisk.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, companyTplRisk.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);

                    companyTplRisk.Risk.Coverages.ForEach(x => x.EndorsementType = companyPolicy.Endorsement.EndorsementType);
                    companyTplRisk.Risk.Coverages.ForEach(x => x.CurrentFrom = companyPolicy.CurrentFrom);
                    companyTplRisk.Risk.Coverages.ForEach(x => x.CurrentTo = companyPolicy.CurrentTo);

                    int primaryCoverageId = companyTplRisk.Risk.Coverages.Where(x => x.IsPrimary == true).FirstOrDefault().Id;

                    List<Deductible> deductibles = DelegateService.underwritingService.GetDeductiblesByCoverageId(primaryCoverageId);
                    int templateDeductibleId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskDeductibleCode));
                    if (deductibles.Any(x => x.Id == templateDeductibleId))
                    {
                        companyTplRisk.Deductible.Id = templateDeductibleId;
                    }
                    else
                    {
                        massiveEmissionRow.HasError = true;
                        massiveEmissionRow.Observations += Errors.InvalidDeductible + KeySettings.ReportErrorSeparatorMessage();
                    }


                    Template templateAdditionalCoverages = file.Templates.Find(p => p.PropertyName == CompanyTemplatePropertyName.AdditionalCoverages);

                    if (templateAdditionalCoverages != null)
                    {

                        companyTplRisk.Risk.Coverages = DelegateService.massiveService.CreateAdditionalCoverages(companyTplRisk.Risk.Coverages, companyTplRisk.Risk.Coverages.Where(x => x.IsMandatory || x.IsSelected).ToList(), templateAdditionalCoverages.Rows);

                        if (templateAdditionalCoverages.Rows.Any(x => x.HasError))
                        {
                            massiveEmissionRow.HasError = true;
                            errorList = templateAdditionalCoverages.Rows.Select(t => string.Join(",", t.ErrorDescription)).ToList();
                            massiveEmissionRow.Observations += string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                        }
                    }
                    else
                    {
                        companyTplRisk.Risk.Coverages = companyTplRisk.Risk.Coverages.Where(x => x.IsMandatory || x.IsSelected).ToList();
                    }

                    if (file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Deductible) != null)
                    {
                        companyTplRisk.Risk.Coverages = DelegateService.massiveService.CreateDeductibles(companyTplRisk.Risk.Coverages, file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Deductible));
                    }

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
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += errorAdditionalBeneficiaries + KeySettings.ReportErrorSeparatorMessage();
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
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += errorClause + KeySettings.ReportErrorSeparatorMessage();
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
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += errorScript;
                        }
                    }

                    templateName = "";

                    if (massiveEmissionRow.HasError)
                    {
                        DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
                        return;
                    }


                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* Without Replicated Database */
                        PendingOperation pendingOperation = new PendingOperation
                        {
                            ParentId = companyPolicy.Id,
                            UserId = companyPolicy.UserId,
                            Operation = JsonConvert.SerializeObject(companyTplRisk)
                        };

                        DelegateService.utilitiesService.CreatePendingOperation(pendingOperation);
                        DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
                        /* Without Replicated Database */
                    }
                    else
                    {
                        /* with Replicated Database */
                        PendingOperation pendingOperationPolicy = new PendingOperation
                        {
                            UserId = companyPolicy.UserId,
                            Operation = JsonConvert.SerializeObject(companyPolicy),
                            IsMassive = true
                        };

                        PendingOperation pendingOperationRisk = new PendingOperation
                        {
                            UserId = companyPolicy.UserId,
                            Operation = JsonConvert.SerializeObject(companyTplRisk),
                            IsMassive = true
                        };

                        string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, JsonConvert.SerializeObject(pendingOperationRisk), (char)007, JsonConvert.SerializeObject(massiveEmissionRow), (char)007, nameof(MassiveEmissionRow));
                        QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "CreatePendingOperationQuee");
                        /* with Replicated Database */
                    }
                }
            }
            catch (Exception ex)
            {
                massiveEmissionRow.HasError = true;
                if (string.IsNullOrEmpty(templateName))
                {
                    massiveEmissionRow.Observations += Errors.ErrorCreateRisk;
                    EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                }
                else
                {
                    string[] messages = ex.Message.Split('|');
                    massiveEmissionRow.Observations += string.Format(Errors.ErrorInTemplate, templateName, messages[0]);
                }

                DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
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
        public string GenerateReportToMassiveLoad(MassiveEmission massiveEmission)
        {
            MassiveLoadProcessStatus processStatus = MassiveLoadProcessStatus.Validation;
            switch (massiveEmission.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    processStatus = MassiveLoadProcessStatus.Events;
                    break;
                case MassiveLoadStatus.Issued:
                    processStatus = MassiveLoadProcessStatus.Finalized;
                    break;
            }
            DelegateService.massiveService.LoadReportCacheList();
            LoadList(massiveEmission);
            List<MassiveEmissionRow> massiveLoadProcesses = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveEmission.Id, processStatus, false, null);
            SubCoveredRiskType subCoveredRiskType = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveEmission.Prefix.Id, (int)massiveEmission.CoveredRiskType);

            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.MassiveReport;
            fileProcessValue.Key4 = massiveEmission.Prefix.Id;
            fileProcessValue.Key5 = (int)subCoveredRiskType;


            File file = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);
            if (file == null && !massiveLoadProcesses.Any())
            {
                return "";
            }


            file.Templates[0].Rows[0].Fields.ForEach(u => u.Value = "");
            string serializeFields = JsonConvert.SerializeObject(file.Templates[0].Rows[0].Fields);
            string key = Guid.NewGuid().ToString();
            string filePath = "";
            
            int bulkExcel = Convert.ToInt32(DelegateService.commonService.GetKeyApplication("MaxMassiveExcelRow"));

            file.FileType = FileType.CSV;

            TP.Parallel.ForEach(massiveLoadProcesses,
                    (process) =>
                    {
                        FillVehicleFields(massiveEmission, process, serializeFields);

                        if (concurrentRows.Count >= bulkExcel || massiveLoadProcesses.Count == 0)
                        {
                            file.Templates[0].Rows = concurrentRows.ToList().OrderBy(x => x.Number).ToList();
                            file.Name = "Reporte RCH_" + key + "_" + massiveEmission.Id;
                            filePath = DelegateService.utilitiesService.GenerateFile(file);
                            concurrentRows = new ConcurrentBag<Row>();
                        }
                    });
            
            return filePath;
        }
        private void FillVehicleFields(MassiveEmission massiveEmission, MassiveEmissionRow proccess, string serializeFields)
        {
            try
            {
                Policy policy = new Policy();
                policy = proccess.Risk.Policy;
                policy.Endorsement = new Endorsement { EndorsementType = EndorsementType.Emission, Id = proccess.Risk.Policy.Endorsement.Id };
                policy.Branch = massiveEmission.Branch;
                policy.Prefix = massiveEmission.Prefix;
                //int goodExpNumPrinter = DelegateService.tplService.GetGoodExpNumPrinter();

                CompanyPolicy companyPolicy = DelegateService.massiveService.GetCompanyPolicyByMassiveLoadStatusPolicy(massiveEmission.Status.Value, proccess.Risk.Policy);
                if (companyPolicy != null)
                {
                    List<Field> fields = DelegateService.massiveService.GetFields(serializeFields, companyPolicy);
                    List<CompanyTplRisk> tplRisks = GetCompanyTplRisk(massiveEmission, proccess, companyPolicy.Id);
                    foreach (CompanyTplRisk tplrisk in tplRisks)
                    {
                        fields.Find(u => u.PropertyName == FieldPropertyName.Identificator).Value = proccess.RowNumber.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.LoadId).Value = massiveEmission.Id.ToString();
                        fields = DelegateService.massiveService.FillInsuredFields(fields, tplrisk.Risk.MainInsured);
                        //fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = tplrisk.Price.ToString();

                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskNumber).Value = "1";// tplrisk.Number.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskRatingZoneDescription).Value = tplrisk.Risk.RatingZone != null ? tplrisk.Risk.RatingZone.Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskVehicleTypeDescription).Value = (types.Count > 0) ? types.FirstOrDefault(u => u.Id == tplrisk.Version.Type.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskMakeDescription).Value = tplrisk.Make.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskVehicleDescription).Value = tplrisk.Model.Description + " " + tplrisk.Version.Description;//pendiente
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskColorDescription).Value = colors.FirstOrDefault(u => u.Id == tplrisk.Color.Id).Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskModel).Value = tplrisk.Year.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskLicensePlate).Value = tplrisk.LicensePlate;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskEngineDescription).Value = tplrisk.EngineSerial;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskChassisDescription).Value = tplrisk.ChassisSerial;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskServiceTypeDescription).Value = (uses.Count > 0 && tplrisk.Use.Id > 0) ? uses.FirstOrDefault(u => u.Id == tplrisk.Use.Id).Description : "";
                        //fields.Find(u => u.PropertyName == FieldPropertyName.RiskLimitRcDescription).Value = tplrisk.Risk.LimitRc.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskRate).Value = tplrisk.Rate.ToString();
                        //fields.Find(u => u.PropertyName == FieldPropertyName.AccesoriesDescription).Value = CreateAccessories(tplrisk.Accesories);
                        //fields.Find(u => u.PropertyName == FieldPropertyName.TotalAccesories).Value = tplrisk.PriceAccesories.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskPrice).Value = tplrisk.Risk.AmountInsured.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.BeneficiaryData).Value = DelegateService.massiveService.CreateBeneficiaries(tplrisk.Risk.Beneficiaries);
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(tplrisk.Risk.Clauses);
                        //if (tplrisk.GoodExperienceYear != null)
                        //{
                        //    if (tplrisk.GoodExperienceYear.GoodExpNumPrinter < 0)
                        //    {
                        //        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = "0";
                        //    }
                        //    else
                        //    {
                        //        //if (tplrisk.GoodExperienceYear.GoodExpNumPrinter >= goodExpNumPrinter)
                        //        //{
                        //        //    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = String.Format("{0} o más", goodExpNumPrinter);
                        //        //}
                        //        //else
                        //        //{
                        //        //    fields.Find(u => u.PropertyName == FieldPropertyName.PolicyYearWithOutClaim).Value = tplrisk.GoodExperienceYear.GoodExpNumPrinter.ToString();
                        //        //}
                        //    }
                        //}

                        //Fecha de vigencia del riesgo
                        //fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskCurrentFrom).Value = companyPolicy.CurrentFrom.ToString();
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
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageInsuredValue).Value = coverage.DeclaredAmount.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoverageRate).Value = coverage.Rate.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.CoveragePremium).Value = coverage.PremiumAmount.ToString();
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.DeductibleDescription).Value = (coverage.Deductible != null && coverage.Deductible.Description != null) ? coverage.Deductible.Description : "";
                            fieldsC.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(coverage.Clauses);
                            concurrentRows.Add(new Row
                            {
                                Fields = fieldsC,
                                Number = proccess.RowNumber
                            });
                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }



        private List<CompanyTplRisk> GetCompanyTplRisk(MassiveEmission massiveEmission, MassiveEmissionRow proccess, int tempId)
        {
            List<CompanyTplRisk> companyTplRisks = new List<CompanyTplRisk>();
            switch (massiveEmission.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    var pendingOperations = new List<PendingOperation>();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* Without Replicated Database */
                        pendingOperations = DelegateService.utilitiesService.GetPendingOperationsByParentId(tempId);
                        /* Without Replicated Database */
                    }
                    else
                    {
                        /* with Replicated Database */
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(tempId);
                        /* with Replicated Database */
                    }

                    foreach (PendingOperation item in pendingOperations)
                    {
                        companyTplRisks.Add(JsonConvert.DeserializeObject<CompanyTplRisk>(item.Operation));
                    }

                    break;
                case MassiveLoadStatus.Issued:
                    //companyTplRisks = DelegateService.tplService.GetVehiclesByPrefixBranchDocumentNumberEndorsementType(massiveEmission.Prefix.Id, massiveEmission.Branch.Id, proccess.Risk.Policy.DocumentNumber, EndorsementType.Emission);
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* without Replicated Database */
                        DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(proccess.Risk.Policy.Endorsement.Id).ForEach(x => companyTplRisks.Add(JsonConvert.DeserializeObject<CompanyTplRisk>(x)));
                    }
                    else
                    {
                        DelegateService.pendingOperationEntityService.GetRiskByEndorsementDocumentNumber(proccess.Risk.Policy.Endorsement.Id).ForEach(x => companyTplRisks.Add(JsonConvert.DeserializeObject<CompanyTplRisk>(x)));
                        /* with Replicated Database */
                    }
                    break;
            }
            return companyTplRisks;

        }


        private void LoadList(MassiveEmission massiveEmission)
        {
            if (uses.Count == 0)
            {
                uses = new List<Use>();//DelegateService.tplService.GetUses();
            }

            if (colors.Count == 0)
            {
                colors = DelegateService.tplService.GetColors();
            }
            if (types.Count == 0)
            {
                types = DelegateService.tplService.GetTypes();
            }


            documentTypes = (List<DocumentType>)DelegateService.massiveService.GetCacheList("documentTypes_", "Report");
            deductibles = (List<Deductible>)DelegateService.massiveService.GetCacheList("deductibles_", "Report");
        }
        #endregion

        /// <summary>
        /// Genera el archivo de error del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveLoadProccessId"></param>
        /// <returns></returns>
        public string GenerateFileErrorsMassiveEmission(int massiveLoadId)
        {
            MassiveEmission massiveEmission = new MassiveEmission();

            massiveEmission = DelegateService.massiveUnderwritingService.GetMassiveEmissionByMassiveLoadId(massiveLoadId);
            massiveEmission.Rows = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoadId, null, null, null);

            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.MassiveEmission;
            fileProcessValue.Key3 = massiveEmission.LoadType.Id;
            fileProcessValue.Key4 = massiveEmission.Prefix.Id;
            fileProcessValue.Key5 = (int)SubCoveredRiskType.Vehicle;


            File file = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);


            foreach (MassiveEmissionRow proccess in massiveEmission.Rows.OrderBy(x => x.Risk.Id))
            {
                File fileSerialized = JsonConvert.DeserializeObject<File>(proccess.SerializedRow);

                if (proccess.HasError && proccess.SerializedRow != null)
                {
                    foreach (Template t in fileSerialized.Templates)
                    {
                        file.Templates.Find(x => x.PropertyName == t.PropertyName).Rows.AddRange(t.Rows);
                    }

                    file.Templates[0].Rows.Last().Fields.Add(new Field()
                    {
                        ColumnSpan = 1,
                        FieldType = FieldType.String,
                        Value = proccess.Observations,
                        IsEnabled = true,
                        IsMandatory = false,
                        Id = 0,
                        Order = file.Templates[0].Rows.Last().Fields.Count(),
                        RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition
                    });
                }
                if (proccess.HasEvents && proccess.SerializedRow != null)
                {
                    foreach (Template t in fileSerialized.Templates.Where(x => x.PropertyName == "EmissionVehicle"))
                    {
                        file.Templates[file.Templates.Count - 1].Rows.AddRange(t.Rows);
                    }
                    //SE CONSULTAN LOS EVENTOS DEL JSON

                    CompanyTplRisk tplrisk = GetCompanyTplRisk(massiveEmission, proccess, proccess.Risk.Policy.Id).FirstOrDefault();
                    string eventMessaje = "";
                    foreach (PoliciesAut item in tplrisk.Risk.InfringementPolicies)
                    {
                        if (item.Type == Core.Application.AuthorizationPoliciesServices.Enums.TypePolicies.Authorization)
                        {
                            eventMessaje = eventMessaje + " " + Errors.Authorization + ": " + item.Message;
                        }
                        else if (item.Type == Core.Application.AuthorizationPoliciesServices.Enums.TypePolicies.Notification)
                        {
                            eventMessaje = eventMessaje + " " + Errors.Notification + ": " + item.Message;
                        }
                        else if (item.Type == Core.Application.AuthorizationPoliciesServices.Enums.TypePolicies.Restrictive)
                        {
                            eventMessaje = eventMessaje + " " + Errors.Restrictive + ": " + item.Message;
                        }
                    }

                    file.Templates[file.Templates.Count - 1].Rows.Last().Fields.Add(new Field()
                    {
                        ColumnSpan = 1,
                        FieldType = FieldType.String,
                        Value = eventMessaje,
                        IsEnabled = true,
                        IsMandatory = false,
                        Id = 0,
                        Order = file.Templates[file.Templates.Count - 1].Rows.Last().Fields.Count(),
                        RowPosition = file.Templates[file.Templates.Count - 1].Rows.Last().Fields.First().RowPosition
                    });
                }
            }

            file.Name = "Errores_" + DateTime.Now.ToString("dd_MM_yyyy_ssms");
            return DelegateService.utilitiesService.GenerateFile(file);
        }

    }

}