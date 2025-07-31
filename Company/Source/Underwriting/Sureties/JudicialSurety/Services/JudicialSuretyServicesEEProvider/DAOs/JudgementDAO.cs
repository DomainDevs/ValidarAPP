using JudicialSuretyServicesEEProvider;
using Newtonsoft.Json;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Assemblers;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Entities.views;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Resources;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using IssuanceEntities = Sistran.Core.Application.Issuance.Entities;
using Rules = Sistran.Core.Framework.Rules;
using UNMOD = Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using UTILITES = Company.UnderwritingUtilities;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.ModelsAutoMapper;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota;
using Sistran.Core.Integration.OperationQuotaServices.Enums;
using Sistran.Core.Application.Utilities.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Enums;
using PER = Sistran.Core.Application.UniquePerson.Entities;
using System.Diagnostics;
using Sistran.Company.Application.ExternalProxyServices.Models;
using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Entities.View;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.DAOs
{
    public class JudicialSuretyDAO
    {
        public List<CompanyJudgement> GetCompanyJudicialSuretyByPolicyId(int policyId)
        {
            List<CompanyJudgement> judicialSuretyRisks = new List<CompanyJudgement>();
            //riesgos
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
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
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);

            JudicialSuretyView view = new JudicialSuretyView();
            ViewBuilder builder = new ViewBuilder("JudicialSuretyView") { Filter = filter.GetPredicate() };
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Risks.Count > 0)
            {
                var endorsements = view.Endorsement.Cast<IssuanceEntities.Endorsement>().ToList();
                var endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                var risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                var judicialSureties = view.RiskJudicialSurety.Cast<IssuanceEntities.RiskJudicialSurety>().ToList();
                var riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                var riskClause = view.RiskClauses.Cast<ISSEN.RiskClause>().ToList();

                foreach (IssuanceEntities.Risk item in risks)
                {
                    DataFacadeManager.Instance.GetDataFacade().LoadDynamicProperties(item);

                    CompanyJudgementMapper companyJudgementMapper = new CompanyJudgementMapper();
                    companyJudgementMapper.risk = item;
                    companyJudgementMapper.RiskJudicialSurety = judicialSureties.First(x => x.RiskId == item.RiskId);
                    companyJudgementMapper.endorsementRisk = endorsementRisks.First(x => x.RiskId == item.RiskId);
                    companyJudgementMapper.endorsement = endorsements.First();

                    CompanyJudgement companyJudgement = ModelAssembler.CreateJudicialSurety(companyJudgementMapper);

                    int insuredNameNum = companyJudgement.Risk.MainInsured.CompanyName.NameNum;
                    var mainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(companyJudgement.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                    //ModelAssembler.CreateMapPersonInsured();
                    companyJudgement.Risk.MainInsured = mainInsured;
                    if (riskClause.Exists(x => x.RiskId == item.RiskId))
                    {
                        companyJudgement.Risk.Clauses = DelegateService.underwritingService.AddClauses(null, riskClause.Where(x => x.RiskId == item.RiskId).Select(x => x.ClauseId).ToList());
                    }
                    companyJudgement.Risk.SecondInsured.CustomerType = CustomerType.Individual;
                    companyJudgement.Risk.SecondInsured.IndividualType = IndividualType.Person;
                    var companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(companyJudgement.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();

                    companyJudgement.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                    Object objlock = new object();
                    ModelAssembler.CreateMapCompanyBeneficiary();
                    companyJudgement.Risk.MainInsured.CompanyName = new IssuanceCompanyName
                    {
                        NameNum = companyName.NameNum,
                        TradeName = companyName.TradeName,
                        Address = companyName.Address == null ? null : new IssuanceAddress
                        {
                            Id = companyName.Address.Id,
                            Description = companyName.Address.Description,
                            City = companyName.Address.City
                        },
                        Phone = companyName.Phone == null ? null : new IssuancePhone
                        {
                            Id = companyName.Phone.Id,
                            Description = companyName.Phone.Description
                        },
                        Email = new IssuanceEmail
                        {
                            Id = companyName.Email == null ? 0 : companyName.Email.Id,
                            Description = companyName.Email == null ? "" : companyName.Email.Description
                        }
                    };
                    TP.Parallel.ForEach(riskBeneficiaries.Where(x => x.RiskId == item.RiskId), riskBeneficiary =>
                    {
                        var imapper = ModelAssembler.CreateMapCompanyBeneficiary();
                        CompanyBeneficiary CiaBeneficiary = new CompanyBeneficiary();
                        var beneficiaryRisk = DelegateService.underwritingService.GetBeneficiariesByDescription(riskBeneficiary.BeneficiaryId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                        if (beneficiaryRisk != null)
                        {
                            CiaBeneficiary = imapper.Map<UNMOD.Beneficiary, CompanyBeneficiary>(beneficiaryRisk);
                            CiaBeneficiary.CustomerType = CustomerType.Individual;
                            CiaBeneficiary.BeneficiaryType = new CompanyBeneficiaryType { Id = riskBeneficiary.BeneficiaryTypeCode };
                            CiaBeneficiary.Participation = riskBeneficiary.BenefitPercentage;
                            List<CompanyName> companyNames = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(CiaBeneficiary.IndividualId, CiaBeneficiary.CustomerType);
                            companyName = new CompanyName();
                            if (companyNames.Exists(x => x.NameNum == 0 && x.IsMain))
                            {
                                companyName = companyNames.First(x => x.IsMain);
                            }
                            else
                            {
                                companyName = companyNames.First();
                            }
                            lock (objlock)
                            {
                                CiaBeneficiary.CompanyName = new IssuanceCompanyName
                                {
                                    NameNum = companyName.NameNum,
                                    TradeName = companyName.TradeName,
                                    Address = companyName.Address == null ? null : new IssuanceAddress
                                    {
                                        Id = companyName.Address.Id,
                                        Description = companyName.Address.Description,
                                        City = companyName.Address.City
                                    },
                                    Phone = companyName.Phone == null ? null : new IssuancePhone
                                    {
                                        Id = companyName.Phone.Id,
                                        Description = companyName.Phone.Description
                                    },
                                    Email = new IssuanceEmail
                                    {
                                        Id = companyName.Email == null ? 0 : companyName.Email.Id,
                                        Description = companyName.Email == null ? "" : companyName.Email.Description
                                    }
                                };
                                companyJudgement.Risk.Beneficiaries.Add(CiaBeneficiary);
                            }
                        }
                    });




                    List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();

                    companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(endorsements.First().PolicyId, endorsementRisks.First(x => x.RiskId == item.RiskId).EndorsementId, item.RiskId);
                    companyJudgement.Risk.Coverages = companyCoverages;

                    judicialSuretyRisks.Add(companyJudgement);
                }
            }
            return judicialSuretyRisks;
        }

        public CompanyJudgement CreateJudgementTemporal(CompanyJudgement judicialSurety, bool isMassive)
        {
            judicialSurety.Risk.InfringementPolicies = ValidateAuthorizationPolicies(judicialSurety);

            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "true";

            PendingOperation pendingOperation = new PendingOperation();
            CompanyPolicy policy = judicialSurety.Risk.Policy;
            judicialSurety.Risk.Policy = null;
            if (judicialSurety.Risk.Id == 0)
            {
                pendingOperation.CreationDate = DateTime.Now;
                pendingOperation.ParentId = policy.Id;



                pendingOperation.Operation = JsonConvert.SerializeObject(judicialSurety);
                if (isMassive && boolUseReplicatedDatabase)
                {
                    //Se guarda el JSON en la base de datos de réplica
                }
                else
                {
                    pendingOperation = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);
                }
            }
            else
            {
                pendingOperation.ModificationDate = DateTime.Now;
                pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(judicialSurety.Risk.Id);
                if (pendingOperation != null)
                {
                    //****************************GUARDAR TEMPORAL*********************************//
                    //companyVehicle = SaveCompanyVehicleTemporalTables(companyVehicle, policy);
                    //****************************************************************************//
                    pendingOperation.Operation = JsonConvert.SerializeObject(judicialSurety);
                    if (isMassive && boolUseReplicatedDatabase)
                    {
                        //Se guarda el JSON en la base de datos de réplica
                    }
                    else
                    {
                        DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
                    }
                }
                else
                {
                    throw new Exception("Error obteniendo el Temporal");
                }
            }

            judicialSurety.Risk.Id = pendingOperation.Id;
            judicialSurety.Risk.Policy = policy;
            ////****************************GUARDAR TEMPORAL*********************************//
            int riskId = SaveCompanyJudicialTemporalTables(judicialSurety);
            if (judicialSurety.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Modification)
            {
                judicialSurety.Risk.RiskId = riskId;
            }
            ////****************************************************************************//

            pendingOperation.Operation = JsonConvert.SerializeObject(judicialSurety);

            if (isMassive && boolUseReplicatedDatabase)
            {
                //Se guarda el JSON en la base de datos de réplica
            }
            else
            {
                DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
            }
            judicialSurety.Risk.Id = pendingOperation.Id;
            return judicialSurety;
        }

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        public CompanyJudgement GetCompanyJudgementByRiskId(int riskId)
        {
            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(riskId);

            if (pendingOperation != null)
            {
                CompanyJudgement companyJudgement = JsonConvert.DeserializeObject<CompanyJudgement>(pendingOperation.Operation);
                companyJudgement.Risk.Id = pendingOperation.Id;
                companyJudgement.Risk.IsPersisted = true;

                return companyJudgement;
            }
            else
            {
                return null;
            }
        }

        public List<PoliciesAut> ValidateAuthorizationPolicies(CompanyJudgement judicialSurety)
        {
            string key = judicialSurety.Risk.Policy.Prefix.Id + "," + (int)judicialSurety.Risk.CoveredRiskType;
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();
            Rules.Facade facade = DelegateService.underwritingService.CreateFacadeGeneral(judicialSurety.Risk.Policy);

            if ((judicialSurety.Risk.MainInsured.AssociationType != null && judicialSurety.Risk.MainInsured?.AssociationType?.Id == 0) || judicialSurety.Risk.MainInsured.AssociationType == null)
            {
                int judicialSurety_associationType = GetDataAssociationType(judicialSurety.Risk.MainInsured.IndividualId);
                if (judicialSurety.Risk.MainInsured.AssociationType == null)
                    judicialSurety.Risk.MainInsured.AssociationType = new IssuanceAssociationType();

                judicialSurety.Risk.MainInsured.AssociationType.Id = judicialSurety_associationType == 0 ? 1 : judicialSurety_associationType;

            }
            EntityAssembler.CreateFacadeRiskJudgement(facade, judicialSurety);

            /*Politica del riesgo*/
            policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_RISK));

            /*Politicas de cobertura*/
            if (judicialSurety.Risk.Coverages != null)
            {
                foreach (CompanyCoverage coverage in judicialSurety.Risk.Coverages)
                {
                    EntityAssembler.CreateFacadeCoverage(facade, coverage);
                    policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_COVERAGE));
                }
            }

            return policiesAuts;
        }

        public int GetDataAssociationType(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PER.CoCompany.Properties.IndividualId, typeof(PER.CoCompany).Name);
            filter.Equal();
            filter.Constant(individualId);
            var result = DataFacadeManager.Instance.GetDataFacade().List<PER.CoCompany>(filter.GetPredicate());
            DataFacadeManager.Dispose();
            if (result.Count > 0)
            {
                return result[0].AssociationTypeCode;
            }
            return 0;
        }

        public bool GetInsuredGuaranteeRelationPolicy(int guaranteeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.RiskJudicialSuretyGuarantee.Properties.GuaranteeId, typeof(ISSEN.RiskJudicialSuretyGuarantee).Name);
            filter.Equal();
            filter.Constant(guaranteeId);
            filter.Or();
            filter.Property(ISSEN.RiskSuretyGuarantee.Properties.GuaranteeId, typeof(ISSEN.RiskSuretyGuarantee).Name);
            filter.Equal();
            filter.Constant(guaranteeId);

            RiskJudicialSuretyGuaranteeView view = new RiskJudicialSuretyGuaranteeView();
            ViewBuilder builder = new ViewBuilder("RiskJudicialSuretyGuaranteeView") { Filter = filter.GetPredicate() };
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            DataFacadeManager.Dispose();

            if (view.RiskSuretyGuarantees != null && view.RiskSuretyGuarantees.Count > 0)
            {
                ISSEN.RiskSuretyGuarantee riskSuretyGuarantee = view.RiskSuretyGuarantees?.Cast<ISSEN.RiskSuretyGuarantee>().FirstOrDefault();
                filter = new ObjectCriteriaBuilder();
                filter.Property(UPEN.InsuredGuarantee.Properties.GuaranteeId, typeof(UPEN.InsuredGuarantee).Name).Equal().Constant(riskSuretyGuarantee.GuaranteeId);

                UPEN.InsuredGuarantee insuredGuarantee = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.InsuredGuarantee), filter.GetPredicate())).Cast<UPEN.InsuredGuarantee>().FirstOrDefault();
                if (insuredGuarantee.ClosedInd)
                {
                    return true;
                }
            }
            if (view.RiskJudicialSuretyGuarantees != null && view.RiskJudicialSuretyGuarantees.Count > 0)
            {
                ISSEN.RiskJudicialSuretyGuarantee riskJudicialSuretyGuarantee = view.RiskJudicialSuretyGuarantees?.Cast<ISSEN.RiskJudicialSuretyGuarantee>().FirstOrDefault();
                filter = new ObjectCriteriaBuilder();
                filter.Property(UPEN.InsuredGuarantee.Properties.GuaranteeId, typeof(UPEN.InsuredGuarantee).Name).Equal().Constant(riskJudicialSuretyGuarantee.GuaranteeId);

                UPEN.InsuredGuarantee insuredGuarantee = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.InsuredGuarantee), filter.GetPredicate())).Cast<UPEN.InsuredGuarantee>().FirstOrDefault();
                if (insuredGuarantee.ClosedInd)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Riesgos</returns>
        public List<CompanyJudgement> GetCompanyJudgementsByTemporalId(int temporalId)
        {
            ConcurrentBag<CompanyJudgement> companyJudgements = new ConcurrentBag<CompanyJudgement>();
            List<PendingOperation> pendingOperations = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(temporalId);
            TP.Parallel.ForEach(pendingOperations, pendingOperation =>
            {
                var companyJudgement = JsonConvert.DeserializeObject<CompanyJudgement>(pendingOperation.Operation);
                companyJudgement.Risk.Id = pendingOperation.Id;
                companyJudgements.Add(companyJudgement);
            });

            return companyJudgements.ToList();
        }

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="endorsementId">Id Endoso</param>
        /// <returns>Riesgos</returns>
        public List<CompanyJudgement> GetCompanyJudgementsByEndorsementId(int endorsementId)
        {
            List<CompanyJudgement> judicialSuretyRisks = new List<CompanyJudgement>();
            //riesgos
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            JudicialSuretyView view = new JudicialSuretyView();
            ViewBuilder builder = new ViewBuilder("JudicialSuretyView") { Filter = filter.GetPredicate() };
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Risks.Count > 0)
            {
                var endorsements = view.Endorsement.Cast<IssuanceEntities.Endorsement>().ToList();
                var endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                var risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                var judicialSureties = view.RiskJudicialSurety.Cast<IssuanceEntities.RiskJudicialSurety>().ToList();
                var riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();


                foreach (IssuanceEntities.Risk item in risks)
                {
                    DataFacadeManager.Instance.GetDataFacade().LoadDynamicProperties(item);

                    CompanyJudgementMapper companyJudgementMapper = new CompanyJudgementMapper();
                    companyJudgementMapper.risk = item;
                    companyJudgementMapper.RiskJudicialSurety = judicialSureties.First(x => x.RiskId == item.RiskId);
                    companyJudgementMapper.endorsementRisk = endorsementRisks.First(x => x.RiskId == item.RiskId);
                    companyJudgementMapper.endorsement = endorsements.First();

                    CompanyJudgement companyJudgement = ModelAssembler.CreateJudicialSurety(companyJudgementMapper);

                    int insuredNameNum = companyJudgement.Risk.MainInsured.CompanyName.NameNum;
                    ModelAssembler.CreatetMapCompanyInsured();
                    companyJudgement.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(companyJudgement.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                    var companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(companyJudgement.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
                    companyJudgement.Risk.MainInsured.CompanyName = new IssuanceCompanyName
                    {
                        NameNum = companyName.NameNum,
                        TradeName = companyName.TradeName,
                        Address = companyName.Address == null ? null : new IssuanceAddress
                        {
                            Id = companyName.Address.Id,
                            Description = companyName.Address.Description,
                            City = companyName.Address.City
                        },
                        Phone = companyName.Phone == null ? null : new IssuancePhone
                        {
                            Id = companyName.Phone.Id,
                            Description = companyName.Phone.Description
                        },
                        Email = companyName.Email == null ? null : new IssuanceEmail
                        {
                            Id = companyName.Email.Id,
                            Description = companyName.Email.Description
                        }
                    };

                    companyJudgement.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                    foreach (ISSEN.RiskBeneficiary riskBeneficiary in riskBeneficiaries.Where(x => x.RiskId == item.RiskId))
                    {
                        CompanyBeneficiary beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(riskBeneficiary.BeneficiaryId.ToString(), InsuredSearchType.IndividualId).Cast<CompanyBeneficiary>().First();
                        beneficiary.BeneficiaryType = new CompanyBeneficiaryType { Id = riskBeneficiary.BeneficiaryTypeCode };
                        companyName = new CompanyName();
                        companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(beneficiary.IndividualId, beneficiary.CustomerType).First();
                        beneficiary.CompanyName = new IssuanceCompanyName
                        {
                            NameNum = companyName.NameNum,
                            TradeName = companyName.TradeName,
                            Address = companyName.Address == null ? null : new IssuanceAddress
                            {
                                Id = companyName.Address.Id,
                                Description = companyName.Address.Description,
                                City = companyName.Address.City
                            },
                            Phone = companyName.Phone == null ? null : new IssuancePhone
                            {
                                Id = companyName.Phone.Id,
                                Description = companyName.Phone.Description
                            },
                            Email = companyName.Email == null ? null : new IssuanceEmail
                            {
                                Id = companyName.Email.Id,
                                Description = companyName.Email.Description
                            }
                        };

                        companyJudgement.Risk.Beneficiaries.Add(beneficiary);
                    }
                    companyJudgement.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(endorsements.First().PolicyId, endorsementId, companyJudgement.Risk.RiskId);


                    judicialSuretyRisks.Add(companyJudgement);


                }
            }
            return judicialSuretyRisks;
        }

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyJudgement> companyJudgements)
        {
            if (companyPolicy == null)
            {
                throw new ArgumentException("Poliza Vacia");
            }
            ValidateInfringementPolicies(companyPolicy, companyJudgements);
            if (companyPolicy?.InfringementPolicies?.Count == 0)
            {
                if (companyPolicy.Endorsement.EndorsementType != EndorsementType.LastEndorsementCancellation && companyPolicy.ExchangeRate.Currency.Id != (int)EnumExchangeRateCurrency.CURRENCY_PESOS)
                {
                    ExchangeRate exchangeRate = new ExchangeRate();
                    exchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DateTime.Now, companyPolicy.ExchangeRate.Currency.Id);
                    if (exchangeRate.RateDate == DateTime.Now.Date)
                    {
                        companyPolicy.ExchangeRate = exchangeRate;
                    }
                    else if (exchangeRate.RateDate != DateTime.Now.Date)
                    {
                        throw new Exception(Errors.TheTRMisNotUpdate);
                    }
                }
                companyPolicy = DelegateService.underwritingService.CreateCompanyPolicy(companyPolicy);
                if (companyPolicy != null)
                {
                    int maxRiskCount = companyPolicy.Summary.RiskCount;
                    int policyId = companyPolicy.Endorsement.PolicyId;
                    int endorsementId = companyPolicy.Endorsement.Id;
                    int endorsementTypeId = (int)companyPolicy.Endorsement.EndorsementType;
                    EndorsementType endorsementType = (EndorsementType)endorsementTypeId;
                    try
                    {

                        TP.Parallel.ForEach(companyJudgements, companyJudgement =>
                        {
                            companyJudgement.Risk.Policy = companyPolicy;
                            if (companyJudgement.Risk.Status == RiskStatusType.Original || companyJudgement.Risk.Status == RiskStatusType.Included)
                            {
                                companyJudgement.Risk.Number = ++maxRiskCount;
                            }
                        });

                        if (companyPolicy.Product.IsCollective)
                        {
                            ConcurrentBag<string> errors = new ConcurrentBag<string>();
                            Parallel.ForEach(companyJudgements, ParallelHelper.DebugParallelFor(), companyJudgement =>
                            {
                                try
                                {
                                    CreateRisk(companyJudgement);
                                    List<OperatingQuotaEventDTO> operatingQuotaEvents = new List<OperatingQuotaEventDTO>();

                                    foreach (CompanyCoverage item in companyJudgement.Risk.Coverages)
                                    {
                                        OperatingQuotaEventDTO operatingQuotaEvent = new OperatingQuotaEventDTO();
                                        switch (companyPolicy.Endorsement.EndorsementType)
                                        {
                                            case EndorsementType.Emission:
                                                operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_ENDORSEMENT;
                                                break;
                                            case EndorsementType.Modification:
                                                operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_MODIFY_ENDORSEMENT;
                                                break;
                                            case EndorsementType.Cancellation:
                                                operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_CANCELLATION_ENDORSEMENT;
                                                break;
                                            case EndorsementType.EffectiveExtension:
                                                operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_EFFECTIVE_EXTENSION_ENDORSEMENT;
                                                break;
                                            case EndorsementType.Renewal:
                                                operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_RENEWAL_ENDORSMENT;
                                                break;
                                            case EndorsementType.LastEndorsementCancellation:
                                                operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLI_LAST_CANCELLATION_ENDORSEMENT;
                                                break;
                                            case EndorsementType.ChangeTermEndorsement:
                                                operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_CHANGE_TERM_ENDORSEMENT;
                                                break;
                                            case EndorsementType.ChangeAgentEndorsement:
                                                operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_AGENT_CHANGE_ENDORSEMENT;
                                                break;
                                            case EndorsementType.ChangeCoinsuranceEndorsement:
                                                operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_CHANGE_COINSURANCE_ENDORSEMENT;
                                                break;
                                            default:
                                                break;
                                        }
                                        operatingQuotaEvent.ApplyEndorsement = new ApplyEndorsementDTO();
                                        operatingQuotaEvent.IssueDate = companyPolicy.IssueDate;
                                        operatingQuotaEvent.IdentificationId = companyJudgement.Risk.Policy.Holder.IndividualId;
                                        operatingQuotaEvent.LineBusinessID = companyPolicy.Prefix.Id;
                                        operatingQuotaEvent.Policy_Init_Date = companyPolicy.CurrentFrom;
                                        operatingQuotaEvent.Policy_End_Date = companyPolicy.CurrentTo;
                                        operatingQuotaEvent.Cov_Init_Date = item.CurrentFrom;
                                        operatingQuotaEvent.Cov_End_Date = item.CurrentTo;
                                        operatingQuotaEvent.ApplyEndorsement.IndividualId = companyJudgement.Risk.Policy.Holder.IndividualId;
                                        operatingQuotaEvent.ApplyEndorsement.CurrencyType = companyPolicy.ExchangeRate.Currency.Id;
                                        operatingQuotaEvent.ApplyEndorsement.CurrencyTypeDesc = companyPolicy.ExchangeRate.Currency.Description;
                                        operatingQuotaEvent.ApplyEndorsement.AmountCoverage = item.LimitAmount;
                                        operatingQuotaEvent.ApplyEndorsement.Endorsement = companyPolicy.Endorsement.Id;
                                        operatingQuotaEvent.ApplyEndorsement.PolicyID = companyPolicy.Endorsement.PolicyId;
                                        operatingQuotaEvent.ApplyEndorsement.EndorsementType = (int)companyPolicy.Endorsement.EndorsementType;
                                        operatingQuotaEvent.ApplyEndorsement.ParticipationPercentage = 100;
                                        operatingQuotaEvents.Add(operatingQuotaEvent);
                                    }
                                    operatingQuotaEvents = DelegateService.OperationQuotaIntegrationService.InsertApplyEndorsementOperatingQuotaEvent(operatingQuotaEvents);

                                }
                                catch (Exception ex)
                                {

                                    errors.Add(ex.Message);

                                }
                                finally
                                {
                                    DataFacadeManager.Dispose();
                                }
                            });
                        }
                        else
                        {
                            ConcurrentBag<string> errors = new ConcurrentBag<string>();
                            Parallel.ForEach(companyJudgements, ParallelHelper.DebugParallelFor(), companyJudgement =>
                            {
                                try
                                {
                                    CreateRisk(companyJudgement);
                                }
                                catch (Exception ex)
                                {
                                    errors.Add(ex.Message);
                                }
                                finally
                                {
                                    DataFacadeManager.Dispose();
                                }

                            });
                            if (errors != null && errors.Any())
                            {
                                throw new ValidationException(string.Join(" ", errors));
                            }
                        }
                        DelegateService.underwritingService.CreateCompanyPolicyPayer(companyPolicy);
                        try
                        {
                            DelegateService.underwritingService.DeleteTemporalByOperationId(companyPolicy.Id, 0, 0, 0);

                            try
                            {
                                DelegateService.underwritingService.SaveControlPolicy(policyId, endorsementId, companyPolicy.Id, (int)PolicyOrigin.Individual);

                                if (DelegateService.commonService.GetParameterByParameterId((int)JudgementKeys.UND_ENABLED_REINSURANCE).BoolParameter.GetValueOrDefault())
                                {
                                    companyPolicy.IsReinsured = DelegateService.underwritingReinsuranceWorkerIntegration.ReinsuranceIssue(policyId, endorsementId, companyPolicy.UserId) > 0;
                                }
                                else
                                {
                                    //Valida 2g
                                    ResponseReinsurance responseReinsurance = new ResponseReinsurance();
                                    RequestReinsurance requestReinsurance = new RequestReinsurance();
                                    requestReinsurance.DocumentNumber = Convert.ToInt32(companyPolicy.DocumentNumber);
                                    requestReinsurance.EndorsementNumber = companyPolicy.Endorsement.Number;
                                    requestReinsurance.Prefix = companyPolicy.Prefix.Id;
                                    requestReinsurance.Branch = companyPolicy.Branch.Id;
                                    responseReinsurance = DelegateService.ExternalServiceWeb.GetReinsurancePolicy(requestReinsurance);
                                    if (responseReinsurance.PolicyStatus == 1)
                                    {
                                        companyPolicy.IsReinsured = true;
                                    }
                                    else
                                    {
                                        companyPolicy.IsReinsured = false;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                EventLog.WriteEntry("Application", Errors.ErrorRegisterIntegration);
                            }
                        }
                        catch (Exception)
                        {

                            throw new ValidationException(Errors.ErrorDeleteTemp);
                        }

                    }
                    catch (Exception)
                    {

                        DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(policyId, endorsementId, endorsementType);
                        throw;
                    }
                }
                else
                {
                    DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id, companyPolicy.Endorsement.EndorsementType.Value);
                }

            }
            return companyPolicy;
        }

        public void CreateRisk(CompanyJudgement companyJudgement)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();

            NameValue[] parameters = new NameValue[48];
            parameters[0] = new NameValue("@IS_FACULTATIVE", companyJudgement.Risk.IsFacultative);
            parameters[1] = new NameValue("@CONTRACT_AMT", companyJudgement.InsuredValue);
            parameters[2] = new NameValue("@BID_NUMBER", companyJudgement.SettledNumber);
            parameters[3] = new NameValue("@INSURED_ID", companyJudgement.Risk.MainInsured.IndividualId);
            parameters[4] = new NameValue("@COVERED_RISK_TYPE_CD", (int)companyJudgement.Risk.CoveredRiskType);
            parameters[5] = new NameValue("@RISK_STATUS_CD", (int)companyJudgement.Risk.Status);
            if (companyJudgement.Risk.Text == null)
            {
                parameters[6] = new NameValue("@CONDITION_TEXT", "--");
            }
            else
            {
                parameters[6] = new NameValue("@CONDITION_TEXT", companyJudgement.Risk.Text.TextBody);
            }
            parameters[7] = new NameValue("@COVER_GROUP_ID", companyJudgement.Risk.GroupCoverage.Id);
            if (companyJudgement.Risk.MainInsured.CompanyName != null && companyJudgement.Risk.MainInsured.CompanyName.NameNum > 0)
            {
                parameters[8] = new NameValue("@NAME_NUM", companyJudgement.Risk.MainInsured.CompanyName.NameNum);
            }
            else
            {
                parameters[8] = new NameValue("@NAME_NUM", DBNull.Value);
            }
            parameters[9] = new NameValue("@ARTICLE_CD", companyJudgement.Article.Id);
            parameters[10] = new NameValue("@COURT_CD", companyJudgement.Court.Id);
            parameters[11] = new NameValue("@INSURED_CAPACITY_OF_CD", (int)companyJudgement.InsuredActAs);
            parameters[12] = new NameValue("@STATE_CD", companyJudgement.City.State.Id);
            parameters[13] = new NameValue("@CITY_CD", companyJudgement.City.Id);
            parameters[14] = new NameValue("@COUNTRY_CD", companyJudgement.City.State.Country.Id);
            DataTable dtBeneficiaries = new DataTable("PARAM_TEMP_RISK_BENEFICIARY");
            dtBeneficiaries.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICIARY_ID", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICIARY_TYPE_CD", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICT_PCT", typeof(decimal));
            dtBeneficiaries.Columns.Add("NAME_NUM", typeof(int));

            foreach (CompanyBeneficiary item in companyJudgement.Risk.Beneficiaries)
            {
                DataRow dataRow = dtBeneficiaries.NewRow();
                dataRow["CUSTOMER_TYPE_CD"] = (int)item.CustomerType;
                dataRow["BENEFICIARY_ID"] = item.IndividualId;
                dataRow["BENEFICIARY_TYPE_CD"] = item.BeneficiaryType.Id;
                dataRow["BENEFICT_PCT"] = item.Participation;

                if (item.CustomerType == CustomerType.Individual && item.CompanyName != null && item.CompanyName.NameNum == 0)
                {
                    if (item.IndividualId == companyJudgement.Risk.MainInsured.IndividualId)
                    {
                        item.CompanyName = companyJudgement.Risk.MainInsured.CompanyName;
                    }
                    else
                    {
                        item.CompanyName.TradeName = "Dirección Principal";
                        item.CompanyName.IsMain = true;
                        item.CompanyName.NameNum = 1;

                        var companyName = new CompanyName
                        {
                            NameNum = item.CompanyName.NameNum,
                            TradeName = item.CompanyName.TradeName,
                            Address = item.CompanyName.Address == null ? null : new Address
                            {
                                Id = item.CompanyName.Address.Id,
                                Description = item.CompanyName.Address.Description,
                                City = item.CompanyName.Address.City
                            },
                            Phone = item.CompanyName.Phone == null ? null : new Phone
                            {
                                Id = item.CompanyName.Phone.Id,
                                Description = item.CompanyName.Phone.Description
                            },
                            Email = item.CompanyName.Email == null ? null : new Email
                            {
                                Id = item.CompanyName.Email.Id,
                                Description = item.CompanyName.Email.Description
                            }
                        };

                        DelegateService.uniquePersonService.CreateCompaniesName(companyName, item.IndividualId);
                    }
                }
                if (item.CompanyName != null && item.CompanyName.NameNum > 0)
                {
                    dataRow["NAME_NUM"] = item.CompanyName.NameNum;
                }

                dtBeneficiaries.Rows.Add(dataRow);
            }

            parameters[15] = new NameValue("@INSERT_TEMP_RISK_BENEFICIARY", dtBeneficiaries);
            DataTable dtCoverages = new DataTable("PARAM_TEMP_RISK_COVERAGE");
            dtCoverages.Columns.Add("COVERAGE_ID", typeof(int));
            dtCoverages.Columns.Add("IS_DECLARATIVE", typeof(bool));
            dtCoverages.Columns.Add("IS_MIN_PREMIUM_DEPOSIT", typeof(bool));
            dtCoverages.Columns.Add("FIRST_RISK_TYPE_CD", typeof(int));
            dtCoverages.Columns.Add("CALCULATION_TYPE_CD", typeof(int));
            dtCoverages.Columns.Add("DECLARED_AMT", typeof(decimal));
            dtCoverages.Columns.Add("PREMIUM_AMT", typeof(decimal));
            dtCoverages.Columns.Add("LIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("SUBLIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("LIMIT_IN_EXCESS", typeof(decimal));
            dtCoverages.Columns.Add("LIMIT_OCCURRENCE_AMT", typeof(decimal));
            dtCoverages.Columns.Add("LIMIT_CLAIMANT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("ACC_PREMIUM_AMT", typeof(decimal));
            dtCoverages.Columns.Add("ACC_LIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("ACC_SUBLIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("CURRENT_FROM", typeof(DateTime));
            dtCoverages.Columns.Add("RATE_TYPE_CD", typeof(int));
            dtCoverages.Columns.Add("RATE", typeof(decimal));
            dtCoverages.Columns.Add("CURRENT_TO", typeof(DateTime));
            dtCoverages.Columns.Add("COVER_NUM", typeof(int));
            dtCoverages.Columns.Add("RISK_COVER_ID", typeof(int));
            dtCoverages.Columns.Add("COVER_STATUS_CD", typeof(int));
            dtCoverages.Columns.Add("COVER_ORIGINAL_STATUS_CD", typeof(int));
            dtCoverages.Columns.Add("CONDITION_TEXT", typeof(string));
            dtCoverages.Columns.Add("ENDORSEMENT_LIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("ENDORSEMENT_SUBLIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("FLAT_RATE_PCT", typeof(decimal));
            dtCoverages.Columns.Add("CONTRACT_AMOUNT_PCT", typeof(decimal));
            dtCoverages.Columns.Add("DYNAMIC_PROPERTIES", typeof(byte[]));
            dtCoverages.Columns.Add("SHORT_TERM_PCT", typeof(decimal));
            dtCoverages.Columns.Add("PREMIUM_AMT_DEPOSIT_PERCENT", typeof(decimal));
            dtCoverages.Columns.Add("MAX_LIABILITY_AMT", typeof(decimal));

            DataTable dtDeductibles = new DataTable("PARAM_TEMP_RISK_COVER_DEDUCT");
            dtDeductibles.Columns.Add("COVERAGE_ID", typeof(int));
            dtDeductibles.Columns.Add("RATE_TYPE_CD", typeof(int));
            dtDeductibles.Columns.Add("RATE", typeof(decimal));
            dtDeductibles.Columns.Add("DEDUCT_PREMIUM_AMT", typeof(decimal));
            dtDeductibles.Columns.Add("DEDUCT_VALUE", typeof(decimal));
            dtDeductibles.Columns.Add("DEDUCT_UNIT_CD", typeof(int));
            dtDeductibles.Columns.Add("DEDUCT_SUBJECT_CD", typeof(int));
            dtDeductibles.Columns.Add("MIN_DEDUCT_VALUE", typeof(decimal));
            dtDeductibles.Columns.Add("MIN_DEDUCT_UNIT_CD", typeof(int));
            dtDeductibles.Columns.Add("MIN_DEDUCT_SUBJECT_CD", typeof(int));
            dtDeductibles.Columns.Add("MAX_DEDUCT_VALUE", typeof(decimal));
            dtDeductibles.Columns.Add("MAX_DEDUCT_UNIT_CD", typeof(int));
            dtDeductibles.Columns.Add("MAX_DEDUCT_SUBJECT_CD", typeof(int));
            dtDeductibles.Columns.Add("CURRENCY_CD", typeof(int));
            dtDeductibles.Columns.Add("ACC_DEDUCT_AMT", typeof(decimal));
            dtDeductibles.Columns.Add("DEDUCT_ID", typeof(int));

            DataTable dtCoverageClauses = new DataTable("PARAM_TEMP_RISK_COVER_CLAUSE");
            dtCoverageClauses.Columns.Add("COVERAGE_ID", typeof(int));
            dtCoverageClauses.Columns.Add("CLAUSE_ID", typeof(int));
            dtCoverageClauses.Columns.Add("CLAUSE_STATUS_CD", typeof(int));
            dtCoverageClauses.Columns.Add("CLAUSE_ORIG_STATUS_CD", typeof(int));

            foreach (CompanyCoverage item in companyJudgement.Risk.Coverages)
            {
                DataRow dataRow = dtCoverages.NewRow();
                dataRow["RISK_COVER_ID"] = item.Id;
                dataRow["COVERAGE_ID"] = item.Id;
                dataRow["IS_DECLARATIVE"] = item.IsDeclarative;
                dataRow["IS_MIN_PREMIUM_DEPOSIT"] = item.IsMinPremiumDeposit;
                dataRow["FIRST_RISK_TYPE_CD"] = (int)Sistran.Core.Application.UnderwritingServices.Enums.FirstRiskType.None;
                dataRow["CALCULATION_TYPE_CD"] = (int)item.CalculationType.Value;
                dataRow["DECLARED_AMT"] = item.DeclaredAmount;
                dataRow["PREMIUM_AMT"] = item.PremiumAmount;
                dataRow["LIMIT_AMT"] = item.LimitAmount;
                dataRow["SUBLIMIT_AMT"] = item.SubLimitAmount;
                dataRow["LIMIT_IN_EXCESS"] = item.ExcessLimit;
                dataRow["LIMIT_OCCURRENCE_AMT"] = item.LimitOccurrenceAmount;
                dataRow["LIMIT_CLAIMANT_AMT"] = item.LimitClaimantAmount;
                dataRow["ACC_PREMIUM_AMT"] = item.AccumulatedPremiumAmount;
                dataRow["ACC_LIMIT_AMT"] = item.AccumulatedLimitAmount;
                dataRow["ACC_SUBLIMIT_AMT"] = item.AccumulatedSubLimitAmount;
                dataRow["CURRENT_FROM"] = item.CurrentFrom;
                dataRow["RATE_TYPE_CD"] = (int)item.RateType;
                dataRow["RATE"] = (object)item.Rate ?? DBNull.Value;
                dataRow["CURRENT_TO"] = item.CurrentTo;
                dataRow["COVER_NUM"] = item.Number;
                dataRow["COVER_STATUS_CD"] = item.CoverStatus.Value;
                if (item.CoverageOriginalStatus.HasValue)
                {
                    dataRow["COVER_ORIGINAL_STATUS_CD"] = (int)item.CoverageOriginalStatus.Value;
                }
                if (item.Text != null)
                {
                    dataRow["CONDITION_TEXT"] = item.Text.TextBody;
                }
                else
                {
                    dataRow["CONDITION_TEXT"] = DBNull.Value;
                }

                dataRow["ENDORSEMENT_LIMIT_AMT"] = item.EndorsementLimitAmount;
                dataRow["ENDORSEMENT_SUBLIMIT_AMT"] = item.EndorsementSublimitAmount;
                dataRow["FLAT_RATE_PCT"] = item.FlatRatePorcentage;
                dataRow["SHORT_TERM_PCT"] = item.ShortTermPercentage;
                dataRow["PREMIUM_AMT_DEPOSIT_PERCENT"] = item.DepositPremiumPercent;
                dataRow["MAX_LIABILITY_AMT"] = item.MaxLiabilityAmount;

                if (item.DynamicProperties != null && item.DynamicProperties.Count > 0)
                {
                    DynamicPropertiesCollection dynamicCollectionCoverage = new DynamicPropertiesCollection();
                    for (int i = 0; i < item.DynamicProperties.Count(); i++)
                    {
                        DynamicProperty dinamycProperty = new DynamicProperty();
                        dinamycProperty.Id = item.DynamicProperties[i].Id;
                        dinamycProperty.Value = item.DynamicProperties[i].Value;
                        dynamicCollectionCoverage[i] = dinamycProperty;
                    }

                    byte[] serializedValuesCoverage = dynamicPropertiesSerializer.Serialize(dynamicCollectionCoverage);
                    dataRow["DYNAMIC_PROPERTIES"] = serializedValuesCoverage;
                }

                if (item.Deductible != null)
                {
                    DataRow dataRowDeductible = dtDeductibles.NewRow();
                    dataRowDeductible["COVERAGE_ID"] = item.Id;
                    dataRowDeductible["RATE_TYPE_CD"] = (int)item.Deductible.RateType;
                    dataRowDeductible["RATE"] = (object)item.Deductible.Rate ?? DBNull.Value;
                    dataRowDeductible["DEDUCT_PREMIUM_AMT"] = item.Deductible.DeductPremiumAmount;
                    dataRowDeductible["DEDUCT_VALUE"] = item.Deductible.DeductValue;
                    if (item.Deductible.DeductibleUnit != null && item.Deductible.DeductibleUnit.Id != 0)
                    {
                        dataRowDeductible["DEDUCT_UNIT_CD"] = item.Deductible.DeductibleUnit.Id;
                    }
                    if (item.Deductible.DeductibleSubject != null)
                    {
                        dataRowDeductible["DEDUCT_SUBJECT_CD"] = item.Deductible.DeductibleSubject.Id;
                    }
                    if (item.Deductible.MinDeductValue.HasValue)
                    {
                        dataRowDeductible["MIN_DEDUCT_VALUE"] = item.Deductible.MinDeductValue.Value;
                    }
                    if (item.Deductible.MinDeductibleUnit != null && item.Deductible.MinDeductibleUnit.Id != 0)
                    {
                        dataRowDeductible["MIN_DEDUCT_UNIT_CD"] = item.Deductible.MinDeductibleUnit.Id;
                    }
                    if (item.Deductible.MinDeductibleSubject != null && item.Deductible.MinDeductibleSubject.Id != 0)
                    {
                        dataRowDeductible["MIN_DEDUCT_SUBJECT_CD"] = item.Deductible.MinDeductibleSubject.Id;
                    }
                    if (item.Deductible.MaxDeductValue.HasValue)
                    {
                        dataRowDeductible["MAX_DEDUCT_VALUE"] = item.Deductible.MaxDeductValue.Value;
                    }
                    if (item.Deductible.MaxDeductibleUnit != null && item.Deductible.MaxDeductibleUnit.Id != 0)
                    {
                        dataRowDeductible["MAX_DEDUCT_UNIT_CD"] = item.Deductible.MaxDeductibleUnit.Id;
                    }
                    if (item.Deductible.MaxDeductibleSubject != null && item.Deductible.MaxDeductibleSubject.Id != 0)
                    {
                        dataRowDeductible["MAX_DEDUCT_SUBJECT_CD"] = item.Deductible.MaxDeductibleSubject.Id;
                    }
                    if (item.Deductible.Currency != null)
                    {
                        dataRowDeductible["CURRENCY_CD"] = item.Deductible.Currency.Id;
                    }
                    dataRowDeductible["ACC_DEDUCT_AMT"] = item.Deductible.AccDeductAmt;
                    dataRowDeductible["DEDUCT_ID"] = item.Deductible.Id;

                    dtDeductibles.Rows.Add(dataRowDeductible);
                }
                if (item.Clauses != null && item.Clauses.Count > 0)
                {
                    foreach (var companyClause in item.Clauses)
                    {
                        DataRow dataRowCoverCluases = dtCoverageClauses.NewRow();
                        dataRowCoverCluases["COVERAGE_ID"] = item.Id;
                        dataRowCoverCluases["CLAUSE_ID"] = companyClause.Id;
                        dataRowCoverCluases["CLAUSE_STATUS_CD"] = (int)Sistran.Core.Application.CommonService.Enums.ClauseStatuses.Original;
                        dtCoverageClauses.Rows.Add(dataRowCoverCluases);
                    }
                }

                dtCoverages.Rows.Add(dataRow);
            }
            parameters[16] = new NameValue("@INSERT_TEMP_RISK_COVERAGE", dtCoverages);
            parameters[17] = new NameValue("@INSERT_TEMP_RISK_COVER_DEDUCT", dtDeductibles);

            DataTable dtClauses = new DataTable("PARAM_TEMP_CLAUSE");
            dtClauses.Columns.Add("CLAUSE_ID", typeof(int));
            dtClauses.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtClauses.Columns.Add("CLAUSE_STATUS_CD", typeof(int));
            dtClauses.Columns.Add("CLAUSE_ORIG_STATUS_CD", typeof(int));

            if (companyJudgement.Risk.Clauses != null)
            {
                foreach (CompanyClause item in companyJudgement.Risk.Clauses)
                {
                    DataRow dataRow = dtClauses.NewRow();
                    dataRow["CLAUSE_ID"] = item.Id;
                    dataRow["CLAUSE_STATUS_CD"] = (int)Sistran.Core.Application.CommonService.Enums.ClauseStatuses.Original;
                    dtClauses.Rows.Add(dataRow);
                }
            }

            parameters[18] = new NameValue("@INSERT_TEMP_CLAUSE", dtClauses);

            if (companyJudgement.Risk.DynamicProperties != null && companyJudgement.Risk.DynamicProperties.Count > 0)
            {
                DynamicPropertiesCollection dynamicCollectionRisk = new DynamicPropertiesCollection();
                for (int i = 0; i < companyJudgement.Risk.DynamicProperties.Count(); i++)
                {
                    DynamicProperty dinamycProperty = new DynamicProperty();
                    dinamycProperty.Id = companyJudgement.Risk.DynamicProperties[i].Id;
                    dinamycProperty.Value = companyJudgement.Risk.DynamicProperties[i].Value;
                    dynamicCollectionRisk[i] = dinamycProperty;
                }
                byte[] serializedValuesRisk = dynamicPropertiesSerializer.Serialize(dynamicCollectionRisk);
                parameters[19] = new NameValue("@DYNAMIC_PROPERTIES", serializedValuesRisk);
            }
            else
            {
                parameters[19] = new NameValue("@DYNAMIC_PROPERTIES", DBNull.Value);
            }
            if (companyJudgement?.Attorney?.IdentificationDocument?.Number != null && companyJudgement?.Attorney?.IdentificationDocument?.Number != "0" && companyJudgement?.Attorney?.IdentificationDocument?.DocumentType?.Id != null)
            {
                parameters[20] = new NameValue("@IDENTIFICATION_ID_AGENT", companyJudgement?.Attorney?.IdentificationDocument?.DocumentType?.Id);
                parameters[21] = new NameValue("@DOCUMENT_NUMBER_AGENT", companyJudgement.Attorney.IdentificationDocument.Number);
            }
            else
            {
                parameters[20] = new NameValue("@IDENTIFICATION_ID_AGENT", DBNull.Value);
                parameters[21] = new NameValue("@DOCUMENT_NUMBER_AGENT", DBNull.Value);
            }

            if (companyJudgement?.Attorney != null && companyJudgement?.Attorney?.InsuredToPrint != null)
            {
                parameters[22] = new NameValue("@INSURED_PRINT_NAME", companyJudgement?.Attorney?.InsuredToPrint);
            }
            else
            {
                parameters[22] = new NameValue("@INSURED_PRINT_NAME", DBNull.Value);
            }
            parameters[23] = new NameValue("@COURT_NUM", companyJudgement.Court.Description);
            parameters[24] = new NameValue("@ENDORSEMENT_ID", companyJudgement.Risk.Policy.Endorsement.Id);
            parameters[25] = new NameValue("@POLICY_ID", companyJudgement.Risk.Policy.Endorsement.PolicyId);
            parameters[26] = new NameValue("@PAYER_ID", companyJudgement.Risk.Policy.Holder.IndividualId);
            if (companyJudgement.Risk.RiskActivity != null && companyJudgement.Risk.RiskActivity.Id > 0)
            {
                parameters[27] = new NameValue("@COMM_RISK_CLASS_CD", companyJudgement.Risk.RiskActivity.Id);
            }
            else
            {
                parameters[27] = new NameValue("@COMM_RISK_CLASS_CD", DBNull.Value);
            }
            parameters[28] = new NameValue("@RISK_COMMERCIAL_TYPE_CD", DBNull.Value);
            if (companyJudgement.Risk.RatingZone != null && companyJudgement.Risk.RatingZone.Id > 0)
            {
                parameters[29] = new NameValue("@RATING_ZONE_CD", 1);
            }
            else
            {
                parameters[29] = new NameValue("@RATING_ZONE_CD", DBNull.Value);
            }
            if (companyJudgement.Risk.LimitRc != null && companyJudgement.Risk.LimitRc.Id > 0)
            {
                parameters[30] = new NameValue("@LIMITS_RC_CD", companyJudgement.Risk.LimitRc.Id);
            }
            else
            {
                parameters[30] = new NameValue("@LIMITS_RC_CD", DBNull.Value);
            }
            if (companyJudgement.Risk.LimitRc != null && companyJudgement.Risk.LimitRc.LimitSum > 0)
            {
                parameters[31] = new NameValue("@LIMIT_RC_SUM", companyJudgement.Risk.LimitRc.LimitSum);
            }
            else
            {
                parameters[31] = new NameValue("@LIMIT_RC_SUM", DBNull.Value);
            }
            parameters[32] = new NameValue("@SINISTER_PCT", DBNull.Value);
            if (companyJudgement.Risk.SecondInsured != null && companyJudgement.Risk.SecondInsured.IndividualId > 0)
            {
                parameters[33] = new NameValue("@SECONDARY_INSURED_ID", companyJudgement.Risk.SecondInsured.IndividualId);
            }
            else
            {
                parameters[33] = new NameValue("@SECONDARY_INSURED_ID", DBNull.Value);
            }
            parameters[34] = new NameValue("@ACTUAL_DATE", DateTime.Now);

            DataTable dtDynamicProperties = new DataTable("PARAM_TEMP_DYNAMIC_PROPERTIES");
            dtDynamicProperties.Columns.Add("DYNAMIC_ID", typeof(int));
            dtDynamicProperties.Columns.Add("CONCEPT_VALUE", typeof(string));

            if (companyJudgement.Risk.DynamicProperties != null)
            {
                foreach (DynamicConcept item in companyJudgement.Risk.DynamicProperties)
                {
                    DataRow dataRow = dtDynamicProperties.NewRow();
                    dataRow["DYNAMIC_ID"] = item.Id;
                    dataRow["CONCEPT_VALUE"] = item.Value ?? "NO ASIGNADO";
                    dtDynamicProperties.Rows.Add(dataRow);
                }
            }

            parameters[35] = new NameValue("@INSERT_TEMP_DYNAMIC_PROPERTIES", dtDynamicProperties);

            DataTable dtDynamicPropertiesCoverage = new DataTable("PARAM_TEMP_DYNAMIC_PROPERTIES");
            dtDynamicPropertiesCoverage.Columns.Add("DYNAMIC_ID", typeof(int));
            dtDynamicPropertiesCoverage.Columns.Add("CONCEPT_VALUE", typeof(string));
            parameters[36] = new NameValue("@INSERT_TEMP_DYNAMIC_PROPERTIES_COVERAGE", dtDynamicPropertiesCoverage);
            parameters[37] = new NameValue("@RISK_NUM", companyJudgement.Risk.Number);
            parameters[38] = new NameValue("@RISK_INSP_TYPE_CD", 1);
            parameters[39] = new NameValue("@INSPECTION_ID", DBNull.Value);
            parameters[40] = new NameValue("@OPERATION", JsonConvert.SerializeObject(companyJudgement));
            parameters[41] = new NameValue("@HOLDER_CAPACITY_OF_CD", (int)companyJudgement.HolderActAs);
            if (companyJudgement?.Attorney?.IdProfessionalCard != null)
            {
                parameters[42] = new NameValue("@PROFESIONAL_CARD_ID", companyJudgement.Attorney.IdProfessionalCard);
            }
            else
            {
                parameters[42] = new NameValue("@PROFESIONAL_CARD_ID", DBNull.Value);
            }

            if (companyJudgement.Risk.MainInsured.CompanyName.Address != null)
            {
                parameters[43] = new NameValue("@ADDRESS_ID", companyJudgement.Risk.MainInsured.CompanyName.Address.Id);
            }
            else
                parameters[43] = new NameValue("@ADDRESS_ID", 1);

            if (companyJudgement.Risk.MainInsured.CompanyName.Phone != null)
            {
                parameters[44] = new NameValue("@PHONE_ID", companyJudgement.Risk.MainInsured.CompanyName.Phone.Id);
            }
            else
                parameters[44] = new NameValue("@PHONE_ID", 1);

            if (companyJudgement?.Attorney != null && companyJudgement?.Attorney?.Name != null)
            {
                parameters[45] = new NameValue("@INSURED_CAUTION", companyJudgement?.Attorney?.Name);
            }
            else
            {
                parameters[45] = new NameValue("@INSURED_CAUTION", DBNull.Value);
            }
            parameters[46] = new NameValue("@INSERT_TEMP_RISK_COVER_CLAUSE", dtCoverageClauses);

            DataTable dtGuarantees = new DataTable("INSERT_TEMP_CROSS_GUARANTEES");

            dtGuarantees.Columns.Add("RISK_ID", typeof(int));
            dtGuarantees.Columns.Add("GUARANTEE_ID", typeof(int));

            if (companyJudgement.Guarantees != null && companyJudgement.Guarantees.Count > 0)
            {
                foreach (var guarantee_item in companyJudgement.Guarantees)
                {
                    DataRow dataRowGuarantee = dtGuarantees.NewRow();
                    dataRowGuarantee["RISK_ID"] = companyJudgement.Risk.RiskId;
                    dataRowGuarantee["GUARANTEE_ID"] = guarantee_item.InsuredGuarantee.Id;
                    dtGuarantees.Rows.Add(dataRowGuarantee);
                }
            }
            parameters[47] = new NameValue("@INSERT_TEMP_CROSS_GUARANTEES", dtGuarantees);

            DataTable result;


            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("ISS.RECORD_RISK_JUDICIAL_SURETY", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                string error = result.Rows[0][0].ToString();
                if (!string.IsNullOrEmpty(error) && !Convert.ToBoolean(result.Rows[0][0]))
                {

                    throw new Exception(error);
                }
            }
            else
            {
                throw new ValidationException(Errors.ErrorRecordEndorsement);
            }
        }

        private void ValidateInfringementPolicies(CompanyPolicy companyPolicy, List<CompanyJudgement> companyJudgements)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();

            infringementPolicies.AddRange(companyPolicy.InfringementPolicies);
            companyJudgements.ForEach(x => infringementPolicies.AddRange(x.Risk.InfringementPolicies));

            companyPolicy.InfringementPolicies = DelegateService.AuthorizationPoliciesServiceCore.ValidateInfringementPolicies(infringementPolicies);
        }

        public int SaveCompanyJudicialTemporalTables(CompanyJudgement companyJudgement)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer =
            new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();
            UTILITES.GetDatatables d = new UTILITES.GetDatatables();

            UTILITES.CommonDataTables dts = d.GetcommonDataTablesMa(companyJudgement.Risk);

            DataTable dataTable;
            NameValue[] parameters = new NameValue[11];

            DataTable dtTempRisk = dts.dtTempRisk;

            parameters[0] = new NameValue(dtTempRisk.TableName, dtTempRisk);

            DataTable dtCOTempRisk = dts.dtCOTempRisk;
            parameters[1] = new NameValue(dtCOTempRisk.TableName, dtCOTempRisk);

            DataTable dtBeneficary = dts.dtBeneficary;
            parameters[2] = new NameValue(dtBeneficary.TableName, dtBeneficary);

            DataTable dtRiskPayer = dts.dtRiskPayer;
            parameters[3] = new NameValue(dtRiskPayer.TableName, dtRiskPayer);

            DataTable dtClause = dts.dtClause;
            parameters[4] = new NameValue(dtClause.TableName, dtClause);

            DataTable dtCoverClause = dts.dtCoverClause;
            parameters[5] = new NameValue(dtCoverClause.TableName, dtCoverClause);


            DataTable dtDeduct = dts.dtDeduct;
            parameters[6] = new NameValue(dtDeduct.TableName, dtDeduct);


            DataTable dtRiskClause = dts.dtRiskClause;
            parameters[7] = new NameValue(dtRiskClause.TableName, dtRiskClause);

            DataTable dtDynamic = dts.dtDynamic;
            parameters[8] = new NameValue("INSERT_TEMP_DYNAMIC_PROPERTIES_RISK", dtDynamic);

            DataTable dtDynamicCoverage = dts.dtDynamicCoverage;
            parameters[9] = new NameValue("INSERT_TEMP_DYNAMIC_PROPERTIES_COVERAGE", dtDynamicCoverage);

            DataTable dtTempRiskJudicialSurety = ModelAssembler.GetDataTableRiskJudicialSurety(companyJudgement);
            parameters[10] = new NameValue(dtTempRiskJudicialSurety.TableName, dtTempRiskJudicialSurety);
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {

                dataTable = pdb.ExecuteSPDataTable("TMP.SAVE_TEMPORAL_RISK_JUDICIAL_SURETY_TEMP", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (companyJudgement.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Modification)
                {
                    companyJudgement.Risk.RiskId = Convert.ToInt32(dataTable.Rows[0][0]);
                }
                return companyJudgement.Risk.RiskId;
            }
            else
            {
                throw new ValidationException(Errors.ErrorCreateTemporalCompanyJudicial);//ErrrRecordTemporal "error al grabar riesgo
            }
        }
    }
}