using Sistran.Company.Application.Transports.TransportBusinessService.EEProvider.Assemblers;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.Linq;
using Rules = Sistran.Core.Framework.Rules;

namespace Sistran.Company.Application.Transports.TransportBusinessService.EEProvider.Business
{
    public class CoverageBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        public CompanyCoverage RunRulesCoverage(CompanyTransport companyTransport, CompanyCoverage companyCoverage, int ruleSetId)
        {
            if (!companyTransport.Risk.Policy.IsPersisted)
            {
                companyTransport.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyTransport.Risk.Policy.Id, false);
            }

            if (!companyTransport.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyTransport.Risk.Policy;

                TransportBusiness transportBusiness = new TransportBusiness();
                companyTransport = transportBusiness.GetCompanyTransportTemporalByRiskId(companyTransport.Risk.Id);

                companyTransport.Risk.Policy = companyPolicy;
            }

            return RunRules(companyTransport, companyCoverage, ruleSetId);
        }

        private CompanyCoverage RunRules(CompanyTransport companyTransport, CompanyCoverage companyCoverage, int ruleSetId)
        {
            EntityAssembler.CreateFacadeGeneral(Facade, companyTransport.Risk.Policy);
            EntityAssembler.CreateFacadeRiskTransport(Facade, companyTransport);
            EntityAssembler.CreateFacadeCoverage(Facade, companyCoverage);

            Facade = RulesEngineDelegate.ExecuteRules(ruleSetId, Facade);

            ModelAssembler.CreateCompanyCoverage(companyCoverage, Facade);
            return companyCoverage;
        }

        public CompanyCoverage Quotate(CompanyTransport companyTransport, CompanyCoverage companyCoverage, bool runRulesPre, bool runRulesPost)
        {
            if (!companyTransport.Risk.Policy.IsPersisted)
            {
                companyTransport.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyTransport.Risk.Policy.Id, false);
            }

            if (!companyTransport.Risk.IsPersisted)
            {
                CompanyPolicy companyPolicy = companyTransport.Risk.Policy;

                TransportBusiness transportBusiness = new TransportBusiness();
                companyTransport = transportBusiness.GetCompanyTransportTemporalByRiskId(companyTransport.Risk.Id);
                companyTransport.Risk.Policy = companyPolicy;
            }

            if (runRulesPre && companyCoverage.RuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyTransport, companyCoverage, companyCoverage.RuleSetId.Value);
            }

            if (runRulesPost && companyCoverage.PosRuleSetId.GetValueOrDefault() > 0)
            {
                companyCoverage = RunRules(companyTransport, companyCoverage, companyCoverage.PosRuleSetId.Value);
            }

            companyCoverage = DelegateService.underwritingService.QuotateCompanyCoverage(companyCoverage, companyTransport.Risk.Policy.Endorsement.PolicyId, companyTransport.Risk.RiskId, 2);

            if (companyTransport.Risk.Policy.Endorsement.EndorsementType == EndorsementType.Emission || companyTransport.Risk.Policy.Endorsement.EndorsementType == EndorsementType.Renewal)
            {
                if (companyTransport.Risk.Policy.PolicyType.IsFloating)
                {
                    companyCoverage.PremiumAmount = (companyCoverage.PremiumAmount * companyCoverage.DepositPremiumPercent) / 100;
                }

            }

            if (companyTransport.Risk.Policy.Endorsement.EndorsementType == EndorsementType.Modification)
            {
                if (companyTransport.Risk.Policy.PolicyType.IsFloating)
                {
                    int policyDays = (int)(companyTransport.Risk.Policy.Endorsement.CurrentTo - companyTransport.Risk.Policy.Endorsement.CurrentFrom).TotalDays;
                    policyDays = (policyDays == 366) ? 365 : policyDays;
                    companyCoverage.PremiumAmount = System.Math.Round((decimal)((companyCoverage.LimitAmount * (companyCoverage.Rate / 100)) * (companyCoverage.DepositPremiumPercent / 100) / 365) * policyDays);
                }
            }

            // Declaración
            else if (companyTransport.Risk.Policy.Endorsement.EndorsementType == EndorsementType.DeclarationEndorsement)
            {
                if (companyTransport.Risk.Policy.PolicyType.IsFloating)
                {
                    TransportBusiness transportBusiness = new TransportBusiness();

                    // La póliza maneja prima en depósito
                    if (transportBusiness.HasDepositPremium(companyTransport))
                    {
                        companyCoverage.PremiumAmount = 0;
                    }
                    else
                    {
                        companyCoverage.SubLimitAmount = companyCoverage.DeclaredAmount;
                        companyCoverage = DelegateService.underwritingService.QuotateCompanyCoverage(companyCoverage,
                            companyTransport.Risk.Policy.Endorsement.PolicyId, companyTransport.Risk.RiskId, 2);
                    }
                }
            }
            // Ajuste
            else if (companyTransport.Risk.Policy.Endorsement.EndorsementType == EndorsementType.AdjustmentEndorsement)
            {
                // TODO: Validar esta condición 
                if (companyTransport.Risk.Policy.PolicyType.IsFloating)
                {
                    TransportBusiness transportBusiness = new TransportBusiness();
                    companyCoverage.PremiumAmount = 0;

                    if (transportBusiness.HasDepositPremium(companyTransport))
                    {
                        List<CompanyEndorsement> endorsements = transportBusiness.GetEndorsements(companyTransport.Risk.Policy);
                        bool hasBeenDepositPremiumOverflowed = transportBusiness.HasBeenDepositPremiumOverflowed(endorsements);
                        List<CompanyEndorsement> declarationEndorsement;
                        if (hasBeenDepositPremiumOverflowed)
                        {
                            // Si la prima ya ha sido excedida sólo se tienen en cuenta las últimas declaraciones
                            declarationEndorsement = transportBusiness.GetLastDeclarationEndorsements(endorsements);
                        }
                        else
                        {
                            declarationEndorsement = transportBusiness.GetAllDeclarationEndorsements(endorsements);
                        }

                        decimal depositPremium = 0;
                        CompanyCoverage tmpCoverage;

                        foreach (var declaration in declarationEndorsement)
                        {
                            List<CompanyRisk> compRisks = DelegateService.underwritingService.GetRiskByPolicyIdEndorsmentId(companyTransport.Risk.Policy.Endorsement.PolicyId,
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
                                            depositPremium = transportBusiness.GetDepositPremiumByCoverageId(declaration, coverage.Id, coRisk);
                                        }

                                        coverage.SubLimitAmount = coverage.DeclaredAmount;
                                        tmpCoverage = DelegateService.underwritingService.QuotateCompanyCoverage(coverage,
                                            companyTransport.Risk.Policy.Endorsement.PolicyId,
                                            coRisk.Id, 2);
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

            //Prorroga
            if (companyTransport.Risk.Policy.Endorsement.EndorsementType == EndorsementType.EffectiveExtension)
            {
                TransportBusiness transportBusiness = new TransportBusiness();                
                    if (transportBusiness.HasDepositPremium(companyTransport))
                    {
                        companyCoverage.PremiumAmount = companyCoverage.PremiumAmount * (companyCoverage.DepositPremiumPercent / 100);           
                    }
            }

            //deducibles
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
                //companyCoverage.EndorsementSublimitAmount = companyCoverage.SubLimitAmount;
            }
            //companyCoverage.EndorsementLimitAmount = companyCoverage.LimitAmount;
            return companyCoverage;
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
            TransportBusiness transportBusiness = new TransportBusiness();
            CompanyTransport companyTransport = transportBusiness.GetCompanyTransportTemporalByRiskId(riskId);
            companyTransport.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            CompanyCoverage companyCoverage = DelegateService.underwritingService.GetCompanyCoverageByRiskCoverageId(coverageId);

            companyCoverage.CoverStatus = CoverageStatusType.Excluded;
            companyCoverage.EndorsementType = companyTransport.Risk.Policy.Endorsement.EndorsementType;
            companyCoverage.CurrentFrom = companyTransport.Risk.Policy.CurrentFrom;
            companyCoverage.AccumulatedPremiumAmount = 0;
            companyCoverage.LimitAmount = 0;
            companyCoverage.SubLimitAmount = 0;
            companyCoverage.EndorsementLimitAmount = companyCoverage.EndorsementLimitAmount * -1;
            companyCoverage.EndorsementSublimitAmount = companyCoverage.EndorsementSublimitAmount * -1;

            companyCoverage = Quotate(companyTransport, companyCoverage, false, false);
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