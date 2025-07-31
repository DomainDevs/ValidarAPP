using Sistran.Company.Application.Location.LiabilityServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.RulesEngine;
using Rules = Sistran.Core.Framework.Rules;
using System.Collections.Generic;
using System.Linq;
using UNDEnums = Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Company.Application.Location.LiabilityServices.Models;

namespace Sistran.Company.Application.Location.LiabilityServices.EEProvider.BusinessModels
{
    public class LiabilityRiskBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        public CompanyLiabilityRisk RunRulesRisk(CompanyLiabilityRisk companyLiabilityRisk, int ruleId)
        {
            if (!companyLiabilityRisk.Risk.Policy.IsPersisted)
            {
                companyLiabilityRisk.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyLiabilityRisk.Risk.Policy.Id, false);
            }

            return RunRules(companyLiabilityRisk, ruleId);
        }

        private CompanyLiabilityRisk RunRules(CompanyLiabilityRisk companyLiabilityRisk, int ruleId)
        {
            UnderwritingServices.Assembler.ModelAssembler.CreateFacadeGeneral(companyLiabilityRisk.Risk.Policy, Facade);
            EntityAssembler.CreateFacadeRiskLiability(Facade, companyLiabilityRisk);

            Facade = RulesEngineDelegate.ExecuteRules(ruleId, Facade);

            ModelAssembler.CreateLiabilityRisk(companyLiabilityRisk, Facade);
            return companyLiabilityRisk;
        }

        public CompanyLiabilityRisk QuotateLiability(CompanyLiabilityRisk companyLiabilityRisk, bool runRulesPre, bool runRulesPost)
        {
            bool updatePolicy = false;

            if (!companyLiabilityRisk.Risk.Policy.IsPersisted)
            {
                companyLiabilityRisk.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyLiabilityRisk.Risk.Policy.Id, false);
                updatePolicy = true;
            }

            if (runRulesPost && companyLiabilityRisk.Risk.Policy.Product.CoveredRisk.RuleSetId.GetValueOrDefault() > 0)
            {
                companyLiabilityRisk = RunRules(companyLiabilityRisk, companyLiabilityRisk.Risk.Policy.Product.CoveredRisk.RuleSetId.Value);
            }

            if (companyLiabilityRisk.Risk.Status == UNDEnums.RiskStatusType.Excluded)
            {
                companyLiabilityRisk.Risk.Coverages = companyLiabilityRisk.Risk.Coverages.Where(x => x.CoverStatus != UNDEnums.CoverageStatusType.Included).ToList();
                companyLiabilityRisk.Risk.Coverages.ForEach(x => x.CoverStatus = UNDEnums.CoverageStatusType.Excluded);
            }

            companyLiabilityRisk.Risk.Premium = 0;
            companyLiabilityRisk.Risk.AmountInsured = 0;
            List<CompanyCoverage> quotateCoverages = new List<CompanyCoverage>();

            foreach (CompanyCoverage coverage in companyLiabilityRisk.Risk.Coverages)
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                quotateCoverages.Add(coverageBusiness.Quotate(companyLiabilityRisk, coverage, runRulesPre, runRulesPost));
                coverageBusiness.UpdateFacadeConcepts(Facade);
            }

            companyLiabilityRisk.Risk.Coverages = quotateCoverages;

            //Eliminar Clausulas Poliza
            companyLiabilityRisk.Risk.Policy.Clauses = DelegateService.underwritingService.RemoveClauses(companyLiabilityRisk.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesRemove));

            //Agregar Clausulas Poliza
            companyLiabilityRisk.Risk.Policy.Clauses = DelegateService.underwritingService.AddClauses(companyLiabilityRisk.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesAdd));

            //Eliminar Clausulas Riesgo
            companyLiabilityRisk.Risk.Clauses = DelegateService.underwritingService.RemoveClauses(companyLiabilityRisk.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesRemove));

            //Agregar Clausulas Riesgo
            companyLiabilityRisk.Risk.Clauses = DelegateService.underwritingService.AddClauses(companyLiabilityRisk.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesAdd));

            //Eliminar Coberturas
            companyLiabilityRisk.Risk.Coverages = DelegateService.underwritingService.RemoveCoverages(companyLiabilityRisk.Risk.Coverages, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesRemove));

            //Agregar Coberturas
            if (Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd) != null)
            {
                foreach (int coverageId in Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd))
                {
                    if (!companyLiabilityRisk.Risk.Coverages.Exists(x => x.Id == coverageId))
                    {
                        CompanyCoverage quotateCoverage = new CompanyCoverage();
                        quotateCoverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, companyLiabilityRisk.Risk.Policy.Product.Id, companyLiabilityRisk.Risk.GroupCoverage.Id);

                        CoverageBusiness coverageBusiness = new CoverageBusiness();
                        companyLiabilityRisk.Risk.Coverages.Add(coverageBusiness.Quotate(companyLiabilityRisk, quotateCoverage, true, true));
                        coverageBusiness.UpdateFacadeConcepts(Facade);
                    }
                }
            }

            //Deducibles
            companyLiabilityRisk.Risk.Coverages = DelegateService.underwritingService.GetDeductiblesByCompanyCoverages(companyLiabilityRisk.Risk.Coverages);

            foreach (CompanyCoverage companyCoverage in companyLiabilityRisk.Risk.Coverages)
            {
                if (companyCoverage.Deductible != null)
                {
                    DelegateService.underwritingService.CalculateCompanyPremiumDeductible(companyCoverage);
                }
            }

            //Prima Mínima
            if (companyLiabilityRisk.Risk.Policy.CalculateMinPremium == true)
            {
                decimal minimumPremiumAmount = DelegateService.underwritingService.GetMinimumPremiumAmountByModelDynamicConcepts(companyLiabilityRisk.Risk.DynamicProperties);

                if (minimumPremiumAmount > 0)
                {
                    bool prorate = DelegateService.underwritingService.GetProrateMinimumPremiumByModelDynamicConcepts(companyLiabilityRisk.Risk.DynamicProperties);
                    companyLiabilityRisk.Risk.Coverages = DelegateService.underwritingService.CalculateMinimumPremiumRatePerCoverage(companyLiabilityRisk.Risk.Coverages, minimumPremiumAmount, prorate, false);
                }
            }
            //Prima Mínima

            //companyLiabilityRisk.Risk.Premium = companyLiabilityRisk.Risk.Coverages.Sum(x => x.PremiumAmount);
            //companyLiabilityRisk.Risk.AmountInsured = companyLiabilityRisk.Risk.Coverages.Sum(x => x.LimitAmount);
            if (companyLiabilityRisk.Risk.Coverages.Any(x => x.IsPrimary))
            {
                companyLiabilityRisk.Risk.Premium = companyLiabilityRisk.Risk.Coverages.Where(x => x.IsPrimary).Sum(x => x.PremiumAmount);
                companyLiabilityRisk.Risk.AmountInsured = companyLiabilityRisk.Risk.Coverages.Where(x => x.IsPrimary).Sum(x => x.LimitAmount);
            }

            if (updatePolicy)
            {
                DelegateService.underwritingService.CreatePolicyTemporal(companyLiabilityRisk.Risk.Policy, false);
            }

            return companyLiabilityRisk;
        }

        public List<CompanyLiabilityRisk> QuotateLiabilities(CompanyPolicy companyPolicy, List<CompanyLiabilityRisk> companyPropertyRisks, bool runRulesPre, bool runRulesPost)
        {
            if (!companyPolicy.IsPersisted)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Id, false);
            }

            foreach (CompanyLiabilityRisk companyLiabilityRisk in companyPropertyRisks)
            {
                companyLiabilityRisk.Risk.Policy = companyPolicy;
                QuotateLiability(companyLiabilityRisk, runRulesPre, runRulesPost);
            }

            return companyPropertyRisks;
        }
    }
}