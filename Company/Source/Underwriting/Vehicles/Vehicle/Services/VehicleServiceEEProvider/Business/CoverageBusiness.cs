using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Assemblers;
using Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.DAOs;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.Utilities.RulesEngine;
using System;
using System.Collections.Generic;
using Rules = Sistran.Core.Framework.Rules;

namespace Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Business
{
    public class CoverageBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        public CompanyCoverage RunRulesCoverage(CompanyVehicle companyVehicle, CompanyCoverage companyCoverage, int ruleSetId)
        {
            if (!companyVehicle.Risk.Policy.IsPersisted)
            {
                companyVehicle.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyVehicle.Risk.Policy.Id, false);
            }

            if (!companyVehicle.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyVehicle.Risk.Policy;

                VehicleDAO vehicleDAO = new VehicleDAO();
                companyVehicle = vehicleDAO.GetCompanyVehicleByRiskId(companyVehicle.Risk.Id);

                companyVehicle.Risk.Policy = companyPolicy;
            }

            return RunRules(companyVehicle, companyCoverage, ruleSetId);
        }

        private CompanyCoverage RunRules(CompanyVehicle companyVehicle, CompanyCoverage companyCoverage, int ruleSetId)
        {
            EntityAssembler.CreateFacadeGeneral(Facade, companyVehicle.Risk.Policy);
            EntityAssembler.CreateFacadeRiskVehicle(Facade, companyVehicle);
            EntityAssembler.CreateFacadeCoverage(Facade, companyCoverage);

            Facade = RulesEngineDelegate.ExecuteRules(ruleSetId, Facade);

            ModelAssembler.CreateCoverage(companyCoverage, Facade);
            return companyCoverage;
        }

        public CompanyCoverage Quotate(CompanyVehicle companyVehicle, CompanyCoverage companyCoverage, bool runRulesPre, bool runRulesPost)
        {
            if (!companyVehicle.Risk.Policy.IsPersisted)
            {
                companyVehicle.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyVehicle.Risk.Policy.Id, false);
            }

            if (!companyVehicle.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyVehicle.Risk.Policy;

                VehicleDAO vehicleDAO = new VehicleDAO();
                companyVehicle = vehicleDAO.GetCompanyVehicleByRiskId(companyVehicle.Risk.Id);

                companyVehicle.Risk.Policy = companyPolicy;
            }

            if (runRulesPre && companyCoverage.RuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyVehicle, companyCoverage, companyCoverage.RuleSetId.Value);
            }

            if (runRulesPost && companyCoverage.PosRuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyVehicle, companyCoverage, companyCoverage.PosRuleSetId.Value);
            }

            if (companyCoverage.FlatRatePorcentage > 0 && Facade.GetConcept<decimal>(CompanyRuleConceptRisk.FlatRatePercentage) > 0)
            {
                if (companyCoverage.SubLimitAmount > 0)
                {
                    if (companyCoverage.RateType != Core.Services.UtilitiesServices.Enums.RateType.FixedValue)
                       companyCoverage.Rate = Math.Round(Convert.ToDecimal(Facade.GetConcept<decimal>(CompanyRuleConceptRisk.FlatRatePercentage) * companyCoverage.FlatRatePorcentage / 100), QuoteManager.PremiumRoundValue);
                       Facade.SetConcept(CompanyRuleConceptRisk.CoverageIdLast, companyCoverage.Id);

                }
                else
                {
                    companyCoverage.FlatRatePorcentage = 0;
                    companyCoverage.Rate = 0;
                    companyCoverage.PremiumAmount = 0;
                }
            }
            companyCoverage.CoveredRiskType = (int)companyVehicle.Risk?.CoveredRiskType;
            return DelegateService.underwritingService.QuotateCompanyCoverage(companyCoverage, companyVehicle.Risk.Policy.Endorsement.PolicyId, companyVehicle.Risk.RiskId,2);
        }

        public void UpdateFacadeConcepts(Rules.Facade facade)
        {
            facade.SetConcept(CompanyRuleConceptGeneral.ClausesRemove, UpdateConceptList(facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesRemove), Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesRemove)));
            facade.SetConcept(CompanyRuleConceptGeneral.ClausesAdd, UpdateConceptList(facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesAdd), Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesAdd)));
            facade.SetConcept(CompanyRuleConceptRisk.ClausesRemove, UpdateConceptList(facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesRemove), Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesRemove)));
            facade.SetConcept(CompanyRuleConceptRisk.ClausesAdd, UpdateConceptList(facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesAdd), Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesAdd)));
            facade.SetConcept(CompanyRuleConceptRisk.CoveragesRemove, UpdateConceptList(facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesRemove), Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesRemove)));
            facade.SetConcept(CompanyRuleConceptRisk.CoveragesAdd, UpdateConceptList(facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd), Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd)));

            if (Facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageIdLast) > 0)
            {
                facade.SetConcept(CompanyRuleConceptRisk.CoverageIdLast, Facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageIdLast));
            }
        }

        private List<int> UpdateConceptList(List<int> initialList, List<int> newItems)
        {
            if (newItems?.Count > 0)
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