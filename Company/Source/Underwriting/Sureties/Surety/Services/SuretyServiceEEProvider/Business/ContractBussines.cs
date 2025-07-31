using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Assemblers;
using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Entities.View;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.Linq;
using UNDEnums = Sistran.Core.Application.UnderwritingServices.Enums;
using Rules = Sistran.Core.Framework.Rules;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.DAF;
using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.RulesScriptsServices.Models;

namespace Sistran.Company.Application.Sureties.SuretyServices.EEProvider.BusinessModels
{
    public class ContractBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        public CompanyContract RunRulesRisk(CompanyContract companyContract, int ruleId)
        {
            if (!companyContract.Risk.Policy.IsPersisted)
            {
                companyContract.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyContract.Risk.Policy.Id, false);
            }

            return RunRules(companyContract, ruleId);
        }

        private CompanyContract RunRules(CompanyContract companyContract, int ruleId)
        {
            UnderwritingServices.Assembler.ModelAssembler.CreateFacadeGeneral(companyContract.Risk.Policy, Facade);
            EntityAssembler.CreateFacadeRiskContract(Facade, companyContract);

            Facade = RulesEngineDelegate.ExecuteRules(ruleId, Facade);

            ModelAssembler.CreateContract(companyContract, Facade);
            return companyContract;
        }

        public CompanyContract QuotateContract(CompanyContract companyContract, bool runRulesPre, bool runRulesPost)
        {
            bool updatePolicy = false;

            if (!companyContract.Risk.Policy.IsPersisted)
            {
                companyContract.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyContract.Risk.Policy.Id, false);
                updatePolicy = true;
            }

            if (runRulesPost && companyContract.Risk.Policy.Product.CoveredRisk.RuleSetId.GetValueOrDefault() > 0)
            {
                companyContract = RunRules(companyContract, companyContract.Risk.Policy.Product.CoveredRisk.RuleSetId.Value);
            }

            if (companyContract.Risk.Status == UNDEnums.RiskStatusType.Excluded)
            {
                companyContract.Risk.Coverages = companyContract.Risk.Coverages.Where(x => x.CoverStatus != UNDEnums.CoverageStatusType.Included).ToList();
                companyContract.Risk.Coverages.ForEach(x => x.CoverStatus = UNDEnums.CoverageStatusType.Excluded);
            }

            companyContract.Risk.Premium = 0;
            companyContract.Risk.AmountInsured = 0;
            List<CompanyCoverage> quotateCoverages = new List<CompanyCoverage>();

            foreach (CompanyCoverage coverage in companyContract.Risk.Coverages ?? quotateCoverages)
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                quotateCoverages.Add(coverageBusiness.Quotate(companyContract, coverage, runRulesPre, runRulesPost));
                coverageBusiness.UpdateFacadeConcepts(Facade);
            }

            companyContract.Risk.Coverages = quotateCoverages;

            //Eliminar Clausulas Poliza
            companyContract.Risk.Policy.Clauses = DelegateService.underwritingService.RemoveClauses(companyContract.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesRemove));

            //Agregar Clausulas Poliza
            companyContract.Risk.Policy.Clauses = DelegateService.underwritingService.AddClauses(companyContract.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesAdd));

            //Eliminar Clausulas Riesgo
            companyContract.Risk.Clauses = DelegateService.underwritingService.RemoveClauses(companyContract.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesRemove));

            //Agregar Clausulas Riesgo
            companyContract.Risk.Clauses = DelegateService.underwritingService.AddClauses(companyContract.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesAdd));

            //Eliminar Coberturas
            companyContract.Risk.Coverages = DelegateService.underwritingService.RemoveCoverages(companyContract.Risk.Coverages, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesRemove));

            //Agregar Coberturas
            if (Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd) != null)
            {
                foreach (int coverageId in Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd))
                {
                    if (!companyContract.Risk.Coverages.Exists(x => x.Id == coverageId))
                    {
                        CompanyCoverage quotateCoverage = new CompanyCoverage();
                        quotateCoverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, companyContract.Risk.Policy.Product.Id, companyContract.Risk.GroupCoverage.Id);

                        CoverageBusiness coverageBusiness = new CoverageBusiness();
                        companyContract.Risk.Coverages.Add(coverageBusiness.Quotate(companyContract, quotateCoverage, true, true));
                        coverageBusiness.UpdateFacadeConcepts(Facade);
                    }
                }
            }

            //Deducibles
            if (companyContract.Risk?.Coverages.Where(x => x.Deductible != null && x.Deductible.Id != -1)?.Count() > 0)
                companyContract.Risk.Coverages = DelegateService.underwritingService.GetDeductiblesByCompanyCoverages(companyContract.Risk.Coverages);

            foreach (CompanyCoverage coverage in companyContract.Risk.Coverages)
            {
                if (coverage.Deductible != null)
                {
                    DelegateService.underwritingService.CalculateCompanyPremiumDeductible(coverage);
                }
            }

            //Prima Mínima
            if (companyContract.Risk.Policy.CalculateMinPremium == true)
            {
                decimal minimumPremiumAmount = DelegateService.underwritingService.GetMinimumPremiumAmountByModelDynamicConcepts(companyContract.Risk.DynamicProperties);

                if (minimumPremiumAmount > 0)
                {
                    bool prorate = DelegateService.underwritingService.GetProrateMinimumPremiumByModelDynamicConcepts(companyContract.Risk.DynamicProperties);
                    companyContract.Risk.Coverages = DelegateService.underwritingService.CalculateMinimumPremiumRatePerCoverage(companyContract.Risk.Coverages, minimumPremiumAmount, prorate, false);
                }
            }
            //Prima Mínima

            companyContract.Risk.Premium = companyContract.Risk.Coverages.Sum(x => x.PremiumAmount);
            companyContract.Risk.AmountInsured = companyContract.Risk.Coverages.Sum(x => x.LimitAmount);

            //Number Risk Parameter
            companyContract.Risk.Number = 1;

            if (updatePolicy)
            {
                DelegateService.underwritingService.CreatePolicyTemporal(companyContract.Risk.Policy, false);
            }

            return companyContract;
        }

        public List<CompanyContract> QuotateContracts(CompanyPolicy companyPolicy, List<CompanyContract> companyPropertyRisks, bool runRulesPre, bool runRulesPost)
        {
            if (!companyPolicy.IsPersisted)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Id, false);
            }

            foreach (CompanyContract companyContract in companyPropertyRisks)
            {
                companyContract.Risk.Policy = companyPolicy;
                QuotateContract(companyContract, runRulesPre, runRulesPost);
            }

            return companyPropertyRisks;
        }
        public List<CompanyContract> GetCompanySuretiesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            List<CompanyContract> companyContracts = new List<CompanyContract>();

            switch (moduleType)
            {
                case ModuleType.Emission:
                    return companyContracts;
                case ModuleType.Claim:
                    return CompanyContractsClaim(endorsementId);
                default:
                    return companyContracts;
            }
        }
        private List<CompanyContract> CompanyContractsClaim(int endorsementId)
        {
            List<CompanyContract> companyContracts = new List<CompanyContract>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            RiskCompanySuretyView riskCompanySuretyView = new RiskCompanySuretyView();
            ViewBuilder builder = new ViewBuilder("RiskCompanySuretyView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, riskCompanySuretyView);

            if (riskCompanySuretyView.Risks.Count > 0)
            {
                List<ISSEN.Risk> risks = riskCompanySuretyView.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.Policy> policies = riskCompanySuretyView.Policies.Cast<ISSEN.Policy>().ToList();
                List<ISSEN.RiskSurety> riskSureties = riskCompanySuretyView.RiskSureties.Cast<ISSEN.RiskSurety>().ToList();
                List<ISSEN.Endorsement> endorsements = riskCompanySuretyView.Endorsements.Cast<ISSEN.Endorsement>().ToList();
                List<ISSEN.EndorsementRisk> endorsementRisks = riskCompanySuretyView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                foreach (ISSEN.Risk risk in risks)
                {
                    ISSEN.EndorsementRisk endorsementRisk = endorsementRisks.First(x => x.RiskId == risk.RiskId);
                    ISSEN.Policy entityPolicy = policies.First(x => x.PolicyId == endorsementRisk.PolicyId);
                    ContractDto contractDto = new ContractDto();
                    contractDto.Risk = risk;
                    contractDto.RiskSurety = riskSureties.FirstOrDefault(x => x.RiskId == risk.RiskId);
                    contractDto.EndorsementRisk = endorsementRisk;

                    CompanyContract companyContract = ModelAssembler.CreateContract(contractDto);

                    if (companyContract != null)
                    {
                        companyContract.Risk = new CompanyRisk
                        {
                            RiskId = risk.RiskId,
                            CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode,
                            GroupCoverage = new GroupCoverage
                            {
                                Id = risk.CoverGroupId.Value,
                                CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode
                            },
                            MainInsured = new CompanyIssuanceInsured
                            {
                                IndividualId = risk.InsuredId,
                                CompanyName = new IssuanceCompanyName
                                {
                                    NameNum = risk.NameNum.GetValueOrDefault(),
                                    Address = new IssuanceAddress
                                    {
                                        Id = risk.AddressId.GetValueOrDefault()
                                    }
                                }
                            },
                            Policy = new CompanyPolicy
                            {
                                DocumentNumber = entityPolicy.DocumentNumber,
                                Endorsement = new CompanyEndorsement { Id = endorsementRisk.EndorsementId, PolicyId = entityPolicy.PolicyId },
                            },
                            Number = endorsementRisk.RiskNum,
                            OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode,
                            Status = RiskStatusType.NotModified,
                            DynamicProperties = new List<DynamicConcept>()
                        };

                        companyContract.Risk.Id = risk.RiskId;
                        companyContract.Value = new Core.Application.CommonService.Models.Amount
                        {
                            Value = contractDto.RiskSurety.ContractAmount
                        };

                        companyContract.Risk.Policy = new CompanyPolicy()
                        {
                            Id = endorsementRisk.PolicyId,
                            DocumentNumber = policies.FirstOrDefault(X => X.PolicyId == endorsementRisk.PolicyId).DocumentNumber,
                            Endorsement =
                                new UnderwritingServices.CompanyEndorsement()
                                {
                                    Id = endorsementRisk.EndorsementId,
                                    Number = endorsements.FirstOrDefault(X => X.EndorsementId == endorsementRisk.EndorsementId).DocumentNum
                                }
                        };

                        ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                        filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                        filterSumAssured.Equal();
                        filterSumAssured.Constant(endorsementId);
                        CompanySuretiesSumAssuredView assuredView = new CompanySuretiesSumAssuredView();
                        ViewBuilder builderAssured = new ViewBuilder("SumAssuredView");
                        builderAssured.Filter = filterSumAssured.GetPredicate();
                        DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                        decimal insuredAmount = 0;

                        foreach (ISSEN.RiskCoverage entityRiskCoverage in assuredView.RiskCoverages)
                        {
                            insuredAmount += entityRiskCoverage.LimitAmount;
                        }

                        companyContract.Risk.AmountInsured = insuredAmount;

                        if (companyContract.Contractor != null)
                        {
                            IssuanceInsured ContractorIdentificacion = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(companyContract.Contractor.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                            companyContract.Contractor.Name = ContractorIdentificacion.Name;
                            companyContract.Contractor.IdentificationDocument = ContractorIdentificacion.IdentificationDocument;
                            companyContract.Risk.Id = endorsementRisk.RiskId;
                            companyContract.Risk.CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode;

                            companyContracts.Add(companyContract);
                        }
                    }
                }
            }

            return companyContracts;
        }

        public decimal GetPremiumAmtByPolicyIdRiskNum(int PolicyId, int RiskNum)
        {

            decimal PremiumAmt = 0;

            //ISS.ENDO_RISK_COVERAGE
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRiskCoverage.Properties.PolicyId, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filter.Equal();
            filter.Constant(PolicyId);
            filter.And();
            filter.Property(ISSEN.EndorsementRiskCoverage.Properties.RiskNum, typeof(ISSEN.EndorsementRiskCoverage).Name);
            filter.Equal();
            filter.Constant(RiskNum);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(ISSEN.EndorsementRiskCoverage), filter.GetPredicate());

            ObjectCriteriaBuilder filter2 = new ObjectCriteriaBuilder();
            filter2.Property(ISSEN.RiskCoverage.Properties.RiskCoverId, typeof(ISSEN.RiskCoverage).Name);
            filter2.In();
            filter2.ListValue();
            foreach (ISSEN.EndorsementRiskCoverage endorsementRiskCoverage in businessCollection)
            {
                filter2.Constant(endorsementRiskCoverage.RiskCoverId);
            }
            filter2.EndList();

            BusinessCollection businessCollection2 = DataFacadeManager.GetObjects(typeof(ISSEN.RiskCoverage), filter2.GetPredicate());

            foreach (ISSEN.RiskCoverage riskCoverage in businessCollection2)
            {
                PremiumAmt += riskCoverage.PremiumAmount;
            }

            return PremiumAmt;
        }

        public List<CompanyContract> GetCompanyRisksSuretyByInsuredId(int insuredId)
        {
            List<CompanyContract> companyContracts = new List<CompanyContract>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(insuredId);

            RiskCompanySuretyView view = new RiskCompanySuretyView();
            ViewBuilder builder = new ViewBuilder("RiskCompanySuretyView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Risks.Count > 0)
            {
                List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.Policy> policies = view.Policies.Cast<ISSEN.Policy>().ToList();
                List<ISSEN.RiskSurety> riskSureties = view.RiskSureties.Cast<ISSEN.RiskSurety>().ToList();
                List<ISSEN.Endorsement> endorsements = view.Endorsements.Cast<ISSEN.Endorsement>().ToList();
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                foreach (ISSEN.Risk risk in risks)
                {
                    ISSEN.EndorsementRisk endorsementRisk = endorsementRisks.First(x => x.RiskId == risk.RiskId);
                    ISSEN.Policy entityPolicy = policies.First(x => x.PolicyId == endorsementRisk.PolicyId);
                    ContractDto contractDto = new ContractDto();
                    contractDto.Risk = risk;
                    contractDto.RiskSurety = riskSureties.FirstOrDefault(x => x.RiskId == risk.RiskId);
                    contractDto.EndorsementRisk = endorsementRisk;

                    CompanyContract companyContract = ModelAssembler.CreateContract(contractDto);

                    if (companyContract != null)
                    {
                        companyContract.Risk = new CompanyRisk
                        {
                            RiskId = risk.RiskId,
                            CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode,
                            GroupCoverage = new GroupCoverage
                            {
                                Id = risk.CoverGroupId.Value,
                                CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode
                            },
                            MainInsured = new CompanyIssuanceInsured
                            {
                                IndividualId = risk.InsuredId,
                                CompanyName = new IssuanceCompanyName
                                {
                                    NameNum = risk.NameNum.GetValueOrDefault(),
                                    Address = new IssuanceAddress
                                    {
                                        Id = risk.AddressId.GetValueOrDefault()
                                    }
                                }
                            },
                            Policy = new CompanyPolicy
                            {
                                DocumentNumber = entityPolicy.DocumentNumber,
                                Endorsement = new CompanyEndorsement { Id = endorsementRisk.EndorsementId, PolicyId = entityPolicy.PolicyId },
                            },
                            Number = endorsementRisk.RiskNum,
                            OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode,
                            Status = RiskStatusType.NotModified,
                            DynamicProperties = new List<DynamicConcept>()
                        };

                        companyContract.Risk.Id = risk.RiskId;

                        companyContract.Risk.Policy = new CompanyPolicy()
                        {
                            Id = endorsementRisk.PolicyId,
                            DocumentNumber = policies.FirstOrDefault(X => X.PolicyId == endorsementRisk.PolicyId).DocumentNumber,
                            Endorsement =
                                new UnderwritingServices.CompanyEndorsement()
                                {
                                    Id = endorsementRisk.EndorsementId,
                                    Number = endorsements.FirstOrDefault(X => X.EndorsementId == endorsementRisk.EndorsementId).DocumentNum
                                }
                        };

                        if (companyContract.Contractor != null)
                        {
                            IssuanceInsured ContractorIdentificacion = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(companyContract.Contractor.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                            companyContract.Contractor.Name = ContractorIdentificacion.Name;
                            companyContract.Contractor.IdentificationDocument = ContractorIdentificacion.IdentificationDocument;
                            companyContract.Risk.Id = endorsementRisk.RiskId;
                            companyContract.Risk.CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode;

                            companyContracts.Add(companyContract);
                        }
                    }
                }
            }

            return companyContracts;
        }

        public List<CompanyContract> GetCompanyRisksSuretyBySuretyId(int suretyId)
        {
            List<CompanyContract> companyContracts = new List<CompanyContract>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.RiskSurety.Properties.IndividualId, typeof(ISSEN.RiskSurety).Name);
            filter.Equal();
            filter.Constant(suretyId);

            RiskCompanySuretyView view = new RiskCompanySuretyView();
            ViewBuilder builder = new ViewBuilder("RiskCompanySuretyView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Risks.Count > 0)
            {
                List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.Policy> policies = view.Policies.Cast<ISSEN.Policy>().ToList();
                List<ISSEN.RiskSurety> riskSureties = view.RiskSureties.Cast<ISSEN.RiskSurety>().ToList();
                List<ISSEN.Endorsement> endorsements = view.Endorsements.Cast<ISSEN.Endorsement>().ToList();
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                foreach (ISSEN.Risk entityRisk in risks)
                {
                    ISSEN.EndorsementRisk endorsementRisk = endorsementRisks.First(x => x.RiskId == entityRisk.RiskId);

                    ContractDto contractDto = new ContractDto();
                    contractDto.Risk = entityRisk;
                    contractDto.RiskSurety = riskSureties.FirstOrDefault(x => x.RiskId == entityRisk.RiskId);
                    contractDto.EndorsementRisk = endorsementRisk;

                    CompanyContract companyContract = ModelAssembler.CreateContract(contractDto);

                    if (companyContract != null)
                    {
                        companyContract.Risk = new CompanyRisk
                        {
                            RiskId = entityRisk.RiskId,
                            CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode,
                            GroupCoverage = new GroupCoverage
                            {
                                Id = entityRisk.CoverGroupId.Value,
                                CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode
                            },
                            MainInsured = new CompanyIssuanceInsured
                            {
                                IndividualId = entityRisk.InsuredId,
                                CompanyName = new IssuanceCompanyName
                                {
                                    NameNum = entityRisk.NameNum.GetValueOrDefault(),
                                    Address = new IssuanceAddress
                                    {
                                        Id = entityRisk.AddressId.GetValueOrDefault()
                                    }
                                }
                            },
                            Policy = new CompanyPolicy
                            {
                                DocumentNumber = policies.FirstOrDefault(x => x.PolicyId == endorsementRisk.PolicyId).DocumentNumber,
                                Endorsement = new CompanyEndorsement
                                {
                                    Id = endorsementRisk.EndorsementId,
                                    PolicyId = policies.FirstOrDefault(x => x.PolicyId == endorsementRisk.PolicyId).PolicyId
                                },
                            },
                            Number = endorsementRisk.RiskNum,
                            OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode,
                            Status = RiskStatusType.NotModified,
                            DynamicProperties = new List<DynamicConcept>()
                        };

                        companyContract.Risk.Policy = new CompanyPolicy()
                        {
                            Id = endorsementRisk.PolicyId,
                            DocumentNumber = policies.FirstOrDefault(X => X.PolicyId == endorsementRisk.PolicyId).DocumentNumber,
                            Endorsement = new CompanyEndorsement()
                            {
                                Id = endorsementRisk.EndorsementId,
                                Number = endorsements.FirstOrDefault(X => X.EndorsementId == endorsementRisk.EndorsementId).DocumentNum
                            }
                        };

                        if (companyContract.Contractor != null)
                        {
                            IssuanceInsured ContractorIdentificacion = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(companyContract.Contractor.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                            companyContract.Contractor.Name = ContractorIdentificacion.Name;
                            companyContract.Contractor.IdentificationDocument = ContractorIdentificacion.IdentificationDocument;
                            companyContract.Risk.Id = endorsementRisk.RiskId;
                            companyContract.Risk.CoveredRiskType = (Core.Application.CommonService.Enums.CoveredRiskType)entityRisk.CoveredRiskTypeCode;

                            companyContracts.Add(companyContract);
                        }
                    }
                }
            }

            return companyContracts;
        }

        public List<CompanyContract> GetCompanyRisksBySurety(string description)
        {
            List<CompanyContract> companyContracts = new List<CompanyContract>();

            List<IssuanceInsured> insureds = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, InsuredSearchType.DocumentNumber, CustomerType.Individual);

            if (insureds.Count > 0)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.RiskSurety.Properties.IndividualId, typeof(ISSEN.RiskSurety).Name);
                filter.In();
                filter.ListValue();
                insureds.ForEach(x => filter.Constant(x.IndividualId));
                filter.EndList();

                RiskCompanySuretyView view = new RiskCompanySuretyView();
                ViewBuilder builder = new ViewBuilder("RiskCompanySuretyView");
                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                if (view.Risks.Count > 0)
                {
                    List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                    List<ISSEN.Policy> policies = view.Policies.Cast<ISSEN.Policy>().ToList();
                    List<ISSEN.RiskSurety> riskSureties = view.RiskSureties.Cast<ISSEN.RiskSurety>().ToList();
                    List<ISSEN.Endorsement> endorsements = view.Endorsements.Cast<ISSEN.Endorsement>().ToList();
                    List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                    foreach (ISSEN.Risk entityRisk in risks)
                    {
                        ISSEN.EndorsementRisk endorsementRisk = endorsementRisks.First(x => x.RiskId == entityRisk.RiskId);
                        ISSEN.Policy entityPolicy = policies.First(x => x.PolicyId == endorsementRisk.PolicyId);
                        ContractDto contractDto = new ContractDto();
                        contractDto.Risk = entityRisk;
                        contractDto.RiskSurety = riskSureties.FirstOrDefault(x => x.RiskId == entityRisk.RiskId);
                        contractDto.EndorsementRisk = endorsementRisk;

                        CompanyContract companyContract = ModelAssembler.CreateContract(contractDto);

                        if (companyContract != null)
                        {
                            companyContract.Risk = new CompanyRisk
                            {
                                RiskId = entityRisk.RiskId,
                                CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode,
                                GroupCoverage = new GroupCoverage
                                {
                                    Id = entityRisk.CoverGroupId.Value,
                                    CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode
                                },
                                MainInsured = new CompanyIssuanceInsured
                                {
                                    IndividualId = entityRisk.InsuredId,
                                    CompanyName = new IssuanceCompanyName
                                    {
                                        NameNum = entityRisk.NameNum.GetValueOrDefault(),
                                        Address = new IssuanceAddress
                                        {
                                            Id = entityRisk.AddressId.GetValueOrDefault()
                                        }
                                    }
                                },
                                Policy = new CompanyPolicy
                                {
                                    DocumentNumber = entityPolicy.DocumentNumber,
                                    Endorsement = new CompanyEndorsement
                                    {
                                        Id = endorsementRisk.EndorsementId,
                                        PolicyId = entityPolicy.PolicyId
                                    },
                                },
                                Number = endorsementRisk.RiskNum,
                                OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode,
                                Status = RiskStatusType.NotModified,
                                DynamicProperties = new List<DynamicConcept>()
                            };

                            companyContract.Risk.Policy = new CompanyPolicy()
                            {
                                Id = endorsementRisk.PolicyId,
                                DocumentNumber = entityPolicy.DocumentNumber,
                                Endorsement =
                                new CompanyEndorsement()
                                {
                                    Id = endorsementRisk.EndorsementId,
                                    Number = endorsements.FirstOrDefault(X => X.EndorsementId == endorsementRisk.EndorsementId).DocumentNum
                                }
                            };

                            if (companyContract.Contractor != null)
                            {
                                IssuanceInsured contractor = insureds.FirstOrDefault(x => x.IndividualId == companyContract.Contractor.IndividualId);
                                companyContract.Contractor.Name = contractor.Name + (String.IsNullOrEmpty(contractor.Surname) ? "" : " " + contractor.Surname) + (String.IsNullOrEmpty(contractor.SecondSurname) ? "" : " " + contractor.SecondSurname);
                                companyContract.Contractor.IdentificationDocument = contractor.IdentificationDocument;
                                companyContract.Risk.Id = endorsementRisk.RiskId;
                                companyContract.Risk.CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode;

                                companyContracts.Add(companyContract);
                            }
                        }
                    }
                }
            }

            return companyContracts;
        }

        public CompanyContract GetCompanySuretyByRiskIdModuleType(int riskId, ModuleType moduleType)
        {
            CompanyContract companyContract = new CompanyContract();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.RiskId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(riskId);

            RiskCompanySuretyView riskCompanySuretyView = new RiskCompanySuretyView();
            ViewBuilder viewBuilder = new ViewBuilder("RiskCompanySuretyView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, riskCompanySuretyView);

            if (riskCompanySuretyView.Risks.Count > 0)
            {
                List<ISSEN.Risk> risks = riskCompanySuretyView.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.Policy> policies = riskCompanySuretyView.Policies.Cast<ISSEN.Policy>().ToList();
                List<ISSEN.RiskSurety> riskSureties = riskCompanySuretyView.RiskSureties.Cast<ISSEN.RiskSurety>().ToList();
                List<ISSEN.Endorsement> endorsements = riskCompanySuretyView.Endorsements.Cast<ISSEN.Endorsement>().ToList();
                List<ISSEN.EndorsementRisk> endorsementRisks = riskCompanySuretyView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();

                foreach (ISSEN.Risk risk in risks)
                {
                    ISSEN.EndorsementRisk endorsementRisk = endorsementRisks.First(x => x.RiskId == risk.RiskId);
                    ISSEN.Policy entityPolicy = policies.First(x => x.PolicyId == endorsementRisk.PolicyId);
                    ContractDto contractDto = new ContractDto();
                    contractDto.Risk = risk;
                    contractDto.RiskSurety = riskSureties.FirstOrDefault(x => x.RiskId == risk.RiskId);
                    contractDto.EndorsementRisk = endorsementRisk;

                    companyContract = ModelAssembler.CreateContract(contractDto);

                    if (companyContract != null)
                    {
                        companyContract.Risk = new CompanyRisk
                        {
                            RiskId = risk.RiskId,
                            CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode,
                            GroupCoverage = new GroupCoverage
                            {
                                Id = risk.CoverGroupId.Value,
                                CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode
                            },
                            MainInsured = new CompanyIssuanceInsured
                            {
                                IndividualId = risk.InsuredId,
                                CompanyName = new IssuanceCompanyName
                                {
                                    NameNum = risk.NameNum.GetValueOrDefault(),
                                    Address = new IssuanceAddress
                                    {
                                        Id = risk.AddressId.GetValueOrDefault()
                                    }
                                }
                            },
                            Policy = new CompanyPolicy
                            {
                                DocumentNumber = entityPolicy.DocumentNumber,
                                Endorsement = new CompanyEndorsement { Id = endorsementRisk.EndorsementId, PolicyId = entityPolicy.PolicyId },
                            },
                            Number = endorsementRisk.RiskNum,
                            OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode,
                            Status = RiskStatusType.NotModified,
                            DynamicProperties = new List<DynamicConcept>()
                        };

                        companyContract.Risk.Id = risk.RiskId;

                        companyContract.Risk.Policy = new CompanyPolicy()
                        {
                            Id = endorsementRisk.PolicyId,
                            DocumentNumber = policies.FirstOrDefault(X => X.PolicyId == endorsementRisk.PolicyId).DocumentNumber,
                            Endorsement =
                                new UnderwritingServices.CompanyEndorsement()
                                {
                                    Id = endorsementRisk.EndorsementId,
                                    Number = endorsements.FirstOrDefault(X => X.EndorsementId == endorsementRisk.EndorsementId).DocumentNum
                                }
                        };

                        if (companyContract.Contractor != null)
                        {
                            IssuanceInsured ContractorIdentificacion = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(companyContract.Contractor.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                            companyContract.Contractor.Name = ContractorIdentificacion.Name;
                            companyContract.Contractor.IdentificationDocument = ContractorIdentificacion.IdentificationDocument;
                            companyContract.Risk.Id = endorsementRisk.RiskId;
                            companyContract.Risk.CoveredRiskType = (Core.Application.CommonService.Enums.CoveredRiskType)risk.CoveredRiskTypeCode;
                        }
                    }
                }
            }

            return companyContract;
        }
    }
}