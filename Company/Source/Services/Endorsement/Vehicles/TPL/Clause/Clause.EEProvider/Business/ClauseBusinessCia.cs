using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TPLEM = Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.ThirdPartyLiabilityClauseService.EEProvider.Resources;
using Sistran.Company.Application.ThirdPartyLiabilityClauseService.EEProvider.Services;
using Sistran.Core.Application.UnderwritingServices.Enums;
using CTSE = Sistran.Company.Application.ClauseEndorsement.EEProvider;
using CTPLSE = Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService;

namespace Sistran.Company.Application.ThirdPartyLiabilityClauseService.EEProvider.Business
{
    

    class ClauseBusinessCia
    {

        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyLiabilityEndorsementBusinessCia" /> class.
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
        public CompanyPolicyResult CreateEndorsementClause(CompanyPolicy companyEndorsement)
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
                policy.IssueDate = companyEndorsement.IssueDate;
                if (policy != null)
                {
                    List<TPLEM.CompanyTplRisk> companytpls = null;
              
                    if (companyEndorsement.Endorsement.TemporalId == 0)
                    {
                        companytpls = DelegateService.tplService.GetCompanyThirdPartyLiabilitiesByPolicyId(companyEndorsement.Endorsement.PolicyId);
                    }
                    else
                    {
                        companytpls = DelegateService.tplService.GetThirdPartyLiabilitiesByTemporalId(policy.Id);
                        companytpls.AsParallel().ForAll(
                      x =>
                      {
                          x.Risk.OriginalStatus = x.Risk.Status;
                          x.Risk.Status = RiskStatusType.NotModified;
                      });

                    }

                    if (companytpls != null && companytpls.Count > 0)
                    {
                        List<CompanyRisk> risks = new List<CompanyRisk>();
                        companytpls.AsParallel().ForAll(item =>
                        {
                            item.Risk.Policy = policy;
                            item.Risk.Status = RiskStatusType.Modified;
                            item = DelegateService.tplModificationService.GetDataModification(item, CoverageStatusType.Modified);
                            var tpl = DelegateService.tplService.CreateThirdPartyLiabilityTemporal(item, false);
                        });
                        risks = companytpls.Select(x => x.Risk).ToList();
                        policy = provider.CalculatePolicyAmounts(policy, risks);
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        policy.IssueDate = companyEndorsement.IssueDate;
                        List<TPLEM.CompanyTplRisk> tpls = DelegateService.tplService.GetThirdPartyLiabilitiesByTemporalId(policy.Id);

                        var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
                        if (message != string.Empty)
                        {
                            throw new Exception(message);
                        }
                        CTPLSE.ThirdPartyLiabilityServiceEEProvider TplServiceEEProvider = new CTPLSE.ThirdPartyLiabilityServiceEEProvider();
                        return TplServiceEEProvider.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, false);
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorThirdPartyLiabilityNotFound);
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
            //if (companyEndorsement == null)
            //{
            //    throw new ArgumentException(Errors.ErrorEndorsement);
            //}
            //try
            //{
            //    CompanyPolicy policy = new CompanyPolicy();
            //    CTSE.CiaClauseEndorsementEEProvider CiaClauseEndorsementEEProvider = new CTSE.CiaClauseEndorsementEEProvider();
            //    policy = CiaClauseEndorsementEEProvider.CreateCiaClause(companyEndorsement);
            //    if (policy != null)
            //    { 
            //        List<TPLEM.CompanyTplRisk> companytpls = null;
            //        companytpls = DelegateService.tplService.GetCompanyThirdPartyLiabilitiesByPolicyId(companyEndorsement.Endorsement.PolicyId);
            //        if (companytpls != null && companytpls.Count > 0)
            //        {
            //            List<CompanyRisk> risks = new List<CompanyRisk>();
            //            companytpls.AsParallel().ForAll(item =>
            //            {
            //                item.Risk.Policy = policy;
            //                item.Risk.Status = RiskStatusType.Modified;
            //                item = DelegateService.tplModificationService.GetDataModification(item, CoverageStatusType.Modified);
            //                var tpl = DelegateService.tplService.CreateThirdPartyLiabilityTemporal(item, false);
            //            });
            //            risks = companytpls.Select(x => x.Risk).ToList();
            //            policy = provider.CalculatePolicyAmounts(policy, risks);
            //            policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
            //            List <TPLEM.CompanyTplRisk > tpls = DelegateService.tplService.GetThirdPartyLiabilitiesByTemporalId(policy.Id);

            //            var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
            //            if (message != string.Empty)
            //            {
            //                throw new Exception(message);
            //            }

            //            CompanyPolicy companyPolicy = DelegateService.tplService.CreateEndorsement(policy, tpls);
            //            CompanyPolicy newCompanyPolicy = new CompanyPolicy();
            //            newCompanyPolicy.Id = companyPolicy.Id;
            //            newCompanyPolicy.DocumentNumber = companyPolicy.DocumentNumber;
            //            newCompanyPolicy.Endorsement = companyPolicy.Endorsement;
            //            newCompanyPolicy.InfringementPolicies = companyPolicy.InfringementPolicies;
            //            return newCompanyPolicy;
            //        }
            //        else
            //        {
            //            throw new Exception(Errors.ErrorThirdPartyLiabilityNotFound);
            //        }
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }
    }
}
