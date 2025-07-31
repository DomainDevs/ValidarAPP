using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistran.Company.Application.ChangeConsolidationEndorsement.EEProvider.Entities.Views;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.ChangeConsolidationEndorsement.EEProvider.Assemblers;
using Sistran.Company.ChangeConsolidationEndorsement.EEProvider.Resources;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;
namespace Sistran.Company.ChangeConsolidationEndorsement.EEProvider.DAOs
{
    public class ChangeConsolidationDAO
    {
        /// <summary>
        /// Tarifar Traslado de vigencia Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>      
        /// <returns>Riesgos</returns>
        public List<CompanyRisk> QuotateChangeConsolidationCia(CompanyPolicy policy)
        {
            if (policy == null)
            {
                throw new ArgumentException(Errors.PolicyNotFound);
            }
            object localLockObject = new object();
            var mapper = ModelAssembler.CreateMapEndorsement();
            List<CompanyEndorsement> endorsements = mapper.Map<List<Endorsement>, List<CompanyEndorsement>>(DelegateService.underwritingService.GetEffectiveEndorsementsByPolicyId(policy.Endorsement.PolicyId));
            if (endorsements != null && endorsements.Any())
            {
                int lastEndorsementId = 0;
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                filter.In();
                filter.ListValue();
                TP.Parallel.ForEach(endorsements, endorsement =>
                {
                    filter.Constant(endorsement.Id);
                    lastEndorsementId = endorsement.Id;
                });
                filter.EndList();
                //Para endoso de cambio de intermediaron no se tiene en cuenta los riesgos excluidos
                filter.And();
                filter.Not();
                filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
                filter.In();
                filter.ListValue();
                filter.Constant(RiskStatusType.Excluded);
                filter.Constant(RiskStatusType.Cancelled);
                filter.EndList();
                //filter.And();
                //filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
                //filter.Equal();
                //filter.Constant(true);

                CoverageConsolidationEndorsementView view = new CoverageConsolidationEndorsementView();
                ViewBuilder builder = new ViewBuilder("CoverageEndorsementView");
                builder.Filter = filter.GetPredicate();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.FillView(builder, view);
                }
                if (view?.RiskCoverages != null && view.RiskCoverages.Count > 0)
                {
                    int modificationDays = Convert.ToInt32((policy.CurrentTo - policy.CurrentFrom).TotalDays);
                    if (modificationDays != 0)
                    {
                        List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().Where(x => x.IsCurrent == true && x.RiskStatusCode != (int)RiskStatusType.Excluded && x.RiskStatusCode != (int)RiskStatusType.Cancelled).ToList();
                        List<ISSEN.EndorsementRiskCoverage> endorsementRiskCoverages = view.EndorsementRiskCoverages.Cast<ISSEN.EndorsementRiskCoverage>().ToList().Where(x => x.CoverStatusCode != (int)CoverageStatusType.Excluded && x.CoverStatusCode != (int)CoverageStatusType.Cancelled).ToList();
                        List<ISSEN.RiskCoverage> riskCoverages = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
                        List<ISSEN.RiskSurety> riskSureties = view.RiskSureties != null && (view.RiskSureties.Count > 0) ? view.RiskSureties.Cast<ISSEN.RiskSurety>().ToList() : null;
                        List<CompanyRisk> risks = new List<CompanyRisk>();
                        Parallel.ForEach<ISSEN.EndorsementRisk, List<CompanyRisk>>(endorsementRisks,
                          () => { return new List<CompanyRisk>(); },
                          (risk, state, localrisk) =>
                          {
                              var currenRiks = new CompanyRisk
                              {
                                  //Id = 0,
                                  //RiskId = 0,
                                  Id = risk.RiskId,
                                  RiskId = risk.RiskId,
                                  Number = risk.RiskNum,
                                  Status = RiskStatusType.Modified,
                                  AmountInsured = riskSureties != null ? riskSureties.Where(x => x.RiskId == risk.RiskId).FirstOrDefault().ContractAmount : 0
                              };
                              var coverages = from er in endorsementRiskCoverages
                                              join rc in riskCoverages
                                              on er.RiskCoverId equals rc.RiskCoverId
                                              where er.EndorsementId.Equals(risk.EndorsementId)
                                              && er.RiskNum == risk.RiskNum
                                              select rc;
                              currenRiks.Coverages = CoveragesByCoverages(coverages.ToList());
                              ConcurrentBag<String> errors = new ConcurrentBag<String>();
                              TP.Parallel.ForEach(currenRiks.Coverages, (coverage) =>
                              {
                                  //Coberturas del riesgo todos los endosos

                                  var endorsementCoverages = from er in endorsementRiskCoverages
                                                             join rc in riskCoverages
                                                             on er.RiskCoverId equals rc.RiskCoverId
                                                             where er.RiskNum == risk.RiskNum
                                                             && rc.CoverageId == coverage.Id
                                                             select rc;
                                  var coverageCurrent = new CompanyCoverage();
                                  decimal premiuntTotal = Decimal.Zero;
                                  object lockData = new object();
                                  Parallel.ForEach<ISSEN.RiskCoverage, CompanyCoverage>(endorsementCoverages.ToList(),
                                       () => { return new CompanyCoverage(); },
                                      (subCoverage, State, coverageResult) =>
                                      {
                                          if (subCoverage != null)
                                          {
                                              decimal premiumCoverage = decimal.Zero;
                                              int originalDays = Convert.ToInt32((subCoverage.CurrentTo.Value - subCoverage.CurrentFrom.Value).TotalDays);
                                              if (policy.CurrentFrom >= subCoverage.CurrentFrom.GetValueOrDefault() && policy.CurrentFrom < subCoverage.CurrentTo.GetValueOrDefault())
                                              {
                                                  originalDays = Convert.ToInt32((subCoverage.CurrentTo.Value - subCoverage.CurrentFrom.Value).TotalDays);

                                                  lock (localLockObject)
                                                  {
                                                      modificationDays = Convert.ToInt32((subCoverage.CurrentTo.Value - policy.CurrentFrom).TotalDays);
                                                      premiumCoverage = decimal.Round((subCoverage.PremiumAmount / originalDays) * modificationDays, QuoteManager.RoundValue);
                                                  }

                                                  coverageResult.CurrentFrom = policy.CurrentFrom;
                                              }
                                              else if (policy.CurrentFrom < subCoverage.CurrentFrom.GetValueOrDefault())
                                              {
                                                  premiumCoverage = subCoverage.PremiumAmount;
                                                  coverageResult.CurrentFrom = policy.CurrentFrom;
                                              }
                                              else if (policy.CurrentFrom > (subCoverage.CurrentTo ?? DateTime.MinValue))
                                              {
                                                  premiumCoverage = 0;
                                              }
                                              lock (lockData)
                                              {
                                                  premiuntTotal += premiumCoverage;
                                              }
                                          }
                                          else
                                          {
                                              errors.Add(Errors.CoverageNotFound);
                                          }
                                          return coverageResult;
                                      }
                                  ,
                                  (finalResult) => { lock (localLockObject) coverageCurrent = finalResult; }
                                  );
                                  coverage.PremiumAmount = decimal.Round(premiuntTotal, QuoteManager.RoundValue);
                                  coverage.EndorsementLimitAmount = coverage.LimitAmount;
                                  coverage.EndorsementSublimitAmount = coverage.SubLimitAmount;
                                  coverage.LimitOccurrenceAmount = coverage.LimitOccurrenceAmount;
                                  coverage.LimitClaimantAmount = coverage.LimitClaimantAmount;
                                  coverage.CurrentFrom = policy.CurrentFrom > endorsementCoverages.Where(x => x.RiskCoverId == coverage.RiskCoverageId).Min(x => x.CurrentFrom).GetValueOrDefault() ? policy.CurrentFrom : endorsementCoverages.Where(x => x.RiskCoverId == coverage.RiskCoverageId).Min(x => x.CurrentFrom).GetValueOrDefault();
                                  coverage.OriginalRate = coverage.Rate;
                                  coverage.CoverageOriginalStatus = coverage.CoverStatus;
                                  coverage.CoverStatus = CoverageStatusType.Included;
                                  coverage.EndorsementType = EndorsementType.ChangeConsolidationEndorsement;
                              }
                              );
                              localrisk.Add(currenRiks);

                              return localrisk;
                          },
                           (riskresult) => { lock (localLockObject) risks.AddRange(riskresult); }
                           );
                        return risks;
                    }
                    else
                    {
                        throw new BusinessException(String.Format("{0} {1}", Errors.ValidDays, 0));
                    }
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
