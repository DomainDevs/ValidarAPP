using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider.Assemblers;
using Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.MassiveVehicleServices.Models;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
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
using EnumsCore = Sistran.Core.Application.UnderwritingServices.Enums;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider.DAOs
{
    public class MassiveLoadVehicleDAO
    {
        string templateName = "";
        private static List<Use> uses = new List<Use>();
        private static List<DocumentType> documentTypes = new List<DocumentType>();
        private static List<Color> colors = new List<Color>();
        private static List<Deductible> deductibles = new List<Deductible>();
        private static List<Accessory> accesoriesList = new List<Accessory>();
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
                Key5 = (int)SubCoveredRiskType.Vehicle
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
                CompanyRequest companyRequest = null;
                if (row.Fields.Any(y => y.PropertyName == FieldPropertyName.RequestGroup))
                {
                    massiveEmission.BillingGroupId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BillingGroup));
                    massiveEmission.RequestId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RequestGroup));


                    if (massiveEmission.BillingGroupId > 0 && massiveEmission.RequestId > 0)
                    {
                        companyRequest = DelegateService.massiveService.GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber((int)massiveEmission.BillingGroupId, massiveEmission.RequestId.ToString(), null).FirstOrDefault();

                        if (companyRequest == null)
                        {
                            throw new ValidationException(Errors.ErrorCompanyRequestNotExist);
                        }
                        else if (companyRequest.Prefix.Id != massiveEmission.Prefix.Id)
                        {
                            throw new ValidationException(Errors.MessageRequestPrefixDoesNotMatch);
                        }
                        else
                        {
                            var reqCurrentEndorsement = companyRequest.CompanyRequestEndorsements.OrderBy(x => x.DocumentNumber).Last();
                            massiveEmission.Product = reqCurrentEndorsement.Product;
                            massiveEmission.Agency = reqCurrentEndorsement.Agencies.First(a => a.IsPrincipal);
                            massiveEmission.Branch = companyRequest.Branch;
                            agentId = massiveEmission.Agency.Code;
                            agentTypeId = massiveEmission.Agency.AgentType.Id;
                            productId = massiveEmission.Product.Id;
                            requestId = massiveEmission.RequestId.GetValueOrDefault();
                            billingGroupId = massiveEmission.BillingGroupId.GetValueOrDefault();
                        }
                    }
                }

                DelegateService.massiveService.UpdateMassiveLoad(massiveEmission);

                massiveEmission.File.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.EmisionAutos).Rows = DelegateService.massiveService.GetMassivePlatesValidation(massiveEmission.File.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.EmisionAutos).Rows);

                List<File> files = DelegateService.utilitiesService.GetDataTemplates(massiveEmission.File.Templates);

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
                    if (companyRequest != null)
                    {
                        DateTime currentFrom = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
                        if (!companyRequest.CompanyRequestEndorsements.Any(r => currentFrom >= r.CurrentFrom && r.CurrentTo >= currentFrom))
                        {
                            file.Templates[0].Rows[0].HasError = true;
                            file.Templates[0].Rows[0].ErrorDescription += Errors.MessageCurrentFromDateExceedsRequest + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }
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
            List<FilterIndividual> filtersIndividuals = DelegateService.massiveService.GetFilterIndividuals(massiveEmission.User.UserId, massiveEmission.Branch.Id, files, TemplatePropertyName.EmisionAutos);

            List<Clause> riskClauses = DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.Risk, (int)CoveredRiskType.Vehicle);
            List<Clause> coverageClauses = DelegateService.underwritingService.GetClausesByEmissionLevel(EmissionLevel.Coverage);

            List<CompanyClause> companyRiskClauses = new List<CompanyClause>();
            List<CompanyClause> companyClauses = new List<CompanyClause>();

            foreach (Clause riskClause in riskClauses)
            {
                companyRiskClauses.Add(MappCompanyClause(riskClause));
            }

            foreach (Clause clause in coverageClauses)
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

        public List<VehicleFilterIndividual> CreateVehicleFilterIndividual(List<FilterIndividual> filtersIndividuals, int prefixId, int userId)
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
            //Mapper.CreateMap<FilterIndividual, VehicleFilterIndividual>();
            List<VehicleFilterIndividual> vehicleFilterIndividuals = Mapper.Map<List<FilterIndividual>, List<VehicleFilterIndividual>>(filtersIndividuals);



            return vehicleFilterIndividuals;
        }

        public void ProcessResponseFromScoreService(string response)
        {
            //int massiveLoadId = DelegateService.externalProxyService.UpdateExternalFilterByInsuredResponse(response, true, false, false, false);
            //UpdatePolicyWithExternaldata(massiveLoadId);
        }

        public void ProcessResponseFromSimitService(string response)
        {

            //int massiveLoadId = DelegateService.externalProxyService.UpdateExternalFilterByInsuredResponse(response, false, true, false, false);
            //UpdatePolicyWithExternaldata(massiveLoadId);
        }

        public void ProcessResponseFromExperienceServiceHistoricPolicies(string response)
        {

            //int massiveLoadId = DelegateService.externalProxyService.UpdateExternalFilterByInsuredResponse(response, false, false, true, false);
            //UpdatePolicyWithExternaldata(massiveLoadId);

        }
        public void ProcessResponseFromExperienceServiceHistoricSinister(string response)
        {
            //int massiveLoadId = DelegateService.externalProxyService.UpdateExternalFilterByInsuredResponse(response, false, false, false, true);
            //UpdatePolicyWithExternaldata(massiveLoadId);

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
        private void GetClauses(CompanyPolicy policy)
        {
            var mapper = ModelAssembler.CreateMapCompanyClause();
            List<CompanyClause> clauses = mapper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingServiceCore.GetClausesByEmissionLevelConditionLevelId(EnumsCore.EmissionLevel.General, policy.Prefix.Id));

            if (policy.Clauses != null)
            {
                policy.Clauses = policy.Clauses.Where(x => x.IsMandatory == false).ToList();
            }
            else
            {
                policy.Clauses = new List<CompanyClause>();
            }

            if (clauses.Count > 0)
            {
                policy.Clauses.AddRange(clauses.Where(x => x.IsMandatory == true).ToList());
            }
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
                Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.EmisionAutos).Rows.First();
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
                    templateName = file.Templates.First(x => x.PropertyName == TemplatePropertyName.EmisionAutos).Description;
                    companyPolicy = DelegateService.massiveUnderwritingService.CreateCompanyPolicy(massiveEmission, massiveEmissionRow, file, TemplatePropertyName.EmisionAutos, filtersIndividuals);
                    //Agrega Clausulas Obligatorias a nivel de riesgo 

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

                    CompanyVehicle companyVehicle = new CompanyVehicle
                    {
                        ActualDateMovement = DateTime.Now,
                        Risk = new CompanyRisk
                        {
                            Status = RiskStatusType.Original,
                            CoveredRiskType = CoveredRiskType.Vehicle,
                            MainInsured = DelegateService.massiveService.CreateInsured(row, companyPolicy.Holder, filtersIndividuals),
                            LimitRc = new CompanyLimitRc
                            {
                                Id = DelegateService.collectiveService.CreateLimitRc(row, companyPolicy.Prefix.Id, companyPolicy.Product.Id, companyPolicy.PolicyType.Id).Id,
                                LimitSum = DelegateService.collectiveService.CreateLimitRc(row, companyPolicy.Prefix.Id, companyPolicy.Product.Id, companyPolicy.PolicyType.Id).LimitSum,

                            },
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
                        ServiceType = new CompanyServiceType
                        {
                            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskServiceType))
                        },
                        GoodExperienceYear = new VehicleServices.Models.GoodExperienceYear(),

                    };
                    //Agrega Clausulas a nivel de riesgo 
                    companyVehicle.Risk.Clauses = DelegateService.massiveService.GetClausesObligatory(EmissionLevel.Risk, companyPolicy.Prefix.Id, null);

                    if (companyPolicy.Product.IsFlatRate)
                    {
                        if (string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskRate).Value))
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += Errors.ErrorProductIsFlateRate + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }
                    companyVehicle.Risk.Beneficiaries.Add(DelegateService.massiveService.CreateBeneficiary(row, companyVehicle.Risk.MainInsured, filtersIndividuals));

                    string fasecoldaCode = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskFasecolda)).ToString();

                    int yearVehicle = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskYear));

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
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += Errors.ErrorModelNotFound + KeySettings.ReportErrorSeparatorMessage();
                        }

                        if (companyVehicle.Version.Type.Id != (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskVehicleType)))
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += Errors.ErrorFasecoldaVehicleType + KeySettings.ReportErrorSeparatorMessage();
                        }

                        if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskBody).Value))
                        {
                            companyVehicle.Version.Body.Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskBody));
                        }
                        else
                        {
                            if (companyVehicle.Version.Body != null)
                            {
                                if (companyVehicle.Version.Body.Id == DelegateService.commonService.GetParameterByParameterId((int)CompanyParameterType.WithOutBodyVehicle).NumberParameter.GetValueOrDefault())
                                {
                                    massiveEmissionRow.HasError = true;
                                    massiveEmissionRow.Observations += Errors.ErrorBodyWithOutBody + KeySettings.ReportErrorSeparatorMessage();
                                }
                            }
                            else
                            {
                                massiveEmissionRow.HasError = true;
                                massiveEmissionRow.Observations += Errors.ErrorBodyNotFound + KeySettings.ReportErrorSeparatorMessage();
                            }
                        }

                        companyVehicle.Year = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskYear));
                        companyVehicle.OriginalPrice = companyVehicle.Price;
                        companyVehicle.NewPrice = companyVehicleFasecolda.Version.NewVehiclePrice.GetValueOrDefault();

                        companyVehicle.Version.Fuel = new CompanyFuel()
                        {
                            Id = 1
                        };
                    }
                    else
                    {
                        massiveEmissionRow.HasError = true;
                        massiveEmissionRow.Observations += Errors.ErrorFasecoldaNotFound + KeySettings.ReportErrorSeparatorMessage();

                        bool fasecoldaHasLettersOrCharacters = fasecoldaCode.Any(x => !char.IsNumber(x));

                        if (fasecoldaHasLettersOrCharacters)
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += Errors.FasecoldaCodeWithCharacters + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }

                    companyVehicle.Use = new CompanyUse
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskUse))
                    };

                    companyVehicle.Rate = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRate));
                    companyVehicle.IsNew = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskIsNew));
                    companyVehicle.LicensePlate = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLicensePlate)).ToString().ToUpper();
                    companyVehicle.EngineSerial = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskEngine)).ToString().ToUpper();
                    companyVehicle.ChassisSerial = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskChassis)).ToString().ToUpper();

                    if (companyVehicle.Price == 0)
                    {
                        CompanyVehicle companyVehicleByLicensePlate = DelegateService.vehicleService.GetVehicleByLicensePlate(companyVehicle.LicensePlate);
                        companyVehicle.Price = companyVehicleByLicensePlate == null ? 0 : companyVehicleByLicensePlate.Price;
                    }

                    companyVehicle.Color = new CompanyColor
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<Int32>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskColor))

                    };

                    companyVehicle.Risk.Text = new CompanyText
                    {
                        TextBody = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText)).ToString()
                    };

                    companyVehicle.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, companyVehicle.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);

                    companyVehicle.Risk.Coverages.ForEach(x => x.EndorsementType = companyPolicy.Endorsement.EndorsementType);
                    companyVehicle.Risk.Coverages.ForEach(x => x.CurrentFrom = companyPolicy.CurrentFrom);
                    companyVehicle.Risk.Coverages.ForEach(x => x.CurrentTo = companyPolicy.CurrentTo);

                    //Plantillas Adicionales
                    int coverageIdAccNoOriginal = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.NonOriginalAccessories).NumberParameter.Value;
                    int coverageIdAccOriginal = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.OriginalAccessories).NumberParameter.Value;


                    Template templateAccesories = file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Accesories);
                    if (templateAccesories != null)
                    {
                        string errorScript = string.Empty;
                        List<CompanyAccessory> companyAccessories = DelegateService.massiveService.GetAccesorysByTemplate(templateAccesories, companyPolicy, companyVehicle, coverageIdAccNoOriginal, coverageIdAccOriginal, ref errorScript);
                        if (string.IsNullOrEmpty(errorScript))
                        {
                            companyVehicle.Accesories = companyAccessories;
                        }
                        else
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += errorScript + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }

                    if (file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Accesories) != null && (companyVehicle.Risk.Coverages.Where(x => x.Id == coverageIdAccNoOriginal).Any() || companyVehicle.Risk.Coverages.Where(x => x.Id == coverageIdAccOriginal).Any())) // Si el producto tiene parametrizado coberturas de accesorios, agrega accesorios.
                    {
                        companyVehicle.Accesories = CreateAccesories(file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Accesories), massiveEmissionRow);
                    }
                    if (companyVehicle.Accesories != null)
                    {

                        foreach (Accessory item in companyVehicle.Accesories)
                        {
                            companyVehicle.PriceAccesories += item.Amount;
                        }
                    }

                    Template templateAdditionalCoverages = file.Templates.Find(p => p.PropertyName == CompanyTemplatePropertyName.AdditionalCoverages);

                    if (templateAdditionalCoverages != null)
                    {

                        companyVehicle.Risk.Coverages = DelegateService.massiveService.CreateAdditionalCoverages(companyVehicle.Risk.Coverages, companyVehicle.Risk.Coverages.Where(x => x.IsMandatory || x.IsSelected).ToList(), templateAdditionalCoverages.Rows);
                        companyVehicle.Risk.Coverages = CreateCoverange(companyVehicle.Risk.Coverages, templateAdditionalCoverages.Rows, massiveEmissionRow);

                        if (templateAdditionalCoverages.Rows.Any(x => x.HasError))
                        {
                            massiveEmissionRow.HasError = true;
                            errorList = templateAdditionalCoverages.Rows.Select(t => string.Join(",", t.ErrorDescription)).ToList();
                            massiveEmissionRow.Observations += string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                        }
                    }
                    else
                    {
                        companyVehicle.Risk.Coverages = companyVehicle.Risk.Coverages.Where(x => x.IsMandatory || x.IsSelected).ToList();
                    }

                    if (file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Deductible) != null)
                    {
                        companyVehicle.Risk.Coverages = DelegateService.massiveService.CreateDeductibles(companyVehicle.Risk.Coverages, file.Templates.Find(x => x.PropertyName == TemplatePropertyName.Deductible));
                    }


                    //Plantilla Beneficiarios Adicionales
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
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += errorAdditionalBeneficiaries + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }

                    templateName = file.Templates.First(x => x.PropertyName == TemplatePropertyName.EmisionAutos).Description;

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
                                companyVehicle.Risk.Clauses = companyClauses.Distinct().ToList();
                            }
                            if (companyCoverages.Count > 0)
                            {
                                companyVehicle.Risk.Coverages = companyCoverages;
                            }
                        }
                        else
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += errorClause + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }

                    Template templateScript = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.DinamicConcepts);

                    if (companyPolicy.Product.CoveredRisk.ScriptId.HasValue && companyPolicy.Product.CoveredRisk.ScriptId > 0)
                    {
                        if (templateScript == null)
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += string.Format(Errors.TemplateScriptRequired, Errors.TemplateScript, companyPolicy.Product.Id);
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
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += errorScript;
                        }
                    }

                    if (massiveEmissionRow.HasError)
                    {
                        DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
                        return;
                    }

                    string pendingOperationRiskJsonIsnotNull = COMUT.JsonHelper.SerializeObjectToJson(companyVehicle);
                    string pendingOperationPolicyJsonIsnotNull = COMUT.JsonHelper.SerializeObjectToJson(companyPolicy);

                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* Without Replicated Database */
                        PendingOperation pendingOperation = new PendingOperation
                        {
                            ParentId = companyPolicy.Id,
                            UserId = companyPolicy.UserId,
                            Operation = pendingOperationPolicyJsonIsnotNull
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
                            Operation = pendingOperationPolicyJsonIsnotNull,
                            IsMassive = true
                        };

                        PendingOperation pendingOperationRisk = new PendingOperation
                        {
                            UserId = companyPolicy.UserId,
                            Operation = pendingOperationRiskJsonIsnotNull,
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

        public void CheckExternalServices(CacheListForVehicle cacheListForVehicle, CompanyPolicy companyPolicy, CompanyVehicle companyVehicle, MassiveEmission massiveEmission, File file, Row row)
        {
            VehicleFilterIndividual vehicleFilterIndividual = cacheListForVehicle.VehicleFilterIndividuals.Find(x => x.InsuredCode == companyVehicle.Risk.MainInsured.InsuredId);
            bool scoreAlreadyQueried = false; bool simitAlreadyQueried = false; bool requireScore = false; bool requireSimit = false; bool requireFasecolda = false;
            string licencePlate = string.Empty; string surname = string.Empty;
            IdentificationDocument identificationDocument = new IdentificationDocument();

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

            if (companyPolicy.Product.IsScore.GetValueOrDefault() && companyVehicle.Risk.MainInsured.ScoreCredit == null)
            {
                requireScore = true;
                scoreAlreadyQueried = !cacheListForVehicle.InsuredForScoreList.TryAdd(companyVehicle.Risk.MainInsured.InsuredId, companyVehicle.Risk.MainInsured.InsuredId);
            }
            //if (companyPolicy.Product.IsFine.GetValueOrDefault() && companyVehicle.ListInfringementCount == null)
            //{
            //    requireSimit = true;
            //    simitAlreadyQueried = !cacheListForVehicle.InsuredForSimitList.TryAdd(companyVehicle.Risk.MainInsured.InsuredId, companyVehicle.Risk.MainInsured.InsuredId); ;
            //}
            if (companyPolicy.Product.IsFasecolda.GetValueOrDefault())
            {
                requireFasecolda = true;
                licencePlate = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.RiskLicensePlate));
            }

            if (requireScore || requireSimit || requireFasecolda)
            {
                //DelegateService.utilitiesService.CheckExternalServices(identificationDocument, surname, vehicleFilterIndividual.InsuredCode.Value, licencePlate, massiveEmission.Id, file.Id, (int)SubCoveredRiskType.Vehicle, massiveEmission.User.UserId, scoreAlreadyQueried, simitAlreadyQueried, requireScore, requireSimit, requireFasecolda);
            }
        }

        private List<CompanyCoverage> CreateCoverange(List<CompanyCoverage> companyCoverages, List<Row> templateAdditionalCoverages, MassiveEmissionRow massiveEmissionRow)
        {
            var consolidatedCoverage =
                         from a in templateAdditionalCoverages
                         group a by new
                         {
                             Id = Convert.ToInt32(a.Fields.First(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage).Value)
                         }
                         into accesoriesId
                         where accesoriesId.Count() > 1
                         select new
                         {
                             Id = accesoriesId.Key.Id,
                             Total = accesoriesId.Count(),
                         };
            foreach (var item in consolidatedCoverage)
            {
                if (companyCoverages.Select(i => i.Id).Distinct().Contains(item.Id))
                {
                    massiveEmissionRow.HasError = true;
                    massiveEmissionRow.Observations += Errors.ErrorCoverangeDuplicated + item.Id + KeySettings.ReportErrorSeparatorMessage();
                }
            }
            return companyCoverages;
        }

        /// <summary>
        /// Crear Accesorios
        /// </summary>
        /// <param name="accesoriesTemplate">Plantilla Accesorios</param>
        /// <returns>Accesorios</returns>
        private List<CompanyAccessory> CreateAccesories(Template template, MassiveEmissionRow massiveEmissionRow)
        {
            List<CompanyAccessory> accesories = new List<CompanyAccessory>();
            List<Accessory> totalAccessories = DelegateService.vehicleService.GetAccessories();

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
                        throw new ValidationException(Errors.ValidateErrorAcessory);
                    }
                    if (!accessory.IsOriginal)
                    {
                        accessory.Amount = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AccesoriesPrice));
                        if (accessory.Amount == 0)
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += string.Format(Errors.ErrorAccesoryValue, row.Fields.Find(x => x.PropertyName == FieldPropertyName.AccesoriesPrice).Description) + KeySettings.ReportErrorSeparatorMessage();
                        }
                    }

                    Accessory realAccesory = totalAccessories.Find(p => p.Id == accessory.Id);

                    if (realAccesory == null)
                    {
                        throw new ValidationException(Errors.ErrorAccesoryIdNotFound);
                    }

                    accessory.Description = realAccesory.Description;
                    accessory.RateType = realAccesory.RateType;
                    accessory.Rate = realAccesory.Rate;
                    accesories.Add(accessory);
                }
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
                        massiveEmissionRow.HasError = true;
                        massiveEmissionRow.Observations += Errors.ErrorAccesoriesDuplicated + item.Id + KeySettings.ReportErrorSeparatorMessage();
                    }
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
                            file.Name = "Reporte Autos_" + key + "_" + massiveEmission.Id;
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
                //int goodExpNumPrinter = DelegateService.vehicleService.GetGoodExpNumPrinter();

                CompanyPolicy companyPolicy = DelegateService.massiveService.GetCompanyPolicyByMassiveLoadStatusPolicy(massiveEmission.Status.Value, proccess.Risk.Policy);
                if (companyPolicy != null)
                {
                    List<Field> fields = DelegateService.massiveService.GetFields(serializeFields, companyPolicy);
                    List<CompanyVehicle> vehicles = GetCompanyVehicle(massiveEmission, proccess, companyPolicy.Id);
                    foreach (CompanyVehicle vehicle in vehicles)
                    {
                        fields.Find(u => u.PropertyName == FieldPropertyName.Identificator).Value = proccess.RowNumber.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.LoadId).Value = massiveEmission.Id.ToString();
                        fields = DelegateService.massiveService.FillInsuredFields(fields, vehicle.Risk.MainInsured);
                        fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = vehicle.Price.ToString();

                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskNumber).Value = "1";// vehicle.Number.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskRatingZoneDescription).Value = vehicle.Risk.RatingZone != null ? vehicle.Risk.RatingZone.Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskVehicleTypeDescription).Value = (types.Count > 0) ? types.FirstOrDefault(u => u.Id == vehicle.Version.Type.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskMakeDescription).Value = vehicle.Make.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskVehicleDescription).Value = vehicle.Model.Description + " " + vehicle.Version.Description;//pendiente
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskColorDescription).Value = colors.FirstOrDefault(u => u.Id == vehicle.Color.Id).Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskModel).Value = vehicle.Year.ToString();
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskFasecolda).Value = vehicle.Fasecolda.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskLicensePlate).Value = vehicle.LicensePlate;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskEngineDescription).Value = vehicle.EngineSerial;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskChassisDescription).Value = vehicle.ChassisSerial;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskServiceTypeDescription).Value = (uses.Count > 0 && vehicle.Use.Id > 0) ? uses.FirstOrDefault(u => u.Id == vehicle.Use.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskLimitRcDescription).Value = vehicle.Risk.LimitRc.Description;
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskRate).Value = vehicle.Rate.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.AccesoriesDescription).Value = CreateAccessories(vehicle.Accesories);
                        fields.Find(u => u.PropertyName == FieldPropertyName.TotalAccesories).Value = vehicle.PriceAccesories.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskPrice).Value = vehicle.Risk.AmountInsured.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.BeneficiaryData).Value = DelegateService.massiveService.CreateBeneficiaries(vehicle.Risk.Beneficiaries);
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(vehicle.Risk.Clauses);
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

                        serializeFields = JsonConvert.SerializeObject(fields);
                        foreach (CompanyCoverage coverage in vehicle.Risk.Coverages.OrderByDescending(u => u.Number))
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



        private List<CompanyVehicle> GetCompanyVehicle(MassiveEmission massiveEmission, MassiveEmissionRow proccess, int tempId)
        {
            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();
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
                        companyVehicles.Add(JsonConvert.DeserializeObject<CompanyVehicle>(item.Operation));
                    }

                    break;
                case MassiveLoadStatus.Issued:
                    //companyVehicles = DelegateService.vehicleService.GetVehiclesByPrefixBranchDocumentNumberEndorsementType(massiveEmission.Prefix.Id, massiveEmission.Branch.Id, proccess.Risk.Policy.DocumentNumber, EndorsementType.Emission);
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* without Replicated Database */
                        DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(proccess.Risk.Policy.Endorsement.Id).ForEach(x => companyVehicles.Add(JsonConvert.DeserializeObject<CompanyVehicle>(x)));
                    }
                    else
                    {
                        DelegateService.pendingOperationEntityService.GetRiskByEndorsementDocumentNumber(proccess.Risk.Policy.Endorsement.Id).ForEach(x => companyVehicles.Add(JsonConvert.DeserializeObject<CompanyVehicle>(x)));
                        /* with Replicated Database */
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

        private void LoadList(MassiveEmission massiveEmission)
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

            //Product product = DelegateService.underwritingService.GetProductByProductIdPrefixId(massiveEmission.Product.Id, massiveEmission.Prefix.Id);

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

                    CompanyVehicle vehicle = GetCompanyVehicle(massiveEmission, proccess, proccess.Risk.Policy.Id).FirstOrDefault();
                    string eventMessaje = "";
                    foreach (PoliciesAut item in vehicle.Risk.InfringementPolicies)
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

            file.Name = "Errores" + DateTime.Now.ToString("dd_MM_yyyy_ssms");
            return DelegateService.utilitiesService.GenerateFile(file);
        }

    }

}