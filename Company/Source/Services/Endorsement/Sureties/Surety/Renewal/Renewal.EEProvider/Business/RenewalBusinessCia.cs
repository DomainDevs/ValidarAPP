using AutoMapper;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.SuretyRenewalService.EEProvider.Resources;
using Sistran.Company.Application.SuretyRenewalService.EEProvider.Services;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SEM = Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.SuretyRenewalService.EEProvider.Assemblers;

namespace Sistran.Company.Application.SuretyRenewalService.EEProvider.Business
{
    class RenewalBusinessCia
    {
        public static readonly string MindateDate = "01/01/1900";
        public static DateTime GetMinDate() => DateTime.Parse(MindateDate);
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenewalBusinessCia" /> class.
        /// </summary>
        public RenewalBusinessCia()
        {
            provider = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the temporal.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// </exception>
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy)
        {
            try
            {
                if (companyPolicy == null || companyPolicy.Endorsement == null)
                {
                    throw new Exception(Errors.ErrorNotDataPolicy);
                }
                CompanyPolicy policy = null;
                int id = 0;
                if (companyPolicy.Endorsement.TemporalId > 0)
                {
                    policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Endorsement.TemporalId, false);
                    id = policy.Id;
                }
                else
                {
                    policy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyPolicy.Endorsement.Id);

                    policy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                    policy.UserId = BusinessContext.Current?.UserId ?? companyPolicy.UserId; 

                    policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
                    if (policy.Endorsement == null)
                    {
                        policy.Endorsement = new CompanyEndorsement();
                    }

                    policy.Endorsement.IsUnderIdenticalConditions = companyPolicy.Endorsement.IsUnderIdenticalConditions;
                    policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                    var immaper = AutoMapperAssembler.CreateMapCompanyClause();
                    policy.Clauses = immaper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                }
                int temporalId = 0;
                temporalId = policy.Endorsement.TemporalId;
                policy.Endorsement = companyPolicy.Endorsement;
                policy.Endorsement.TemporalId = temporalId;
                policy.Endorsement.EndorsementType = EndorsementType.Renewal;
                policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                policy.TemporalType = TemporalType.Endorsement;
                //Define la fecha hasta de la renovación a partir de la vigencia del endoso anterior
                policy.CurrentFrom = companyPolicy.Endorsement.CurrentFrom == GetMinDate() ? policy.CurrentFrom : companyPolicy.Endorsement.CurrentFrom;
                policy.CurrentTo = companyPolicy.Endorsement.CurrentTo == GetMinDate() ? policy.CurrentFrom.AddYears(1) : companyPolicy.Endorsement.CurrentTo;
                policy.IssueDate = DelegateService.commonService.GetDate();
                policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);
                policy.TimeHour = "00";
                policy.UserId = companyPolicy.UserId;
                policy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();


                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                if (policy != null)
                {
                    if (!companyPolicy.Endorsement.IsUnderIdenticalConditions && policy.Product.IsCollective)
                    {
                        return policy;
                    }
                    List<SEM.CompanyContract> companySuretys = new List<SEM.CompanyContract>();
                    if (id > 0)
                    {
                        companySuretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(id);
                        if (companySuretys == null && companySuretys.Count < 1)
                        {
                            companySuretys = DelegateService.suretyService.GetCompanySuretiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                            companySuretys.AsParallel().ForAll(
                          x =>
                          {
                              x.Risk.Id = 0;
                              x.Risk.OriginalStatus = x.Risk.Status;
                              x.Risk.Status = RiskStatusType.Included;

                          });
                        }
                    }
                    else
                    {
                        companySuretys = DelegateService.suretyService.GetCompanySuretiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                        companySuretys.AsParallel().ForAll(
                      x =>
                      {
                          x.Risk.Id = 0;
                          x.Risk.OriginalStatus = x.Risk.Status;
                          x.Risk.Status = RiskStatusType.Included;

                      });
                    }
                    if (companySuretys != null)
                    {
                        ConcurrentBag<CompanyRisk> risks = new ConcurrentBag<CompanyRisk>();
                        Object objectLock = new Object();
                        if (policy.Product.IsCollective && companyPolicy.Endorsement.IsUnderIdenticalConditions)
                        {

                            Parallel.ForEach(companySuretys, ParallelHelper.DebugParallelFor(), item =>
                            {
                                item.Risk.Policy = policy;
                                CompanyRisk risk = CreateSurety(item, companyPolicy);
                                risks.Add(risk);
                            });
                            policy = provider.CalculatePolicyAmounts(policy, risks.ToList());
                        }
                        else
                        {
                            companySuretys.AsParallel().ForAll(x => x.Risk.Policy = policy);
                            ConcurrentBag<string> errorsRisk = new ConcurrentBag<string>();
                            Parallel.ForEach(companySuretys, ParallelHelper.DebugParallelFor(), item =>
                            {
                                try
                                {
                                    if (item != null)
                                    {
                                        var risk = new SEM.CompanyContract();
                                        lock (objectLock)
                                        {
                                            risk = GetDataRenewal(item, companyPolicy);
                                        }
                                        if (risk != null && risk.Risk != null)
                                        {

                                            risk = DelegateService.suretyService.CreateSuretyTemporal(risk, false);

                                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies.Where(x => x != null));
                                            risks.Add(risk.Risk);
                                        }
                                    }
                                    else
                                    {
                                        errorsRisk.Add(Errors.RiskNotFound);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errorsRisk.Add(ex.GetBaseException().Message);
                                }

                            });
                            if (errorsRisk != null && errorsRisk.Count > 0)
                            {
                                throw new Exception(string.Join(" ", errorsRisk));
                            }
                            policy = provider.CalculatePolicyAmounts(policy, risks.ToList());
                        }

                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        if (policy != null && policy.InfringementPolicies != null && policy.InfringementPolicies.Count > 0)
                        {
                            var infPolicies = risks.SelectMany(x => x.InfringementPolicies).ToList();
                            policy.InfringementPolicies.AddRange(infPolicies);
                        }

                        return policy;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorSuretyNotFound);
                    }

                }
                else
                {
                    throw new Exception(Errors.ErrorCreateTemporalPolicy);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Gets the data renewal.
        /// </summary>
        /// <param name="risk">The risk.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// </exception>
        private SEM.CompanyContract GetDataRenewal(SEM.CompanyContract risk, CompanyPolicy policy)
        {
            risk.Risk.Status = RiskStatusType.Original;
          

            if (risk.Risk?.Beneficiaries?[0].IdentificationDocument == null)
            {
                List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();
                ConcurrentBag<string> error = new ConcurrentBag<string>();
                if (risk.Risk.Beneficiaries != null)
                {
                    risk.Risk.Beneficiaries.AsParallel().ForAll(
                        item =>
                        {
                            var beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                            if (beneficiary != null)
                            {
                                item.IdentificationDocument = beneficiary.IdentificationDocument;
                                item.Name = beneficiary.Name;
                            }
                            else
                            {
                                error.Add(Errors.ErrorBeneficiaryNotFound);
                            }
                        }
                        );
                    if (error.Any())
                    {
                        throw new Exception(string.Join(",", error));
                    }
                }
                else
                {
                    throw new Exception(Errors.ErrorBeneficiaryEmpty);
                }
            }
            List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(risk.Risk.Policy.Product.Id, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Prefix.Id);
            if (coverages != null && coverages.Count > 0)
            {
                coverages = coverages.Where(c => (risk.Risk.Coverages.Any(x => x.Id == c.Id))).ToList();
                var ciaCoverages = risk.Risk.Coverages.Where(x => coverages.Select(z => z.Id).Contains(x.Id)).ToList();
                ciaCoverages.AsParallel().ForAll(item =>
                {
                    item.CoverStatus = CoverageStatusType.Original;
                    item.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original));
                    item.CurrentFrom = policy.Endorsement.CurrentFrom;
                    item.CurrentTo = policy.Endorsement.CurrentTo;
                    item.Description = coverages.FirstOrDefault(u => u.Id == item.Id).Description;
                    item.EndorsementType = risk.Risk.Policy.Endorsement.EndorsementType;
                    item.AccumulatedPremiumAmount = 0;
                    item.FlatRatePorcentage = coverages.FirstOrDefault(u => u.Id == item.Id).FlatRatePorcentage;
                    item.SubLineBusiness = coverages.First(x => x.Id == item.Id).SubLineBusiness;
                    item.RuleSetId = coverages.First(x => x.Id == item.Id).RuleSetId;
                    item.PosRuleSetId = coverages.First(x => x.Id == item.Id).PosRuleSetId;
                    item.IsPrimary = coverages.First(x => x.Id == item.Id).IsPrimary;
                    item.IsVisible = true;
                });
                risk.Risk.Coverages = ciaCoverages;
                risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
            }
            else
            {
                throw new Exception(Errors.ErrorCoverages);
            }
            risk.Risk.IsPersisted = true;
            risk = DelegateService.suretyService.QuotateSurety(risk, true, true);

            return risk;
        }

        /// <summary>
        /// Creates the surety.
        /// </summary>
        /// <param name="item">The item.</param>
        private CompanyRisk CreateSurety(SEM.CompanyContract item, CompanyPolicy policy)
        {
            item.Risk.IsPersisted = true;
            SEM.CompanyContract surety = item;

            surety = GetDataRenewal(surety, policy);
            surety.Risk.RiskId = 0;

            surety = DelegateService.suretyService.CreateSuretyTemporal(surety, false);

            return surety.Risk;
        }
    }
}
