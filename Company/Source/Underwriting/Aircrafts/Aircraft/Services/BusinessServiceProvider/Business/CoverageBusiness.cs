using Sistran.Company.Application.Aircrafts.AircraftBusinessService.EEProvider.Assemblers;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.RulesEngine;
using System;
using System.Collections.Generic;
using Rules = Sistran.Core.Framework.Rules;

namespace Sistran.Company.Application.Aircrafts.AircraftBusinessService.EEProvider.Business
{
    public class CoverageBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        public CompanyCoverage RunRulesCoverage(CompanyAircraft companyAircraft, CompanyCoverage companyCoverage, int ruleSetId)
        {
            if (!companyAircraft.Risk.Policy.IsPersisted)
            {
                companyAircraft.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyAircraft.Risk.Policy.Id, false);
            }

            if (!companyAircraft.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyAircraft.Risk.Policy;

                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                companyAircraft = AircraftBusiness.GetCompanyAircraftTemporalByRiskId(companyAircraft.Risk.Id);

                companyAircraft.Risk.Policy = companyPolicy;
            }

            return RunRules(companyAircraft, companyCoverage, ruleSetId);
        }

        private CompanyCoverage RunRules(CompanyAircraft companyAircraft, CompanyCoverage companyCoverage, int ruleSetId)
        {
            EntityAssembler.CreateFacadeGeneral(Facade, companyAircraft.Risk.Policy);
            EntityAssembler.CreateFacadeRiskAircraft(Facade, companyAircraft);
            EntityAssembler.CreateFacadeCoverage(Facade, companyCoverage);

            Facade = RulesEngineDelegate.ExecuteRules(ruleSetId, Facade);

            ModelAssembler.CreateCompanyCoverage(companyCoverage, Facade);
            return companyCoverage;
        }

        public CompanyCoverage Quotate(CompanyAircraft companyAircraft, CompanyCoverage companyCoverage, bool runRulesPre, bool runRulesPost)
        {
            if (!companyAircraft.Risk.Policy.IsPersisted)
            {
                companyAircraft.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyAircraft.Risk.Policy.Id, false);
            }

            if (!companyAircraft.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyAircraft.Risk.Policy;

                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                companyAircraft = AircraftBusiness.GetCompanyAircraftTemporalByRiskId(companyAircraft.Risk.Id);

                companyAircraft.Risk.Policy = companyPolicy;
            }

            if (runRulesPre && companyCoverage.RuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyAircraft, companyCoverage, companyCoverage.RuleSetId.Value);
            }

            if (runRulesPost && companyCoverage.PosRuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyAircraft, companyCoverage, companyCoverage.PosRuleSetId.Value);
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
            companyCoverage.EndorsementType = companyAircraft.Risk.Policy.Endorsement.EndorsementType;
            //List<Core.Application.ProductServices.Models.ProductCurrency> productCurrencies = DelegateService.productServiceCore.GetProductCurrencies(companyAircraft.Risk.Policy.Product.Id);
            //foreach (Core.Application.ProductServices.Models.ProductCurrency item in productCurrencies)
            //{
            //    if (item.Id == companyAircraft.Risk.Policy.)
            //    {

            //    }
            //}
            //productCurrencies.ForEach(x  = > x.)
            return DelegateService.underwritingService.QuotateCompanyCoverage(companyCoverage, companyAircraft.Risk.Policy.Endorsement.PolicyId, companyAircraft.Risk.RiskId,2);


            //companyCoverage = DelegateService.underwritingService.QuotateCompanyCoverage(companyCoverage, companyAircraft.Risk.Policy.Endorsement.PolicyId, companyAircraft.Risk.RiskId);
            //companyCoverage.PremiumAmount = (companyCoverage.PremiumAmount * companyCoverage.DepositPremiumPercent)/100;
            //return companyCoverage;
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
            AircraftBusiness AircraftBusiness = new AircraftBusiness();
            CompanyAircraft companyAircraft = AircraftBusiness.GetCompanyAircraftTemporalByRiskId(riskId);
            companyAircraft.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            CompanyCoverage companyCoverage = DelegateService.underwritingService.GetCompanyCoverageByRiskCoverageId(coverageId);

            companyCoverage.CoverStatus = CoverageStatusType.Excluded;
            companyCoverage.EndorsementType = companyAircraft.Risk.Policy.Endorsement.EndorsementType;
            companyCoverage.CurrentFrom = companyAircraft.Risk.Policy.CurrentFrom;
            companyCoverage.AccumulatedPremiumAmount = 0;
            companyCoverage.LimitAmount = 0;
            companyCoverage.SubLimitAmount = 0;
            companyCoverage.EndorsementLimitAmount = companyCoverage.EndorsementLimitAmount * -1;
            companyCoverage.EndorsementSublimitAmount = companyCoverage.EndorsementSublimitAmount * -1;

            companyCoverage = Quotate(companyAircraft, companyCoverage, false, false);
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