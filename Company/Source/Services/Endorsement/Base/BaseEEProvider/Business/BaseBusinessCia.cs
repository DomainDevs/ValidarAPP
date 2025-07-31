using Sistran.Company.Application.BaseEndorsementService.EEProvider.Assemblers;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.BaseEndorsementService.EEProvider.Business
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseBusinessCia
    {
        /// <summary>
        /// Calculates the policy amounts.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="risks">The risks.</param>
        /// <returns></returns>
        public CompanyPolicy CalculatePolicyAmounts(CompanyPolicy policy, List<CompanyRisk> risks)
        {
            policy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponentsByCompanyPolicy(policy, risks);
            policy.Summary = DelegateService.underwritingService.CalculateSummaryByCompanyPolicy(policy, risks);

            if (policy.Summary != null)
            {
                List<CompanyRiskInsured> companyRisksInsured = new List<CompanyRiskInsured>();
                foreach (var companyRisk in risks)
                {
                    CompanyRiskInsured companyRiskInsured = new CompanyRiskInsured();
                    companyRiskInsured.Beneficiaries = new List<CompanyBeneficiary>();
                    foreach (var beneficiary in companyRisk.Beneficiaries)
                    {
                        companyRiskInsured.Beneficiaries.Add(beneficiary);
                    }
                    companyRiskInsured.Insured = companyRisk.MainInsured;
                    companyRisksInsured.Add(companyRiskInsured);
                }
                policy.Summary.RisksInsured = companyRisksInsured;

                policy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(new QuotaFilterDTO { PlanId = policy.PaymentPlan.Id, CurrentFrom = policy.CurrentFrom, IssueDate = policy.IssueDate, ComponentValueDTO = ModelAssembler.CreateCompanyComponentValueDTO(policy.Summary) });
                policy.Agencies = DelegateService.underwritingService.CalculateCommissionsByCompanyPolicy(policy, risks);
            }
            else
            {
                policy.PaymentPlan.Quotas = new List<Quota>();
                policy.Agencies.Where(x => x.Commissions != null).SelectMany(m => m.Commissions).AsParallel().ForAll(z =>
                  {
                      z.CalculateBase = 0;
                      z.Amount = 0;
                  });
            }

            return policy;
        }


        public string ValidateEndorsement(int temporalId)
        {
            string message = string.Empty;
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

                if (policy != null)
                {
                    if (policy.Endorsement.EndorsementType != EndorsementType.Cancellation && policy.Endorsement.EndorsementType != EndorsementType.Nominative_cancellation)
                    {
                        List<CompanyRisk> risks = DelegateService.underwritingService.GetCompanyRisksByTemporalId(temporalId, false);

                        if (risks != null && risks.Any())
                        {
                            bool validEndorsement = true;
                            Parallel.ForEach(risks, (risk, state) =>
                            {
                                if (risk != null)
                                {
                                    if (policy.Product != null && policy.Product.IsCollective)
                                    {
                                        if (risk.Status != null)
                                        {
                                            validEndorsement = false;
                                            message = string.Empty;
                                            state.Stop();
                                        }
                                    }

                                    if (risk.Status != null && risk.Status.Value != RiskStatusType.Excluded)
                                    {
                                        validEndorsement = false;
                                        message = string.Empty;
                                    }
                                    switch (policy.Endorsement.EndorsementType)
                                    {
                                        case EndorsementType.Modification:
                                            switch (policy.Product.CoveredRisk.CoveredRiskType)
                                            {
                                                case CoveredRiskType.Surety:
                                                    message = ValidateDate(risk, policy, ValidateTyeEndorsement.DateSurety);
                                                    if (message != string.Empty)
                                                    {
                                                        state.Stop();
                                                    }
                                                    break;

                                                default:
                                                    message = ValidateDate(risk, policy, ValidateTyeEndorsement.DateDefault);
                                                    if (message != string.Empty)
                                                    {
                                                        state.Stop();
                                                    }
                                                    break;

                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            });                           
                            if (validEndorsement)
                            {
                                message = Errors.NoCanEndorsementsWithoutRisks;
                            }

                            return message;
                        }
                        else
                        {
                            return Errors.NoCanEndorsementsWithoutRisks;
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return Errors.NoCanEndorsementsWithoutRisks;
                }
            }
            catch (Exception)
            {
                return Errors.ErrorValideEndorsement;
            }
        }

        private string ValidateDate(CompanyRisk risk, CompanyPolicy policy, ValidateTyeEndorsement validateTyeEndorsement)
        {
            var message = string.Empty;
            DateTime? CurrentFrom = null;
            DateTime? CurrentTo = null;
            var query = new List<CompanyCoverage>();
            switch (validateTyeEndorsement)
            {

                case ValidateTyeEndorsement.DateSurety:
                    CurrentTo = risk.Coverages.Where(x => x.CoverStatus == CoverageStatusType.Modified).Max(i => i.CurrentTo);
                    query = risk.Coverages.Where(x => x.CoverStatus == CoverageStatusType.Modified).Select(x => x).ToList();
                    CurrentFrom = query.Any() ? query.Min(o => o.CurrentFrom) : (DateTime?)null;
                    if (CurrentTo != null)
                    {
                        if (policy.CurrentTo < CurrentTo)
                        {
                            message = Errors.ErrorDatesCoveragesPolicy;
                        }
                    }
                    break;
                case ValidateTyeEndorsement.DateDefault:
                    CurrentTo = risk.Coverages.Where(x => x.CoverStatus == CoverageStatusType.Modified).Max(i => i.CurrentTo);
                    query = risk.Coverages.Where(x => x.CoverStatus == CoverageStatusType.Modified).Select(x => x).ToList();
                    CurrentFrom = query.Any() ? query.Min(o => o.CurrentFrom) : (DateTime?)null;
                    if (CurrentTo != null)
                    {
                        if (CurrentTo > policy.CurrentTo)
                        {
                            message = Errors.ErrorDatesCoverageGreaterEndorsement;
                        }
                    }
                    if (CurrentFrom != null)
                    {
                        if (CurrentFrom > policy.CurrentFrom)
                        {
                            message = Errors.ErrorDatesFromCoverageGreaterEndorsement;
                        }
                    }
                    break;
                default:
                    break;

            }
            return message;
        }
        enum ValidateTyeEndorsement
        {
            DateSurety = 1,
            DateDefault = 2
        }
    }
}
