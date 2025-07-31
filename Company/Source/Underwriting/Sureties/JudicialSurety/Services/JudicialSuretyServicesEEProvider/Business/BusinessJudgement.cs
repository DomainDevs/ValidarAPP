using JudicialSuretyServicesEEProvider;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Assemblers;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.RulesEngine;
using System.Collections.Generic;
using System.Linq;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Services.UtilitiesServices.Enums;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using UPEN = Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Framework.Queries;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Entities.views;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.ModelsAutoMapper;

namespace Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Business
{
    public class JudgementBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        public CompanyJudgement RunRulesRisk(CompanyJudgement companyJudgement, int ruleId)
        {
            if (!companyJudgement.Risk.Policy.IsPersisted)
            {
                companyJudgement.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyJudgement.Risk.Policy.Id, false);
            }

            return RunRules(companyJudgement, ruleId);
        }

        private CompanyJudgement RunRules(CompanyJudgement companyJudgement, int ruleId)
        {
            UnderwritingServices.Assembler.ModelAssembler.CreateFacadeGeneral(companyJudgement.Risk.Policy, Facade);
            EntityAssembler.CreateFacadeRiskJudgement(Facade, companyJudgement);

            Facade = RulesEngineDelegate.ExecuteRules(ruleId, Facade);

            ModelAssembler.CreateJudgement(companyJudgement, Facade);
            return companyJudgement;
        }

        public CompanyJudgement QuotateJudgement(CompanyJudgement companyJudgement, bool runRulesPre, bool runRulesPost)
        {
            bool updatePolicy = false;

            if (!companyJudgement.Risk.Policy.IsPersisted)
            {
                companyJudgement.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyJudgement.Risk.Policy.Id, false);
                updatePolicy = true;
            }

            if (runRulesPost && companyJudgement.Risk.Policy.Product.CoveredRisk.RuleSetId.GetValueOrDefault() > 0)
            {
                companyJudgement = RunRules(companyJudgement, companyJudgement.Risk.Policy.Product.CoveredRisk.RuleSetId.Value);
            }

            if (companyJudgement.Risk.Status == RiskStatusType.Excluded)
            {
                companyJudgement.Risk.Coverages = companyJudgement.Risk.Coverages.Where(x => x.CoverStatus != CoverageStatusType.Included).ToList();
                companyJudgement.Risk.Coverages.ForEach(x => x.CoverStatus = CoverageStatusType.Excluded);
            }

            companyJudgement.Risk.Premium = 0;
            companyJudgement.Risk.AmountInsured = 0;
            List<CompanyCoverage> quotateCoverages = new List<CompanyCoverage>();

            foreach (CompanyCoverage coverage in companyJudgement.Risk.Coverages)
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                quotateCoverages.Add(coverageBusiness.Quotate(companyJudgement, coverage, runRulesPre, runRulesPost));
                coverageBusiness.UpdateFacadeConcepts(Facade);
            }

            companyJudgement.Risk.Coverages = quotateCoverages;

            //Eliminar Clausulas Poliza
            companyJudgement.Risk.Policy.Clauses = DelegateService.underwritingService.RemoveClauses(companyJudgement.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesRemove));

            //Agregar Clausulas Poliza
            companyJudgement.Risk.Policy.Clauses = DelegateService.underwritingService.AddClauses(companyJudgement.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesAdd));

            //Eliminar Clausulas Riesgo
            companyJudgement.Risk.Clauses = DelegateService.underwritingService.RemoveClauses(companyJudgement.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesRemove));

            //Agregar Clausulas Riesgo
            companyJudgement.Risk.Clauses = DelegateService.underwritingService.AddClauses(companyJudgement.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesAdd));

            //Eliminar Coberturas
            companyJudgement.Risk.Coverages = DelegateService.underwritingService.RemoveCoverages(companyJudgement.Risk.Coverages, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesRemove));

            //Agregar Coberturas
            if (Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd) != null)
            {
                foreach (int coverageId in Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd))
                {
                    if (!companyJudgement.Risk.Coverages.Exists(x => x.Id == coverageId))
                    {
                        CompanyCoverage quotateCoverage = new CompanyCoverage();
                        quotateCoverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, companyJudgement.Risk.Policy.Product.Id, companyJudgement.Risk.GroupCoverage.Id);

                        CoverageBusiness coverageBusiness = new CoverageBusiness();
                        companyJudgement.Risk.Coverages.Add(coverageBusiness.Quotate(companyJudgement, quotateCoverage, true, true));
                        coverageBusiness.UpdateFacadeConcepts(Facade);
                    }
                }
            }

            //Deducibles
            companyJudgement.Risk.Coverages = DelegateService.underwritingService.GetDeductiblesByCompanyCoverages(companyJudgement.Risk.Coverages);

            foreach (CompanyCoverage coverage in companyJudgement.Risk.Coverages)
            {
                if (coverage.Deductible != null)
                {
                    DelegateService.underwritingService.CalculateCompanyPremiumDeductible(coverage);
                }
            }

            //Prima Mínima
            if (companyJudgement.Risk.Policy.CalculateMinPremium == true)
            {
                decimal minimumPremiumAmount = DelegateService.underwritingService.GetMinimumPremiumAmountByModelDynamicConcepts(companyJudgement.Risk.DynamicProperties);

                if (minimumPremiumAmount > 0)
                {
                    bool prorate = DelegateService.underwritingService.GetProrateMinimumPremiumByModelDynamicConcepts(companyJudgement.Risk.DynamicProperties);
                    companyJudgement.Risk.Coverages = DelegateService.underwritingService.CalculateMinimumPremiumRatePerCoverage(companyJudgement.Risk.Coverages, minimumPremiumAmount, prorate, false);
                }
            }
            //Prima Mínima

            companyJudgement.Risk.Premium = companyJudgement.Risk.Coverages.Sum(x => x.PremiumAmount);
            companyJudgement.Risk.AmountInsured = companyJudgement.Risk.Coverages.Sum(x => x.LimitAmount);

            //Number Risk Parameter
            companyJudgement.Risk.Number = 1;

            if (updatePolicy)
            {
                DelegateService.underwritingService.CreatePolicyTemporal(companyJudgement.Risk.Policy, false);
            }

            return companyJudgement;
        }

        public List<CompanyJudgement> QuotateJudgements(CompanyPolicy companyPolicy, List<CompanyJudgement> companyPropertyRisks, bool runRulesPre, bool runRulesPost)
        {
            if (!companyPolicy.IsPersisted)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Id, false);
            }

            foreach (CompanyJudgement companyJudgement in companyPropertyRisks)
            {
                companyJudgement.Risk.Policy = companyPolicy;
                QuotateJudgement(companyJudgement, runRulesPre, runRulesPost);
            }

            return companyPropertyRisks;
        }

        public List<CompanyJudgement> GetCompanyJudicialSuretiesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            List<CompanyJudgement> companyJudgements = new List<CompanyJudgement>();
            switch (moduleType)
            {
                case ModuleType.Emission:
                    return companyJudgements;
                case ModuleType.Claim:

                    List<ISSEN.Risk> risks = new List<ISSEN.Risk>();
                    List<ISSEN.EndorsementRisk> endorsementRisks = new List<ISSEN.EndorsementRisk>();
                    List<ISSEN.RiskJudicialSurety> riskJudicialSureties = new List<ISSEN.RiskJudicialSurety>();
                    List<ISSEN.Endorsement> endorsements = new List<ISSEN.Endorsement>();
                    List<UPEN.Insured> insureds = new List<UPEN.Insured>();

                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                    filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
                    filter.Equal();
                    filter.Constant(endorsementId);

                    JudicialSuretyView view = new JudicialSuretyView();
                    ViewBuilder builder = new ViewBuilder("JudicialSuretyView");
                    builder.Filter = filter.GetPredicate();

                    DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                    if (view.Risks.Count > 0)
                    {
                        risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                        endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                        riskJudicialSureties = view.RiskJudicialSurety.Cast<ISSEN.RiskJudicialSurety>().ToList();
                        endorsements = view.Endorsement.Cast<ISSEN.Endorsement>().ToList();

                        foreach (ISSEN.Risk risk in risks)
                        {
                            ISSEN.EndorsementRisk endorsementRisk = endorsementRisks.Where(x => x.RiskId == risk.RiskId).FirstOrDefault();
                            ISSEN.RiskJudicialSurety riskJudicialSurety = riskJudicialSureties.First(x => x.RiskId == risk.RiskId);
                            ISSEN.Endorsement endorsement = endorsements.First(x => x.EndorsementId == endorsementRisk.EndorsementId);

                            CompanyJudgementMapper companyJudgementMapper = new CompanyJudgementMapper();
                            companyJudgementMapper.risk = risk;
                            companyJudgementMapper.RiskJudicialSurety = riskJudicialSurety;
                            companyJudgementMapper.endorsementRisk = endorsementRisk;
                            companyJudgementMapper.endorsement = endorsement;
                            CompanyJudgement companyJudgement = ModelAssembler.CreateJudicialSurety(companyJudgementMapper);

                            if (companyJudgement.Risk.MainInsured.IndividualId != 0)
                            {
                                IssuanceInsured insured = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(companyJudgement.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                                companyJudgement.Risk.MainInsured.Name = insured.Name;
                                companyJudgement.Risk.MainInsured.IdentificationDocument = insured.IdentificationDocument;
                            }

                            companyJudgements.Add(companyJudgement);
                        }
                    }

                    return companyJudgements;
                default:
                    return companyJudgements;
            }
        }

        public CompanyJudgement GetCompanyJudicialSuretyByRiskIdModuleType(int riskId, ModuleType moduleType)
        {
            CompanyJudgement companyJudgement = new CompanyJudgement();
            switch (moduleType)
            {
                case ModuleType.Emission:
                    return null;
                case ModuleType.Claim:
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                    filter.Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name);
                    filter.Equal();
                    filter.Constant(riskId);

                    JudicialSuretyView view = new JudicialSuretyView();
                    ViewBuilder builder = new ViewBuilder("JudicialSuretyView");
                    builder.Filter = filter.GetPredicate();

                    DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                    if (view.Risks.Count > 0)
                    {
                        ISSEN.Risk entityRisk = view.Risks.Cast<ISSEN.Risk>().FirstOrDefault();
                        ISSEN.EndorsementRisk entityEndorsementRisk = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().Where(x => x.RiskId == entityRisk.RiskId).FirstOrDefault();
                        ISSEN.RiskJudicialSurety entityRiskJudicialSurety = view.RiskJudicialSurety.Cast<ISSEN.RiskJudicialSurety>().First(x => x.RiskId == entityRisk.RiskId);
                        ISSEN.Endorsement entityEndorsement = view.Endorsement.Cast<ISSEN.Endorsement>().First(x => x.EndorsementId == entityEndorsementRisk.EndorsementId);

                        CompanyJudgementMapper companyJudgementMapper = new CompanyJudgementMapper();
                        companyJudgementMapper.risk = entityRisk;
                        companyJudgementMapper.RiskJudicialSurety = entityRiskJudicialSurety;
                        companyJudgementMapper.endorsementRisk = entityEndorsementRisk;
                        companyJudgementMapper.endorsement = entityEndorsement;
                        companyJudgement = ModelAssembler.CreateJudicialSurety(companyJudgementMapper);

                        if (companyJudgement.Risk.MainInsured.IndividualId != 0)
                        {
                            IssuanceInsured insured = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(companyJudgement.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                            companyJudgement.Risk.MainInsured.Name = insured.Name;
                            companyJudgement.Risk.MainInsured.IdentificationDocument = insured.IdentificationDocument;
                        }
                    }

                    return companyJudgement;
            }

            return null;
        }
    }
}