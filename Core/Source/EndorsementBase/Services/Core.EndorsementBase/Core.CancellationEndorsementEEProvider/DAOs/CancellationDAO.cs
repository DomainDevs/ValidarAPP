using Core.CancellationEndorsement3GProvider.Assemblers;
using Core.CancellationEndorsement3GProvider.Entities.Views;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Parameter = Sistran.Core.Application.CommonService.Models.Parameter;

namespace Core.CancellationEndorsement3GProvider.DAOs
{
    public class CancellationDAO
    {
        /// <summary>
        /// Tarifar Cancelación de Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="cancellationFactor">Factor de Cancelación</param>
        /// <returns>Riesgos</returns>
        public List<Risk> QuotateCancellation(Policy policy, int cancellationFactor)
        {
            List<Endorsement> endorsements = DelegateService.underwritingService.GetEffectiveEndorsementsByPolicyId(policy.Endorsement.PolicyId);
            object localLockObject = new object();
            if (endorsements != null)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                filter.In();
                filter.ListValue();
                foreach (Endorsement endorsement in endorsements)
                {
                    filter.Constant(endorsement.Id);
                }
                filter.EndList();
                CoverageCancellationView view = new CoverageCancellationView();
                ViewBuilder builder = new ViewBuilder("CoverageCancellationView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                if (view.RiskCoverages.Count > 0)
                {
                    int cancellationDays = Convert.ToInt32((policy.CurrentTo - policy.CurrentFrom).TotalDays);
                    List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                    List<ISSEN.EndorsementRiskCoverage> endorsementRiskCoverages = view.EndorsementRiskCoverages.Cast<ISSEN.EndorsementRiskCoverage>().ToList();
                    List<ISSEN.RiskCoverage> riskCoverages = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
                    List<Risk> risks = new List<Risk>();
                    //Riesgos Activos
                    Parallel.ForEach<ISSEN.EndorsementRisk, List<Risk>>(endorsementRisks.Where(x => x.IsCurrent == true && x.RiskStatusCode != (int)RiskStatusType.Excluded),
                        () => { return new List<Risk>(); },
                        (risk, state, localrisk) =>
                        {
                            var currenRiks = new Risk
                            {
                                Id = risk.RiskNum,
                                RiskId = risk.RiskId,
                                Number = risk.RiskNum
                            };
                            var coverages = from er in endorsementRiskCoverages
                                            join rc in riskCoverages
                                            on er.RiskCoverId equals rc.RiskCoverId
                                            where er.EndorsementId.Equals(risk.EndorsementId)
                                            && er.RiskNum == risk.RiskNum
                                            select rc;
                            currenRiks.Coverages = CoveragesByCoverages(coverages.ToList());
                            TP.Parallel.ForEach(currenRiks.Coverages, (coverage) =>
                            {
                                //Coberturas del riesgo todos los endosos

                                var endorsementCoverages = from er in endorsementRiskCoverages
                                                           join rc in riskCoverages
                                                           on er.RiskCoverId equals rc.RiskCoverId
                                                           where er.RiskNum == risk.RiskNum
                                                           && rc.CoverageId == coverage.Id
                                                           select rc;
                                var coverageCurrent = new Coverage();
                                Parallel.ForEach<ISSEN.RiskCoverage, Coverage>(endorsementCoverages.ToList(),
                                     () => { return new Coverage(); },
                                    (subCoverage, State, coverageResult) =>
                                    {
                                        if (policy.CurrentFrom >= subCoverage.CurrentFrom.GetValueOrDefault() && policy.CurrentFrom < subCoverage.CurrentTo.GetValueOrDefault())
                                        {
                                            int originalDays = Convert.ToInt32((subCoverage.CurrentTo.Value - subCoverage.CurrentFrom.Value).TotalDays);
                                            if (policy.CurrentTo > subCoverage.CurrentTo)
                                            {
                                                lock (localLockObject)
                                                {
                                                    cancellationDays = originalDays;
                                                }
                                            }
                                            coverageResult.PremiumAmount += (subCoverage.PremiumAmount / originalDays) * cancellationDays;
                                            coverageResult.CurrentFrom = policy.CurrentFrom;
                                        }
                                        else if (policy.CurrentFrom < subCoverage.CurrentFrom.GetValueOrDefault())
                                        {
                                            coverageResult.PremiumAmount += subCoverage.PremiumAmount;
                                            coverageResult.CurrentFrom = policy.CurrentFrom;
                                        }
                                        else if (policy.CurrentFrom > (subCoverage.CurrentTo ?? DateTime.MinValue))
                                        {
                                            coverageResult.PremiumAmount = 0;
                                        }
                                        return coverageResult;
                                    }
                                ,
                                (finalResult) => { lock (localLockObject) coverageCurrent = finalResult; }
                                );
                                coverage.PremiumAmount = coverageCurrent.PremiumAmount * cancellationFactor;
                                coverage.EndorsementLimitAmount = coverage.EndorsementLimitAmount * cancellationFactor;
                                coverage.EndorsementSublimitAmount = coverage.EndorsementLimitAmount * cancellationFactor;
                                coverage.LimitOccurrenceAmount = coverage.LimitOccurrenceAmount * cancellationFactor;
                                coverage.LimitClaimantAmount = coverage.LimitClaimantAmount * cancellationFactor;
                                coverageCurrent.CurrentFrom = policy.CurrentFrom > endorsementCoverages.Where(x => x.RiskCoverId == coverage.RiskCoverageId).Min(x => x.CurrentFrom).GetValueOrDefault() ? policy.CurrentFrom : endorsementCoverages.Where(x => x.RiskCoverId == coverage.RiskCoverageId).Min(x => x.CurrentFrom).GetValueOrDefault();
                                coverage.CoverStatus = CoverageStatusType.Excluded;
                                coverage.EndorsementType = EndorsementType.Cancellation;
                            }
                            );
                            localrisk.Add(currenRiks);

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
        private List<Risk> CalculateShortTerm(Policy policy, List<Risk> risks)
        {
            RuleProcessRuleSet rules = DelegateService.rulesService.GetRulestByRulsetProcessType((int)ProcessTypes.ShortTerm);
            var dynamicConceptId = rules.ConceptId;
            decimal shortTermPercentage = 0;
            Policy policyRule;
            policyRule = DelegateService.underwritingService.RunRulesPolicy(policy, rules.PosRuleSet);

            foreach (var item in policyRule.DynamicProperties)
            {
                if (item.Id == dynamicConceptId)
                {
                    shortTermPercentage = (decimal)item.Value;
                }
            }

            if (shortTermPercentage > 0)
            {
                foreach (Risk item in risks)
                {
                    item.Coverages.ForEach(c => c.PremiumAmount = (c.PremiumAmount - (c.PremiumAmount * shortTermPercentage / 100)));
                    item.Coverages.ForEach(c => c.ShortTermPercentage = shortTermPercentage);
                    item.Premium = item.Coverages.Sum(x => x.PremiumAmount);
                }
            }
            return risks;
        }

        /// <summary>
        /// Coverageses the by coverages.
        /// </summary>
        /// <param name="rc">RiskCoverage</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">rc</exception>
        private List<Coverage> CoveragesByCoverages(List<ISSEN.RiskCoverage> rc)
        {
            if (rc == null)
            {
                throw new ArgumentNullException(nameof(rc));
            }

            var resultCollection = new List<Coverage>();
            object localLockObject = new object();
            Parallel.ForEach<ISSEN.RiskCoverage, List<Coverage>>(rc, () => { return new List<Coverage>(); }
                                , (coverage, state, localCoverage) =>
                                {
                                    localCoverage.Add(ModelAssembler.CreateCoverage(coverage));
                                    return localCoverage;
                                }
                                ,
                (finalResult) => { lock (localLockObject) resultCollection.AddRange(finalResult); }
                );
            return resultCollection;
        }

    }
}
