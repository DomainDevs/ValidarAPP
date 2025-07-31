using Newtonsoft.Json;
using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Assemblers;
using Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Application.Vehicles.EEProvider.DAOs;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rules = Sistran.Core.Framework.Rules;
using VEBMO = Sistran.Core.Application.Vehicles.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Business
{
    public class VehicleBusiness
    {
        Rules.Facade Facade = new Rules.Facade();
        private int coverageIdAccNoOriginal;
        private int coverageIdAccOriginal;
        private List<int> coverageIdsFromRate;
        private Parameter coverageHeritage;
        private int goodExpNumPrinter;
        private int vehicleReplacement = 0;

        public int CoverageIdAccNoOriginal
        {
            get
            {
                if (coverageIdAccNoOriginal == 0)
                {
                    coverageIdAccNoOriginal = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.NonOriginalAccessories).NumberParameter.Value;
                }

                return coverageIdAccNoOriginal;
            }
        }

        public int CoverageIdAccOriginal
        {
            get
            {
                if (coverageIdAccOriginal == 0)
                {
                    coverageIdAccOriginal = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.OriginalAccessories).NumberParameter.Value;
                }

                return coverageIdAccOriginal;
            }
        }

        public List<int> CoverageIdsFromRate
        {
            get
            {
                if (coverageIdsFromRate == null)
                {
                    Parameter parameter = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.RateAccessories);

                    if (parameter != null && !string.IsNullOrEmpty(parameter.TextParameter))
                    {
                        coverageIdsFromRate = parameter.TextParameter.Split('|').Select(x => int.Parse(x)).ToList();
                    }
                    else
                    {
                        coverageIdsFromRate = new List<int>();
                    }
                }

                return coverageIdsFromRate;
            }
        }

        public Parameter CoverageHeritage
        {
            get
            {
                if (coverageHeritage == null)
                {
                    coverageHeritage = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.CoverageHeritage);
                }

                return coverageHeritage;
            }
        }

        public CompanyVehicle RunRulesRisk(CompanyVehicle companyVehicle, int ruleId)
        {
            if (!companyVehicle.Risk.Policy.IsPersisted)
            {
                companyVehicle.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyVehicle.Risk.Policy.Id, false);
            }

            return RunRules(companyVehicle, ruleId);
        }

        private CompanyVehicle RunRules(CompanyVehicle companyVehicle, int ruleId)
        {
            UnderwritingServices.Assembler.ModelAssembler.CreateFacadeGeneral(companyVehicle.Risk.Policy, Facade);
            EntityAssembler.CreateFacadeRiskVehicle(Facade, companyVehicle);

            Facade = RulesEngineDelegate.ExecuteRules(ruleId, Facade);

            ModelAssembler.CreateVehicle(companyVehicle, Facade);

            if (Facade.GetConcept<int?>(CompanyRuleConceptRisk.LimitsRcCodePreview) != null &&
                Facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCodePreview) != Facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCode))
            {
                companyVehicle.Risk.LimitRc = DelegateService.underwritingService.GetCompanyLimitRcById(Facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCode));
            }

            if (Facade.GetConcept<int?>(CompanyRuleConceptRisk.CoverageGroupIdPreview) != null &&
                Facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageGroupIdPreview) != Facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageGroupId))
            {
                companyVehicle.Risk.GroupCoverage = DelegateService.underwritingService.GetProductCoverageGroupRiskByProductId(companyVehicle.Risk.Policy.Product.Id)
                    .First(x => x.Id == Facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageGroupId));

                companyVehicle.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(companyVehicle.Risk.Policy.Product.Id,
                    companyVehicle.Risk.GroupCoverage.Id, companyVehicle.Risk.Policy.Prefix.Id).Where(x => x.IsSelected).ToList();
            }

            return companyVehicle;
        }

        private CompanyVehicle GetCompanyVehiclePersisted(CompanyVehicle companyVehicle)
        {
            CompanyPolicy companyPolicy = companyVehicle.Risk.Policy;
            if (!companyVehicle.Risk.IsPersisted)
            {
                PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(companyVehicle.Risk.Id);

                if (pendingOperation != null)
                {
                    companyVehicle = JsonConvert.DeserializeObject<Models.CompanyVehicle>(pendingOperation.Operation);
                }
                else
                {
                    throw new ValidationException(Errors.ValidationRiskNotFound);
                }
            }

            companyVehicle.Risk.Policy = GetCompanyPolicyPersisted(companyPolicy);

            return companyVehicle;
        }


        private CompanyPolicy GetCompanyPolicyPersisted(CompanyPolicy companyPolicy)
        {
            if (!companyPolicy.IsPersisted)
            {
                PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(companyPolicy.Id);

                if (pendingOperation != null)
                {
                    companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                }
                else
                {
                    throw new ValidationException(Errors.ValidationTemporalNotFound);
                }
            }

            return companyPolicy;
        }


        private CompanyVehicle SetClauses(CompanyVehicle companyVehicle)
        {
            DAOs.VehicleDAO companyVehicleDAO = new DAOs.VehicleDAO();
            companyVehicle.Risk.Clauses = companyVehicle.Risk.Clauses ?? new List<CompanyClause>();
            companyVehicle.Risk.Policy.Clauses = companyVehicle.Risk.Policy.Clauses ?? new List<CompanyClause>();
            companyVehicleDAO.AddBeneficiariesClauses(companyVehicle.Risk.Clauses);
            companyVehicle.Risk.Policy = GetCompanyPolicyPersisted(companyVehicle.Risk.Policy);
            return companyVehicle;
        }


        public CompanyVehicle QuotateVehicle(CompanyVehicle companyVehicle, bool runRulesPre, bool runRulesPost, int? appVersion, bool isEndorsement = false)
        {
            bool updatePolicy = false;
            bool RuleSet = true;

            if (companyVehicle.Risk == null)
            {
                throw new ValidationException(Errors.ErrorRiskEmpty);
            }

            if (companyVehicle.Risk.Policy == null)
            {
                throw new ValidationException(Errors.ErrorPolicyEmpty);
            }

            if (!companyVehicle.Risk.Policy.IsPersisted)
            {
                companyVehicle.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyVehicle.Risk.Policy.Id, false);
                updatePolicy = true;
            }

            if (companyVehicle.Risk.LimitRc.LimitSum == 0)
            {
                companyVehicle.Risk.LimitRc = DelegateService.underwritingService.GetCompanyLimitRcById(companyVehicle.Risk.LimitRc.Id);
            }

            if (companyVehicle.Version.Engine == null)
            {
                VehicleDAO vehicleDao = new VehicleDAO();
                var imapper = ModelAssembler.CreateMapCompanyVersion();
                companyVehicle.Version = imapper.Map<VEBMO.Version, CompanyVersion>(vehicleDao.GetVersionByVersionIdModelIdMakeId(companyVehicle.Version.Id, companyVehicle.Model.Id, companyVehicle.Make.Id));
            }

            if (companyVehicle.Risk.Policy.PolicyOrigin == PolicyOrigin.Collective)
            {
                if (companyVehicle.Risk.Policy.Endorsement.EndorsementType == EndorsementType.Emission || companyVehicle.Risk.Policy.Endorsement.EndorsementType == EndorsementType.Renewal)
                {
                    companyVehicle.Risk.Status = RiskStatusType.Original;
                }
                else if (companyVehicle.Risk.Policy.Endorsement.EndorsementType == EndorsementType.Modification && companyVehicle.Risk.Status != RiskStatusType.Excluded && companyVehicle.Risk.Status != RiskStatusType.Modified && companyVehicle.Risk.RiskId == 0)
                {
                    companyVehicle.Risk.OriginalStatus = RiskStatusType.Included;
                    companyVehicle.Risk.Status = RiskStatusType.Included;
                }
                else if (companyVehicle.Risk.Status != RiskStatusType.Excluded)
                {
                    VehicleDAO vehicleDao = new VehicleDAO();
                    var imapper = ModelAssembler.CreateMapCompanyVersion();
                    companyVehicle.Version = imapper.Map<VEBMO.Version, Vehicles.Models.CompanyVersion>(vehicleDao.GetVersionByVersionIdModelIdMakeId(companyVehicle.Version.Id, companyVehicle.Model.Id, companyVehicle.Make.Id));
                }
            }

            if (companyVehicle.Accesories != null && companyVehicle.Accesories.Count > 0)
            {
                List<CompanyCoverage> coveragesAccessories = new List<CompanyCoverage>();
                coveragesAccessories = DelegateService.underwritingService.GetCompanyCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(companyVehicle.Risk.Policy.Product.Id, companyVehicle.Risk.GroupCoverage.Id, companyVehicle.Risk.Policy.Prefix.Id);
                var Companycoverages = coveragesAccessories.Where(x => !companyVehicle.Risk.Coverages.Select(z => z.Id).Contains(x.Id));
                if (Companycoverages != null && Companycoverages.Count() > 0)
                {
                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    Companycoverages.AsParallel().ForAll(item =>
                    {
                        try
                        {
                            item.CurrentFrom = companyVehicle.Risk.Policy.CurrentFrom;
                            item.CurrentTo = companyVehicle.Risk.Policy.CurrentTo;
                            item.EndorsementType = companyVehicle.Risk.Policy.Endorsement.EndorsementType;
                            if (companyVehicle.Risk.Policy?.Endorsement?.EndorsementType == EndorsementType.Emission)
                            {
                                item.CoverStatus = CoverageStatusType.Original;
                            }
                            if (companyVehicle.Risk.Coverages.Exists(x => x.Id == item.Id))
                            {
                                if (item.CoverStatus == CoverageStatusType.Included)
                                {
                                    item.CoverStatus = CoverageStatusType.NotModified;
                                }
                            }
                            else
                            {
                                item.CoverStatus = CoverageStatusType.Included;
                            }
                        }
                        catch (Exception ex)
                        {

                            errors.Add(ex.Message);
                        }

                    });
                    companyVehicle.Risk.Coverages = companyVehicle.Risk.Coverages?.Where(c => (!coveragesAccessories.Any(x => x.Id == c.Id))).ToList();
                    companyVehicle.Risk.Coverages.AddRange(coveragesAccessories);
                }
            }

            if (companyVehicle.Risk.Policy.PolicyOrigin == PolicyOrigin.Collective)
            {
                companyVehicle.Risk.Coverages.AsParallel().ForAll(z =>
                {
                    z.CurrentFrom = companyVehicle.Risk.Policy.CurrentFrom;
                    z.CurrentTo = companyVehicle.Risk.Policy.CurrentTo;
                    if (z.CoverStatus == null)
                    {
                        z.CoverStatus = CoverageStatusType.Original;
                    }
                });
            }
            companyVehicle.Risk.Coverages.AsParallel().ForAll(z =>
            {
                z.EndorsementType = companyVehicle.Risk.Policy.Endorsement.EndorsementType;
            });

            if (companyVehicle.Risk.Status == RiskStatusType.Excluded)
            {
                companyVehicle.Risk.Coverages = companyVehicle.Risk.Coverages.Where(x => x.CoverStatus != CoverageStatusType.Included).ToList();
                companyVehicle.Risk.Coverages.ForEach(x => x.CoverStatus = CoverageStatusType.Excluded);
                companyVehicle.Rate = companyVehicle.Rate * -1;
            }

            CompanyCoverage coverageAccessoryNoOriginal = companyVehicle.Risk.Coverages.FirstOrDefault(x => x.Id == CoverageIdAccNoOriginal);
            CompanyCoverage coverageAccessoryOriginal = companyVehicle.Risk.Coverages.FirstOrDefault(x => x.Id == CoverageIdAccOriginal);

            if (coverageAccessoryNoOriginal != null)
            {
                companyVehicle.Risk.Coverages.Remove(coverageAccessoryNoOriginal);
            }

            if (coverageAccessoryOriginal != null)
            {
                companyVehicle.Risk.Coverages.Remove(coverageAccessoryOriginal);
            }

            if (runRulesPost && companyVehicle.Risk.Policy.Product.CoveredRisk.RuleSetId.GetValueOrDefault() > 0)
            {
                companyVehicle = RunRules(companyVehicle, companyVehicle.Risk.Policy.Product.CoveredRisk.RuleSetId.Value);
            }

            companyVehicle.Risk.Premium = 0;
            companyVehicle.Risk.AmountInsured = 0;
            List<CompanyCoverage> quotateCoverages = new List<CompanyCoverage>();

            //vehiculode remplazo
            if (companyVehicle?.Risk?.Policy?.Product?.CoveredRisk?.ScriptId != null)
            {
                if (GetVehicleReplacement != -1)
                {
                    if (companyVehicle.Risk.Coverages.FirstOrDefault(x => x.Id == vehicleReplacement) == null)
                    {
                        var coverageVehicleReplacement = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(vehicleReplacement, companyVehicle.Risk.Policy.Product.Id, companyVehicle.Risk.GroupCoverage.Id);
                        if (coverageVehicleReplacement != null)
                        {

                            if (coverageVehicleReplacement.RuleSetId == null && coverageVehicleReplacement.PosRuleSetId == null)
                            {
                                RuleSet = false;
                            }

                            if (RuleSet)
                            {
                                coverageVehicleReplacement.CurrentFrom = companyVehicle.Risk.Policy.CurrentFrom;
                                coverageVehicleReplacement.CurrentTo = companyVehicle.Risk.Policy.CurrentTo;
                                coverageVehicleReplacement.EndorsementType = companyVehicle.Risk.Policy.Endorsement.EndorsementType;
                                if (companyVehicle.Risk.Policy?.Endorsement?.EndorsementType == EndorsementType.Emission)
                                {
                                    coverageVehicleReplacement.CoverStatus = CoverageStatusType.Original;
                                }
                                if (companyVehicle.Risk.Coverages.Exists(x => x.Id == coverageVehicleReplacement.Id))
                                {
                                    if (coverageVehicleReplacement.CoverStatus == CoverageStatusType.Included)
                                    {
                                        coverageVehicleReplacement.CoverStatus = CoverageStatusType.NotModified;
                                    }
                                }
                                else
                                {
                                    coverageVehicleReplacement.CoverStatus = CoverageStatusType.Included;
                                }
                                companyVehicle.Risk.Coverages.Add(coverageVehicleReplacement);
                            }
                        }
                    }
                }
            }

            if (companyVehicle.Risk?.DynamicProperties?.Count > 0)
            {
                ConcurrentBag<string> log = new ConcurrentBag<string>();
                var totalDinamicConcept = companyVehicle.Risk.Coverages.Where(x => x.DynamicProperties?.Count() > 0).Count();
                var coverageDinamicConcepts = companyVehicle.Risk.Coverages.Where(x => x.DynamicProperties?.Count() > 0).ToList();
                TP.Parallel.For(0, totalDinamicConcept, z =>
                      {
                          try
                          {

                              var coverage = coverageDinamicConcepts[z];
                              var coverageDinamics = coverage.DynamicProperties.Where(r => companyVehicle.Risk.DynamicProperties.Select(u => new { EntityId = u.EntityId, Id = u.Id }).Contains(new { r.EntityId, r.Id })).ToList();
                              TP.Parallel.For(0, coverageDinamics.Count(), n =>
                              {
                                  try
                                  {
                                      var dataDinamic = companyVehicle.Risk.DynamicProperties.FirstOrDefault(b => b.EntityId == coverageDinamics[n].EntityId && b.Id == coverageDinamics[n].Id);
                                      if (dataDinamic != null)
                                      {
                                          coverageDinamics[n].Value = dataDinamic.Value;
                                      }
                                  }
                                  catch (Exception ex)
                                  {

                                      log.Add(ex.Message);
                                  }


                              });
                          }
                          catch (Exception ex)
                          {

                              log.Add(ex.Message);
                          }

                      });
                //var coveragedinamic = companyVehicle.Risk.Coverages.SelectMany(x => x.DynamicProperties).Where(z => companyVehicle.Risk.DynamicProperties.Select(x => new { EntityId = x.EntityId, Id = x.Id }).Contains(new { z.EntityId, z.Id })).ToList();
            }
            foreach (CompanyCoverage coverage in companyVehicle.Risk.Coverages)
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                quotateCoverages.Add(coverageBusiness.Quotate(companyVehicle, coverage, runRulesPre, runRulesPost));
                coverageBusiness.UpdateFacadeConcepts(Facade);
            }

            companyVehicle.Risk.Coverages = quotateCoverages;

            companyVehicle?.Risk?.Coverages?.AsParallel().ForAll(x =>
            {
                if (x.CoverStatus.HasValue)
                {
                    if (Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(x?.CoverStatus.Value)) == null)
                    {

                        x.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(x.CoverStatus.Value);
                    }
                    else
                    {
                        x.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(x.CoverStatus.Value));
                    }
                }
            });

            if (companyVehicle.Risk.Policy.Product.IsFlatRate && companyVehicle.Rate > 0)
            {
                decimal differenceRate = companyVehicle.Rate - companyVehicle.Risk.Coverages.Where(z => z.RateType == RateType.Percentage && z.Rate != null).Sum(x => x.Rate.Value);

                if (differenceRate != 0)
                {
                    if (this.Truncate(differenceRate,4) != 0)
                    {
                        CompanyCoverage lastCoverage = companyVehicle.Risk.Coverages.FirstOrDefault(x => x.Id == Facade.GetConcept<int>(RuleConceptRisk.CoverageIdLast));

                        if (lastCoverage != null)
                        {
                            lastCoverage.Rate += differenceRate;
                            lastCoverage.FlatRatePorcentage += 100 - companyVehicle.Risk.Coverages.Where(x => x != null && x.RateType == RateType.Percentage && x.Rate != null && x.PremiumAmount != 0 && x.Rate != 0).Sum(x => x.FlatRatePorcentage);

                            CoverageBusiness coverageBusiness = new CoverageBusiness();
                            lastCoverage = coverageBusiness.Quotate(companyVehicle, lastCoverage, runRulesPre, runRulesPost);
                            if (companyVehicle.Risk.Coverages.FirstOrDefault(x => x.Id == lastCoverage.Id) != null)
                            {
                                companyVehicle.Risk.Coverages.First(x => x.Id == lastCoverage.Id).PremiumAmount = lastCoverage.PremiumAmount;
                                companyVehicle.Risk.Coverages.First(x => x.Id == lastCoverage.Id).Rate = lastCoverage.Rate;
                                companyVehicle.Risk.Coverages.First(x => x.Id == lastCoverage.Id).SubLimitAmount = lastCoverage.SubLimitAmount;
                                companyVehicle.Risk.Coverages.First(x => x.Id == lastCoverage.Id).RateType = lastCoverage.RateType;
                            }
                        }
                    }
                }
            }

            //Eliminar Clausulas Poliza
            companyVehicle.Risk.Policy.Clauses = DelegateService.underwritingService.RemoveClauses(companyVehicle.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesRemove));

            //Agregar Clausulas Poliza
            companyVehicle.Risk.Policy.Clauses = DelegateService.underwritingService.AddClauses(companyVehicle.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesAdd));

            //Eliminar Clausulas Riesgo
            companyVehicle.Risk.Clauses = DelegateService.underwritingService.RemoveClauses(companyVehicle.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesRemove));

            //Agregar Clausulas Riesgo
            companyVehicle.Risk.Clauses = DelegateService.underwritingService.AddClauses(companyVehicle.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesAdd));

            var coverageRemove = Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesRemove);

            //Eliminar Coberturas
            if (coverageRemove?.Count > 0)
            {
                companyVehicle.Risk.Coverages.RemoveAll(x => coverageRemove.Contains(x.Id));
            }
            //Agregar Coberturas
            if (Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd)?.Count() > 0)
            {
                foreach (int coverageId in Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd))
                {

                    if (!companyVehicle.Risk.Coverages.Exists(x => x.Id == coverageId))
                    {
                        CompanyCoverage quotateCoverage = new CompanyCoverage();
                        quotateCoverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, companyVehicle.Risk.Policy.Product.Id, companyVehicle.Risk.GroupCoverage.Id);
                        quotateCoverage.CurrentFrom = companyVehicle.Risk.Policy.CurrentFrom;
                        quotateCoverage.CurrentTo = companyVehicle.Risk.Policy.CurrentTo;
                        quotateCoverage.EndorsementType = companyVehicle.Risk.Policy.Endorsement.EndorsementType;
                        if (companyVehicle.Risk.Policy?.Endorsement?.EndorsementType == EndorsementType.Emission)
                        {
                            quotateCoverage.CoverStatus = CoverageStatusType.Original;
                        }
                        if (companyVehicle.Risk.Coverages.Exists(x => x.Id == quotateCoverage.Id))
                        {
                            if (quotateCoverage.CoverStatus == CoverageStatusType.Included)
                            {
                                quotateCoverage.CoverStatus = CoverageStatusType.NotModified;
                            }
                        }
                        else
                        {
                            quotateCoverage.CoverStatus = CoverageStatusType.Included;
                        }
                        CoverageBusiness coverageBusiness = new CoverageBusiness();
                        companyVehicle.Risk.Coverages.Add(coverageBusiness.Quotate(companyVehicle, quotateCoverage, true, true));
                        coverageBusiness.UpdateFacadeConcepts(Facade);
                    }
                }
            }
            //Accesorios No Originales

            if (coverageAccessoryNoOriginal != null && companyVehicle.Accesories != null && companyVehicle.Accesories.Where(x => !x.IsOriginal)?.Count() > 0)
            {
                int day = QuoteManager.CalculateEffectiveDays(coverageAccessoryNoOriginal.CurrentFrom, coverageAccessoryNoOriginal.CurrentTo);
                var rate = companyVehicle.Risk.Coverages.Where(x => CoverageIdsFromRate.Contains(x.Id)).Sum(x => x.Rate.GetValueOrDefault());
                decimal totalAmount = 0;
                coverageAccessoryNoOriginal.EndorsementType = companyVehicle.Risk.Policy.Endorsement.EndorsementType;
                coverageAccessoryNoOriginal.RateType = RateType.Percentage;
                totalAmount = companyVehicle.Accesories.Where(x => !x.IsOriginal).Sum(x => Math.Abs(x.Amount)) + companyVehicle.Accesories.Where(x => !x.IsOriginal && x.Status == (int)CoverageStatusType.Excluded).Sum(x => x.Amount);

                coverageAccessoryNoOriginal.DeclaredAmount = totalAmount;
                coverageAccessoryNoOriginal.LimitAmount = totalAmount;
                coverageAccessoryNoOriginal.SubLimitAmount = totalAmount;
                coverageAccessoryNoOriginal.LimitOccurrenceAmount = totalAmount;
                coverageAccessoryNoOriginal.LimitClaimantAmount = totalAmount;
                coverageAccessoryNoOriginal.EndorsementLimitAmount = totalAmount;
                coverageAccessoryNoOriginal.EndorsementSublimitAmount = totalAmount;
                coverageAccessoryNoOriginal.CurrentFrom = companyVehicle.Risk.Policy.CurrentFrom;
                coverageAccessoryNoOriginal.CurrentTo = companyVehicle.Risk.Policy.CurrentTo;

                if (companyVehicle.Rate > 0)
                {
                    coverageAccessoryNoOriginal.Rate = companyVehicle.Rate;
                }
                else
                {
                    if (!companyVehicle.CalculateMinPremium)
                        coverageAccessoryNoOriginal.Rate = rate;
                }
                CoverageBusiness businessCoverage = new CoverageBusiness();
                coverageAccessoryNoOriginal = businessCoverage.Quotate(companyVehicle, coverageAccessoryNoOriginal, true, true);
                if (coverageAccessoryNoOriginal?.Rate > 0 && coverageAccessoryNoOriginal.RateType == RateType.Percentage)
                {
                    foreach (Accessory accessory in companyVehicle.Accesories.Where(x => !x.IsOriginal))
                    {
                        accessory.Rate = coverageAccessoryNoOriginal.Rate.Value;
                        accessory.RateType = coverageAccessoryNoOriginal.RateType;
                        if (companyVehicle.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Emission && companyVehicle.Risk.Policy.Endorsement.EndorsementType != EndorsementType.EffectiveExtension && companyVehicle.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Renewal)
                        {
                            if (accessory.Amount != 0 && coverageAccessoryNoOriginal.LimitAmount > 0 && accessory.Status != (int)CoverageStatusType.Excluded)
                            {
                                decimal premiumbase = accessory.Amount * CalculateCoeficientRate(accessory.Rate, accessory.RateType) / QuoteManager.AnnualDays * day;
                                accessory.Premium = decimal.Round(premiumbase - accessory.AccumulatedPremium, QuoteManager.DecimalRound);
                                if (accessory.Premium != 0)
                                    accessory.Status = (int)CoverageStatusType.Modified;
                            }
                            else if (accessory.Status == (int)CoverageStatusType.Excluded)
                            {
                                accessory.Premium = decimal.Round(Math.Abs(accessory.Amount * CalculateCoeficientRate(accessory.Rate, accessory.RateType) / QuoteManager.AnnualDays * day) * -1, QuoteManager.DecimalRound);
                            }
                        }
                        else
                        {
                            if (accessory.Amount != 0 && coverageAccessoryNoOriginal.LimitAmount > 0 && accessory.Status != (int)CoverageStatusType.Excluded)
                            {
                                accessory.Premium = decimal.Round(accessory.Amount * CalculateCoeficientRate(accessory.Rate, accessory.RateType) / QuoteManager.AnnualDays * day, QuoteManager.DecimalRound);
                                accessory.AccumulatedPremium = 0;
                                accessory.Status = (int)CoverageStatusType.Original;
                            }
                        }
                    }
                }
                companyVehicle.Risk.Coverages.Add(coverageAccessoryNoOriginal);
            }

            //Accesorios Originales
            if (coverageAccessoryOriginal != null && companyVehicle.Accesories != null && companyVehicle.Accesories.Count > 0)
            {
                coverageAccessoryOriginal.Rate = 0;
                coverageAccessoryOriginal.RateType = RateType.Percentage;
                coverageAccessoryOriginal.CurrentFrom = companyVehicle.Risk.Policy.CurrentFrom;
                coverageAccessoryOriginal.CurrentTo = companyVehicle.Risk.Policy.CurrentTo;

                if (companyVehicle.Accesories.Exists(x => x.IsOriginal))
                {
                    foreach (Accessory accessory in companyVehicle.Accesories.Where(x => x.IsOriginal))
                    {
                        accessory.Rate = coverageAccessoryNoOriginal.Rate.Value;
                        accessory.RateType = coverageAccessoryNoOriginal.RateType;
                    }
                }

                companyVehicle.Risk.Coverages.Add(coverageAccessoryOriginal);
            }

            //Proteccion Patrimonial
            if (CoverageHeritage != null && companyVehicle.Rate == decimal.Zero && companyVehicle.Risk.Coverages.Exists(x => x.Id == CoverageHeritage.NumberParameter))
            {
                string[] coverages = null;
                if (CoverageHeritage.TextParameter?.Split('|')?.Count() > 0)
                {
                    coverages = new string[3];
                    coverages = CoverageHeritage.TextParameter.Split('|');
                }
                if (coverages?.Length > 0)
                {
                    var coveragePmd = coverages[0] == null ? -1 : Convert.ToInt32(coverages[0]);
                    var coveragePsd = coverages[1] == null ? -1 : Convert.ToInt32(coverages[1]);
                    var coverageRce = coverages[2] == null ? -1 : Convert.ToInt32(coverages[2]);
                    CompanyCoverage coveragePatrimonial = companyVehicle.Risk.Coverages.First(x => x.Id == CoverageHeritage.NumberParameter);
                    if (companyVehicle.Risk.Coverages.FirstOrDefault(x => x.Id == coveragePmd)?.SubLimitAmount > 0 ||
                        companyVehicle.Risk.Coverages.FirstOrDefault(x => x.Id == coveragePsd)?.SubLimitAmount > 0 ||
                        companyVehicle.Risk.Coverages.FirstOrDefault(x => x.Id == coverageRce)?.SubLimitAmount > 0)
                    {
                        decimal premiumPatrimonial = companyVehicle.Risk.Coverages.Where(x => coverages.Where(y => !string.IsNullOrEmpty(y)).Select(z => Convert.ToInt32(z)).Contains(x.Id)).Sum(x => x.PremiumAmount);
                        premiumPatrimonial = (premiumPatrimonial * CoverageHeritage.PercentageParameter.GetValueOrDefault()) / 100;

                        coveragePatrimonial.PremiumAmount = decimal.Round(premiumPatrimonial, QuoteManager.DecimalRound);
                        coveragePatrimonial.Rate = decimal.Round(premiumPatrimonial, QuoteManager.PremiumRoundValue);
                        coveragePatrimonial.RateType = RateType.FixedValue;
                    }
                }
                else
                {
                    throw new Exception(Errors.ValidationCoveragePatrimonial);
                }
            }

            //Deducibles
            if (companyVehicle.Risk.Coverages.Where(x => x.Deductible != null && x.Deductible.Id != -1 && x.Deductible.Id != 0)?.Count() > 0)
                companyVehicle.Risk.Coverages = DelegateService.underwritingService.GetDeductiblesByCompanyCoverages(companyVehicle.Risk.Coverages);

            foreach (CompanyCoverage companyCoverage in companyVehicle.Risk.Coverages)
            {
                if (companyCoverage.Deductible != null)
                {
                    DelegateService.underwritingService.CalculateCompanyPremiumDeductible(companyCoverage);
                }
            }
            if (companyVehicle?.Risk?.Policy?.Endorsement?.EndorsementType == EndorsementType.Emission || companyVehicle.Risk.Policy.Endorsement.EndorsementType == EndorsementType.Renewal)
            {
                VehicleMinimumPremium vehicleMinimumBonus = new VehicleMinimumPremium(CoverageHeritage);
                companyVehicle = vehicleMinimumBonus.CalculateVehicleMinimumPremium(companyVehicle);
            }
            else if (companyVehicle?.Risk?.Policy?.Endorsement?.EndorsementType == EndorsementType.Modification)
            {
                if (companyVehicle?.Risk?.Status == RiskStatusType.Excluded || companyVehicle?.Risk?.Status == RiskStatusType.Included)
                {
                    VehicleMinimumPremium vehicleMinimumBonus = new VehicleMinimumPremium(CoverageHeritage);
                    companyVehicle = vehicleMinimumBonus.CalculateVehicleMinimumPremiumModification(companyVehicle);
                }
            }
            else if (companyVehicle?.Risk?.Policy?.Endorsement?.EndorsementType == EndorsementType.ProrrateRehabilitation)
            {

            }

            companyVehicle.Risk.Premium = companyVehicle.Risk.Coverages.Sum(x => x.PremiumAmount);
            companyVehicle.Risk.AmountInsured = companyVehicle.Risk.Coverages.Sum(x => x.LimitAmount);

            if (updatePolicy)
            {
                DelegateService.underwritingService.CreatePolicyTemporal(companyVehicle.Risk.Policy, false);
            }

            return companyVehicle;
        }

        private decimal Truncate(decimal valor, int precision)
        {
            decimal valortrunc = valor;
            try
            {
                decimal step = (decimal)Math.Pow(10, precision);
                decimal tmp = Math.Truncate(step * valortrunc);
                valortrunc= tmp / step;
            }
            catch(Exception ex)
            {
                return valor;
            }
            return valortrunc;
        }

        private CompanyCoverage CalculateAccesory(List<CompanyAccessory> Accesories, CompanyCoverage coverageAccessoryNoOriginal, decimal rate, EndorsementType? endorsementType, bool CalculateMinPremium, decimal flateRate, int day)
        {
            return coverageAccessoryNoOriginal;

        }
        private void CalculateCoveragePatrimonial(List<CompanyCoverage> listCompanyCoverage, CompanyCoverage lastCoverage, decimal CoveragePatProtecAmount)
        {
            decimal diffPatProtect = 0;
            foreach (CompanyCoverage coverage in listCompanyCoverage)
            {
                if (coverage.Id == CoverageHeritage.NumberParameter && CoveragePatProtecAmount != 0)
                {
                    diffPatProtect = decimal.Round(coverage.PremiumAmount - CoveragePatProtecAmount, 2);
                }

                if (lastCoverage.Id == coverage.Id)
                {
                    coverage.PremiumAmount = coverage.PremiumAmount - diffPatProtect;

                    if (coverage.RateType == RateType.FixedValue)
                    {
                        coverage.Rate = coverage.PremiumAmount;
                    }
                    else
                    {
                        coverage.Rate = decimal.Round(coverage.PremiumAmount * 100 / ((coverage.DeclaredAmount == 0) ? coverage.SubLimitAmount : coverage.DeclaredAmount), QuoteManager.PremiumRoundValue);
                    }

                    //Reverso
                    decimal diffFinal = 0;
                    ///////////////////////////
                    decimal premiumAmtFinal = decimal.Round((decimal)coverage.Rate * coverage.DeclaredAmount / 100, 2);
                    diffFinal = decimal.Round(premiumAmtFinal - coverage.PremiumAmount, 2);
                    //trc.PremiumAmount = premiumAmtFinal;
                    if (diffFinal > 0)
                    {
                        string rateString = Convert.ToString(coverage.Rate);
                        int lastDecimalRate = 0;// Convert.ToInt32(Convert.ToString(rateString[rateString.Length - 1]));
                        string lastDecimalRateTmp = Convert.ToString(rateString[rateString.Length - 1]);
                        int i = 1;
                        string number = "";
                        while (lastDecimalRateTmp != "," && lastDecimalRateTmp != ".")
                        {
                            if (lastDecimalRateTmp == "," || lastDecimalRateTmp == ".")
                            {
                                break;
                            }
                            lastDecimalRate = Convert.ToInt32(Convert.ToString(rateString[rateString.Length - i]));
                            if (lastDecimalRate > 0)
                            {
                                number = Convert.ToString(lastDecimalRate) + number;
                                break;
                            }
                            else
                            {
                                number += Convert.ToString(lastDecimalRate);
                            }
                            i++;
                            lastDecimalRateTmp = Convert.ToString(rateString[rateString.Length - i]);
                        }
                        lastDecimalRate = Convert.ToInt32(number);
                        if (lastDecimalRate > 0)
                            lastDecimalRate--;
                        rateString = rateString.Remove(rateString.Length - number.Length);
                        rateString += lastDecimalRate;
                        coverage.Rate = Convert.ToDecimal(rateString);
                    }
                }
            }
        }


        public List<CompanyVehicle> QuotateVehicles(CompanyPolicy companyPolicy, List<CompanyVehicle> companyVehicles, bool runRulesPre, bool runRulesPost, bool isEndorsement = false)
        {
            if (!companyPolicy.IsPersisted)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Id, false);
            }
            companyVehicles.AsParallel().ForAll(x => { x.Risk.Policy = companyPolicy; });
            Parallel.ForEach(companyVehicles, ParallelHelper.DebugParallelFor(), companyVehicle =>
            {
                QuotateVehicle(companyVehicle, runRulesPre, runRulesPost, companyPolicy.Endorsement.AppRelation);
                DataFacadeManager.Dispose();
            });

            return companyVehicles;
        }


        public int GetGoodExpNumPrinter
        {
            get
            {
                if (goodExpNumPrinter == 0)
                {
                    CompanyParameter parameter = DelegateService.commonService.FindCoParameter((int)CommonServices.Enums.CompanyParameterType.GoodExpNumPrinter);
                    if (parameter != null && parameter.NumberParameter.HasValue)
                    {
                        goodExpNumPrinter = parameter.NumberParameter.Value;
                    }
                    else
                    {
                        //Se genera una excepción, porque este parámetro es requerido
                        throw new ValidationException(String.Format("Parámetro {0} No encontrado en el sistema", (int)CompanyParameterType.GoodExpNumPrinter));
                    }
                }
                return goodExpNumPrinter;
            }
        }
        public int GetVehicleReplacement
        {
            get
            {
                if (vehicleReplacement == 0 && vehicleReplacement != -1)
                {
                    CompanyParameter parameter = DelegateService.commonService.FindCoParameter((int)CommonServices.Enums.CompanyParameterType.VehicleReplacement);
                    if (parameter != null && parameter.NumberParameter.HasValue)
                    {
                        vehicleReplacement = parameter.NumberParameter.Value;
                    }
                    else
                    {
                        vehicleReplacement = -1;
                    }
                }
                return vehicleReplacement;
            }
        }


        #region Previsora


        public Models.CompanyVehicle RunRulesRisk(Models.CompanyVehicle companyVehicle, int ruleSetId, Rules.Facade facade)
        {
            if (facade == null)
            {
                companyVehicle = GetCompanyVehiclePersisted(companyVehicle);
                facade = new Rules.Facade();
                facade = DelegateService.underwritingService.CreateFacadeGeneral(companyVehicle.Risk.Policy);
            }

            EntityAssembler.CreateFacadeRiskVehicle(facade, companyVehicle);
            facade = RulesEngineDelegate.ExecuteRules(ruleSetId, facade);

            return ModelAssembler.CreateVehicle(companyVehicle, facade);
        }

        private static CompanyVehicle BuildAccesories(CompanyVehicle companyVehicle)
        {
            List<CompanyCoverage> coveragesAccessories = new List<CompanyCoverage>();
            coveragesAccessories = DelegateService.underwritingService.GetCompanyCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(companyVehicle.Risk.Policy.Product.Id, companyVehicle.Risk.GroupCoverage.Id, companyVehicle.Risk.Policy.Prefix.Id);
            var Companycoverages = coveragesAccessories.Where(x => !companyVehicle.Risk.Coverages.Select(z => z.Id).Contains(x.Id));
            if (Companycoverages != null && Companycoverages.Count() > 0)
            {
                Companycoverages.AsParallel().ForAll(item =>
                {
                    item.CurrentFrom = companyVehicle.Risk.Policy.CurrentFrom;
                    item.CurrentTo = companyVehicle.Risk.Policy.CurrentTo;
                    item.EndorsementType = companyVehicle.Risk.Policy.Endorsement.EndorsementType;
                    if (companyVehicle.Risk.Policy?.Endorsement?.EndorsementType == EndorsementType.Emission)
                    {
                        item.CoverStatus = CoverageStatusType.Original;
                    }
                    if (companyVehicle.Risk.Coverages.Exists(x => x.Id == item.Id))
                    {
                        if (item.CoverStatus == CoverageStatusType.Included)
                        {
                            item.CoverStatus = CoverageStatusType.NotModified;
                        }
                    }
                    else
                    {
                        item.CoverStatus = CoverageStatusType.Included;
                    }
                });
                companyVehicle.Risk.Coverages = companyVehicle.Risk.Coverages?.Where(c => (!coveragesAccessories.Any(x => x.Id == c.Id))).ToList();
                companyVehicle.Risk.Coverages.AddRange(coveragesAccessories);
            }
            return companyVehicle;
        }

        #endregion

        #region ajustar
        /// <summary>
        /// Calcular Tasa
        /// </summary>
        /// <param name="rate">Tasa</param>
        /// <param name="rateType">Tipo de Tasa</param>
        /// <returns>Tasa</returns>
        private decimal CalculateCoeficientRate(decimal rate, RateType? rateType)
        {
            decimal coeficientPremiumRate = 0;

            if (rate > 0)
            {
                decimal factor = Convert.ToDecimal(GetFactor(rateType));

                if (rateType != RateType.FixedValue)
                {
                    coeficientPremiumRate = rate * factor;
                }
                else
                {
                    coeficientPremiumRate = 1;
                }
            }

            return coeficientPremiumRate;
        }
        /// <summary>
        /// Obtener Factor por Tipo de Tasa
        /// </summary>
        /// <param name="rateType">Tipo de Tasa</param>
        /// <returns>Factor</returns>
        private decimal GetFactor(RateType? rateType)
        {
            decimal numerator = 1;
            decimal divider = 1;

            switch (rateType)
            {
                case RateType.FixedValue:
                    divider = 1;
                    break;
                case RateType.Percentage:
                    divider = 100;
                    break;
                case RateType.Permilage:
                    divider = 1000;
                    break;
            }

            return (numerator / divider);
        }
        #endregion

    }
}