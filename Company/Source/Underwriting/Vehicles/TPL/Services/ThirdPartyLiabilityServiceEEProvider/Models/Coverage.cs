using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Assemblers;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.DAOs;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Core.Application.Utilities.RulesEngine;
using System.Collections.Generic;
using Rules = Sistran.Core.Framework.Rules;

namespace Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.BusinessModels
{
    public class CoverageBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        public CompanyCoverage RunRulesCoverage(CompanyTplRisk companyTplRisk, CompanyCoverage companyCoverage, int ruleSetId)
        {
            if (!companyTplRisk.Risk.Policy.IsPersisted)
            {
                companyTplRisk.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyTplRisk.Risk.Policy.Id, false);
            }

            if (!companyTplRisk.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyTplRisk.Risk.Policy;

                ThirdPartyLiabilityDAO thirdPartyLiabilityDAO = new ThirdPartyLiabilityDAO();
                companyTplRisk = thirdPartyLiabilityDAO.GetCompanyTplRiskByRiskId(companyTplRisk.Risk.Id);

                companyTplRisk.Risk.Policy = companyPolicy;
            }

            return RunRules(companyTplRisk, companyCoverage, ruleSetId);
        }

        private CompanyCoverage RunRules(CompanyTplRisk companyTplRisk, CompanyCoverage companyCoverage, int ruleSetId)
        {
            UnderwritingServices.Assembler.ModelAssembler.CreateFacadeGeneral(companyTplRisk.Risk.Policy, Facade);
            EntityAssembler.CreateFacadeRiskThirdPartyLiability(Facade, companyTplRisk);
            EntityAssembler.CreateFacadeCoverage(Facade, companyCoverage);

            Facade = RulesEngineDelegate.ExecuteRules(ruleSetId, Facade);

            ModelAssembler.CreateCoverage(companyCoverage, Facade);
            return companyCoverage;
        }

        public CompanyCoverage Quotate(CompanyTplRisk companyTplRisk, CompanyCoverage companyCoverage, bool runRulesPre, bool runRulesPost)
        {
            if (!companyTplRisk.Risk.Policy.IsPersisted)
            {
                companyTplRisk.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyTplRisk.Risk.Policy.Id, false);
            }

            if (!companyTplRisk.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyTplRisk.Risk.Policy;

                ThirdPartyLiabilityDAO thirdPartyLiabilityDAO = new ThirdPartyLiabilityDAO();
                companyTplRisk = thirdPartyLiabilityDAO.GetCompanyTplRiskByRiskId(companyTplRisk.Risk.Id);

                companyTplRisk.Risk.Policy = companyPolicy;
            }

            if (runRulesPre && companyCoverage.RuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyTplRisk, companyCoverage, companyCoverage.RuleSetId.Value);
            }

            if (runRulesPost && companyCoverage.PosRuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyTplRisk, companyCoverage, companyCoverage.PosRuleSetId.Value);
            }

            return DelegateService.underwritingService.QuotateCompanyCoverage(companyCoverage, companyTplRisk.Risk.Policy.Endorsement.PolicyId, companyTplRisk.Risk.RiskId,2);
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