using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.SuretyCancellationService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.DTOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Integration.OperationQuotaServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using SEM = Sistran.Company.Application.Sureties.SuretyServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.SuretyCancellationService.EEProvider.Business
{
    public class SuretyCancellationBusinessCia
    {

        BaseBusinessCia baseBusinessCia;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuretyEndorsementBusinessCia" /> class.
        /// </summary>
        public SuretyCancellationBusinessCia()
        {
            baseBusinessCia = new BaseBusinessCia();
        }
        public CompanyPolicy CreateTemporal(CompanyEndorsement companyEndorsement)
        {
            try
            {
                var companyPolicy = new CompanyPolicy();
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
                }
                
                if (companyEndorsement.OnlyCancelation)
                {
                    if (companyPolicy.Endorsement.ExchangeRate > 0)
                        exchangeRate = companyPolicy.Endorsement.ExchangeRate;
                    else if (companyPolicy.ExchangeRate.BuyAmount > 0)
                        exchangeRate = companyPolicy.ExchangeRate.BuyAmount;
                    else if (companyPolicy.ExchangeRate.SellAmount > 0)
                        exchangeRate = companyPolicy.ExchangeRate.SellAmount;
                    if (companyPolicy.ExchangeRate.Currency.Id != (int)EnumExchangeRateCurrency.CURRENCY_PESOS )
                    {
                        if (companyEndorsement.CancellationTypeId == 1)
                        {
                            CompanyEndorsement firstEndorsements = DelegateService.underwritingService.GetCiaEndorsementsByFilterPolicy(companyPolicy.Branch.Id, companyPolicy.Prefix.Id, companyPolicy.DocumentNumber, false,true).Where(x => x.EndorsementType == EndorsementType.Emission).FirstOrDefault();
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
                    ExchangeRate = exchangeRate
                };

                companyPolicy.CurrentFrom = companyEndorsement.CurrentFrom;
                companyPolicy.CurrentTo = companyEndorsement.CurrentTo;

                if (companyEndorsement.CancellationTypeId != (int)CancellationType.Nominative && companyEndorsement.CancellationTypeId != (int)CancellationType.ShortTerm)
                    if (companyPolicy.Endorsement.CurrentFrom == companyPolicy.CurrentFrom)
                    {
                        companyPolicy.Endorsement.CancellationTypeId = 1;
                    }
                companyPolicy.Id = companyEndorsement.TemporalId;
                companyPolicy.CurrentTo = companyEndorsement.CancelationCurrentTo;
                companyPolicy.CurrentFrom = companyEndorsement.CancelationCurrentFrom;
                companyPolicy.Endorsement.TicketNumber = companyEndorsement.TicketNumber;
                companyPolicy.Endorsement.TicketDate = companyEndorsement.TicketDate;
                companyPolicy.IssueDate = companyEndorsement.IssueDate;
                companyPolicy.UserId = companyEndorsement.UserId;
                companyPolicy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(companyPolicy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                if (companyEndorsement.OnlyCancelation)
                {
                    companyPolicy.ExchangeRate.SellAmount = exchangeRate;
                    companyPolicy.ExchangeRate.BuyAmount = exchangeRate;
                }

                companyPolicy = CancellationPolicy(companyPolicy);

                return companyPolicy;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
           
            }
        }


        private CompanyPolicy CancellationPolicy(CompanyPolicy companyPolicy)
        {
            List<SEM.CompanyContract> companySuretys = null;
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
                    companyPolicy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(TemporalType.Endorsement);
                    companyPolicy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };

                    companySuretys = DelegateService.suretyService.GetCompanySuretiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                }
                else
                {
                    companySuretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyPolicy.Id);
                    if (companySuretys == null)
                    {
                        companySuretys = DelegateService.suretyService.GetCompanySuretiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                    }
                }
                var companyPolicyTemp = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                companyPolicy.Endorsement.TemporalId = companyPolicyTemp.Endorsement.TemporalId;
                companyPolicy.Id = companyPolicyTemp.Id;
                companyRisk = CreateSuretyCancelation(companyPolicy, companySuretys, cancellationFactor);
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

        private List<CompanyRisk> CreateSuretyCancelation(CompanyPolicy companyPolicy, List<SEM.CompanyContract> companySuretys, int cancellationFactor)
        {
            if (companyPolicy == null || companySuretys == null || companySuretys.Count < 1)
            {
                throw new ArgumentException(Errors.EmptyVehicles);
            }

            if (companySuretys != null && companySuretys.Count > 0)
            {
                var risks = DelegateService.cancellationEndorsement.QuotateCiaCancellation(companyPolicy, cancellationFactor);
                var companysuretyRisk = companySuretys.Select(x => x.Risk).ToList();
                if (risks != null && risks.Count > 0)
                {

                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    companySuretys.Where(a => a != null).AsParallel().ForAll(
                        z =>
                        {
                            var companyRisk = risks.FirstOrDefault(x => x.Number == z.Risk.Number);
                            if (z.Risk.Coverages != null)
                            {
                                companyRisk.Coverages.AsParallel().ForAll(
                                    y =>
                                    {
                                        var riskCovergae = z.Risk.Coverages.FirstOrDefault(m => m.Id == y.Id);
                                        y.InsuredObject = riskCovergae?.InsuredObject;
                                    });
                            }

                            if (companyRisk != null)
                            {
                                z.Risk.Policy = companyPolicy;
                                z.Risk.RiskId = companyRisk.RiskId;
                                z.Risk.Number = companyRisk.Number;
                                z.Risk.Status = companyRisk.Status;
                                companyRisk.Coverages.ForEach(x => x.CurrentTo = z.Risk.Coverages.FirstOrDefault(y => y.Id == x.Id)?.CurrentTo ==null? x.CurrentTo: z.Risk.Coverages.FirstOrDefault(y => y.Id == x.Id)?.CurrentTo);
                                z.Risk.Coverages = companyRisk.Coverages;
                                z.Risk.Premium = companyRisk.Coverages.Where(m => m != null).Sum(x => x.PremiumAmount);
                            }
                            else
                            {
                                errors.Add(Errors.RiskNotFound);
                            }
                        }
                    );

                    TP.Parallel.ForEach(companySuretys.Where(z => risks.Select(x => x.Number).Contains(z.Risk.Number)), item =>
                       {
                           item = DelegateService.suretyModificationService.GetDataModification(item, CoverageStatusType.Modified);

                           var coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, item.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);
                           item.Risk.Coverages.AsParallel().ForAll(coverage =>
                           {
                               coverage.SubLineBusiness = coverages?.First(x => x.Id == coverage.Id).SubLineBusiness;
                               coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                               coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                           });

                           var surety = DelegateService.suretyService.CreateSuretyTemporal(item, false);
                           if (surety != null && surety.Risk != null)
                           {
                               item.Risk.Id = surety.Risk.Id;
                               item.Risk.InfringementPolicies = surety.Risk.InfringementPolicies;
                           }
                       });
                    return companySuretys.Select(x => x.Risk).ToList(); ;
                }
                else
                {
                    throw new ArgumentException(Errors.CancelingVehiclesError);
                }
            }
            else
            {
                throw new ArgumentException(Errors.RecoveringVehiclesError);
            }

        }


        public CompanyPolicy ExecuteThread(List<SEM.CompanyContract> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            foreach (SEM.CompanyContract surety in risksThread)
            {
                if (surety.Risk.Beneficiaries[0].IdentificationDocument == null)
                {
                    foreach (CompanyBeneficiary item in surety.Risk.Beneficiaries)
                    {
                        Beneficiary beneficiary = new Beneficiary();
                        beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                        item.IdentificationDocument = beneficiary.IdentificationDocument;
                        item.Name = beneficiary.Name;
                        item.CustomerType = beneficiary.CustomerType;
                    }
                }

                surety.Risk.Status = RiskStatusType.Excluded;

                surety.Risk.Coverages = risks.First(x => x.RiskId == surety.Risk.RiskId).Coverages;

                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(policy.Product.Id, surety.Risk.GroupCoverage.Id, policy.Prefix.Id);
                foreach (CompanyCoverage coverage in surety.Risk.Coverages)
                {
                    coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                    coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                    coverage.CoverStatus = CoverageStatusType.Excluded;
                    coverage.EndorsementType = policy.Endorsement.EndorsementType;
                    coverage.CurrentFrom = policy.CurrentFrom;
                }


                DelegateService.suretyService.CreateSuretyTemporal(surety, false);

            }
            return policy;
        }
    }

}
