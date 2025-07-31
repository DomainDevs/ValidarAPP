using AutoMapper;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Assemblers;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.BusinessModels;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.DAOs;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using CiaPersonModel = Sistran.Company.Application.UniquePersonServices.V1.Models;
using TM = System.Threading.Tasks;
using Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

/// <summary>
/// 
/// </summary>
namespace Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]

    public class ThirdPartyLiabilityServiceEEProvider : Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.ThirdPartyLiabilityServiceEEProvider, IThirdPartyLiabilityService
    {
        /// <summary>
        /// Ejecutar reglas pre de riesgo
        /// </summary>
        /// <param name="companyTplRisk">riesgo de rcp</param>
        /// <param name="ruleSetId">id de regla</param>
        /// <returns>modelo de riesgo</returns>

        public CompanyTplRisk RunRulesRisk(CompanyTplRisk companyTplRisk, int ruleSetId)
        {
            try
            {
                ThirdPartyLiabilityBusiness thirdPartyLiabilityBusiness = new ThirdPartyLiabilityBusiness();
                return thirdPartyLiabilityBusiness.RunRulesRisk(companyTplRisk, ruleSetId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Ejecutar reglas de cobertura
        /// </summary>
        /// <param name="coverage">Cobertura</param>
        /// <param name="ruleSetId">Id regla</param>
        /// <returns>Cobertura</returns>
        public CompanyCoverage RunRulesCompanyCoverage(CompanyTplRisk companyTplRisk, CompanyCoverage coverage, int ruleId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.RunRulesCoverage(companyTplRisk, coverage, ruleId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Tarifacion de poliza
        /// </summary>
        /// <param name="companyTplPolicy">poliza de rcp</param>
        /// <param name="runRulesPre">ejecutar reglas pre</param>
        /// <param name="runRulesPost">ejecutar reglas post</param>
        /// <returns>Modelo de poliza rcp</returns>
        public CompanyTplRisk QuotateThirdPartyLiability(CompanyTplRisk companyTplRisk, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                ThirdPartyLiabilityBusiness thirdPartyLiabilityBusiness = new ThirdPartyLiabilityBusiness();
                return thirdPartyLiabilityBusiness.QuotateThirdPartyLiability(companyTplRisk, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Tarifacion de riesgo
        /// </summary>
        /// <param name="companyTplRisk">modelo de riesgo rcp</param>
        /// <param name="runRulesPre">ejecutar reglas pre</param>
        /// <param name="runRulesPost">ejecutar reglas post</param>
        /// <returns>modelo de riesgo rcp</returns>
        public List<CompanyTplRisk> QuotateThirdPartyLiabilities(CompanyPolicy companyPolicy, List<CompanyTplRisk> companyTplRisks, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                ThirdPartyLiabilityBusiness thirdPartyLiabilityBusiness = new ThirdPartyLiabilityBusiness();
                return thirdPartyLiabilityBusiness.QuotateThirdPartyLiabilities(companyPolicy, companyTplRisks, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Tarifacion de cobetura
        /// </summary>
        /// <param name="coverage">modelo de cobertura</param>
        /// <param name="runRulesPre">ejecutar reglas pre</param>
        /// <param name="runRulesPost">ejecutar reglas post</param>
        /// <returns>modelo cobertura</returns>
        public CompanyCoverage QuotateCompanyCoverage(CompanyTplRisk companyTplRisk, CompanyCoverage coverage, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.Quotate(companyTplRisk, coverage, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="temporalId">Id temporal</param>
        /// <param name="companyTplRisk">Modelo companyTplRisk</param>
        public CompanyTplRisk CreateThirdPartyLiabilityTemporal(CompanyTplRisk companyTplRisk, bool isMassive)
        {
            try
            {
                ThirdPartyLiabilityDAO thirdPartyLiabilityDAO = new ThirdPartyLiabilityDAO();
                companyTplRisk = thirdPartyLiabilityDAO.CreateThirdPartyLiabilityTemporal(companyTplRisk, isMassive);
                
                if (companyTplRisk.Risk.Policy != null && companyTplRisk.Risk.Policy.Endorsement.QuotationId == 0)
                {
                    //Se agrega validación para generar la impresión correctamente, 
                    //dado que varios metodos llegan a este para actualizar el temporal, se implementa acá
                    if (companyTplRisk.Risk.LimitRc == null)
                    {
                        companyTplRisk.Risk.LimitRc = new CompanyLimitRc
                        {
                            Id = DelegateService.commonService.GetParameterByParameterId(3000).NumberParameter.Value,
                            LimitSum = DelegateService.commonService.GetParameterByParameterId(3001).NumberParameter.Value,
                        };
                    }

                }
                return companyTplRisk;

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Poliza de RC pasajeros
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>CompanyTplPolicy</returns>
        public List<CompanyTplRisk> GetCompanyThirdPartyLiabilitiesByPolicyId(int policyId)
        {
            try
            {
                ThirdPartyLiabilityDAO thirdPartyLiabilityDAO = new ThirdPartyLiabilityDAO();
                return thirdPartyLiabilityDAO.GetCompanyThirdPartyLiabilitiesByPolicyId(policyId);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Poliza de RC pasajeros
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>CompanyTplPolicy</returns>
        public List<CompanyTplRisk> GetThirdPartyLiabilityPolicyByPolicyIdEndorsementIdlicensePlate(int policyId, int endorsementId, string licensePlate)
        {
            try
            {
                ThirdPartyLiabilityDAO thirdPartyLiabilityDAO = new ThirdPartyLiabilityDAO();
                return thirdPartyLiabilityDAO.GetThirdPartyLiabilityPolicyByPolicyIdEndorsementIdLicensePlate(policyId, endorsementId, licensePlate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public int GetCountThirdPartyLiabilityPolicyByPolicyId(int policyId)
        {
            try
            {
                ThirdPartyLiabilityDAO thirdPartyLiabilityDAO = new ThirdPartyLiabilityDAO();
                return thirdPartyLiabilityDAO.GetCountThirdPartyLiabilityPolicyByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public ServiceType GetServiceTypeByServiceTypeId(int serviceTypeId)
        {
            try
            {
                var tplDAO = new ThirdPartyLiabilityDAO();
                return tplDAO.GetServiceTypeByServiceTypeId(serviceTypeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public bool ValidateThirdPartyLiabilityCorrelativePolicy(int prefixId, int branchId, decimal correlativePolicyNumber, int productId, string licensePlate)
        {
            try
            {
                var dao = new ThirdPartyLiabilityDAO();
                return dao.ValidateThirdPartyLiabilityCorrelativePolicy(prefixId, branchId, correlativePolicyNumber, productId, licensePlate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="endorsementId">Id Endoso</param>
        /// <returns>Riesgos</returns>
        public List<CompanyTplRisk> GetCompanyThirdPartyLiabilitiesByEndorsementId(int endorsementId)
        {
            try
            {
                ThirdPartyLiabilityDAO thirdPartyLiabilityDAO = new ThirdPartyLiabilityDAO();
                return thirdPartyLiabilityDAO.GetCompanyThirdPartyLiabilitiesByEndorsementId(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, "Error al consultar riesgos"), ex);
            }
        }

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Riesgos</returns>
        public List<CompanyTplRisk> GetThirdPartyLiabilitiesByTemporalId(int temporalId)
        {
            try
            {
                ThirdPartyLiabilityDAO thirdPartyLiabilityDAO = new ThirdPartyLiabilityDAO();
                return thirdPartyLiabilityDAO.GetThirdPartyLiabilitiesByTemporalId(temporalId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, "Error al consultar riesgos"), ex);
            }
        }

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyTplRisk> companyTplRisks)
        {
            try
            {
                ThirdPartyLiabilityDAO thirdPartyLiabilityDAO = new ThirdPartyLiabilityDAO();
                return thirdPartyLiabilityDAO.CreateEndorsement(companyPolicy, companyTplRisks);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error creando endoso", ex);
            }
        }

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        public CompanyTplRisk GetCompanyTplRiskByRiskId(int riskId)
        {
            try
            {
                ThirdPartyLiabilityDAO thirdPartyLiabilityDAO = new ThirdPartyLiabilityDAO();
                return thirdPartyLiabilityDAO.GetCompanyTplRiskByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #region emision

        public CompanyCoverage ExcludeCompanyCoverage(int temporalId, int riskId, int riskCoverageId, string description)
        {
            try
            {
                CompanyPolicy thirdPartyLiabilityPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                CompanyTplRisk thirdPartyLiability = GetCompanyTplRiskByRiskId(riskId);

                CompanyCoverage coverage = DelegateService.underwritingService.GetCompanyCoverageByRiskCoverageId(riskCoverageId);
                coverage.Description = description;
                coverage.SubLineBusiness = thirdPartyLiability.Risk.Coverages.First(x => x.RiskCoverageId == riskCoverageId).SubLineBusiness;

                coverage.Rate = coverage.Rate * -1;
                coverage.AccumulatedPremiumAmount = 0;
                coverage.EndorsementType = thirdPartyLiabilityPolicy.Endorsement.EndorsementType;
                coverage.CurrentFrom = thirdPartyLiabilityPolicy.CurrentFrom;

                thirdPartyLiability.Risk.Coverages = new List<CompanyCoverage>();
                thirdPartyLiability.Risk.Coverages.Add(coverage);


                coverage = QuotateCompanyCoverage(thirdPartyLiability, coverage, false, false);

                coverage.CoverStatus = CoverageStatusType.Excluded;
                coverage.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Excluded);
                coverage.LimitAmount = 0;
                coverage.SubLimitAmount = 0;
                coverage.EndorsementLimitAmount = coverage.EndorsementLimitAmount * -1;
                coverage.EndorsementSublimitAmount = coverage.EndorsementSublimitAmount * -1;

                return coverage;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public Boolean SaveCompanyCoverages(int temporalId, int riskId, List<CompanyCoverage> coverages)
        {
            try
            {
                if (coverages != null)
                {
                    CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                    companyPolicy.IsPersisted = true;
                    CompanyTplRisk companyTplRisk = GetCompanyTplRiskByRiskId(riskId);
                    if (companyPolicy != null && companyTplRisk != null && coverages != null)
                    {
                        //Policy policyCore = new Policy();
                        //Mapper.CreateMap(companyPolicy.GetType(), policyCore.GetType());
                        //Mapper.Map(companyPolicy, policyCore);
                        companyTplRisk.Risk.IsPersisted = true;
                        companyTplRisk.Risk.Id = riskId;
                        companyTplRisk.Risk.Coverages = coverages;
                        companyTplRisk.Risk.Policy = companyPolicy;
                        companyTplRisk = QuotateThirdPartyLiability(companyTplRisk, false, true);

                        companyTplRisk = CreateThirdPartyLiabilityTemporal(companyTplRisk, false);
                        return true;

                    }
                    else
                    {
                        throw new BusinessException(Errors.NoExistTemporaryNoHaveCoverages);
                    }

                }
                return false;

            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, "Error al Guardar Coberturas"), ex);
            }
        }

        public CompanyPolicy UpdateCompanyRisks(int temporalId)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                List<CompanyTplRisk> companyTplRisks = GetThirdPartyLiabilitiesByTemporalId(temporalId);


                if (companyTplRisks != null && companyTplRisks.Any())
                {
                    foreach (CompanyTplRisk companyTplRisk in companyTplRisks)
                    {
                        companyTplRisk.Risk.Policy = companyPolicy;
                        companyTplRisk.Risk.Coverages.ForEach(x => x.CurrentTo = companyPolicy.CurrentTo);
                        companyTplRisk.Risk.Coverages.ForEach(x => x.CurrentFrom = companyPolicy.CurrentFrom);

                        QuotateThirdPartyLiability(companyTplRisk, true, true);
                        CompanySaveCompanyTPLTemporal(companyTplRisk, false);
                    }
                    companyPolicy = DelegateService.underwritingService.UpdatePolicyComponents(companyPolicy.Id);
                    return companyPolicy;
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTemporaryNotHaveRisks);
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    throw new BusinessException(Errors.ErrorTemporaryNotHaveRisks);
                }
                throw new Exception(Errors.ErrorUpdatePolicy);
            }


        }
        public string ExistCompanyRiskByTemporalId(int tempId)
        {
            try
            {
                var message = "";
                CompanyPolicy thirdPartyLiabilityPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(tempId, false);

                if (thirdPartyLiabilityPolicy != null)
                {
                    List<CompanyTplRisk> thirdPartyLiabilities = GetThirdPartyLiabilitiesByTemporalId(tempId);

                    if (thirdPartyLiabilities != null)
                    {
                        foreach (CompanyTplRisk risk in thirdPartyLiabilities)
                        {
                            message = ExistsRiskByLicensePlateEngineNumberChassisNumberProductId(risk.LicensePlate, risk.EngineSerial, risk.ChassisSerial, thirdPartyLiabilityPolicy.Product.Id, thirdPartyLiabilityPolicy.Endorsement.Id, thirdPartyLiabilityPolicy.CurrentFrom);
                            if (message != "")
                            {
                                return message;
                            }
                        }

                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorTemporalNotFound);
                    }
                    return message;
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTemporalNotFound);
                }
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.WithoutEngineChassis);
            }
        }
        public CompanyTplRisk GetCompanyRiskById(EndorsementType endorsementType, int temporalId, int id)
        {
            try
            {
                CompanyTplRisk risk = GetCompanyTplRiskByRiskId(id);

                if (risk != null)
                {

                    CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                    if (risk.Risk.Beneficiaries == null)
                    {
                        risk.Risk.Beneficiaries = companyPolicy.DefaultBeneficiaries;
                    }
                    risk = GetRiskDescriptions((EndorsementType)companyPolicy.Endorsement.EndorsementType, risk);
                    return risk;
                }
                else
                {
                    throw new BusinessException(Errors.NoRiskWasFound);

                }
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                throw new BusinessException(Errors.NoRiskWasFound);
            }

        }
        private CompanyTplRisk GetRiskDescriptions(EndorsementType endorsementType, CompanyTplRisk risk)
        {
            switch (endorsementType)
            {
                case EndorsementType.Emission:
                    risk = GetDataEmission(risk);
                    break;
                default:
                    break;
            }

            return risk;
        }
        private CompanyTplRisk GetDataEmission(CompanyTplRisk risk)
        {

            if (risk?.Risk?.MainInsured?.IdentificationDocument == null)
            {
                risk.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(risk.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, risk.Risk.MainInsured.CustomerType);
                risk.Risk.MainInsured.Name = risk.Risk.MainInsured.Surname + " " + (string.IsNullOrEmpty(risk.Risk.MainInsured.SecondSurname) ? "" : risk.Risk.MainInsured.SecondSurname + " ") + risk.Risk.MainInsured.Name;
            }

            if (risk?.Risk?.Beneficiaries != null && risk.Risk.Beneficiaries.Count > 0 && risk.Risk.Beneficiaries[0].IdentificationDocument == null)
            {
                List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();

                foreach (CompanyBeneficiary item in risk.Risk.Beneficiaries)
                {
                    var config = MapperCache.GetMapper<Beneficiary, CompanyBeneficiary>(cfg =>
                    {
                        cfg.CreateMap<Beneficiary, CompanyBeneficiary>();
                        cfg.CreateMap<BeneficiaryType, CompanyBeneficiaryType>();
                    });

                    beneficiaries.Add(config.Map<Beneficiary, CompanyBeneficiary>(DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault()));
                }

                risk.Risk.Beneficiaries = beneficiaries;
            }

            foreach (CompanyCoverage item in risk.Risk.Coverages)
            {
                if (item.CoverStatusName == null)
                {
                    item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original);
                }
            }

            return risk;
        }

        public CompanyTplRisk GetCompanyPremium(int policyId, CompanyTplRisk ThirdPartyLiability)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);


                if (companyPolicy != null)
                {
                    companyPolicy.IsPersisted = true;

                    foreach (CompanyCoverage item in ThirdPartyLiability.Risk.Coverages)
                    {
                        item.CurrentFrom = companyPolicy.CurrentFrom;
                        item.CurrentTo = companyPolicy.CurrentTo;
                    }
                    if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Emission || companyPolicy.Endorsement.EndorsementType == EndorsementType.Renewal)
                    {
                        ThirdPartyLiability.Risk.Status = RiskStatusType.Original;
                    }
                    else if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Modification && ThirdPartyLiability.Risk.RiskId == 0)
                    {
                        ThirdPartyLiability.Risk.Status = RiskStatusType.Included;
                    }
                    else
                    {
                        ThirdPartyLiability.Risk.Status = RiskStatusType.Modified;
                    }
                    ThirdPartyLiability.Risk.Policy = companyPolicy;
                    ThirdPartyLiability = QuotateThirdPartyLiability(ThirdPartyLiability, true, true);

                    foreach (CompanyCoverage item in ThirdPartyLiability.Risk.Coverages)
                    {
                        if (item.CoverStatus == CoverageStatusType.NotModified)
                        {
                            if (item.EndorsementSublimitAmount > 0)
                            {
                                item.CoverStatus = CoverageStatusType.Modified;
                            }
                        }

                        item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(item.CoverStatus.Value);
                    }
                }
                return ThirdPartyLiability;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.GetBaseException().Message);
            }
        }

        public CompanyTplRisk SaveCompanyRisk(int temporalId, CompanyTplRisk thirdPartyLiability)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (policy != null)
                {
                    List<CompanyTplRisk> CompanyTplRisks = GetThirdPartyLiabilitiesByTemporalId(temporalId);
                    thirdPartyLiability.Alerts = ExistsRisk(CompanyTplRisks, thirdPartyLiability.Risk.Id, thirdPartyLiability.LicensePlate, thirdPartyLiability.EngineSerial, thirdPartyLiability.ChassisSerial, policy);
                    if (!thirdPartyLiability.Alerts.Any() || (policy.TemporalType == TemporalType.TempQuotation))
                    {
                        if (thirdPartyLiability != null && thirdPartyLiability.Risk != null)
                        {
                            if (policy.Endorsement.EndorsementType == EndorsementType.Modification)
                            {
                                thirdPartyLiability.Risk.Status = RiskStatusType.Included;
                            }
                            if (thirdPartyLiability.Risk?.Id == 0)
                            {
                                if (CompanyTplRisks.Count < policy.Product.CoveredRisk.MaxRiskQuantity)
                                {
                                    if (CompanyTplRisks.Count > 0)
                                    {
                                        thirdPartyLiability.Risk.Number = CompanyTplRisks.Where(x => x.Risk.Number != 0).Count() + 1;
                                    }
                                    else
                                    {
                                        thirdPartyLiability.Risk.Number = 1;
                                    }
                                    if (policy.DefaultBeneficiaries != null && policy.DefaultBeneficiaries.Count > 0)
                                    {
                                        thirdPartyLiability.Risk.Beneficiaries = policy.DefaultBeneficiaries;
                                    }
                                    else
                                    {
                                        ModelAssembler.CreateMapCompanyInsured();
                                        if (thirdPartyLiability.Risk.Beneficiaries == null)
                                        {
                                            thirdPartyLiability.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                                        }

                                        thirdPartyLiability.Risk.Beneficiaries.Add(ModelAssembler.CreateBeneficiaryFromInsured(thirdPartyLiability.Risk.MainInsured));
                                    }
                                }
                                else
                                {
                                    throw new BusinessException(Errors.ProductNotAddingMoreRisks);
                                }
                            }
                            else
                            {
                                switch (policy.Endorsement.EndorsementType.Value)
                                {
                                    case EndorsementType.Emission:
                                    case EndorsementType.Renewal:
                                        thirdPartyLiability = SetDataEmission(thirdPartyLiability);
                                        break;
                                    case EndorsementType.Modification:
                                        thirdPartyLiability = SetDataModification(thirdPartyLiability);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            thirdPartyLiability.Risk.Policy = policy;
                            thirdPartyLiability = CreateThirdPartyLiabilityTemporal(thirdPartyLiability, false);
                            return thirdPartyLiability;
                        }
                        else
                        {
                            throw new BusinessException(Errors.ErrorSaveRiskVehicle);
                        }
                    }
                    else
                    {
                        throw new BusinessException(String.Join(" : ", thirdPartyLiability.Alerts));

                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTemporalNotFound);
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    throw new BusinessException(ex.Message);
                }
                else
                {
                    throw new Exception(Errors.ErrorSaveRiskVehicle);
                }
            }
        }

        public List<string> ExistsRisk(List<CompanyTplRisk> CompanyTplRisks, int riskId, string licensePlate, string engineNumber, string chassisNumber, CompanyPolicy policy)
        {
            try
            {
                var endorsementType = policy.Endorsement.EndorsementType.Value;
                ConcurrentBag<string> messages = new ConcurrentBag<string>();
                string message = "";
                if (endorsementType == EndorsementType.Emission || endorsementType == EndorsementType.Renewal || endorsementType == EndorsementType.Modification)
                {
                    message = ExistsRiskByLicensePlateEngineNumberChassisNumberProductId(licensePlate, engineNumber, chassisNumber, policy.Product.Id, policy.Endorsement.Id, policy.CurrentFrom);

                    if (message != "")
                    {
                        messages.Add(message);
                        return messages.ToList();
                    }
                }

                var vehicleEngine = (riskId > 0) ? CompanyTplRisks.SingleOrDefault(x => x.EngineSerial == engineNumber && x.Risk.Id != riskId) : CompanyTplRisks.SingleOrDefault(x => x.EngineSerial == engineNumber);
                if (vehicleEngine != null)
                {
                    messages.Add(CreateMessageExist(vehicleEngine, policy));
                    return messages.ToList();
                }

                var vehiclePlate = (riskId > 0) ? CompanyTplRisks.SingleOrDefault(x => x.LicensePlate == licensePlate && x.Risk.Id != riskId) : CompanyTplRisks.SingleOrDefault(x => x.LicensePlate == licensePlate);
                if (vehiclePlate != null)
                {
                    messages.Add(CreateMessageExist(vehiclePlate, policy));
                    return messages.ToList();
                }

                var vehicleChasis = (riskId > 0) ? CompanyTplRisks.SingleOrDefault(x => x.ChassisSerial == chassisNumber && x.Risk.Id != riskId) : CompanyTplRisks.SingleOrDefault(x => x.ChassisSerial == chassisNumber);
                if (vehicleChasis != null)
                {
                    messages.Add(CreateMessageExist(vehicleChasis, policy));
                    return messages.ToList();
                }

                return messages.ToList();

            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }

        }
        public string CreateMessageExist(CompanyTplRisk thirdPartyLiability, CompanyPolicy policy)
        {
            return $@"**El vehiculo ya posee una poliza vigente. 

                       Vehiculo:
                       Placa = {thirdPartyLiability.LicensePlate} 
                       Número de chasis = {thirdPartyLiability.ChassisSerial} 
                       Número de motor = {thirdPartyLiability.EngineSerial}
                       Sucursal = {policy.Branch.Id}
                       Producto = {policy.Product.Description}
                       Nro Poliza = {policy.DocumentNumber} 
                       Fecha de inicio = {policy.CurrentFrom } 
                       Fecha de fin = {policy.CurrentTo}.";
        }
        private CompanyTplRisk SetDataEmission(CompanyTplRisk thirdPartyLiability)
        {
            CompanyTplRisk thirdPartyLiabilityOld = GetCompanyTplRiskByRiskId(thirdPartyLiability.Risk.Id);
            thirdPartyLiability.Risk.Beneficiaries = thirdPartyLiabilityOld.Risk.Beneficiaries;
            thirdPartyLiability.Risk.Text = thirdPartyLiabilityOld.Risk.Text;
            thirdPartyLiability.Risk.Clauses = thirdPartyLiabilityOld.Risk.Clauses;
            thirdPartyLiability.Version.Body = thirdPartyLiabilityOld.Version.Body;
            thirdPartyLiability.Risk.SecondInsured = thirdPartyLiabilityOld.Risk.SecondInsured;
            thirdPartyLiability.Risk.Number = thirdPartyLiabilityOld.Risk.Number;
            return thirdPartyLiability;
        }

        private CompanyTplRisk SetDataModification(CompanyTplRisk thirdPartyLiability)
        {
            CompanyTplRisk thirdPartyLiabilityOld = GetCompanyTplRiskByRiskId(thirdPartyLiability.Risk.Id);
            thirdPartyLiability.Risk.RiskId = thirdPartyLiabilityOld.Risk.RiskId;
            thirdPartyLiability.Risk.Description = thirdPartyLiabilityOld.Risk.Description;
            thirdPartyLiability.LicensePlate = thirdPartyLiabilityOld.LicensePlate;
            thirdPartyLiability.EngineSerial = thirdPartyLiabilityOld.EngineSerial;
            thirdPartyLiability.ChassisSerial = thirdPartyLiabilityOld.ChassisSerial;
            thirdPartyLiability.Make = thirdPartyLiabilityOld.Make;
            thirdPartyLiability.Version.Type = thirdPartyLiabilityOld.Version.Type;
            thirdPartyLiability.Year = thirdPartyLiabilityOld.Year;
            thirdPartyLiability.Risk.RatingZone = thirdPartyLiabilityOld.Risk.RatingZone;
            thirdPartyLiability.Risk.MainInsured = thirdPartyLiabilityOld.Risk.MainInsured;
            thirdPartyLiability.Risk.Beneficiaries = thirdPartyLiabilityOld.Risk.Beneficiaries;
            thirdPartyLiability.Risk.Text = thirdPartyLiabilityOld.Risk.Text;
            thirdPartyLiability.Risk.Clauses = thirdPartyLiabilityOld.Risk.Clauses;
            thirdPartyLiability.Risk.Status = thirdPartyLiabilityOld.Risk.Status;
            thirdPartyLiability.Risk.OriginalStatus = thirdPartyLiabilityOld.Risk.OriginalStatus;
            thirdPartyLiability.Risk.Status = thirdPartyLiabilityOld.Risk.Status;
            thirdPartyLiability.Risk.Number = thirdPartyLiabilityOld.Risk.Number;
            if (thirdPartyLiability.Risk.Status != RiskStatusType.Included && thirdPartyLiability.Risk.Status != RiskStatusType.Excluded)
            {
                thirdPartyLiability.Risk.Status = RiskStatusType.Modified;
            }
            return thirdPartyLiability;
        }

        public bool DeleteCompanyRisk(int temporalId, int id)
        {
            try
            {
                bool result = false;
                CompanyPolicy thirdPartyLiabilityPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (thirdPartyLiabilityPolicy != null)
                {
                    CompanyTplRisk thirdPartyLiability = GetCompanyTplRiskByRiskId(id);
                    if (thirdPartyLiability.Risk.Status == RiskStatusType.Original || thirdPartyLiability.Risk.Status == RiskStatusType.Included)
                    {
                        result = DelegateService.underwritingService.DeleteCompanyRisksByRiskId(id, false);
                    }
                    else
                    {
                        thirdPartyLiability.Risk.Status = RiskStatusType.Excluded;
                        thirdPartyLiability.Risk.Description = thirdPartyLiability.LicensePlate + " (" + EnumHelper.GetItemName<RiskStatusType>(thirdPartyLiability.Risk.Status) + ")";
                        thirdPartyLiability.Risk.IsPersisted = true;
                        thirdPartyLiability.Risk.Policy = thirdPartyLiabilityPolicy;

                        thirdPartyLiability = QuotateThirdPartyLiability(thirdPartyLiability, false, false);

                        thirdPartyLiability.Risk.Coverages.ForEach(x => x.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(x.CoverStatus));

                        CreateThirdPartyLiabilityTemporal(thirdPartyLiability, false);
                        result = true;
                    }

                    if (result)
                    {
                        return true;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorDeleteRisk);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorDeleteRisk);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public Boolean ConvertProspectToInsured(int temporalId, int individualId, string documentNumber)
        {
            try
            {
                DelegateService.underwritingService.ConvertProspectToHolder(temporalId, individualId, documentNumber);

                List<CompanyTplRisk> companyTplRisks = GetThirdPartyLiabilitiesByTemporalId(temporalId);

                if (companyTplRisks.Count > 0)
                {
                    foreach (CompanyTplRisk thirdPartyLiability in companyTplRisks)
                    {

                        CompanyRisk risk = DelegateService.underwritingService.ConvertProspectToInsured(thirdPartyLiability.Risk, individualId, documentNumber);
                        thirdPartyLiability.Risk.MainInsured = risk.MainInsured;
                        thirdPartyLiability.Risk.Beneficiaries = risk.Beneficiaries;
                        CreateThirdPartyLiabilityTemporal(thirdPartyLiability, false);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorConvertingProspectIntoIndividual);
            }
        }

        public CompanyText SaveCompanyTexts(int riskId, CompanyText companyText)
        {
            try
            {
                CompanyTplRisk risk = GetCompanyTplRiskByRiskId(riskId);
                if (risk.Risk.Id > 0)
                {


                    if (risk != null)
                    {
                        risk.Risk.Text = companyText;
                        risk = CreateThirdPartyLiabilityTemporal(risk, false);
                        if (risk != null)
                        {
                            return risk.Risk.Text;
                        }
                        else
                        {
                            throw new BusinessException(Errors.ErrorSaveText);
                        }
                    }

                    else
                    {
                        throw new BusinessException(Errors.ErrorSaveText);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.NoExistRisk);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<CompanyClause> SaveCompanyClauses(int riskId, List<CompanyClause> clauses)
        {
            try
            {
                if (clauses != null)
                {
                    CompanyTplRisk risk = GetCompanyTplRiskByRiskId(riskId);
                    if (risk.Risk.Id > 0)
                    {
                        risk.Risk.Clauses = clauses;
                        CreateThirdPartyLiabilityTemporal(risk, false);
                        return clauses;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSaveClauses);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.NoExistRisk);
                }
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }

        }
        public CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType, bool clearPolicies)
        {
            try
            {
                CompanyPolicyResult companyPolicyResult = new CompanyPolicyResult();
                companyPolicyResult.IsError = false;
                companyPolicyResult.Errors = new List<ErrorBase>();
                string message = string.Empty;
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                policy.Errors = new List<ErrorBase>();
                if (policy == null)
                {
                    companyPolicyResult.IsError = true;
                    companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorTemporalNotFound });
                }
                else
                {
                    if (temporalType != TempType.Quotation)
                    {
                        ValidateHolder(ref policy);
                    }
                    if (policy.Errors != null && !policy.Errors.Any() && policy.Product.CoveredRisk != null)
                    {
                        List<CompanyTplRisk> tpls = GetThirdPartyLiabilitiesByTemporalId(policy.Id);

                        if (tpls != null && tpls.Any())
                        {
                            if (clearPolicies)
                            {
                                policy.InfringementPolicies.Clear();
                                tpls.ForEach(x => x.Risk.InfringementPolicies.Clear());
                            }

                            policy = CreateEndorsement(policy, tpls);
                            //se agrega la validación para el caso en que tenga un evento de autorzación
                            if (policy?.InfringementPolicies?.Count == 0)
                            {
                                DelegateService.underwritingService.SaveTextLarge(policy.Endorsement.PolicyId, policy.Endorsement.Id);
                            }
                        }
                        else
                        {
                            throw new ArgumentException(Errors.NoExistRisk);
                        }
                        if (temporalType != TempType.Quotation)
                        {
                            if (policy.InfringementPolicies.Any())
                            {
                                companyPolicyResult.TemporalId = policy.Id;
                                companyPolicyResult.InfringementPolicies = policy.InfringementPolicies;
                            }
                            else
                            {
                                if (policy.Endorsement.EndorsementType == EndorsementType.Emission)
                                {
                                    if (policy.PaymentPlan.PremiumFinance == null)
                                    {
                                        companyPolicyResult.Message = string.Format(Errors.PolicyNumber, policy.DocumentNumber);
                                    }
                                    else
                                    {
                                        companyPolicyResult.Message = string.Format(Errors.PolicyNumber, policy.DocumentNumber +
                                       " \n\r " + Errors.labelPay + policy.PaymentPlan.PremiumFinance.PromissoryNoteNumCode +
                                       " \n\r " + Errors.labelUser + policy.UserId.ToString());
                                    }
                                }
                                else
                                {
                                    string additionalFinancing = "";

                                    if (policy.PaymentPlan.PremiumFinance != null)
                                    {
                                        companyPolicyResult.PromissoryNoteNumCode = policy.PaymentPlan.PremiumFinance.PromissoryNoteNumCode;
                                        additionalFinancing = string.Format(Errors.PromissoryNote, policy.PaymentPlan.PremiumFinance.PromissoryNoteNumCode, policy.User.UserId);
                                    }

                                    companyPolicyResult.Message = string.Format(Errors.EndorsementNumber, policy.DocumentNumber, policy.Endorsement.Number, policy.Endorsement.Id, additionalFinancing);
                                }

                                companyPolicyResult.DocumentNumber = policy.DocumentNumber;
                                companyPolicyResult.EndorsementId = policy.Endorsement.Id;
                                companyPolicyResult.EndorsementNumber = policy.Endorsement.Number;
                            }
                        }
                        else
                        {
                            companyPolicyResult.Message = string.Format(Errors.QuotationNumber, policy.Endorsement.QuotationId.ToString());
                            companyPolicyResult.DocumentNumber = Convert.ToDecimal(policy.Endorsement.QuotationId);
                        }
                    }
                    else
                    {
                        companyPolicyResult.IsError = true;
                        companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = string.Join(" - ", policy.Errors) });
                    }

                }
                return companyPolicyResult;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message != String.Empty && ex.InnerException.Message.Contains("PK_RSK_BENEF"))
                    throw new BusinessException(Errors.ErrorBeneficiary);
                else
                    throw new BusinessException(Errors.ErrorCreatePolicy);
            }

        }

        /// <summary>
        /// Validates the holder.
        /// </summary>
        /// <param name="policy">The policy.</param>
        public void ValidateHolder(ref CompanyPolicy policy)
        {
            if (policy.Holder != null)
            {
                if (policy.Holder.CustomerType == CustomerType.Prospect)
                {
                    policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorHolderNoInsuredRole });
                }
                else
                {
                    List<Holder> holders = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, policy.Holder.CustomerType);

                    if (holders != null && holders.Count == 1)
                    {
                        if (holders[0].InsuredId == 0)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorPolicyholderWithoutRol });
                        }
                        else if (holders[0]?.DeclinedDate > DateTime.MinValue)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorPolicyholderDisabled });
                        }
                    }
                    else
                    {
                        policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorConsultPolicyholder });
                    }

                    if (policy.Holder.PaymentMethod != null)
                    {
                        if (policy.Holder.PaymentMethod.Id == 0)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorPolicyholderDefaultPaymentPlan });
                        }
                    }

                    //Validación asegurado principal como prospecto
                    switch (policy.Product.CoveredRisk.CoveredRiskType)
                    {
                        case CoveredRiskType.Vehicle:
                            List<CompanyTplRisk> tplR = GetThirdPartyLiabilitiesByTemporalId(policy.Id);

                            var result = tplR.Select(x => x.Risk).Where(z => z.MainInsured?.CustomerType == CustomerType.Prospect).Count();
                            if (result > 0)
                            {
                                policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorInsuredNoInsuredRole });
                            }
                            break;
                    }
                }
            }

        }
        #endregion

        public List<CargoType> GetCargoTypes()
        {
            try
            {
                List<CargoType> cargotype = ModelAssembler.CreateCargoTypes(DelegateService.TransportServiceCore.GetCargoTypes());
                return cargotype;
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                throw new BusinessException(Errors.NoRiskWasFound);
            }

        }

        public CompanyTplRisk CompanySaveCompanyTPLTemporal(CompanyTplRisk companyTplRisk, bool isMassive)
        {
            try
            {
                ThirdPartyLiabilityDAO thirdPartyLiabilityDAO = new ThirdPartyLiabilityDAO();
                companyTplRisk = thirdPartyLiabilityDAO.CreateThirdPartyLiabilityTemporal(companyTplRisk, isMassive);
                if (companyTplRisk.Risk.Policy.TemporalType != TemporalType.TempQuotation)
                {
                    if (companyTplRisk.Risk.LimitRc == null)
                    {
                        companyTplRisk.Risk.LimitRc = new CompanyLimitRc
                        {
                            Id = DelegateService.commonService.GetParameterByParameterId(3000).NumberParameter.Value,
                            LimitSum = DelegateService.commonService.GetParameterByParameterId(3001).NumberParameter.Value,
                        };
                    }
                    companyTplRisk = thirdPartyLiabilityDAO.SaveCompanyTplTemporalTables(companyTplRisk);
                }

                return companyTplRisk;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public List<PoliciesAut> ValidateAuthorizationPolicies(CompanyTplRisk companyTplRisk)
        {
            try
            {
                ThirdPartyLiabilityDAO thirdPartyLiabilityDAO = new ThirdPartyLiabilityDAO();
                return thirdPartyLiabilityDAO.ValidateAuthorizationPolicies(companyTplRisk);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void CreateRisk(CompanyTplRisk companyTplRisk)
        {
            try
            {
                ThirdPartyLiabilityDAO thirdPartyLiabilityDAO = new ThirdPartyLiabilityDAO();
                thirdPartyLiabilityDAO.CreateCompanyTpl(companyTplRisk);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesMassive(CompanyTplRisk companyTpl, int hierarchy, List<int> ruleToValidateRisk, List<int> ruleToValidateCoverage)
        {
            try
            {
                ThirdPartyLiabilityDAO tplDao = new ThirdPartyLiabilityDAO();
                return tplDao.ValidateAuthorizationPoliciesMassive(companyTpl, hierarchy, ruleToValidateRisk, ruleToValidateCoverage);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public Core.Application.Vehicles.Models.Version GetVersionsByMakeIdYear(int makeId, int year)
        {
            try
            {
                ThirdPartyLiabilityDAO thirdPartyLiabilityDAO = new ThirdPartyLiabilityDAO();
                return thirdPartyLiabilityDAO.GetVersionsByMakeIdYear(makeId, year);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);

            }
        }

        /// <summary>
        /// Obtener Vehículo por Placa
        /// </summary>
        /// <param name="licencePlate">Placa</param>
        /// <returns>Vehículo</returns>
        public List<Validation> GetVehicleLicensePlate(List<Validation> validations, List<ValidationLicensePlate> validationsLicensePlate)
        {
            try
            {
                ThirdPartyLiabilityDAO thirdPartyLiabilityDAO = new ThirdPartyLiabilityDAO();
                return thirdPartyLiabilityDAO.GetVehicleLicensePlate(validations, validationsLicensePlate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}