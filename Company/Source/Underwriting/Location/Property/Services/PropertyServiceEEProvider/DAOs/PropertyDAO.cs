using AutoMapper;
using Newtonsoft.Json;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.Location.PropertyServices.EEProvider.Assemblers;
using Sistran.Company.Application.Location.PropertyServices.EEProvider.Entities.View;
using Sistran.Company.Application.Location.PropertyServices.EEProvider.Resources;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CiaPersonModel = Sistran.Company.Application.UniquePersonServices.V1.Models;
using COISSEN = Sistran.Company.Application.Issuance.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using UNMO = Sistran.Core.Application.UnderwritingServices.Models;
using UNMOD = Sistran.Core.Application.UnderwritingServices.Models;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Framework.Rules.Engine;
using Sistran.Core.Framework.Rules.Integration;
using Sistran.Company.Application.Location.PropertyServices.EEProvider.Entities;
using UTILITIES = Company.UnderwritingUtilities;
using System.Threading;
using Sistran.Core.Framework.BAF;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Company.Application.Location.PropertyServices.EEProvider.Business;
using Sistran.Company.Application.Location.PropertyServices.Enum;
using Sistran.Core.Application.Utilities.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.PropertyServices.EEProvider.DAOs

{
    public class PropertyDAO
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CompanyAssuranceMode> GetRiskAssuranceMode()
        {
            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(InsuranceMode));


            return ModelAssembler.CreateAssunaceMode(businessCollection);
        }

        /// <summary>
        /// Obtener Poliza de hogar
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <returns>
        /// propertyPolicy
        /// </returns>
        /// <exception cref="Exception">Error obteniendo los objetos del seguro</exception>
        public List<CompanyPropertyRisk> GetCompanyPropertiesByPolicyId(int policyId)
        {
            List<CompanyPropertyRisk> companyPropertyRisks = new List<CompanyPropertyRisk>();

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
            RiskPropertyView view = new RiskPropertyView();
            ViewBuilder builder = new ViewBuilder("RiskPropertyView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Risks.Count > 0)
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.RiskLocation> riskLocations = view.RiskLocations.Cast<ISSEN.RiskLocation>().ToList();
                List<ISSEN.RiskInsuredObject> riskInsuredObjects = view.RiskInsuredObjects.Cast<ISSEN.RiskInsuredObject>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<ISSEN.RiskClause> riskClauses = view.RiskClauses.Cast<ISSEN.RiskClause>().ToList();
                List<ISSEN.Policy> policies = view.Policies.Cast<ISSEN.Policy>().ToList();
                if (view.InsuredObjects == null)
                {
                    throw new Exception(Errors.ErrorCreateInsured);

                }
                List<QUOEN.InsuredObject> insuredObjects = view.InsuredObjects.Cast<QUOEN.InsuredObject>().ToList();
                if (endorsementRisks != null && endorsementRisks.Any() && risks != null && risks.Any() && riskLocations != null && riskLocations.Any())
                {

                    TP.Parallel.For(0, risks.Count, row =>
                  {
                      var item = risks[row];
                      DataFacadeManager.Instance.GetDataFacade().LoadDynamicProperties(item);
                      CompanyPropertyRisk propertyRisk = new CompanyPropertyRisk();
                      List<CompanyInsuredObject> companyInsuredObjects = new List<CompanyInsuredObject>();
                      propertyRisk = ModelAssembler.CreatePropertyRisk(item,
                          riskLocations.First(x => x.RiskId == item.RiskId),
                          endorsementRisks.First(x => x.RiskId == item.RiskId));
                      if (propertyRisk != null)
                      {
                          propertyRisk.Risk.Policy = ModelAssembler.CreateCompanyPolicy(policies.First(x => x.PolicyId == propertyRisk.Risk.Policy.Id));

                          if (propertyRisk.Risk.Status != RiskStatusType.Excluded)
                          {
                              int insuredNameNum = propertyRisk.Risk.MainInsured.CompanyName.NameNum;
                              ModelAssembler.CreateMapCompanyPersonInsured();
                              var companyInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(propertyRisk.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                              if (companyInsured != null)
                              {
                                  propertyRisk.Risk.MainInsured = companyInsured;
                                  var companyName = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(propertyRisk.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
                                  IssuanceCompanyName IssCompanyName = new IssuanceCompanyName();
                                  IssCompanyName.NameNum = companyName.NameNum;
                                  IssCompanyName.TradeName = companyName.TradeName;
                                  if (companyName.Address != null)
                                  {
                                      IssCompanyName.Address = new IssuanceAddress
                                      {
                                          Id = companyName.Address.Id,
                                          Description = companyName.Address.Description,
                                          City = companyName.Address.City
                                      };
                                  }
                                  if (companyName.Phone != null)
                                  {
                                      IssCompanyName.Phone = new IssuancePhone
                                      {
                                          Id = companyName.Phone.Id,
                                          Description = companyName.Phone.Description
                                      };
                                  }
                                  if (companyName.Email != null)
                                  {
                                      IssCompanyName.Email = new IssuanceEmail
                                      {
                                          Id = companyName.Email != null ? companyName.Email.Id : 0,
                                          Description = companyName.Email != null ? companyName.Email.Description : ""
                                      };
                                  }
                              }
                              propertyRisk.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                              if (riskBeneficiaries != null && riskBeneficiaries.Count > 0)
                              {
                                  propertyRisk.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                                  Object objlock = new object();
                                  var imapper = ModelAssembler.CreateMapCompanyBeneficiary();
                                  TP.Parallel.ForEach(riskBeneficiaries.Where(x => x.RiskId == item.RiskId), riskBeneficiary =>
                                  {
                                      CompanyBeneficiary CiaBeneficiary = new CompanyBeneficiary();
                                      var beneficiaryRisk = DelegateService.underwritingService.GetBeneficiariesByDescription(riskBeneficiary.BeneficiaryId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                                      if (beneficiaryRisk != null)
                                      {
                                          CiaBeneficiary = imapper.Map<UNMOD.Beneficiary, CompanyBeneficiary>(beneficiaryRisk);
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
                                              CiaBeneficiary.CompanyName = new IssuanceCompanyName();
                                              CiaBeneficiary.CompanyName.NameNum = companyName.NameNum;
                                              CiaBeneficiary.CompanyName.TradeName = companyName.TradeName;
                                              if (companyName.Address != null)
                                              {
                                                  CiaBeneficiary.CompanyName.Address = new IssuanceAddress
                                                  {
                                                      Id = companyName.Address.Id,
                                                      Description = companyName.Address.Description,
                                                      City = companyName.Address.City
                                                  };
                                              }
                                              if (companyName.Phone != null)
                                              {
                                                  CiaBeneficiary.CompanyName.Phone = new IssuancePhone
                                                  {
                                                      Id = companyName.Phone.Id,
                                                      Description = companyName.Phone.Description
                                                  };
                                              }
                                              if (companyName.Email != null)
                                              {
                                                  CiaBeneficiary.CompanyName.Email = new IssuanceEmail
                                                  {
                                                      Id = companyName.Email != null ? companyName.Email.Id : 0,
                                                      Description = companyName.Email != null ? companyName.Email.Description : ""
                                                  };
                                              }                                              
                                              propertyRisk.Risk.Beneficiaries.Add(CiaBeneficiary);
                                          }
                                      }
                                  });
                              }
                              //clausulas
                              ConcurrentBag<CompanyClause> clauses = new ConcurrentBag<CompanyClause>();
                              if (riskClauses != null && riskClauses.Count > 0)
                              {
                                  TP.Parallel.ForEach(riskClauses.Where(x => x.RiskId == item.RiskId).ToList(), riskClause =>
                                  {
                                      clauses.Add(new CompanyClause { Id = riskClause.ClauseId });
                                  });
                                  propertyRisk.Risk.Clauses = clauses.ToList();
                              }
                              //No se filtra por ID del endoso ya que en la tabla ENDO_RISK_COVERAGE se guardan los riesgos afectados con el último EndorsementId y no se actualizan los demás riesgos de la póliza
                              //var coveragesRisk = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdByRiskId(endorsementRisks.First(x => x.RiskId == item.RiskId).PolicyId, propertyRisk.Risk.RiskId);
                              //DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, entityEndorsementOperation.EndorsementId, companyTransport.Risk.RiskId);
                              var coveragesRisk = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementRisks.First(x => x.RiskId == item.RiskId).EndorsementId, item.RiskId);
                              ConcurrentBag<CompanyInsuredObject> companyInsuredObject = new ConcurrentBag<CompanyInsuredObject>();
                              if (riskInsuredObjects != null)
                              {
                                  propertyRisk.InsuredObjects = new List<CompanyInsuredObject>();

                                  TP.Parallel.ForEach(riskInsuredObjects.Where(x => x.RiskId == item.RiskId), riskInsuredObject =>
                                  {
                                      if (riskInsuredObject != null)
                                          companyInsuredObject.Add(new CompanyInsuredObject
                                          {
                                              Id = riskInsuredObject.InsuredObjectId,
                                              LimitAmount = riskInsuredObject.InsuredValue,
                                              DepositPremiunPercent = riskInsuredObject.InsuredPercentage,
                                              Rate = riskInsuredObject.InsuredRate,
                                              IsDeclarative = insuredObjects.Where(x => x.InsuredObjectId == riskInsuredObject.InsuredObjectId).FirstOrDefault().IsDeclarative
                                          });
                                  });

                                  propertyRisk.InsuredObjects = companyInsuredObject.ToList();
                              }

                              if (coveragesRisk != null && coveragesRisk.Any())
                              {
                                  propertyRisk.Risk.Coverages = coveragesRisk;
                                  if (riskInsuredObjects != null)
                                  {
                                      TP.Parallel.ForEach(propertyRisk.Risk.Coverages, coverage =>
                                      {
                                          //validar si la cobertura esta asosiada al objeto del seguro seleccionado 
                                          var Object = riskInsuredObjects.Where(x => x.InsuredObjectId == coverage.InsuredObject.Id).FirstOrDefault();
                                          if (Object != null)
                                          {
                                              coverage.InsuredObject.Amount = riskInsuredObjects.First(x => x.RiskId == item.RiskId).InsuredValue.GetValueOrDefault();
                                          }

                                      });
                                      //                     var companyInsuredObjects = propertyRisk.Risk.Coverages.GroupBy(p => p.InsuredObject.Id, (key, g) => new { Id = key, key, CompanyCoverages = g.ToList() }).ToList();
                                      //                     propertyRisk.InsuredObjects = companyInsuredObjects.Select(x => new CompanyInsuredObject { Id = x.Id, Premium = x.CompanyCoverages.Sum(m => m.PremiumAmount), Amount = x.CompanyCoverages.Sum(m => m.LimitAmount) }).ToList();
                                      var defaultInsuredObjects = DelegateService.underwritingService.GetCompanyInsuredObjectsByProductIdGroupCoverageId(propertyRisk.Risk.Policy.Product.Id, propertyRisk.Risk.GroupCoverage.Id, propertyRisk.Risk.Policy.Prefix.Id);
                                      foreach (var insured in propertyRisk.InsuredObjects)
                                      {
                                          CompanyInsuredObject insuredObject = new CompanyInsuredObject();
                                          insuredObject = defaultInsuredObjects.Where(x => x.Id == insured.Id).FirstOrDefault();
                                          if (insuredObject != null)
                                          {
                                              insured.Description = insuredObject.Description;
                                              insured.IsMandatory = insuredObject.IsMandatory;
                                              insured.IsDeclarative = insuredObject.IsDeclarative;

                                          }
                                          else
                                          {
                                              insured.Description = DelegateService.underwritingService.GetInsuredObjectByInsuredObjectId(insured.Id).Description;
                                              insured.IsMandatory = false;
                                          }
                                      }
                                  }
                                  companyPropertyRisks.Add(propertyRisk);
                              }
                              else
                              {
                                  throw new Exception("Error Obteniendo Coberturas");
                              }
                          }


                      }
                      else
                      {
                          throw new Exception("Error Creando Riesgo");
                      }

                  });
                }

            }

            return companyPropertyRisks;
        }

        /// <summary>
        /// Obtener Poliza de hogar
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>propertyPolicy</returns>
        public List<CompanyPropertyRisk> GetCompanyPropertiesByEndorsementId(int endorsementId)
        {
            List<CompanyPropertyRisk> companyPropertyRisks = new List<CompanyPropertyRisk>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            RiskPropertyView view = new RiskPropertyView();
            ViewBuilder builder = new ViewBuilder("RiskPropertyView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();

            if (view.Risks.Count > 0)
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.RiskLocation> riskLocations = view.RiskLocations.Cast<ISSEN.RiskLocation>().ToList();
                List<ISSEN.RiskInsuredObject> riskInsuredObjects = view.RiskInsuredObjects.Cast<ISSEN.RiskInsuredObject>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<ISSEN.RiskClause> riskClauses = view.RiskClauses.Cast<ISSEN.RiskClause>().ToList();

                foreach (ISSEN.Risk item in risks)
                {
                    dataFacade.LoadDynamicProperties(item);
                    CompanyPropertyRisk propertyRisk = new CompanyPropertyRisk();

                    propertyRisk = ModelAssembler.CreatePropertyRisk(item,
                        riskLocations.First(x => x.RiskId == item.RiskId),
                        endorsementRisks.First(x => x.RiskId == item.RiskId));

                    int insuredNameNum = propertyRisk.Risk.MainInsured.CompanyName.NameNum;
                    ModelAssembler.CreateMapCompanyPersonInsured();
                    propertyRisk.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(propertyRisk.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);

                    var companyName = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(propertyRisk.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
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
                        },
                        Email = new IssuanceEmail
                        {
                            Id = companyName.Email != null ? companyName.Email.Id : 0,
                            Description = companyName.Email != null ? companyName.Email.Description : ""
                        }
                    };

                    propertyRisk.Risk.Beneficiaries = new List<CompanyBeneficiary>();
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

                        propertyRisk.Risk.Beneficiaries.Add(beneficiary);
                    }

                    List<CompanyClause> clauses = new List<CompanyClause>();
                    //clausulas
                    foreach (ISSEN.RiskClause riskClause in riskClauses.Where(x => x.RiskId == item.RiskId))
                    {
                        clauses.Add(new CompanyClause { Id = riskClause.ClauseId });
                    }

                    propertyRisk.Risk.Clauses = clauses;

                    propertyRisk.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(propertyRisk.Risk.Policy.Id, endorsementId, item.RiskId);

                    foreach (CompanyCoverage coverage in propertyRisk.Risk.Coverages)
                    {
                        coverage.InsuredObject.Amount = riskInsuredObjects.First(x => x.InsuredObjectId == coverage.InsuredObject.Id && x.RiskId == item.RiskId).InsuredValue.GetValueOrDefault();
                    }

                    companyPropertyRisks.Add(propertyRisk);
                }
            }

            return companyPropertyRisks;
        }

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="propertyRisk">Modelo propertyRisk</param>
        public CompanyPropertyRisk CreatePropertyTemporal(CompanyPropertyRisk propertyRisk, bool isMassive)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            propertyRisk.Risk.InfringementPolicies = ValidateAuthorizationPolicies(propertyRisk);


            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "true";

            PendingOperation pendingOperation = new PendingOperation();
            if (propertyRisk.Risk.Id == 0)
            {
                pendingOperation.ParentId = propertyRisk.Risk.Policy.Id;
                pendingOperation.Operation = JsonConvert.SerializeObject(propertyRisk);

                if (!isMassive && boolUseReplicatedDatabase)
                {
                    pendingOperation = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);
                }
            }
            else
            {
                pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(propertyRisk.Risk.Id);
                pendingOperation.Operation = JsonConvert.SerializeObject(propertyRisk);
                if (!isMassive && boolUseReplicatedDatabase)
                {
                    DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
                }
            }
            propertyRisk.Risk.Id = pendingOperation.Id;

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.Properties.PropertyServices.EEProvider.DAOs.CreateVehicleTemporal");

            return propertyRisk;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PoliciesAut> ValidateAuthorizationPolicies(CompanyPropertyRisk companyPropertyRisk)
        {
            Rules.Facade facade = new Rules.Facade();
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();
            if (companyPropertyRisk != null && companyPropertyRisk.Risk.Policy != null)
            {
                var key = companyPropertyRisk.Risk.Policy.Prefix.Id + "," + (int)companyPropertyRisk.Risk.Policy.Product.CoveredRisk.CoveredRiskType;

                //IList facadeList = new List<Rules.Facade>();
                facade = DelegateService.underwritingService.CreateFacadeGeneral(companyPropertyRisk.Risk.Policy);
                //EntityAssembler.CreateFacadeGeneral(facade, companyPropertyRisk.Risk.Policy);
                //facadeList.Add(facadeGeneral);

                EntityAssembler.CreateFacadeRiskProperty(facade, companyPropertyRisk);
                //facadeList.Add(facadeRisk);

                /*Politica del riesgo*/
                policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_RISK));

                /*Politicas de cobertura*/
                if (companyPropertyRisk.Risk.Coverages != null)
                {
                    foreach (var coverage in companyPropertyRisk.Risk.Coverages)
                    {
                        // facadeList.Clear();

                        EntityAssembler.CreateFacadeCoverage(facade, coverage);
                        //facadeList.Add(facadeGeneral);
                        //facadeList.Add(facadeRisk);
                        //facadeList.Add(facadeCoverage);

                        policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_COVERAGE));
                    }
                }
            }

            return policiesAuts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CompanyPropertyRisk> GetCompanyPropertiesByTemporalId(int temporalId)
        {

            ConcurrentBag<CompanyPropertyRisk> companyProperties = new ConcurrentBag<CompanyPropertyRisk>();
            List<PendingOperation> pendingOperations = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(temporalId);
            if (pendingOperations != null)
            {
                TP.Parallel.ForEach(pendingOperations, pendingOperation =>
                {
                    var companyPropertyRisk = JsonConvert.DeserializeObject<CompanyPropertyRisk>(pendingOperation.Operation);
                    companyPropertyRisk.Risk.Id = pendingOperation.Id;
                    companyProperties.Add(companyPropertyRisk);

                });

            }
            return companyProperties.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CompanyPropertyRisk GetCompanyPropertyByRiskId(int riskId)
        {
            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(riskId);

            if (pendingOperation != null)
            {
                CompanyPropertyRisk companyPropertyRisk = COMUT.JsonHelper.DeserializeJson<CompanyPropertyRisk>(pendingOperation.Operation);

                companyPropertyRisk.Risk.Id = pendingOperation.Id;
                companyPropertyRisk.Risk.IsPersisted = true;

                return companyPropertyRisk;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener Poliza de hogar
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <param name="RistIdList">Id temporal</param>
        /// <returns>propertyPolicy</returns>
        public List<CompanyPropertyRisk> GetPropertiesByPolicyIdEndorsementIdRiskIdList(int policyId, int endorsementId, List<int> RiskIdList)
        {
            List<CompanyPropertyRisk> companyPropertyRisks = new List<CompanyPropertyRisk>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            filter.And();
            filter.Not();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(RiskStatusType.Excluded);
            filter.And();
            filter.ListValue();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name);
            filter.In();
            foreach (int r in RiskIdList)
            {
                filter.Constant(r);
            }
            filter.EndList();

            RiskPropertyView view = new RiskPropertyView();
            ViewBuilder builder = new ViewBuilder("RiskPropertyView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();

            if (view.Risks.Count > 0)
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.RiskLocation> riskLocations = view.RiskLocations.Cast<ISSEN.RiskLocation>().ToList();
                List<COISSEN.CoRiskInsuredObject> riskInsuredObjects = view.RiskInsuredObjects.Cast<COISSEN.CoRiskInsuredObject>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<ISSEN.RiskClause> riskClauses = view.RiskClauses.Cast<ISSEN.RiskClause>().ToList();

                foreach (ISSEN.Risk item in risks)
                {
                    dataFacade.LoadDynamicProperties(item);
                    CompanyPropertyRisk propertyRisk = new CompanyPropertyRisk();

                    propertyRisk = ModelAssembler.CreatePropertyRisk(item,
                        riskLocations.First(x => x.RiskId == item.RiskId),
                        endorsementRisks.First(x => x.RiskId == item.RiskId));

                    int insuredNameNum = propertyRisk.Risk.MainInsured.CompanyName.NameNum;
                    ModelAssembler.CreateMapCompanyPersonInsured();
                    propertyRisk.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(propertyRisk.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);

                    var companyName = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(propertyRisk.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
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
                        },
                        Email = new IssuanceEmail
                        {
                            Id = companyName.Email != null ? companyName.Email.Id : 0,
                            Description = companyName.Email != null ? companyName.Email.Description : ""
                        }
                    };

                    propertyRisk.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                    foreach (ISSEN.RiskBeneficiary riskBeneficiary in riskBeneficiaries.Where(x => x.RiskId == item.RiskId))
                    {
                        CompanyBeneficiary beneficiary = new CompanyBeneficiary();
                        beneficiary = ModelAssembler.CreateBeneficiary(riskBeneficiary);

                        int beneficiaryNameNum = beneficiary.CompanyName.NameNum;

                        List<CompanyName> companyNames = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(beneficiary.IndividualId, beneficiary.CustomerType);
                        companyName = new CompanyName();
                        if (beneficiaryNameNum == 0)
                        {
                            companyName = companyNames.First(x => x.IsMain);
                        }
                        else
                        {
                            companyName = companyNames.First(x => x.NameNum == beneficiaryNameNum);
                        }
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

                        propertyRisk.Risk.Beneficiaries.Add(beneficiary);
                    }

                    List<CompanyClause> clauses = new List<CompanyClause>();
                    //clausulas
                    foreach (ISSEN.RiskClause riskClause in riskClauses.Where(x => x.RiskId == item.RiskId))
                    {
                        clauses.Add(new CompanyClause { Id = riskClause.ClauseId });
                    }

                    propertyRisk.Risk.Clauses = clauses;
                    //coberturas
                    //AddCoveragesToRisk(propertyRisk, policyId, endorsementId, item.RiskId, riskInsuredObjects);
                    companyPropertyRisks.Add(propertyRisk);
                }
            }

            return companyPropertyRisks;
        }

        /// <summary>
        /// Metodo para devolver riesgos desde el esquema report
        /// </summary>
        /// <param name="prefixId">ramo</param>
        /// <param name="branchId"> sucursal</param>
        /// <param name="documentNumber"> numero de poliza</param>
        /// <param name="endorsementType"> endorsement</param>
        /// <returns>modelo de Hogar</returns>
        public List<CompanyPropertyRisk> GetCompanyPropertyByPrefixBranchDocumentNumberEndorsementType(int prefixId, int branchId, decimal documentNumber, EndorsementType endorsementType)
        {
            List<CompanyPropertyRisk> risks = new List<CompanyPropertyRisk>();
            NameValue[] parameters = new NameValue[5];
            parameters[0] = new NameValue("@PREFIX_ID", prefixId);
            parameters[1] = new NameValue("@BRANCH_ID", branchId);
            parameters[2] = new NameValue("@DOCUMENT_NUM", documentNumber);
            parameters[3] = new NameValue("@ENDORSEMENT_TYPE_ID", endorsementType);
            parameters[4] = new NameValue("@ONLY_POLICY", 0);


            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("REPORT.REPORT_GET_OPERATION", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {

                foreach (DataRow arrayItem in result.Rows)
                {
                    risks.Add(COMUT.JsonHelper.DeserializeJson<CompanyPropertyRisk>(arrayItem[0].ToString()));

                }
            }


            return risks;
        }

        //public List<CompanyPropertyRisk> GetCompanyPropertyRiskByEndorsementId(int endorsementId)
        //{
        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.Property(ISSEN.EndorsementOperation.Properties.EndorsementId, typeof(ISSEN.EndorsementOperation).Name).Equal().Constant(endorsementId);
        //    filter.And();
        //    filter.Property(ISSEN.EndorsementOperation.Properties.RiskNumber, typeof(ISSEN.EndorsementOperation).Name).IsNotNull();

        //    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ISSEN.EndorsementOperation), filter.GetPredicate()));
        //    List<CompanyPropertyRisk> companyProperties = ModelAssembler.CreatePropertyRisk(businessCollection);

        //    if (companyProperties.Count > 0)
        //    {
        //        if (companyProperties[0].Coverages != null)
        //        {
        //            return companyProperties;
        //        }
        //        else
        //        {
        //            return GetPropertiesByPolicyIdEndorsementId(0, endorsementId);
        //        }
        //    }
        //    else
        //    {
        //        return GetPropertiesByPolicyIdEndorsementId(0, endorsementId);
        //    }
        //}

        /// <summary>
        /// Gets the properties by policy identifier by risk identifier list.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="RiskIdList">The risk identifier list.</param>
        /// <returns></returns>
        public List<CompanyPropertyRisk> GetPropertiesByPolicyIdByRiskIdList(int policyId, List<int> RiskIdList, int riskId)
        {
            List<CompanyPropertyRisk> companyPropertyRisks = new List<CompanyPropertyRisk>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);
            if (riskId == 0)
            {
                filter.And();
                filter.Not();
                filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
                filter.In();
                filter.ListValue();
                filter.Constant((int)RiskStatusType.Excluded);
                filter.Constant((int)RiskStatusType.Cancelled);
                filter.EndList();
                filter.And();
                filter.Property(ISSEN.EndorsementRisk.Properties.RiskNum, typeof(ISSEN.EndorsementRisk).Name);
                filter.In();
                filter.ListValue();
                foreach (int r in RiskIdList)
                {
                    filter.Constant(r);
                }
                filter.EndList();
            }
            else
            {
                filter.And();
                filter.Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name);
                filter.Equal();
                filter.Constant(riskId);
            }


            RiskPropertyView view = new RiskPropertyView();
            ViewBuilder builder = new ViewBuilder("RiskPropertyView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();

            if (view.Risks.Count > 0)
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.RiskLocation> riskLocations = view.RiskLocations.Cast<ISSEN.RiskLocation>().ToList();
                List<ISSEN.RiskInsuredObject> riskInsuredObjects = view.RiskInsuredObjects.Cast<ISSEN.RiskInsuredObject>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<ISSEN.RiskClause> riskClauses = view.RiskClauses.Cast<ISSEN.RiskClause>().ToList();
                List<COMMEN.RiskUseEarthquake> riskUseEarthquakes = view.RiskUseEarthquakes == null ? new List<COMMEN.RiskUseEarthquake>() : view.RiskUseEarthquakes.Cast<COMMEN.RiskUseEarthquake>().ToList();
                List<QUOEN.ConstructionCategory> constructionCategories = view.ConstructionCategories == null ? new List<QUOEN.ConstructionCategory>() : view.ConstructionCategories.Cast<QUOEN.ConstructionCategory>().ToList();
                List<COMMEN.Country> countries = view.Countries.Cast<COMMEN.Country>().ToList();
                List<COMMEN.State> states = view.States.Cast<COMMEN.State>().ToList();
                List<COMMEN.City> cities = view.Cities.Cast<COMMEN.City>().ToList();

                object itemLock = new object();
                Parallel.ForEach(risks, ParallelHelper.DebugParallelFor(), item =>
                {
                    try
                    {
                        dataFacade.LoadDynamicProperties(item);
                        CompanyPropertyRisk propertyRisk = new CompanyPropertyRisk();

                        propertyRisk = ModelAssembler.CreatePropertyRisk(item,
                            riskLocations.First(x => x.RiskId == item.RiskId),
                            endorsementRisks.First(x => x.RiskId == item.RiskId));

                        propertyRisk.City.State.Country.Description = countries.Where(x => x.CountryCode == propertyRisk.City.State.Country.Id).FirstOrDefault().Description;
                        propertyRisk.City.State.Description = states.Where(x => x.StateCode == propertyRisk.City.State.Id && x.CountryCode == propertyRisk.City.State.Country.Id).FirstOrDefault().Description;
                        propertyRisk.City.Description = cities.Where(x => x.CityCode == propertyRisk.City.Id && x.StateCode == propertyRisk.City.State.Id && x.CountryCode == propertyRisk.City.State.Country.Id).FirstOrDefault().Description;
                        if (riskUseEarthquakes.Count > 0)
                        {
                            propertyRisk.RiskUse.Description = riskUseEarthquakes.Where(b => b.RiskUseCode == propertyRisk.RiskUse.Id).FirstOrDefault().Description;
                        }
                        if (constructionCategories.Count > 0)
                        {
                            propertyRisk.ConstructionType.Description = constructionCategories.Where(b => b.ConstructionCategoryCode == propertyRisk.ConstructionType.Id).FirstOrDefault().SmallDescription;
                        }

                        int insuredNameNum = propertyRisk.Risk.MainInsured.CompanyName.NameNum;
                        ModelAssembler.CreateMapCompanyPersonInsured();
                        propertyRisk.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(propertyRisk.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);


                        var companyName = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(propertyRisk.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
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
                            },
                            Email = new IssuanceEmail
                            {
                                Id = companyName.Email != null ? companyName.Email.Id : 0,
                                Description = companyName.Email != null ? companyName.Email.Description : ""
                            }
                        };
                        propertyRisk.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                        object obj = new object();
                        Parallel.ForEach(riskBeneficiaries.Where(x => x.RiskId == item.RiskId), ParallelHelper.DebugParallelFor(), riskBeneficiary =>
                         {
                             try
                             {
                                 CompanyBeneficiary beneficiary = new CompanyBeneficiary();
                                 beneficiary = ModelAssembler.CreateBeneficiary(riskBeneficiary);
                                 int beneficiaryNameNum = beneficiary.CompanyName.NameNum;

                                 List<CompanyName> companyNames = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(beneficiary.IndividualId, beneficiary.CustomerType);
                                 companyName = new CompanyName();
                                 if (beneficiaryNameNum == 0)
                                 {
                                     companyName = companyNames.First(x => x.IsMain);
                                 }
                                 else
                                 {
                                     companyName = companyNames.First(x => x.NameNum == beneficiaryNameNum);
                                 }
                                 lock (obj)
                                 {
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
                                     UNMO.Beneficiary beneficiaryIdentificacion = DelegateService.underwritingService.GetBeneficiariesByDescription(beneficiary.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                                     beneficiary.Name = beneficiaryIdentificacion.Name;
                                     beneficiary.IdentificationDocument = beneficiaryIdentificacion.IdentificationDocument;
                                     propertyRisk.Risk.Beneficiaries.Add(beneficiary);
                                 }

                             }
                             finally
                             {
                                 DataFacadeManager.Dispose();
                             }

                         });

                        List<CompanyClause> clauses = new List<CompanyClause>();
                        //clausulas
                        foreach (ISSEN.RiskClause riskClause in riskClauses.Where(x => x.RiskId == item.RiskId))
                        {
                            clauses.Add(new CompanyClause { Id = riskClause.ClauseId });
                        }

                        propertyRisk.Risk.Clauses = clauses;
                        propertyRisk.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementRisks.First(x => x.RiskId == item.RiskId).EndorsementId, item.RiskId);
                        TP.Parallel.ForEach(propertyRisk.Risk.Coverages, coverage =>
                        {
                            coverage.InsuredObject.Amount = (decimal)riskInsuredObjects?.First(x => x.InsuredObjectId == coverage.InsuredObject.Id && x.RiskId == item.RiskId).InsuredValue.GetValueOrDefault();
                        });
                        lock (itemLock)
                        {
                            companyPropertyRisks.Add(propertyRisk);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    finally
                    {
                        DataFacadeManager.Dispose();
                    }
                });
            }

            return companyPropertyRisks;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyPropertyRisk> companyPropertyRisks)
        {

            if (companyPolicy == null || companyPropertyRisks == null || companyPropertyRisks.Count < 1)
            {
                throw new Exception(Errors.ErrorParametersEmpty);
            }
            ValidateInfringementPolicies(companyPolicy, companyPropertyRisks);
            if (companyPolicy.InfringementPolicies != null && companyPolicy.InfringementPolicies.Count == 0)
            {
                companyPolicy = DelegateService.underwritingService.CreateCompanyPolicy(companyPolicy);
                if (companyPolicy != null)
                {
                    int maxRiskCount = companyPolicy.Summary.RiskCount;
                    int policyId = companyPolicy.Endorsement.PolicyId;
                    int endorsementId = companyPolicy.Endorsement.Id;
                    int endorsementTypeId = (int)companyPolicy.Endorsement.EndorsementType;
                    EndorsementType endorsementType = (EndorsementType)endorsementTypeId;
                    int operationId = companyPolicy.Id;
                    try
                    {

                        TP.Parallel.ForEach(companyPropertyRisks, companyProperty =>
                        {

                            companyProperty.Risk.Policy = companyPolicy;

                            if (companyProperty.Risk.Number == 0 && (companyProperty.Risk.Status == RiskStatusType.Original || companyProperty.Risk.Status == RiskStatusType.Included))
                            {
                                Interlocked.Increment(ref maxRiskCount);
                                companyProperty.Risk.Number = maxRiskCount;
                            }

                            if (endorsementType == EndorsementType.EffectiveExtension)
                            {
                                companyProperty.Risk.Status = RiskStatusType.Original;
                            }
                        });
                        if (companyPolicy.Product.IsCollective)
                        {
                            ConcurrentBag<string> errors = new ConcurrentBag<string>();
                            Parallel.ForEach(companyPropertyRisks, ParallelHelper.DebugParallelFor(), companyProperty =>
                            {
                                try
                                {
                                    CreateRisk(companyProperty);
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
                            Parallel.ForEach(companyPropertyRisks, ParallelHelper.DebugParallelFor(), companyProperty =>
                            {
                                try
                                {
                                    CreateRisk(companyProperty);
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
                                DelegateService.underwritingService.SaveControlPolicy(policyId, endorsementId, operationId, (int)companyPolicy.PolicyOrigin);
                            }
                            catch (Exception)
                            {
                                EventLog.WriteEntry("Application", Errors.ErrorRegisterIntegration);
                            }
                        }
                        catch (Exception ex)
                        {

                            throw new ValidationException(Errors.ErrorDeleteTemp);
                        }

                    }
                    catch (Exception ex)
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

       

        /// <summary>
        /// Deletes the pending operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void DeletePendingOperation(int id)
        {
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@OPERATION_ID", id);
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                pdb.ExecuteSPNonQuery("ISS.DELETE_PENDING_OPERATIONS", parameters);
            }
        }

        /// <summary>
        /// Validates the infringement policies.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <param name="companyPropertyRisks">The company property risks.</param>
        private void ValidateInfringementPolicies(CompanyPolicy companyPolicy, List<CompanyPropertyRisk> companyPropertyRisks)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();

            infringementPolicies.AddRange(companyPolicy.InfringementPolicies);
            companyPropertyRisks.ForEach(x => infringementPolicies.AddRange(x.Risk.InfringementPolicies));

            companyPolicy.InfringementPolicies = DelegateService.AuthorizationPoliciesServiceCore.ValidateInfringementPolicies(infringementPolicies);
        }

        /// <summary>
        /// Crea póliza hogar
        /// </summary>
        /// <param name="companyProperty"></param>
        public void CreateRisk(CompanyPropertyRisk companyProperty)
        {
            try
            {
                if (companyProperty == null || companyProperty.Risk == null)
                {
                    throw new ArgumentException(Errors.NoExistRisk);
                }
                IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();
                NameValue[] parameters = new NameValue[69];
                parameters[0] = new NameValue("@ENDORSEMENT_ID", companyProperty.Risk.Policy.Endorsement.Id);
                parameters[1] = new NameValue("@POLICY_ID", companyProperty.Risk.Policy.Endorsement.PolicyId);
                parameters[2] = new NameValue("@PAYER_ID", companyProperty.Risk.Policy.Holder.IndividualId);

                if (companyProperty.PML != null)
                {
                    parameters[3] = new NameValue("@EML_PCT", companyProperty.PML.Value);
                }
                else
                {
                    parameters[3] = new NameValue("@EML_PCT", 0);
                }

                parameters[4] = new NameValue("@STREET_TYPE_CD", Convert.ToInt16(companyProperty.NomenclatureAddress.Type.Id));
                parameters[5] = new NameValue("@STREET", companyProperty.FullAddress);

                if (companyProperty.NomenclatureAddress.ApartmentOrOffice != null)
                {
                    parameters[6] = new NameValue("@APARTMENT", companyProperty.NomenclatureAddress.ApartmentOrOffice.Id.ToString());
                }
                else
                {
                    parameters[6] = new NameValue("@APARTMENT", 1.ToString());
                }

                parameters[7] = new NameValue("@ZIP_CODE", string.Empty);
                parameters[8] = new NameValue("@URBANIZATION", string.Empty);
                parameters[9] = new NameValue("@IS_MAIN", false);

                parameters[10] = new NameValue("@CITY_CD", companyProperty.City.Id);
                parameters[11] = new NameValue("@HOUSE_NUMBER", DBNull.Value);
                parameters[12] = new NameValue("@STATE_CD", companyProperty.City.State.Id);
                parameters[13] = new NameValue("@COUNTRY_CD", companyProperty.City.State.Country.Id);

                parameters[14] = new NameValue("@OCCUPATION_TYPE_CD", DBNull.Value, DbType.Int16);
                if (companyProperty.Risk.RiskActivity != null && companyProperty.Risk.RiskActivity.Id > 0)
                {
                    parameters[15] = new NameValue("@COMM_RISK_CLASS_CD", companyProperty.Risk.RiskActivity.Id);
                }
                else
                {
                    parameters[15] = new NameValue("@COMM_RISK_CLASS_CD", DBNull.Value);
                }
                if (companyProperty.RiskSubActivity?.Id > 0)
                {
                    parameters[16] = new NameValue("@RISK_COMMERCIAL_TYPE_CD", companyProperty.RiskSubActivity.Id, DbType.Int16);
                }
                else
                {
                    parameters[16] = new NameValue("@RISK_COMMERCIAL_TYPE_CD", DBNull.Value, DbType.Int16);
                }
                parameters[17] = new NameValue("@RISK_COMM_SUBTYPE_CD", DBNull.Value, DbType.Int16);

                if (companyProperty.Square != null)
                {
                    parameters[18] = new NameValue("@BLOCK", companyProperty.Square);
                }
                else
                {
                    parameters[18] = new NameValue("@BLOCK", DBNull.Value, DbType.String);
                }

                if (companyProperty.RiskType != null && companyProperty.RiskType.Id != 0)
                {
                    parameters[19] = new NameValue("@RISK_TYPE_CD", companyProperty.RiskType.Id);
                }
                else
                {
                    parameters[19] = new NameValue("@RISK_TYPE_CD", (int)Sistran.Core.Application.CommonService.Enums.CoveredRiskType.Location);
                }

                parameters[20] = new NameValue("@RISK_AGE", companyProperty.RiskAge);
                parameters[21] = new NameValue("@IS_RETENTION", companyProperty.Risk.IsRetention);
                parameters[22] = new NameValue("@INSPECTION_RECOMENDATION", false);

                DataTable dtInsuredObjects = new DataTable("PARAM_TEMP_RISK_INSURED_OBJECT");
                dtInsuredObjects.Columns.Add("INSURED_OBJECT_ID", typeof(int));
                dtInsuredObjects.Columns.Add("INSURED_VALUE", typeof(decimal));
                dtInsuredObjects.Columns.Add("INSURED_PCT", typeof(decimal));
                dtInsuredObjects.Columns.Add("INSURED_RATE", typeof(decimal));

                foreach (CompanyInsuredObject item in companyProperty.InsuredObjects)
                {
                    if (dtInsuredObjects.AsEnumerable().All(x => x.Field<int>("INSURED_OBJECT_ID") != item.Id))
                    {
                        DataRow dataRowInsuredObject = dtInsuredObjects.NewRow();
                        dataRowInsuredObject["INSURED_OBJECT_ID"] = item.Id;
                        dataRowInsuredObject["INSURED_VALUE"] = item.Amount;
                        dataRowInsuredObject["INSURED_PCT"] = Convert.ToDecimal(item.DepositPremiunPercent);
                        dataRowInsuredObject["INSURED_RATE"] = Convert.ToDecimal(item.Rate);
                        dtInsuredObjects.Rows.Add(dataRowInsuredObject);
                    }
                }

                parameters[23] = new NameValue("@INSERT_TEMP_RISK_INSURED_OBJECT", dtInsuredObjects);


                parameters[24] = new NameValue("@INSURED_ID", companyProperty.Risk.MainInsured.IndividualId);
                parameters[25] = new NameValue("@COVERED_RISK_TYPE_CD", Convert.ToInt16(companyProperty.Risk.CoveredRiskType.Value));

                if (companyProperty.Risk.Status != null)
                {
                    parameters[26] = new NameValue("@RISK_STATUS_CD", Convert.ToInt16(companyProperty.Risk.Status));

                }
                else
                {
                    parameters[26] = new NameValue("@RISK_STATUS_CD", Convert.ToInt16(RiskStatusType.Original));
                }

                if (companyProperty.Risk.Text == null)
                {
                    parameters[27] = new NameValue("@CONDITION_TEXT", DBNull.Value);
                }
                else
                {
                    parameters[27] = new NameValue("@CONDITION_TEXT", companyProperty.Risk.Text.TextBody);
                }

                if (companyProperty.Risk.RatingZone != null)
                {
                    parameters[28] = new NameValue("@RATING_ZONE_CD", Convert.ToInt16(companyProperty.Risk.RatingZone.Id));
                }
                else
                {
                    parameters[28] = new NameValue("@RATING_ZONE_CD", DBNull.Value, DbType.Int16);
                }

                parameters[29] = new NameValue("@COVER_GROUP_ID", Convert.ToInt16(companyProperty.Risk.GroupCoverage.Id));
                parameters[30] = new NameValue("@IS_FACULTATIVE", companyProperty.Risk.IsFacultative);

                if (companyProperty.Risk.MainInsured.CompanyName.NameNum > 0)
                {
                    parameters[31] = new NameValue("@NAME_NUM", companyProperty.Risk.MainInsured.CompanyName.NameNum);
                }
                else
                {
                    parameters[31] = new NameValue("@NAME_NUM", DBNull.Value);
                }

                parameters[32] = new NameValue("@LIMITS_RC_CD", 0);
                parameters[33] = new NameValue("@LIMIT_RC_SUM", 0);

                parameters[34] = new NameValue("@LONGITUDE_EARTHQUAKE", companyProperty.Longitude);
                parameters[35] = new NameValue("@LATITUDE_EARTHQUAKE", companyProperty.Latitude);
                parameters[36] = new NameValue("@CONSTRUCTION_YEAR_EARTHQUAKE", companyProperty.ConstructionYear);
                parameters[37] = new NameValue("@FLOOR_NUMBER_EARTHQUAKE", companyProperty.FloorNumber);

                if (companyProperty.ConstructionType != null && companyProperty.ConstructionType.Id > 0)
                {
                    parameters[38] = new NameValue("@CONSTRUCTION_CATEGORY_CD", companyProperty.ConstructionType.Id);
                }
                else
                {
                    parameters[38] = new NameValue("@CONSTRUCTION_CATEGORY_CD", 1);
                }

                if (companyProperty.Risk.SecondInsured != null && companyProperty.Risk.SecondInsured.IndividualId > 0)
                {
                    parameters[39] = new NameValue("@SECONDARY_INSURED_ID", companyProperty.Risk.SecondInsured.IndividualId);
                }
                else
                {
                    parameters[39] = new NameValue("@SECONDARY_INSURED_ID", DBNull.Value);
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
                dtDeductibles.Columns.Add("DEDUCT_VALUE", typeof(int));
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

                foreach (CompanyCoverage item in companyProperty.Risk.Coverages)
                {

                    DataRow dataRow = dtCoverages.NewRow();
                    dataRow["RISK_COVER_ID"] = item.Id;
                    dataRow["COVERAGE_ID"] = item.Id;
                    dataRow["IS_DECLARATIVE"] = item.IsDeclarative;
                    dataRow["IS_MIN_PREMIUM_DEPOSIT"] = item.IsMinPremiumDeposit;
                    dataRow["FIRST_RISK_TYPE_CD"] = (int)Sistran.Core.Application.UnderwritingServices.Enums.FirstRiskType.None;
                    dataRow["CALCULATION_TYPE_CD"] = item.CalculationType.Value;
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
                        dataRow["CONDITION_TEXT"] = item.Text.TextBody;

                    else
                        dataRow["CONDITION_TEXT"] = null;

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

                foreach (CompanyBeneficiary item in companyProperty.Risk.Beneficiaries)
                {
                    DataRow dataRow = dtBeneficiaries.NewRow();
                    dataRow["CUSTOMER_TYPE_CD"] = item.CustomerType;
                    dataRow["BENEFICIARY_ID"] = item.IndividualId;
                    dataRow["BENEFICIARY_TYPE_CD"] = item.BeneficiaryType.Id;
                    dataRow["BENEFICT_PCT"] = item.Participation;

                    if (item.CustomerType == CustomerType.Individual && item.CompanyName.NameNum == 0)
                    {
                        if (item.IndividualId == companyProperty.Risk.MainInsured.IndividualId)
                        {
                            item.CompanyName = companyProperty.Risk.MainInsured.CompanyName;
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
                    if (item.CompanyName.NameNum > 0)
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

                if (companyProperty.Risk.Clauses != null)
                {
                    foreach (CompanyClause item in companyProperty.Risk.Clauses)
                    {
                        DataRow dataRow = dtClauses.NewRow();
                        dataRow["CLAUSE_ID"] = item.Id;
                        dataRow["CLAUSE_STATUS_CD"] = (int)Sistran.Core.Application.CommonService.Enums.ClauseStatuses.Original;
                        dtClauses.Rows.Add(dataRow);
                    }
                }

                parameters[44] = new NameValue("@INSERT_TEMP_CLAUSE", dtClauses);

                if (companyProperty.Risk.DynamicProperties != null && companyProperty.Risk.DynamicProperties.Count > 0)
                {
                    DynamicPropertiesCollection dynamicCollectionRisk = new DynamicPropertiesCollection();
                    for (int i = 0; i < companyProperty.Risk.DynamicProperties.Count(); i++)
                    {
                        DynamicProperty dinamycProperty = new DynamicProperty();
                        dinamycProperty.Id = companyProperty.Risk.DynamicProperties[i].Id;
                        dinamycProperty.Value = companyProperty.Risk.DynamicProperties[i].Value;
                        dynamicCollectionRisk[i] = dinamycProperty;
                    }
                    byte[] serializedValuesRisk = dynamicPropertiesSerializer.Serialize(dynamicCollectionRisk);
                    parameters[45] = new NameValue("@DYNAMIC_PROPERTIES", serializedValuesRisk);
                }
                else
                {
                    parameters[45] = new NameValue("@DYNAMIC_PROPERTIES", null);
                }

                parameters[46] = new NameValue("@INSPECTION_ID", DBNull.Value);

                DataTable dtDynamicProperties = new DataTable("INSERT_TEMP_DYNAMIC_PROPERTIES");
                dtDynamicProperties.Columns.Add("DYNAMIC_ID", typeof(int));
                dtDynamicProperties.Columns.Add("CONCEPT_VALUE", typeof(string));
                if (companyProperty.Risk.DynamicProperties != null)
                {
                    foreach (var item in companyProperty.Risk.DynamicProperties)
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

                parameters[49] = new NameValue("ADDITIONAL_STREET", string.Format("-1|{0}||-1||||-1|-1||-1||1", companyProperty.FullAddress));
                parameters[50] = new NameValue("BUILD_YEAR", companyProperty.ConstructionYear);
                parameters[51] = new NameValue("LEVEL_ZONE", DBNull.Value, DbType.Int16);
                parameters[52] = new NameValue("IS_RESIDENTIAL", false);
                parameters[53] = new NameValue("IS_OUT_COMMUNITY", false);

                parameters[54] = new NameValue("RISK_TYPE_EQ_CD", DBNull.Value);

                parameters[55] = new NameValue("STRUCTURE_CD", DBNull.Value);

                parameters[56] = new NameValue("@IRREGULAR_CD", 0);

                parameters[57] = new NameValue("@IRREGULAR_HEIGHT_CD", 0);

                parameters[58] = new NameValue("@PREVIOUS_DAMAGE_CD", 0);

                parameters[59] = new NameValue("@REPAIRED_CD", 0);

                parameters[60] = new NameValue("@REINFORCED_STRUCTURE_TYPE_CD", 0);

                parameters[61] = new NameValue("@RISK_DANGEROUSNESS_CD", 1);
                parameters[62] = new NameValue("@RISK_NUM", companyProperty.Risk.Number);

                if (companyProperty.RiskUse?.Id > 0)
                {
                    parameters[63] = new NameValue("@RISK_USE_CD", companyProperty.RiskUse.Id);
                }
                else
                {
                    parameters[63] = new NameValue("@RISK_USE_CD", DBNull.Value);
                }
                parameters[64] = new NameValue("@RISK_INSP_TYPE_CD", 1);
                parameters[65] = new NameValue("OPERATION", JsonConvert.SerializeObject(companyProperty));
                if (companyProperty.AssuranceMode != null)
                {
                    parameters[66] = new NameValue("INSURANCE_MODE_CD", companyProperty.AssuranceMode.Id);
                }
                else
                {
                    parameters[66] = new NameValue("INSURANCE_MODE_CD", DBNull.Value);
                }
                if (companyProperty.DeclarationPeriodCode > 0)
                {
                    parameters[67] = new NameValue("@DECLARATIVE_PERIOD_CD", companyProperty.DeclarationPeriodCode);
                }
                else
                {
                    parameters[67] = new NameValue("@DECLARATIVE_PERIOD_CD", DBNull.Value);

                }
                if (companyProperty.BillingPeriodDepositPremium > 0)
                {
                    parameters[68] = new NameValue("@PREMIUM_ADJUSTMENT_PERIOD_CD", companyProperty.BillingPeriodDepositPremium);
                }
                else
                {
                    parameters[68] = new NameValue("@PREMIUM_ADJUSTMENT_PERIOD_CD", DBNull.Value);
                }
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
                    throw new ValidationException(Errors.ErrorCreateEndorsement);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Polizas asociadas a individual
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<CompanyPropertyRisk> GetPropertiesByIndividualId(int individualId)
        {
            List<CompanyPropertyRisk> companyPropertyRisks = new List<CompanyPropertyRisk>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);

            RiskPropertyView view = new RiskPropertyView();
            ViewBuilder builder = new ViewBuilder("RiskPropertyView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Risks.Count > 0)
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.RiskLocation> riskLocations = view.RiskLocations.Cast<ISSEN.RiskLocation>().ToList();
                List<ISSEN.RiskInsuredObject> riskInsuredObjects = view.RiskInsuredObjects.Cast<ISSEN.RiskInsuredObject>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<ISSEN.RiskClause> riskClauses = view.RiskClauses.Cast<ISSEN.RiskClause>().ToList();
                List<COMMEN.RiskUseEarthquake> riskUseEarthquakes = view.RiskUseEarthquakes == null ? new List<COMMEN.RiskUseEarthquake>() : view.RiskUseEarthquakes.Cast<COMMEN.RiskUseEarthquake>().ToList();
                List<QUOEN.ConstructionCategory> constructionCategories = view.ConstructionCategories == null ? new List<QUOEN.ConstructionCategory>() : view.ConstructionCategories.Cast<QUOEN.ConstructionCategory>().ToList();
                List<COMMEN.Country> countries = view.Countries.Cast<COMMEN.Country>().ToList();
                List<COMMEN.State> states = view.States.Cast<COMMEN.State>().ToList();
                List<COMMEN.City> cities = view.Cities.Cast<COMMEN.City>().ToList();
                List<ISSEN.Policy> policies = view.Policies.Cast<ISSEN.Policy>().ToList();

                object itemLock = new object();
                Parallel.ForEach(risks, ParallelHelper.DebugParallelFor(), item =>
                {
                    CompanyPropertyRisk propertyRisk = new CompanyPropertyRisk();
                    ISSEN.EndorsementRisk endorsementRisk = endorsementRisks.Where(x => x.RiskId == item.RiskId).First();
                    propertyRisk = ModelAssembler.CreatePropertyRisk(riskLocations.First(x => x.RiskId == item.RiskId),
                        policies.Where(x => x.PolicyId == endorsementRisk.PolicyId).First());
                    propertyRisk.Risk.Policy.Endorsement = new CompanyEndorsement() { Id = endorsementRisk.EndorsementId };
                    propertyRisk.City.State.Country.Description = countries.Where(x => x.CountryCode == propertyRisk.City.State.Country.Id).FirstOrDefault().Description;
                    propertyRisk.City.State.Description = states.Where(x => x.StateCode == propertyRisk.City.State.Id && x.CountryCode == propertyRisk.City.State.Country.Id).FirstOrDefault().Description;
                    propertyRisk.City.Description = cities.Where(x => x.CityCode == propertyRisk.City.Id && x.StateCode == propertyRisk.City.State.Id && x.CountryCode == propertyRisk.City.State.Country.Id).FirstOrDefault().Description;
                    companyPropertyRisks.Add(propertyRisk);
                });
            }
            return companyPropertyRisks;
        }

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        public CompanyPropertyRisk GetCompanyPropertyRiskByRiskId(int riskId)
        {
            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(riskId);

            if (pendingOperation != null)
            {
                CompanyPropertyRisk companyPropertyRisk = COMUT.JsonHelper.DeserializeJson<CompanyPropertyRisk>(pendingOperation.Operation);
                companyPropertyRisk.Risk.Id = pendingOperation.Id;
                if (companyPropertyRisk.InsuredObjects == null && companyPropertyRisk?.Risk?.Coverages != null && companyPropertyRisk.Risk.Coverages.Any())
                {
                    companyPropertyRisk.InsuredObjects = companyPropertyRisk.Risk.Coverages.GroupBy(z => new { z.InsuredObject.Id, z.InsuredObject.Description }).Select(x => new CompanyInsuredObject { Id = x.Key.Id, Description = x.Key.Description, Premium = x.Sum(m => m.PremiumAmount), Amount = x.Sum(m => m.LimitAmount), BillingPeriodDepositPremium = companyPropertyRisk.BillingPeriodDepositPremium, DeclarationPeriod = companyPropertyRisk.DeclarationPeriod.Id }).ToList();
                }

                companyPropertyRisk.InsuredObjects = GetCompanyPropertyRiskByRiskId(companyPropertyRisk.InsuredObjects, companyPropertyRisk.Risk.Coverages);
                return companyPropertyRisk;
            }
            else
            {
                return null;
            }
        }

        public CompanyRiskLocation GetCompanyPropertyRiskByRiskIdModuleType(int riskId, ModuleType moduleType)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.RiskLocation.Properties.RiskId, typeof(ISSEN.RiskLocation).Name);
            filter.Equal();
            filter.Constant(riskId);

            ClaimRiskLocationView riskLocationView = new ClaimRiskLocationView();
            ViewBuilder builder = new ViewBuilder("ClaimRiskLocationView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, riskLocationView);

            if (riskLocationView.RiskLocations.Count > 0)
            {
                ISSEN.RiskLocation entityRiskLocation = riskLocationView.RiskLocations.Cast<ISSEN.RiskLocation>().First();
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = riskLocationView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Policy> entityPolicies = riskLocationView.Policies.Cast<ISSEN.Policy>().ToList();

                CompanyRiskLocation riskLocation = ModelAssembler.CreateClaimCompanyLocationsByRiskLocation(entityRiskLocation);
                ISSEN.Risk entityRisk = riskLocationView.Risks.Cast<ISSEN.Risk>().FirstOrDefault(x => x.RiskId == riskLocation.Risk.RiskId);
                riskLocation.Risk = new Risk
                {
                    RiskId = entityRiskLocation.RiskId,
                    MainInsured = new IssuanceInsured
                    {
                        InsuredId = entityRisk.InsuredId
                    },
                    Number = Convert.ToInt32(entityEndorsementRisks.Where(X => X.RiskId == riskLocation.Risk.RiskId).FirstOrDefault()?.RiskNum),
                    CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode,
                    Policy = new Policy
                    {
                        Id = entityEndorsementRisks.FirstOrDefault(X => X.RiskId == riskLocation.Risk.RiskId).PolicyId,
                        Endorsement = new Endorsement
                        {
                            Id = entityEndorsementRisks.FirstOrDefault(X => X.RiskId == riskLocation.Risk.RiskId).EndorsementId
                        }
                    },
                };

                riskLocation.Risk.Policy.DocumentNumber = entityPolicies.FirstOrDefault(X => X.PolicyId == riskLocation.Risk.Policy.Id).DocumentNumber;

                return riskLocation;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the company property risk by risk identifier.
        /// </summary>
        /// <param name="InsuredObjects">The insured objects.</param>
        /// <param name="companyCoverages">The company coverages.</param>
        /// <returns></returns>
        public List<CompanyInsuredObject> GetCompanyPropertyRiskByRiskId(List<CompanyInsuredObject> InsuredObjects, List<CompanyCoverage> companyCoverages)
        {

            if (InsuredObjects != null && InsuredObjects.Any() && companyCoverages != null && companyCoverages.Any())
            {
                TP.Parallel.ForEach(InsuredObjects, insuredObject =>
                 {
                     var insuredObjectData = insuredObject;
                     insuredObjectData.Premium = companyCoverages.Where(x => x.InsuredObject.Id == insuredObjectData.Id).Sum(y => y.PremiumAmount);
                     insuredObjectData.Amount = companyCoverages.Where(x => x.InsuredObject.Id == insuredObjectData.Id).Sum(y => y.LimitAmount);
                     insuredObjectData.Rate = insuredObjectData.Rate;
                 });
                return InsuredObjects;
            }
            else
            {
                return InsuredObjects;
            }

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        public List<CompanyRiskLocation> GetRiskLocationsByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            switch (moduleType)
            {
                case ModuleType.Claim:
                    List<CompanyRiskLocation> riskLocations = new List<CompanyRiskLocation>();
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
                    filter.Equal();
                    filter.Constant(endorsementId);
                    filter.And();
                    filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
                    filter.Distinct();
                    filter.Constant(RiskStatusType.Excluded);

                    ClaimRiskLocationView riskLocationView = new ClaimRiskLocationView();
                    ViewBuilder builder = new ViewBuilder("ClaimRiskLocationView");
                    builder.Filter = filter.GetPredicate();

                    DataFacadeManager.Instance.GetDataFacade().FillView(builder, riskLocationView);

                    if (riskLocationView.Risks.Count > 0)
                    {
                        List<ISSEN.EndorsementRisk> entityEndorsementRisks = riskLocationView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                        List<ISSEN.Policy> entityPolicies = riskLocationView.Policies.Cast<ISSEN.Policy>().ToList();

                        foreach (ISSEN.RiskLocation entityRiskLocation in riskLocationView.RiskLocations)
                        {
                            CompanyRiskLocation riskLocation = ModelAssembler.CreateClaimCompanyLocationsByRiskLocation(entityRiskLocation);
                            ISSEN.Risk entityRisk = riskLocationView.Risks.Cast<ISSEN.Risk>().FirstOrDefault(x => x.RiskId == riskLocation.Risk.RiskId);
                            COMMEN.City entityCity = riskLocationView.Cities.Cast<COMMEN.City>().FirstOrDefault(x => x.CityCode == riskLocation.City.Id);
                            COMMEN.State entityState = riskLocationView.States.Cast<COMMEN.State>().FirstOrDefault(x => x.StateCode == riskLocation.State.Id);
                            COMMEN.Country entityCountry = riskLocationView.Countries.Cast<COMMEN.Country>().FirstOrDefault(x => x.CountryCode == riskLocation.Country.Id);

                            ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                            filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                            filterSumAssured.Equal();
                            filterSumAssured.Constant(endorsementId);
                            SumAssuredView assuredView = new SumAssuredView();
                            ViewBuilder builderAssured = new ViewBuilder("SumAssuredView");
                            builderAssured.Filter = filterSumAssured.GetPredicate();
                            DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                            decimal insuredAmount = 0;

                            foreach (var item in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                            {
                                insuredAmount += item.LimitAmount;
                            }

                            riskLocation.Risk = new Risk
                            {
                                RiskId = entityRiskLocation.RiskId,
                                MainInsured = new IssuanceInsured
                                {
                                    InsuredId = entityRisk.InsuredId
                                },
                                Number = Convert.ToInt32(entityEndorsementRisks.Where(X => X.RiskId == riskLocation.Risk.RiskId).FirstOrDefault()?.RiskNum),
                                CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode,
                                Policy = new Policy
                                {
                                    Id = entityEndorsementRisks.FirstOrDefault(X => X.RiskId == riskLocation.Risk.RiskId).PolicyId,
                                    Endorsement = new Endorsement
                                    {
                                        Id = entityEndorsementRisks.FirstOrDefault(X => X.RiskId == riskLocation.Risk.RiskId).EndorsementId
                                    }
                                },
                                AmountInsured = insuredAmount
                            };

                            riskLocation.City.Description = entityCity?.Description;
                            riskLocation.State.Description = entityState?.Description;
                            riskLocation.Country.Description = entityCountry?.Description;

                            riskLocation.Risk.Policy.DocumentNumber = entityPolicies.FirstOrDefault(X => X.PolicyId == riskLocation.Risk.Policy.Id).DocumentNumber;
                            
                            riskLocations.Add(riskLocation);
                        }
                    }

                    return riskLocations;

                default:
                    return new List<CompanyRiskLocation>();
            }
        }

        /// <summary>
        /// Realiza el llamado de cada uno de los métodos que crean los datatable del riesgo y ejecuta el procedimiento de almacenado
        /// </summary>
        /// <param name="liabilityRisk"></param>
        /// <returns>CompanyLiabilityRisk</returns>
        public CompanyPropertyRisk SaveCompanyPropertyTemporal(CompanyPropertyRisk propertyRisk)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();

            DataTable dataTable;
            NameValue[] parameters = new NameValue[3];

            DataTable dtTempRisk = UTILITIES.ModelAssembler.GetDataTableTempRISK(propertyRisk.Risk);
            parameters[0] = new NameValue(dtTempRisk.TableName, dtTempRisk);

            DataTable dtCoTempRisk = UTILITIES.ModelAssembler.GetDataTableCOTempRisk(propertyRisk.Risk);
            parameters[1] = new NameValue(dtCoTempRisk.TableName, dtCoTempRisk);

            DataTable dtTempRiskLocation = ModelAssembler.GetDataTableTempRiskProperty(propertyRisk);
            parameters[2] = new NameValue(dtTempRiskLocation.TableName, dtTempRiskLocation);

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("TMP.CIA_SAVE_TEMPORAL_RISK_LOCATION", parameters);
            }
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (propertyRisk.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Modification)
                {
                    propertyRisk.Risk.RiskId = Convert.ToInt32(dataTable.Rows[0][0]);
                }
                return propertyRisk;
            }
            else
            {
                throw new ValidationException(Errors.ErrorCreateTemporalCompanyProperty);//ErrrRecordTemporal "error al grabar riesgo
            }
        }     
		
		public List<CompanyRiskLocation> GetCompanyRisksLocationByInsuredId(int insuredId)
        {
            List<CompanyRiskLocation> riskLocations = new List<CompanyRiskLocation>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(insuredId);

            ClaimRiskLocationView riskLocationView = new ClaimRiskLocationView();
            ViewBuilder builder = new ViewBuilder("ClaimRiskLocationView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, riskLocationView);

            if (riskLocationView.RiskLocations.Count > 0)
            {
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = riskLocationView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Policy> entityPolicies = riskLocationView.Policies.Cast<ISSEN.Policy>().ToList();

                foreach (ISSEN.RiskLocation entityRiskLocation in riskLocationView.RiskLocations)
                {
                    CompanyRiskLocation riskLocation = ModelAssembler.CreateClaimCompanyLocationsByRiskLocation(entityRiskLocation);
                    ISSEN.Risk entityRisk = riskLocationView.Risks.Cast<ISSEN.Risk>().FirstOrDefault(x => x.RiskId == riskLocation.Risk.RiskId);
                    COMMEN.City entityCity = riskLocationView.Cities.Cast<COMMEN.City>().FirstOrDefault(x => x.CityCode == riskLocation.City.Id);
                    COMMEN.State entityState = riskLocationView.States.Cast<COMMEN.State>().FirstOrDefault(x => x.StateCode == riskLocation.State.Id);
                    COMMEN.Country entityCountry = riskLocationView.Countries.Cast<COMMEN.Country>().FirstOrDefault(x => x.CountryCode == riskLocation.Country.Id);
                    riskLocation.Risk = new Risk
                    {
                        RiskId = entityRiskLocation.RiskId,
                        MainInsured = new IssuanceInsured
                        {
                            InsuredId = entityRisk.InsuredId
                        },
                        Number = Convert.ToInt32(entityEndorsementRisks.Where(X => X.RiskId == riskLocation.Risk.RiskId).FirstOrDefault()?.RiskNum),
                        CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode,
                        Policy = new Policy
                        {
                            Id = entityEndorsementRisks.FirstOrDefault(X => X.RiskId == riskLocation.Risk.RiskId).PolicyId,
                            Endorsement = new Endorsement
                            {
                                Id = entityEndorsementRisks.FirstOrDefault(X => X.RiskId == riskLocation.Risk.RiskId).EndorsementId
                            }
                        },
                    };

                    riskLocation.City.Description = entityCity?.Description;
                    riskLocation.State.Description = entityState?.Description;
                    riskLocation.Country.Description = entityCountry?.Description;

                    riskLocation.Risk.Policy.DocumentNumber = entityPolicies.FirstOrDefault(X => X.PolicyId == riskLocation.Risk.Policy.Id).DocumentNumber;

                    riskLocations.Add(riskLocation);
                }
            }

            return riskLocations;
        }   
		
		public List<CompanyRiskLocation> GetRisksLocationByAddress(string address)
        {
            try
            {
                List<CompanyRiskLocation> riskLocations = new List<CompanyRiskLocation>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.RiskLocation.Properties.Street, typeof(ISSEN.RiskLocation).Name);
                filter.Like();
                filter.Constant(address + "%");

                ClaimRiskLocationView riskLocationView = new ClaimRiskLocationView();
                ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskLocationView");
                viewBuilder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, riskLocationView);

                if (riskLocationView.RiskLocations.Count > 0)
                {
                    List<ISSEN.EndorsementRisk> entityEndorsementRisks = riskLocationView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                    List<ISSEN.Policy> entityPolicies = riskLocationView.Policies.Cast<ISSEN.Policy>().ToList();

                    foreach (ISSEN.RiskLocation entityRiskLocation in riskLocationView.RiskLocations)
                    {
                        CompanyRiskLocation riskLocation = ModelAssembler.CreateClaimCompanyLocationsByRiskLocation(entityRiskLocation);
                        ISSEN.Risk entityRisk = riskLocationView.Risks.Cast<ISSEN.Risk>().FirstOrDefault(x => x.RiskId == riskLocation.Risk.RiskId);
                        COMMEN.City entityCity = riskLocationView.Cities.Cast<COMMEN.City>().FirstOrDefault(x => x.CityCode == riskLocation.City.Id);
                        COMMEN.State entityState = riskLocationView.States.Cast<COMMEN.State>().FirstOrDefault(x => x.StateCode == riskLocation.State.Id);
                        COMMEN.Country entityCountry = riskLocationView.Countries.Cast<COMMEN.Country>().FirstOrDefault(x => x.CountryCode == riskLocation.Country.Id);
                        riskLocation.Risk = new Risk
                        {
                            RiskId = entityRiskLocation.RiskId,
                            MainInsured = new IssuanceInsured
                            {
                                InsuredId = entityRisk.InsuredId
                            },
                            Number = Convert.ToInt32(entityEndorsementRisks.Where(X => X.RiskId == riskLocation.Risk.RiskId).FirstOrDefault()?.RiskNum),
                            CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode,
                            Policy = new Policy
                            {
                                Id = entityEndorsementRisks.FirstOrDefault(X => X.RiskId == riskLocation.Risk.RiskId).PolicyId,
                                Endorsement = new Endorsement
                                {
                                    Id = entityEndorsementRisks.FirstOrDefault(X => X.RiskId == riskLocation.Risk.RiskId).EndorsementId
                                }
                            },
                        };

                        riskLocation.City.Description = entityCity?.Description;
                        riskLocation.State.Description = entityState?.Description;
                        riskLocation.Country.Description = entityCountry?.Description;

                        riskLocation.Risk.Policy.DocumentNumber = entityPolicies.FirstOrDefault(X => X.PolicyId == riskLocation.Risk.Policy.Id).DocumentNumber;

                        riskLocations.Add(riskLocation);
                    }
                }

                return riskLocations;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en GetRisksByLocation", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CompanyPropertyRisk SaveCompanyPropertyTemporalTables(CompanyPropertyRisk propertyRisk)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();
            UTILITIES.GetDatatables dts = new UTILITIES.GetDatatables();
            UTILITIES.CommonDataTables datatables = dts.GetcommonDataTables(propertyRisk.Risk);

            DataTable dataTable;
            NameValue[] parameters = new NameValue[11];


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

            DataTable dtTempRiskLocation = ModelAssembler.GetDataTableTempRiskLocation(propertyRisk);
            parameters[10] = new NameValue(dtTempRiskLocation.TableName, dtTempRiskLocation);

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("TMP.CIA_SAVE_TEMPORAL_RISK_PROPERTY_TEMP", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (propertyRisk.Risk.Policy.Endorsement.EndorsementType == EndorsementType.Emission)
                {
                    propertyRisk.Risk.RiskId = Convert.ToInt32(dataTable.Rows[0][0]);
                }
                return propertyRisk;
            }
            else
            {
                throw new ValidationException(Errors.ErrorCreateTemporalCompanyProperty);//ErrrRecordTemporal "error al grabar riesgo
            }

        }
        #region Persistencia Emision (Ajuste/Declaracion)
        private DatatableToList DatatableToList = new DatatableToList();
        private bool tryAgain = true;
        public CompanyEndorsementPeriod SaveCompanyEndorsementPeriod(CompanyEndorsementPeriod companyEndorsementPeriod)
        {
            try
            {
                NameValue[] parameters = new NameValue[8];
                CompanyEndorsementPeriod resultEndorsementPeriod = new CompanyEndorsementPeriod();
                decimal monthsVigency = GetMonthsByVigency(companyEndorsementPeriod.CurrentFrom, companyEndorsementPeriod.CurrentTo);
                companyEndorsementPeriod.DeclarationPeriod = GetMothsByDeclarationPeriod(companyEndorsementPeriod.DeclarationPeriod);
                companyEndorsementPeriod.AdjustPeriod = GetMothsByAdjustmentPeriod(companyEndorsementPeriod.AdjustPeriod);
                companyEndorsementPeriod.TotalAdjustment = (int)Math.Floor(monthsVigency / companyEndorsementPeriod.AdjustPeriod);
                companyEndorsementPeriod.TotalDeclarations = (int)Math.Ceiling(monthsVigency / companyEndorsementPeriod.DeclarationPeriod);
                if (monthsVigency < 12 && companyEndorsementPeriod.TotalAdjustment == 0)
                {
                    companyEndorsementPeriod.TotalAdjustment = 1;
                }
                //int rowcount = 0;
                parameters[0] = new NameValue("@CURRENT_FROM", companyEndorsementPeriod.CurrentFrom);
                parameters[1] = new NameValue("@CURRENT_TO", companyEndorsementPeriod.CurrentTo);
                parameters[2] = new NameValue("@ADJUST_PERIOD", companyEndorsementPeriod.AdjustPeriod);
                parameters[3] = new NameValue("@DECLARATION_PERIOD", companyEndorsementPeriod.DeclarationPeriod);
                parameters[4] = new NameValue("@POLICY_ID", companyEndorsementPeriod.PolicyId);
                parameters[5] = new NameValue("@TOTAL_DECLARATIONS", companyEndorsementPeriod.TotalDeclarations);
                parameters[6] = new NameValue("@TOTAL_ADJUSTMENT", companyEndorsementPeriod.TotalAdjustment);
                parameters[7] = new NameValue("@VERSION", companyEndorsementPeriod.Version);
                DataSet result;
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataSet("ISS.SAVE_ENDORSEMENT_COUNT_PERIOD", parameters);
                }
                if (result != null)
                {
                    return resultEndorsementPeriod;
                    //resultEndorsementPeriod = DatatableToList.ConvertTo<CompanyEndorsementPeriod>(result.Tables[0]).FirstOrDefault();
                }
                return new CompanyEndorsementPeriod();
            }
            catch (Exception ex)
            {

                EventLog.WriteEntry("SaveCompanyEndorsementPeriod", String.Format("Error Persistiendo Datos de la poliza en ISS.ENDORSEMENT_COUNT_PERIOD DETALLES {0} : {1}", ex.Message, JsonConvert.SerializeObject(companyEndorsementPeriod)));
                if (tryAgain)
                {
                    tryAgain = false;
                    SaveCompanyEndorsementPeriod(companyEndorsementPeriod);

                }
                throw new Exception(String.Format("Error Persistiendo Datos de la poliza en ISS.ENDORSEMENT_COUNT_PERIOD DETALLES {0}", ex.Message));
            }
        }
        public List<CompanyEndorsementDetail> SaveEndorsementDetailS(List<CompanyEndorsementDetail> endorsementDetails)
        {

            List<CompanyEndorsementDetail> ValidEndtrsementDetails = new List<CompanyEndorsementDetail>();
            try
            {
                foreach (CompanyEndorsementDetail item in endorsementDetails)
                {
                    ValidEndtrsementDetails.Add(SaveEndorsementDetail(item));
                }

                return ValidEndtrsementDetails;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public CompanyEndorsementDetail SaveEndorsementDetail(CompanyEndorsementDetail model)
        {

            CompanyEndorsementDetail resultEndorsementPeriod = new CompanyEndorsementDetail();
            NameValue[] parameters = new NameValue[12];
            parameters[0] = new NameValue("@POLICY_ID", model.PolicyId);
            parameters[1] = new NameValue("@ENDORSEMENT_TYPE", model.EndorsementType);
            parameters[2] = new NameValue("@RISK_NUM", model.RiskNum);
            parameters[3] = new NameValue("@INSURED_OBJECT_ID", model.InsuredObjectId);
            parameters[4] = new NameValue("@VERSION", model.Version);
            parameters[5] = new NameValue("@ENDORSEMENT_DATE", model.EndorsementDate);
            if (model.DeclarationValue != null)
            {
                parameters[6] = new NameValue("@DECLARATION_VALUE", model.DeclarationValue);
            }
            else
            {
                parameters[6] = new NameValue("@DECLARATION_VALUE", DBNull.Value, DbType.Decimal);
            }
            parameters[7] = new NameValue("@PREMIUM_AMOUNT", model.PremiumAmount);
            if (model.DeductibleAmmount != null)
            {
                parameters[8] = new NameValue("@DEDUCTIBLE_AMOUNT", model.DeductibleAmmount);
            }
            else
            {
                parameters[8] = new NameValue("@DEDUCTIBLE_AMOUNT", DBNull.Value, DbType.Int32);
            }
            if (model.Taxes != null)
            {
                parameters[9] = new NameValue("@TAXES", model.Taxes);
            }
            else
            {
                parameters[9] = new NameValue("@TAXES", DBNull.Value, DbType.Int32);
            }
            if (model.Surchanges != null)
            {
                parameters[10] = new NameValue("@SURCHANGE", model.Surchanges);
            }
            else
            {
                parameters[10] = new NameValue("@SURCHANGE", DBNull.Value, DbType.Int32);
            }
            if (model.Expenses != null)
            {
                parameters[11] = new NameValue("@EXPENSES", model.Expenses);
            }
            else
            {
                parameters[11] = new NameValue("@EXPENSES", DBNull.Value, DbType.Int32);
            }



            DataSet result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataSet("ISS.SAVE_ENDORSEMENT_COUNT_DETAIL", parameters);
            }
            if (result != null)
            {
                //resultEndorsementPeriod;
                return resultEndorsementPeriod = DatatableToList.ConvertTo<CompanyEndorsementDetail>(result.Tables[0]).FirstOrDefault();
            }

            return new CompanyEndorsementDetail();
        }

        public CompanyEndorsementPeriod GetEndorsementPeriodByPolicyId(decimal policyId)
        {
            CompanyEndorsementPeriod endorsementPeriod = new CompanyEndorsementPeriod();
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@POLICY_ID", policyId);
            DataSet result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataSet("ISS.GET_ENDORSEMENT_COUNT_PERIOD", parameters);
            }
            if (result != null)
            {

                endorsementPeriod = DatatableToList.ConvertTo<CompanyEndorsementPeriod>(result.Tables[0]).FirstOrDefault();
            }
            return endorsementPeriod;
        }

        public List<CompanyEndorsementDetail> GetEndorsementDetailsListByPolicyId(decimal policyId, decimal version)
        {
            try
            {
                List<CompanyEndorsementDetail> endorsementPeriod = new List<CompanyEndorsementDetail>();
                NameValue[] parameters = new NameValue[2];
                parameters[0] = new NameValue("@POLICY_ID", policyId);
                parameters[1] = new NameValue("@VERSION", version);
                DataSet result;
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataSet("ISS.GET_ENDORSEMENT_COUNT_DETAIL", parameters);
                }
                if (result != null)
                {
                    endorsementPeriod = DatatableToList.ConvertTo<CompanyEndorsementDetail>(result.Tables[0]);
                }
                return endorsementPeriod;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public bool CanMakeEndorsementByRiskByInsuredObjectId(decimal policyId, decimal riskId, decimal insuredObjectId, EndorsementType endorsementType)
        {

            try
            {

                CompanyEndorsementPeriod period = GetEndorsementPeriodByPolicyId(policyId);
                List<CompanyEndorsementDetail> detailsList = GetEndorsementDetailsListByPolicyId(policyId, period.Version);
                int endorsementCount = detailsList.Where(x => x.PolicyId == policyId && x.RiskNum == riskId && x.InsuredObjectId == insuredObjectId && x.Version == period.Version && x.EndorsementType == (int)endorsementType).Count();
                switch (endorsementType)
                {
                    case EndorsementType.DeclarationEndorsement:
                        return (endorsementCount < period.TotalDeclarations) ? true : false;
                        break;
                    case EndorsementType.AdjustmentEndorsement:
                        return (endorsementCount < period.TotalAdjustment) ? true : false;
                        break;
                    default:
                        return false;
                        break;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private decimal GetMonthsByVigency(DateTime currentFrom, DateTime currentTo)
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

        /// <summary>
        /// Obtiene la cantidad de meses que tiene un periodo de declaración
        /// </summary>
        /// <param name="declarationId">Identificador del periodo de declaración</param>
        /// <returns>Canitdad de meses</returns>
        private int GetMothsByDeclarationPeriod(int declarationId)
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
        /// Obtiene la cantidad de meses que tiene un periodo de ajuste
        /// </summary>
        /// <param name="adjustmentId">Identificador del periodo de ajuste</param>
        /// <returns>Cantidad de meses</returns>
        private int GetMothsByAdjustmentPeriod(int adjustmentId)
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
        #endregion

    }
}