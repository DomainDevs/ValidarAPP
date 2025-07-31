using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SEM = Sistran.Company.Application.Sureties.SuretyServices.Models;


namespace Sistran.Company.Application.SuretyClauseService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.Sureties.SuretyServices.Models;
    using Sistran.Company.Application.SuretyClauseService.EEProvider.Resources;
    using Sistran.Company.Application.SuretyClauseService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using CTSE = Sistran.Company.Application.ClauseEndorsement.EEProvider;
    using CVSE = Sistran.Company.Application.Sureties.SuretyServices.EEProvider;

    class ClauseBusinessCia
    {

        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuretyEndorsementBusinessCia" /> class.
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
        public CompanyPolicyResult CreateEndorsementClause(CompanyPolicy companyEndorsement, CompanyModification companyModification)
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
                    policy.IssueDate = companyEndorsement.IssueDate;
                    List<SEM.CompanyContract> companySuretys = null;
                    companySuretys = DelegateService.suretyService.GetCompanySuretiesByPolicyId(companyEndorsement.Endorsement.PolicyId);
                    if (companySuretys != null && companySuretys.Count > 0)
                    {
                        List<CompanyRisk> risks = new List<CompanyRisk>();
                        companySuretys.AsParallel().ForAll(item =>
                        {
                            item.Risk.Policy = policy;
                            item.Risk.Status = RiskStatusType.Modified;
                            item = DelegateService.suretyModificationService.GetDataModification(item, CoverageStatusType.Modified);
                            foreach (CompanyCoverage itemCoverage in item.Risk.Coverages)
                            {
                                if (itemCoverage.PremiumAmount == 0)
                                {
                                    itemCoverage.EndorsementLimitAmount = itemCoverage.PremiumAmount;
                                    itemCoverage.EndorsementSublimitAmount = itemCoverage.PremiumAmount;
                                }
                                else
                                {
                                    itemCoverage.EndorsementLimitAmount = itemCoverage.LimitAmount;
                                    itemCoverage.EndorsementSublimitAmount = itemCoverage.SubLimitAmount;
                                }
                               
                            }
                            var surety = DelegateService.suretyService.CreateSuretyTemporal(item, false);
                        });
                        risks = companySuretys.Select(x => x.Risk).ToList();
                        policy = provider.CalculatePolicyAmounts(policy, risks);
                        policy.UserId = companyEndorsement.UserId;
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        List<CompanyContract> suretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(policy.Id);

                        var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
                        if (message != string.Empty)
                        {
                            if(message != "Las Fechas Hasta de la cobertura, no esta dentro de la vigencia de la poliza")
                                
                            throw new Exception(message);
                        }
                        CVSE.SuretyServiceEEProvider suretyServiceEEProvider = new CVSE.SuretyServiceEEProvider();

                        var companyPolicy = suretyServiceEEProvider.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, false, companyModification);
                        return companyPolicy;
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
