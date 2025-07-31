using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AutoMapper;
using Sistran.Company.Application.Location.LiabilityServices.EEProvider.Assemblers;
using Sistran.Company.Application.Location.LiabilityServices.EEProvider.BusinessModels;
using Sistran.Company.Application.Location.LiabilityServices.EEProvider.DAOs;
using Sistran.Company.Application.Location.LiabilityServices.EEProvider.Resources;
using Sistran.Company.Application.Location.LiabilityServices.Enum;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Locations.EEProvider;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;
using UtlCore = Sistran.Core.Application.Utilities;

namespace Sistran.Company.Application.Location.LiabilityServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class LiabilityServiceEEProvider : LocationsEEProvider, ILiabilityService
    {
        /// <summary>
        /// Tarifar Poliza
        /// </summary>
        /// <param name="LiabilityPolicy">Modelo de Poliza Rc</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post</param>
        /// <returns>LiabilityPolicy</returns>
        public CompanyLiabilityRisk QuotateLiability(CompanyLiabilityRisk CompanyLiabilityRisk, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                LiabilityRiskBusiness liabilityRiskBusiness = new LiabilityRiskBusiness();
                return liabilityRiskBusiness.QuotateLiability(CompanyLiabilityRisk, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        /// <summary>
        /// Ejecutar Reglas de Riesgo
        /// </summary>
        /// <param name="LiabilityRisk">LiabilityRisk</param>
        /// <param name="ruleSetId">Id Regla</param>
        /// <returns>LiabilityRisk</returns>
        public CompanyLiabilityRisk RunRulesRisk(CompanyLiabilityRisk liabilityRisk, int ruleSetId)
        {
            try
            {
                LiabilityRiskBusiness liabilityRiskBusiness = new LiabilityRiskBusiness();
                return liabilityRiskBusiness.RunRulesRisk(liabilityRisk, ruleSetId);
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
        public CompanyCoverage QuotationCompanyCoverage(CompanyLiabilityRisk liabilityRisk, CompanyCoverage coverage, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();

                if (runRulesPost)
                {
                    coverage = coverageBusiness.Quotate(liabilityRisk, coverage, runRulesPre, !runRulesPost);
                }

                return coverageBusiness.Quotate(liabilityRisk, coverage, runRulesPre, runRulesPost);
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
        public CompanyCoverage RunRulesCompanyCoverage(CompanyLiabilityRisk liabilityRisk, CompanyCoverage coverage, int ruleSetId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.RunRulesCoverage(liabilityRisk, coverage, ruleSetId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Ejecutar Reglas de riesgo
        /// </summary>
        /// <param name="LiabilityRisk">liabilityRisk</param>
        /// <param name="runRulesPre"> reglasPre</param>
        /// <param name="runRulesPost"> reglasPost</param>
        /// <returns></returns>
        public List<CompanyLiabilityRisk> QuotateLiabilities(CompanyPolicy companyPolicy, List<CompanyLiabilityRisk> companyLiabilityRisks, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                LiabilityRiskBusiness liabilityRiskBusiness = new LiabilityRiskBusiness();
                return liabilityRiskBusiness.QuotateLiabilities(companyPolicy, companyLiabilityRisks, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="liabilityRisk">Modelo LiabilityRisk</param>
        public CompanyLiabilityRisk CreateLiabilityTemporal(CompanyLiabilityRisk liabilityRisk, bool isMassive)
        {
            try
            {
                LiabilityDAO liabilityDAO = new LiabilityDAO();
                liabilityRisk.InfringementPolicies = liabilityDAO.ValidateAuthorizationPolicies(liabilityRisk);
                liabilityRisk = liabilityDAO.CreateLiabilityTemporal(liabilityRisk, isMassive);
                if (liabilityRisk.Risk.Policy != null && liabilityRisk.Risk.Policy.TemporalType != TemporalType.TempQuotation)
                {
                    //Se agrega validación para generar la impresión correctamente, 
                    //dado que varios metodos llegan a este para actualizar el temporal, se implementa acá
                    if (liabilityRisk.Risk.LimitRc == null)
                    {
                        liabilityRisk.Risk.LimitRc = new CompanyLimitRc
                        {
                            Id = DelegateService.commonService.GetParameterByParameterId(3000).NumberParameter.Value,
                            LimitSum = DelegateService.commonService.GetParameterByParameterId(3001).NumberParameter.Value,
                        };
                    }
                    liabilityRisk = liabilityDAO.SaveCompanyLiabilityTemporalTables(liabilityRisk);
                }
                return liabilityRisk;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public List<CompanyAssuranceMode> GetAssuranceMode()
        {
            List<CompanyAssuranceMode> riskSubActivities = new List<CompanyAssuranceMode>();
            LiabilityDAO liabilityDAO = new LiabilityDAO();

            return liabilityDAO.GetRiskAssuranceMode();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="riskActivity"></param>
        /// <returns></returns>
        public List<CompanyRiskSubActivity> GetSubActivities(int riskActivity)
        {
            try
            {
                List<CompanyRiskSubActivity> riskSubActivities = new List<CompanyRiskSubActivity>();
                LiabilityDAO liabilityDAO = new LiabilityDAO();
                //UTIER.Result<List<CompanyRiskSubActivity>, UTIER.ErrorModel> result = liabilityDAO.GetRiskSubActivitiesByActivity(riskActivity);
                //if (result is UTIER.ResultValue<List<CompanyRiskSubActivity>, UTIER.ErrorModel>)
                //{
                //riskSubActivities = (result as UTIER.ResultValue<List<CompanyRiskSubActivity>, UTIER.ErrorModel>).Value;
                return liabilityDAO.GetRiskSubActivitiesByActivity(riskActivity);

                //}
                //return riskSubActivities;

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
        /// <returns>LiabilityPolicy</returns>
        public List<CompanyLiabilityRisk> GetCompanyLiebilitiesByPolicyId(int policyId)
        {
            try
            {
                LiabilityDAO liabilityDAO = new LiabilityDAO();
                return liabilityDAO.GetCompanyLiabilitysByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyLiabilityRisk> GetCompanyLiabilitiesByEndorsementId(int endorsementId)
        {
            try
            {
                LiabilityDAO liabilityDAO = new LiabilityDAO();
                return liabilityDAO.GetCompanyLiabilitiesByEndorsementId(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        //public List<CompanyLiabilityRisk> GetCompanyLiabilitiesRiskByEndorsementId(int endorsementId)
        //{
        //    LiabilityDAO liabilityDAO = new LiabilityDAO();
        //    return liabilityDAO.GetCompanyLiabilitiesRiskByEndorsementId(endorsementId);
        //}

        public List<CompanyLiabilityRisk> GetCompanyLiabilitiesByTemporalId(int temporalId)
        {
            LiabilityDAO liabilityDAO = new LiabilityDAO();
            return liabilityDAO.GetCompanyLiabilitiesByTemporalId(temporalId);
        }

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyLiabilityRisk> liabilityRisks)
        {
            try
            {
                LiabilityDAO liabilityDAO = new LiabilityDAO();
                return liabilityDAO.CreateEndorsement(companyPolicy, liabilityRisks);
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
        public CompanyLiabilityRisk GetCompanyLiabilityByRiskId(int riskId)
        {
            try
            {

                LiabilityDAO liabilityDAO = new LiabilityDAO();
                return liabilityDAO.GetCompanyLiabilityByRiskId(riskId);
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
                    CompanyLiabilityRisk liabilityRisk = GetCompanyLiabilityByRiskId(riskId);
                    if (liabilityRisk.Risk.Id > 0)
                    {

                        liabilityRisk.Risk.Clauses = clauses;

                        if (liabilityRisk != null)
                        {
                            CreateLiabilityTemporal(liabilityRisk, false);
                            return clauses;
                        }
                        else
                        {
                            throw new Exception(Errors.ErrorSaveClauses);
                        }
                    }
                    else
                    {
                        throw new BusinessException(Errors.NoExistRisk);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorSelectedClauses);
                }
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorSaveClauses);
            }
        }

        #endregion Clauses


        public Boolean ConvertProspectToInsured(int temporalId, int individualId, string documentNumber)
        {
            try
            {
                DelegateService.underwritingService.ConvertProspectToHolder(temporalId, individualId, documentNumber);
                List<CompanyLiabilityRisk> companyTplRisks = GetCompanyLiabilitiesByTemporalId(temporalId);

                if (companyTplRisks.Count > 0)
                {
                    foreach (CompanyLiabilityRisk liability in companyTplRisks)
                    {
                        CompanyRisk risk = DelegateService.underwritingService.ConvertProspectToInsured(liability.Risk, individualId, documentNumber);
                        liability.Risk.Beneficiaries = risk.Beneficiaries;
                        CreateLiabilityTemporal(liability, false);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorConvertingProspectIntoIndividual);
            }
        }

        public List<CompanyLiabilityRisk> GetTemporalById(int id)
        {
            try
            {

                List<CompanyLiabilityRisk> liabilityRisk = GetCompanyLiabilitiesByTemporalId(id);

                if (liabilityRisk != null)
                {
                    List<CompanyLiabilityRisk> risks = new List<CompanyLiabilityRisk>();

                    foreach (CompanyLiabilityRisk item in liabilityRisk)
                    {
                        CompanyLiabilityRisk risk = GetCompanyLiabilityByRiskId(item.Risk.Id);

                        risk.Risk.Id = item.Risk.Id;
                        risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
                        risk.Risk.Premium = risk.Risk.Coverages.Sum(x => x.PremiumAmount);
                        risks.Add(risk);

                    }

                    return risks;
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTempNoExist);
                }
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorSearchRisk);
            }
        }

        public CompanyLiabilityRisk GetRiskById(int id)
        {
            try
            {
                CompanyLiabilityRisk risk = GetCompanyLiabilityByRiskId(id);

                if (risk != null)
                {
                    risk = GetRiskDescriptions(risk);

                    return risk;
                }
                else
                {
                    throw new BusinessException(Errors.NoRiskWasFound);
                }
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorSearchRisk);
            }
        }

        public CompanyLiabilityRisk GetRiskDescriptions(CompanyLiabilityRisk risk)
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

            if (risk.Risk?.Coverages != null)
            {
                foreach (CompanyCoverage item in risk.Risk.Coverages)
                {
                    if (item.CoverStatusName == null)
                    {
                        item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original);
                    }
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

                    CompanyLiabilityRisk liabilityRisk = GetCompanyLiabilityByRiskId(riskId);

                    if (liabilityRisk.Risk.Status == RiskStatusType.Original || companyPolicy.Endorsement.EndorsementType == EndorsementType.Renewal || liabilityRisk.Risk.Status == RiskStatusType.Included)
                    {
                        result = DelegateService.utilitiesServiceCore.DeletePendingOperation(riskId);

                        DelegateService.underwritingService.DeleteRisk(riskId);
                    }
                    else
                    {
                        liabilityRisk.Risk.Status = RiskStatusType.Excluded;
                        liabilityRisk.Risk.Description = liabilityRisk.Risk.Description + " (" + EnumHelper.GetItemName<RiskStatusType>(RiskStatusType.Excluded) + ")";
                        liabilityRisk.Risk.IsPersisted = true;
                        liabilityRisk.Risk.Policy = companyPolicy;

                        liabilityRisk = QuotateLiability(liabilityRisk, false, false);
                        liabilityRisk.Risk.Coverages.ForEach(x => x.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(x.CoverStatus));

                        liabilityRisk = CreateLiabilityTemporal(liabilityRisk, false);
                        result = true;
                    }

                    if (result)
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorDeleteRisk);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTempNoExist);
                }
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorDeleteRisk);
            }
        }

        public CompanyLiabilityRisk SaveRisk(CompanyLiabilityRisk liabilityRisk, int temporalId, int? riskId, int? RiskEndorsementType)
        {

            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (policy != null)
                {
                    if (liabilityRisk != null && liabilityRisk.Risk != null)
                    {
                        liabilityRisk.Risk.Beneficiaries = new List<CompanyBeneficiary>();

                        liabilityRisk.Risk.CoveredRiskType = policy.Product?.CoveredRisk?.CoveredRiskType;
                        liabilityRisk.Risk.Policy = policy;

                        if (policy.Endorsement.EndorsementType == EndorsementType.Modification)
                        {
                            liabilityRisk.Risk.Status = RiskStatusType.Included;
                        }

                        List<CompanyLiabilityRisk> risks = GetCompanyLiabilitiesByTemporalId(temporalId);
                        foreach (CompanyLiabilityRisk risk in risks.Where(b => (b.Risk?.Status == RiskStatusType.Included) || ((b.Risk?.Status == RiskStatusType.Original) && (b.Risk?.OriginalStatus == null))))
                        {
                            if (risk.FullAddress == liabilityRisk.FullAddress)
                            {
                            }
                        }
                        if (liabilityRisk.Risk?.Id == 0)
                        {
                            if (risks.Count < policy.Product.CoveredRisk.MaxRiskQuantity || risks.Where(x => x.Risk.Status != RiskStatusType.Excluded).Count() < policy.Product.CoveredRisk.MaxRiskQuantity)
                            {
                                if (policy.DefaultBeneficiaries != null && policy.DefaultBeneficiaries.Count > 0)
                                {
                                    liabilityRisk.Risk.Beneficiaries = policy.DefaultBeneficiaries;
                                }

                                else if (policy.Product.Id == (int)Products.RCE)
                                {
                                   if (risks.Count > 0)
                                    {
                                       liabilityRisk.Risk.Number = risks.Where(x => x.Risk.Number != 0).Count() + 1;
                                    }
                                   else
                                    {
                                        liabilityRisk.Risk.Number = 1;
                                    }

                                   AutoMapperAssembler.CreateMapCompanyBeneficiary();
                                   CompanyBeneficiary beneficiary = DelegateService.underwritingService.GetBeneficiaryByPrefixId(policy.Prefix.Id);
                                   if (beneficiary != null)
                                   {
                                        beneficiary.BeneficiaryType.Id = UtlCore.Configuration.KeySettings.NotApplyBeneficiaryTypeId;
                                        liabilityRisk.Risk.Beneficiaries.Add(ModelAssembler.CreateCompanyBeneficiary(beneficiary));
                                   }
                                   else
                                   {
                                       throw new BusinessException(Errors.BeneficiaryNotParameterized);
                                    }
                                }
                                else
                                {
                                    int countRiskNumber = DelegateService.underwritingService.GetEndorsementRiskCount(policy.Endorsement.PolicyId, (EndorsementType)policy.Endorsement.EndorsementType);//agregar el metodo creado GetEndorsementRiskCount
                                    int? maxRiskNumTemporal = risks?.OrderByDescending(x => x.Risk?.Number).FirstOrDefault()?.Risk?.Number;
                                    if (maxRiskNumTemporal.HasValue)
                                    {
                                        liabilityRisk.Risk.Number = maxRiskNumTemporal.GetValueOrDefault() + 1;
                                    }
                                    else
                                    {
                                        liabilityRisk.Risk.Number = countRiskNumber + 1;
                                    }
                                    ModelAssembler.CreateMapCompanyBeneficiary();
                                    liabilityRisk.Risk.Beneficiaries.Add(ModelAssembler.CreateBeneficiaryFromInsured(liabilityRisk.Risk.MainInsured));
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
                                    liabilityRisk = SetDataEmission(liabilityRisk);
                                    break;
                                case EndorsementType.Modification:
                                    liabilityRisk = SetDataModification(liabilityRisk);
                                    break;
                                default:
                                    break;
                            }
                        }

                        liabilityRisk = CreateLiabilityTemporal(liabilityRisk, false);
                        return liabilityRisk;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSaveRisk);
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
                    throw new Exception(Errors.ErrorSaveRisk);
                }
            }
        }

        private CompanyLiabilityRisk SetDataEmission(CompanyLiabilityRisk liability)
        {
            try
            {
                CompanyLiabilityRisk liabilityOld = GetCompanyLiabilityByRiskId(liability.Risk.Id);

                liability.Risk.Beneficiaries = liabilityOld.Risk.Beneficiaries;
                liability.Risk.Text = liabilityOld.Risk.Text;
                liability.Risk.Clauses = liabilityOld.Risk.Clauses;
                liability.Risk.SecondInsured = liabilityOld.Risk.SecondInsured;
                liability.Risk.Number = liabilityOld.Risk.Number;

                foreach (CompanyCoverage coverage in liability.Risk.Coverages)
                {
                    if (!coverage.IsPrimary)
                    {
                        coverage.LimitAmount = coverage.DeclaredAmount;
                    }
                }

                return liability;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        private CompanyLiabilityRisk SetDataModification(CompanyLiabilityRisk liabilityRisk)
        {
            try
            {
                CompanyLiabilityRisk riskOld = GetCompanyLiabilityByRiskId(liabilityRisk.Risk.Id);

                liabilityRisk.Risk.Beneficiaries = riskOld.Risk.Beneficiaries;
                liabilityRisk.Risk.RiskId = riskOld.Risk.RiskId;
                liabilityRisk.Risk.Text = riskOld.Risk.Text;
                liabilityRisk.Risk.Status = riskOld.Risk.Status;
                liabilityRisk.Risk.Clauses = riskOld.Risk.Clauses;
                liabilityRisk.Risk.OriginalStatus = riskOld.Risk.OriginalStatus;
                liabilityRisk.Risk.Number = riskOld.Risk.Number;
                if (liabilityRisk.Risk.Status != RiskStatusType.Included && liabilityRisk.Risk.Status != RiskStatusType.Excluded)
                {
                    liabilityRisk.Risk.Status = RiskStatusType.Modified;
                }

                foreach (CompanyCoverage coverage in liabilityRisk.Risk.Coverages)
                {
                    if (!coverage.IsPrimary)
                    {
                        coverage.LimitAmount = coverage.DeclaredAmount;
                    }
                }
                return liabilityRisk;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public CompanyLiabilityRisk GetPremium(CompanyLiabilityRisk liabilityRisk, List<CompanyCoverage> coverages, List<DynamicConcept> dynamicProperties, int temporalId)
        {
            try
            {
                liabilityRisk.Risk.IsPersisted = true;
                liabilityRisk.Risk.Coverages = coverages;
                liabilityRisk.Risk.DynamicProperties = dynamicProperties;

                CompanyPolicy liabilityPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                liabilityPolicy.IsPersisted = true;
                liabilityRisk.Risk.Policy = liabilityPolicy;

                liabilityRisk = QuotateLiability(liabilityRisk, true, true);
                liabilityRisk.Risk.Coverages.ForEach(x => x.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(x.CoverStatus.Value));

                return liabilityRisk;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private int GetValidRisks(List<CompanyLiabilityRisk> liabilityRisks, CompanyPolicy policy)
        {
            if (policy.Endorsement.EndorsementType == EndorsementType.Modification)
            {
                var countRisk = 0;
                foreach (var item in liabilityRisks)
                {

                    CompanyLiabilityRisk risk = GetCompanyLiabilityByRiskId(item.Risk.Id);
                    if (risk.Risk.Status != RiskStatusType.Excluded)
                    {
                        countRisk++;
                    }
                }
                return countRisk;
            }
            else
            {
                return liabilityRisks.Count;
            }
        }

        public Boolean SaveClause(int riskId, List<CompanyClause> clauses)
        {
            try
            {
                if (clauses != null)
                {
                    CompanyLiabilityRisk risk = GetCompanyLiabilityByRiskId(riskId);
                    if (risk.Risk.Id > 0)
                    {
                        risk.Risk.Clauses = clauses;

                        if (risk != null)
                        {
                            CreateLiabilityTemporal(risk, false);
                            return true;
                        }
                        else
                        {
                            throw new Exception(Errors.ErrorSaveClauses);
                        }
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorSaveClauses);
                    }
                }
                else
                {
                    throw new Exception(Errors.ErrorSaveClauses);
                }
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorSaveClauses);
            }
        }

        #region Coverages

        public CompanyCoverage ExcludeCoverage(int temporalId, int riskId, int riskCoverageId, string description)
        {
            CompanyCoverage coverage = DelegateService.underwritingService.GetCompanyCoverageByRiskCoverageId(riskCoverageId);
            if (coverage != null)
            {
                CompanyPolicy liabilityPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

                CompanyLiabilityRisk risk = GetCompanyLiabilityByRiskId(riskId);

                coverage.Description = description;
                coverage.SubLineBusiness = risk.Risk.Coverages.First(x => x.RiskCoverageId == riskCoverageId).SubLineBusiness;
                coverage.CoverStatus = CoverageStatusType.Excluded;
                coverage.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Excluded);
                coverage.EndorsementType = liabilityPolicy.Endorsement.EndorsementType;
                coverage.CurrentFrom = liabilityPolicy.CurrentFrom;
                risk.Risk.Policy = liabilityPolicy;
                coverage = QuotationCompanyCoverage(risk, coverage, false, false);

            }
            return coverage;
        }

        public Boolean SaveCoverages(int temporalId, int riskId, List<CompanyCoverage> coverages)
        {
            try
            {
                CompanyPolicy liabilityPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                liabilityPolicy.IsPersisted = true;

                CompanyLiabilityRisk liabilityRisk = GetCompanyLiabilityByRiskId(riskId);
                liabilityRisk.Risk.IsPersisted = true;

                if (coverages != null)
                {
                    liabilityRisk.Risk.Policy = liabilityPolicy;
                    liabilityRisk.Risk.Coverages = coverages;

                    liabilityRisk = QuotateLiability(liabilityRisk, false, true);
                    liabilityRisk = CreateLiabilityTemporal(liabilityRisk, false);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorSaveCoverages);
            }
        }

        public List<CompanyCoverage> GetCoverageByCoverageId(int coverageId, int riskId, int temporalId, int groupCoverageId)
        {
            try
            {
                CompanyPolicy liabilityPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (liabilityPolicy != null)
                {
                    CompanyLiabilityRisk liabilityRisk = GetRiskById(riskId);


                    var coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(liabilityPolicy.Product.Id, groupCoverageId, liabilityPolicy.Prefix.Id);
                    if (coverages != null)
                    {
                        coverages = GetCoveragesByRiskId(liabilityPolicy, coverages);
                        coverages = coverages.Where(x => x.Id == coverageId).ToList();
                        liabilityRisk.Risk.IsPersisted = true;
                        liabilityPolicy.IsPersisted = true;
                        liabilityRisk.Risk.Policy = liabilityPolicy;
                        //Ejecutar reglas Pre
                        var ciacoverages = new ConcurrentBag<CompanyCoverage>();
                        object obj = new object();
                        liabilityRisk.Risk.Coverages = coverages;
                        TP.Parallel.For(0, coverages.Count, coverageRow =>
                        {
                            CompanyCoverage coverage;
                            lock (obj)
                            {
                                coverage = coverages[coverageRow];
                                if (coverage.RuleSetId.HasValue)
                                {
                                    coverage = RunRulesCompanyCoverage(liabilityRisk, coverage, coverage.RuleSetId.Value);
                                }
                                ciacoverages.Add(coverage);
                            }

                        });
                        return ciacoverages.ToList();
                        //    coverage.EndorsementType = liabilityPolicy.Endorsement.EndorsementType;
                        //coverage.CurrentFrom = liabilityPolicy.CurrentFrom;
                        //coverage.CurrentTo = liabilityPolicy.CurrentTo;
                        //coverage.Days = Convert.ToInt32((coverage.CurrentTo.Value - coverage.CurrentFrom).TotalDays);

                        //if (coverages.EndorsementType == EndorsementType.Modification)
                        //{
                        //    coverage.CoverStatus = CoverageStatusType.Included;
                        //}

                        //coverage.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus);


                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorTemporalNotFound);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTemporalNotFound);
                }

            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorSearchCoverage);
            }
        }

        /// <summary>
        /// Gets the coverages by risk identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="coverages">The coverages.</param>
        /// <returns></returns>
        private List<CompanyCoverage> GetCoveragesByRiskId(CompanyPolicy policy, List<CompanyCoverage> coverages)
        {
            if (policy != null && policy.Id > 0)
            {
                string coverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original);
                coverages.AsParallel().ForAll(item =>
                {
                    item.EndorsementType = policy.Endorsement.EndorsementType;
                    item.CalculationType = Core.Services.UtilitiesServices.Enums.CalculationType.Prorate;
                    item.CurrentFrom = policy.CurrentFrom;
                    item.CurrentTo = policy.CurrentTo;
                    item.RateType = RateType.Percentage;
                    item.LimitAmount = 0;
                    item.SubLimitAmount = 0;
                    item.CoverStatus = CoverageStatusType.Original;
                    item.CoverStatusName = coverStatusName;
                });
            }

            return coverages;
        }


        public List<CompanyCoverage> GetCoveragesByProductIdGroupCoverageId(int temporalId, int productId, int groupCoverageId, int prefixId)
        {
            try
            {

                CompanyPolicy liabilityPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

                if (liabilityPolicy.Id > 0)
                {
                    List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);
                    foreach (CompanyCoverage item in coverages)
                    {
                        IMapper mapper = AutoMapperAssembler.CreateMapCompanyDeductible();
                        List<CompanyDeductible> deductibles = mapper.Map<List<Deductible>, List<CompanyDeductible>>(DelegateService.underwritingService.GetDeductiblesByCoverageId(item.Id)).ToList();

                        if (deductibles != null)
                        {
                            item.Deductible = deductibles.Where(x => x.IsDefault == true).FirstOrDefault();

                        }

                        item.EndorsementType = liabilityPolicy.Endorsement.EndorsementType;
                        item.CalculationType = Core.Services.UtilitiesServices.Enums.CalculationType.Prorate;
                        item.CurrentFrom = liabilityPolicy.CurrentFrom;
                        item.CurrentTo = liabilityPolicy.CurrentTo;
                        item.RateType = RateType.Percentage;
                        item.LimitAmount = 0;
                        item.SubLimitAmount = 0;
                        if (liabilityPolicy.Endorsement.EndorsementType == EndorsementType.Emission ||
                            liabilityPolicy.Endorsement.EndorsementType == EndorsementType.Renewal)
                        {
                            item.CoverStatus = CoverageStatusType.Original;
                        }
                        else
                        {
                            item.CoverStatus = CoverageStatusType.Included;
                        }
                        item.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(item.CoverStatus));
                    }

                    coverages = coverages.Where(x => x.IsSelected == true).ToList();

                    return coverages;
                }
                else
                {
                    throw new BusinessException(Errors.ErrorSearchCoverages);
                }
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorQueryCoverageGroups);
            }
        }

        public List<CompanyCoverage> GetAllyCoverageByCoverage(int tempId, int riskId, int groupCoverageId, CompanyCoverage companyCoverage)
        {
            try
            {
                List<CompanyCoverage> coverages = new List<CompanyCoverage>();

                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(tempId, false);
                coverages = DelegateService.underwritingService.GetAllyCompanyCoveragesByCoverageIdProductIdGroupCoverageId(companyCoverage.Id, policy.Product.Id, groupCoverageId);

                CompanyLiabilityRisk liabilityRisk = new CompanyLiabilityRisk
                {
                    Risk = new CompanyRisk
                    {
                        Id = riskId,
                        Policy = new CompanyPolicy
                        {
                            Id = tempId,
                            Endorsement = new CompanyEndorsement
                            {
                                EndorsementType = (EndorsementType)companyCoverage.EndorsementType
                            }
                        }
                    }
                };

                List<CompanyCoverage> quotateCoverages = new List<CompanyCoverage>();

                foreach (CompanyCoverage coverage in coverages)
                {
                    coverage.EndorsementType = companyCoverage.EndorsementType;
                    coverage.DeclaredAmount = companyCoverage.DeclaredAmount;
                    CompanyPolicy liabilityPolicy = new CompanyPolicy { Id = tempId, Endorsement = new CompanyEndorsement { EndorsementType = (EndorsementType)coverage.EndorsementType } };
                    companyCoverage = QuotationCompanyCoverage(liabilityRisk, coverage, false, true);

                    if (policy.Endorsement.EndorsementType == EndorsementType.Modification)
                    {
                        if (companyCoverage.CoverStatus != null && companyCoverage.CoverStatus == CoverageStatusType.NotModified)
                            companyCoverage.CoverStatus = CoverageStatusType.Modified;
                        companyCoverage.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Modified);
                    }

                    quotateCoverages.Add(companyCoverage);
                }
                return quotateCoverages;
            }
            catch (Exception)
            {

                throw new Exception(Errors.ErrorGettingAlliedCoverage);
            }
        }

        public List<CompanyCoverage> GetAddCoveragesByCoverage(int tempId, int riskId, int groupCoverageId, CompanyCoverage companyCoverage)
        {
            try
            {
                List<CompanyCoverage> coverages = new List<CompanyCoverage>();

                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(tempId, false);
                coverages = DelegateService.underwritingService.GetAddCompanyCoveragesByCoverageIdProductIdGroupCoverageId(companyCoverage.Id, policy.Product.Id, groupCoverageId);

                CompanyLiabilityRisk liabilityRisk = new CompanyLiabilityRisk
                {
                    Risk = new CompanyRisk
                    {
                        Id = riskId,
                        Policy = new CompanyPolicy
                        {
                            Id = tempId,
                            Endorsement = new CompanyEndorsement
                            {
                                EndorsementType = (EndorsementType)companyCoverage.EndorsementType
                            }
                        }
                    }
                };

                List<CompanyCoverage> quotateCoverages = new List<CompanyCoverage>();
                foreach (CompanyCoverage coverage in coverages)
                {
                    coverage.EndorsementType = companyCoverage.EndorsementType;
                    coverage.DeclaredAmount = companyCoverage.DeclaredAmount;
                    coverage.Rate = companyCoverage.Rate;
                    coverage.CurrentFrom = policy.CurrentFrom;
                    coverage.CurrentTo = policy.CurrentTo;
                    coverage.RateType = companyCoverage.RateType;
                    coverage.CalculationType = companyCoverage.CalculationType;

                    CompanyPolicy liabilityPolicy = new CompanyPolicy { Id = tempId, Endorsement = new CompanyEndorsement { EndorsementType = (EndorsementType)coverage.EndorsementType } };
                    companyCoverage = QuotationCompanyCoverage(liabilityRisk, coverage, false, true);
                    quotateCoverages.Add(companyCoverage);
                    if (policy.Endorsement.EndorsementType == EndorsementType.Modification)
                    {
                        if (companyCoverage.CoverStatus != null && companyCoverage.CoverStatus == CoverageStatusType.NotModified)
                            companyCoverage.CoverStatus = CoverageStatusType.Modified;
                        companyCoverage.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Modified);
                    }
                }

                return quotateCoverages;
            }
            catch (Exception ex)
            {
                throw new Exception(Errors.ErrorGettingAdditionalCoverages, ex);
            }
        }

        public CompanyCoverage QuotationCoverage(CompanyCoverage coverage, int riskId, int temporalId, int endorsementType, bool runRulesPost)
        {
            try
            {
                CompanyLiabilityRisk liability = new CompanyLiabilityRisk
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

                var endorsmentType = liability.Risk.Policy.Endorsement.EndorsementType;
                if (endorsmentType != EndorsementType.Emission && endorsmentType != EndorsementType.Renewal && endorsmentType != EndorsementType.EffectiveExtension)
                {
                    if (coverage.CoverStatus == CoverageStatusType.NotModified && coverage.PremiumAmount != 0)
                    {
                        coverage.CoverStatus = CoverageStatusType.Modified;
                        coverage.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Modified));
                    }
                }

                coverage = QuotationCompanyCoverage(liability, coverage, false, runRulesPost);
                coverage.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus);
                return coverage;

            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorQuotationCoverage);
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
                CompanyLiabilityRisk liabilityRisk = GetCompanyLiabilityByRiskId(riskId);

                if (liabilityRisk.Risk.Id > 0)
                {
                    liabilityRisk.Risk.Beneficiaries = beneficiaries;

                    if (liabilityRisk != null)
                    {
                        CreateLiabilityTemporal(liabilityRisk, false);
                        return beneficiaries;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorSaveBeneficiaries);
                    }
                }
                else
                {
                    throw new Exception(Errors.ErrorTempNoExist);
                }
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorSaveBeneficiaries);
            }
        }

        #endregion Beneficiary

        #region Reglas
        public CompanyLiabilityRisk RunRulesRiskPreLiability(int policyId, int? ruleSetId)
        {
            try
            {
                CompanyLiabilityRisk liabilityRisk = new CompanyLiabilityRisk
                {
                    Risk = new CompanyRisk
                    {
                        Policy = new CompanyPolicy
                        {
                            Id = policyId,
                            IsPersisted = false
                        },
                        CoveredRiskType = CoveredRiskType.Location,
                        IsPersisted = true,
                    }
                };

                if (ruleSetId.GetValueOrDefault() > 0)
                {
                    liabilityRisk.Risk.IsPersisted = true;

                    liabilityRisk = RunRulesRisk(liabilityRisk, ruleSetId.Value);
                }

                return liabilityRisk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> RunRulesCoveragesPreLiability(int temporalId, int riskId, List<CompanyCoverage> coverages)
        {
            try
            {

                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                CompanyLiabilityRisk companyLiabilityRisk = GetCompanyLiabilityByRiskId(riskId);
                List<CompanyLiabilityRisk> CompanyLiabilityRisks = GetCompanyLiabilitiesByTemporalId(temporalId);

                object obj = new object();

                if (companyLiabilityRisk != null)
                {

                    companyLiabilityRisk.Risk.Policy = companyPolicy;
                    companyLiabilityRisk.Risk.IsPersisted = true;
                    companyLiabilityRisk.Risk.CoveredRiskType = CoveredRiskType.Location;

                    TP.Parallel.For(0, coverages.Count, coverageRow =>
                    {
                        lock (obj)
                        {
                            if (coverages[coverageRow].RuleSetId.HasValue)
                            {
                                coverages[coverageRow] = RunRulesCompanyCoverage(companyLiabilityRisk, coverages[coverageRow], coverages[coverageRow].RuleSetId.Value);
                            }
                        }
                    });
                }
                return coverages;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Metodo utilizado para la ejecución de las reglas POST de coverturas
        /// </summary>
        /// <param name="temporalId">temporal de la poliza</param>
        /// <param name="riskId">Riesgo que se esta validando</param>
        /// <param name="coverages">Coberturas asociadas al riesgo</param>
        public List<CompanyCoverage> RunRulesPostCoverages(int temporalId, int riskId, List<CompanyCoverage> coverages)
        {
            try
            {

                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                CompanyLiabilityRisk companyLiabilityRisk = GetCompanyLiabilityByRiskId(riskId);
                List<CompanyLiabilityRisk> CompanyLiabilityRisks = GetCompanyLiabilitiesByTemporalId(temporalId);

                object obj = new object();

                if (companyLiabilityRisk != null)
                {

                    companyLiabilityRisk.Risk.Policy = companyPolicy;
                    companyLiabilityRisk.Risk.IsPersisted = true;
                    companyLiabilityRisk.Risk.CoveredRiskType = CoveredRiskType.Location;
                    TP.Parallel.For(0, coverages.Count, coverageRow =>
                    {
                        CompanyCoverage coverage;
                        lock (obj)
                        {
                            coverage = coverages[coverageRow];
                            if (coverage.PosRuleSetId.GetValueOrDefault() > 0)
                            {
                                coverage = RunRulesCompanyCoverage(companyLiabilityRisk, coverage, coverage.PosRuleSetId.Value);
                            }
                        }
                    });
                }
                return coverages;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyPolicy UpdateRisks(int temporalId)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                List<CompanyLiabilityRisk> CompanyLiabilityRisks = GetCompanyLiabilitiesByTemporalId(temporalId);

                if (companyPolicy != null)
                {
                    companyPolicy.IsPersisted = true;

                    if (CompanyLiabilityRisks != null && CompanyLiabilityRisks.Any())
                    {
                        TP.Parallel.ForEach(CompanyLiabilityRisks, companyLiabilityRisk =>
                            {
                                companyLiabilityRisk.Risk.Policy = companyPolicy;
                                companyLiabilityRisk.Risk.IsPersisted = true;
                                companyLiabilityRisk?.Risk.Coverages.AsParallel().ForAll(x =>
                                {
                                    x.CurrentTo = companyPolicy.CurrentTo;
                                    x.CurrentFrom = companyPolicy.CurrentFrom;
                                });
                                QuotateLiability(companyLiabilityRisk, false, true);
                                CompanySaveCompanyLiabilityTemporal(companyLiabilityRisk, false);
                            });
                        companyPolicy = DelegateService.underwritingService.UpdatePolicyComponents(companyPolicy.Id);
                        return companyPolicy;
                    }
                    else
                    {
                        return companyPolicy;
                    }
                }
                else
                {
                    throw new Exception(Errors.ErrorTemporalNotFound);

                }
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorUpdatePolicy);
            }
        }
        public CompanyLiabilityRisk CompanySaveCompanyLiabilityTemporal(CompanyLiabilityRisk companyLiabilityRisk, bool isMassive)
        {
            try
            {
                LiabilityDAO liabilityDAO = new LiabilityDAO();
                companyLiabilityRisk = liabilityDAO.CreateLiabilityTemporal(companyLiabilityRisk, isMassive);
                if (companyLiabilityRisk.Risk.Policy.TemporalType != TemporalType.TempQuotation)
                {
                    if (companyLiabilityRisk.Risk.LimitRc == null)
                    {
                        companyLiabilityRisk.Risk.LimitRc = new CompanyLimitRc
                        {
                            Id = DelegateService.commonService.GetParameterByParameterId(3000).NumberParameter.Value,
                            LimitSum = DelegateService.commonService.GetParameterByParameterId(3001).NumberParameter.Value,
                        };
                    }
                    companyLiabilityRisk = liabilityDAO.SaveCompanyLiabilityTemporalTables(companyLiabilityRisk);
                }

                return companyLiabilityRisk;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
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
        public CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType, bool clearPolicies, CompanyModification companyModification)
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
                    if (EndorsementType.Modification == policy.Endorsement.EndorsementType && companyModification != null)
                    {
                        if (policy.Endorsement.Text != null)
                        {
                            policy.Endorsement.Text.TextBody = companyModification.Text;
                            policy.Endorsement.Text.Observations = companyModification.Observations;
                            policy.Endorsement.TicketDate = companyModification.RegistrationDate;
                            policy.Endorsement.TicketNumber = companyModification.RegistrationNumber;
                        }
                    }
                    if (temporalType != TempType.Quotation && policy.Endorsement.EndorsementType != EndorsementType.Cancellation)
                    {
                        ValidateHolder(ref policy);
                    }
                    if (policy.Errors != null && !policy.Errors.Any() && policy.Product.CoveredRisk != null)
                    {
                        List<CompanyLiabilityRisk> LiabilityRisk = GetCompanyLiabilitiesByTemporalId(policy.Id);

                        if (LiabilityRisk != null && LiabilityRisk.Any())
                        {
                            if (LiabilityRisk[0].Risk.Coverages.Exists(x => x.IsPrimary == true && (x.EndorsementLimitAmount != x.DeclaredAmount || x.EndorsementLimitAmount != x.DeclaredAmount || x.SubLimitAmount != x.DeclaredAmount || x.LimitAmount != x.DeclaredAmount)) && policy.Endorsement.EndorsementType == EndorsementType.Emission)
                            {
                                throw new ArgumentException(Errors.ErrorCoveragesZero);
                            }
                            else
                            {
                                if (clearPolicies)
                                {
                                    policy.InfringementPolicies.Clear();
                                    LiabilityRisk.ForEach(x => x.Risk.InfringementPolicies.Clear());
                                }

                                policy = CreateEndorsement(policy, LiabilityRisk);
                                //se agrega la validación para el caso en que tenga un politica de autorzación
                                if (policy?.InfringementPolicies?.Count == 0)
                                {
                                    DelegateService.underwritingService.SaveTextLarge(policy.Endorsement.PolicyId, policy.Endorsement.Id);

                                }
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
                                companyPolicyResult.IsReinsured = policy.IsReinsured;
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
                        companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = string.Join(" - ", policy.Errors.FirstOrDefault().Error) });
                    }
                }
                return companyPolicyResult;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);

                //                throw new BusinessException(Errors.ErrorCreatePolicy);
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
                        case CoveredRiskType.Location:
                            List<CompanyLiabilityRisk> LiabilityRisk = GetCompanyLiabilitiesByTemporalId(policy.Id);

                            var result = LiabilityRisk.Select(x => x.Risk).Where(z => z.MainInsured?.CustomerType == CustomerType.Prospect).Count();
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
        #endregion

        public CompanySummary ConvertProspectToInsuredRisk(CompanyPolicy companyPolicy, int individualId)
        {
            try
            {
                List<CompanyLiabilityRisk> companyLiability = GetCompanyLiabilitiesByTemporalId(companyPolicy.Id);

                if (companyLiability.Count > 0)
                {
                    foreach (CompanyLiabilityRisk liability in companyLiability)
                    {
                        liability.Risk.Policy = companyPolicy;
                        if (liability.Risk.MainInsured.CustomerType == CustomerType.Prospect)
                        {

                            CompanyRisk risk = DelegateService.underwritingService.ConvertProspectToInsuredRisk(liability.Risk, individualId);
                            liability.Risk.Beneficiaries = risk.Beneficiaries;
                        }
                        List<CompanyBeneficiary> listBeneficiary = new List<CompanyBeneficiary>();
                        liability.Risk.Beneficiaries.ToList().ForEach(x =>
                        {
                            if (x.CustomerType == CustomerType.Prospect)
                            {
                                CompanyBeneficiary result = DelegateService.underwritingService.ConvertProspectToBeneficiary(x, individualId);
                                listBeneficiary.Add(result);
                            }
                            else
                            {
                                listBeneficiary.Add(x);
                            }
                        });
                        liability.Risk.Beneficiaries = listBeneficiary;
                        liability.Risk.Description = liability.Risk.MainInsured.Name;
                        CreateLiabilityTemporal(liability, false);
                        CompanyRiskInsured companyRiskInsureds = new CompanyRiskInsured
                        {
                            Insured = liability.Risk.MainInsured,
                            Beneficiaries = liability.Risk.Beneficiaries
                        };
                        companyPolicy.Summary.RisksInsured[0] = companyRiskInsureds;
                        companyPolicy = DelegateService.underwritingService.CompanySavePolicyTemporal(companyPolicy, false);
                    }
                    return companyPolicy.Summary;
                }
                else
                {
                    return companyPolicy.Summary;
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorConvertingProspectIntoIndividual, ex);
            }
        }


        /// <summary>
        /// Gets the company premium.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns></returns>
        public CompanyLiabilityRisk GetCompanyPremium(int policyId, CompanyLiabilityRisk companyLiabilityRisk)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);
                policy.IsPersisted = true;
                companyLiabilityRisk.Risk.Policy = policy;
                //companyLiabilityRisk.ActualDateMovement = DateTime.Now;
                companyLiabilityRisk = QuotateLiability(companyLiabilityRisk, true, true);
                companyLiabilityRisk?.Risk?.Coverages.AsParallel().ForAll(x =>
                {
                    x.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(x?.CoverStatus.Value));
                });
                return companyLiabilityRisk;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.GetBaseException().Message);
            }

        }
    }
}
