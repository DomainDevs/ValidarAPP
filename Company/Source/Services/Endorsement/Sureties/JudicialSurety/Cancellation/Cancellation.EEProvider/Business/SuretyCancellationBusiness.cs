using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.JudicialSuretyCancellationService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Services.UtilitiesServices.Enums;
using JSEM = Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.JudicialSuretyCancellationService.EEProvider.Business
{
    public class JudicialSuretyCancellationBusinessCompany
    {

        BaseBusinessCia baseBusinessCompany;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuretyEndorsementBusinessCompany" /> class.
        /// </summary>
        public JudicialSuretyCancellationBusinessCompany()
        {
            baseBusinessCompany = new BaseBusinessCia();
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
                    companyPolicy.UserId = companyEndorsement.UserId;
                }

                if (companyEndorsement.OnlyCancelation)
                {
                    if (companyPolicy.Endorsement.ExchangeRate > 0)
                        exchangeRate = companyPolicy.Endorsement.ExchangeRate;
                    else if (companyPolicy.ExchangeRate.BuyAmount > 0)
                        exchangeRate = companyPolicy.ExchangeRate.BuyAmount;
                    else if (companyPolicy.ExchangeRate.SellAmount > 0)
                        exchangeRate = companyPolicy.ExchangeRate.SellAmount;
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
                throw ex;
            }
        }


        /// <summary>
        /// Cancellations the policy.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception">
        /// </exception>
        private CompanyPolicy CancellationPolicy(CompanyPolicy companyPolicy)
        {
            List<JSEM.CompanyJudgement> companySuretys = null;
            if (companyPolicy == null)
            {
                throw new ArgumentException("Poliza Vacia");
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

                    companySuretys = DelegateService.judicialsuretyService.GetCompanyJudicialSuretyByPolicyId(companyPolicy.Endorsement.PolicyId);
                    companySuretys.AsParallel().ForAll(
                          x =>
                          {
                              x.Risk.OriginalStatus = x.Risk.Status;
                              x.Risk.Status = RiskStatusType.Modified;
                          });
                }
                else
                {
                    companySuretys = DelegateService.judicialsuretyService.GetCompanyJudgementsByTemporalId(companyPolicy.Id);
                    if (companySuretys == null || companySuretys.Count < 1)
                    {
                        companySuretys = DelegateService.judicialsuretyService.GetCompanyJudicialSuretyByPolicyId(companyPolicy.Endorsement.PolicyId);
                        companySuretys.AsParallel().ForAll(
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
                companyRisk = CreateSuretyCancelation(companyPolicy, companySuretys, cancellationFactor);
                if (companyRisk != null && companyRisk.Any())
                {

                    riskInfringementPolicies.AddRange(companyRisk.SelectMany(x => x.InfringementPolicies).Where(m => m != null).ToList());
                    companyPolicy = baseBusinessCompany.CalculatePolicyAmounts(companyPolicy, companyRisk);

                    companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                    if (companyPolicy.InfringementPolicies != null)
                    {
                        companyPolicy.InfringementPolicies.AddRange(riskInfringementPolicies);
                    }
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

        private List<CompanyRisk> CreateSuretyCancelation(CompanyPolicy companyPolicy, List<JSEM.CompanyJudgement> companySuretys, int cancellationFactor)
        {
            if (companyPolicy == null || companySuretys == null || companySuretys.Count < 1)
            {
                throw new ArgumentException("Vehiculos vacios");
            }

            if (companySuretys != null && companySuretys.Count > 0)
            {
                var risks = DelegateService.cancellationEndorsement.QuotateCiaCancellation(companyPolicy, cancellationFactor);
                if (risks?.Count > 0)
                {

                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    companySuretys.Where(a => a != null).AsParallel().ForAll(
                        z =>
                        {
                            try
                            {
                                ///*Accesorios*/
                                //if (z.Accesories != null)
                                //{
                                //    foreach (var item in z.Accesories)
                                //    {
                                //        item.Premium = item.Premium * cancellationFactor;
                                //        item.AccumulatedPremium = item.AccumulatedPremium * cancellationFactor;
                                //        item.Amount = item.Amount * cancellationFactor;
                                //    }
                                //}

                                var companyRisk = risks.FirstOrDefault(x => x.Number == z.Risk.Number);
                                if (companyRisk != null)
                                {
                                    ///*Deducibles*/
                                    //foreach (var item in companyRisk.Coverages)
                                    //{
                                    //    item.Deductible = z.Risk.Coverages.Where(x => x.Id == item.Id).FirstOrDefault()?.Deductible;
                                    //}
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
                            catch (Exception)
                            {

                                errors.Add(Errors.ErrorCreateTemporalCancellationSurety);
                            }
                        }
                    );
                    ConcurrentBag<string> error = new ConcurrentBag<string>();
                    TP.Parallel.ForEach(companySuretys.Where(z => risks.Select(x => x.Number).Contains(z.Risk.Number)), item =>
                    {
                        try
                        {
                            item = DelegateService.judicialsuretyModificationService.GetDataModification(item, CoverageStatusType.Modified);
                            //item.Rate = item.Rate * -1;

                            var coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyPolicy.Product.Id, item.Risk.GroupCoverage.Id, companyPolicy.Prefix.Id);
                            item.Risk.Coverages.AsParallel().ForAll(coverage =>
                            {
                                //      try
                                //      {
                                //          if (coverages?.FirstOrDefault(x => x.Id == coverage.Id)?.SubLineBusiness == null)
                                //          {
                                //              new Exception("Ramo tecnico no encontrado: " + coverage.Id.ToString());
                                // 	}
                                coverage.SubLineBusiness = coverages?.FirstOrDefault(x => x.Id == coverage.Id).SubLineBusiness;
                                coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                                coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                                //     }
                                //     catch (Exception ex)
                                //     {

                                //       error.Add(ex.Message);
                                //     }
                            });
                            //     item.Risk.Coverages.AsParallel().ForAll(x => x.EndorsementType = EndorsementType.Cancellation);
                            var surety = DelegateService.judicialsuretyService.CreateJudgementTemporal(item, false);
                            if (surety != null && surety.Risk != null)
                            {
                                item.Risk.Id = surety.Risk.Id;
                                item.Risk.InfringementPolicies = surety.Risk.InfringementPolicies;
                            }
                        }
                        catch (Exception ex)
                        {

                            error.Add(ex.Message);
                        }
                    });
                    if (error != null && error.Count > 0)
                    {
                        throw new Exception(string.Join("-", error.ToList()));
                    }
                    return companySuretys.Select(x => x.Risk).ToList();
                }
                else
                {
                    throw new ArgumentException("Error Cancelando Vehiculos");
                }
            }
            else
            {
                throw new ArgumentException("Error Recuperando Vehiculos");
            }

        }


        public CompanyPolicy ExecuteThread(List<JSEM.CompanyJudgement> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            foreach (JSEM.CompanyJudgement judicialsurety in risksThread)
            {
                if (judicialsurety.Risk.Beneficiaries[0].IdentificationDocument == null)
                {
                    foreach (CompanyBeneficiary item in judicialsurety.Risk.Beneficiaries)
                    {
                        Beneficiary beneficiary = new Beneficiary();
                        beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                        item.IdentificationDocument = beneficiary.IdentificationDocument;
                        item.Name = beneficiary.Name;
                        item.CustomerType = beneficiary.CustomerType;
                    }
                }

                judicialsurety.Risk.Status = RiskStatusType.Excluded;
                //      judicialsurety.Rate = judicialsurety.Rate * -1;
                judicialsurety.Risk.Coverages = risks.First(x => x.RiskId == judicialsurety.Risk.RiskId).Coverages;

                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(policy.Product.Id, judicialsurety.Risk.GroupCoverage.Id, policy.Prefix.Id);
                foreach (CompanyCoverage coverage in judicialsurety.Risk.Coverages)
                {
                    coverage.SubLineBusiness = coverages.First(x => x.Id == coverage.Id).SubLineBusiness;
                    coverage.Number = coverages.First(x => x.Id == coverage.Id).Number;
                    coverage.CoverStatus = CoverageStatusType.Excluded;
                    coverage.EndorsementType = policy.Endorsement.EndorsementType;
                    coverage.CurrentFrom = policy.CurrentFrom;
                }


                DelegateService.judicialsuretyService.CreateJudgementTemporal(judicialsurety, false);

            }
            return policy;
        }


    }

}
