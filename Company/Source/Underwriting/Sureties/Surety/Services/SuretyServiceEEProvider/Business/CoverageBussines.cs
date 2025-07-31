using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Assemblers;
using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.DAOs;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Utilities.RulesEngine;
using System.Collections.Generic;
using Rules = Sistran.Core.Framework.Rules;
using System.Linq;

namespace Sistran.Company.Application.Sureties.SuretyServices.EEProvider.BusinessModels
{
    public class CoverageBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        public CompanyCoverage RunRulesCoverage(CompanyContract companyContract, CompanyCoverage companyCoverage, int ruleSetId)
        {
            if (!companyContract.Risk.Policy.IsPersisted)
            {
                companyContract.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyContract.Risk.Policy.Id, false);
            }

            if (!companyContract.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyContract.Risk.Policy;

                ContractDAO contractDAO = new ContractDAO();
                companyContract = contractDAO.GetCompanyContractByRiskId(companyContract.Risk.Id);

                companyContract.Risk.Policy = companyPolicy;
            }

            return RunRules(companyContract, companyCoverage, ruleSetId);
        }

        private CompanyCoverage RunRules(CompanyContract companyContract, CompanyCoverage companyCoverage, int ruleSetId)
        {
            Rules.Facade Facade = new Rules.Facade();
            Facade = DelegateService.underwritingService.CreateFacadeGeneral(companyContract.Risk.Policy);
            EntityAssembler.CreateFacadeCoverage(Facade, companyCoverage);
            EntityAssembler.CreateFacadeRiskContract(Facade, companyContract);

            Facade = RulesEngineDelegate.ExecuteRules(ruleSetId, Facade);
            ModelAssembler.CreateCoverage(companyCoverage, Facade);

            return companyCoverage;
        }

        public CompanyCoverage Quotate(CompanyContract companyContract, CompanyCoverage companyCoverage, bool runRulesPre, bool runRulesPost)
        {
            if (!companyContract.Risk.Policy.IsPersisted)
            {
                companyContract.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyContract.Risk.Policy.Id, false);
            }

            if (!companyContract.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyContract.Risk.Policy;

                ContractDAO contractDAO = new ContractDAO();
                companyContract = contractDAO.GetCompanyContractByRiskId(companyContract.Risk.Id);

                companyContract.Risk.Policy = companyPolicy;
            }

            if (runRulesPre && companyCoverage.RuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyContract, companyCoverage, companyCoverage.RuleSetId.Value);
            }

            if (runRulesPost && companyCoverage.PosRuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyContract, companyCoverage, companyCoverage.PosRuleSetId.Value);
            }
            companyCoverage.CoveredRiskType = (int)companyContract.Risk.CoveredRiskType;
            Core.Application.ProductServices.Models.ProductCurrency productCurrency = DelegateService.productServiceCore.GetProductCurrencies(companyContract.Risk.Policy.Product.Id).Where(x => x.Id == companyContract.Risk.Policy.ExchangeRate.Currency.Id).FirstOrDefault();
            return DelegateService.underwritingService.QuotateCompanyCoverage(companyCoverage, companyContract.Risk.Policy.Endorsement.PolicyId, companyContract.Risk.RiskId, productCurrency.DecimalQuantity);
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