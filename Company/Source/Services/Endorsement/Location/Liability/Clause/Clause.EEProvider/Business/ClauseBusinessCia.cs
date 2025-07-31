using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using LEM = Sistran.Company.Application.Location.LiabilityServices.Models;


namespace Sistran.Company.Application.LiabilityClauseService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.LiabilityClauseService.EEProvider.Resources;
    using Sistran.Company.Application.LiabilityClauseService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using CTSE = Sistran.Company.Application.ClauseEndorsement.EEProvider;
    using ClibialitySE = Sistran.Company.Application.Location.LiabilityServices.EEProvider;

    class ClauseBusinessCia
    {

        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiabilityEndorsementBusinessCia" /> class.
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
                    List<LEM.CompanyLiabilityRisk> companyLibialitys = null;
                    companyLibialitys = DelegateService.LibialityService.GetCompanyLiebilitiesByPolicyId(companyEndorsement.Endorsement.PolicyId);
                    if (companyLibialitys != null && companyLibialitys.Count > 0)
                    {
                        List<CompanyRisk> risks = new List<CompanyRisk>();
                        companyLibialitys.AsParallel().ForAll(item =>
                        {
                            item.Risk.Policy = policy;
                            item.Risk.Status = RiskStatusType.Modified;
                            item = DelegateService.libialityModificationService.GetDataModification(item, CoverageStatusType.Modified);
                            foreach (CompanyCoverage itemCoverage in item.Risk.Coverages)
                            {
                                itemCoverage.EndorsementLimitAmount = itemCoverage.LimitAmount;
                                itemCoverage.EndorsementSublimitAmount = itemCoverage.SubLimitAmount;
                            }
                            var libiality = DelegateService.LibialityService.CreateLiabilityTemporal(item, false);
                        });
                        risks = companyLibialitys.Select(x => x.Risk).ToList();
                        policy = provider.CalculatePolicyAmounts(policy, risks);
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        List<LEM.CompanyLiabilityRisk> libialitys = DelegateService.LibialityService.GetCompanyLiabilitiesByTemporalId(policy.Id);

                        var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
                        if (message != string.Empty)
                        {
                            throw new Exception(message);
                        }
                        ClibialitySE.LiabilityServiceEEProvider LibialityServiceEEProvider = new ClibialitySE.LiabilityServiceEEProvider();


                        var companyPolicy = LibialityServiceEEProvider.CreateEndorsement(policy, libialitys);
                        CompanyPolicy newCompanyPolicy = new CompanyPolicy();
                        newCompanyPolicy.Id = companyPolicy.Id;
                        newCompanyPolicy.DocumentNumber = companyPolicy.DocumentNumber;
                        newCompanyPolicy.Endorsement = companyPolicy.Endorsement;
                        newCompanyPolicy.InfringementPolicies = companyPolicy.InfringementPolicies;
                        return newCompanyPolicy;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorLibialityNotFound);
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
