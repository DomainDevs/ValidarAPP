using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TM=System.Threading.Tasks;
using AutoMapper;
using Renewal.EEProvider.Assemblers;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.TransportRenewalService.EEProvider.Resources;
using Sistran.Company.Application.TransportRenewalService.EEProvider.Services;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.TransportRenewalService.EEProvider.Business
{
    public class RenewalBusinessCia
    {
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

                    policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
                    if (policy.Endorsement == null)
                    {
                        policy.Endorsement = new CompanyEndorsement();
                    }
                    policy.Endorsement.IsUnderIdenticalConditions = companyPolicy.Endorsement.IsUnderIdenticalConditions;
                    policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                    policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                    var imapper = AutoMapperAssembler.CreateMapCompanyClause();
                    policy.Clauses = imapper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                }
                policy.Endorsement = companyPolicy.Endorsement;
                policy.Endorsement.EndorsementType = EndorsementType.Renewal;
                policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                policy.TemporalType = TemporalType.Endorsement;
                policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);
                policy.TimeHour = "00";
                policy.CurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                policy.CurrentTo = companyPolicy.Endorsement.CurrentTo;
                policy.IssueDate = companyPolicy.IssueDate;

                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);

                if (policy != null)
                {
                    if (!companyPolicy.Endorsement.IsUnderIdenticalConditions && policy.Product.IsCollective)
                    {
                        return policy;
                    }
                    List<CompanyTransport> companyTransports = new List<CompanyTransport>();
                    if (id > 0)
                    {
                        companyTransports = DelegateService.transportService.GetCompanyTransportsByTemporalId(id);
                    }
                    else
                    {
                        companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyPolicy.Endorsement.PolicyId);
                    }
                    if (companyTransports != null)
                    {
                        ConcurrentBag<CompanyRisk> risks = new ConcurrentBag<CompanyRisk>();
                        Object objectLock = new Object();
                        if (policy.Product.IsCollective && companyPolicy.Endorsement.IsUnderIdenticalConditions)
                        {

                            TP.Parallel.ForEach(companyTransports,  item =>
                            {
                                item.Risk.Id = 0;
                                item.Risk.OriginalStatus = item.Risk.Status;
                                item.Risk.Status = RiskStatusType.Included;
                                item.Risk.Policy = policy;
                                CompanyRisk risk = Createtransport(item);
                                risks.Add(risk);
                            });
                            policy = provider.CalculatePolicyAmounts(policy, risks.ToList());
                        }
                        else
                        {
                            companyTransports.AsParallel().ForAll(x => x.Risk.Policy = policy);
                            ConcurrentBag<string> errorsRisk = new ConcurrentBag<string>();
                            TP.Parallel.ForEach(companyTransports,  item =>
                            {
                                try
                                {
                                    if (item != null)
                                    {
                                        var risk = new CompanyTransport();
                                        lock (objectLock)
                                        {
                                            risk.Risk.Id = 0;
                                            risk.Risk.OriginalStatus = item.Risk.Status;
                                            risk.Risk.Status = RiskStatusType.Included;
                                            risk = GetDataRenewal(item);
                                        }
                                        if (risk != null && risk.Risk != null)
                                        {

                                            risk = DelegateService.transportService.CreateCompanyTransportTemporal(risk);

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
                        throw new Exception(Errors.ErrorTransportNotFound);
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
        private CompanyTransport GetDataRenewal(CompanyTransport risk)
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

                String coverStatusName = String.Empty;
                if (Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original)) == null)
                {
                    coverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original);
                }
                else
                {
                    coverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original));
                }

                ciaCoverages.AsParallel().ForAll(item =>
                {

                    var coverageLocal = coverages.FirstOrDefault(u => u.Id == item.Id);

                    item.CoverStatus = CoverageStatusType.Original;
                    item.CoverStatusName = coverStatusName;
                    item.CurrentFrom = risk.Risk.Policy.CurrentFrom;
                    item.CurrentTo = risk.Risk.Policy.CurrentTo;
                    item.Description = coverageLocal.Description;
                    item.EndorsementType = risk.Risk.Policy.Endorsement.EndorsementType;
                    item.AccumulatedPremiumAmount = 0;
                    item.FlatRatePorcentage = coverageLocal.FlatRatePorcentage;
                    item.SubLineBusiness = coverageLocal.SubLineBusiness;
                    item.RuleSetId = coverageLocal.RuleSetId;
                    item.PosRuleSetId = coverageLocal.PosRuleSetId;
                    item.ScriptId = coverageLocal.ScriptId;
                    item.IsSelected = coverageLocal.IsSelected;
                    item.IsMandatory = coverageLocal.IsMandatory;
                    item.IsVisible = coverageLocal.IsVisible;
                    item.EndorsementLimitAmount = 0;
                    item.EndorsementSublimitAmount = 0;
                });
                risk.Risk.Coverages = ciaCoverages;
                risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
            }
            else
            {
                throw new Exception(Errors.ErrorCoverages);
            }
            risk.Risk.IsPersisted = true;
            risk = DelegateService.transportService.QuotateCompanyTransport(risk, true, true);

            return risk;
        }

        /// <summary>
        /// Creates the transport.
        /// </summary>
        /// <param name="item">The item.</param>
        private CompanyRisk Createtransport(CompanyTransport item)
        {
            item.Risk.IsPersisted = true;
            CompanyTransport transport = item;

            transport = GetDataRenewal(transport);
            transport.Risk.RiskId = 0;

            transport = DelegateService.transportService.CreateCompanyTransportTemporal(transport);

            return transport.Risk;
        }
    }
}
