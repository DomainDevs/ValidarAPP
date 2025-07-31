using Sistran.Company.Application.Location.LiabilityServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.Utilities.RulesEngine;
using System.Collections.Generic;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.Location.LiabilityServices.EEProvider.DAOs;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Linq;
using Sistran.Core.Application.UnderwritingServices.Enums;

namespace Sistran.Company.Application.Location.LiabilityServices.EEProvider.BusinessModels
{
    public class CoverageBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        public CompanyCoverage RunRulesCoverage(CompanyLiabilityRisk companyLiabilityRisk, CompanyCoverage companyCoverage, int ruleSetId)
        {
            if (!companyLiabilityRisk.Risk.Policy.IsPersisted)
            {
                companyLiabilityRisk.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyLiabilityRisk.Risk.Policy.Id, false);
            }

            if (!companyLiabilityRisk.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyLiabilityRisk.Risk.Policy;

                LiabilityDAO liabilityDAO = new LiabilityDAO();
                companyLiabilityRisk = liabilityDAO.GetCompanyLiabilityByRiskId(companyLiabilityRisk.Risk.Id);

                companyLiabilityRisk.Risk.Policy = companyPolicy;
            }

            return RunRules(companyLiabilityRisk, companyCoverage, ruleSetId);
        }

        private CompanyCoverage RunRules(CompanyLiabilityRisk companyLiabilityRisk, CompanyCoverage companyCoverage, int ruleSetId)
        {
            Facade = DelegateService.underwritingService.CreateFacadeGeneral(companyLiabilityRisk.Risk.Policy);
            EntityAssembler.CreateFacadeRiskLiability(Facade, companyLiabilityRisk);
            EntityAssembler.CreateFacadeCoverage(Facade, companyCoverage);

            Facade = RulesEngineDelegate.ExecuteRules(ruleSetId, Facade);

            ModelAssembler.CreateCoverage(companyCoverage, Facade);
            return companyCoverage;
        }

        public CompanyCoverage Quotate(CompanyLiabilityRisk companyLiabilityRisk, CompanyCoverage companyCoverage, bool runRulesPre, bool runRulesPost)
        {
            CompanyCoverage companyCoverage1 = new CompanyCoverage();

            if (!companyLiabilityRisk.Risk.Policy.IsPersisted)
            {
                companyLiabilityRisk.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyLiabilityRisk.Risk.Policy.Id, false);
            }

            if (!companyLiabilityRisk.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyLiabilityRisk.Risk.Policy;

                LiabilityDAO liabilityDAO = new LiabilityDAO();
                companyLiabilityRisk = liabilityDAO.GetCompanyLiabilityByRiskId(companyLiabilityRisk.Risk.Id);

                companyLiabilityRisk.Risk.Policy = companyPolicy;
            }

            if (runRulesPre && companyCoverage.RuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyLiabilityRisk, companyCoverage, companyCoverage.RuleSetId.Value);
            }

            if (runRulesPost && companyCoverage.PosRuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyLiabilityRisk, companyCoverage, companyCoverage.PosRuleSetId.Value);
            }
            if (companyCoverage.Deductible != null)
            {
                if (companyCoverage.Deductible.Id > -1)
                {
                    List<CompanyCoverage> coverages = new List<CompanyCoverage>();
                    coverages.Add(companyCoverage);
                    coverages = DelegateService.underwritingService.GetDeductiblesByCompanyCoverages(coverages);
                    companyCoverage.Deductible = coverages.First().Deductible;
                }
                companyCoverage.Deductible.Rate = companyCoverage.Deductible.Rate;
                companyCoverage.Deductible.RateType = companyCoverage.Deductible.RateType;
                DelegateService.underwritingService.CalculateCompanyPremiumDeductible(companyCoverage);
            }
            Core.Application.ProductServices.Models.ProductCurrency productCurrency = DelegateService.productServiceCore.GetProductCurrencies(companyLiabilityRisk.Risk.Policy.Product.Id).Where(x => x.Id == companyLiabilityRisk.Risk.Policy.ExchangeRate.Currency.Id).FirstOrDefault();
            companyCoverage.CoveredRiskType = (int)Core.Application.CommonService.Enums.CoveredRiskType.Location;
            companyCoverage1 = DelegateService.underwritingService.QuotateCompanyCoverage(companyCoverage, companyLiabilityRisk.Risk.Policy.Endorsement.PolicyId, companyLiabilityRisk.Risk.RiskId, productCurrency.DecimalQuantity, 0, companyLiabilityRisk?.Risk?.Policy?.Prefix?.Id);
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