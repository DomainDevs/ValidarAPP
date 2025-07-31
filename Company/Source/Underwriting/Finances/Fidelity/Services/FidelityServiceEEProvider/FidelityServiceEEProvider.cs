using AutoMapper;
using Sistran.Company.Application.Finances.FidelityServices.EEProvider.Assemblers;
using Sistran.Company.Application.Finances.FidelityServices.EEProvider.BusinessModels;
using Sistran.Company.Application.Finances.FidelityServices.EEProvider.DAOs;
using Sistran.Company.Application.Finances.FidelityServices.EEProvider.Resources;
using Sistran.Company.Application.Finances.FidelityServices.Models;
using Sistran.Company.Application.Location.FidelityServices.DTOs;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Finances.EEProvider;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using RES = Sistran.Company.Application.Finances.FidelityServices.EEProvider.Resources;

namespace Sistran.Company.Application.Finances.FidelityServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class FidelityServiceEEProvider : FinancesEEProviderCore, IFidelityService
    {
        /// <summary>
        /// Tarifar Poliza
        /// </summary>
        /// <param name="FidelityPolicy">Modelo de Poliza Rc</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post</param>
        /// <returns>FidelityPolicy</returns>
        public CompanyFidelityRisk QuotateFidelity(CompanyFidelityRisk CompanyFidelityRisk, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                FidelityRiskBusiness fidelityRiskBusiness = new FidelityRiskBusiness();
                return fidelityRiskBusiness.QuotateFidelity(CompanyFidelityRisk, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Ejecutar Reglas de Riesgo
        /// </summary>
        /// <param name="FidelityRisk">FidelityRisk</param>
        /// <param name="ruleSetId">Id Regla</param>
        /// <returns>FidelityRisk</returns>
        public CompanyFidelityRisk RunRulesRisk(CompanyFidelityRisk fidelityRisk, int ruleSetId)
        {
            try
            {
                FidelityRiskBusiness fidelityRiskBusiness = new FidelityRiskBusiness();
                return fidelityRiskBusiness.RunRulesRisk(fidelityRisk, ruleSetId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Ejecutar Reglas de cobertura
        /// </summary>
        /// <param name="Coverage">Coverage</param>
        /// <param name="runRulesPre"> reglasPre</param>
        /// <param name="runRulesPost"> reglasPost</param>
        /// <returns>modelo coverage</returns>
        public CompanyCoverage QuotationCompanyCoverage(CompanyFidelityRisk fidelityRisk, CompanyCoverage coverage, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.Quotate(fidelityRisk, coverage, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Ejecucion de reglas pre cobertura
        /// </summary>
        /// <param name="coverage">Modelo cobertura</param>
        /// <returns>modelo cobertura</returns>
        public CompanyCoverage RunRulesCompanyCoverage(CompanyFidelityRisk fidelityRisk, CompanyCoverage coverage, int ruleSetId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.RunRulesCoverage(fidelityRisk, coverage, ruleSetId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Ejecutar Reglas de riesgo
        /// </summary>
        /// <param name="FidelityRisk">fidelityRisk</param>
        /// <param name="runRulesPre"> reglasPre</param>
        /// <param name="runRulesPost"> reglasPost</param>
        /// <returns></returns>
        public List<CompanyFidelityRisk> QuotateFidelities(CompanyPolicy companyPolicy, List<CompanyFidelityRisk> companyFidelityRisks, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                FidelityRiskBusiness fidelityRiskBusiness = new FidelityRiskBusiness();
                return fidelityRiskBusiness.QuotateFidelities(companyPolicy, companyFidelityRisks, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="fidelityRisk">Modelo FidelityRisk</param>
        public CompanyFidelityRisk CreateFidelityTemporal(CompanyFidelityRisk fidelityRisk, bool isMassive)
        {
            try
            {
                FidelityBusiness fidelityDAO = new FidelityBusiness();
                //fidelityRisk.InfringementPolicies = fidelityDAO.ValidateAuthorizationPolicies(fidelityRisk);
                return fidelityDAO.CreateFidelityTemporal(fidelityRisk, isMassive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Poliza de responsabilidad civil
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>FidelityPolicy</returns>
        public List<CompanyFidelityRisk> GetCompanyFidelitiesByPolicyId(int policyId)
        {
            try
            {
                FidelityBusiness fidelityDAO = new FidelityBusiness();
                return fidelityDAO.GetCompanyFidelitiesByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyFidelityRisk> GetCompanyFidelitiesByEndorsementId(int endorsementId)
        {
            try
            {
                FidelityBusiness fidelityDAO = new FidelityBusiness();
                return fidelityDAO.GetCompanyFidelitiesByEndorsementId(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        //public List<CompanyFidelityRisk> GetCompanyFidelitiesRiskByEndorsementId(int endorsementId)
        //{
        //    FidelityDAO fidelityDAO = new FidelityDAO();
        //    return fidelityDAO.GetCompanyFidelitiesRiskByEndorsementId(endorsementId);
        //}

        public List<CompanyFidelityRisk> GetCompanyFidelitiesByTemporalId(int temporalId)
        {
            FidelityBusiness fidelityDAO = new FidelityBusiness();
            return fidelityDAO.GetCompanyFidelitiesByTemporalId(temporalId);
        }

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyFidelityRisk> fidelityRisks)
        {
            try
            {
                FidelityBusiness fidelityDAO = new FidelityBusiness();
                return fidelityDAO.CreateEndorsement(companyPolicy, fidelityRisks);
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
        public CompanyFidelityRisk GetCompanyFidelityByRiskId(int riskId)
        {
            try
            {
                FidelityBusiness fidelityDAO = new FidelityBusiness();
                return fidelityDAO.GetCompanyFidelityByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #region emision

        #region Clauses
        public List<CompanyClause> SaveClauses(int riskId, List<CompanyClause> clauses)
        {
            try
            {
                if (clauses != null)
                {
                    CompanyFidelityRisk fidelityRisk = GetCompanyFidelityByRiskId(riskId);
                    if (fidelityRisk.Risk.Id > 0)
                    {

                        fidelityRisk.Risk.Clauses = clauses;

                        if (fidelityRisk != null)
                        {
                            CreateFidelityTemporal(fidelityRisk, false);
                            return clauses;
                        }
                        else
                        {
                            throw new Exception(RES.Errors.ErrorSaveClauses);
                        }
                    }
                    else
                    {
                        throw new BusinessException(RES.Errors.NoExistRisk);
                    }
                }
                else
                {
                    throw new BusinessException(RES.Errors.ErrorSelectedClauses);
                }
            }
            catch (Exception)
            {
                throw new Exception(RES.Errors.ErrorSaveClauses);
            }
        }

        #endregion Clauses


        public Boolean ConvertProspectToInsured(int temporalId, int individualId, string documentNumber)
        {
            try
            {
                DelegateService.underwritingService.ConvertProspectToHolder(temporalId, individualId, documentNumber);
                List<CompanyFidelityRisk> companyTplRisks = GetCompanyFidelitiesByTemporalId(temporalId);

                if (companyTplRisks.Count > 0)
                {
                    foreach (CompanyFidelityRisk fidelity in companyTplRisks)
                    {
                        CompanyRisk risk = DelegateService.underwritingService.ConvertProspectToInsured(fidelity.Risk, individualId, documentNumber);
                        fidelity.Risk.Beneficiaries = risk.Beneficiaries;
                        CreateFidelityTemporal(fidelity, false);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                throw new Exception(RES.Errors.ErrorConvertingProspectIntoIndividual);
            }
        }

        public List<CompanyFidelityRisk> GetTemporalById(int id)
        {
            try
            {

                List<CompanyFidelityRisk> fidelityRisk = GetCompanyFidelitiesByTemporalId(id);

                if (fidelityRisk != null)
                {
                    List<CompanyFidelityRisk> risks = new List<CompanyFidelityRisk>();

                    foreach (CompanyFidelityRisk item in fidelityRisk)
                    {
                        CompanyFidelityRisk risk = GetCompanyFidelityByRiskId(item.Risk.Id);

                        risk.Risk.Id = item.Risk.Id;
                        risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
                        risk.Risk.Premium = risk.Risk.Coverages.Sum(x => x.PremiumAmount);
                        risks.Add(risk);

                    }

                    return risks;
                }
                else
                {
                    throw new BusinessException(RES.Errors.ErrorTempNoExist);
                }
            }
            catch (Exception)
            {
                throw new Exception(RES.Errors.ErrorSearchRisk);
            }
        }

        public CompanyFidelityRisk GetRiskById(int id)
        {
            try
            {
                CompanyFidelityRisk risk = GetCompanyFidelityByRiskId(id);

                if (risk != null)
                {
                    risk = GetRiskDescriptions(risk);

                    return risk;
                }
                else
                {
                    throw new BusinessException(RES.Errors.NoRiskWasFound);
                }
            }
            catch (Exception)
            {
                throw new Exception(RES.Errors.ErrorSearchRisk);
            }
        }

        public CompanyFidelityRisk GetRiskDescriptions(CompanyFidelityRisk risk)
        {
            if (risk.Risk.MainInsured.IdentificationDocument == null)
            {
                risk.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(risk.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, risk.Risk.MainInsured.CustomerType);
                risk.Risk.MainInsured.Name = risk.Risk.MainInsured.Surname + " " + (string.IsNullOrEmpty(risk.Risk.MainInsured.SecondSurname) ? "" : risk.Risk.MainInsured.SecondSurname + " ") + risk.Risk.MainInsured.Name;
            }

            if (risk.Risk.Beneficiaries == null && risk.Risk.Beneficiaries[0].IdentificationDocument == null)
            {
                foreach (CompanyBeneficiary item in risk.Risk.Beneficiaries)
                {
                    Beneficiary beneficiary = new Beneficiary();
                    beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                    item.IdentificationDocument = beneficiary.IdentificationDocument;
                    item.Name = beneficiary.Name;
                }
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

        public Boolean DeleteRisk(int temporalId, int riskId)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

                bool result = false;

                if (companyPolicy != null)
                {
                    CompanyPolicy fidelityPolicy = new CompanyPolicy();

                    var imapper = ModelAssembler.CreateMapPolicy();
                    imapper.Map(companyPolicy, fidelityPolicy);

                    CompanyFidelityRisk fidelityRisk = GetCompanyFidelityByRiskId(riskId);

                    if (fidelityRisk.Risk.Status == RiskStatusType.Original || fidelityPolicy.Endorsement.EndorsementType == EndorsementType.Renewal || fidelityRisk.Risk.Status == RiskStatusType.Included)
                    {
                        result = DelegateService.utilitiesServiceCore.DeletePendingOperation(riskId);
                        DelegateService.underwritingService.DeleteRisk(riskId);
                    }
                    else
                    {
                        fidelityRisk.Risk.Status = RiskStatusType.Excluded;
                        fidelityRisk.Risk.Description = fidelityRisk.Risk.Description + " (" + EnumHelper.GetItemName<RiskStatusType>(RiskStatusType.Excluded) + ")";
                        fidelityRisk.Risk.IsPersisted = true;
                        fidelityRisk.Risk.Policy = fidelityPolicy;

                        fidelityRisk = QuotateFidelity(fidelityRisk, false, false);
                        fidelityRisk.Risk.Coverages.ForEach(x => x.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(x.CoverStatus));

                        fidelityRisk = CreateFidelityTemporal(fidelityRisk, false);
                        result = true;
                    }

                    if (result)
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception(RES.Errors.ErrorDeleteRisk);
                    }
                }
                else
                {
                    throw new BusinessException(RES.Errors.ErrorTempNoExist);
                }
            }
            catch (Exception)
            {
                throw new Exception(RES.Errors.ErrorDeleteRisk);
            }
        }

        public CompanyFidelityRisk SaveRisk(CompanyFidelityRisk fidelityRisk, int temporalId, int? riskId, int? RiskEndorsementType)
        {

            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (policy != null)
                {
                    List<CompanyFidelityRisk> risks = GetCompanyFidelitiesByTemporalId(temporalId);
                    // List<string> message = ExistsRisk(risks, fidelityRisk.Risk.Id, fidelityRisk.LicensePlate, fidelity.EngineSerial, fidelity.ChassisSerial, policy.Endorsement.EndorsementType.Value, policy.Endorsement.Id);

                    //if (message?.Count == 0)
                    //{
                    if (fidelityRisk != null && fidelityRisk.Risk != null)
                    {
                        fidelityRisk.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                        // fidelityRisk.Risk.LimitRc = DelegateService.underwritingService.GetCompanyLimitRcById(fidelityRisk.Risk.LimitRc.Id);

                        fidelityRisk.Risk.CoveredRiskType = policy.Product.CoveredRisk.CoveredRiskType;
                        fidelityRisk.Risk.Policy = policy;

                        if (policy.Endorsement.EndorsementType == EndorsementType.Modification)
                        {
                            fidelityRisk.Risk.Status = RiskStatusType.Included;
                        }

                        if (fidelityRisk.Risk?.Id == 0)
                        {
                            if (risks.Count < policy.Product.CoveredRisk.MaxRiskQuantity)
                            {

                                if (policy.DefaultBeneficiaries != null && policy.DefaultBeneficiaries.Count > 0)
                                {
                                    fidelityRisk.Risk.Beneficiaries = policy.DefaultBeneficiaries;
                                }
                                else
                                {
                                    ModelAssembler.CreateMapCompanyInsured();
                                    fidelityRisk.Risk.Beneficiaries.Add(ModelAssembler.CreateBeneficiaryFromInsured(fidelityRisk.Risk.MainInsured));
                                }
                            }
                            else
                            {
                                throw new BusinessException(RES.Errors.ProductNotAddingMoreRisks);
                            }
                        }
                        else
                        {
                            switch (policy.Endorsement.EndorsementType.Value)
                            {
                                case EndorsementType.Emission:
                                case EndorsementType.Renewal:
                                    fidelityRisk = SetDataEmission(fidelityRisk);
                                    break;
                                case EndorsementType.Modification:
                                    fidelityRisk = SetDataModification(fidelityRisk);
                                    break;
                                default:
                                    break;
                            }
                        }

                        fidelityRisk = CreateFidelityTemporal(fidelityRisk, false);
                        return fidelityRisk;
                    }
                    else
                    {
                        throw new BusinessException(RES.Errors.ErrorSaveRisk);
                    }
                }
                else
                {
                    throw new BusinessException(RES.Errors.ErrorTemporalNotFound);
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
                    throw new Exception(RES.Errors.ErrorSaveRisk);
                }
            }
        }

        private CompanyFidelityRisk SetDataEmission(CompanyFidelityRisk fidelity)
        {
            try
            {
                CompanyFidelityRisk fidelityOld = GetCompanyFidelityByRiskId(fidelity.Risk.Id);

                fidelity.Risk.Beneficiaries = fidelityOld.Risk.Beneficiaries;
                fidelity.Risk.Text = fidelityOld.Risk.Text;
                fidelity.Risk.Clauses = fidelityOld.Risk.Clauses;
                fidelity.Risk.SecondInsured = fidelityOld.Risk.SecondInsured;

                return fidelity;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        private CompanyFidelityRisk SetDataModification(CompanyFidelityRisk fidelityRisk)
        {
            try
            {
                CompanyFidelityRisk riskOld = GetCompanyFidelityByRiskId(fidelityRisk.Risk.Id);

                fidelityRisk.Risk.Beneficiaries = riskOld.Risk.Beneficiaries;
                fidelityRisk.Risk.RiskId = riskOld.Risk.RiskId;
                fidelityRisk.Risk.Text = riskOld.Risk.Text;
                fidelityRisk.Risk.Status = riskOld.Risk.Status;
                fidelityRisk.Risk.Clauses = riskOld.Risk.Clauses;
                fidelityRisk.Risk.OriginalStatus = riskOld.Risk.OriginalStatus;
                fidelityRisk.Risk.Number = riskOld.Risk.Number;
                if (fidelityRisk.Risk.Status != RiskStatusType.Included && fidelityRisk.Risk.Status != RiskStatusType.Excluded)
                {
                    fidelityRisk.Risk.Status = RiskStatusType.Modified;
                }
                return fidelityRisk;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public CompanyFidelityRisk GetPremium(CompanyFidelityRisk fidelityRisk, List<CompanyCoverage> coverages, List<DynamicConcept> dynamicProperties, int temporalId)
        {
            try
            {
                fidelityRisk.Risk.IsPersisted = true;
                fidelityRisk.Risk.Coverages = coverages;
                fidelityRisk.Risk.DynamicProperties = dynamicProperties;

                CompanyPolicy fidelityPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                fidelityPolicy.IsPersisted = true;
                fidelityRisk.Risk.Policy = fidelityPolicy;

                fidelityRisk = QuotateFidelity(fidelityRisk, false, true);
                fidelityRisk.Risk.Coverages.ForEach(x => x.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(x.CoverStatus.Value));

                return fidelityRisk;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private int GetValidRisks(List<CompanyFidelityRisk> fidelityRisks, CompanyPolicy policy)
        {
            if (policy.Endorsement.EndorsementType == EndorsementType.Modification)
            {
                var countRisk = 0;
                foreach (var item in fidelityRisks)
                {

                    CompanyFidelityRisk risk = GetCompanyFidelityByRiskId(item.Risk.Id);
                    if (risk.Risk.Status != RiskStatusType.Excluded)
                    {
                        countRisk++;
                    }
                }
                return countRisk;
            }
            else
            {
                return fidelityRisks.Count;
            }
        }

        public Boolean SaveClause(int riskId, List<CompanyClause> clauses)
        {
            try
            {
                if (clauses != null)
                {
                    CompanyFidelityRisk risk = GetCompanyFidelityByRiskId(riskId);
                    if (risk.Risk.Id > 0)
                    {
                        risk.Risk.Clauses = clauses;

                        if (risk != null)
                        {
                            CreateFidelityTemporal(risk, false);
                            return true;
                        }
                        else
                        {
                            throw new Exception(RES.Errors.ErrorSaveClauses);
                        }
                    }
                    else
                    {
                        throw new Exception(RES.Errors.ErrorSaveClauses);
                    }
                }
                else
                {
                    throw new Exception(RES.Errors.ErrorSaveClauses);
                }
            }
            catch (Exception)
            {
                throw new Exception(RES.Errors.ErrorSaveClauses);
            }
        }

        #region Coverages

        public CompanyCoverage ExcludeCoverage(int temporalId, int riskId, int riskCoverageId, string description)
        {
            CompanyCoverage coverage = DelegateService.underwritingService.GetCompanyCoverageByRiskCoverageId(riskCoverageId);
            if (coverage != null)
            {
                CompanyPolicy fidelityPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

                CompanyFidelityRisk risk = GetCompanyFidelityByRiskId(riskId);

                coverage.Description = description;
                coverage.SubLineBusiness = risk.Risk.Coverages.First(x => x.RiskCoverageId == riskCoverageId).SubLineBusiness;
                coverage.CoverStatus = CoverageStatusType.Excluded;
                coverage.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Excluded);
                coverage.EndorsementType = fidelityPolicy.Endorsement.EndorsementType;
                coverage.CurrentFrom = fidelityPolicy.CurrentFrom;
                risk.Risk.Policy = fidelityPolicy;
                coverage = QuotationCompanyCoverage(risk, coverage, false, false);

            }
            return coverage;
        }

        public Boolean SaveCoverages(int temporalId, int riskId, List<CompanyCoverage> coverages)
        {
            try
            {
                CompanyPolicy fidelityPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                fidelityPolicy.IsPersisted = true;

                CompanyFidelityRisk fidelityRisk = GetCompanyFidelityByRiskId(riskId);
                fidelityRisk.Risk.IsPersisted = true;

                if (coverages != null)
                {
                    fidelityRisk.Risk.Policy = fidelityPolicy;
                    fidelityRisk.Risk.Coverages = coverages;

                    fidelityRisk = QuotateFidelity(fidelityRisk, false, true);
                    fidelityRisk = CreateFidelityTemporal(fidelityRisk, false);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw new Exception(RES.Errors.ErrorSaveCoverages);
            }
        }

        public CompanyCoverage GetCoverageByCoverageId(int coverageId, int riskId, int temporalId, int groupCoverageId)
        {
            try
            {
                CompanyPolicy fidelityPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

                CompanyCoverage coverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, fidelityPolicy.Product.Id, groupCoverageId);

                coverage.EndorsementType = fidelityPolicy.Endorsement.EndorsementType;
                coverage.CurrentFrom = fidelityPolicy.CurrentFrom;
                coverage.CurrentTo = fidelityPolicy.CurrentTo;
                coverage.Days = Convert.ToInt32((coverage.CurrentTo.Value - coverage.CurrentFrom).TotalDays);

                if (coverage.EndorsementType == EndorsementType.Modification)
                {
                    coverage.CoverStatus = CoverageStatusType.Included;
                }

                coverage.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus);

                return coverage;
            }
            catch (Exception)
            {
                throw new Exception(RES.Errors.ErrorSearchCoverage);
            }
        }

        public List<CompanyCoverage> GetCoveragesByProductIdGroupCoverageId(int temporalId, int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                CompanyPolicy fidelityPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                //List<CompanyCoverage> coverages = new List<CompanyCoverage>();
                if (fidelityPolicy.Id > 0)
                {
                    List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(fidelityPolicy.Product.Id, groupCoverageId, fidelityPolicy.Prefix.Id);

                    coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);

                    foreach (CompanyCoverage item in coverages)
                    {
                        item.EndorsementType = fidelityPolicy.Endorsement.EndorsementType;
                        item.CalculationType = Core.Services.UtilitiesServices.Enums.CalculationType.Prorate;
                        item.CurrentFrom = fidelityPolicy.CurrentFrom;
                        item.CurrentTo = fidelityPolicy.CurrentTo;
                        item.RateType = RateType.Percentage;
                        item.LimitAmount = 0;
                        item.SubLimitAmount = 0;
                        if (fidelityPolicy.Endorsement.EndorsementType == EndorsementType.Emission ||
                            fidelityPolicy.Endorsement.EndorsementType == EndorsementType.Renewal)
                        {
                            item.CoverStatus = CoverageStatusType.Original;
                        }
                        else
                        {
                            item.CoverStatus = CoverageStatusType.Included;
                        }
                        item.CoverStatusName = RES.Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(item.CoverStatus));
                    }
                    coverages = coverages.Where(x => x.IsSelected == true).ToList();
                    return coverages;
                }
                else
                {
                    throw new BusinessException(RES.Errors.ErrorSearchCoverages);
                }
            }
            catch (Exception)
            {
                throw new Exception(RES.Errors.ErrorQueryCoverageGroups);
            }
        }

        public List<CompanyCoverage> GetAllyCoverageByCoverage(int tempId, int riskId, int groupCoverageId, CompanyCoverage coverage)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(tempId, false);
                List<CompanyCoverage> coverages = new List<CompanyCoverage>();
                coverages = DelegateService.underwritingService.GetAllyCompanyCoveragesByCoverageIdProductIdGroupCoverageId(coverage.Id, policy.Product.Id, groupCoverageId);
                CompanyFidelityRisk fidelityRisk = new CompanyFidelityRisk
                {
                    Risk = new CompanyRisk
                    {
                        Id = riskId
                    }
                };
                foreach (CompanyCoverage item in coverages)
                {
                    item.DeclaredAmount = (coverage.DeclaredAmount * (item.SublimitPercentage == null ? 0 : item.SublimitPercentage.Value)) / 100;
                    CompanyPolicy fidelityPolicy = new CompanyPolicy { Id = tempId, Endorsement = new CompanyEndorsement { EndorsementType = (EndorsementType)item.EndorsementType } };
                    QuotationCompanyCoverage(fidelityRisk, item, false, true);
                    if (policy.Endorsement.EndorsementType == EndorsementType.Modification)
                    {
                        if (coverage.CoverStatus != null && coverage.CoverStatus == CoverageStatusType.NotModified)
                            coverage.CoverStatus = CoverageStatusType.Modified;
                        coverage.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Modified);
                    }
                }

                return coverages;
            }
            catch (Exception)
            {

                throw new Exception(RES.Errors.ErrorGettingAlliedCoverage);
            }
        }

        public CompanyCoverage QuotationCoverage(CompanyCoverage coverage, int riskId, int temporalId, int endorsementType)
        {
            try
            {
                CompanyFidelityRisk fidelity = new CompanyFidelityRisk
                {
                    Risk = new CompanyRisk
                    {
                        Id = riskId,
                        Policy = new CompanyPolicy
                        {
                            Id = temporalId,
                            Endorsement = new CompanyEndorsement
                            {
                                EndorsementType = (EndorsementType)endorsementType
                            }
                        }
                    }
                };

                coverage = QuotationCompanyCoverage(fidelity, coverage, false, true);

                coverage.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus);

                return coverage;

            }
            catch (Exception)
            {
                throw new Exception(RES.Errors.ErrorQuotationCoverage);
            }

        }

        #endregion Coverages

        #region text

        #endregion text

        #region Beneficiary
        public List<CompanyBeneficiary> SaveBeneficiaries(int riskId, List<CompanyBeneficiary> beneficiaries)
        {
            try
            {
                CompanyFidelityRisk fidelityRisk = GetCompanyFidelityByRiskId(riskId);

                if (fidelityRisk.Risk.Id > 0)
                {
                    fidelityRisk.Risk.Beneficiaries = beneficiaries;

                    if (fidelityRisk != null)
                    {
                        CreateFidelityTemporal(fidelityRisk, false);
                        return beneficiaries;
                    }
                    else
                    {
                        throw new Exception(RES.Errors.ErrorSaveBeneficiaries);
                    }
                }
                else
                {
                    throw new Exception(RES.Errors.ErrorTempNoExist);
                }
            }
            catch (Exception)
            {
                throw new Exception(RES.Errors.ErrorSaveBeneficiaries);
            }
        }

        #endregion Beneficiary

        #region Reglas
        public CompanyFidelityRisk RunRulesRiskPreFidelity(int policyId, int? ruleSetId)
        {
            try
            {
                CompanyFidelityRisk fidelityRisk = new CompanyFidelityRisk
                {
                    Risk = new CompanyRisk
                    {
                        Policy = new CompanyPolicy
                        {
                            Id = policyId,
                            IsPersisted = false
                        },
                        CoveredRiskType = CoveredRiskType.Location,
                        IsPersisted = true
                    }
                };

                if (ruleSetId.GetValueOrDefault() > 0)
                {
                    fidelityRisk.Risk.IsPersisted = true;

                    fidelityRisk = RunRulesRisk(fidelityRisk, ruleSetId.Value);
                }

                return fidelityRisk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public CompanyPolicy UpdateRisks(int temporalId)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (companyPolicy != null)
                {
                    companyPolicy.IsPersisted = true;
                    List<CompanyFidelityRisk> CompanyFidelityRisks = GetCompanyFidelitiesByTemporalId(temporalId);

                    if (CompanyFidelityRisks != null && CompanyFidelityRisks.Any())
                    {
                        Parallel.ForEach(CompanyFidelityRisks, ParallelHelper.DebugParallelFor(), companyFidelityRisk =>
                        {
                            companyFidelityRisk.Risk.Policy = companyPolicy;
                            companyFidelityRisk.Risk.IsPersisted = true;
                            companyFidelityRisk?.Risk.Coverages.AsParallel().ForAll(x =>
                            {
                                x.CurrentTo = companyPolicy.CurrentTo;
                                x.CurrentFrom = companyPolicy.CurrentFrom;
                            });
                            var companyFidelityRiskData = QuotateFidelity(companyFidelityRisk, true, true);
                            CreateFidelityTemporal(companyFidelityRiskData, false);
                        });
                        companyPolicy = DelegateService.underwritingService.UpdatePolicyComponents(companyPolicy.Id);
                        return companyPolicy;
                    }
                    else
                    {
                        throw new Exception(RES.Errors.ErrorTemporalNotFound);
                    }
                }
                else
                {
                    throw new Exception(RES.Errors.ErrorTemporalNotFound);

                }
            }
            catch (Exception)
            {
                throw new BusinessException(RES.Errors.ErrorUpdatePolicy);
            }
        }
        #endregion Reglas

        #region poliza riego
        /// <summary>
        /// Creates the company policy.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="temporalType">Type of the temporal.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType)
        {
            try
            {
                CompanyPolicyResult companyPolicyResult = new CompanyPolicyResult();
                companyPolicyResult.IsError = false;
                companyPolicyResult.Errors = new List<ErrorBase>();
                string message = string.Empty;
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (policy == null)
                {
                    companyPolicyResult.IsError = true;
                    companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = RES.Errors.ErrorTemporalNotFound });
                }
                else
                {
                    policy.Errors = new List<ErrorBase>();
                    if (policy.Summary == null || policy.Summary.Premium == 0)
                    {
                        policy.Errors.Add(new ErrorBase { StateData = false, Error = RES.Errors.ErrorTempPremiumZero });
                    }
                    else
                    {
                        if (temporalType != TempType.Quotation)
                        {
                            ValidateHolder(ref policy);
                        }
                        if (!policy.Errors.Any())
                        {
                            switch (policy.Product.CoveredRisk.SubCoveredRiskType)
                            {
                                case SubCoveredRiskType.Fidelity:
                                    List<CompanyFidelityRisk> FidelityRisk = GetCompanyFidelitiesByTemporalId(policy.Id);

                                    if (FidelityRisk != null && FidelityRisk.Any())
                                    {
                                        policy = CreateEndorsement(policy, FidelityRisk);
                                    }
                                    else
                                    {
                                        throw new ArgumentException(RES.Errors.NoExistRisk);
                                    }
                                    if (temporalType != TempType.Quotation)
                                    {
                                        companyPolicyResult.Message = string.Format(RES.Errors.PolicyNumber, policy.DocumentNumber);
                                        companyPolicyResult.DocumentNumber = policy.DocumentNumber;
                                    }
                                    else
                                    {
                                        companyPolicyResult.Message = string.Format(RES.Errors.QuotationNumber, policy.Endorsement.QuotationId.ToString());
                                        companyPolicyResult.DocumentNumber = Convert.ToDecimal(policy.Endorsement.QuotationId);
                                    }
                                    break;
                                case SubCoveredRiskType.FidelityNewVersion:
                                    List<CompanyFidelityRisk> FidelityNvRisk = GetCompanyFidelitiesByTemporalId(policy.Id);

                                    if (FidelityNvRisk != null && FidelityNvRisk.Any())
                                    {
                                        policy = CreateEndorsement(policy, FidelityNvRisk);
                                    }
                                    else
                                    {
                                        throw new ArgumentException(RES.Errors.NoExistRisk);
                                    }
                                    if (temporalType != TempType.Quotation)
                                    {
                                        companyPolicyResult.Message = string.Format(RES.Errors.PolicyNumber, policy.DocumentNumber);
                                        companyPolicyResult.DocumentNumber = policy.DocumentNumber;
                                        companyPolicyResult.EndorsementId = policy.Endorsement.Id;
                                        companyPolicyResult.EndorsementNumber = policy.Endorsement.Number;
                                    }
                                    else
                                    {
                                        companyPolicyResult.Message = string.Format(RES.Errors.QuotationNumber, policy.Endorsement.QuotationId.ToString());
                                        companyPolicyResult.DocumentNumber = Convert.ToDecimal(policy.Endorsement.QuotationId);
                                        companyPolicyResult.EndorsementId = policy.Endorsement.Id;
                                        companyPolicyResult.EndorsementNumber = policy.Endorsement.Number;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            companyPolicyResult.IsError = true;
                            companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = string.Join(" - ", policy.Errors) });
                        }
                    }
                }
                return companyPolicyResult;
            }
            catch (Exception)
            {
                throw new BusinessException(RES.Errors.ErrorCreatePolicy);
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
                    policy.Errors.Add(new ErrorBase { StateData = false, Error = RES.Errors.ErrorHolderNoInsuredRole });
                }
                else
                {
                    List<Holder> holders = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, policy.Holder.CustomerType);

                    if (holders != null && holders.Count == 1)
                    {
                        if (holders[0].InsuredId == 0)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = RES.Errors.ErrorPolicyholderWithoutRol });
                        }
                        else if (holders[0]?.DeclinedDate > DateTime.MinValue)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = RES.Errors.ErrorPolicyholderDisabled });
                        }
                    }
                    else
                    {
                        policy.Errors.Add(new ErrorBase { StateData = false, Error = RES.Errors.ErrorConsultPolicyholder });
                    }

                    if (policy.Holder.PaymentMethod != null)
                    {
                        if (policy.Holder.PaymentMethod.Id == 0)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = RES.Errors.ErrorPolicyholderDefaultPaymentPlan });
                        }
                    }

                    //Validación asegurado principal como prospecto
                    switch (policy.Product.CoveredRisk.CoveredRiskType)
                    {
                        case CoveredRiskType.Location:
                            List<CompanyFidelityRisk> FidelityRisk = GetCompanyFidelitiesByTemporalId(policy.Id);

                            var result = FidelityRisk.Select(x => x.Risk).Where(z => z.MainInsured?.CustomerType == CustomerType.Prospect).Count();
                            if (result > 0)
                            {
                                policy.Errors.Add(new ErrorBase { StateData = false, Error = RES.Errors.ErrorInsuredNoInsuredRole });
                            }
                            break;
                    }
                }
            }

        }
        #endregion
        #endregion


        public List<OccupationDTO> GetOccupations()
        {
            try
            {
                FidelityBusiness fidelityBusiness = new FidelityBusiness();
                return fidelityBusiness.GetOccupations(GetIssuanceOccupations());
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetOccupations), ex); ;
            }
        }

        public List<CompanyFidelityRisk> GetCompanyFidelityRisksByInsuredId(int insuredId)
        {
            try
            {
                FidelityBusiness fidelityBusiness = new FidelityBusiness();
                return fidelityBusiness.GetCompanyFidelityRisksByInsuredId(insuredId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex); ;
            }
        }

        public CompanyFidelityRisk GetCompanyFidelityRiskByRiskId(int riskId)
        {
            try
            {
                FidelityBusiness fidelityBusiness = new FidelityBusiness();
                return fidelityBusiness.GetCompanyFidelityRiskByRiskId(riskId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex); ;
            }
        }

        public List<CompanyFidelityRisk> GetCompanyFidelitiesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            try
            {
                List<CompanyFidelityRisk> companyFidelities = new List<CompanyFidelityRisk>();
                FidelityBusiness fidelityBusiness = new FidelityBusiness();

                switch (moduleType)
                {
                    case ModuleType.Emission:
                        companyFidelities = fidelityBusiness.GetCompanyFidelitiesByEndorsementId(endorsementId);
                        break;
                    case ModuleType.Claim:
                        companyFidelities = fidelityBusiness.GetCompanyClaimFidelitiesByEndorsementId(endorsementId);
                        break;
                    default:
                        break;
                }

                return companyFidelities;
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex); ;
            }
        }
    }
}
