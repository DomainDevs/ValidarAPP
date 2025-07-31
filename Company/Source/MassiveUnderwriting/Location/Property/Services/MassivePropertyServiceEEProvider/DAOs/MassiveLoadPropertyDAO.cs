using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.UniquePersonService.Enums;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Locations.Models;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Framework;
using Sistran.Company.Application.Location.MassivePropertyServices.EEProvider.Resources;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using System.Collections.Concurrent;
using CommonModels = Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Massive.Models;
using Sistran.Company.Application.UniquePersonServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using System.Configuration;
using System.Globalization;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Location.MassivePropertyServices.Models;
using Sistran.Company.Application.ExternalProxyServices.Models;
using Sistran.Company.Application.CommonServices.Enums;
using System.Text.RegularExpressions;
using Sistran.Company.Application.Utilities.Extensions;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Company.Application.MassiveServices.Models;
using System.Diagnostics;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.MassivePropertyServices.EEProvider.DAOs
{
    public class MassiveLoadPropertyDAO
    {
        string templateName = "";
        private List<DocumentType> documentTypes = null;
        private List<Deductible> deductibles = null;
        private List<CompanyIrregularHeight> irregularHeights = null;
        private List<CompanyIrregularPlant> irregularPlants = null;
        private List<CompanyLevelZone> levelzones = null;
        private List<CompanyLocation> locations = null;
        private List<CompanyReinforcedStructureType> reinforcedStructureTypes = null;
        private List<CompanyRepair> companyRepairs = null;
        private List<CompanyStructureType> companyStructureTypes = null;
        private List<RiskUse> riskUse = null;
        private List<CompanyRiskTypeEarthquake> riskTypes = null;
        private List<ConstructionType> constructionTypes = null;
        private List<LineBusiness> lineBusiness = null;
        private List<SubLineBusiness> subLineBusiness = null;
        private List<CompanyInsuredObject> insuredObjects = null;
        private List<City> cities = null;
        private List<State> states = null;
        private List<CompanyDamage> damages = null;
        private ConcurrentBag<Row> concurrentRows = null;
        private List<RatingZone> ratingZones = null;
        private List<CompanyMicroZone> companyMicroZones = null;

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
                Key5 = (int)SubCoveredRiskType.Property
            };

            string fileName = massiveEmission.File.Name;
            massiveEmission.File = DelegateService.commonService.GetFileByFileProcessValue(fileProcessValue);

            if (massiveEmission.File != null)
            {
                massiveEmission.File.Name = fileName;
                massiveEmission.File = DelegateService.commonService.ValidateFile(massiveEmission.File, massiveEmission.User.AccountName);
            }
            else
            {
                throw new ValidationException(Errors.ErrorFileParametrizationNotFound);
            }
        }

        /// <summary>
        /// Validar archivo
        /// </summary>
        /// <param name="massiveEmission">Cargue</param>
        private void ValidateData(MassiveEmission massiveEmission)
        {
            try
            {
                massiveEmission.File = DelegateService.commonService.ValidateDataFile(massiveEmission.File, massiveEmission.User.AccountName);
                massiveEmission.TotalRows = massiveEmission.File.Templates.First(p => p.IsPrincipal).Rows.Count();

                Row row = massiveEmission.File.Templates.First(p => p.IsPrincipal).Rows.First();
                int agentId = 0, agentTypeId = 0, productId = 0, requestId = 0, billingGroupId = 0; ;
                CompanyRequest companyRequest = null;
                if (row.Fields.Any(p => p.PropertyName == FieldPropertyName.RequestGroup))
                {
                    massiveEmission.BillingGroupId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.BillingGroup));
                    massiveEmission.RequestId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.RequestGroup));

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

                List<File> files = DelegateService.commonService.GetDataTemplates(massiveEmission.File.Templates);

                MassiveLoadPropertyValidationDAO massiveLoadPropertyValidationDAO = new MassiveLoadPropertyValidationDAO();
                List<Validation> validations = massiveLoadPropertyValidationDAO.GetValidationsByFiles(files, massiveEmission, agentId, agentTypeId, productId, requestId, billingGroupId);

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

                    Row rowPrincipal = file.Templates.First(x => x.PropertyName == TemplatePropertyName.EmissionProperty).Rows.First();

                    //Validación Años de Comstrucción
                    int minYearAllowed = DelegateService.commonService.GetExtendedParameterByParameterId((int)CompanyParameterType.MinYearAllowed).NumberParameter.GetValueOrDefault();
                    int constructionYear = (int)DelegateService.commonService.GetValueByField<int>(rowPrincipal.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskConstructionYear));

                    if (constructionYear > DateTime.Now.Year)
                    {
                        file.Templates[0].Rows[0].HasError = true;
                        file.Templates[0].Rows[0].ErrorDescription += Errors.ErrorTheYearOfConstructionCanNotBeGreaterThanTheCurrent + KeySettings.ReportErrorSeparatorMessage();
                    }
                    else if (constructionYear < minYearAllowed)
                    {
                        file.Templates[0].Rows[0].HasError = true;
                        file.Templates[0].Rows[0].ErrorDescription += Errors.ErrorTheYearOfConstructionCanNotBeLessThan + " " + minYearAllowed + KeySettings.ReportErrorSeparatorMessage();
                    }

                    //Validación número de pisos
                    int floorNumber = (int)DelegateService.commonService.GetValueByField<int>(rowPrincipal.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskFloorNumber));
                    int maxFloorNumber = (int)DelegateService.commonService.GetExtendedParameterByParameterId((int)CompanyParameterType.MaxFloorNumber).NumberParameter;
                    int minFloorNumber = (int)DelegateService.commonService.GetExtendedParameterByParameterId((int)CompanyParameterType.MinFloorNumber).NumberParameter;

                    if (floorNumber > maxFloorNumber)
                    {
                        file.Templates[0].Rows[0].HasError = true;
                        file.Templates[0].Rows[0].ErrorDescription += Errors.ErrorTheNumberOfFloorsCanNotBeGreaterThan + " " + maxFloorNumber + KeySettings.ReportErrorSeparatorMessage();
                    }
                    else if (floorNumber < minFloorNumber)
                    {
                        file.Templates[0].Rows[0].HasError = true;
                        file.Templates[0].Rows[0].ErrorDescription += Errors.ErrorTheNumberOfFloorsCanNotBeLessThan + " " + minFloorNumber + KeySettings.ReportErrorSeparatorMessage();
                    }

                    //Validación EML
                    int eml = (int)DelegateService.commonService.GetValueByField<int>(rowPrincipal.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskEML));
                    if (eml < 0 || eml > 100)
                    {
                        file.Templates[0].Rows[0].HasError = true;
                        file.Templates[0].Rows[0].ErrorDescription += Errors.ErrorEMLMustBeBetween0And100 + KeySettings.ReportErrorSeparatorMessage();
                    }

                    //Validación Fecha Solicitud
                    if (companyRequest != null)
                    {
                        DateTime currentFrom = (DateTime)DelegateService.commonService.GetValueByField<DateTime>(rowPrincipal.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
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
            List<FilterIndividual> filtersIndividuals = DelegateService.massiveService.GetFilterIndividuals(massiveEmission.User.UserId, massiveEmission.Branch.Id, files, TemplatePropertyName.EmissionProperty);

            CacheListForProperty cacheListForProperty = new CacheListForProperty();
            List<FilterIndividual> individualWithError = new List<FilterIndividual>();

            individualWithError.AddRange(filtersIndividuals.Where(i => i.IsCLintonList == true));
            individualWithError.AddRange(filtersIndividuals.Where(i => !string.IsNullOrEmpty(i.Error)));

            filtersIndividuals.RemoveAll(i => i.IsCLintonList == true);
            filtersIndividuals.RemoveAll(i => !string.IsNullOrEmpty(i.Error));

            if (Settings.UseReplicatedDatabase())
            {
                filtersIndividuals = DelegateService.externalProxyMirrorService.GetMassiveScoresCreditByLastValid(filtersIndividuals, massiveEmission.Prefix.Id, massiveEmission.User.UserId);
            }
            else
            {
                filtersIndividuals = DelegateService.externalProxyService.GetMassiveScoresCreditByLastValid(filtersIndividuals, massiveEmission.Prefix.Id, massiveEmission.User.UserId);
            }

            cacheListForProperty.FilterIndividuals = filtersIndividuals;
            cacheListForProperty.FilterIndividuals.AddRange(individualWithError);
            cacheListForProperty.Alliances = DelegateService.uniquePersonService.GetAlliances();
            cacheListForProperty.InsuredForScoreList = new List<int>();

            List<Clause> riskClauses = DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.Risk, (int)CoveredRiskType.Location);
            List<Clause> coverageClauses = DelegateService.underwritingService.GetClausesByEmissionLevel(EmissionLevel.Coverage);

            ParallelHelper.ForEach(files, file =>
            {
                CreateModel(massiveEmission, file, cacheListForProperty, riskClauses, coverageClauses);
            });
        }

        /// <summary>
        /// Crear Modelo
        /// </summary>
        /// <param name="massiveEmission">Cargue</param>
        /// <param name="file">Datos</param>
        private void CreateModel(MassiveEmission massiveEmission, File file, CacheListForProperty cacheListForProperty, List<Clause> riskClauses, List<Clause> coverageClauses)
        {
            MassiveEmissionRow massiveEmissionRow = new MassiveEmissionRow();

            try
            {
                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                List<string> errorList = file.Templates.Select(t => string.Join(",", t.Rows.Where(r => !string.IsNullOrEmpty(r.ErrorDescription)).Select(r => r.ErrorDescription).Distinct())).ToList();

                massiveEmissionRow.MassiveLoadId = massiveEmission.Id;
                massiveEmissionRow.RowNumber = file.Id;
                massiveEmissionRow.Status = MassiveLoadProcessStatus.Validation;
                massiveEmissionRow.HasError = hasError;
                massiveEmissionRow.Observations = string.Join(KeySettings.ReportErrorSeparatorMessage(), errorList.Where(x => !string.IsNullOrEmpty(x)));
                massiveEmissionRow.SerializedRow = JsonConvert.SerializeObject(file);

                massiveEmissionRow = DelegateService.massiveUnderwritingService.CreateMassiveEmissionRow(massiveEmissionRow);

                if (!hasError)
                {
                    templateName = file.Templates.First(x => x.PropertyName == TemplatePropertyName.EmissionProperty).Description;
                    CompanyPolicy companyPolicy = DelegateService.massiveUnderwritingService.CreateCompanyPolicy(massiveEmission, massiveEmissionRow, file, TemplatePropertyName.EmissionProperty, cacheListForProperty.FilterIndividuals);

                    if (companyPolicy != null)
                    {
                        massiveEmissionRow.Risk = new Risk
                        {
                            Policy = new Policy
                            {
                                Id = companyPolicy.Id
                            }
                        };

                        Row row = file.Templates.First(x => x.PropertyName == TemplatePropertyName.EmissionProperty).Rows.First();

                        CompanyPropertyRisk companyProperty = new CompanyPropertyRisk
                        {
                            Status = RiskStatusType.Original,
                            CoveredRiskType = CoveredRiskType.Location,
                            CompanyRisk = new CompanyRisk(),
                            Number = 1
                        };
                        companyProperty.CompanyRisk.CompanyInsured = DelegateService.massiveService.CreateInsured(row, companyPolicy.Holder, cacheListForProperty.FilterIndividuals);


                        if (Settings.ImplementWebServices())
                        {
                            //Vaidación Externos 
                            Alliance validateAlliance = new Alliance();
                            if (companyPolicy.Alliance != null)
                            {
                                validateAlliance = cacheListForProperty.Alliances.Find(x => x.Id == companyPolicy.Alliance.Id);
                            }

                            companyPolicy.CompanyProduct.IsScore = DelegateService.externalProxyService.ValidateApplyScoreCreditByProduct(companyPolicy.CompanyProduct, validateAlliance, companyPolicy.Prefix.Id);
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

                        companyProperty.CompanyRisk.CompanyInsured = DelegateService.massiveService.CreateInsured(row, companyPolicy.Holder, cacheListForProperty.FilterIndividuals);
                        companyProperty.GroupCoverage = DelegateService.massiveUnderwritingService.CreateGroupCoverage(row, companyPolicy.CompanyProduct.Id);

                        companyProperty.Beneficiaries = new List<Beneficiary>();
                        companyProperty.Beneficiaries.Add(DelegateService.massiveService.CreateBeneficiary(row, companyProperty.CompanyRisk.CompanyInsured, cacheListForProperty.FilterIndividuals));

                        List<Beneficiary> beneficiaries = new List<Beneficiary>();

                        if (file.Templates.Find(x => x.PropertyName == TemplatePropertyName.AdditionalBeneficiaries) != null)
                        {
                            templateName = file.Templates.First(x => x.PropertyName == TemplatePropertyName.AdditionalBeneficiaries).Description;

                            try
                            {
                                beneficiaries = DelegateService.massiveService.CreateAdditionalBeneficiaries(file.Templates.FirstOrDefault(p => p.PropertyName == TemplatePropertyName.AdditionalBeneficiaries), cacheListForProperty.FilterIndividuals);

                                foreach (Beneficiary beneficiary in beneficiaries)
                                {
                                    if (beneficiary.Participation == 0)
                                    {
                                        massiveEmissionRow.HasError = true;
                                        massiveEmissionRow.Observations += Errors.ErrorBeneficiaryParticipation + KeySettings.ReportErrorSeparatorMessage();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                massiveEmissionRow.HasError = true;
                                massiveEmissionRow.Observations += string.Format(Errors.ErrorInTemplate, templateName, ex.InnerException.InnerException.Message) + KeySettings.ReportErrorSeparatorMessage();
                            }
                        }
                        templateName = file.Templates.First(x => x.PropertyName == TemplatePropertyName.EmissionProperty).Description;

                        companyProperty.Beneficiaries.AddRange(beneficiaries);

                        if (companyProperty.Beneficiaries.GroupBy(b => b.IndividualId, ben => ben).Select(b => b.First()).ToList().Count != companyProperty.Beneficiaries.Count)
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += Errors.ErrorBeneficiariesAdditionalDuplicated + KeySettings.ReportErrorSeparatorMessage();
                        }

                        decimal beneficiariesParticipation = beneficiaries.Sum(x => x.Participation);

                        if (beneficiariesParticipation < 100)
                        {
                            companyProperty.Beneficiaries[0].Participation -= beneficiariesParticipation;
                        }
                        else
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += Errors.ErrorParticipationBeneficiary + KeySettings.ReportErrorSeparatorMessage();
                        }

                        var principalRateTri = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRateTRI));

                        List<CompanyInsuredObject> insuredObjects = CreateInsuredObjects(file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.InsuredObjects), principalRateTri, massiveEmissionRow);

                        if (companyProperty.CompanyRisk == null)
                        {
                            companyProperty.CompanyRisk = new CompanyRisk();
                        }
                        if (insuredObjects.Count > 0)
                        {
                            companyProperty.CompanyRisk.CompanyCoverages =
                            DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdsGroupCoverageIdProductId(
                                insuredObjects.Select(x => x.Id).ToList(), companyProperty.GroupCoverage.Id, companyPolicy.CompanyProduct.Id, true);
                        }
                        else
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations = Errors.ErrorInsuredObjectsIsMandatories + KeySettings.ReportErrorSeparatorMessage();
                        }

                        if (companyProperty.CompanyRisk.CompanyCoverages != null)
                        {
                            companyProperty.CompanyRisk.CompanyCoverages.ForEach(x =>
                            {
                                x.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                                x.CurrentFrom = companyPolicy.CurrentFrom;
                                x.CurrentTo = companyPolicy.CurrentTo;
                                CompanyInsuredObject insuredObject = new CompanyInsuredObject();
                                if (x.CompanyInsuredObject == null || (insuredObject = insuredObjects.FirstOrDefault(y => y.Id == x.CompanyInsuredObject.Id)) == null)
                                {
                                    massiveEmissionRow.HasError = true;
                                    massiveEmissionRow.Observations += string.Format(Errors.ErrorMissingInsuredObjectForCoverage, x.Id) + KeySettings.ReportErrorSeparatorMessage();
                                }
                                x.CompanyInsuredObject = insuredObject;
                            });

                            //Deducibles
                            massiveEmissionRow = CreateDeductibles(companyProperty.CompanyRisk.CompanyCoverages, file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.ModifyCoverages), insuredObjects, massiveEmissionRow);

                        }

                        List<CommonModels.Nomenclature> nomencaltures = DelegateService.commonService.GetNomenclatures();
                        string fullAddress = DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskAddress)).ToString();
                        companyProperty.FullAddress = DelegateService.uniquePersonService.NormalizeAddress(fullAddress);

                        companyProperty.NomenclatureAddress = new Core.Application.Locations.Models.NomenclatureAddress
                        {
                            Type = new Core.Application.Locations.Models.RouteType
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

                        companyProperty.CompanyMicroZone = new CompanyMicroZone
                        {
                            Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskMicrozoningZone))
                        };

                        companyProperty.RiskActivity = new RiskActivity
                        {
                            Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskActivity))
                        };

                        companyProperty.RiskUse = new RiskUse
                        {
                            Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskUseCode))
                        };

                        companyProperty.IsDeclarative = (bool)DelegateService.commonService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskIsFacultative));

                        int cOfConstruction = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCOfConstruction));

                        if (cOfConstruction > 1)
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += string.Format((Errors.ErrorCOfConstruction + KeySettings.ReportErrorSeparatorMessage()), row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCOfConstruction).Description);
                        }
                        companyProperty.ConstructionType = new ConstructionType
                        {
                            Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCOfConstruction))
                        };

                        companyProperty.PML = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskEML));

                        companyProperty.Square = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskBlock));

                        companyProperty.Latitude = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLatitude));

                        companyProperty.Longitude = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLongitude));

                        companyProperty.ConstructionYear = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskConstructionYear));

                        companyProperty.FloorNumber = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskFloorNumber));
                        int maxFloorNumber = (int)DelegateService.commonService.GetExtendedParameterByParameterId((int)CompanyParameterType.MaxFloorNumber).NumberParameter;
                        int minFloorNumber = (int)DelegateService.commonService.GetExtendedParameterByParameterId((int)CompanyParameterType.MinFloorNumber).NumberParameter;

                        if (companyProperty.FloorNumber > maxFloorNumber)
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += Errors.ErrorTheNumberOfFloorsCanNotBeGreaterThan + " " + maxFloorNumber + KeySettings.ReportErrorSeparatorMessage();
                        }
                        else if (companyProperty.FloorNumber < minFloorNumber)
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations += Errors.ErrorTheNumberOfFloorsCanNotBeLessThan + " " + minFloorNumber + KeySettings.ReportErrorSeparatorMessage();
                        }

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

                        companyProperty.CompanyRisk.CompanyInsured.ClaimIncurred = new ClaimIncurred();
                        decimal? accidentRate = DelegateService.uniquePersonService.GetSinisterPercentageByInsuredId(companyProperty.CompanyRisk.CompanyInsured.InsuredId).AccidentRate;

                        if (accidentRate == null)
                        {
                            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskSinisterPercentage).Value))
                            {
                                companyProperty.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskSinisterPercentage));
                                if (companyProperty.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate < 0 || companyProperty.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate > 100)
                                {
                                    massiveEmissionRow.HasError = true;
                                    massiveEmissionRow.Observations += string.Format(Errors.ErrorAccidentRateValue) + KeySettings.ReportErrorSeparatorMessage();
                                }
                            }
                            else
                            {
                                massiveEmissionRow.HasError = true;
                                massiveEmissionRow.Observations += string.Format(Errors.AccidentRateMandatory) + KeySettings.ReportErrorSeparatorMessage();
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

                        Template templateClauses = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.Clauses);

                        if (templateClauses != null)
                        {
                            templateName = templateClauses.Description;
                            companyProperty.Clauses = new List<Clause>();
                            companyProperty.CompanyRisk.CompanyCoverages.ForEach(c => c.Clauses = new List<Clause>());

                            foreach (Row clausesRow in templateClauses.Rows)
                            {
                                int levelCode = (int)DelegateService.commonService.GetValueByField<int>(clausesRow.Fields.Find(p => p.PropertyName == FieldPropertyName.LevelCode));
                                int clauseCode = (int)DelegateService.commonService.GetValueByField<int>(clausesRow.Fields.Find(p => p.PropertyName == FieldPropertyName.ClauseCode));
                                if (levelCode == (int)EmissionLevel.Risk)
                                {
                                    if (riskClauses.Any(c => c.Id == clauseCode))
                                    {
                                        companyProperty.Clauses.Add(riskClauses.First(c => c.Id == clauseCode));
                                    }
                                    else
                                    {
                                        massiveEmissionRow.HasError = true;
                                        massiveEmissionRow.Observations += Errors.ErrorClauseNotFound + KeySettings.ReportErrorSeparatorMessage();
                                    }
                                }
                                else if (levelCode == (int)EmissionLevel.Coverage)
                                {
                                    if (coverageClauses.Any(a => a.Id == clauseCode))
                                    {
                                        Clause clauseToAdd = coverageClauses.First(c => c.Id == clauseCode);

                                        if (companyProperty.CompanyRisk.CompanyCoverages.Any(c => c.Id == clauseToAdd.ConditionLevel.ConditionValue))
                                        {
                                            companyProperty.CompanyRisk.CompanyCoverages.First(c => c.Id == clauseToAdd.ConditionLevel.ConditionValue).Clauses.Add(clauseToAdd);
                                        }
                                        else
                                        {
                                            massiveEmissionRow.HasError = true;
                                            massiveEmissionRow.Observations += string.Format(Errors.ClauseCoverageNotPresentOnRisk, clauseCode) + KeySettings.ReportErrorSeparatorMessage();
                                        }

                                    }
                                    else
                                    {
                                        massiveEmissionRow.HasError = true;
                                        massiveEmissionRow.Observations += string.Format(Errors.ClauseNotRelatedToCoverage, clauseCode) + KeySettings.ReportErrorSeparatorMessage();
                                    }
                                }
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
                                Operation = JsonConvert.SerializeObject(companyProperty)
                            };

                            DelegateService.commonService.CreatePendingOperation(pendingOperation);
                            DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);

                            /* Without Replicated Database */
                        }
                        else
                        {
                            /* with Replicated Database */
                            PendingOperation pendingOperationPolicy = new PendingOperation
                            {
                                UserId = companyPolicy.UserId,
                                Operation = JsonConvert.SerializeObject(companyPolicy)
                            };

                            PendingOperation pendingOperationRisk = new PendingOperation
                            {
                                UserId = companyPolicy.UserId,
                                Operation = JsonConvert.SerializeObject(companyProperty)
                            };

                            string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, JsonConvert.SerializeObject(pendingOperationRisk), (char)007, JsonConvert.SerializeObject(massiveEmissionRow), (char)007, nameof(MassiveEmissionRow));
                            var queue = new Core.Framework.Queues.BaseQueueFactory().CreateQueue("CreatePendingOperationQuee", routingKey: "CreatePendingOperationQuee", serialization: "JSON");
                            queue.PutOnQueue(pendingOperationJson);
                            /* with Replicated Database */
                        }

                        if (Settings.ImplementWebServices() &&
                            (companyPolicy.CompanyProduct.IsScore.GetValueOrDefault() && companyProperty.CompanyRisk.CompanyInsured.ScoreCredit == null))
                        {
                            CheckExternalServices(cacheListForProperty, companyPolicy, companyProperty, massiveEmission, file, row);
                        }
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

                massiveEmissionRow = DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void CheckExternalServices(CacheListForProperty cacheListForProperty, CompanyPolicy companyPolicy, CompanyPropertyRisk companyProperty, MassiveEmission massiveEmission, File file, Row row)
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
                DelegateService.externalProxyService.CheckExternalServices(filterIndividual.Person.IdentificationDocument, surname, filterIndividual.InsuredCode.Value, licencePlate, massiveEmission.Id, row.Number, (int)SubCoveredRiskType.Property, massiveEmission.User.UserId, scoreAlreadyQueried, simitAlreadyQueried, requireScore, requireSimit, requireFasecolda);
            }
        }

        private List<CompanyInsuredObject> CreateInsuredObjects(Template insuredObjectTemplate, decimal principalRateTri, MassiveEmissionRow massiveEmissionRow)
        {
            List<CompanyInsuredObject> insuredObjects = new List<CompanyInsuredObject>();

            if (insuredObjectTemplate != null)
            {
                //templateName = insuredObjectTemplate.Description;

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

                var consolidatedinsuredObject =
                   from p in insuredObjectTemplate.Rows
                   group p by new
                   {
                       Id = Convert.ToInt32(p.Fields.First(x => x.PropertyName == FieldPropertyName.InsuredObjectCode).Value)
                   }
                   into policies
                   where policies.Count() > 1
                   select new
                   {
                       Id = policies.Key.Id,
                       Total = policies.Count(),
                   };

                foreach (var item in consolidatedinsuredObject)
                {
                    if (insuredObjects.Select(i => i.Id).Distinct().Contains(item.Id))
                    {
                        massiveEmissionRow.HasError = true;
                        massiveEmissionRow.Observations += Errors.ErrorInsuredObjectDuplicated + item.Id + KeySettings.ReportErrorSeparatorMessage();
                    }
                }
                if (!insuredObjects.Any())
                {
                    throw new ValidationException(Errors.ErrorNoInsuredObjectsFound);
                }
            }
            else
            {
                throw new ValidationException(Errors.ErrorNoInsuredObjectsFound);
            }

            return insuredObjects;
        }

        private MassiveEmissionRow CreateDeductibles(List<CompanyCoverage> coverages, Template deductiblesTemplate, List<CompanyInsuredObject> insuredObject, MassiveEmissionRow massiveEmissionRow)
        {
            if (deductiblesTemplate != null)
            {
                foreach (Row row in deductiblesTemplate.Rows)
                {
                    int insuredObjectCode = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                    if (!insuredObject.Exists(c => c.Id == insuredObjectCode))
                    {
                        massiveEmissionRow.HasError = true;
                        massiveEmissionRow.Observations = Errors.TheInsuranceObjectInTheModificationIsNotRelatedToTheRisk + KeySettings.ReportErrorSeparatorMessage();
                        return massiveEmissionRow;
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
            return massiveEmissionRow;
        }

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
            concurrentRows = new ConcurrentBag<Row>();
            DelegateService.massiveService.LoadReportCacheList();
            LoadList(massiveEmission);
            List<MassiveEmissionRow> massiveEmissionRows = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveEmission.Id, processStatus, false, null);
            SubCoveredRiskType subCoveredRiskType = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveEmission.Prefix.Id, (int)massiveEmission.CoveredRiskType);

            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.MassiveReport;
            fileProcessValue.Key4 = massiveEmission.Prefix.Id;
            fileProcessValue.Key5 = (int)subCoveredRiskType;

            File file = DelegateService.commonService.GetFileByFileProcessValue(fileProcessValue);
            if (file == null)
            {
                return "";
            }
            else
            {
                file.Templates[0].Rows[0].Fields.ForEach(u => u.Value = "");
                string serializeFields = JsonConvert.SerializeObject(file.Templates[0].Rows[0].Fields);
                string key = Guid.NewGuid().ToString();
                string filePath = "";
                
                int bulkExcel = Convert.ToInt32(DelegateService.commonService.GetKeyApplication("MaxMassiveExcelRow"));

                file.FileType = FileType.CSV;

                TP.Parallel.ForEach(massiveEmissionRows,
                    (process) =>
                    {
                        string serialize = serializeFields;
                        FillPropertyFields(massiveEmission, process, serialize);

                        if (concurrentRows.Count >= bulkExcel || massiveEmissionRows.Count == 0)
                        {
                            file.Templates[0].Rows = concurrentRows.ToList().OrderBy(x => x.Number).ToList();
                            file.Name = "Reporte Hogar_" + key + "_" + massiveEmission.Id;
                            filePath = DelegateService.commonService.GenerateFile(file);
                            concurrentRows = new ConcurrentBag<Row>();
                        }
                    });

                return filePath;
            }
        }

        public void FillPropertyFields(MassiveEmission massiveEmission, MassiveEmissionRow proccess, string serializeFields)
        {
            List<CompanyPropertyRisk> risks;
            List<Field> fields;
            try
            {

                Policy policy = new Policy
                {
                    Endorsement = new Endorsement { EndorsementType = EndorsementType.Emission, Id = proccess.Risk.Policy.Endorsement.Id },
                    Branch = massiveEmission.Branch,
                    Prefix = massiveEmission.Prefix
                };
                if (proccess.Risk != null)
                {
                    policy.Id = proccess.Risk.Policy.Id;
                    policy.DocumentNumber = proccess.Risk.Policy.DocumentNumber;
                }
                CompanyPolicy companyPolicy = DelegateService.massiveService.GetCompanyPolicyByMassiveLoadStatusPolicy(massiveEmission.Status.Value, policy);
                if (companyPolicy != null)
                {
                    risks = GetCompanyPropertyRisk(massiveEmission, proccess, companyPolicy.Id);

                    fields = DelegateService.massiveService.GetFields(serializeFields, companyPolicy);
                    foreach (CompanyPropertyRisk property in risks)
                    {
                        fields = DelegateService.massiveService.FillInsuredFields(fields, property.CompanyRisk?.CompanyInsured);

                        fields.Find(u => u.PropertyName == FieldPropertyName.Identificator).Value = proccess.RowNumber.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.LoadId).Value = massiveEmission.Id.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.TotalInsuredValue).Value = property.AmountInsured.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskNumber).Value = "1";// property.Number.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskAddress).Value = CreateAddress(property);
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskFloorNumber).Value = property.FloorNumber.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskConstructionYear).Value = property.ConstructionYear.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskLongitude).Value = property.Longitude.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskLatitude).Value = property.Latitude.ToString();
                        fields.Find(u => u.PropertyName == FieldPropertyName.BeneficiaryData).Value = DelegateService.massiveService.CreateBeneficiaries(property.Beneficiaries);
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskUseDescription).Value = (property.RiskUse != null && property.RiskUse.Id > 0) ? riskUse.FirstOrDefault(u => u.Id == property.RiskUse.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.RiskTypeDescription).Value = (property.CompanyRiskTypeEarthquake != null && property.CompanyRiskTypeEarthquake.Id > 0) ? riskTypes.FirstOrDefault(u => u.Id == property.CompanyRiskTypeEarthquake.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(property.Clauses);

                        fields.Find(u => u.PropertyName == FieldPropertyName.RaitingZoneDescription).Value = (property.RatingZone != null && property.RatingZone.Id > 0) ? ratingZones.FirstOrDefault(u => u.Id == property.RatingZone.Id).Description : "";
                        fields.Find(u => u.PropertyName == FieldPropertyName.Observations).Value = "";

                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.StructureDescription).Value = (property.CompanyStructureType != null && property.CompanyStructureType.Id > 0) ? companyStructureTypes.FirstOrDefault(u => u.TypeCD == property.CompanyStructureType.Id).Description : ""; ;
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskIrregularHeightDescription).Value = (property.CompanyIrregularHeight != null) ? irregularHeights.FirstOrDefault(u => u.Id == property.CompanyIrregularHeight.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskIrregularPlantDescription).Value = (property.CompanyIrregularPlant != null) ? irregularPlants.FirstOrDefault(u => u.Id == property.CompanyIrregularPlant.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskRepairedDescripcion).Value = (property.CompanyRepair != null) ? companyRepairs.FirstOrDefault(u => u.Id == property.CompanyRepair.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskReinforcedStructureDescription).Value = (property.CompanyReinforcedStructureType != null) ? reinforcedStructureTypes.FirstOrDefault(u => u.Id == property.CompanyReinforcedStructureType.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskPreviousDamageDescription).Value = (property.CompanyDamage != null) ? damages.FirstOrDefault(u => u.Id == property.CompanyDamage.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskLocationDescription).Value = "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskNeighborhoodDescription).Value = "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskCOfConstructionDescription).Value = (property.ConstructionType != null && property.ConstructionType.Id > 0) ? constructionTypes.FirstOrDefault(u => u.Id == property.ConstructionType.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskMicrozoningZone).Value = (property.CompanyMicroZone != null && property.CompanyMicroZone.Id > 0) ? companyMicroZones.FirstOrDefault(u => u.Id == property.CompanyMicroZone.Id).Description : "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskSinisterPercentage).Value = property.CompanyRisk.CompanyInsured.ClaimIncurred.AccidentRate.ToString();

                        //Fecha de vigencia del riesgo
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.RiskCurrentFrom).Value = companyPolicy.CurrentFrom.ToString();

                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValue).Value = "";
                        fields.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValueRC).Value = "";

                        //Asistencia
                        List<int> assistanceCoveragesIds = DelegateService.underwritingService.GetAssistanceCoveragesIds(CompanyParameterType.AssistanceProperty);
                        List<CompanyCoverage> coveragesAssistance = property.CompanyRisk.CompanyCoverages.Where(u => assistanceCoveragesIds.Exists(id => id == u.Id)).ToList();
                        decimal assistancePremium = coveragesAssistance.Sum(x => x.PremiumAmount);
                        if (assistancePremium > 0)
                        {
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyPremium).Value =
                                (companyPolicy.Summary.Premium - assistancePremium).ToString("F2");
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyExpensesWithAssistance).Value =
                                (companyPolicy.Summary.Expenses + assistancePremium).ToString("F2");
                        }

                        serializeFields = JsonConvert.SerializeObject(fields);
                        decimal valueRc = 0, insuredValue = 0;
                        valueRc = property.CompanyRisk.CompanyCoverages.Where(u => u.Description.Contains("R.C.E")).Sum(u => u.LimitAmount);
                        insuredValue = (companyPolicy.Summary.AmountInsured - valueRc);

                        foreach (CompanyCoverage coverage in property.CompanyRisk.CompanyCoverages.OrderByDescending(u => u.Number))
                        {

                            fields = JsonConvert.DeserializeObject<List<Field>>(serializeFields);
                            fields.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValue).Value = insuredValue.ToString();
                            fields.Find(u => u.PropertyName == CompanyFieldPropertyName.InsuredValueRC).Value = valueRc.ToString();
                            if (coverage.SubLineBusiness != null && coverage.SubLineBusiness.LineBusiness != null)
                            {
                                fields.Find(u => u.PropertyName == FieldPropertyName.LineBusinessDescripcion).Value = lineBusiness.DefaultIfEmpty(new LineBusiness { Description = "" }).FirstOrDefault(u => u.Id == coverage.SubLineBusiness.LineBusiness.Id).Description;
                                fields.Find(u => u.PropertyName == FieldPropertyName.SubLineBusinessDescripcion).Value = subLineBusiness.DefaultIfEmpty(new SubLineBusiness { Description = "" }).FirstOrDefault(u => u.Id == coverage.SubLineBusiness.Id).Description;
                            }
                            else
                            {
                                fields.Find(u => u.PropertyName == FieldPropertyName.LineBusinessDescripcion).Value = "";
                                fields.Find(u => u.PropertyName == FieldPropertyName.SubLineBusinessDescripcion).Value = "";

                            }
                            fields.Find(u => u.PropertyName == FieldPropertyName.InsuredObjectDescription).Value = insuredObjects.DefaultIfEmpty(new CompanyInsuredObject { Description = "" }).FirstOrDefault(u => u.Id == coverage.CompanyInsuredObject.Id).Description;
                            fields.Find(u => u.PropertyName == FieldPropertyName.CoverageDescription).Value = coverage.Description;
                            fields.Find(u => u.PropertyName == FieldPropertyName.CoverageInsuredValue).Value = coverage.DeclaredAmount.ToString();
                            fields.Find(u => u.PropertyName == FieldPropertyName.CoverageRate).Value = coverage.Rate.ToString();
                            fields.Find(u => u.PropertyName == FieldPropertyName.CoveragePremium).Value = coverage.PremiumAmount.ToString();
                            fields.Find(u => u.PropertyName == FieldPropertyName.DeductibleDescription).Value = (coverage.Deductible != null && coverage.Deductible.Description != null) ? coverage.Deductible.Description : "";
                            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value += DelegateService.massiveService.CreateClauses(coverage.Clauses);
                            Row row = new Row { Fields = fields, Number = proccess.RowNumber };
                            concurrentRows.Add(row);

                        }

                    }
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                risks = null;
                serializeFields = null;
                fields = null;
                DataFacadeManager.Dispose();
            }


        }
        private List<CompanyPropertyRisk> GetCompanyPropertyRisk(MassiveEmission massiveEmission, MassiveEmissionRow proccess, int tempId)
        {
            List<CompanyPropertyRisk> companyProperties = new List<CompanyPropertyRisk>();
            switch (massiveEmission.Status)
            {
                case MassiveLoadStatus.Tariffed:
                    var pendingOperations = new List<PendingOperation>();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* Without Replicated Database */
                        pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(tempId);
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
                        companyProperties.Add(JsonConvert.DeserializeObject<CompanyPropertyRisk>(item.Operation));
                    }

                    break;
                case MassiveLoadStatus.Issued:
                    //companyProperties = DelegateService.propertyService.GetCompanyPropertyByPrefixBranchDocumentNumberEndorsementType(massiveEmission.Prefix.Id, massiveEmission.Branch.Id, proccess.Risk.Policy.DocumentNumber, EndorsementType.Emission);
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* without Replicated Database */
                        DelegateService.underwritingService.GetRiskByEndorsementDocumentNumber(proccess.Risk.Policy.Endorsement.Id).ForEach(x => companyProperties.Add(JsonConvert.DeserializeObject<CompanyPropertyRisk>(x)));
                    }
                    else
                    {
                        DelegateService.pendingOperationEntityService.GetRiskByEndorsementDocumentNumber(proccess.Risk.Policy.Endorsement.Id).ForEach(x => companyProperties.Add(JsonConvert.DeserializeObject<CompanyPropertyRisk>(x)));
                        /* with Replicated Database */

                    }
                    break;
            }
            return companyProperties;

        }
        private void LoadList(MassiveEmission massiveEmission)
        {
            if (documentTypes == null)
            {
                documentTypes = new List<DocumentType>();
                deductibles = new List<Deductible>();
                irregularHeights = new List<CompanyIrregularHeight>();
                irregularPlants = new List<CompanyIrregularPlant>();
                levelzones = new List<CompanyLevelZone>();
                locations = new List<CompanyLocation>();
                reinforcedStructureTypes = new List<CompanyReinforcedStructureType>();
                companyRepairs = new List<CompanyRepair>();
                companyStructureTypes = new List<CompanyStructureType>();
                riskUse = new List<RiskUse>();
                riskTypes = new List<CompanyRiskTypeEarthquake>();
                constructionTypes = new List<ConstructionType>();
                lineBusiness = new List<LineBusiness>();
                subLineBusiness = new List<SubLineBusiness>();
                insuredObjects = new List<CompanyInsuredObject>();
                cities = new List<City>();
                states = new List<State>();
                damages = new List<CompanyDamage>();
                ratingZones = new List<RatingZone>();
                companyMicroZones = new List<CompanyMicroZone>();

                documentTypes = (List<DocumentType>)DelegateService.massiveService.GetCacheList("documentTypes_", "Report");
                deductibles = (List<Deductible>)DelegateService.massiveService.GetCacheList("deductibles_", "Report");
                irregularHeights = DelegateService.propertyService.GetCompanyIrregularHeights();
                irregularPlants = DelegateService.propertyService.GetCompanyIrregularPlants();
                levelzones = new List<CompanyLevelZone>();
                locations = new List<CompanyLocation>();
                reinforcedStructureTypes = DelegateService.propertyService.GetCompanyReinforcedStructureTypes();
                companyRepairs = DelegateService.propertyService.GetCompanyRepairs();
                companyStructureTypes = DelegateService.propertyService.GetCompanyStructureTypes();
                riskUse = DelegateService.propertyService.GetRiskUses();
                constructionTypes = DelegateService.propertyService.GetConstructionTypes();
                lineBusiness = DelegateService.commonService.GetLinesBusiness();
                subLineBusiness = DelegateService.commonService.GetSubLineBusiness();
                insuredObjects = DelegateService.underwritingService.GetCompanyInsuredObjects();
                cities = DelegateService.commonService.GetCities();
                states = DelegateService.commonService.GetStates();
                riskTypes = DelegateService.propertyService.GetCompanyRiskTypeEarthquakes();
                damages = DelegateService.propertyService.GetCompanyDamages();
                ratingZones = DelegateService.commonService.GetRatingZones();
                companyMicroZones = DelegateService.propertyService.GetCompanyMicroZones();
            }

        }
        private string CreateAddress(CompanyPropertyRisk propertyRisk)
        {
            string address = "";
            address += propertyRisk.FullAddress;

            if (propertyRisk.City != null && propertyRisk.City.State != null)
            {
                address += StringHelper.ConcatenateString(" | ",
                    states.DefaultIfEmpty(new State { Description = "" }).FirstOrDefault(u => u.Id == propertyRisk.City.State.Id).Description,
                    " | ",
                    cities.DefaultIfEmpty(new City { Description = "" }).FirstOrDefault(u => u.Id == propertyRisk.City.Id && u.State.Id == propertyRisk.City.State.Id).Description);
            }

            return address;

        }

        public MassiveLoad IssuanceMassiveEmission(MassiveLoad massiveLoad)
        {
            if (massiveLoad != null)
            {
                List<MassiveEmissionRow> massiveEmissionRows = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoad.Id, MassiveLoadProcessStatus.Events, false, false);

                if (massiveEmissionRows.Count > 0)
                {
                    TP.Task.Run(() => IssuanceMassiveEmissionRows(massiveLoad, massiveEmissionRows));
                }
                else
                {
                    massiveLoad.HasError = true;
                    massiveLoad.ErrorDescription = Errors.ErrorRecordsNotFoundToIssue;
                }
            }

            return massiveLoad;
        }

        public void IssuanceMassiveEmissionRows(MassiveLoad massiveLoad, List<MassiveEmissionRow> massiveEmissionRows)
        {
            try
            {
                massiveLoad.Status = MassiveLoadStatus.Issuing;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);

                List<CompanyPolicy> companyPolicies = DelegateService.massiveUnderwritingService.GetCompanyPoliciesToIssueByOperationIds(massiveEmissionRows.Where(x => !x.HasError).Select(x => x.Risk.Policy.Id).ToList());

                ParallelHelper.ForEach(companyPolicies, companyPolicy =>
                {
                    MassiveEmissionRow massiveEmissionRow = massiveEmissionRows.FirstOrDefault(x => x.Risk.Policy.Id == companyPolicy.Id);

                    massiveEmissionRow.Status = MassiveLoadProcessStatus.Issuance;
                    DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
                    //Fecha de emisión
                    companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);

                    PendingOperation pendingOperationPolicy = new PendingOperation();
                    pendingOperationPolicy.Id = companyPolicy.Id;
                    pendingOperationPolicy.UserId = massiveLoad.User.UserId;
                    pendingOperationPolicy.Operation = JsonConvert.SerializeObject(companyPolicy);

                    string issuanceJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, JsonConvert.SerializeObject(massiveEmissionRow), (char)007, nameof(CompanyPropertyRisk), (char)007, nameof(MassiveEmissionRow));
                    var queue = new Core.Framework.Queues.BaseQueueFactory().CreateQueue("CreatePolicyQuee", routingKey: "CreatePolicyQuee", serialization: "JSON");
                    queue.PutOnQueue(issuanceJson);
                });

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", ex.ToString(), EventLogEntryType.Error);
                massiveLoad.ErrorDescription = string.Format(Errors.ErrorIssuing, ex.Message);
                massiveLoad.Status = MassiveLoadStatus.Issued;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
            }
        }

        private void ExecuteCreatePolicy(CompanyPolicy companyPolicy, MassiveEmissionRow massiveEmissionRow)
        {
            try
            {
                massiveEmissionRow.Status = MassiveLoadProcessStatus.Issuance;

                if (massiveEmissionRow.Risk.Policy.Id > 0)
                {
                    massiveEmissionRow.Status = MassiveLoadProcessStatus.Finalized;

                    List<PendingOperation> pendingOperations;
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(massiveEmissionRow.Risk.Policy.Id);
                    }
                    else
                    {
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(massiveEmissionRow.Risk.Policy.Id);
                    }
                    List<CompanyPropertyRisk> companyPropertyRisks = new List<CompanyPropertyRisk>();
                    foreach (PendingOperation po in pendingOperations)
                    {
                        CompanyPropertyRisk companyPropertyRisk = JsonConvert.DeserializeObject<CompanyPropertyRisk>(po.Operation);
                        companyPropertyRisk.CompanyPolicy = companyPolicy;
                        companyPropertyRisks.Add(companyPropertyRisk);
                    }
                    companyPolicy = DelegateService.propertyService.CreateEndorsement(companyPolicy, companyPropertyRisks);
                    UpdateJSONPolicyAndRecordEndorsementOperation(massiveEmissionRow, companyPolicy);
                    massiveEmissionRow.Risk.Policy.DocumentNumber = companyPolicy.DocumentNumber;
                    massiveEmissionRow.Risk.Policy.Endorsement.Id = companyPolicy.Endorsement.Id;

                }
                else
                {
                    massiveEmissionRow.HasError = true;
                    massiveEmissionRow.Observations = Errors.ErrorTemporalNotFound + KeySettings.ReportErrorSeparatorMessage();
                }
                DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
            }
            catch (Exception ex)
            {
                massiveEmissionRow.HasError = true;
                massiveEmissionRow.Observations = string.Format(Errors.ErrorIssuing, ex.Message);
                DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        public void ProcessResponseFromScoreService(string response)
        {
            int massiveLoadId = DelegateService.externalProxyService.UpdateExternalFilterByInsuredResponse(response, true, false, false, false);
            UpdatePolicyWithExternaldata(massiveLoadId);
        }

        private void UpdatePolicyWithExternaldata(int massiveLoadId)
        {
            List<ExternalFilterResult> externalFilterResults = DelegateService.externalProxyService.GetFullExternalFilterRecords(massiveLoadId);
            if (externalFilterResults != null && externalFilterResults.Count() > 0)
            {
                foreach (ExternalFilterResult externalFilterResult in externalFilterResults)
                {
                    int pendingOperationId = DelegateService.massiveService.GetpendingOperationIdByMassiveLoadIdRowId(externalFilterResult.MassiveLoadId, externalFilterResult.RowId);

                    var pendingOperations = new List<PendingOperation>();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* Without Replicated Database */
                        pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(pendingOperationId);
                        /* Without Replicated Database */
                    }
                    else
                    {
                        /* with Replicated Database */
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(pendingOperationId);
                        /* with Replicated Database */
                    }

                    if (!pendingOperations.Any()) /* Colectivas */
                    {
                        if (!Settings.UseReplicatedDatabase())
                        {
                            /* Without Replicated Database */
                            pendingOperations.Add(DelegateService.commonService.GetPendingOperationById(pendingOperationId));
                            /* Without Replicated Database */
                        }
                        else
                        {
                            /* with Replicated Database */
                            pendingOperations.Add(DelegateService.pendingOperationEntityService.GetPendingOperationById(pendingOperationId));
                            /* with Replicated Database */
                        }
                    }

                    CompanyPropertyRisk companyPropertyRisk = new CompanyPropertyRisk();
                    companyPropertyRisk = JsonConvert.DeserializeObject<CompanyPropertyRisk>(pendingOperations.First().Operation);
                    companyPropertyRisk.Id = pendingOperations.First().Id;

                    if (externalFilterResult.RequireScore && externalFilterResult.ScoreCredit != null)
                    {
                        companyPropertyRisk.CompanyRisk.CompanyInsured.ScoreCredit = externalFilterResult.ScoreCredit;
                    }

                }

                DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(massiveLoadId);
            }
        }


        private void UpdateJSONPolicyAndRecordEndorsementOperation(MassiveEmissionRow massiveEmissionRow, CompanyPolicy companyPolicy)
        {
            var pendingOperation = new PendingOperation();
            if (!Settings.UseReplicatedDatabase())
            {
                /* Without Replicated Database */
                pendingOperation = DelegateService.commonService.GetPendingOperationById(massiveEmissionRow.Risk.Policy.Id);
                pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                DelegateService.underwritingService.RecordEndorsementOperation(companyPolicy.Endorsement.Id, massiveEmissionRow.Risk.Policy.Id);
                /* Without Replicated Database */
            }
            else
            {
                /* with Replicated Database */
                pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(massiveEmissionRow.Risk.Policy.Id);
                PendingOperation pendingOperationPolicy = new PendingOperation
                {
                    UserId = companyPolicy.UserId,
                    Operation = JsonConvert.SerializeObject(companyPolicy),
                    Id = companyPolicy.Id
                };

                string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, companyPolicy.Endorsement.Id, (char)007, companyPolicy.Id, (char)007, nameof(MassiveEmissionRow));
                var queue = new Core.Framework.Queues.BaseQueueFactory().CreateQueue("UpdatePendingOperationQuee", routingKey: "UpdatePendingOperationQuee", serialization: "JSON");
                queue.PutOnQueue(pendingOperationJson);
                /* with Replicated Database */
            }

        }

    }
}
