using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using JSEM = Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;


namespace Sistran.Company.Application.JudicialSuretyClauseService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.JudicialSuretyClauseService.EEProvider.Resources;
    using Sistran.Company.Application.JudicialSuretyClauseService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using CTSE = Sistran.Company.Application.ClauseEndorsement.EEProvider;
    using CVSE = Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider;
    class ClauseBusinessCompany
    {

        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="JudicialSuretyEndorsementBusinessCompany" /> class.
        /// </summary>
        public ClauseBusinessCompany()
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
                CTSE.CiaClauseEndorsementEEProvider CompanyClauseEndorsementEEProvider = new CTSE.CiaClauseEndorsementEEProvider();
                policy = CompanyClauseEndorsementEEProvider.CreateCiaClause(companyEndorsement);
                if (policy != null)
                {
                    List<JSEM.CompanyJudgement> companySuretys = null;
                    companySuretys = DelegateService.judicialsuretyService.GetCompanyJudicialSuretyByPolicyId(companyEndorsement.Endorsement.PolicyId);
                    if (companySuretys != null && companySuretys.Count > 0)
                    {
                        List<CompanyRisk> risks = new List<CompanyRisk>();
                        companySuretys.AsParallel().ForAll(item =>
                        {
                            item.Risk.Policy = policy;
                            item.Risk.Status = RiskStatusType.Modified;
                            item = DelegateService.judicialsuretyModificationService.GetDataModification(item, CoverageStatusType.Modified);
                            foreach (CompanyCoverage itemCoverage in item.Risk.Coverages)
                            {
                                itemCoverage.EndorsementLimitAmount = itemCoverage.LimitAmount;
                                itemCoverage.EndorsementSublimitAmount = itemCoverage.SubLimitAmount;
                            }
                            var surety = DelegateService.judicialsuretyService.CreateJudgementTemporal(item, false);
                        });
                        risks = companySuretys.Select(x => x.Risk).ToList();
                        policy = provider.CalculatePolicyAmounts(policy, risks);
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        List<JSEM.CompanyJudgement> suretys = DelegateService.judicialsuretyService.GetCompanyJudgementsByTemporalId(policy.Id);

                        var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
                        if (message != string.Empty)
                        {
                            throw new Exception(message);
                        }
                        CVSE.JudicialSuretyServiceEEProvider suretyServiceEEProvider = new CVSE.JudicialSuretyServiceEEProvider();


                        var companyPolicy = suretyServiceEEProvider.CreateEndorsement(policy, suretys);
                        CompanyPolicy newCompanyPolicy = new CompanyPolicy();
                        newCompanyPolicy.Id = companyPolicy.Id;
                        newCompanyPolicy.DocumentNumber = companyPolicy.DocumentNumber;
                        newCompanyPolicy.Endorsement = companyPolicy.Endorsement;
                        newCompanyPolicy.InfringementPolicies = companyPolicy.InfringementPolicies;
                        return newCompanyPolicy;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorSuretyNotFound);
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
