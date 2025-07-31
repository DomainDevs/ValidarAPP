using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;
using Sistran.Core.Application.Aircrafts.AircraftBusinessService.EEProvider;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.EEProvider.Business;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.Linq;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.CommonService.Enums;

namespace Sistran.Company.Application.Aircrafts.AircraftBusinessService.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CompanyAircraftBusinessServiceProvider : AircraftBusinessServiceProvider, ICompanyAircraftBusinessService
    {
        public CompanyAircraft CreateCompanyAircraftTemporal(CompanyAircraft companyAircraft)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.CreateCompanyAircraftTemporal(companyAircraft);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateCompanyAircraftTemporal), ex); ;
            }

        }

        public CompanyAircraft UpdateCompanyAircraftTemporal(CompanyAircraft companyAircraft)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.UpdateCompanyAircraftTemporal(companyAircraft);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErorrUpdateCompanyAircraftTemporal), ex);
            }
        }

        public bool DeleteCompanyAircraftTemporal(int riskId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.DeleteCompanyAircraftTemporal(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorDeleteCompanyAircraftTemporal), ex);
            }
        }

        public CompanyAircraft GetCompanyAircraftTemporalByRiskId(int riskId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetCompanyAircraftTemporalByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyAircraftByRiskId), ex);
            }
        }

        public List<CompanyAircraft> GetCompanyAircraftsByTemporalId(int temporalId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetCompanyAircraftsByTemporalId(temporalId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyAircraftsByTemporalId));

            }
        }

        public List<CompanyAircraft> GetCompanyAircraftsByPolicyId(int policyId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetCompanyAircraftsByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyAircraftsByPolicyId), ex);
            }
        }

        public CompanyAircraft RunRulesRisk(CompanyAircraft companyAircraft, int ruleId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.RunRulesRisk(companyAircraft, ruleId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorRunRulesRisk), ex);
            }
        }

        public CompanyAircraft QuotateCompanyAircraft(CompanyAircraft companyAircraft, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.QuotateAircraft(companyAircraft, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorQuotateCompanyAircraft), ex);
            }

        }

        public List<CompanyAircraft> QuotateCompanyAircrafts(List<CompanyAircraft> companyAircrafts, bool runRulesPre, bool runRulesPost)
        {
            throw new System.NotImplementedException();
        }

        public CompanyCoverage QuotateCompanyCoverage(CompanyAircraft companyAircraft, CompanyCoverage companyCoverage, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.Quotate(companyAircraft, companyCoverage, runRulesPre, runRulesPost);
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

        public CompanyAircraft ExcludeCompanyAircraft(int temporalId, int riskId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.ExcludeCompanyAircraft(temporalId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExcludeCompanyAircraft), ex);
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

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyAircraft> companyAircrafts)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.CreateEndorsement(companyPolicy, companyAircrafts);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateEndorsement), ex);
            }
        }

        public List<CompanyAircraft> GetCompanyAircraftsByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetCompanyAircraftsByPolicyIdEndorsementId(policyId, endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyAircraftsByPolicyId), ex);
            }
        }

        public List<CompanyEndorsement> GetCompanyEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int policyId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetCompanyEndorsementByEndorsementTypeIdPolicyId(endorsementTypeId, policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyEndorsement), ex);
            }
        }

        public List<CompanyCoverage> GetCoveragesByRiskId(int riskId, int temporalId)
        {
            AircraftBusiness AircraftBusiness = new AircraftBusiness();
            return AircraftBusiness.GetCoveragesByRiskId(riskId, temporalId);

        }
        public List<CompanyCoverage> GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, riskId);
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
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetLeapYear();
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
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.DeleteCompanyRisk(temporalId, riskId);
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
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }

        public bool saveInsuredObject(int riskId, InsuredObject insuredObject, int tempId, int groupCoverageId)
        {
            AircraftBusiness AircraftBusiness = new AircraftBusiness();
            return AircraftBusiness.saveInsuredObject(riskId, insuredObject, tempId, groupCoverageId);
        }

        public List<CompanyAircraft> GetCompanyAircraftsByEndorsementId(int endorsementId)
        {
            try
            {
                AircraftBusiness aircraftBusiness = new AircraftBusiness();
                return aircraftBusiness.GetCompanyAircraftsByEndorsementId(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyAircraftsByEndorsementId), ex);
            }
        }

        public List<CompanyAircraft> GetCompanyAircraftByInsuredId(int insuredId)
        {
            try
            {
                AircraftBusiness aircraftBusiness = new AircraftBusiness();
                return aircraftBusiness.GetCompanyAircraftByInsuredId(insuredId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyAircraftsByEndorsementId), ex);
            }
        }

        public CompanyAircraft GetCompanyAircraftByRiskId(int riskId)
        {
            try
            {
                AircraftBusiness aircraftBusiness = new AircraftBusiness();
                return aircraftBusiness.GetCompanyAircraftByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyAircraftsByEndorsementId), ex);
            }
        }

        public List<CompanyAircraft> GetCompanyAircraftsByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            try
            {
                AircraftBusiness aircraftBusiness = new AircraftBusiness();
                List<CompanyAircraft> companyAircrafts = new List<CompanyAircraft>();
                switch (moduleType)
                {
                    case ModuleType.Emission:
                        companyAircrafts = aircraftBusiness.GetCompanyAircraftsByEndorsementId(endorsementId);
                        break;
                    case ModuleType.Claim:
                        companyAircrafts = aircraftBusiness.GetCompanyClaimAircraftsByEndorsementId(endorsementId);
                        break;
                }

                return companyAircrafts;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyAircraftsByEndorsementId), ex);
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

                    if (temporalType != TempType.Quotation)
                    {
                        ValidateHolder(ref policy);
                    }
                    if (!policy.Errors.Any())
                    {
                        switch (policy.Product.CoveredRisk.SubCoveredRiskType)
                        {
                            case SubCoveredRiskType.Aircraft:
                                List<CompanyAircraft> companyAircrafts = GetCompanyAircraftsByTemporalId(policy.Id);
                                if (companyAircrafts != null && companyAircrafts.Any())
                                {
                                    policy = CreateEndorsement(policy, companyAircrafts);
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
                        //}
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
                            List<CompanyAircraft> aircraft = GetCompanyAircraftsByTemporalId(policy.Id);

                            var result = aircraft.Select(x => x.Risk).Where(z => z.MainInsured?.CustomerType == CustomerType.Prospect).Count();
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