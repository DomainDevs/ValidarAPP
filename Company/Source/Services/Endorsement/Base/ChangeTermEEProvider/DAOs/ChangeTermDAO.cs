using AutoMapper;
using Sistran.Company.Application.ChangeTermEndorsement.EEProvider.Assemblers;
using ENVW = Sistran.Company.Application.ChangeTermEndorsement.EEProvider.Entities.Views;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.ChangeTermEndorsement.EEProvider.Resources;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using QUO = Sistran.Core.Application.Quotation.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;
using Sistran.Company.Application.ChangeTermEndorsement.EEProvider.Entities.Views;

namespace Sistran.Company.Application.ChangeTermEndorsement.EEProvider.DAOs
{
    /// <summary>
    /// Traslado de vigencia
    /// </summary>
    public class CiaChangeTermDAO
    {

        /// <summary>
        /// Tarifar Traslado de vigencia Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>      
        /// <returns>Riesgos</returns>
        public List<CompanyRisk> QuotateChangeTermCia(CompanyPolicy policy)
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
                foreach (CompanyEndorsement endorsement in endorsements)
                {
                    filter.Constant(endorsement.Id);
                    lastEndorsementId = endorsement.Id;
                }
                filter.EndList();

                CompanyCoverageEndorsementTerm view = new CompanyCoverageEndorsementTerm();
                ViewBuilder viewbuilder = new ViewBuilder("CompanyCoverageEndorsementTerm");
                viewbuilder.Filter = filter.GetPredicate();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.FillView(viewbuilder, view);
                }
                if (view?.RiskCoverages != null && view.RiskCoverages.Count > 0)
                {
                    int daysDifference = (policy.CurrentFrom - policy.Endorsement.CurrentFrom).Days;
                    int modificationDays = Convert.ToInt32((policy.CurrentTo - policy.CurrentFrom).TotalDays);
                    if (modificationDays != 0)
                    {
                        List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().Where(x => x.IsCurrent == true && x.RiskStatusCode != (int)RiskStatusType.Excluded && x.RiskStatusCode != (int)RiskStatusType.Cancelled).ToList();
                        List<ISSEN.EndorsementRiskCoverage> endorsementRiskCoverages = view.EndorsementRiskCoverages.Cast<ISSEN.EndorsementRiskCoverage>().ToList().Where(x => x.CoverStatusCode != (int)CoverageStatusType.Excluded /*&& x.CoverStatusCode != (int)CoverageStatusType.Cancelled*/).ToList();
                        List<ISSEN.RiskCoverage> riskCoverages = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();
                        List<QUO.Coverage> coveragesView = view.Coverages.Cast<QUO.Coverage>().ToList();
                        List<CompanyRisk> risks = new List<CompanyRisk>();
                        Parallel.ForEach<ISSEN.EndorsementRisk, List<CompanyRisk>>(endorsementRisks,
                          () => { return new List<CompanyRisk>(); },
                          (risk, state, localrisk) =>
                          {
                              var currenRiks = new CompanyRisk
                              {
                                  Id = risk.RiskId,
                                  RiskId = risk.RiskId,
                                  Number = risk.RiskNum,
                                  Status = RiskStatusType.Included
                              };
                              var coverages = from er in endorsementRiskCoverages
                                              join rc in riskCoverages
                                              on er.RiskCoverId equals rc.RiskCoverId
                                              where er.EndorsementId.Equals(risk.EndorsementId)
                                              && er.RiskNum == risk.RiskNum
                                              select rc;
                              currenRiks.Coverages = CoveragesByCoverages(coverages.ToList());
                              ConcurrentBag<String> errors = new ConcurrentBag<String>();
                              Parallel.ForEach(currenRiks.Coverages, (coverage) =>
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
                                              decimal premiumCoverage = subCoverage.PremiumAmount;
                                              int originalDays = Convert.ToInt32((subCoverage.CurrentTo.Value - subCoverage.CurrentFrom.Value).TotalDays);
                                              coverageResult.ContractAmountPercentage += subCoverage.ContractAmountPercentage == null ? 0 : subCoverage.ContractAmountPercentage.Value;
                                              if (policy.CurrentFrom >= subCoverage.CurrentFrom.GetValueOrDefault().AddDays(daysDifference) && policy.CurrentFrom < subCoverage.CurrentTo.GetValueOrDefault().AddDays(daysDifference))
                                              {
                                                  coverageResult.CurrentFrom = policy.CurrentFrom;
                                                  coverageResult.CurrentTo = coverageResult.CurrentFrom.AddDays(modificationDays);
                                              }
                                              else if (policy.CurrentFrom < subCoverage.CurrentFrom.GetValueOrDefault().AddDays(daysDifference))
                                              {
                                                  coverageResult.CurrentFrom = (DateTime)subCoverage.CurrentFrom;
                                                  coverageResult.CurrentTo = subCoverage.CurrentFrom.Value.AddDays(modificationDays);
                                              }
                                              else if (policy.CurrentFrom > (subCoverage.CurrentTo ?? DateTime.MinValue).AddDays(daysDifference))
                                              {
                                                  premiumCoverage = 0;
                                              }
                                              if (premiumCoverage != 0)
                                              {
                                                  lock (lockData)
                                                  {
                                                      premiuntTotal += subCoverage.PremiumAmount;
                                                  }
                                              }
                                              coverageResult.CurrentFromOriginal = (DateTime)subCoverage.CurrentFrom;
                                              coverageResult.CurrentToOriginal = (DateTime)subCoverage.CurrentTo;
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
                                  coverage.ContractAmountPercentage = coverageCurrent.ContractAmountPercentage;
                                  coverageCurrent.CurrentFrom = policy.CurrentFrom > endorsementCoverages?.Where(x => x.RiskCoverId == coverage.RiskCoverageId).Min(x => x.CurrentFrom).GetValueOrDefault() ? policy.CurrentFrom : endorsementCoverages.Where(x => x.RiskCoverId == coverage.RiskCoverageId).Min(x => x.CurrentFrom).GetValueOrDefault();
                                  coverage.CurrentFrom = coverageCurrent.CurrentFrom;
                                  coverage.CurrentTo = coverageCurrent.CurrentTo;
                                  coverage.CurrentFromOriginal = coverageCurrent.CurrentFromOriginal;
                                  coverage.CurrentToOriginal = coverageCurrent.CurrentToOriginal;
                                  coverage.OriginalRate = coverage.Rate;
                                  coverage.CoverageOriginalStatus = coverage.CoverStatus;
                                  coverage.CoverStatus = CoverageStatusType.Included;
                                  coverage.EndorsementType = EndorsementType.ChangeTermEndorsement;

                                  foreach (QUO.Coverage coverageV in coveragesView)
                                  {
                                      if (coverage.Id == coverageV.CoverageId)
                                      {
                                          coverage.IsPrimary = coverageV.IsPrimary;
                                      }
                                  }
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
                //IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();
                //foreach (ISSEN.RiskCoverage riskCoverage in riskCoverages)
                //{
                //    dataFacade.LoadDynamicProperties(riskCoverage);
                //    if (policy.CurrentFrom > riskCoverage.CurrentFrom.Value)
                //    {
                //        int originalDays = Convert.ToInt32((riskCoverage.CurrentTo.Value - riskCoverage.CurrentFrom.Value).TotalDays);
                //        riskCoverage.CurrentFrom = policy.CurrentFrom;
                //        riskCoverage.CurrentTo = riskCoverage.CurrentFrom.Value.AddDays(originalDays);
                //    }
                //    CompanyRisk risk = null;
                //    ISSEN.EndorsementRiskCoverage endorsementRiskCoverage = endorsementRiskCoverages.First(x => x.RiskCoverId == riskCoverage.RiskCoverId);

                //    if (risks.Exists(x => x.Id == endorsementRiskCoverage.RiskNum))
                //    {
                //        risk = risks.First(x => x.Id == endorsementRiskCoverage.RiskNum);
                //        risks.Remove(risk);

                //        if (endorsementRisks.First(x => x.EndorsementId == endorsementRiskCoverage.EndorsementId && x.RiskNum == endorsementRiskCoverage.RiskNum).IsCurrent)
                //        {
                //            risk.RiskId = endorsementRiskCoverage.RiskId;
                //            if (risk.Coverages.Exists(x => x.Id == riskCoverage.CoverageId))
                //            {
                //                risk.Coverages.First(x => x.Id == riskCoverage.CoverageId).DeclaredAmount = riskCoverage.DeclaredAmount;
                //                risk.Coverages.First(x => x.Id == riskCoverage.CoverageId).LimitAmount = riskCoverage.LimitAmount;
                //                risk.Coverages.First(x => x.Id == riskCoverage.CoverageId).SubLimitAmount = riskCoverage.SublimitAmount;
                //                risk.Coverages.First(x => x.Id == riskCoverage.CoverageId).ContractAmountPercentage += riskCoverage.ContractAmountPercentage == null ? 0 : riskCoverage.ContractAmountPercentage.Value;
                //            }
                //        }

                //        if (risk.Coverages.Exists(x => x.Id == riskCoverage.CoverageId))
                //        {
                //            risk.Coverages.First(x => x.Id == riskCoverage.CoverageId).PremiumAmount += riskCoverage.PremiumAmount;
                //        }
                //        else
                //        {
                //            risk.Coverages.Add(ModelAssembler.CreateCoverage(riskCoverage));
                //        }
                //        risk.Status = RiskStatusType.Included;
                //    }
                //    else
                //    {
                //        risk = new CompanyRisk()
                //        {
                //            Id = endorsementRiskCoverage.RiskNum,
                //            RiskId = endorsementRiskCoverage.RiskId,
                //            Status = RiskStatusType.Included,
                //            Coverages = new List<CompanyCoverage>()
                //        };

                //        risk.Coverages.Add(ModelAssembler.CreateCoverage(riskCoverage));
                //    }
                //    risks.Add(risk);
                //}

                //return risks;
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
