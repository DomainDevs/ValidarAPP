using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.EEProvider;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Rules = Sistran.Core.Framework.Rules;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Business
{
    public class PolicyBusiness
    {
        UnderwritingServiceEEProviderCore coreProvider;

        public PolicyBusiness()
        {
            coreProvider = new UnderwritingServiceEEProviderCore();
        }

        public CompanyPolicy RunRulesCompanyPolicy(CompanyPolicy companyPolicy, int ruleId)
        {
            Rules.Facade facade = new Rules.Facade();
            if (companyPolicy != null)
            {
                if (!companyPolicy.IsPersisted)
                {
                    PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(companyPolicy.Id);
                    if (pendingOperation != null)
                    {
                        companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    }
                    else
                    {
                        throw new ValidationException(Errors.ErrorTempNoExist);
                    }
                }
            }
            EntityAssembler.CreateFacadeGeneral(companyPolicy, facade);
            RulesEngineDelegate.ExecuteRules(ruleId, facade);
            return ModelAssembler.CreateCompanyPolicy(companyPolicy, facade);
        }

        public CompanyPolicy CreatePolicyTemporal(CompanyPolicy policy, bool isMasive)
        {
            PolicyDAO policyDAO = new PolicyDAO();
            policy.InfringementPolicies = policyDAO.ValidateAuthorizationPolicies(policy);
            return policyDAO.CreatePolicyTemporal(policy, isMasive);
        }

        #region CompanyPolicy Extendidos

        /// <summary>
        /// Calcular Cuotas
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <returns>Cuotas</returns>
        //public List<Quota> CalculateQuotasByCompanyPolicy(CompanyPolicy policy)
        //{
        //    try
        //    {
        //        Policy corePolicy = ModelAssembler.CreatePolicy(policy);
        //        return coreProvider.CalculateQuotas(corePolicy);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Calcular Componentes De La Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="risks">Riesgos</param>
        /// <returns>Componentes</returns>
        public List<CompanyPayerComponent> CalculatePayerComponentsByCompanyPolicy(CompanyPolicy companyPolicy, List<CompanyRisk> companyRisks)
        {
            Policy policy = ModelAssembler.CreatePolicy(companyPolicy);
            List<Risk> risks = ModelAssembler.CreateRisks(companyRisks);
            List<PayerComponent> payerComponent = coreProvider.CalculatePayerComponents(policy, risks);
            IMapper imapper = AutoMapperAssembler.CreateMapCompanyPayerComponent();
            return imapper.Map<List<PayerComponent>, List<CompanyPayerComponent>>(payerComponent);
        }

        /// <summary>
        /// Agregar Summary a laPóliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="risks">Riesgos</param>
        /// <returns>Summary</returns>        
        public CompanySummary CalculateSummaryByCompanyPolicy(CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                Policy corePolicy = ModelAssembler.CreatePolicy(policy);
                List<Risk> coreRisks = risks.Select(ModelAssembler.CreateRisk).ToList();
                IMapper immaper = ModelAssembler.CreateMapCompanySummary();
                Summary summary = new Summary();
                if (policy.Endorsement.EndorsementType == EndorsementType.DeclarationEndorsement)
                {
                    summary = coreProvider.CalculateSummaryPropertyDeclaration(corePolicy, coreRisks, (int)policy.Endorsement.RiskId, (int)policy.Endorsement.InsuredObjectId);
                }
                else
                {
                    summary = coreProvider.CalculateSummary(corePolicy, coreRisks);
                }
                return immaper.Map<Summary, CompanySummary>(summary);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Calcular Comisiones
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="risks">Riesgos</param>
        /// <returns>Agencias</returns>
        public List<IssuanceAgency> CalculateCommissionsByCompanyPolicy(CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                Policy corePolicy = ModelAssembler.CreatePolicy(policy);
                List<Risk> coreRisks = risks.Select(ModelAssembler.CreateRisk).ToList();
                return coreProvider.CalculateCommissions(corePolicy, coreRisks);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Listado de Temporales Por Filtro
        /// </summary>
        /// <param name="policy">Filtro</param>
        /// <returns>Temporales</returns>
        public List<CompanyPolicy> GetCompanyTemporalPoliciesByCompanyPolicy(CompanyPolicy policy)
        {
            try
            {
                Policy corePolicy = ModelAssembler.CreatePolicy(policy);
                List<Policy> corePolicies = coreProvider.GetTemporalPoliciesByPolicy(corePolicy);
                return corePolicies.Select(ModelAssembler.CreateCompanyPolicy).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Listado de Polizas Por Filtro
        /// </summary>
        /// <param name="policy">Filtro</param>
        /// <returns>Temporales</returns>
        public List<CompanyPolicy> GetCompanyPoliciesByCompanyPolicy(CompanyPolicy policy)
        {
            try
            {
                Policy corePolicy = ModelAssembler.CreatePolicy(policy);
                List<Policy> corePolicies = coreProvider.GetPoliciesByPolicy(corePolicy);
                return corePolicies.Select(ModelAssembler.CreateCompanyPolicy).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixId"></param>
        /// <param name="branchId"></param>
        /// <param name="policyNumber"></param>
        /// <returns></returns>
        public CompanyPolicy GetCurrentCompanyPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        {
            try
            {
                Policy corePolicy = coreProvider.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber);
                return ModelAssembler.CreateCompanyPolicy(corePolicy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #region endosos
        /// <summary>
        /// Gets the cia endorsements by filter policy.
        /// </summary>
        /// <param name="branchId">The branch identifier.</param>
        /// <param name="prefixId">The prefix identifier.</param>
        /// <param name="policyNumber">The policy number.</param>
        /// <param name="isCurrent">if set to <c>true</c> [is current].</param>
        /// <returns></returns>
        public List<CompanyEndorsement> GetCiaEndorsementsByFilterPolicy(int branchId, int prefixId, decimal policyNumber, bool isCurrent = false, bool? isExchange = false)
        {
            PolicyDAO policyDAO = new PolicyDAO();
            return policyDAO.GetCiaEndorsementsByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber, 0, isCurrent, isExchange);

        }
        public CompanyPolicy GetCiaCurrentStatusPolicyByEndorsementIdIsCurrent(int endorsementId, bool isCurrent)
        {
            Policy policy = DelegateService.underwritingService.GetCurrentStatusPolicyByEndorsementIdIsCurrent(endorsementId, isCurrent);
            policy.Summary.Risks = null;
            IMapper imapper = ModelAssembler.CreateMapCompanyPolicy();
            return imapper.Map<Policy, CompanyPolicy>(policy);
        }

        public CompanyPolicy GetEndorsementInformation(int endorsementId, bool isCurrent)
        {
            PolicyDAO policyDAO = new PolicyDAO();
            CompanyPolicy companyPolicy = new CompanyPolicy();
            IMapper imapper = ModelAssembler.CreateMapCompanyPolicy();
            Policy policy = DelegateService.underwritingService.GetStatusPolicyByEndorsementIdIsCurrent(endorsementId, isCurrent);
            companyPolicy = policyDAO.MoodificationEndorsementPremium(imapper.Map<Policy, CompanyPolicy>(policy));
            companyPolicy.Summary.Risks = null;
            companyPolicy.UserId = BusinessContext.Current.UserId;
            return companyPolicy;
        }
        #endregion

        #endregion

        public List<CompanyEndorsement> GetCoPolicyEndorsementsWithPremiumByPolicyId(int policyId)
        {
            return ModelAssembler.CreateMapCompanyEndorsement(
                DelegateService.underwritingServiceCore.GetPolicyEndorsementsWithPremiumByPolicyId(policyId));
        }
    }
}