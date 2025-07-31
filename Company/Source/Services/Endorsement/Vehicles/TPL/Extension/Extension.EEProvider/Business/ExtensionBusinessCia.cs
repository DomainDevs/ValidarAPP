using AutoMapper;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.ThirdPartyLiabilityEndorsementExtensionService.EEProvider.Assemblers;
using Sistran.Company.Application.ThirdPartyLiabilityEndorsementExtensionService.EEProvider.Resources;
using Sistran.Company.Application.ThirdPartyLiabilityEndorsementExtensionService.EEProvider.Services;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TM=System.Threading.Tasks;
using baf = Sistran.Core.Framework.BAF;
using TPLEM = Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.ThirdPartyLiabilityEndorsementExtensionService.EEProvider.Business
{
    class ExtensionBusinessCia
    {
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyLiabilityEndorsementBusinessCia" /> class.
        /// </summary>
        public ExtensionBusinessCia()
        {
            provider = new BaseBusinessCia();
        }

        public CompanyPolicy CreateEndorsementExtension(CompanyPolicy companyPolicy)
        {
            try
            {
                CompanyPolicy policy;
                int Id = 0;
                if (companyPolicy == null)
                {
                    throw new ArgumentException(Errors.EmptyPolicy);
                }
                if (companyPolicy?.Endorsement?.TemporalId > 0)
                {
                    policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Endorsement.TemporalId, false);
                    Id = policy.Id;
                }
                else
                {
                    policy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyPolicy.Endorsement.Id);
                    if (policy != null)
                    {
                        policy.Id = 0;
                        Id = policy.Id;
                        policy.UserId = companyPolicy.UserId;
                        policy.CurrentFrom = policy.CurrentTo;
                        policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
                        if (policy.Endorsement == null)
                        {
                            policy.Endorsement = new CompanyEndorsement();
                        }
                        policy.Endorsement.Text = new CompanyText
                        {
                            TextBody = companyPolicy.Endorsement.Text.TextBody,
                            Observations = companyPolicy.Endorsement.Text.Observations
                        };
                        policy.Endorsement.TicketDate = companyPolicy.Endorsement.TicketDate;
                        policy.Endorsement.TicketNumber = companyPolicy.Endorsement.TicketNumber;
                        policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                        policy.Endorsement.EndorsementType = EndorsementType.EffectiveExtension;
                        policy.Endorsement.EndorsementTypeDescription = Errors.ResourceManager.GetString(EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType));
                        policy.TemporalType = TemporalType.Endorsement;
                        policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);
                        var mapper =  ModelAssembler.CreateMapCompanyClause();
                        var companyclauses = DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(Core.Application.UnderwritingServices.Enums.EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory)?.ToList();
                        if (companyclauses != null)
                        {
                            policy.Clauses = mapper.Map<List<Clause>, List<CompanyClause>>(companyclauses);
                        }
                        policy.Summary = new CompanySummary
                        {
                            RiskCount = 0
                        };
                    }
                    else
                    {
                        throw new ArgumentException(Errors.NonExistentPolicyOrNonExistentEndorsement);
                    }
                }
                policy.CurrentTo = companyPolicy.Endorsement.CurrentTo;
                policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                policy.Endorsement.EndorsementReasonDescription = companyPolicy.Endorsement.EndorsementReasonDescription;
                policy.Endorsement.Text = new CompanyText
                {
                    TextBody = companyPolicy.Endorsement.Text?.TextBody,
                    Observations = companyPolicy.Endorsement.Text?.Observations
                };
                var policyEndorsement = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                var policyResult = CreateExtension(Id, policyEndorsement);
                return policyResult;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private CompanyPolicy CreateExtension(int Id, CompanyPolicy policy)
        {

            if (policy != null)
            {
                List<TPLEM.CompanyTplRisk> companyTpls = new List<TPLEM.CompanyTplRisk>();
                ConcurrentBag<CompanyRisk> risks = new ConcurrentBag<CompanyRisk>();
                PendingOperation pendingOperation = new PendingOperation();

                if (Id > 0)
                {
                    companyTpls = DelegateService.tplService.GetThirdPartyLiabilitiesByTemporalId(policy.Id);
                }
                else
                {
                    companyTpls = DelegateService.tplService.GetCompanyThirdPartyLiabilitiesByPolicyId(policy.Endorsement.PolicyId);
                    if (companyTpls != null && companyTpls.Any())
                    {
                        companyTpls.AsParallel().ForAll(x =>
                        {
                            x.Risk.Id = 0;
                            x.Risk.IsPersisted = true;
                        }
                        );
                    }
                    else
                    {
                        throw new Exception(Errors.ThereIsNoRiskForTheEndorsement);
                    }

                }
                if (companyTpls != null)
                {
                    // companyTpls.AsParallel().ForAll(x => x.Risk.Policy = policy);
                    TP.Parallel.ForEach(companyTpls, item =>
                        {
                            item.Risk.IsPersisted = true;
                            item.Risk.Policy = policy;
                            var companyTplrisks = GetDataExtension(item);
                            companyTplrisks = DelegateService.tplService.CreateThirdPartyLiabilityTemporal(companyTplrisks, false);
                            if (companyTplrisks.Risk.InfringementPolicies != null && companyTplrisks.Risk.InfringementPolicies.Any())
                            {
                                companyTplrisks.Risk.Policy.InfringementPolicies.AddRange(companyTplrisks.Risk.InfringementPolicies);
                            }
                            risks.Add(companyTplrisks.Risk);

                        });
                    if (risks != null)
                    {
                        var tplPolicy = provider.CalculatePolicyAmounts(policy, risks.ToList());
                        tplPolicy = DelegateService.underwritingService.CreatePolicyTemporal(tplPolicy, false);
                        return tplPolicy;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorObtainingRisks);
                    }
                }
                else
                {
                    throw new Exception(Errors.ErrorObtainingRisks);
                }              
            }
            else
            {
                throw new Exception(Errors.PolicyParameterEmptyOrNull);
            }


        }
        private TPLEM.CompanyTplRisk GetDataExtension(TPLEM.CompanyTplRisk risk)
        {
            risk.StandardPrice = risk.NewPrice;
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
                    item.CoverStatus = item.CoverageOriginalStatus;
                    //item.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(item.CoverageOriginalStatus));
                    item.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original));
                    item.CurrentFrom = risk.Risk.Policy.CurrentFrom;
                    item.CurrentTo = risk.Risk.Policy.CurrentTo;
                    item.Description = coverages.FirstOrDefault(u => u.Id == item.Id).Description;
                    item.EndorsementType = risk.Risk.Policy.Endorsement.EndorsementType;
                    item.AccumulatedPremiumAmount = 0;
                    item.FlatRatePorcentage = 0;
                    item.SubLineBusiness = coverages.First(x => x.Id == item.Id).SubLineBusiness;
                });
                risk.Risk.Coverages = ciaCoverages;
                risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
            }
            else
            {
                throw new Exception(Errors.ErrorCoverages);
            }


            risk = DelegateService.tplService.QuotateThirdPartyLiability(risk, false, false);

            return risk;
        }
    }
}
