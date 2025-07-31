using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using LEM = Sistran.Company.Application.Finances.FidelityServices.Models;


namespace Sistran.Company.Application.FidelityClauseService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.FidelityClauseService.EEProvider.Resources;
    using Sistran.Company.Application.FidelityClauseService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using CTSE = Sistran.Company.Application.ClauseEndorsement.EEProvider;
    using CfidelitySE = Sistran.Company.Application.Finances.FidelityServices.EEProvider;

    class ClauseBusinessCia
    {

        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FidelityEndorsementBusinessCia" /> class.
        /// </summary>
        public ClauseBusinessCia()
        {
            provider = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        public CompanyPolicy CreateEndorsementClause(CompanyPolicy companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            try
            {
                CompanyPolicy policy = new CompanyPolicy();
                CTSE.CiaClauseEndorsementEEProvider CiaClauseEndorsementEEProvider = new CTSE.CiaClauseEndorsementEEProvider();
                policy = CiaClauseEndorsementEEProvider.CreateCiaClause(companyEndorsement);
                if (policy != null)
                { 
                    List<LEM.CompanyFidelityRisk> companyFidelitys = null;
                    companyFidelitys = DelegateService.FidelityService.GetCompanyFidelitiesByPolicyId(companyEndorsement.Endorsement.PolicyId);
                    if (companyFidelitys != null && companyFidelitys.Count > 0)
                    {
                        List<CompanyRisk> risks = new List<CompanyRisk>();
                        companyFidelitys.AsParallel().ForAll(item =>
                        {
                            item.Risk.Policy = policy;
                            item.Risk.Status = RiskStatusType.Modified;
                            item = DelegateService.fidelityModificationService.GetDataModification(item, CoverageStatusType.Modified);
                            var fidelity = DelegateService.FidelityService.CreateFidelityTemporal(item, false);
                        });
                        risks = companyFidelitys.Select(x => x.Risk).ToList();
                        policy = provider.CalculatePolicyAmounts(policy, risks);
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        List <LEM.CompanyFidelityRisk> fidelitys = DelegateService.FidelityService.GetCompanyFidelitiesByTemporalId(policy.Id);

                        var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
                        if (message != string.Empty)
                        {
                            throw new Exception(message);
                        }
                        CfidelitySE.FidelityServiceEEProvider FidelityServiceEEProvider = new CfidelitySE.FidelityServiceEEProvider();

                      
                        var companyPolicy= FidelityServiceEEProvider.CreateEndorsement(policy, fidelitys);
                        CompanyPolicy newCompanyPolicy = new CompanyPolicy();
                        newCompanyPolicy.Id = companyPolicy.Id;
                        newCompanyPolicy.DocumentNumber = companyPolicy.DocumentNumber;
                        newCompanyPolicy.Endorsement = companyPolicy.Endorsement;
                        newCompanyPolicy.InfringementPolicies = companyPolicy.InfringementPolicies;
                        return newCompanyPolicy;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorFidelityNotFound);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
