using Sistran.Core.Application.ChangeAgentEndorsement.EEProvider.Assemblers;
using Sistran.Core.Application.ChangeAgentEndorsement.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
namespace Sistran.Core.Application.ChangeAgentEndorsement.EEProvider.DAOs
{
    /// <summary>
    /// Cambio de intermediario
    /// </summary>
    public class ChangeAgentDAO
    {

        /// <summary>
        /// Tarifar Traslado de vigencia Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>      
        /// <returns>Riesgos</returns>
        public List<Risk> QuotateChangeAgent(Policy policy)
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
                //Para endoso de cambio de intermediaron no se tiene en cuenta los riesgos excluidos
                filter.And();
                filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
                filter.In();
                filter.ListValue();
                filter.Constant(RiskStatusType.Included);
                filter.Constant(RiskStatusType.Modified);
                filter.Constant(RiskStatusType.NotModified);
                filter.Constant(RiskStatusType.Original);
                filter.Constant(RiskStatusType.Rehabilitated);
                filter.EndList();
                filter.And();
                filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
                filter.Equal();
                filter.Constant(true);

                CoverageEndorsementView view = new CoverageEndorsementView();
                ViewBuilder builder = new ViewBuilder("CoverageEndorsementView");
                builder.Filter = filter.GetPredicate();
                ChangeAgentEndorsementEEProvider.dataFacadeManager.GetDataFacade().FillView(builder, view);
                if (view.RiskCoverages.Count > 0)
                {
                    int cancellationDays = Convert.ToInt32((policy.CurrentTo - policy.CurrentFrom).TotalDays);
                    List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                    List<ISSEN.EndorsementRiskCoverage> endorsementRiskCoverages = view.EndorsementRiskCoverages.Cast<ISSEN.EndorsementRiskCoverage>().ToList();
                    List<ISSEN.RiskCoverage> riskCoverages = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
                    List<Risk> risks = new List<Risk>();

                    IDataFacade dataFacade = ChangeAgentEndorsementEEProvider.dataFacadeManager.GetDataFacade();
                    foreach (ISSEN.RiskCoverage riskCoverage in riskCoverages)
                    {
                        int originalDays = Convert.ToInt32((riskCoverage.CurrentTo.Value - riskCoverage.CurrentFrom.Value).TotalDays);

                        dataFacade.LoadDynamicProperties(riskCoverage);
                        if (policy.CurrentFrom > riskCoverage.CurrentFrom.Value)
                        {
                            riskCoverage.CurrentFrom = policy.CurrentFrom;
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
                                    risk.Coverages.First(x => x.Id == riskCoverage.CoverageId).DeclaredAmount = riskCoverage.DeclaredAmount;
                                    risk.Coverages.First(x => x.Id == riskCoverage.CoverageId).LimitAmount = riskCoverage.LimitAmount;
                                    risk.Coverages.First(x => x.Id == riskCoverage.CoverageId).SubLimitAmount = riskCoverage.SublimitAmount;
                                    risk.Coverages.First(x => x.Id == riskCoverage.CoverageId).ContractAmountPercentage += riskCoverage.ContractAmountPercentage == null ? 0 : riskCoverage.ContractAmountPercentage.Value;
                                }
                            }

                            riskCoverage.PremiumAmount = (riskCoverage.PremiumAmount / originalDays) * cancellationDays;
                            if (risk.Coverages.Exists(x => x.Id == riskCoverage.CoverageId))
                            {
                                risk.Coverages.First(x => x.Id == riskCoverage.CoverageId).PremiumAmount += riskCoverage.PremiumAmount;
                            }
                            else
                            {
                                risk.Coverages.Add(ModelAssembler.CreateCoverage(riskCoverage));
                            }

                            risk.Status = RiskStatusType.Included;
                        }
                        else
                        {
                            risk = new Risk()
                            {
                                Id = endorsementRiskCoverage.RiskNum,
                                RiskId = endorsementRiskCoverage.RiskId,
                                Status = RiskStatusType.Included,
                                Number = endorsementRiskCoverage.RiskNum,
                                Coverages = new List<Coverage>()
                            };
                            riskCoverage.PremiumAmount = (riskCoverage.PremiumAmount / originalDays) * cancellationDays;
                            risk.Coverages.Add(ModelAssembler.CreateCoverage(riskCoverage));
                        }
                        risks.Add(risk);
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

        /// <summary>
        /// Emitir Cambio de Intermediario de la Póliza       
        /// </summary>
        /// <param name="Id">Temporal</param>    
        /// <returns>Numero Endoso</returns>
        public Policy Execute(int Id)
        {
            var result = DelegateService.baseEndorsementService.CreateEndorsement(Id);
            return result;
        }
    }
}
