using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.LiabilityCancellationService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Integration.OperationQuotaServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using LEM = Sistran.Company.Application.Location.LiabilityServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;
namespace Sistran.Company.Application.LiabilityCancellationService.EEProvider.Business
{
    public class LiabilityCancellationBusinessCia
    {

        BaseBusinessCia baseBusinessCia;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibialityEndorsementBusinessCia" /> class.
        /// </summary>
        public LiabilityCancellationBusinessCia()
        {
            baseBusinessCia = new BaseBusinessCia();
        }
        public CompanyPolicy CreateTemporal(CompanyEndorsement companyEndorsement)
        {
            try
            {
                CompanyPolicy companyPolicy = new CompanyPolicy();
                decimal exchangeRate = 0;
                if (companyEndorsement == null)
                {
                    throw new ArgumentException("");
                }
                if (companyEndorsement.TemporalId != 0)
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);
                }
                else
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyEndorsement.Id);
                    companyPolicy.Id = 0;
                    companyPolicy.Endorsement.TemporalId = 0;
                }

                if (companyEndorsement.OnlyCancelation)
                {
                    if (companyPolicy.Endorsement.ExchangeRate > 0)
                        exchangeRate = companyPolicy.Endorsement.ExchangeRate;
                    else if (companyPolicy.ExchangeRate.BuyAmount > 0)
                        exchangeRate = companyPolicy.ExchangeRate.BuyAmount;
                    else if (companyPolicy.ExchangeRate.SellAmount > 0)
                        exchangeRate = companyPolicy.ExchangeRate.SellAmount;
                    if (companyPolicy.ExchangeRate.Currency.Id != (int)EnumExchangeRateCurrency.CURRENCY_PESOS)
                    {
                        if (companyEndorsement.CancellationTypeId == 1)
                        {
                            CompanyEndorsement firstEndorsements = DelegateService.underwritingService.GetCiaEndorsementsByFilterPolicy(companyPolicy.Branch.Id, companyPolicy.Prefix.Id, companyPolicy.DocumentNumber, false, true).Where(x => x.EndorsementType == EndorsementType.Emission).FirstOrDefault();
                            exchangeRate = firstEndorsements.ExchangeRate;
                        }
                        else if (companyEndorsement.CancellationTypeId == 2)
                        {
                            DateTime IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Today);
                            companyPolicy.ExchangeRate.SellAmount = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId
                                (IssueDate, companyPolicy.ExchangeRate.Currency.Id).SellAmount;

                            exchangeRate = companyPolicy.ExchangeRate.SellAmount;
                        }


                    }
                }

                companyPolicy.Endorsement = new CompanyEndorsement
                {

                    Id = companyEndorsement.Id,
                    PolicyId = companyEndorsement.PolicyId,
                    CancellationTypeId = companyEndorsement.CancellationTypeId,
                    EndorsementReasonId = companyEndorsement.EndorsementReasonId,
                    EndorsementType = EndorsementType.Cancellation,
                    Text = new CompanyText
                    {
                        TextBody = companyEndorsement.Text.TextBody,
                        Observations = companyEndorsement.Text.Observations
                    },
                    EndorsementDays = companyEndorsement.EndorsementDays,
                    CurrentTo = companyEndorsement.CancelationCurrentTo,
                    CurrentFrom = companyEndorsement.CancelationCurrentFrom,
                    EndorsementReasonDescription = companyEndorsement.EndorsementReasonDescription

                };

                if (companyEndorsement.OnlyCancelation)
                {
                    companyPolicy.ExchangeRate.SellAmount = exchangeRate;
                    companyPolicy.ExchangeRate.BuyAmount = exchangeRate;
                }

                companyPolicy.CurrentFrom = companyEndorsement.CurrentFrom;
                if (companyEndorsement.CancellationTypeId != (int)CancellationType.Nominative && companyEndorsement.CancellationTypeId != (int)CancellationType.ShortTerm)
                {
                    if (companyPolicy.Endorsement.CurrentFrom == companyPolicy.CurrentFrom)
                    {
                        companyPolicy.Endorsement.CancellationTypeId = 1;
                    }
                }

                companyPolicy.Id = companyEndorsement.TemporalId;
                companyPolicy.CurrentTo = companyEndorsement.CancelationCurrentTo;
                companyPolicy.CurrentFrom = companyEndorsement.CancelationCurrentFrom;
                companyPolicy.Endorsement.TicketNumber = companyEndorsement.TicketNumber;
                companyPolicy.Endorsement.TicketDate = companyEndorsement.TicketDate;
                companyPolicy.Endorsement.IsMassive = companyEndorsement.IsMassive;
                companyPolicy.UserId = companyEndorsement.UserId;
                companyPolicy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(companyPolicy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                companyPolicy = CancellationPolicy(companyPolicy);
                return companyPolicy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private CompanyPolicy CancellationPolicy(CompanyPolicy companyPolicy)
        {
            List<LEM.CompanyLiabilityRisk> companyLibiality = null;
            if (companyPolicy == null)
            {
                throw new ArgumentException(Errors.EmptyPolicy);
            }
            if (companyPolicy?.Endorsement != null)
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
                    companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                    companyLibiality = DelegateService.LibialityService.GetCompanyLiebilitiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                    companyLibiality.AsParallel().ForAll(
                          x =>
                          {
                              x.Risk.OriginalStatus = x.Risk.Status;
                              x.Risk.Status = RiskStatusType.Modified;
                          });
                }
                else
                {
                    companyLibiality = DelegateService.LibialityService.GetCompanyLiabilitiesByTemporalId(companyPolicy.Id);
                    if (companyLibiality == null || companyLibiality.Count < 1)
                    {
                        companyLibiality = DelegateService.LibialityService.GetCompanyLiebilitiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                        companyLibiality.AsParallel().ForAll(
                         x =>
                         {
                             x.Risk.OriginalStatus = x.Risk.Status;
                             x.Risk.Status = RiskStatusType.Modified;
                         });
                    }
                }

                var companyPolicyTemp = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                companyPolicy.Endorsement.TemporalId = companyPolicyTemp.Endorsement.TemporalId;
                companyPolicy.Id = companyPolicyTemp.Id;
                companyRisk = CreateLiabilityCancelation(companyPolicy, companyLibiality, cancellationFactor);
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

        private List<CompanyRisk> CreateLiabilityCancelation(CompanyPolicy companyPolicy, List<LEM.CompanyLiabilityRisk> companyLiability, int cancellationFactor)
        {
            if (companyPolicy == null || companyLiability == null || companyLiability.Count < 1)
            {
                throw new ArgumentException(Errors.EmptyRisk);
            }

            if (companyLiability != null && companyLiability.Count > 0)
            {
                var risks = DelegateService.cancellationEndorsement.QuotateCiaCancellation(companyPolicy, cancellationFactor);
                var companyLibialityRisk = companyLiability.Select(x => x.Risk).ToList();
                if (risks != null && risks.Count > 0)
                {
                    foreach (LEM.CompanyLiabilityRisk liability in companyLiability)
                    {
                        var primaryId = liability.Risk.Coverages.Where(x => x.IsPrimary == true).Select(x => x.Id);
                        foreach (CompanyRisk risk in risks.Where(x => x.Id == liability.Risk.Id))
                        {
                            risk.Coverages.Where(x => primaryId.Contains(x.Id)).ForEachParallel(x => x.IsPrimary = true);
                        }
                    }
                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    companyLiability.Where(a => a != null).AsParallel().ForAll(
                        z =>
                        {
                            var companyRisk = risks.FirstOrDefault(x => x.Number == z.Risk.Number);
                            if (z.Risk.Coverages != null)
                            {
                                companyRisk.Coverages.AsParallel().ForAll(
                                    y =>
                                    {
                                        y.IsPrimary = z.Risk.Coverages.FirstOrDefault(m => m.Id == y.Id).IsPrimary;
                                        y.InsuredObject = z.Risk.Coverages.FirstOrDefault(m => m.Id == y.Id).InsuredObject;
                                    });
                            }

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
                                errors.Add(Errors.ErrorRiskNotFound);
                            }
                        }
                    );

                    TP.Parallel.ForEach(companyLiability.Where(z => risks.Select(x => x.Number).Contains(z.Risk.Number)), item =>
                       {
                           item = DelegateService.libialityModificationService.GetDataModification(item, CoverageStatusType.Modified);

                           var coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, item.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);
                           item.Risk.Coverages.AsParallel().ForAll(coverage =>
                                              {
                                                  coverage.SubLineBusiness = coverages?.First(x => x.Id == coverage.Id).SubLineBusiness;
                                                  coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                                                  coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                                              });

                           var libiality = DelegateService.LibialityService.CreateLiabilityTemporal(item, false);
                           if (libiality != null && libiality.Risk != null)
                           {
                               item.Risk.Id = libiality.Risk.Id;
                               item.Risk.InfringementPolicies = libiality.Risk.InfringementPolicies;
                           }
                       });
                    return companyLiability.Select(x => x.Risk).ToList();
                }
                else
                {
                    throw new ArgumentException(Errors.ErrorCancelingRisk);
                }
            }
            else
            {
                throw new ArgumentException(Errors.ErrorRecoveringRisks);
            }

        }

        public CompanyPolicy ExecuteThread(List<LEM.CompanyLiabilityRisk> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            foreach (LEM.CompanyLiabilityRisk Liability in risksThread)
            {
                if (Liability.Risk.Beneficiaries[0].IdentificationDocument == null)
                {
                    foreach (CompanyBeneficiary item in Liability.Risk.Beneficiaries)
                    {
                        Beneficiary beneficiary = new Beneficiary();
                        beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                        item.IdentificationDocument = beneficiary.IdentificationDocument;
                        item.Name = beneficiary.Name;
                        item.CustomerType = beneficiary.CustomerType;
                    }
                }

                Liability.Risk.Status = RiskStatusType.Excluded;
                Liability.Risk.Coverages = risks.First(x => x.RiskId == Liability.Risk.RiskId).Coverages;

                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(policy.Product.Id, Liability.Risk.GroupCoverage.Id, policy.Prefix.Id);
                foreach (CompanyCoverage coverage in Liability.Risk.Coverages)
                {
                    coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                    coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                    coverage.CoverStatus = CoverageStatusType.Excluded;
                    coverage.EndorsementType = policy.Endorsement.EndorsementType;
                    coverage.CurrentFrom = policy.CurrentFrom;
                }


                DelegateService.LibialityService.CreateLiabilityTemporal(Liability, false);

            }
            return policy;
        }
    }

}
