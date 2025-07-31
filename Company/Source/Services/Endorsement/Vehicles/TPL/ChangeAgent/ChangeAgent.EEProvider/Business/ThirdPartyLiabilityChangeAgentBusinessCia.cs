using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.ThirdPartyLiabilityChangeAgentService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TM=System.Threading.Tasks;
using TPLEM = Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using TP = Sistran.Core.Application.Utilities.Utility;


namespace Sistran.Company.Application.ThirdPartyLiabilityChangeAgentService.EEProvider.Business
{

    public class ThirdPartyLiabilityChangeAgentBusinessCia
    {
        BaseBusinessCia baseBusinessCia;
        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyLiabilityChangeAgentBusinessCia"/> class.
        /// </summary>
        public ThirdPartyLiabilityChangeAgentBusinessCia()
        {
            baseBusinessCia = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the temporal.
        /// </summary>
        /// <param name="companyPolicyBase">The company policy base.</param>
        /// <param name="isMassive">if set to <c>true</c> [is massive].</param>
        /// <returns></returns>
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicyBase, bool isMassive = false)
        {
            try
            {
                if (companyPolicyBase == null)
                {
                    throw new Exception(Errors.PolicyDataNotSent);
                }
                companyPolicyBase.Endorsement.EndorsementType = EndorsementType.ChangeAgentEndorsement;
                CompanyPolicy companyPolicy = new CompanyPolicy();

                if (companyPolicyBase.Endorsement.TemporalId != 0)
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicyBase.Endorsement.TemporalId, false);
                }
                else
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyPolicyBase.Endorsement.Id);
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                }
                if (companyPolicy != null)
                {
                    if ((companyPolicy.CurrentFrom != Convert.ToDateTime(companyPolicyBase.CurrentFrom))
                       || (companyPolicy.Agencies != companyPolicyBase.Agencies))
                    {
                      
                        companyPolicy.TemporalType = TemporalType.Endorsement;
                        companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                        companyPolicy.CurrentFrom = companyPolicyBase.CurrentFrom;
                        companyPolicy.CurrentTo = companyPolicyBase.CurrentTo;
                        companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
                        companyPolicy.BeginDate = Convert.ToDateTime(companyPolicyBase.CurrentFrom);
                        companyPolicy.Endorsement = companyPolicyBase.Endorsement;
                        companyPolicy.Agencies = companyPolicyBase.Agencies;
                        companyPolicy = ChangeAgentPolicy(companyPolicy);

                        return companyPolicy;
                    }

                    return null;
                }
                else
                {
                    throw new Exception(Errors.PolicyNotFound);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Changes the agent policy.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Poliza Vacia
        /// or
        /// or
        /// Vehiculos no ERncontrados
        /// </exception>
        private CompanyPolicy ChangeAgentPolicy(CompanyPolicy companyPolicy)
        {
            try
            {
                if (companyPolicy == null)
                {
                    throw new Exception(Errors.EmptyPolicy);
                }
                List<PoliciesAut> riskInfringementPolicies = new List<PoliciesAut>();
                List<TPLEM.CompanyTplRisk> companytpls = new List<TPLEM.CompanyTplRisk>();
                if (companyPolicy.Id == 0)
                {
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                    companytpls = DelegateService.tplService.GetCompanyThirdPartyLiabilitiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                }
                else
                {
                    companytpls = DelegateService.tplService.GetThirdPartyLiabilitiesByTemporalId(companyPolicy.Id);
                }
                var risks = DelegateService.changeAgentEndorsementService.QuotateChangeAgentCia(companyPolicy);
                companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                if (companytpls != null && companytpls.Count > 0 && risks != null)
                {
                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    companytpls.Where(a => a != null).AsParallel().ForAll(
                    z =>
                    {
                        var companyRisk = risks.FirstOrDefault(x => x.RiskId == z.Risk.RiskId);
                        if (companyRisk != null)
                        {
                            z.Risk.Policy = companyPolicy;
                            z.Risk.RiskId = companyRisk.RiskId;
                            z.Risk.Number = companyRisk.Number;
                            z.Risk.Status = companyRisk.Status;
                            z.Risk.Coverages = companyRisk.Coverages;
                            z.Risk.Premium = companyRisk.Coverages.Where(m => m != null).Sum(x => x.PremiumAmount);
                        }
                        else
                        {
                            errors.Add(Errors.ErrorNotFoundRisk);
                        }
                    }
                    );
                    companytpls.AsParallel().ForAll(item =>
                    {
                        item = DelegateService.tplModificationService.GetDataModification(item, CoverageStatusType.Included);
                        item.Risk.Status = RiskStatusType.Included;
                        var coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, item.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);
                        if (coverages.Count < item.Risk.Coverages.Count)
                        {
                            throw new Exception(Errors.CoverageDoesNotExistInTheCoverage);
                        }
                        TP.Parallel.ForEach(item.Risk.Coverages, coverage =>
                        {
                            if (coverages != null && coverages.Where(x => x.Id == coverage.Id).FirstOrDefault() != null)
                            {
                                coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                                coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                            }
                        });
                        item = DelegateService.tplService.CreateThirdPartyLiabilityTemporal(item, false);
                        if (item.Risk.InfringementPolicies != null)
                            riskInfringementPolicies.AddRange(item.Risk?.InfringementPolicies.Where(x => x != null));
                    });

                    if (companytpls != null && companytpls.Select(x => x.Risk).Any())
                    {

                        riskInfringementPolicies.AddRange(companytpls.Select(x => x.Risk).SelectMany(x => x.InfringementPolicies).Where(m => m != null).ToList());

                        companyPolicy = baseBusinessCia.CalculatePolicyAmounts(companyPolicy, companytpls.Select(x => x.Risk).ToList());

                        companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);

                        companyPolicy.InfringementPolicies.AddRange(riskInfringementPolicies);

                        return companyPolicy;
                    }
                    else
                    {
                        throw new Exception(Errors.UnquotedRisks);
                    }

                }
                else
                {
                    throw new Exception(Errors.VehicleNotFound);
                }

            }
            catch (Exception ex)
            {

                throw;
            }

        }


        /// <summary>
        /// Creates the endorsement change agent.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        public List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyEndorsement)
        {
            try
            {
                List<CompanyPolicy> companyPolicies = new List<CompanyPolicy>();
                var TempId = companyEndorsement.TemporalId;
                var cancelation = CancellationPolicy(companyEndorsement);
                companyEndorsement.TemporalId = TempId;
                try
                {
                    companyEndorsement.Id = cancelation.Id;
                    var createEndorsementChangeAgent = CreateTplEndorsementChangeAgent(companyEndorsement);
                    companyPolicies.Add(createEndorsementChangeAgent);
                    companyPolicies.Add(cancelation);
                    return companyPolicies;
                }
                catch (Exception)
                {
                    DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(cancelation.Endorsement.PolicyId, cancelation.Endorsement.Id, EndorsementType.Cancellation);
                    throw new Exception(Errors.ErrorCreatingEndorsementAgentChange);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the tpl endorsement change agent.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        private CompanyPolicy CreateTplEndorsementChangeAgent(CompanyEndorsement companyEndorsement)
        {
            var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);
            List<TPLEM.CompanyTplRisk> tpls = DelegateService.tplService.GetThirdPartyLiabilitiesByTemporalId(companyPolicy.Id);
            return DelegateService.tplService.CreateEndorsement(companyPolicy, tpls);

        }

        /// <summary>
        /// Cancellations the policy.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        private CompanyPolicy CancellationPolicy(CompanyEndorsement companyEndorsement)
        {
            companyEndorsement.TemporalId = 0;
            companyEndorsement.CancellationTypeId = (int)CancellationType.FromDate;
            companyEndorsement.EndorsementReasonId = 1;
            var date = companyEndorsement.CurrentTo - companyEndorsement.CurrentFrom;
            companyEndorsement.EndorsementDays = date.Days;
            var companyPolicy = DelegateService.endorsementtplCancellationService.CreateTemporalEndorsementCancellation(companyEndorsement);
            var tpls = DelegateService.tplService.GetThirdPartyLiabilitiesByTemporalId(companyPolicy.Id);
            return DelegateService.tplService.CreateEndorsement(companyPolicy, tpls);
        }
    }
}
