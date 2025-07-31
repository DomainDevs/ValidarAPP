using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.ThirdPartyLiabilityCancellationService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPLEM = Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
namespace Sistran.Company.Application.ThirdPartyLiabilityCancellationService.EEProvider.Business
{
    public class ThirdPartyLiabilityCancellationBusinessCia
    {

        BaseBusinessCia baseBusinessCia;

        /// <summary>
        /// Initializes a new instance of the <see cref="tplEndorsementBusinessCia" /> class.
        /// </summary>
        public ThirdPartyLiabilityCancellationBusinessCia()
        {
            baseBusinessCia = new BaseBusinessCia();
        }
        public CompanyPolicy CreateTemporal(CompanyEndorsement companyEndorsement)
        {
            try
            {
                var companyPolicy = new CompanyPolicy();
                if (companyEndorsement == null)
                {
                    throw new ArgumentException(Errors.CompanyEndorsementNull);
                }

                if (companyEndorsement.TemporalId != 0)
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);
                    companyPolicy.IsPersisted = true;

                }
                else
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyEndorsement.Id);
                    companyPolicy.IsPersisted = true;

                }
                companyPolicy.Endorsement = new CompanyEndorsement
                {

                    Id = companyEndorsement.Id,
                    PolicyId = companyEndorsement.PolicyId,
                    CancellationTypeId = companyEndorsement.CancellationTypeId,
                    EndorsementReasonId = companyEndorsement.EndorsementReasonId,
                    EndorsementReasonDescription = companyEndorsement.EndorsementReasonDescription,
                    EndorsementType = EndorsementType.Cancellation,
                    Text = new CompanyText
                    {
                        TextBody = companyEndorsement.Text.TextBody,
                        Observations = companyEndorsement.Text.Observations
                    },
                    EndorsementDays = companyEndorsement.EndorsementDays,
                    CurrentTo = companyEndorsement.CurrentTo,
                    CurrentFrom = companyEndorsement.CurrentFrom
                };
                companyPolicy.CurrentTo = companyEndorsement.CurrentTo;
                companyPolicy.CurrentFrom = companyEndorsement.CurrentFrom;
                companyPolicy.UserId = companyEndorsement.UserId;
                companyPolicy = CancellationPolicy(companyPolicy);
                return companyPolicy;


            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private CompanyPolicy CancellationPolicy(CompanyPolicy companyPolicy)
        {
            List<TPLEM.CompanyTplRisk> companyTpl = null;
            if (companyPolicy == null)
            {
                throw new ArgumentException(Errors.EmptyPolicy);
            }
            if (companyPolicy.Endorsement != null)
            {
                int cancellationFactor = -1;
                List<CompanyRisk> companyRisk = new List<CompanyRisk>();
                var riskInfringementPolicies = new List<PoliciesAut>();
                if ((CancellationType)companyPolicy.Endorsement.CancellationTypeId == CancellationType.Nominative)
                {
                    cancellationFactor = 0;
                }
                if (companyPolicy.Id == 0)
                {
                    companyPolicy.TemporalType = TemporalType.Endorsement;
                    companyPolicy.UserId = companyPolicy.UserId;
                    companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                    companyTpl = DelegateService.tplService.GetCompanyThirdPartyLiabilitiesByPolicyId(companyPolicy.Endorsement.PolicyId);

                }
                else
                {
                    companyTpl = DelegateService.tplService.GetThirdPartyLiabilitiesByTemporalId(companyPolicy.Id);
                    if (companyTpl == null)
                    {
                        companyTpl = DelegateService.tplService.GetCompanyThirdPartyLiabilitiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                    }
                }
                var companyPolicyTemp = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                companyPolicy.Endorsement.TemporalId = companyPolicyTemp.Endorsement.TemporalId;
                companyPolicy.Id = companyPolicyTemp.Id;
                companyRisk = CreateThirdPartyLiabilityCancelation(companyPolicy, companyTpl, cancellationFactor);
                if (companyRisk != null && companyRisk.Any())
                {

                    riskInfringementPolicies.AddRange(companyRisk.SelectMany(x => x.InfringementPolicies).Where(m => m != null).ToList());
                    companyPolicy = baseBusinessCia.CalculatePolicyAmounts(companyPolicy, companyRisk);

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
                throw new Exception(Errors.EmptyEndorsement);
            }

        }

        private List<CompanyRisk> CreateThirdPartyLiabilityCancelation(CompanyPolicy companyPolicy, List<TPLEM.CompanyTplRisk> companyTpl, int cancellationFactor)
        {
            try
            {
                if (companyPolicy == null || companyTpl == null || companyTpl.Count < 1)
                {
                    throw new ArgumentException(Errors.EmptyVehicles);
                }

                if (companyTpl != null && companyTpl.Count > 0)
                {
                    var risks = DelegateService.cancellationEndorsement.QuotateCiaCancellation(companyPolicy, cancellationFactor);
                    if (risks != null && risks.Count > 0)
                    {
                        ConcurrentBag<string> errors = new ConcurrentBag<string>();

                        companyTpl.Where(a => a != null).AsParallel().WithDegreeOfParallelism(ParallelHelper.DebugParallelFor().MaxDegreeOfParallelism).ForAll(
                            z =>
                            {
                                var tplRisk = z;
                                var companyRisk = risks.FirstOrDefault(x => x.Number == z.Risk.Number);
                                if (companyRisk != null)
                                {
                                    tplRisk.Risk.IsPersisted = true;
                                    tplRisk.Risk.Policy = companyPolicy;
                                    tplRisk.Risk.RiskId = companyRisk.RiskId;
                                    tplRisk.Risk.Number = companyRisk.Number;
                                    tplRisk.Risk.Status = companyRisk.Status;
                                    tplRisk.Risk.Coverages = companyRisk.Coverages;
                                    tplRisk.Risk.Premium = companyRisk.Coverages.Where(m => m != null).Sum(x => x.PremiumAmount);
                                    tplRisk = DelegateService.tplModificationService.GetDataModification(z, CoverageStatusType.Modified);
                                    tplRisk.Rate = z.Rate * -1;
                                    var coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, tplRisk.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);
                                    if (coverages != null)
                                    {
                                        tplRisk.Risk.Coverages.AsParallel().ForAll(coverage =>
                                        {
                                            coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                                            coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                                            coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                                        });
                                    }
                                    var tpl = DelegateService.tplService.CreateThirdPartyLiabilityTemporal(tplRisk, false);
                                    if (tpl != null && tpl.Risk != null)
                                    {
                                        tplRisk.Risk.Id = tpl.Risk.Id;
                                        tplRisk.Risk.InfringementPolicies = tpl.Risk.InfringementPolicies;
                                        z = tplRisk;
                                    }
                                    else
                                    {
                                        errors.Add(Errors.ErrorCreatingTemporaryRisk);
                                    }

                                }
                                else
                                {
                                    errors.Add(Errors.ErrorRiskNotFound);
                                }
                            });
                        if (errors != null && errors.Any())
                        {
                            throw new Exception(string.Join(" : ", errors));
                        }

                        return companyTpl.Select(x => x.Risk).ToList();
                    }
                    else
                    {
                        throw new ArgumentException(Errors.TPLCancellationFeeError);
                    }
                }
                else
                {
                    throw new ArgumentException(Errors.ErrorRecoveringVehicles);
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public CompanyPolicy ExecuteThread(List<TPLEM.CompanyTplRisk> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            foreach (TPLEM.CompanyTplRisk ThirdPartyLiability in risksThread)
            {
                if (ThirdPartyLiability.Risk.Beneficiaries[0].IdentificationDocument == null)
                {
                    foreach (CompanyBeneficiary item in ThirdPartyLiability.Risk.Beneficiaries)
                    {
                        Beneficiary beneficiary = new Beneficiary();
                        beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                        item.IdentificationDocument = beneficiary.IdentificationDocument;
                        item.Name = beneficiary.Name;
                        item.CustomerType = beneficiary.CustomerType;
                    }
                }

                ThirdPartyLiability.Risk.Status = RiskStatusType.Excluded;
                ThirdPartyLiability.Rate = ThirdPartyLiability.Rate * -1;
                ThirdPartyLiability.Risk.Coverages = risks.First(x => x.RiskId == ThirdPartyLiability.Risk.RiskId).Coverages;

                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(policy.Product.Id, ThirdPartyLiability.Risk.GroupCoverage.Id, policy.Prefix.Id);
                foreach (CompanyCoverage coverage in ThirdPartyLiability.Risk.Coverages)
                {
                    coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                    coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                    coverage.CoverStatus = CoverageStatusType.Excluded;
                    coverage.EndorsementType = policy.Endorsement.EndorsementType;
                    coverage.CurrentFrom = policy.CurrentFrom;
                }


                DelegateService.tplService.CreateThirdPartyLiabilityTemporal(ThirdPartyLiability, false);

            }
            return policy;
        }
    }

}
