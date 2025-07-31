using Sistran.Company.Application.Finances.FidelityServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.RulesEngine;
using Rules = Sistran.Core.Framework.Rules;
using System.Collections.Generic;
using System.Linq;
using Enums = Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Company.Application.Finances.FidelityServices.Models;

namespace Sistran.Company.Application.Finances.FidelityServices.EEProvider.BusinessModels
{
    public class FidelityRiskBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        public CompanyFidelityRisk RunRulesRisk(CompanyFidelityRisk companyFidelityRisk, int ruleId)
        {
            if (!companyFidelityRisk.Risk.Policy.IsPersisted)
            {
                companyFidelityRisk.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyFidelityRisk.Risk.Policy.Id, false);
            }

            return RunRules(companyFidelityRisk, ruleId);
        }

        private CompanyFidelityRisk RunRules(CompanyFidelityRisk companyFidelityRisk, int ruleId)
        {
            EntityAssembler.CreateFacadeGeneral(Facade, companyFidelityRisk.Risk.Policy);
            EntityAssembler.CreateFacadeRiskFidelity(Facade, companyFidelityRisk);

            Facade = RulesEngineDelegate.ExecuteRules(ruleId, Facade);

            ModelAssembler.CreateFidelityRisk(companyFidelityRisk, Facade);
            return companyFidelityRisk;
        }

        public CompanyFidelityRisk QuotateFidelity(CompanyFidelityRisk companyFidelityRisk, bool runRulesPre, bool runRulesPost)
        {
            bool updatePolicy = false;

            if (!companyFidelityRisk.Risk.Policy.IsPersisted)
            {
                companyFidelityRisk.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyFidelityRisk.Risk.Policy.Id, false);
                updatePolicy = true;
            }

            if (runRulesPost && companyFidelityRisk.Risk.Policy.Product.CoveredRisk.RuleSetId.GetValueOrDefault() > 0)
            {
                companyFidelityRisk = RunRules(companyFidelityRisk, companyFidelityRisk.Risk.Policy.Product.CoveredRisk.RuleSetId.Value);
            }

            if (companyFidelityRisk.Risk.Status == Enums.RiskStatusType.Excluded)
            {
                companyFidelityRisk.Risk.Coverages = companyFidelityRisk.Risk.Coverages.Where(x => x.CoverStatus != Enums.CoverageStatusType.Included).ToList();
                companyFidelityRisk.Risk.Coverages.ForEach(x => x.CoverStatus = Enums.CoverageStatusType.Excluded);
            }

            companyFidelityRisk.Risk.Premium = 0;
            companyFidelityRisk.Risk.AmountInsured = 0;
            List<CompanyCoverage> quotateCoverages = new List<CompanyCoverage>();

            foreach (CompanyCoverage coverage in companyFidelityRisk.Risk.Coverages)
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                quotateCoverages.Add(coverageBusiness.Quotate(companyFidelityRisk, coverage, runRulesPre, runRulesPost));
                coverageBusiness.UpdateFacadeConcepts(Facade);
            }

            companyFidelityRisk.Risk.Coverages = quotateCoverages;

            //Eliminar Clausulas Poliza
            companyFidelityRisk.Risk.Policy.Clauses = DelegateService.underwritingService.RemoveClauses(companyFidelityRisk.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesRemove));

            //Agregar Clausulas Poliza
            companyFidelityRisk.Risk.Policy.Clauses = DelegateService.underwritingService.AddClauses(companyFidelityRisk.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesAdd));

            //Eliminar Clausulas Riesgo
            companyFidelityRisk.Risk.Clauses = DelegateService.underwritingService.RemoveClauses(companyFidelityRisk.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesRemove));

            //Agregar Clausulas Riesgo
            companyFidelityRisk.Risk.Clauses = DelegateService.underwritingService.AddClauses(companyFidelityRisk.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesAdd));

            //Eliminar Coberturas
            companyFidelityRisk.Risk.Coverages = DelegateService.underwritingService.RemoveCoverages(companyFidelityRisk.Risk.Coverages, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesRemove));

            //Agregar Coberturas
            if (Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd) != null)
            {
                foreach (int coverageId in Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd))
                {
                    if (!companyFidelityRisk.Risk.Coverages.Exists(x => x.Id == coverageId))
                    {
                        CompanyCoverage quotateCoverage = new CompanyCoverage();
                        quotateCoverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, companyFidelityRisk.Risk.Policy.Product.Id, companyFidelityRisk.Risk.GroupCoverage.Id);

                        CoverageBusiness coverageBusiness = new CoverageBusiness();
                        companyFidelityRisk.Risk.Coverages.Add(coverageBusiness.Quotate(companyFidelityRisk, quotateCoverage, true, true));
                        coverageBusiness.UpdateFacadeConcepts(Facade);
                    }
                }
            }

            //Deducibles
            companyFidelityRisk.Risk.Coverages = DelegateService.underwritingService.GetDeductiblesByCompanyCoverages(companyFidelityRisk.Risk.Coverages);

            foreach (CompanyCoverage companyCoverage in companyFidelityRisk.Risk.Coverages)
            {
                if (companyCoverage.Deductible != null)
                {
                    DelegateService.underwritingService.CalculateCompanyPremiumDeductible(companyCoverage);
                }
            }

            //Prima Mínima
            if (companyFidelityRisk.Risk.Policy.CalculateMinPremium == true)
            {
                decimal minimumPremiumAmount = DelegateService.underwritingService.GetMinimumPremiumAmountByModelDynamicConcepts(companyFidelityRisk.Risk.DynamicProperties);

                if (minimumPremiumAmount > 0)
                {
                    bool prorate = DelegateService.underwritingService.GetProrateMinimumPremiumByModelDynamicConcepts(companyFidelityRisk.Risk.DynamicProperties);
                    companyFidelityRisk.Risk.Coverages = DelegateService.underwritingService.CalculateMinimumPremiumRatePerCoverage(companyFidelityRisk.Risk.Coverages, minimumPremiumAmount, prorate, false);
                }
            }
            //Prima Mínima

            companyFidelityRisk.Risk.Premium = companyFidelityRisk.Risk.Coverages.Sum(x => x.PremiumAmount);
            companyFidelityRisk.Risk.AmountInsured = companyFidelityRisk.Risk.Coverages.Sum(x => x.LimitAmount);

            if (updatePolicy)
            {
                DelegateService.underwritingService.CreatePolicyTemporal(companyFidelityRisk.Risk.Policy, false);
            }

            return companyFidelityRisk;
        }

        public List<CompanyFidelityRisk> QuotateFidelities(CompanyPolicy companyPolicy, List<CompanyFidelityRisk> companyPropertyRisks, bool runRulesPre, bool runRulesPost)
        {
            if (!companyPolicy.IsPersisted)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Id, false);
            }

            foreach (CompanyFidelityRisk companyFidelityRisk in companyPropertyRisks)
            {
                companyFidelityRisk.Risk.Policy = companyPolicy;
                QuotateFidelity(companyFidelityRisk, runRulesPre, runRulesPost);
            }

            return companyPropertyRisks;
        }
    }
}