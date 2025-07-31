using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Marines.MarineBusinessService.Models.Base;
using Sistran.Core.Application.Marines.MarineBusinessService.EEProvider;
using Sistran.Company.Application.Marines.MarineBusinessService.EEProvider.Business;
using Sistran.Company.Application.Marines.MarineBusinessService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.Linq;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.CommonService.Enums;

namespace Sistran.Company.Application.Marines.MarineBusinessService.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CompanyMarineBusinessServiceProvider : MarineBusinessServiceProvider, ICompanyMarineBusinessService
    {
        public CompanyMarine CreateCompanyMarineTemporal(CompanyMarine companyMarine)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.CreateCompanyMarineTemporal(companyMarine);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateCompanyMarineTemporal), ex); ;
            }

        }

        public CompanyMarine UpdateCompanyMarineTemporal(CompanyMarine companyMarine)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.UpdateCompanyMarineTemporal(companyMarine);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErorrUpdateCompanyMarineTemporal), ex);
            }
        }
        
        public bool DeleteCompanyMarineTemporal(int riskId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.DeleteCompanyMarineTemporal(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorDeleteCompanyMarineTemporal), ex);
            }
        }
        
        public CompanyMarine GetCompanyMarineTemporalByRiskId(int riskId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetCompanyMarineTemporalByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyMarineByRiskId), ex);
            }
        }
        
        public List<CompanyMarine> GetCompanyMarinesByTemporalId(int temporalId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetCompanyMarinesByTemporalId(temporalId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyMarinesByTemporalId));

            }
        }
        
        public List<CompanyMarine> GetCompanyMarinesByPolicyId(int policyId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetCompanyMarinesByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyMarinesByPolicyId), ex);
            }
        }
        
        public CompanyMarine RunRulesRisk(CompanyMarine companyMarine, int ruleId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.RunRulesRisk(companyMarine, ruleId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorRunRulesRisk), ex);
            }
        }
        
        public CompanyMarine QuotateCompanyMarine(CompanyMarine companyMarine, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.QuotateMarine(companyMarine, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorQuotateCompanyMarine), ex);
            }

        }
        
        public List<CompanyMarine> QuotateCompanyMarines(List<CompanyMarine> companyMarines, bool runRulesPre, bool runRulesPost)
        {
            throw new System.NotImplementedException();
        }
        
        public CompanyCoverage QuotateCompanyCoverage(CompanyMarine companyMarine, CompanyCoverage companyCoverage, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.Quotate(companyMarine, companyCoverage, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorQuotateCompanyCoverage), ex);
            }
        }

        public List<CompanyCoverage> QuotateCompanyCoverages(List<CompanyCoverage> companyCoverages, bool runRulesPre, bool runRulesPost)
        {
            throw new System.NotImplementedException();
        }

        public CompanyMarine ExcludeCompanyMarine(int temporalId, int riskId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.ExcludeCompanyMarine(temporalId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExcludeCompanyMarine), ex);
            }
        }
        
        public CompanyCoverage ExcludeCompanyCoverage(int temporalId, int riskId, int coverageId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.ExcludeCompanyCoverage(temporalId, riskId, coverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExcludeCompanyCoverage), ex);
            }
        }

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyMarine> companyMarines)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.CreateEndorsement(companyPolicy, companyMarines);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateEndorsement), ex);
            }
        }

        public List<CompanyMarine> GetCompanyMarinesByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetCompanyMarinesByPolicyIdEndorsementId(policyId,endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyMarinesByPolicyId), ex);
            }
        }
     
        public List<CompanyEndorsement> GetCompanyEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int policyId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetCompanyEndorsementByEndorsementTypeIdPolicyId(endorsementTypeId, policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyEndorsement), ex);
            }
        }

        public List<CompanyCoverage> GetCoveragesByRiskId(int riskId, int temporalId) {
            MarineBusiness MarineBusiness = new MarineBusiness();
            return MarineBusiness.GetCoveragesByRiskId(riskId, temporalId);

        }
        public List<CompanyCoverage> GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyCoveragesByInsuredObjectId), ex); //Modificar
            }
        }

        public bool GetLeapYear()
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetLeapYear();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public bool DeleteCompanyRisk(int temporalId, int riskId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.DeleteCompanyRisk(temporalId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> GetTemporalCoveragesByRiskIdInsuredObjectId(int riskId, int insuredObjectId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
           
        }

        public bool saveInsuredObject(int riskId, InsuredObject insuredObject, int tempId, int groupCoverageId)
        {
            MarineBusiness MarineBusiness = new MarineBusiness();
            return MarineBusiness.saveInsuredObject(riskId, insuredObject, tempId, groupCoverageId);
        }

        public List<CompanyMarine> GetCompanyMarinesByEndorsementId(int endorsementId)
        {
            try
            {
                MarineBusiness marineBusiness = new MarineBusiness();
                return marineBusiness.GetCompanyAircraftsByEndorsementId(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyMarinesByEndorsementId), ex);
            }
        }

        public List<CompanyMarine> GetCompanyMarinesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            List<CompanyMarine> companyMarines = new List<CompanyMarine>();
            MarineBusiness marineBusiness = new MarineBusiness();
            switch (moduleType)
            {
                case ModuleType.Emission:
                    companyMarines = marineBusiness.GetCompanyAircraftsByEndorsementId(endorsementId);
                    break;
                case ModuleType.Claim:
                    companyMarines = marineBusiness.GetClaimCompanyMarinesByEndorsementId(endorsementId);
                    break;
            }

            return companyMarines;
        }

        public List<CompanyMarine> GetCompanyMarinesByInsuredId(int insuredId)
        {
            try
            {
                MarineBusiness marineBusiness = new MarineBusiness();
                return marineBusiness.GetCompanyMarinesByInsuredId(insuredId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyMarine GetCompanyMarineByRiskId(int riskId)
        {
            try
            {
                MarineBusiness marineBusiness = new MarineBusiness();
                return marineBusiness.GetCompanyMarineByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

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
                    companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorTemporalNotFound });
                }
                else
                {
                    policy.Errors = new List<ErrorBase>();
                    if (policy.Summary == null || policy.Summary.Premium == 0)
                    {
                        companyPolicyResult.IsError = true;
                        companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorTempPremiumZero });
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
                                case SubCoveredRiskType.Marine:
                                    List<CompanyMarine> companyMarines = GetCompanyMarinesByTemporalId(policy.Id);
                                    if (companyMarines != null && companyMarines.Any())
                                    {
                                        policy = CreateEndorsement(policy, companyMarines);
                                    }
                                    else
                                    {
                                        throw new ArgumentException(Errors.NoExistRisk);
                                    }
                                    if (temporalType != TempType.Quotation)
                                    {
                                        companyPolicyResult.Message = string.Format(Errors.PolicyNumber, policy.DocumentNumber);
                                        companyPolicyResult.DocumentNumber = policy.DocumentNumber;
                                        companyPolicyResult.EndorsementId = policy.Endorsement.Id;
                                        companyPolicyResult.EndorsementNumber = policy.Endorsement.Number;
                                        companyPolicyResult.TemporalId = policy.Endorsement.TemporalId;
                                    }
                                    else
                                    {
                                        companyPolicyResult.Message = string.Format(Errors.QuotationNumber, policy.Endorsement.QuotationId.ToString());
                                        companyPolicyResult.DocumentNumber = Convert.ToDecimal(policy.Endorsement.QuotationId);
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
                throw new BusinessException(Errors.ErrorCreatePolicy);
            }

        }

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
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorHolderNoInsuredRole });
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
                        case CoveredRiskType.Surety:
                            List<CompanyMarine> marine = GetCompanyMarinesByTemporalId(policy.Id);

                            var result = marine.Select(x => x.Risk).Where(z => z.MainInsured?.CustomerType == CustomerType.Prospect).Count();
                            if (result > 0)
                            {
                                policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorInsuredNoInsuredRole });
                            }
                            break;
                    }
                }
            }

        }
    }
}