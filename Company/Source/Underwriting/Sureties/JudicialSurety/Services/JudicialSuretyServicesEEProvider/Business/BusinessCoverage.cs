using JudicialSuretyServicesEEProvider;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Assemblers;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.DAOs;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.Utilities.RulesEngine;
using System.Collections.Generic;
using Rules = Sistran.Core.Framework.Rules;
using System.Linq;
using System;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Business
{
    public class CoverageBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        public CompanyCoverage RunRulesCoverage(CompanyJudgement companyJudgement, CompanyCoverage companyCoverage, int ruleSetId)
        {
            if (!companyJudgement.Risk.Policy.IsPersisted)
            {
                companyJudgement.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyJudgement.Risk.Policy.Id, false);
            }

            if (!companyJudgement.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyJudgement.Risk.Policy;

                JudicialSuretyDAO judicialSuretyDAO = new JudicialSuretyDAO();
                companyJudgement = judicialSuretyDAO.GetCompanyJudgementByRiskId(companyJudgement.Risk.Id);

                companyJudgement.Risk.Policy = companyPolicy;
            }

            return RunRules(companyJudgement, companyCoverage, ruleSetId);
        }

        private CompanyCoverage RunRules(CompanyJudgement companyJudgement, CompanyCoverage companyCoverage, int ruleSetId)
        {
            Rules.Facade Facade = new Rules.Facade();
            Facade = DelegateService.underwritingService.CreateFacadeGeneral(companyJudgement.Risk.Policy);
            EntityAssembler.CreateFacadeRiskJudgement(Facade, companyJudgement);
            EntityAssembler.CreateFacadeCoverage(Facade, companyCoverage);

            Facade = RulesEngineDelegate.ExecuteRules(ruleSetId, Facade);

            ModelAssembler.CreateCoverage(companyCoverage, Facade);
            return companyCoverage;
        }

        public CompanyCoverage Quotate(CompanyJudgement companyJudgement, CompanyCoverage companyCoverage, bool runRulesPre, bool runRulesPost)
        {
            CompanyCoverage companyCoverage1 = new CompanyCoverage();
            if (!companyJudgement.Risk.Policy.IsPersisted)
            {
                companyJudgement.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyJudgement.Risk.Policy.Id, false);
            }

            if (!companyJudgement.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyJudgement.Risk.Policy;

                JudicialSuretyDAO judicialSuretyDAO = new JudicialSuretyDAO();
                companyJudgement = judicialSuretyDAO.GetCompanyJudgementByRiskId(companyJudgement.Risk.Id);

                companyJudgement.Risk.Policy = companyPolicy;
            }

            if (runRulesPre && companyCoverage.RuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyJudgement, companyCoverage, companyCoverage.RuleSetId.Value);
            }

            if (runRulesPost && companyCoverage.PosRuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyJudgement, companyCoverage, companyCoverage.PosRuleSetId.Value);
            }
            Core.Application.ProductServices.Models.ProductCurrency productCurrency = DelegateService.productServiceCore.GetProductCurrencies(companyJudgement.Risk.Policy.Product.Id).Where(x => x.Id == companyJudgement.Risk.Policy.ExchangeRate.Currency.Id).FirstOrDefault();
            companyCoverage1 = DelegateService.underwritingService.QuotateCompanyCoverage(companyCoverage, companyJudgement.Risk.Policy.Endorsement.PolicyId, companyJudgement.Risk.RiskId, productCurrency.DecimalQuantity);
            return companyCoverage1;

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