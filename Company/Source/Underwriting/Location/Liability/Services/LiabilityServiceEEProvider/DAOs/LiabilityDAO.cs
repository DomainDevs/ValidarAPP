using Newtonsoft.Json;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.Location.LiabilityServices.EEProvider.Assemblers;
using Sistran.Company.Application.Location.LiabilityServices.EEProvider.Entities.View;
using Sistran.Company.Application.Location.LiabilityServices.EEProvider.Resources;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Application.Utilities.Helper;
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
using Sistran.Company.Application.Location.LiabilityServices.EEProvider.Entities;
using UTILITIES = Company.UnderwritingUtilities;
using Sistran.Company.Application.Location.LiabilityServices.EEProvider.BusinessModels;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using System.Diagnostics;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota;
using Sistran.Core.Integration.OperationQuotaServices.Enums;
using Sistran.Core.Application.Utilities.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;
using Sistran.Company.Application.Location.LiabilityServices.EEProvider.Enums;
using Sistran.Company.Application.Utilities.RulesEngine;
using PER = Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Company.Application.ExternalProxyServices.Models;
using System.Threading;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Company.Application.Location.LiabilityServices.EEProvider.DAOs
{
    public class LiabilityDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="riskActivity"></param>
        /// <returns></returns>
        public List<CompanyRiskSubActivity> GetRiskSubActivitiesByActivity(int riskActivity)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(RiskCommercialType.Properties.RiskCommercialClassCode, typeof(RiskCommercialType).Name);
            filter.Equal();
            filter.Constant(riskActivity);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RiskCommercialType), filter.GetPredicate()));
            return ModelAssembler.CreateRisksubActivities(businessCollection);
        }

        public List<CompanyAssuranceMode> GetRiskAssuranceMode()
        {
            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(InsuranceMode));


            return ModelAssembler.CreateAssunaceMode(businessCollection);
        }

        /// <summary>
        /// Obtener Poliza de hogar
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>propertyPolicy</returns>
        public List<CompanyLiabilityRisk> GetCompanyLiabilitysByPolicyId(int policyId)
        {
            ConcurrentBag<CompanyLiabilityRisk> companyPropertyRisks = new ConcurrentBag<CompanyLiabilityRisk>();

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
            RiskLiabilityView view = new RiskLiabilityView();
            ViewBuilder builder = new ViewBuilder("RiskLiabilityView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();

            if (view.Risks.Count > 0)
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.RiskLocation> riskLocations = view.RiskLocations.Cast<ISSEN.RiskLocation>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<ISSEN.RiskClause> riskClauses = view.RiskClauses.Cast<ISSEN.RiskClause>().ToList();
                if (risks != null && risks.Any())
                {
                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    Parallel.For(0, risks.Count, ParallelHelper.DebugParallelFor(), row =>
                      {
                          var item = risks[row];
                          DataFacadeManager.Instance.GetDataFacade().LoadDynamicProperties(item);
                          CompanyLiabilityRisk liabilityRisk = new CompanyLiabilityRisk();

                          liabilityRisk = ModelAssembler.CreateLiabilityRisk(item, riskLocations.Where(x => x.RiskId == item.RiskId).First(), endorsementRisks.Where(x => x.RiskId == item.RiskId).First());

                          int insuredNameNum = liabilityRisk.Risk.MainInsured.CompanyName.NameNum;
                          var insured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(liabilityRisk.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                          if (insured != null)
                          {
                              //ModelAssembler.CreateMapPersonInsured();
                              liabilityRisk.Risk.MainInsured = insured;
                              var companyName = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(liabilityRisk.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
                              liabilityRisk.Risk.MainInsured.CompanyName = new IssuanceCompanyName
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
                                      Id = companyName.Email != null ? companyName.Email.Id : 0,
                                      Description = companyName.Email != null ? companyName.Email.Description : ""
                                  }
                              };
                          }

                          liabilityRisk.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                          if (riskBeneficiaries != null && riskBeneficiaries.Count > 0)
                          {
                              liabilityRisk.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                              Object objlock = new object();
                              var imapper = AutoMapperAssembler.CreateMapCompanyBeneficiary();
                              TP.Parallel.ForEach(riskBeneficiaries.Where(x => x.RiskId == item.RiskId), riskBeneficiary =>
                              {
                                  CompanyBeneficiary CiaBeneficiary = new CompanyBeneficiary();
                                  var beneficiaryRisk = DelegateService.underwritingService.GetBeneficiariesByDescription(riskBeneficiary.BeneficiaryId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                                  if (beneficiaryRisk != null)
                                  {
                                      CiaBeneficiary = imapper.Map<Beneficiary, CompanyBeneficiary>(beneficiaryRisk);
                                      CiaBeneficiary.CustomerType = CustomerType.Individual;
                                      CiaBeneficiary.BeneficiaryType = new CompanyBeneficiaryType { Id = riskBeneficiary.BeneficiaryTypeCode };
                                      CiaBeneficiary.Participation = riskBeneficiary.BenefitPercentage;
                                      List<CompanyName> companyNames = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(CiaBeneficiary.IndividualId, CiaBeneficiary.CustomerType);
                                      var companyName = new CompanyName();
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
                                                  Id = companyName.Email != null ? companyName.Email.Id : 0,
                                                  Description = companyName.Email != null ? companyName.Email.Description : ""
                                              }
                                          };
                                          liabilityRisk.Risk.Beneficiaries.Add(CiaBeneficiary);
                                      }
                                  }
                              });
                          }

                          ConcurrentBag<CompanyClause> clauses = new ConcurrentBag<CompanyClause>();
                          //clausulas
                          TP.Parallel.ForEach(riskClauses.Where(x => x.RiskId == item.RiskId), riskClause =>
                          {
                              clauses.Add(new CompanyClause { Id = riskClause.ClauseId });
                          });

                          liabilityRisk.Risk.Clauses = clauses.ToList();
                          var coverageProduct = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementRisks.First(x => x.RiskId == item.RiskId).EndorsementId, item.RiskId);
                          if (coverageProduct != null)
                          {
                              liabilityRisk.Risk.Coverages = coverageProduct;
                          }
                          else
                          {
                              errors.Add("No existe Coberturas parametrizadas");
                          }

                          companyPropertyRisks.Add(liabilityRisk);
                      });
                    if (errors != null && errors.Any())
                    {
                        throw new Exception(string.Join(" : ", errors));
                    }
                }
                else
                {
                    throw new Exception("Error Riesgo no Existe");
                }

            }

            return companyPropertyRisks.ToList();
        }
        /// <summary>
        /// Obtener Poliza de responsabilidad civil
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>liabilityPolicy</returns>
        public List<CompanyLiabilityRisk> GetCompanyLiabilitiesByEndorsementId(int endorsementId)
        {
            List<CompanyLiabilityRisk> companyLiabilityRisk = new List<Models.CompanyLiabilityRisk>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            RiskLiabilityView view = new RiskLiabilityView();
            ViewBuilder builder = new ViewBuilder("RiskLiabilityView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();

            if (view.Risks.Count > 0)
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.RiskLocation> riskLocations = view.RiskLocations.Cast<ISSEN.RiskLocation>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<ISSEN.RiskClause> riskClauses = view.RiskClauses.Cast<ISSEN.RiskClause>().ToList();

                foreach (ISSEN.Risk item in risks)
                {
                    dataFacade.LoadDynamicProperties(item);
                    CompanyLiabilityRisk liabilityRisk = new CompanyLiabilityRisk();

                    ModelAssembler.CreateLiabilityRisk(item,
                       riskLocations.Where(x => x.RiskId == item.RiskId).First(),
                       endorsementRisks.Where(x => x.RiskId == item.RiskId).First());

                    int insuredNameNum = liabilityRisk.Risk.MainInsured.CompanyName.NameNum;
                    liabilityRisk.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(liabilityRisk.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);


                    var companyName = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(liabilityRisk.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
                    liabilityRisk.Risk.MainInsured.CompanyName = new IssuanceCompanyName
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
                            Id = companyName.Email != null ? companyName.Email.Id : 0,
                            Description = companyName.Email != null ? companyName.Email.Description : ""
                        }
                    };


                    liabilityRisk.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                    foreach (ISSEN.RiskBeneficiary riskBeneficiary in riskBeneficiaries.Where(x => x.RiskId == item.RiskId))
                    {
                        CompanyBeneficiary beneficiary = new CompanyBeneficiary();
                        beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(riskBeneficiary.BeneficiaryId.ToString(), InsuredSearchType.IndividualId).Cast<CompanyBeneficiary>().First();
                        beneficiary.BeneficiaryType = new CompanyBeneficiaryType { Id = riskBeneficiary.BeneficiaryTypeCode };
                        companyName = new CompanyName();
                        companyName = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(beneficiary.IndividualId, beneficiary.CustomerType).First();
                        beneficiary.CompanyName = new IssuanceCompanyName
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
                                Id = companyName.Email != null ? companyName.Email.Id : 0,
                                Description = companyName.Email != null ? companyName.Email.Description : ""
                            }
                        };

                        liabilityRisk.Risk.Beneficiaries.Add(beneficiary);
                    }

                    List<CompanyClause> clauses = new List<CompanyClause>();
                    //clausulas
                    foreach (ISSEN.RiskClause riskClause in riskClauses.Where(x => x.RiskId == item.RiskId))
                    {
                        clauses.Add(new CompanyClause { Id = riskClause.ClauseId });
                    }
                    liabilityRisk.Risk.Clauses = clauses;

                    liabilityRisk.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(liabilityRisk.Risk.Policy.Id, endorsementId, item.RiskId);

                    companyLiabilityRisk.Add(liabilityRisk);
                }
            }

            return companyLiabilityRisk;
        }

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="liabilityRisk">Modelo liabilityRisk</param>
        public CompanyLiabilityRisk CreateLiabilityTemporal(CompanyLiabilityRisk liabilityRisk, bool isMassive)
        {
            liabilityRisk.Risk.InfringementPolicies = ValidateAuthorizationPolicies(liabilityRisk);

            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "true";
            PendingOperation pendingOperation = new PendingOperation();

            if (liabilityRisk.Risk.Id == 0)
            {
                pendingOperation.CreationDate = DateTime.Now;
                pendingOperation.ParentId = liabilityRisk.Risk.Policy.Id;
                pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(liabilityRisk);

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
                pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(liabilityRisk.Risk.Id);
                if (pendingOperation != null)

                {
                    pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(liabilityRisk);

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

            liabilityRisk.Risk.Id = pendingOperation.Id;
            return liabilityRisk;
        }

        public List<PoliciesAut> ValidateAuthorizationPolicies(CompanyLiabilityRisk companyLiabilityRisk)
        {
            Rules.Facade facade = new Rules.Facade();
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();
            if (companyLiabilityRisk != null && companyLiabilityRisk.Risk.Policy != null)
            {
                var key = companyLiabilityRisk.Risk.Policy.Prefix.Id + "," + (int)companyLiabilityRisk.Risk.Policy.Product.CoveredRisk.CoveredRiskType;

                facade = DelegateService.underwritingService.CreateFacadeGeneral(companyLiabilityRisk.Risk.Policy);
                facade.SetConcept(CompanyRuleConceptPolicies.UserId, companyLiabilityRisk.Risk.Policy.UserId);

                if ((companyLiabilityRisk.Risk.MainInsured.AssociationType != null && companyLiabilityRisk.Risk.MainInsured.AssociationType.Id == 0) || companyLiabilityRisk.Risk.MainInsured.AssociationType == null)
                {
                    int LiabilityAssociationType = GetDataAssociationType(companyLiabilityRisk.Risk.MainInsured.IndividualId);
                    if (companyLiabilityRisk.Risk.MainInsured.AssociationType == null)
                    {
                        companyLiabilityRisk.Risk.MainInsured.AssociationType = new IssuanceAssociationType();
                    }

                    companyLiabilityRisk.Risk.MainInsured.AssociationType.Id = LiabilityAssociationType == 0 ? 1 : LiabilityAssociationType;
                }
                /*Politica del riesgo*/
                EntityAssembler.CreateFacadeRiskLiability(facade, companyLiabilityRisk);
                policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_RISK));

                /*Politicas de cobertura*/
                if (companyLiabilityRisk.Risk.Coverages != null)
                {
                    foreach (var coverage in companyLiabilityRisk.Risk.Coverages)
                    {
                        EntityAssembler.CreateFacadeCoverage(facade, coverage);
                        policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_COVERAGE));
                    }
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

        public List<CompanyLiabilityRisk> GetCompanyLiabilitiesByTemporalId(int temporalId)
        {
            List<CompanyLiabilityRisk> companyContract = new List<CompanyLiabilityRisk>();
            List<PendingOperation> pendingOperations = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(temporalId);

            foreach (PendingOperation pendingOperation in pendingOperations)
            {
                CompanyLiabilityRisk risk = COMUT.JsonHelper.DeserializeJson<CompanyLiabilityRisk>(pendingOperation.Operation);
                risk.Risk.Id = pendingOperation.Id;
                companyContract.Add(risk);
            }

            return companyContract;
        }

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyLiabilityRisk> companyLiabilities)
        {
            if (companyPolicy == null || companyLiabilities == null || companyLiabilities.Count < 1)
            {
                throw new ArgumentException("Poliza y Riesgos Vacios");
            }
            ValidateInfringementPolicies(companyPolicy, companyLiabilities);
            if (companyPolicy.InfringementPolicies != null && companyPolicy.InfringementPolicies.Count == 0)
            {
                if (companyPolicy.Endorsement.EndorsementType != EndorsementType.LastEndorsementCancellation && companyPolicy.ExchangeRate.Currency.Id != (int)EnumExchangeRateCurrency.CURRENCY_PESOS)
                {
                    ExchangeRate exchangeRate = new ExchangeRate();
                    exchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DateTime.Now, companyPolicy.ExchangeRate.Currency.Id);

                    if (exchangeRate.RateDate == DateTime.Now.Date)
                    {
                        if (companyPolicy.Endorsement.CancellationTypeId != 1)
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
                    int operationId = companyPolicy.Id;

                    EndorsementType endorsementType = (EndorsementType)endorsementTypeId;
                    try
                    {

                        TP.Parallel.ForEach(companyLiabilities, companyLiability =>
                        {
                            companyLiability.Risk.Policy = companyPolicy;
                            if (companyLiability.Risk.Number == 0 && companyLiability.Risk.Status == RiskStatusType.Original || companyLiability.Risk.Status == RiskStatusType.Included)
                            {
                                companyLiability.Risk.Number = ++maxRiskCount;
                            }
                        });

                        object objeto = new object();
                        ConcurrentBag<string> errors = new ConcurrentBag<string>();
                        Parallel.ForEach(companyLiabilities, ParallelHelper.DebugParallelFor(), companyLiability =>
                        {
                            try
                            {
                                CreateCompanyLiability(companyLiability);

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
                        DelegateService.underwritingService.CreateCompanyPolicyPayer(companyPolicy);
                        try
                        {
                            DelegateService.underwritingService.DeleteTemporalByOperationId(companyPolicy.Id, 0, 0, 0);
                            try
                            {
                                companyPolicy.PolicyOrigin = companyPolicy.PolicyOrigin == 0 ? PolicyOrigin.Individual : companyPolicy.PolicyOrigin;
                                DelegateService.underwritingService.SaveControlPolicy(policyId, endorsementId, operationId, (int)companyPolicy.PolicyOrigin);
                                if (DelegateService.commonService.GetParameterByParameterId((int)LiabilityKeys.UND_ENABLED_REINSURANCE).BoolParameter.GetValueOrDefault())
                                {
                                    companyPolicy.IsReinsured = DelegateService.underwritingReinsuranceWorkerIntegration.ReinsuranceIssue(policyId, endorsementId, companyPolicy.UserId) > 0;
                                }
                                else
                                {
                                    var stopwatch = Stopwatch.StartNew();
                                    Thread.Sleep(TimeSpan.FromSeconds(5));
                                    stopwatch.Stop();
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

        private void ValidateInfringementPolicies(CompanyPolicy companyPolicy, List<CompanyLiabilityRisk> companyLiabilities)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();

            infringementPolicies.AddRange(companyPolicy.InfringementPolicies);
            companyLiabilities.ForEach(x => infringementPolicies.AddRange(x.Risk.InfringementPolicies));

            companyPolicy.InfringementPolicies = DelegateService.AuthorizationPoliciesServiceCore.ValidateInfringementPolicies(infringementPolicies);
        }

        public void CreateCompanyLiability(CompanyLiabilityRisk companyLiability)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();
            NameValue[] parameters = new NameValue[72];
            parameters[0] = new NameValue("@ENDORSEMENT_ID", companyLiability.Risk.Policy.Endorsement.Id);
            parameters[1] = new NameValue("@POLICY_ID", companyLiability.Risk.Policy.Endorsement.PolicyId);
            parameters[2] = new NameValue("@PAYER_ID", companyLiability.Risk.Policy.Holder.IndividualId);
            if (companyLiability.PML != null)
            {
                parameters[3] = new NameValue("@EML_PCT", companyLiability.PML.Value);
            }
            else
            {
                parameters[3] = new NameValue("@EML_PCT", 0);
            }
            parameters[4] = new NameValue("@STREET_TYPE_CD", 1);
            if (companyLiability.FullAddress != null)
            {
                parameters[5] = new NameValue("@STREET", companyLiability.FullAddress);
            }
            else
            {
                throw new Exception(Errors.ErrorAddressEmpty);
            }

            if (companyLiability.NomenclatureAddress?.ApartmentOrOffice != null)
            {
                parameters[6] = new NameValue("@APARTMENT", (companyLiability.NomenclatureAddress.ApartmentOrOffice.Id).ToString());
            }
            else
            {
                parameters[6] = new NameValue("@APARTMENT", 1.ToString());
            }
            parameters[7] = new NameValue("@ZIP_CODE", string.Empty);
            parameters[8] = new NameValue("@URBANIZATION", string.Empty);
            parameters[9] = new NameValue("@IS_MAIN", false);

            parameters[10] = new NameValue("@CITY_CD", companyLiability.City.Id);
            parameters[11] = new NameValue("@HOUSE_NUMBER", DBNull.Value, DbType.Int32);
            parameters[12] = new NameValue("@STATE_CD", companyLiability.City.State.Id);
            parameters[13] = new NameValue("@COUNTRY_CD", companyLiability.City.State.Country.Id);

            parameters[14] = new NameValue("@OCCUPATION_TYPE_CD", DBNull.Value, DbType.Int16);
            if (companyLiability.Risk.RiskActivity == null || companyLiability.Risk.RiskActivity.Id == 0)
            {
                parameters[15] = new NameValue("@COMM_RISK_CLASS_CD", DBNull.Value, DbType.Int16);
            }
            else
            {
                parameters[15] = new NameValue("@COMM_RISK_CLASS_CD", companyLiability.Risk.RiskActivity.Id);
            }

            if (companyLiability.RiskSubActivity == null)
            {
                parameters[16] = new NameValue("@RISK_COMMERCIAL_TYPE_CD", DBNull.Value, DbType.Int16);
            }
            else
            {
                parameters[16] = new NameValue("@RISK_COMMERCIAL_TYPE_CD", companyLiability.RiskSubActivity.Id, DbType.Int16);
            }

            parameters[17] = new NameValue("@RISK_COMM_SUBTYPE_CD", DBNull.Value, DbType.Int16);
            if (companyLiability.Square != null)
            {
                parameters[18] = new NameValue("@BLOCK", companyLiability.Square);
            }
            else
            {
                parameters[18] = new NameValue("@BLOCK", DBNull.Value, DbType.String);
            }
            if (companyLiability.RiskType != null && companyLiability.RiskType.Id != 0)
            {
                parameters[19] = new NameValue("@RISK_TYPE_CD", companyLiability.RiskType.Id);
            }
            else
            {
                parameters[19] = new NameValue("@RISK_TYPE_CD", (int)CoveredRiskType.Location);
            }
            parameters[20] = new NameValue("@RISK_AGE", companyLiability.RiskAge);
            parameters[21] = new NameValue("@IS_RETENTION", companyLiability.Risk.IsRetention);
            parameters[22] = new NameValue("@INSPECTION_RECOMENDATION", false);

            DataTable dtInsuredObjects = new DataTable("PARAM_TEMP_RISK_INSURED_OBJECT");
            dtInsuredObjects.Columns.Add("INSURED_OBJECT_ID", typeof(int));
            dtInsuredObjects.Columns.Add("INSURED_VALUE", typeof(decimal));
            dtInsuredObjects.Columns.Add("INSURED_PCT", typeof(decimal));
            dtInsuredObjects.Columns.Add("INSURED_RATE", typeof(decimal));

            foreach (CompanyCoverage item in companyLiability.Risk.Coverages)
            {
                if (dtInsuredObjects.AsEnumerable().All(x => x.Field<int>("INSURED_OBJECT_ID") != item.InsuredObject.Id))
                {
                    if (item.InsuredObject != null)
                    {
                        DataRow dataRow = dtInsuredObjects.NewRow();
                        dataRow["INSURED_OBJECT_ID"] = item.InsuredObject.Id;
                        dataRow["INSURED_VALUE"] = item.InsuredObject.Amount;

                        if (companyLiability.Risk.MainInsured != null && companyLiability.Risk.Coverages != null && companyLiability.Risk.Coverages.Any())
                        {
                            CompanyCoverage companyCoverage = companyLiability.Risk.Coverages.Find(x => x.Id == item.Id);
                            if (companyCoverage != null)
                            {
                                CompanyInsuredObject companyInsuredObject = companyCoverage.InsuredObject;
                                dataRow["INSURED_PCT"] = 0;
                                dataRow["INSURED_RATE"] = 0;
                            }
                        }
                        dtInsuredObjects.Rows.Add(dataRow);
                    }
                }
            }

            parameters[23] = new NameValue("@INSERT_TEMP_RISK_INSURED_OBJECT", dtInsuredObjects);

            parameters[24] = new NameValue("@INSURED_ID", companyLiability.Risk.MainInsured.IndividualId);
            parameters[25] = new NameValue("@COVERED_RISK_TYPE_CD", Convert.ToInt16(companyLiability.Risk.CoveredRiskType.Value));

            if (companyLiability.Risk.Status != null)
            {
                parameters[26] = new NameValue("@RISK_STATUS_CD", Convert.ToInt16(companyLiability.Risk.Status));

            }
            else
            {
                parameters[26] = new NameValue("@RISK_STATUS_CD", DBNull.Value, DbType.Int16);
            }

            if (companyLiability.Risk.Text == null)
            {
                parameters[27] = new NameValue("@CONDITION_TEXT", DBNull.Value, DbType.String);
            }
            else
            {
                if (companyLiability.Risk.Text.TextBody == null)
                {
                    parameters[27] = new NameValue("@CONDITION_TEXT", DBNull.Value, DbType.String);
                }
                else
                {
                    while (companyLiability.Risk.Text.TextBody.IndexOf("'") > 0)
                    {
                        int posicion = companyLiability.Risk.Text.TextBody.IndexOf("'");
                        string parte1 = companyLiability.Risk.Text.TextBody.Substring(0, posicion);
                        parte1 = parte1 + " ";
                        string parte2 = companyLiability.Risk.Text.TextBody.Substring(posicion + 1);
                        companyLiability.Risk.Text.TextBody = parte1 + parte2;
                    }
                    parameters[27] = new NameValue("@CONDITION_TEXT", companyLiability.Risk.Text.TextBody, DbType.String);
                }
            }

            if (companyLiability.Risk.RatingZone != null)
            {
                parameters[28] = new NameValue("@RATING_ZONE_CD", companyLiability.Risk.RatingZone.Id);
            }
            else
            {
                parameters[28] = new NameValue("@RATING_ZONE_CD", DBNull.Value, DbType.Int16);
            }

            parameters[29] = new NameValue("@COVER_GROUP_ID", companyLiability.Risk.GroupCoverage.Id);
            parameters[30] = new NameValue("@IS_FACULTATIVE", companyLiability.IsDeclarative);

            if (companyLiability.Risk.MainInsured.CompanyName.NameNum > 0)
            {
                parameters[31] = new NameValue("@NAME_NUM", companyLiability.Risk.MainInsured.CompanyName.NameNum);
            }
            else
            {
                parameters[31] = new NameValue("@NAME_NUM", DBNull.Value, DbType.Int16);
            }

            parameters[32] = new NameValue("@LIMITS_RC_CD", 0);
            parameters[33] = new NameValue("@LIMIT_RC_SUM", 0);

            parameters[34] = new NameValue("@LONGITUDE_EARTHQUAKE", companyLiability.Longitude);
            parameters[35] = new NameValue("@LATITUDE_EARTHQUAKE", companyLiability.Latitude);
            parameters[36] = new NameValue("@CONSTRUCTION_YEAR_EARTHQUAKE", companyLiability.ConstructionYear);
            parameters[37] = new NameValue("@FLOOR_NUMBER_EARTHQUAKE", companyLiability.FloorNumber);

            if (companyLiability.ConstructionType != null && companyLiability.ConstructionType.Id > 0)
            {
                parameters[38] = new NameValue("@CONSTRUCTION_CATEGORY_CD", companyLiability.ConstructionType.Id);
            }
            else
            {
                parameters[38] = new NameValue("@CONSTRUCTION_CATEGORY_CD", 1);
            }

            if (companyLiability.Risk.SecondInsured != null && companyLiability.Risk.SecondInsured.IndividualId > 0)
            {
                parameters[39] = new NameValue("@SECONDARY_INSURED_ID", companyLiability.Risk.SecondInsured.IndividualId);
            }
            else
            {
                parameters[39] = new NameValue("@SECONDARY_INSURED_ID", DBNull.Value, DbType.Int32);
            }

            parameters[40] = new NameValue("@ACTUAL_DATE", DateTime.Now);

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
            dtDeductibles.Columns.Add("DEDUCT_PREMIUM_AMT", typeof(int));
            dtDeductibles.Columns.Add("DEDUCT_VALUE", typeof(long));
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

            foreach (CompanyCoverage item in companyLiability.Risk.Coverages)
            {
                DataRow dataRow = dtCoverages.NewRow();
                dataRow["RISK_COVER_ID"] = item.Id;
                dataRow["COVERAGE_ID"] = item.Id;
                dataRow["IS_DECLARATIVE"] = item.IsDeclarative;
                dataRow["IS_MIN_PREMIUM_DEPOSIT"] = item.IsMinPremiumDeposit;
                dataRow["FIRST_RISK_TYPE_CD"] = (int)Sistran.Core.Application.UnderwritingServices.Enums.FirstRiskType.None;
                dataRow["CALCULATION_TYPE_CD"] = item.CalculationType.Value;
                dataRow["DECLARED_AMT"] = item.DeclaredAmount;
                dataRow["PREMIUM_AMT"] = Math.Round(item.PremiumAmount, 2);

                if (item.IsPrimary)
                {
                    dataRow["LIMIT_AMT"] = item.LimitAmount;
                    dataRow["SUBLIMIT_AMT"] = item.SubLimitAmount;
                }
                else
                {
                    dataRow["LIMIT_AMT"] = 0;
                    dataRow["SUBLIMIT_AMT"] = item.SubLimitAmount;
                }
                dataRow["LIMIT_IN_EXCESS"] = item.ExcessLimit;
                dataRow["LIMIT_OCCURRENCE_AMT"] = item.LimitOccurrenceAmount;
                dataRow["LIMIT_CLAIMANT_AMT"] = item.LimitClaimantAmount;
                dataRow["ACC_PREMIUM_AMT"] = item.AccumulatedPremiumAmount;
                dataRow["ACC_LIMIT_AMT"] = item.AccumulatedLimitAmount;
                dataRow["ACC_SUBLIMIT_AMT"] = item.AccumulatedSubLimitAmount;
                dataRow["CURRENT_FROM"] = item.CurrentFrom;
                dataRow["RATE_TYPE_CD"] = item.RateType;
                dataRow["RATE"] = (object)item.Rate ?? DBNull.Value;
                dataRow["CURRENT_TO"] = item.CurrentTo;
                dataRow["COVER_NUM"] = item.Number;
                dataRow["COVER_STATUS_CD"] = item.CoverStatus.Value;
                dataRow["MAX_LIABILITY_AMT"] = item.MaxLiabilityAmount;

                if (item.CoverageOriginalStatus.HasValue)
                {
                    dataRow["COVER_ORIGINAL_STATUS_CD"] = item.CoverageOriginalStatus.Value;
                }
                if (item.Text != null)
                {
                    dataRow["CONDITION_TEXT"] = item.Text.TextBody;
                }
                else
                {
                    dataRow["CONDITION_TEXT"] = null;
                }
                if (item.IsPrimary)
                {
                    dataRow["ENDORSEMENT_LIMIT_AMT"] = item.EndorsementLimitAmount;
                    dataRow["ENDORSEMENT_SUBLIMIT_AMT"] = item.EndorsementSublimitAmount;
                }
                else
                {
                    dataRow["ENDORSEMENT_LIMIT_AMT"] = 0;
                    dataRow["ENDORSEMENT_SUBLIMIT_AMT"] = item.EndorsementSublimitAmount;
                }
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
                    dataRowDeductible["RATE_TYPE_CD"] = item.Deductible.RateType;
                    dataRowDeductible["RATE"] = (object)item.Deductible.Rate ?? DBNull.Value;
                    dataRowDeductible["DEDUCT_PREMIUM_AMT"] = item.Deductible.DeductPremiumAmount;
                    dataRowDeductible["DEDUCT_VALUE"] = item.Deductible.DeductValue;
                    if (item.Deductible.DeductibleUnit != null && item.Deductible.DeductibleUnit.Id != 0)
                    {
                        dataRowDeductible["DEDUCT_UNIT_CD"] = item.Deductible.DeductibleUnit.Id;
                    }
                    else
                    {
                        dataRowDeductible["DEDUCT_UNIT_CD"] = 0;
                    }
                    if (item.Deductible.DeductibleSubject != null && item.Deductible.DeductibleSubject.Id != 0)
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
            parameters[41] = new NameValue("@INSERT_TEMP_RISK_COVERAGE", dtCoverages);
            parameters[42] = new NameValue("@INSERT_TEMP_RISK_COVER_DEDUCT", dtDeductibles);

            DataTable dtBeneficiaries = new DataTable("PARAM_TEMP_RISK_BENEFICIARY");
            dtBeneficiaries.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICIARY_ID", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICIARY_TYPE_CD", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICT_PCT", typeof(decimal));
            dtBeneficiaries.Columns.Add("NAME_NUM", typeof(int));

            foreach (CompanyBeneficiary item in companyLiability.Risk.Beneficiaries)
            {
                DataRow dataRow = dtBeneficiaries.NewRow();
                dataRow["CUSTOMER_TYPE_CD"] = item.CustomerType;
                dataRow["BENEFICIARY_ID"] = item.IndividualId;
                dataRow["BENEFICIARY_TYPE_CD"] = item.BeneficiaryType.Id;
                dataRow["BENEFICT_PCT"] = item.Participation;

                if (item.CustomerType == CustomerType.Individual && item.CompanyName != null && item.CompanyName.NameNum == 0)
                {
                    if (item.IndividualId == companyLiability.Risk.MainInsured.IndividualId)
                    {
                        item.CompanyName = companyLiability.Risk.MainInsured.CompanyName;
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
                            Address = new Address
                            {
                                Id = item.CompanyName.Address.Id,
                                Description = item.CompanyName.Address.Description,
                                City = item.CompanyName.Address.City
                            },
                            Phone = new Phone
                            {
                                Id = item.CompanyName.Phone.Id,
                                Description = item.CompanyName.Phone.Description
                            },
                            Email = new Email
                            {
                                Id = item.CompanyName.Email != null ? item.CompanyName.Email.Id : 0,
                                Description = item.CompanyName.Email != null ? item.CompanyName.Email.Description : ""
                            }
                        };
                        DelegateService.uniquePersonServiceV1.CreateCompaniesName(companyName, item.IndividualId);
                    }
                }
                if (item.CompanyName != null && item.CompanyName.NameNum > 0)
                {
                    dataRow["NAME_NUM"] = item.CompanyName.NameNum;
                }

                dtBeneficiaries.Rows.Add(dataRow);
            }

            parameters[43] = new NameValue("@INSERT_TEMP_RISK_BENEFICIARY", dtBeneficiaries);

            DataTable dtClauses = new DataTable("PARAM_TEMP_CLAUSE");
            dtClauses.Columns.Add("CLAUSE_ID", typeof(int));
            dtClauses.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtClauses.Columns.Add("CLAUSE_STATUS_CD", typeof(int));
            dtClauses.Columns.Add("CLAUSE_ORIG_STATUS_CD", typeof(int));

            if (companyLiability.Risk.Clauses != null)
            {
                foreach (CompanyClause item in companyLiability.Risk.Clauses)
                {
                    DataRow dataRow = dtClauses.NewRow();
                    dataRow["CLAUSE_ID"] = item.Id;
                    dataRow["CLAUSE_STATUS_CD"] = (int)Sistran.Core.Application.CommonService.Enums.ClauseStatuses.Original;
                    dtClauses.Rows.Add(dataRow);
                }
            }

            parameters[44] = new NameValue("@INSERT_TEMP_CLAUSE", dtClauses);

            if (companyLiability.Risk.DynamicProperties != null && companyLiability.Risk.DynamicProperties.Count > 0)
            {
                DynamicPropertiesCollection dynamicCollectionRisk = new DynamicPropertiesCollection();
                for (int i = 0; i < companyLiability.Risk.DynamicProperties.Count(); i++)
                {
                    DynamicProperty dinamycProperty = new DynamicProperty();
                    dinamycProperty.Id = companyLiability.Risk.DynamicProperties[i].Id;
                    dinamycProperty.Value = companyLiability.Risk.DynamicProperties[i].Value;
                    dynamicCollectionRisk[i] = dinamycProperty;
                }
                byte[] serializedValuesRisk = dynamicPropertiesSerializer.Serialize(dynamicCollectionRisk);
                parameters[45] = new NameValue("@DYNAMIC_PROPERTIES", serializedValuesRisk);
            }
            else
            {
                parameters[45] = new NameValue("@DYNAMIC_PROPERTIES", null);
            }

            parameters[46] = new NameValue("@INSPECTION_ID", DBNull.Value, DbType.Int32);

            DataTable dtDynamicProperties = new DataTable("PARAM_TEMP_DYNAMIC_PROPERTIES");
            dtDynamicProperties.Columns.Add("DYNAMIC_ID", typeof(int));
            dtDynamicProperties.Columns.Add("CONCEPT_VALUE", typeof(string));
            if (companyLiability.Risk.DynamicProperties != null)
            {
                foreach (var item in companyLiability.Risk.DynamicProperties)
                {
                    DataRow dataRow = dtDynamicProperties.NewRow();
                    dataRow["DYNAMIC_ID"] = item.Id;
                    dataRow["CONCEPT_VALUE"] = item.Value ?? "NO ASIGNADO";
                    dtDynamicProperties.Rows.Add(dataRow);
                }
            }
            parameters[47] = new NameValue("@INSERT_TEMP_DYNAMIC_PROPERTIES", dtDynamicProperties);

            DataTable dtDynamicPropertiesCoverage = new DataTable("PARAM_TEMP_DYNAMIC_PROPERTIES");
            dtDynamicPropertiesCoverage.Columns.Add("DYNAMIC_ID", typeof(int));
            dtDynamicPropertiesCoverage.Columns.Add("CONCEPT_VALUE", typeof(string));

            parameters[48] = new NameValue("@INSERT_TEMP_DYNAMIC_PROPERTIES_COVERAGE", dtDynamicPropertiesCoverage);

            parameters[49] = new NameValue("ADDITIONAL_STREET", string.Format("-1|{0}||-1||||-1|-1||-1", companyLiability.FullAddress));
            parameters[50] = new NameValue("BUILD_YEAR", companyLiability.ConstructionYear);
            parameters[51] = new NameValue("LEVEL_ZONE", DBNull.Value, DbType.Int16);
            parameters[52] = new NameValue("IS_RESIDENTIAL", false);
            parameters[53] = new NameValue("IS_OUT_COMMUNITY", false);

            parameters[54] = new NameValue("RISK_TYPE_EQ_CD", DBNull.Value, DbType.Int32);

            parameters[55] = new NameValue("STRUCTURE_CD", DBNull.Value, DbType.Int32);

            parameters[56] = new NameValue("IRREGULAR_CD", DBNull.Value, DbType.Int32);

            parameters[57] = new NameValue("IRREGULAR_HEIGHT_CD", DBNull.Value, DbType.Int32);

            parameters[58] = new NameValue("PREVIOUS_DAMAGE_CD", DBNull.Value, DbType.Int32);

            parameters[59] = new NameValue("REPAIRED_CD", DBNull.Value, DbType.Int32);

            parameters[60] = new NameValue("REINFORCED_STRUCTURE_TYPE_CD", DBNull.Value, DbType.Int32);

            parameters[61] = new NameValue("@RISK_DANGEROUSNESS_CD", 1);
            parameters[62] = new NameValue("@RISK_NUM", companyLiability.Risk.Number);
            if (companyLiability.RiskUse != null)
            {
                parameters[63] = new NameValue("@RISK_USE_CD", companyLiability.RiskUse);
            }
            else
            {
                parameters[63] = new NameValue("@RISK_USE_CD", DBNull.Value, DbType.Int64);
            }
            parameters[64] = new NameValue("@RISK_INSP_TYPE_CD", 1);
            parameters[65] = new NameValue("OPERATION", JsonConvert.SerializeObject(companyLiability));
            if (companyLiability.AssuranceMode != null)
            {
                parameters[66] = new NameValue("INSURANCE_MODE_CD", companyLiability.AssuranceMode.Id);
            }
            else
            {
                parameters[66] = new NameValue("INSURANCE_MODE_CD", DBNull.Value, DbType.Int64);
            }
            parameters[67] = new NameValue("@DECLARATIVE_PERIOD_CD", DBNull.Value);
            parameters[68] = new NameValue("@PREMIUM_ADJUSTMENT_PERIOD_CD", DBNull.Value);
            parameters[69] = new NameValue("@ADDRESS_ID", companyLiability.Risk.MainInsured.CompanyName.Address.Id);
            parameters[70] = new NameValue("@PHONE_ID", companyLiability.Risk.MainInsured.CompanyName.Phone.Id);
            parameters[71] = new NameValue("@INSERT_TEMP_RISK_COVER_CLAUSE", dtCoverageClauses);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("ISS.RECORD_RISK_LOCATION", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                if (!Convert.ToBoolean(result.Rows[0][0]))
                {
                    throw new ValidationException((string)result.Rows[0][1]);
                }
            }
            else
            {
                throw new ValidationException(Errors.ErrorRecordEndorsement);
            }
        }

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        public CompanyLiabilityRisk GetCompanyLiabilityByRiskId(int riskId)
        {
            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(riskId);
            CoverageBusiness coverageBusiness = new CoverageBusiness();


            if (pendingOperation != null)
            {
                CompanyLiabilityRisk companyLiability = COMUT.JsonHelper.DeserializeJson<CompanyLiabilityRisk>(pendingOperation.Operation);
                companyLiability.Risk.Id = pendingOperation.Id;
                companyLiability.Risk.IsPersisted = true;
                return companyLiability;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Realiza el llamado de cada uno de los métodos que crean los datatable del riesgo y ejecuta el procedimiento de almacenado
        /// </summary>
        /// <param name="liabilityRisk"></param>
        /// <returns>CompanyLiabilityRisk</returns>
        public CompanyLiabilityRisk SaveCompanyLiabilityTemporal(CompanyLiabilityRisk liabilityRisk)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();

            DataTable dataTable;
            NameValue[] parameters = new NameValue[3];

            DataTable dtTempRisk = UTILITIES.ModelAssembler.GetDataTableTempRISK(liabilityRisk.Risk);
            parameters[0] = new NameValue(dtTempRisk.TableName, dtTempRisk);

            DataTable dtCoTempRisk = UTILITIES.ModelAssembler.GetDataTableCOTempRisk(liabilityRisk.Risk);
            parameters[1] = new NameValue(dtCoTempRisk.TableName, dtCoTempRisk);

            DataTable dtTempRiskLocation = ModelAssembler.GetDataTableTempRiskLocation(liabilityRisk);
            parameters[2] = new NameValue(dtTempRiskLocation.TableName, dtTempRiskLocation);

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("TMP.CIA_SAVE_TEMPORAL_RISK_LOCATION", parameters);
            }
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (liabilityRisk.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Modification)
                {
                    liabilityRisk.Risk.RiskId = Convert.ToInt32(dataTable.Rows[0][0]);
                }
                return liabilityRisk;
            }
            else
            {
                throw new ValidationException(Errors.ErrorCreateTemporalCompanyLiability);//ErrrRecordTemporal "error al grabar riesgo
            }
        }
        /// <summary>
        /// Realiza el llamado de cada uno de los métodos que crean los datatable del riesgo y ejecuta el procedimiento de almacenado
        /// </summary>
        /// <param name="liabilityRisk"></param>
        /// <returns>CompanyLiabilityRisk</returns>
        public CompanyLiabilityRisk SaveCompanyLiabilityTemporalTables(CompanyLiabilityRisk liabilityRisk)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();


            foreach (CompanyCoverage coverage in liabilityRisk.Risk.Coverages)
            {
                if (!coverage.IsPrimary)
                {
                    coverage.LimitAmount = 0;
                    coverage.EndorsementLimitAmount = 0;
                }
            }

            DataTable dataTable;
            NameValue[] parameters = new NameValue[11];

            UTILITIES.GetDatatables dts = new UTILITIES.GetDatatables();
            UTILITIES.CommonDataTables datatables = dts.GetcommonDataTables(liabilityRisk.Risk);


            DataTable dtTempRisk = datatables.dtTempRisk;
            parameters[0] = new NameValue(dtTempRisk.TableName, dtTempRisk);

            DataTable dtCOTempRisk = datatables.dtCOTempRisk;
            parameters[1] = new NameValue(dtCOTempRisk.TableName, dtCOTempRisk);

            DataTable dtBeneficary = datatables.dtBeneficary;
            parameters[2] = new NameValue(dtBeneficary.TableName, dtBeneficary);

            DataTable dtRiskPayer = datatables.dtRiskPayer;
            parameters[3] = new NameValue(dtRiskPayer.TableName, dtRiskPayer);

            DataTable dtRiskClause = datatables.dtRiskClause;
            parameters[4] = new NameValue(dtRiskClause.TableName, dtRiskClause);

            DataTable dtRiskCoverage = datatables.dtRiskCoverage; //UTILITIES.ModelAssembler.GetDataTableRisCoverage(liabilityRisk.Risk);
            parameters[5] = new NameValue(dtRiskCoverage.TableName, dtRiskCoverage);

            DataTable dtDeduct = datatables.dtDeduct;
            parameters[6] = new NameValue(dtDeduct.TableName, dtDeduct);

            DataTable dtCoverClause = datatables.dtCoverClause;
            parameters[7] = new NameValue(dtCoverClause.TableName, dtCoverClause);

            DataTable dtDynamicRisk = datatables.dtDynamic;
            parameters[8] = new NameValue("INSERT_TEMP_DYNAMIC_PROPERTIES_RISK", dtDynamicRisk);

            DataTable dtDynamicCoverage = datatables.dtDynamicCoverage;
            parameters[9] = new NameValue("INSERT_TEMP_DYNAMIC_PROPERTIES_COVERAGE", dtDynamicCoverage);

            DataTable dtTempRiskLocation = ModelAssembler.GetDataTableTempRiskLocation(liabilityRisk);
            parameters[10] = new NameValue(dtTempRiskLocation.TableName, dtTempRiskLocation);

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("TMP.CIA_SAVE_TEMPORAL_RISK_LOCATION_TEMP", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (liabilityRisk.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Modification)
                {
                    liabilityRisk.Risk.RiskId = Convert.ToInt32(dataTable.Rows[0][0]);
                }
                return liabilityRisk;
            }
            else
            {
                throw new ValidationException(Errors.ErrorCreateTemporalCompanyLiability);//ErrrRecordTemporal "error al grabar riesgo
            }

        }


    }
}