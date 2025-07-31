using Sistran.Core.Application.SuretyEndorsementCancellationService.EEProvider;
using System;
using sp = Sistran.Core.Framework.DAF.Engine.StoredProcedure;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using Sistran.Core.Framework.Queries;
using Core.CancellationEndorsement3GProvider.Entities.Views;
using Sistran.Core.Framework.DAF.Engine;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.Linq;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Co.Application.Data;

namespace Sistran.Core.Application.SuretyEndorsementCancellationService3GProvider.DAOs
{
    public class CancellationDAO
    {
        /// <summary>
        /// Crear temporal de cancelacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <param name="cancellationFactor">factor de cancelacion</param>
        /// <returns>Id temporal</returns>
        public int CreateTemporalEndorsementCancellation(Policy policy, int cancellationFactor, string userName)
        {
            NameValue[] parameters = new NameValue[7];

            parameters[0] = new NameValue("@POLICY_ID", policy.Endorsement.PolicyId);
            parameters[1] = new NameValue("@USER_ID", policy.UserId);
            if (string.IsNullOrEmpty(policy.Endorsement.Text.TextBody))
            {
                parameters[2] = new NameValue("@CONDITION_TEXT", DBNull.Value);
            }
            else
            {
                parameters[2] = new NameValue("@CONDITION_TEXT", policy.Endorsement.Text.TextBody);
            }
            parameters[3] = new NameValue("@ENDO_REASON_CD", policy.Endorsement.EndorsementReasonId);
            if (string.IsNullOrEmpty(policy.Endorsement.Text.Observations))
            {
                parameters[4] = new NameValue("@ANNOTATIONS", DBNull.Value);
            }
            else
            {
                parameters[4] = new NameValue("@ANNOTATIONS", policy.Endorsement.Text.Observations);
            }
            parameters[5] = new NameValue("@CANCELL", cancellationFactor);
            parameters[6] = new NameValue("@USER_NAME", userName);
            
            object result = null;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPScalar("TMP.CANCELLATION_POLICY_SURETY", parameters);
            }

            return Convert.ToInt32(result);
        }
        /// <summary>
        /// Tarifar Cancelación de Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="cancellationFactor">Factor de Cancelación</param>
        /// <returns>Riesgos</returns>
        public List<Risk> QuotateCancellationSurety(Policy policy, int cancellationFactor)
        {
            List<Endorsement> endorsements = DelegateService.underwritingService.GetEffectiveEndorsementsByPolicyId(policy.Endorsement.PolicyId);

            if (endorsements != null)
            {
                int lastEndorsementId = 0;

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                filter.In();
                filter.ListValue();
                foreach (Endorsement endorsement in endorsements)
                {
                    filter.Constant(endorsement.Id);
                    lastEndorsementId = endorsement.Id;
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

                    foreach (ISSEN.RiskCoverage riskCoverage in riskCoverages)
                    {
                        DataFacadeManager.Instance.GetDataFacade().LoadDynamicProperties(riskCoverage);

                        if (policy.CurrentFrom > riskCoverage.CurrentFrom.Value && policy.CurrentFrom < riskCoverage.CurrentTo.Value)
                        {
                            int originalDays = Convert.ToInt32((riskCoverage.CurrentTo.Value - riskCoverage.CurrentFrom.Value).TotalDays);
                            riskCoverage.PremiumAmount = (riskCoverage.PremiumAmount / originalDays) * cancellationDays;
                        }
                        else if (policy.CurrentFrom > riskCoverage.CurrentTo.Value)
                        {
                            riskCoverage.PremiumAmount = 0;
                        }

                        Risk risk = null;
                        ISSEN.EndorsementRiskCoverage endorsementRiskCoverage = endorsementRiskCoverages.First(x => x.RiskCoverId == riskCoverage.RiskCoverId);

                        if (risks.Exists(x => x.Id == endorsementRiskCoverage.RiskNum))
                        {
                            risk = risks.First(x => x.Id == endorsementRiskCoverage.RiskNum);
                            risks.Remove(risk);

                            if (endorsementRisks.First(x => x.EndorsementId == endorsementRiskCoverage.EndorsementId && x.RiskNum == endorsementRiskCoverage.RiskNum).IsCurrent)
                            {
                                risk.RiskId = endorsementRiskCoverage.RiskId;
                                if (risk.Coverages.Exists(x => x.Id == riskCoverage.CoverageId))
                                {
                                    risk.Coverages.First(x => x.Id == riskCoverage.CoverageId).EndorsementLimitAmount = riskCoverage.LimitAmount;
                                    risk.Coverages.First(x => x.Id == riskCoverage.CoverageId).EndorsementSublimitAmount = riskCoverage.SublimitAmount;
                                }
                            }
                            if (risk.Coverages.Exists(x => x.Id == riskCoverage.CoverageId))
                            {
                                risk.Coverages.First(x => x.Id == riskCoverage.CoverageId).PremiumAmount += riskCoverage.PremiumAmount;
                            }
                            else
                            {
                                risk.Coverages.Add(ModelAssembler.CreateCoverage(riskCoverage));
                            }
                        }
                        else
                        {
                            risk = new Risk()
                            {
                                Id = endorsementRiskCoverage.RiskNum,
                                RiskId = endorsementRiskCoverage.RiskId,
                                Coverages = new List<Coverage>()
                            };
                            risk.Coverages.Add(ModelAssembler.CreateCoverage(riskCoverage));
                        }

                        risk.Status = (RiskStatusType)endorsementRisks.First(x => x.RiskId == risk.RiskId).RiskStatusCode;
                        risks.Add(risk);
                    }

                    foreach (Risk risk in risks)
                    {
                        risk.Coverages.ForEach(x => x.LimitOccurrenceAmount = x.LimitOccurrenceAmount * cancellationFactor);
                        risk.Coverages.ForEach(x => x.LimitClaimantAmount = x.LimitClaimantAmount * cancellationFactor);
                        risk.Coverages.ForEach(x => x.EndorsementLimitAmount = x.EndorsementLimitAmount * cancellationFactor);
                        risk.Coverages.ForEach(x => x.EndorsementSublimitAmount = x.EndorsementSublimitAmount * cancellationFactor);
                        risk.Coverages.ForEach(x => x.PremiumAmount = x.PremiumAmount * cancellationFactor);
                    }

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
    }
}
