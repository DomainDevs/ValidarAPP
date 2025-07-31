using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.CancellationEndorsementEEProvider.Entities.Views;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.CancellationEndorsement.EEProvider.Resources;
using Sistran.Company.CancellationEndorsementEEProvider.Assemblers;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.CancellationEndorsement.EEProvider.DAOs
{
    public class CiaCancellationDAO
    {
        /// <summary>
        /// Tarifar Cancelación de Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="cancellationFactor">Factor de Cancelación</param>
        /// <returns>Riesgos</returns>
        public List<CompanyRisk> QuotateCancellation(CompanyPolicy policy, int cancellationFactor)
        {
            if (policy == null)
            {
                throw new ArgumentException(Errors.PolicyNotFound);
            }
            IMapper mapper = ModelAssembler.CreateMapEndorsement();
            List<CompanyEndorsement> endorsements = mapper.Map<List<Endorsement>, List<CompanyEndorsement>>(DelegateService.underwritingService.GetEffectiveEndorsementsByPolicyId(policy.Endorsement.PolicyId));
            object localLockObject = new object();
            if (endorsements != null)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                filter.In();
                filter.ListValue();
                foreach (CompanyEndorsement endorsement in endorsements)
                {
                    filter.Constant(endorsement.Id);
                }
                filter.EndList();
                CompanyCoverageCancellationView view = new CompanyCoverageCancellationView();
                ViewBuilder builder = new ViewBuilder("CompanyCoverageCancellationView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                if (view?.RiskCoverages != null && view.RiskCoverages.Count > 0)
                {
                    int cancellationDays = Convert.ToInt32((policy.CurrentTo - policy.CurrentFrom).TotalDays);
                    List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                    List<ISSEN.EndorsementRiskCoverage> endorsementRiskCoverages = view.EndorsementRiskCoverages.Cast<ISSEN.EndorsementRiskCoverage>().ToList();
                    List<ISSEN.RiskCoverage> riskCoverages = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
                    List<CompanyRisk> risks = new List<CompanyRisk>();
                    //Riesgos Activos
                    Parallel.ForEach<ISSEN.EndorsementRisk, List<CompanyRisk>>(endorsementRisks.Where(x => x.IsCurrent == true && x.RiskStatusCode != (int)RiskStatusType.Excluded),
                        () => { return new List<CompanyRisk>(); },
                        (risk, state, localrisk) =>
                        {
                            CompanyRisk currentRisks = new CompanyRisk
                            {
                                Id = 0,
                                RiskId = 0,
                                Number = risk.RiskNum,
                                Status = RiskStatusType.Excluded
                            };
                            IEnumerable<ISSEN.RiskCoverage> coverages = from er in endorsementRiskCoverages
                                                                        join rc in riskCoverages
                                                                        on er.RiskCoverId equals rc.RiskCoverId
                                                                        where er.EndorsementId.Equals(risk.EndorsementId)
                                                                        && er.RiskNum == risk.RiskNum
                                                                        select rc;
                            currentRisks.Coverages = CoveragesByCoverages(coverages.ToList());
                            TP.Parallel.ForEach(currentRisks.Coverages, (coverage) =>
                           {
                                //Coberturas del riesgo todos los endosos
                                IEnumerable<ISSEN.RiskCoverage> endorsementCoverages = from er in endorsementRiskCoverages
                                                                                      join rc in riskCoverages
                                                                                      on er.RiskCoverId equals rc.RiskCoverId
                                                                                      where er.RiskNum == risk.RiskNum
                                                                                      && rc.CoverageId == coverage.Id
                                                                                      select rc;
                               CompanyCoverage coverageCurrent = new CompanyCoverage();
                               decimal premiuntTotal = Decimal.Zero;
                               object lockData = new object();
                               Parallel.ForEach<ISSEN.RiskCoverage, CompanyCoverage>(endorsementCoverages.ToList(),
                                    () => { return new CompanyCoverage(); },
                                   (subCoverage, State, coverageResult) =>
                                   {
                                       decimal premiumCoverage = Decimal.Zero;
                                       int originalDays = Convert.ToInt32((subCoverage.CurrentTo.Value - subCoverage.CurrentFrom.Value).TotalDays);

                                       if (policy.CurrentFrom >= subCoverage.CurrentFrom.GetValueOrDefault() && policy.CurrentFrom < subCoverage.CurrentTo.GetValueOrDefault())
                                       {
                                           originalDays = Convert.ToInt32((subCoverage.CurrentTo.Value - subCoverage.CurrentFrom.Value).TotalDays);

                                           lock (localLockObject)
                                           {
                                               cancellationDays = Convert.ToInt32((subCoverage.CurrentTo.Value - policy.CurrentFrom).TotalDays);
                                               premiumCoverage = decimal.Round((subCoverage.PremiumAmount / originalDays) * cancellationDays, QuoteManager.RoundValue);
                                           }

                                           coverageResult.CurrentFrom = policy.CurrentFrom;
                                       }
                                       else if (policy.CurrentFrom <= subCoverage.CurrentFrom.GetValueOrDefault())
                                       {
                                           premiumCoverage = subCoverage.PremiumAmount;
                                           coverageResult.CurrentFrom = policy.CurrentFrom;
                                       }
                                       else if (policy.CurrentFrom > (subCoverage.CurrentTo ?? DateTime.MinValue))
                                       {
                                           coverageResult.PremiumAmount = 0;
                                       }
                                       lock (lockData)
                                       {
                                           premiuntTotal += premiumCoverage;
                                       }
                                       return coverageResult;
                                   }
                               ,
                               (finalResult) => { lock (localLockObject) coverageCurrent = finalResult; }
                               );
                               coverage.PremiumAmount = decimal.Round(premiuntTotal * cancellationFactor, QuoteManager.RoundValue);
                               if (coverage.RateType == Core.Services.UtilitiesServices.Enums.RateType.FixedValue)
                               {
                                   coverage.Rate = 0;
                               }

                               coverage.EndorsementLimitAmount = coverage.LimitAmount * cancellationFactor;
                               coverage.EndorsementSublimitAmount = coverage.SubLimitAmount * cancellationFactor;
                               coverage.LimitOccurrenceAmount = coverage.LimitOccurrenceAmount * cancellationFactor;
                               coverage.LimitClaimantAmount = coverage.LimitClaimantAmount * cancellationFactor;
                               coverage.MaxLiabilityAmount = coverage.MaxLiabilityAmount * cancellationFactor;
                               coverage.ExcessLimit = coverage.ExcessLimit * cancellationFactor;
                               coverage.CurrentFrom = policy.CurrentFrom > endorsementCoverages.Where(x => x.RiskCoverId == coverage.RiskCoverageId).Min(x => x.CurrentFrom).GetValueOrDefault() ? policy.CurrentFrom : endorsementCoverages.Where(x => x.RiskCoverId == coverage.RiskCoverageId).Min(x => x.CurrentFrom).GetValueOrDefault();
                               coverage.OriginalRate = coverage.Rate;
                               coverage.CoverageOriginalStatus = coverage.CoverStatus;
                               coverage.CoverStatus = CoverageStatusType.Cancelled;
                               coverage.EndorsementType = policy.Endorsement.EndorsementType;
                                //Regla de cancelacion
                                coverage.LimitAmount = 0;
                               if ((CancellationType)policy.Endorsement.CancellationTypeId != CancellationType.Nominative)
                               {
                                   coverage.SubLimitAmount = 0;
                                   coverage.DeclaredAmount = 0;
                               }

                           }
                            );
                            localrisk.Add(currentRisks);

                            if ((CancellationType)policy.Endorsement.CancellationTypeId == CancellationType.ShortTerm)
                            {
                                localrisk = CalculateShortTerm(policy, localrisk);
                            }
                            return localrisk;
                        },
                         (riskresult) => { lock (localLockObject) risks.AddRange(riskresult); }
                         );
                    return risks;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        private List<CompanyRisk> CalculateShortTerm(CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                RuleProcessRuleSet rules = DelegateService.rulesService.GetRulestByRulsetProcessType((int)ProcessTypes.ShortTerm);
                var dynamicConceptId = rules.ConceptId;
                decimal shortTermPercentage = 0;
                Policy policyCore = new Policy();

                var config = ModelAssembler.CreateMapPolicy();

                policyCore = config.Map<CompanyPolicy, Policy>(policy);

                policyCore = DelegateService.underwritingService.RunRulesPolicy(policyCore, rules.PosRuleSet);

                foreach (var item in policyCore.DynamicProperties)
                {
                    if (item.Id == dynamicConceptId)
                    {
                        shortTermPercentage = (decimal)item.Value;
                    }
                }

                if (shortTermPercentage > 0)
                {
                    foreach (CompanyRisk item in risks)
                    {
                        item.Coverages.ForEach(c => c.PremiumAmount = (c.PremiumAmount - (c.PremiumAmount * shortTermPercentage / 100)));
                        item.Coverages.ForEach(c => c.ShortTermPercentage = shortTermPercentage);
                        item.Premium = item.Coverages.Sum(x => x.PremiumAmount);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("WARM UP SHORT TERM:", ex.Message);

            }
            return risks;
        }

        /// <summary>
        /// Coverageses the by coverages.
        /// </summary>
        /// <param name="rc">RiskCoverage</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">rc</exception>
        private List<CompanyCoverage> CoveragesByCoverages(List<ISSEN.RiskCoverage> rc)
        {
            if (rc == null)
            {
                throw new ArgumentNullException(nameof(rc));
            }
            return ModelAssembler.CreateCiaCoverages(rc);
        }
    }
}
