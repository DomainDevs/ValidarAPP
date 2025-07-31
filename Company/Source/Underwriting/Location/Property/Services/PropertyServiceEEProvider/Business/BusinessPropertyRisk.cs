using Sistran.Company.Application.Location.PropertyServices.EEProvider.Assemblers;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Rules = Sistran.Core.Framework.Rules;
using System.Collections.Generic;
using System.Linq;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using System.Threading.Tasks;
using Enums = Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Company.Application.Location.PropertyServices.EEProvider.DAOs;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Company.Application.Location.PropertyServices.EEProvider.Entities.View;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Company.Application.Location.PropertyServices.EEProvider.Resources;
using System;
using Sistran.Core.Application.UnderwritingServices.Models;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.Location.PropertyServices.Enum;
using Sistran.Company.Application.Location.PropertyServices.DTO;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.PropertyServices.EEProvider.Business
{
    public class PropertyRiskBusiness
    {
        Rules.Facade Facade = new Rules.Facade();

        /// <summary>
        /// 
        /// </summary>
        public CompanyPropertyRisk RunRulesRisk(CompanyPropertyRisk companyPropertyRisk, int ruleId)
        {
            if (!companyPropertyRisk.Risk.Policy.IsPersisted)
            {
                companyPropertyRisk.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPropertyRisk.Risk.Policy.Id, false);
            }

            return RunRules(companyPropertyRisk, ruleId);
        }

        /// <summary>
        /// 
        /// </summary>
        private CompanyPropertyRisk RunRules(CompanyPropertyRisk companyPropertyRisk, int ruleId)
        {
            UnderwritingServices.Assembler.ModelAssembler.CreateFacadeGeneral(companyPropertyRisk.Risk.Policy, Facade);
            EntityAssembler.CreateFacadeRiskProperty(Facade, companyPropertyRisk);

            Facade = RulesEngineDelegate.ExecuteRules(ruleId, Facade);

            ModelAssembler.CreatePropertyRisk(companyPropertyRisk, Facade);
            return companyPropertyRisk;
        }

        /// <summary>
        /// 
        /// </summary>
        public CompanyPropertyRisk QuotateProperty(CompanyPropertyRisk companyPropertyRisk, bool runRulesPre, bool runRulesPost)
        {
            bool updatePolicy = false;

            if (!companyPropertyRisk.Risk.Policy.IsPersisted)
            {
                companyPropertyRisk.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPropertyRisk.Risk.Policy.Id, false);
                updatePolicy = true;
            }

            if (runRulesPost && companyPropertyRisk.Risk.Policy.Product.CoveredRisk.RuleSetId.GetValueOrDefault() > 0)
            {
                companyPropertyRisk = RunRules(companyPropertyRisk, companyPropertyRisk.Risk.Policy.Product.CoveredRisk.RuleSetId.Value);
            }

            if (companyPropertyRisk.Risk.Status == Enums.RiskStatusType.Excluded)
            {
                companyPropertyRisk.Risk.Coverages = companyPropertyRisk.Risk.Coverages.Where(x => x.CoverStatus != Enums.CoverageStatusType.Included).ToList();
                companyPropertyRisk.Risk.Coverages.ForEach(x => x.CoverStatus = Enums.CoverageStatusType.Excluded);
            }

            companyPropertyRisk.Risk.Premium = 0;
            companyPropertyRisk.Risk.AmountInsured = 0;
            List<CompanyCoverage> quotateCoverages = new List<CompanyCoverage>();

            foreach (CompanyCoverage coverage in companyPropertyRisk.Risk.Coverages)
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                quotateCoverages.Add(coverageBusiness.Quotate(companyPropertyRisk, coverage, runRulesPre, runRulesPost));
                coverageBusiness.UpdateFacadeConcepts(Facade);
            }

            companyPropertyRisk.Risk.Coverages = quotateCoverages;

            //Eliminar Clausulas Poliza
            companyPropertyRisk.Risk.Policy.Clauses = DelegateService.underwritingService.RemoveClauses(companyPropertyRisk.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesRemove));

            //Agregar Clausulas Poliza
            companyPropertyRisk.Risk.Policy.Clauses = DelegateService.underwritingService.AddClauses(companyPropertyRisk.Risk.Policy.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptGeneral.ClausesAdd));

            //Eliminar Clausulas Riesgo
            companyPropertyRisk.Risk.Clauses = DelegateService.underwritingService.RemoveClauses(companyPropertyRisk.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesRemove));

            //Agregar Clausulas Riesgo
            companyPropertyRisk.Risk.Clauses = DelegateService.underwritingService.AddClauses(companyPropertyRisk.Risk.Clauses, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.ClausesAdd));

            //Eliminar Coberturas
            companyPropertyRisk.Risk.Coverages = DelegateService.underwritingService.RemoveCoverages(companyPropertyRisk.Risk.Coverages, Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesRemove));

            //Agregar Coberturas
            if (Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd) != null)
            {
                foreach (int coverageId in Facade.GetConcept<List<int>>(CompanyRuleConceptRisk.CoveragesAdd))
                {
                    if (!companyPropertyRisk.Risk.Coverages.Exists(x => x.Id == coverageId))
                    {
                        CompanyCoverage quotateCoverage = new CompanyCoverage();
                        quotateCoverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, companyPropertyRisk.Risk.Policy.Product.Id, companyPropertyRisk.Risk.GroupCoverage.Id);

                        CoverageBusiness coverageBusiness = new CoverageBusiness();
                        companyPropertyRisk.Risk.Coverages.Add(coverageBusiness.Quotate(companyPropertyRisk, quotateCoverage, true, true));
                        coverageBusiness.UpdateFacadeConcepts(Facade);
                    }
                }
            }

            //Deducibles
            companyPropertyRisk.Risk.Coverages = DelegateService.underwritingService.GetDeductiblesByCompanyCoverages(companyPropertyRisk.Risk.Coverages);

            foreach (CompanyCoverage coverage in companyPropertyRisk.Risk.Coverages)
            {
                if (coverage.Deductible != null)
                {
                    DelegateService.underwritingService.CalculateCompanyPremiumDeductible(coverage);
                }
            }

            //Prima Mínima
            if (companyPropertyRisk.Risk.Policy.CalculateMinPremium == true)
            {
                decimal minimumPremiumAmount = DelegateService.underwritingService.GetMinimumPremiumAmountByModelDynamicConcepts(companyPropertyRisk.Risk.DynamicProperties);

                if (minimumPremiumAmount > 0)
                {
                    bool prorate = DelegateService.underwritingService.GetProrateMinimumPremiumByModelDynamicConcepts(companyPropertyRisk.Risk.DynamicProperties);
                    companyPropertyRisk.Risk.Coverages = DelegateService.underwritingService.CalculateMinimumPremiumRatePerCoverage(companyPropertyRisk.Risk.Coverages, minimumPremiumAmount, prorate, false);
                }
            }
            //Prima Mínima

            companyPropertyRisk.Risk.Premium = companyPropertyRisk.Risk.Coverages.Sum(x => x.PremiumAmount);
            companyPropertyRisk.Risk.AmountInsured = companyPropertyRisk.Risk.Coverages.Sum(x => x.LimitAmount);

            if (updatePolicy)
            {
                DelegateService.underwritingService.CreatePolicyTemporal(companyPropertyRisk.Risk.Policy, false);
            }

            return companyPropertyRisk;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyPropertyRisk> QuotateProperties(CompanyPolicy companyPolicy, List<CompanyPropertyRisk> companyPropertyRisks, bool runRulesPre, bool runRulesPost)
        {
            if (!companyPolicy.IsPersisted)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Id, false);
            }

            foreach (CompanyPropertyRisk companyPropertyRisk in companyPropertyRisks)
            {
                companyPropertyRisk.Risk.Policy = companyPolicy;
                QuotateProperty(companyPropertyRisk, runRulesPre, runRulesPost);
            }

            return companyPropertyRisks;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyEndorsement> GetCompanyEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int policyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Endorsement.Properties.EndoTypeCode, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(endorsementTypeId);
            filter.And();
            filter.Property(ISSEN.Endorsement.Properties.PolicyId, typeof(ISSEN.Endorsement).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Not();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.In();
            filter.ListValue();
            filter.Constant((int)RiskStatusType.Excluded);
            filter.Constant((int)RiskStatusType.Cancelled);
            filter.EndList();

            RiskPropertyViewR1 viewr = new RiskPropertyViewR1();

            ViewBuilder builder = new ViewBuilder("RiskPropertyviewR1");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, viewr);


            List<ISSEN.Endorsement> entityEndorsements = viewr.Endorsements.Cast<ISSEN.Endorsement>().ToList();
            List<CompanyEndorsement> companyEndorsements = new List<CompanyEndorsement>();
            if (viewr.Endorsements.Count > 0)
            {
                if (entityEndorsements == null || entityEndorsements.Count < 1)
                {
                    throw new ArgumentException(Errors.ErrorEndorsementTypeIdEmpty);
                }
                try
                {
                    companyEndorsements = ModelAssembler.CreateCompanyEndorsements(entityEndorsements);
                }
                catch (Exception)
                {
                    throw new ArgumentException(Errors.ErrorEndorsementTypeIdEmpty);
                }
            }
            return companyEndorsements;
        }

        /// <summary>
        /// 
        /// </summary>
        public EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId)
        {

            Endorsement endorsement = DelegateService.endorsementBaseService.GetTemporalEndorsementByPolicyId(policyId);
            return DTOAssembler.CreateEndorsementDTO(endorsement);
        }

        /// <summary>
        /// 
        /// </summary>
        public CompanyEndorsement GetNextAdjustmentEndorsementByPolicyId(int policyId)
        {
            PropertyDAO propertyDAO = new PropertyDAO();
            CompanyPropertyRisk companyPropertyRisk = propertyDAO.GetCompanyPropertiesByPolicyId(policyId).FirstOrDefault();
            List<CompanyEndorsement> endorsements = GetEndorsements(companyPropertyRisk.Risk.Policy);

            var currentFrom = GetCurrentFromForNextAdjustment(endorsements);
            int months = GetMothsByAdjustmentPeriod(companyPropertyRisk.BillingPeriodDepositPremium);

            return new CompanyEndorsement
            {
                CurrentFrom = currentFrom,
                CurrentTo = currentFrom.AddMonths(months),
                EndorsementType = EndorsementType.AdjustmentEndorsement
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyPropertyRisk> GetCompanyPropertyRiskByPolicyId(int policyId)
        {
            List<CompanyPropertyRisk> companyPropertyRisks = new List<CompanyPropertyRisk>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementOperation.Properties.RiskNumber, typeof(ISSEN.EndorsementOperation).Name);
            filter.IsNotNull();
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);
            filter.And();
            filter.Not();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.In();
            filter.ListValue();
            filter.Constant((int)RiskStatusType.Excluded);
            filter.Constant((int)RiskStatusType.Cancelled);
            filter.EndList();

            //filter.EndList();


            RiskPropertyViewR1 view = new RiskPropertyViewR1();
            ViewBuilder builder = new ViewBuilder("RiskPropertyviewR1");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.EndorsementOperations.Count > 0)
            {
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.RiskCoverage> RiskCoverage = view.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList();

                foreach (ISSEN.EndorsementOperation entityEndorsementOperation in view.EndorsementOperations)
                {
                    CompanyPropertyRisk companyPropertyRisk = new CompanyPropertyRisk();
                    companyPropertyRisk = JsonConvert.DeserializeObject<CompanyPropertyRisk>(entityEndorsementOperation.Operation);

                    companyPropertyRisk.Risk.Id = 0;
                    companyPropertyRisk.Risk.RiskId = entityEndorsementRisks.First(x => x.RiskNum == entityEndorsementOperation.RiskNumber).RiskId;

                    companyPropertyRisk.Risk.OriginalStatus = companyPropertyRisk.Risk.Status;
                    companyPropertyRisk.Risk.Status = RiskStatusType.NotModified;

                    companyPropertyRisk.Risk.Coverages.ForEach(x => x.CoverageOriginalStatus = x.CoverStatus);
                    companyPropertyRisk.Risk.Coverages.ForEach(x => x.CoverStatus = CoverageStatusType.NotModified);

                    companyPropertyRisk.Risk.Beneficiaries.ForEach(x => x.CustomerType = (Core.Services.UtilitiesServices.Enums.CustomerType)CustomerType.Individual);

                    foreach (ISSEN.RiskCoverage riskCoverages in view.RiskCoverages)
                    {
                        companyPropertyRisk.Risk.Coverages.ForEach(x => x.RiskCoverageId = x.RiskCoverageId);
                    }


                    companyPropertyRisks.Add(companyPropertyRisk);
                }
            }
            else
            {
                ObjectCriteriaBuilder filterR1 = new ObjectCriteriaBuilder();
                filterR1.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
                filterR1.Equal();
                filterR1.Constant(policyId);
                filterR1.And();
                filterR1.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
                filterR1.Equal();
                filterR1.Constant(true);
                filterR1.And();
                filterR1.Not();
                filterR1.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
                filterR1.In();
                filterR1.ListValue();
                filterR1.Constant((int)RiskStatusType.Excluded);
                filterR1.Constant((int)RiskStatusType.Cancelled);
                filterR1.EndList();

                RiskPropertyViewR1 viewr1 = new RiskPropertyViewR1();
                ViewBuilder builderR1 = new ViewBuilder("RiskPropertyviewR1");
                builderR1.Filter = filterR1.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builderR1, viewr1);

                List<ISSEN.Risk> risks = viewr1.CompanyPropertyRisk.Cast<ISSEN.Risk>().ToList();
                companyPropertyRisks.AddRange(GetRisks(policyId, risks, viewr1));
            }

            return companyPropertyRisks;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyPropertyRisk> GetRisks(int policyId, List<ISSEN.Risk> risks, RiskPropertyViewR1 viewR1)
        {
            if (risks == null || risks.Count < 1 || viewR1.CompanyPropertyRisk == null || viewR1.CompanyPropertyRisk.Count < 1)
            {
                throw new ArgumentException(Errors.ErrorRiskEmpty);
            }
            try
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = viewR1.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.RiskLocation> riskLocation = viewR1.RiskLocation.Cast<ISSEN.RiskLocation>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = viewR1.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<ISSEN.RiskPayer> RiskPayers = viewR1.RiskPayers.Cast<ISSEN.RiskPayer>().ToList();
                List<ISSEN.RiskClause> riskClauses = viewR1.RiskClause.Cast<ISSEN.RiskClause>().ToList();
                //List<ISSEN.TransportCargoType> riskCargoType = viewR1.TransportCargoTypes.Cast<ISSEN.TransportCargoType>().ToList();
                //List<ISSEN.TransportPackagingType> riskPackagin = viewR1.TransportPackagingTypes.Cast<ISSEN.TransportPackagingType>().ToList();
                //List<ISSEN.TransportViaType> riskVia = viewR1.TransportViaTypes.Cast<ISSEN.TransportViaType>().ToList();
                List<CompanyPropertyRisk> propertyRisks = new List<CompanyPropertyRisk>();
                foreach (ISSEN.Risk item in risks)
                {
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.LoadDynamicProperties(item);
                    }
                    CompanyPropertyRisk propertyRisk = new CompanyPropertyRisk();
                    propertyRisk = ModelAssembler.CreatePropertyRisk(item,
                    riskLocation.First(x => x.RiskId == item.RiskId),
                    endorsementRisks.First(x => x.RiskId == item.RiskId));
                    propertyRisk.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(propertyRisk.Risk.MainInsured.IndividualId.ToString(), Core.Services.UtilitiesServices.Enums.InsuredSearchType.IndividualId, Core.Services.UtilitiesServices.Enums.CustomerType.Individual);// Mapper.Map<CompanyInsured, CompanyIssuanceInsured>(companyInsured);
                    propertyRisk.Risk.MainInsured.Name = propertyRisk.Risk.MainInsured.Name + " " + propertyRisk.Risk.MainInsured.Surname + " " + propertyRisk.Risk.MainInsured.SecondSurname;
                    ConcurrentBag<CompanyClause> clauses = new ConcurrentBag<CompanyClause>();
                    //clausulas
                    if (riskClauses != null && riskClauses.Count > 0)
                    {
                        TP.Parallel.ForEach(riskClauses.Where(x => x.RiskId == item.RiskId).ToList(), riskClause =>
                        {
                            clauses.Add(new CompanyClause { Id = riskClause.ClauseId });
                        });
                        propertyRisk.Risk.Clauses = clauses.ToList();
                    }
                    var companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(propertyRisk.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
                    propertyRisk.Risk.MainInsured.CompanyName = new IssuanceCompanyName
                    {
                        NameNum = companyName.NameNum,
                        TradeName = companyName.TradeName,
                        Address = new IssuanceAddress
                        {
                            Id = companyName.Address.Id,
                            Description = companyName.Address.Description,
                            City = companyName.Address.City
                        },
                        Phone = new IssuancePhone
                        {
                            Id = companyName.Phone.Id,
                            Description = companyName.Phone.Description
                        }
                    };
                    if (riskBeneficiaries != null && riskBeneficiaries.Count > 0)
                    {
                        propertyRisk.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                        Object objlock = new object();
                        var imapper = ModelAssembler.CreateMapCompanyBeneficiary();
                        TP.Parallel.ForEach(riskBeneficiaries.Where(x => x.RiskId == item.RiskId), riskBeneficiary =>
                        {
                            CompanyBeneficiary CiaBeneficiary = new CompanyBeneficiary();
                            var beneficiaryRisk = DelegateService.underwritingService.GetBeneficiariesByDescription(riskBeneficiary.BeneficiaryId.ToString(), (Core.Services.UtilitiesServices.Enums.InsuredSearchType)InsuredSearchType.IndividualId).FirstOrDefault();
                            if (beneficiaryRisk != null)
                            {
                                CiaBeneficiary = imapper.Map<Beneficiary, CompanyBeneficiary>(beneficiaryRisk);
                                CiaBeneficiary.CustomerType = (Core.Services.UtilitiesServices.Enums.CustomerType)CustomerType.Individual;
                                CiaBeneficiary.BeneficiaryType = new CompanyBeneficiaryType { Id = riskBeneficiary.BeneficiaryTypeCode };
                                CiaBeneficiary.Participation = riskBeneficiary.BenefitPercentage;
                                List<CompanyName> companyNames = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(CiaBeneficiary.IndividualId, (CustomerType)CiaBeneficiary.CustomerType);
                                companyName = new CompanyName();
                                if (companyNames.Exists(x => x.NameNum == 0 && x.IsMain))
                                {
                                    companyName = companyNames.First(x => x.IsMain);
                                }
                                else
                                {
                                    companyName = companyNames.First();
                                }
                                CiaBeneficiary.CompanyName = new IssuanceCompanyName
                                {
                                    NameNum = companyName.NameNum,
                                    TradeName = companyName.TradeName,
                                    Address = new IssuanceAddress
                                    {
                                        Id = companyName.Address.Id,
                                        Description = companyName.Address.Description,
                                        City = companyName.Address.City
                                    },
                                    Phone = new IssuancePhone
                                    {
                                        Id = companyName.Phone.Id,
                                        Description = companyName.Phone.Description
                                    },
                                    Email = new IssuanceEmail
                                    {
                                        Id = companyName.Email.Id,
                                        Description = companyName.Email.Description
                                    }
                                };
                                lock (objlock)
                                {
                                    propertyRisk.Risk.Beneficiaries.Add(CiaBeneficiary);
                                }
                            }
                        });
                    }

                    //coberturas
                    List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();

                    companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementRisks.First(x => x.RiskId == item.RiskId).EndorsementId, item.RiskId);
                    propertyRisk.Risk.Coverages = companyCoverages;
                    propertyRisks.Add(propertyRisk);
                }
                return propertyRisks;
            }

            catch (Exception ex)
            {

                throw new BusinessException(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyEndorsement> GetEndorsements(CompanyPolicy companyPolicy)
        {
            return DelegateService.underwritingService.GetCoPolicyEndorsementsByPrefixIdBranchIdPolicyNumber(
                   companyPolicy.Prefix.Id,
                   companyPolicy.Branch.Id,
                   companyPolicy.DocumentNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        private DateTime GetCurrentFromForNextAdjustment(List<CompanyEndorsement> endorsements)
        {
            endorsements = endorsements.OrderByDescending(x => x.Id).ToList();

            try
            {
                return endorsements.Where(x => EndorsementType.AdjustmentEndorsement == x.EndorsementType.Value).First().CurrentTo.Date;
            }
            catch (Exception)
            {
            }
            return endorsements.Last().CurrentFrom.Date;
        }

        /// <summary>
        /// 
        /// </summary>
        public int GetMothsByAdjustmentPeriod(int adjustmentId)
        {
            EnumsAdjustmentPeriod adjustmentPeriod = (EnumsAdjustmentPeriod)adjustmentId;

            switch (adjustmentPeriod)
            {
                case EnumsAdjustmentPeriod.Monthly:
                    return 1;
                case EnumsAdjustmentPeriod.BiMonthly:
                    return 2;
                case EnumsAdjustmentPeriod.Quarterly:
                    return 3;
                case EnumsAdjustmentPeriod.FourMonths:
                    return 4;
                case EnumsAdjustmentPeriod.Biannual:
                    return 6;
                case EnumsAdjustmentPeriod.Annual:
                    return 12;
            }
            return 1;
        }

        /// <summary>
        /// Genera el próximo endoso de declaración para una póliza
        /// </summary>
        /// <param name="policyId">Identificador de una póliza</param>
        /// <returns>Endoso de declaración</returns>

        public CompanyEndorsement GetNextDeclarationEndorsementByPolicyId(int policyId)
        {
            PropertyDAO propertyDAO = new PropertyDAO();
            CompanyPropertyRisk companyPropertyRisk = propertyDAO.GetCompanyPropertiesByPolicyId(policyId).FirstOrDefault();
            List<CompanyEndorsement> endorsements = GetEndorsements(companyPropertyRisk.Risk.Policy);

            var currentFrom = GetCurrentFromForNextDeclaration(endorsements);
            int months = GetMothsByDeclarationPeriod(companyPropertyRisk.DeclarationPeriod.Id);

            return new CompanyEndorsement
            {
                CurrentFrom = currentFrom,
                CurrentTo = currentFrom.AddMonths(months),
                EndorsementType = EndorsementType.DeclarationEndorsement
            };
        }

        /// <summary>
        /// Determina la fecha de inicio de vigencia para el próximo endoso de declaración
        /// </summary>
        /// <param name="endorsements">Listado de endosos</param>
        /// <returns>Fecha de inicio de vigencia</returns>
        private DateTime GetCurrentFromForNextDeclaration(List<CompanyEndorsement> endorsements)
        {
            endorsements = endorsements.OrderByDescending(x => x.Id).ToList();

            foreach (var endorsement in endorsements)
            {
                if (endorsement.EndorsementType.Value == EndorsementType.DeclarationEndorsement
                    || endorsement.EndorsementType.Value == EndorsementType.AdjustmentEndorsement)
                {
                    return endorsement.CurrentTo.Date;
                }
            }
            return endorsements.Last().CurrentFrom.Date;

        }

        /// <summary>
        /// Obtiene la cantidad de meses que tiene un periodo de declaración
        /// </summary>
        /// <param name="declarationId">Identificador del periodo de declaración</param>
        /// <returns>Canitdad de meses</returns>
        public int GetMothsByDeclarationPeriod(int declarationId)
        {
            EnumsDeclarationPeriod declarationPeriod = (EnumsDeclarationPeriod)declarationId;

            switch (declarationPeriod)
            {
                case EnumsDeclarationPeriod.Monthly:
                    return 1;
                case EnumsDeclarationPeriod.BiMonthly:
                    return 2;
                case EnumsDeclarationPeriod.Quarterly:
                    return 3;
                case EnumsDeclarationPeriod.FourMonths:
                    return 4;
                case EnumsDeclarationPeriod.Biannual:
                    return 6;
                case EnumsDeclarationPeriod.Annual:
                    return 12;
            }
            return 1;
        }

        /// <summary>
        /// Valida si en la poliza actual se pueden realizar endosos de declaracion
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public bool CanMakeDeclarationEndorsement(int policyId)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                CompanyPropertyRisk companyPropertyRisk = propertyDAO.GetCompanyPropertiesByPolicyId(policyId).FirstOrDefault();
                List<CompanyEndorsement> endorsements = GetEndorsements(companyPropertyRisk.Risk.Policy);

                var quantityofDeclaration = GetNumberofDeclarationsExpected(companyPropertyRisk.BillingPeriodDepositPremium, companyPropertyRisk.DeclarationPeriod.Id);
                List<CompanyEndorsement> declarationEndorsements = GetLastDeclarationEndorsements(endorsements);

                if (declarationEndorsements == null)
                {
                    return false;
                }
                return declarationEndorsements.Count < quantityofDeclaration;
            }
            catch (Exception)
            {
                throw new BusinessException("Error al consultar los endosos de declaración");
            }
        }

        /// <summary>
        /// Valida si en la poliza actual se pueden realizar endosos de ajuste 
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public bool CanMakeAdjustmentEndorsement(int policyId)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                CompanyPropertyRisk companyPropertyRisk = propertyDAO.GetCompanyPropertiesByPolicyId(policyId).FirstOrDefault();
                List<CompanyEndorsement> endorsements = GetEndorsements(companyPropertyRisk.Risk.Policy);

                if (endorsements.OrderByDescending(x => x.Id).FirstOrDefault().EndorsementType == EndorsementType.AdjustmentEndorsement)
                    return false;

                var quantityOfDeclarations = GetNumberofDeclarationsExpected(companyPropertyRisk.BillingPeriodDepositPremium, companyPropertyRisk.DeclarationPeriod.Id);
                List<CompanyEndorsement> declarationEndorsements = GetLastDeclarationEndorsements(endorsements);

                if (declarationEndorsements == null)
                {
                    return false;
                }
                return declarationEndorsements.Count == quantityOfDeclarations;
            }
            catch (Exception)
            {
                throw new BusinessException("Error al consultar los endosos de declaración");
            }
        }

        /// <summary>
        /// obtiene el numero de declaraciones que tiene que realizarse para hacer un ajuste.
        /// </summary>
        /// <param name="adjustmentId"></param>
        /// <param name="declarationId"></param>
        /// <returns></returns>
        private int GetNumberofDeclarationsExpected(int adjustmentId, int declarationId)
        {

            var monthsForAdjustment = GetMothsByAdjustmentPeriod(adjustmentId);
            var monthsForDeclaration = GetMothsByDeclarationPeriod(declarationId);

            return monthsForAdjustment / monthsForDeclaration;

        }

        /// <summary>
        /// Consultar el listado endosos de declaración más recientes, si hay endosos de 
        ///     ajuste, retorna las declaraciones posteriores a dicho ajuste, si no, trae
        ///     las declaraciones desde la emisión
        /// </summary>
        /// <param name="endorsements"></param>
        /// <returns></returns>
        public List<CompanyEndorsement> GetLastDeclarationEndorsements(List<CompanyEndorsement> endorsements)
        {
            // Se verifica si hay endoso de ajuste anterior
            if (endorsements.Where(x => x.EndorsementType.Value == EndorsementType.AdjustmentEndorsement).Count() > 0)
            {
                // Se toma el endoso de ajuste más reciente
                CompanyEndorsement adjustmentEndorsement = endorsements.OrderByDescending(x => x.Id).
                    First(x => x.EndorsementType.Value == EndorsementType.AdjustmentEndorsement);

                List<CompanyEndorsement> endorsementsList = endorsements.Where(x => x.Id > adjustmentEndorsement.Id).ToList();
                endorsements = (endorsementsList.Count() > 0) ? endorsementsList : endorsements;
            }

            return endorsements.Where(x => x.EndorsementType.Value == EndorsementType.DeclarationEndorsement).ToList();
        }
        public bool CanMakeEndorsement(int policyId, out Dictionary<string, object> endorsementValidate)
        {
            try
            {
                bool result = false;
                endorsementValidate = new Dictionary<string, object>();
                PropertyDAO propertyDAO = new PropertyDAO();
                CompanyPropertyRisk companyPropertyRisk = propertyDAO.GetCompanyPropertiesByPolicyId(policyId).FirstOrDefault();
                List<CompanyEndorsement> endorsements = GetEndorsements(companyPropertyRisk.Risk.Policy);
                decimal monthsVigency = GetMonthsByVigency(companyPropertyRisk.Risk.Policy.CurrentFrom, companyPropertyRisk.Risk.Policy.CurrentTo);
                decimal QuantityOfDeclaration = Math.Ceiling(monthsVigency / GetMothsByDeclarationPeriod(companyPropertyRisk.DeclarationPeriod.Id));
                decimal QuantityOfAdjustment = Math.Floor(monthsVigency / GetMothsByAdjustmentPeriod(companyPropertyRisk.BillingPeriodDepositPremium));
                decimal declarationEndorsmentCount = endorsements.Where(x => x.EndorsementType == EndorsementType.DeclarationEndorsement && x.CurrentFrom >= companyPropertyRisk.Risk.Policy.CurrentFrom).Count();
                decimal ajustmentEndorsmentCount = endorsements.Where(x => x.EndorsementType == EndorsementType.AdjustmentEndorsement && x.CurrentFrom >= companyPropertyRisk.Risk.Policy.CurrentFrom).Count();
                decimal QuantityOfDeclarationbyAjust = QuantityOfDeclaration / QuantityOfAdjustment;
                List<decimal> declarationByAjustment = new List<decimal>();
                decimal aux = QuantityOfDeclaration;
                decimal eval = 0;
                do
                {
                    eval = QuantityOfDeclaration - QuantityOfDeclarationbyAjust;
                    if (eval > QuantityOfDeclarationbyAjust)
                    {
                        declarationByAjustment.Add(QuantityOfDeclaration - eval);
                        QuantityOfDeclaration = eval;
                    }
                    else
                    {
                        declarationByAjustment.Add(QuantityOfDeclaration);
                    }

                } while (eval > QuantityOfDeclarationbyAjust);
                QuantityOfDeclaration = aux;

                decimal endorsementeLimit = 0;

                if (declarationEndorsmentCount < QuantityOfDeclaration || ajustmentEndorsmentCount < QuantityOfAdjustment)
                {
                    if (declarationByAjustment.Count == 1)
                    {
                        endorsementeLimit = declarationByAjustment[0];
                    }
                    else
                    {
                        endorsementeLimit = ((declarationByAjustment[(int)ajustmentEndorsmentCount] * ajustmentEndorsmentCount) + QuantityOfDeclarationbyAjust);
                    }
                    if (declarationEndorsmentCount < endorsementeLimit)
                    {
                        endorsementValidate.Add("AllowEndorsement", EndorsementType.DeclarationEndorsement);
                        endorsementValidate.Add("Message", "Endoso de Declaracion Pendiente");
                    }
                    else
                    {
                        endorsementValidate.Add("AllowEndorsement", EndorsementType.AdjustmentEndorsement);
                        endorsementValidate.Add("Message", "Endoso de Ajuste Pendiente");
                    }
                    result = true;
                }
                else
                {
                    result = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al consultar los endosos");
            }


        }


        public List<InsuredObjectDTO> GetInsuredObjectsByRiskId(int riskId)
        {
            return DTOAssembler.CreateInsuredObjects(DelegateService.underwritingService.GetInsuredObjectsByRiskId(riskId));
        }

        /// <summary>
        /// Realiza el llamado los objetos del seguro asociados al riesgo
        /// </summary>
        /// <param name="liabilityRisk"></param>
        /// <returns>CompanyLiabilityRisk</returns>
        public List<CompanyCoverage> GetTemporalCoveragesByRiskIdInsuredObjectId(int riskId, int insuredObjectId)
        {
            PropertyDAO propertyDAO = new PropertyDAO();
            CompanyPropertyRisk companyPropertyRisk = propertyDAO.GetCompanyPropertyRiskByRiskId(riskId);
            if (companyPropertyRisk != null && companyPropertyRisk.Risk != null)
            {
                List<CompanyCoverage> coverages = new List<CompanyCoverage>();
                CompanyInsuredObject insuredObject = new CompanyInsuredObject();
                if (companyPropertyRisk.Risk.Coverages != null && companyPropertyRisk.Risk.Coverages.Any())
                {
                    if (insuredObjectId != 0)
                    {
                        insuredObject = companyPropertyRisk.InsuredObjects.Where(x => x.Id == insuredObjectId).FirstOrDefault();
                        var cover = companyPropertyRisk.Risk.Coverages.Where(x => x.InsuredObject.Id == insuredObjectId && x.IsSelected).FirstOrDefault();
                        if (cover != null)
                        {
                            insuredObject.Amount = cover.DeclaredAmount;
                        }
                        if (insuredObject == null)
                        {
                            coverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectId, companyPropertyRisk.Risk.GroupCoverage.Id, companyPropertyRisk.Risk.Policy.Product.Id).Where(u => u.IsSelected == true).ToList();
                        }
                        else
                        {
                            coverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectId, companyPropertyRisk.Risk.GroupCoverage.Id, companyPropertyRisk.Risk.Policy.Product.Id).ToList();
                            foreach (CompanyCoverage item in companyPropertyRisk.Risk.Coverages.Where(x => x.InsuredObject.Id == insuredObjectId))
                            {
                                item.IsPrimary = coverages.First(x => x.Id == item.Id).IsPrimary;
                                item.CoverNum = coverages.First(x => x.Id == item.Id).CoverNum;
                                item.Number = coverages.First(x => x.Id == item.Id).CoverNum;
                                item.MainCoverageId = coverages.First(x => x.Id == item.Id).MainCoverageId.GetValueOrDefault();
                                item.SublimitPercentage = coverages.First(x => x.Id == item.Id).SublimitPercentage;
                                item.AllyCoverageId = coverages.First(x => x.Id == item.Id).AllyCoverageId;
                            }
                            coverages = companyPropertyRisk.Risk.Coverages.Where(x => x.InsuredObject.Id == insuredObject.Id).Select(item =>
                            {
                                item.InsuredObject = insuredObject;
                                return item;
                            }).ToList();
                        }
                        if (coverages?.Count < 1)
                        {
                            coverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectId, companyPropertyRisk.Risk.GroupCoverage.Id, companyPropertyRisk.Risk.Policy.Product.Id).Where(u => u.IsSelected == true).ToList();
                        }
                    }
                    else
                    {
                        coverages = companyPropertyRisk.Risk.Coverages;
                    }

                    if (coverages != null && coverages.Any())
                    {
                        List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();
                        CoverageBusiness coverageBusiness = new CoverageBusiness();
                        foreach (var item in coverages)
                        {
                            if (item.RuleSetId != null)
                            {
                                CompanyCoverage coverage = new CompanyCoverage();
                                //Ejecuta reglas Pre
                                coverage = coverageBusiness.RunRulesCoverage(companyPropertyRisk, item, item.RuleSetId.Value);
                                if (coverage != null)
                                {
                                    companyCoverages.Add(coverage);
                                }
                            }
                            else
                            {
                                //no tiene regla es porque es aliada
                                companyCoverages.Add(item);
                            }
                        }
                        return companyCoverages;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorCoverages);
                    }
                }
                else
                {
                    coverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectId, companyPropertyRisk.Risk.GroupCoverage.Id, companyPropertyRisk.Risk.Policy.Product.Id).Where(u => u.IsSelected == true).ToList();
                    List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();
                    CoverageBusiness coverageBusiness = new CoverageBusiness();
                    foreach (var item in coverages)
                    {
                        if (item.RuleSetId != null)
                        {
                            CompanyCoverage coverage = new CompanyCoverage();
                            //Ejecuta reglas Pre
                            coverage = coverageBusiness.RunRules(companyPropertyRisk, item, item.RuleSetId.Value);
                            if (coverage != null)
                            {
                                companyCoverages.Add(coverage);
                            }
                        }
                        else
                        {
                            //no tiene regla es porque es aliada
                            companyCoverages.Add(item);
                        }

                    }
                    return companyCoverages;
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorCoverages);
            }
        }
        public decimal GetMonthsByVigency(DateTime currentFrom, DateTime currentTo)
        {
            //var result = currentFrom;
            //var monthCount = 0;
            //while (result < currentTo)
            //{
            //    result = result.AddMonths(1);
            //    monthCount++;
            //}
            //return monthCount;
            int referenceDay = currentFrom.Day;
            var result = currentFrom;
            var monthCount = 0;
            while (result < currentTo)
            {
                result = result.AddMonths(1);
                if (result.Day < referenceDay)
                {
                    DateTime tempDate;
                    bool validDate = false;
                    validDate = DateTime.TryParse(string.Format("{0}/{1}/{2}", result.Year, result.Month, referenceDay), out tempDate);
                    if (validDate)
                    {
                        result = tempDate;
                    }
                }
                monthCount++;
            }
            return monthCount;
        }
        public bool HasDepositPremium(CompanyInsuredObject companyInsuredObject)
        {
            if (companyInsuredObject.IsDeclarative && companyInsuredObject.DepositPremiunPercent > 0)
            {
                return true;
            }
            return false;
        }
        public bool HasBeenDepositPremiumOverflowed(List<CompanyEndorsement> endorsements)
        {
            // Se listan los endosos de ajuste
            List<CompanyEndorsement> adjustmentEndorsements = endorsements.Where(
                x => x.EndorsementType == Core.Application.UnderwritingServices.Enums.EndorsementType.AdjustmentEndorsement).ToList();

            if (adjustmentEndorsements == null || adjustmentEndorsements.Count == 0)
                return false;

            adjustmentEndorsements = adjustmentEndorsements.OrderByDescending(x => x.Id).ToList();

            // Se recorren los endosos de ajuste
            foreach (var adjustmentEndorsement in adjustmentEndorsements)
            {
                List<CompanyRisk> corisks = DelegateService.underwritingService.GetRiskByPolicyIdEndorsmentId(adjustmentEndorsement.PolicyId,
                    adjustmentEndorsement.Id);

                foreach (var risk in corisks)
                {
                    foreach (var coverage in risk.Coverages)
                    {
                        // Si cualquier cobertura cobró prima, quiere decir que desbordó la prima en depósito
                        if (coverage.PremiumAmount > 0)
                            return true;
                    }
                }
            }
            return false;
        }
        public decimal GetDepositPremiumByCoverageId(CompanyEndorsement companyEndorsement, int coverageId, CompanyRisk companyRisk)
        {
            CoverageBusiness coverageBusiness = new CoverageBusiness();
            foreach (var coverage in companyRisk.Coverages)
            {
                if (coverage.Id == coverageId)
                {
                    coverage.SubLimitAmount = coverage.DeclaredAmount;
                    Core.Application.ProductServices.Models.ProductCurrency productCurrency = DelegateService.productServiceCore.GetProductCurrencies(companyRisk.Policy.Product.Id).Where(x => x.Id == companyRisk.Policy.ExchangeRate.Currency.Id).FirstOrDefault();
                    CompanyCoverage _coverage = DelegateService.underwritingService.QuotateCompanyCoverage(coverage, companyEndorsement.PolicyId, companyRisk.Id, productCurrency.DecimalQuantity);
                    return (_coverage.PremiumAmount * (coverage.DepositPremiumPercent / 100)) * ((decimal)(companyEndorsement.EndorsementDays) / 365);
                }
            }
            return 0;
        }
        public List<CompanyEndorsement> GetAllDeclarationEndorsements(List<CompanyEndorsement> endorsements)
        {
            return endorsements.Where(x => x.EndorsementType.Value == Core.Application.UnderwritingServices.Enums.EndorsementType.DeclarationEndorsement).ToList();
        }

    }
}