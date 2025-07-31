using Sistran.Company.Application.Location.PropertyServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.RulesEngine;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Company.Application.Location.PropertyServices.EEProvider.DAOs;
using Sistran.Company.Application.Location.PropertyServices.Models;
using System.Collections.Generic;
using Sistran.Company.Application.Utilities.RulesEngine;
using System;
using System.Linq;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Company.Application.UnderwritingServices;

namespace Sistran.Company.Application.Location.PropertyServices.EEProvider.Business
{
    public class CoverageBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        public CompanyCoverage RunRulesCoverage(CompanyPropertyRisk companyPropertyRisk, CompanyCoverage companyCoverage, int ruleSetId)
        {
            if (!companyPropertyRisk.Risk.Policy.IsPersisted)
            {
                companyPropertyRisk.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPropertyRisk.Risk.Policy.Id, false);
            }

            if (!companyPropertyRisk.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyPropertyRisk.Risk.Policy;

                PropertyDAO propertyDAO = new PropertyDAO();
                companyPropertyRisk = propertyDAO.GetCompanyPropertyByRiskId(companyPropertyRisk.Risk.Id);

                companyPropertyRisk.Risk.Policy = companyPolicy;
            }

            return RunRules(companyPropertyRisk, companyCoverage, ruleSetId);
        }

        public CompanyCoverage RunRules(CompanyPropertyRisk companyPropertyRisk, CompanyCoverage companyCoverage, int ruleSetId)
        {
            Facade = DelegateService.underwritingService.CreateFacadeGeneral(companyPropertyRisk.Risk.Policy);
            //EntityAssembler.CreateFacadeGeneral(Facade, companyPropertyRisk.Risk.Policy);
            EntityAssembler.CreateFacadeRiskProperty(Facade, companyPropertyRisk);
            EntityAssembler.CreateFacadeCoverage(Facade, companyCoverage);

            Facade = RulesEngineDelegate.ExecuteRules(ruleSetId, Facade);

            ModelAssembler.CreateCoverage(companyCoverage, Facade);
            return companyCoverage;
        }

        public CompanyCoverage Quotate(CompanyPropertyRisk companyPropertyRisk, CompanyCoverage companyCoverage, bool runRulesPre, bool runRulesPost)
        {
            if (!companyPropertyRisk.Risk.Policy.IsPersisted)
            {
                companyPropertyRisk.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPropertyRisk.Risk.Policy.Id, false);
            }

            if (!companyPropertyRisk.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyPropertyRisk.Risk.Policy;

                PropertyDAO propertyDAO = new PropertyDAO();
                companyPropertyRisk = propertyDAO.GetCompanyPropertyByRiskId(companyPropertyRisk.Risk.Id);
                companyPropertyRisk.Risk.Policy = companyPolicy;
            }

            if (runRulesPre && companyCoverage.RuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyPropertyRisk, companyCoverage, companyCoverage.RuleSetId.Value);
            }

            if (runRulesPost && companyCoverage.PosRuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyPropertyRisk, companyCoverage, companyCoverage.PosRuleSetId.Value);
            }
            Core.Application.ProductServices.Models.ProductCurrency productCurrency = DelegateService.productServiceCore.GetProductCurrencies(companyPropertyRisk.Risk.Policy.Product.Id).Where(x => x.Id == companyPropertyRisk.Risk.Policy.ExchangeRate.Currency.Id).FirstOrDefault();
            companyCoverage = DelegateService.underwritingService.QuotateCompanyCoverage(companyCoverage, companyPropertyRisk.Risk.Policy.Endorsement.PolicyId, companyPropertyRisk.Risk.RiskId, productCurrency.DecimalQuantity);

            if (companyPropertyRisk.Risk.Policy.Endorsement.EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.Emission
                || companyPropertyRisk.Risk.Policy.Endorsement.EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.Modification)
            {
                if (companyCoverage.InsuredObject.IsDeclarative)
                {
                    companyCoverage.PremiumAmount = decimal.Round(((companyCoverage.PremiumAmount * companyCoverage.DepositPremiumPercent) / 100), productCurrency.DecimalQuantity);
                }
            }
            else if (companyPropertyRisk.Risk.Policy.Endorsement.EndorsementType == EndorsementType.DeclarationEndorsement)
            {
                CompanyInsuredObject companyInsuredObject = companyPropertyRisk.InsuredObjects.Where(x => x.Id == companyCoverage.InsuredObject.Id).FirstOrDefault();
                companyCoverage.DepositPremiumPercent = (decimal)companyInsuredObject.DepositPremiunPercent;
                if (companyInsuredObject.IsDeclarative)
                {
                    PropertyRiskBusiness propertyRiskBusiness = new PropertyRiskBusiness();

                    // La póliza maneja prima en depósito
                    if (propertyRiskBusiness.HasDepositPremium(companyInsuredObject))
                    {
                        companyCoverage.PremiumAmount = 0;
                    }
                    else
                    {
                        companyCoverage.SubLimitAmount = companyCoverage.DeclaredAmount;
                        companyCoverage = DelegateService.underwritingService.QuotateCompanyCoverage(companyCoverage,
                            companyPropertyRisk.Risk.Policy.Endorsement.PolicyId, companyPropertyRisk.Risk.RiskId, productCurrency.DecimalQuantity);
                    }
                }
            }
            else if (companyPropertyRisk.Risk.Policy.Endorsement.EndorsementType == EndorsementType.AdjustmentEndorsement)
            {
                // TODO: Validar esta condición 
                CompanyInsuredObject companyInsuredObject = companyPropertyRisk.InsuredObjects.Where(x => x.Id == companyCoverage.InsuredObject.Id).FirstOrDefault();
                if (companyInsuredObject.IsDeclarative)
                {
                    PropertyRiskBusiness propertyRiskBusiness = new PropertyRiskBusiness();
                    companyCoverage.PremiumAmount = 0;

                    if (propertyRiskBusiness.HasDepositPremium(companyInsuredObject))
                    {
                        List<CompanyEndorsement> endorsements = propertyRiskBusiness.GetEndorsements(companyPropertyRisk.Risk.Policy);
                        bool hasBeenDepositPremiumOverflowed = propertyRiskBusiness.HasBeenDepositPremiumOverflowed(endorsements);
                        List<CompanyEndorsement> declarationEndorsement;
                        if (hasBeenDepositPremiumOverflowed)
                        {
                            // Si la prima ya ha sido excedida sólo se tienen en cuenta las últimas declaraciones
                            declarationEndorsement = propertyRiskBusiness.GetLastDeclarationEndorsements(endorsements);
                        }
                        else
                        {
                            declarationEndorsement = propertyRiskBusiness.GetAllDeclarationEndorsements(endorsements);
                        }

                        decimal depositPremium = 0;
                        CompanyCoverage tmpCoverage;

                        foreach (var declaration in declarationEndorsement)
                        {
                            List<CompanyRisk> compRisks = DelegateService.underwritingService.GetRiskByPolicyIdEndorsmentId(companyPropertyRisk.Risk.Policy.Endorsement.PolicyId,
                                declaration.Id);

                            foreach (var coRisk in compRisks)
                            {
                                foreach (var coverage in coRisk.Coverages)
                                {
                                    if (coverage.Id == companyCoverage.Id)
                                    {
                                        if (!hasBeenDepositPremiumOverflowed && depositPremium == 0)
                                        {
                                            // Se consulta la prima en depósito, sólo si no se ha desbordado
                                            depositPremium = propertyRiskBusiness.GetDepositPremiumByCoverageId(declaration, coverage.Id, coRisk);
                                        }
                                        coverage.SubLimitAmount = coverage.DeclaredAmount;
                                        tmpCoverage = DelegateService.underwritingService.QuotateCompanyCoverage(coverage,
                                            companyPropertyRisk.Risk.Policy.Endorsement.PolicyId,
                                            coRisk.Id, productCurrency.DecimalQuantity);
                                        companyCoverage.PremiumAmount += tmpCoverage.PremiumAmount;
                                    }
                                }
                            }
                        }
                        if (companyCoverage.PremiumAmount < depositPremium)
                        {
                            companyCoverage.PremiumAmount = 0;
                        }
                        else
                        {
                            companyCoverage.PremiumAmount = companyCoverage.PremiumAmount - depositPremium;
                        }
                    }
                }
            }
            //Deducible
            if (companyCoverage.Deductible != null)
            {
                if (companyCoverage.Deductible.Id > -1)
                {
                    List<CompanyCoverage> coverages = new List<CompanyCoverage>();
                    coverages.Add(companyCoverage);
                    coverages = DelegateService.underwritingService.GetDeductiblesByCompanyCoverages(coverages);
                    companyCoverage.Deductible = coverages.First().Deductible;
                }
                companyCoverage.Deductible.Rate = companyCoverage.Rate;
                companyCoverage.Deductible.RateType = (RateType)companyCoverage.RateType;
                DelegateService.underwritingService.CalculateCompanyPremiumDeductible(companyCoverage);
            }         
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