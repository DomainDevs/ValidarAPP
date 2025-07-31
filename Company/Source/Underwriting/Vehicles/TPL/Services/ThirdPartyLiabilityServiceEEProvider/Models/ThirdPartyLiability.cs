using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Assemblers;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.RulesEngine;
using System.Collections.Generic;
using System.Linq;
using Rules = Sistran.Core.Framework.Rules;

namespace Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.BusinessModels
{
    public class ThirdPartyLiabilityBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        public CompanyTplRisk RunRulesRisk(CompanyTplRisk companyTplRisk, int ruleId)
        {
            if (!companyTplRisk.Risk.Policy.IsPersisted)
            {
                companyTplRisk.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyTplRisk.Risk.Policy.Id, false);
            }

            return RunRules(companyTplRisk, ruleId);
        }

        private CompanyTplRisk RunRules(CompanyTplRisk companyTplRisk, int ruleId)
        {
            Facade = DelegateService.underwritingService.CreateFacadeGeneral(companyTplRisk.Risk.Policy);
            EntityAssembler.CreateFacadeRiskThirdPartyLiability(Facade, companyTplRisk);
            Facade = RulesEngineDelegate.ExecuteRules(ruleId, Facade);
            ModelAssembler.CreateThirdPartyLiability(companyTplRisk, Facade);
            return companyTplRisk;
        }

        public CompanyTplRisk QuotateThirdPartyLiability(CompanyTplRisk companyTplRisk, bool runRulesPre, bool runRulesPost)
        {
            bool updatePolicy = false;

            if (!companyTplRisk.Risk.Policy.IsPersisted)
            {
                companyTplRisk.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyTplRisk.Risk.Policy.Id, false);
                updatePolicy = true;
            }

            if (runRulesPost && companyTplRisk.Risk.Policy.Product.CoveredRisk.RuleSetId.GetValueOrDefault() > 0)
            {
                companyTplRisk = RunRules(companyTplRisk, companyTplRisk.Risk.Policy.Product.CoveredRisk.RuleSetId.Value);
            }

            if (companyTplRisk.Risk.Status == RiskStatusType.Excluded)
            {
                companyTplRisk.Risk.Coverages = companyTplRisk.Risk.Coverages.Where(x => x.CoverStatus != CoverageStatusType.Included).ToList();
                companyTplRisk.Risk.Coverages.ForEach(x => x.CoverStatus = CoverageStatusType.Excluded);
            }

            companyTplRisk.Risk.Premium = 0;
            companyTplRisk.Risk.AmountInsured = 0;
            List<CompanyCoverage> quotateCoverages = new List<CompanyCoverage>();

            foreach (CompanyCoverage coverage in companyTplRisk.Risk.Coverages)
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                quotateCoverages.Add(coverageBusiness.Quotate(companyTplRisk, coverage, runRulesPre, runRulesPost));
                coverageBusiness.UpdateFacadeConcepts(Facade);
            }

            companyTplRisk.Risk.Coverages = quotateCoverages;

            //Eliminar Clausulas Poliza
            companyTplRisk.Risk.Policy.Clauses = DelegateService.underwritingService.RemoveClauses(companyTplRisk.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesRemove));

            //Agregar Clausulas Poliza
            companyTplRisk.Risk.Policy.Clauses = DelegateService.underwritingService.AddClauses(companyTplRisk.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesAdd));

            //Eliminar Clausulas Riesgo
            companyTplRisk.Risk.Clauses = DelegateService.underwritingService.RemoveClauses(companyTplRisk.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesRemove));

            //Agregar Clausulas Riesgo
            companyTplRisk.Risk.Clauses = DelegateService.underwritingService.AddClauses(companyTplRisk.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesAdd));

            //Eliminar Coberturas
            companyTplRisk.Risk.Coverages = DelegateService.underwritingService.RemoveCoverages(companyTplRisk.Risk.Coverages, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesRemove));

            //Agregar Coberturas
            if (Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd) != null)
            {
                foreach (int coverageId in Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd))
                {
                    if (!companyTplRisk.Risk.Coverages.Exists(x => x.Id == coverageId))
                    {
                        CompanyCoverage quotateCoverage = new CompanyCoverage();
                        quotateCoverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, companyTplRisk.Risk.Policy.Product.Id, companyTplRisk.Risk.GroupCoverage.Id);

                        CoverageBusiness coverageBusiness = new CoverageBusiness();
                        companyTplRisk.Risk.Coverages.Add(coverageBusiness.Quotate(companyTplRisk, quotateCoverage, true, true));
                        coverageBusiness.UpdateFacadeConcepts(Facade);
                    }
                }
            }

            //Deducibles
            companyTplRisk.Risk.Coverages = DelegateService.underwritingService.GetDeductiblesByCompanyCoverages(companyTplRisk.Risk.Coverages);

            foreach (CompanyCoverage coverage in companyTplRisk.Risk.Coverages)
            {
                if (coverage.Deductible != null)
                {
                    DelegateService.underwritingService.CalculateCompanyPremiumDeductible(coverage);
                }
            }

            //Prima Mínima
            if (companyTplRisk.Risk.Policy.CalculateMinPremium == true)
            {
                decimal minimumPremiumAmount = DelegateService.underwritingService.GetMinimumPremiumAmountByModelDynamicConcepts(companyTplRisk.Risk.DynamicProperties);

                if (minimumPremiumAmount > 0)
                {
                    bool prorate = DelegateService.underwritingService.GetProrateMinimumPremiumByModelDynamicConcepts(companyTplRisk.Risk.DynamicProperties);
                    companyTplRisk.Risk.Coverages = DelegateService.underwritingService.CalculateMinimumPremiumRatePerCoverage(companyTplRisk.Risk.Coverages, minimumPremiumAmount, prorate, false);
                }
            }
            //Prima Mínima

            companyTplRisk.Risk.Premium = companyTplRisk.Risk.Coverages.Sum(x => x.PremiumAmount);
            companyTplRisk.Risk.AmountInsured = companyTplRisk.Risk.Coverages.Sum(x => x.LimitAmount);

            if (updatePolicy)
            {
                DelegateService.underwritingService.CreatePolicyTemporal(companyTplRisk.Risk.Policy, false);
            }

            return companyTplRisk;
        }

        public List<CompanyTplRisk> QuotateThirdPartyLiabilities(CompanyPolicy companyPolicy, List<CompanyTplRisk> companyPropertyRisks, bool runRulesPre, bool runRulesPost)
        {
            if (!companyPolicy.IsPersisted)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Id, false);
            }

            foreach (CompanyTplRisk companyTplRisk in companyPropertyRisks)
            {
                companyTplRisk.Risk.Policy = companyPolicy;
                QuotateThirdPartyLiability(companyTplRisk, runRulesPre, runRulesPost);
            }

            return companyPropertyRisks;
        }
    }
}