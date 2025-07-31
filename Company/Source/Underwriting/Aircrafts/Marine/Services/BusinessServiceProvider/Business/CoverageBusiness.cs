using Sistran.Company.Application.Marines.MarineBusinessService.EEProvider.Assemblers;
using Sistran.Company.Application.Marines.MarineBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.RulesEngine;
using System;
using System.Collections.Generic;
using Rules = Sistran.Core.Framework.Rules;

namespace Sistran.Company.Application.Marines.MarineBusinessService.EEProvider.Business
{
    public class CoverageBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        public CompanyCoverage RunRulesCoverage(CompanyMarine companyMarine, CompanyCoverage companyCoverage, int ruleSetId)
        {
            if (!companyMarine.Risk.Policy.IsPersisted)
            {
                companyMarine.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyMarine.Risk.Policy.Id, false);
            }

            if (!companyMarine.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyMarine.Risk.Policy;

                MarineBusiness MarineBusiness = new MarineBusiness();
                companyMarine = MarineBusiness.GetCompanyMarineTemporalByRiskId(companyMarine.Risk.Id);

                companyMarine.Risk.Policy = companyPolicy;
            }

            return RunRules(companyMarine, companyCoverage, ruleSetId);
        }

        private CompanyCoverage RunRules(CompanyMarine companyMarine, CompanyCoverage companyCoverage, int ruleSetId)
        {
            EntityAssembler.CreateFacadeGeneral(Facade, companyMarine.Risk.Policy);
            EntityAssembler.CreateFacadeRiskMarine(Facade, companyMarine);
            EntityAssembler.CreateFacadeCoverage(Facade, companyCoverage);

            Facade = RulesEngineDelegate.ExecuteRules(ruleSetId, Facade);

            ModelAssembler.CreateCompanyCoverage(companyCoverage, Facade);
            return companyCoverage;
        }

        public CompanyCoverage Quotate(CompanyMarine companyMarine, CompanyCoverage companyCoverage, bool runRulesPre, bool runRulesPost)
        {
            if (!companyMarine.Risk.Policy.IsPersisted)
            {
                companyMarine.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyMarine.Risk.Policy.Id, false);
            }

            if (!companyMarine.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyMarine.Risk.Policy;

                MarineBusiness MarineBusiness = new MarineBusiness();
                companyMarine = MarineBusiness.GetCompanyMarineTemporalByRiskId(companyMarine.Risk.Id);

                companyMarine.Risk.Policy = companyPolicy;
            }

            if (runRulesPre && companyCoverage.RuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyMarine, companyCoverage, companyCoverage.RuleSetId.Value);
            }

            if (runRulesPost && companyCoverage.PosRuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyMarine, companyCoverage, companyCoverage.PosRuleSetId.Value);
            }


            if (companyCoverage.FlatRatePorcentage > 0 && Facade.GetConcept<decimal>(CompanyRuleConceptRisk.FlatRatePercentage) > 0)
            {
                if (companyCoverage.SubLimitAmount > 0)
                {
                    companyCoverage.Rate = Math.Round(Convert.ToDecimal(Facade.GetConcept<decimal>(CompanyRuleConceptRisk.FlatRatePercentage) * companyCoverage.FlatRatePorcentage / 100), 6);
                    Facade.SetConcept(CompanyRuleConceptRisk.CoverageIdLast, companyCoverage.Id);
                }
                else
                {
                    companyCoverage.FlatRatePorcentage = 0;
                    companyCoverage.Rate = 0;
                    companyCoverage.PremiumAmount = 0;
                }
            }
            companyCoverage.EndorsementType = companyMarine.Risk.Policy.Endorsement.EndorsementType;

            return DelegateService.underwritingService.QuotateCompanyCoverage(companyCoverage, companyMarine.Risk.Policy.Endorsement.PolicyId, companyMarine.Risk.RiskId,2);
        }

        /// <summary>
        /// Excluir coberturas de una compañía
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <param name="riskCoverageId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public CompanyCoverage ExcludeCompanyCoverage(int temporalId, int riskId, int coverageId)
        {
            MarineBusiness MarineBusiness = new MarineBusiness();
            CompanyMarine companyMarine = MarineBusiness.GetCompanyMarineTemporalByRiskId(riskId);
            companyMarine.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            CompanyCoverage companyCoverage = DelegateService.underwritingService.GetCompanyCoverageByRiskCoverageId(coverageId);

            companyCoverage.CoverStatus = CoverageStatusType.Excluded;
            companyCoverage.EndorsementType = companyMarine.Risk.Policy.Endorsement.EndorsementType;
            companyCoverage.CurrentFrom = companyMarine.Risk.Policy.CurrentFrom;
            companyCoverage.AccumulatedPremiumAmount = 0;
            companyCoverage.LimitAmount = 0;
            companyCoverage.SubLimitAmount = 0;
            companyCoverage.EndorsementLimitAmount = companyCoverage.EndorsementLimitAmount * -1;
            companyCoverage.EndorsementSublimitAmount = companyCoverage.EndorsementSublimitAmount * -1;

            companyCoverage = Quotate(companyMarine, companyCoverage, false, false);
            return companyCoverage;
        }
        
        public void UpdateFacadeConcepts(Rules.Facade facade)
        {
            facade.SetConcept(CompanyRuleConceptGeneral.ClausesAdd, UpdateConceptList(facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesRemove), Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesRemove)));
            facade.SetConcept(CompanyRuleConceptGeneral.ClausesAdd, UpdateConceptList(facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesAdd), Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesAdd)));
            facade.SetConcept(CompanyRuleConceptGeneral.ClausesAdd, UpdateConceptList(facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesRemove), Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesRemove)));
            facade.SetConcept(CompanyRuleConceptGeneral.ClausesAdd, UpdateConceptList(facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesAdd), Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesAdd)));
            facade.SetConcept(CompanyRuleConceptGeneral.ClausesAdd, UpdateConceptList(facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesRemove), Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesRemove)));
            facade.SetConcept(CompanyRuleConceptGeneral.ClausesAdd, UpdateConceptList(facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd), Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd)));
        }

        private List<int> UpdateConceptList(List<int> initialList, List<int> newItems)
        {
            if (newItems != null)
            {
                if (initialList != null)
                {
                    initialList.AddRange(newItems);
                }
                else
                {
                    initialList = newItems;
                }
            }

            return initialList;
        }
    }
}