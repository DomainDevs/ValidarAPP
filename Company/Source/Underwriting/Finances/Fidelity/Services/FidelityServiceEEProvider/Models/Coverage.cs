using Sistran.Company.Application.Finances.FidelityServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.Utilities.RulesEngine;
using System.Collections.Generic;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Company.Application.Finances.FidelityServices.Models;
using Sistran.Company.Application.Finances.FidelityServices.EEProvider.DAOs;

namespace Sistran.Company.Application.Finances.FidelityServices.EEProvider.BusinessModels
{
    public class CoverageBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        public CompanyCoverage RunRulesCoverage(CompanyFidelityRisk companyFidelityRisk, CompanyCoverage companyCoverage, int ruleSetId)
        {
            if (!companyFidelityRisk.Risk.Policy.IsPersisted)
            {
                companyFidelityRisk.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyFidelityRisk.Risk.Policy.Id, false);
            }

            if (!companyFidelityRisk.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyFidelityRisk.Risk.Policy;

                FidelityBusiness fidelityDAO = new FidelityBusiness();
                companyFidelityRisk = fidelityDAO.GetCompanyFidelityByRiskId(companyFidelityRisk.Risk.Id);

                companyFidelityRisk.Risk.Policy = companyPolicy;
            }

            return RunRules(companyFidelityRisk, companyCoverage, ruleSetId);
        }

        private CompanyCoverage RunRules(CompanyFidelityRisk companyFidelityRisk, CompanyCoverage companyCoverage, int ruleSetId)
        {
            EntityAssembler.CreateFacadeGeneral(Facade, companyFidelityRisk.Risk.Policy);
            EntityAssembler.CreateFacadeRiskFidelity(Facade, companyFidelityRisk);
            EntityAssembler.CreateFacadeCoverage(Facade, companyCoverage);

            Facade = RulesEngineDelegate.ExecuteRules(ruleSetId, Facade);

            ModelAssembler.CreateCoverage(companyCoverage, Facade);
            return companyCoverage;
        }

        public CompanyCoverage Quotate(CompanyFidelityRisk companyFidelityRisk, CompanyCoverage companyCoverage, bool runRulesPre, bool runRulesPost)
        {
            if (!companyFidelityRisk.Risk.Policy.IsPersisted)
            {
                companyFidelityRisk.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyFidelityRisk.Risk.Policy.Id, false);
            }

            if (!companyFidelityRisk.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyFidelityRisk.Risk.Policy;

                FidelityBusiness fidelityDAO = new FidelityBusiness();
                companyFidelityRisk = fidelityDAO.GetCompanyFidelityByRiskId(companyFidelityRisk.Risk.Id);

                companyFidelityRisk.Risk.Policy = companyPolicy;
            }

            if (runRulesPre && companyCoverage.RuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyFidelityRisk, companyCoverage, companyCoverage.RuleSetId.Value);
            }

            if (runRulesPost && companyCoverage.PosRuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyFidelityRisk, companyCoverage, companyCoverage.PosRuleSetId.Value);
            }

            return DelegateService.underwritingService.QuotateCompanyCoverage(companyCoverage, companyFidelityRisk.Risk.Policy.Endorsement.PolicyId, companyFidelityRisk.Risk.RiskId, 2);
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