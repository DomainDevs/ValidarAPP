using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Core.Application.Transports.TransportBusinessService.EEProvider;
using Sistran.Company.Application.Transports.TransportBusinessService.EEProvider.Business;
using Sistran.Company.Application.Transports.TransportBusinessService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.Linq;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Transports.TransportBusinessService.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Company.Application.Transports.TransportBusinessService.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CompanyTransportBusinessServiceProvider : TransportBusinessServiceProvider, ICompanyTransportBusinessService
    {
        public CompanyTransport CreateCompanyTransportTemporal(CompanyTransport companyTransport)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.CreateCompanyTransportTemporal(companyTransport);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateCompanyTransportTemporal), ex); ;
            }

        }

        public CompanyPolicy UpdateCompanyRisks(int temporalId)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                List<CompanyTransport> companyTplRisks = GetCompanyTransportsByTemporalId(temporalId);


                if (companyTplRisks != null && companyTplRisks.Any())
                {
                    foreach (CompanyTransport companyTplRisk in companyTplRisks)
                    {
                        companyTplRisk.Risk.Policy = companyPolicy;
                        companyTplRisk.Risk.Coverages.ForEach(x => x.CurrentTo = companyPolicy.CurrentTo);
                        companyTplRisk.Risk.Coverages.ForEach(x => x.CurrentFrom = companyPolicy.CurrentFrom);
                        TransportBusiness transportBusiness = new TransportBusiness();
                        transportBusiness.SaveCompanyTransportTemporalTables(companyTplRisk);

                    }
                    companyPolicy = DelegateService.underwritingService.UpdatePolicyComponents(companyPolicy.Id);
                    return companyPolicy;
                }
                else
                {
                    throw new BusinessException(Errors.ErrorCreateCompanyTransportTemporal);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorCreateCompanyTransportTemporal);

            }


        }

        public CompanyTransport UpdateCompanyTransportTemporal(CompanyTransport companyTransport)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.UpdateCompanyTransportTemporal(companyTransport);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErorrUpdateCompanyTransportTemporal), ex);
            }
        }

        public bool DeleteCompanyTransportTemporal(int riskId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.DeleteCompanyTransportTemporal(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorDeleteCompanyTransportTemporal), ex);
            }
        }

        public CompanyTransport GetCompanyTransportTemporalByRiskId(int riskId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCompanyTransportTemporalByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyTransportByRiskId), ex);
            }
        }

        public List<CompanyTransport> GetCompanyTransportsByTemporalId(int temporalId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCompanyTransportsByTemporalId(temporalId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyTransportsByTemporalId));

            }
        }

        public List<CompanyTransport> GetCompanyTransportsByPolicyId(int policyId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCompanyTransportsByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyTransportsByPolicyId), ex);
            }
        }

        public CompanyTransport RunRulesRisk(CompanyTransport companyTransport, int ruleId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.RunRulesRisk(companyTransport, ruleId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorRunRulesRisk), ex);
            }
        }
        public CompanyCoverage RunRulesCoverage(CompanyTransport companyTransport, CompanyCoverage companyCoverage, int ruleSetId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.RunRulesCoverage(companyTransport, companyCoverage, ruleSetId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyTransport QuotateCompanyTransport(CompanyTransport companyTransport, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.QuotateTransport(companyTransport, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorQuotateCompanyTransport), ex);
            }

        }

        public List<CompanyTransport> QuotateCompanyTransports(List<CompanyTransport> companyTransports, bool runRulesPre, bool runRulesPost)
        {
            throw new System.NotImplementedException();
        }

        public CompanyCoverage QuotateCompanyCoverage(CompanyTransport companyTransport, CompanyCoverage companyCoverage, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.Quotate(companyTransport, companyCoverage, runRulesPre, runRulesPost);
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

        public CompanyTransport ExcludeCompanyTransport(int temporalId, int riskId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.ExcludeCompanyTransport(temporalId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExcludeCompanyTransport), ex);
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

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyTransport> companyTransports)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.CreateEndorsement(companyPolicy, companyTransports);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateEndorsement), ex);
            }
        }

        public List<CompanyTransport> GetCompanyTransportsByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCompanyTransportsByPolicyIdEndorsementId(policyId, endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyTransportsByPolicyId), ex);
            }
        }

        public List<CompanyEndorsement> GetCompanyEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int policyId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCompanyEndorsementByEndorsementTypeIdPolicyId(endorsementTypeId, policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyEndorsement), ex);
            }
        }

        public List<CompanyCoverage> GetCoveragesByRiskId(int riskId, int temporalId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCoveragesByRiskId(riskId, temporalId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public bool GetLeapYear()
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetLeapYear();
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
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.DeleteCompanyRisk(temporalId, riskId);
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
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }

        public bool saveInsuredObject(int riskId, CompanyInsuredObject insuredObject, int tempId, int groupCoverageId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.SaveInsuredObject(riskId, insuredObject, tempId, groupCoverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyTransport> GetCompanyTransportByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCompanyTransportByEndorsementIdModuleType(endorsementId, moduleType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<CompanyTransport> GetCompanyTransportsByInsuredId(int insuredId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCompanyTransportsByInsuredId(insuredId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyTransport GetCompanyTransportByRiskId(int riskId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCompanyTransportByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyRiskCommercialClass> GetRiskCommercialClasses(string description)
        {
            TransportBusiness transportBusiness = new TransportBusiness();
            return transportBusiness.GetRiskCommercialClasses(description);
        }
        /// <summary>
        /// Valida si en la poliza actual se pueden realizar endosos de declaracion
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public bool CanMakeDeclarationEndorsement(int policyId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.CanMakeDeclarationEndorsement(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        /// <summary>
        /// Valida si en la poliza actual se pueden realizar endosos de ajuste 
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public bool CanMakeAdjustmentEndorsement(int policyId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.CanMakeAdjustmentEndorsement(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyHolderType> GetHolderTypes()
        {
            TransportBusiness transportBusiness = new TransportBusiness();
            return transportBusiness.GetHolderTypes();
        }

        public CompanyEndorsement GetNextDeclarationEndorsementByPolicyId(int policyId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetNextDeclarationEndorsementByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetNextDeclarationEndorsementByPolicyId), ex);
            }
        }

        public CompanyEndorsement GetNextAdjustmentEndorsementByPolicyId(int policyId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetNextAdjustmentEndorsementByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetNextAdjustmentEndorsementByPolicyId), ex);
            }
        }

        public CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType, bool clearPolicies)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.CreateCompanyPolicy(temporalId, temporalType, clearPolicies);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreatePolicy), ex);
            }
        }

        public bool CanMakeEndorsement(int policyId, out Dictionary<string, object> endorsementValidate)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.CanMakeEndorsement(policyId, out endorsementValidate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> GetCoveragesByCoveragesAdd(int productId, int coverageGroupId, int prefixId, string coveragesAdd, int insuredObjectId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCoveragesByCoveragesAdd(productId, coverageGroupId, prefixId, coveragesAdd, insuredObjectId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyEndorsementPeriod SaveCompanyEndorsementPeriod(CompanyEndorsementPeriod companyEndorsementPeriod) {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.SaveCompanyEndorsementPeriod(companyEndorsementPeriod);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            }

        public bool CanMakeEndorsementByRiskByInsuredObjectId(decimal policyId, decimal riskId, decimal insuredObjectId, EndorsementType endorsementType) {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.CanMakeEndorsementByRiskByInsuredObjectId(policyId,riskId,insuredObjectId,endorsementType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

    


        public CompanyCoverage GetCoverageByCoverageId(int coverageId, int temporalId, int groupCoverageId)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                CompanyCoverage coverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, policy.Product.Id, groupCoverageId);
                coverage.EndorsementType = policy.Endorsement.EndorsementType;
                coverage.CurrentFrom = policy.CurrentFrom;
                coverage.CurrentTo = policy.CurrentTo;
                coverage.Days = Convert.ToInt32((coverage.CurrentTo.Value - coverage.CurrentFrom).TotalDays);
                coverage.FirstRiskType = FirstRiskType.None;

                if (coverage.EndorsementType == EndorsementType.Modification)
                {
                    coverage.CoverStatus = CoverageStatusType.Included;
                }
                coverage.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus);
               
                return coverage;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}