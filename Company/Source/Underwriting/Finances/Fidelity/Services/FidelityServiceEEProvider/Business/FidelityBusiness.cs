using AutoMapper;
using Newtonsoft.Json;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.Finances.FidelityServices.EEProvider.Assemblers;
using Sistran.Company.Application.Finances.FidelityServices.EEProvider.Entities.View;
using Sistran.Company.Application.Finances.FidelityServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.RulesScriptsServices.Enums;
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
using CiaPersonModel = Sistran.Company.Application.UniquePersonServices.V1.Models;
using CiaUnderwritingModel = Sistran.Company.Application.UnderwritingServices.Models;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using RES = Sistran.Company.Application.Finances.FidelityServices.EEProvider.Resources;
using Sistran.Company.Application.Location.FidelityServices.DTOs;
using Sistran.Core.Application.Finances.Models;
using Sistran.Company.Application.Finances.FidelityServices.EEProvider.Resources;
using UTILITES = Company.UnderwritingUtilities;
using Sistran.Core.Application.Utilities.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;
namespace Sistran.Company.Application.Finances.FidelityServices.EEProvider.DAOs

{
    public class FidelityBusiness
    {
        /// <summary>
        /// Obtener Poliza de hogar
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>propertyPolicy</returns>
        public List<CompanyFidelityRisk> GetCompanyFidelitiesByPolicyId(int policyId)
        {
            ConcurrentBag<CompanyFidelityRisk> companyPropertyRisks = new ConcurrentBag<CompanyFidelityRisk>();

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
            RiskFidelityView view = new RiskFidelityView();
            ViewBuilder builder = new ViewBuilder("RiskFidelityView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();

            if (view.Risks.Count > 0)
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.RiskFidelity> riskFidelities = view.RiskFidelities.Cast<ISSEN.RiskFidelity>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<ISSEN.RiskClause> riskClauses = view.RiskClauses.Cast<ISSEN.RiskClause>().ToList();
                if (risks != null && risks.Any())
                {
                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    Parallel.For(0, risks.Count, ParallelHelper.DebugParallelFor(), row =>
                      {
                          var item = risks[row];
                          DataFacadeManager.Instance.GetDataFacade().LoadDynamicProperties(item);
                          CompanyFidelityRisk fidelityRisk = new CompanyFidelityRisk();

                          fidelityRisk= ModelAssembler.CreateFidelityRisk(item, riskFidelities.Where(x => x.RiskId == item.RiskId).First(), endorsementRisks.Where(x => x.RiskId == item.RiskId).First());

                          int insuredNameNum = fidelityRisk.Risk.MainInsured.CompanyName.NameNum;
                          var insured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(fidelityRisk.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                          if (insured != null)
                          {
                              fidelityRisk.Risk.MainInsured = insured;
                              var companyName  = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(fidelityRisk.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
                              fidelityRisk.Risk.MainInsured.CompanyName = new IssuanceCompanyName
                              {
                                  NameNum = companyName.NameNum,
                                  TradeName = companyName.TradeName,
                                  Address = companyName.Address==null ? null: new IssuanceAddress
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
                          }

                          fidelityRisk.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                          if (riskBeneficiaries != null && riskBeneficiaries.Count > 0)
                          {
                              object objlock = new object();
                              TP.Parallel.ForEach(riskBeneficiaries.Where(x => x.RiskId == item.RiskId), riskBeneficiary =>
                              {
                                  CompanyBeneficiary beneficiary = new CompanyBeneficiary();
                                  beneficiary = ModelAssembler.CreateBeneficiary(riskBeneficiary);

                                  int beneficiaryNameNum = beneficiary.CompanyName.NameNum;
                                  List<CompanyName> companyNames = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(beneficiary.IndividualId,(CustomerType) beneficiary.CustomerType);
                                  CompanyName companyName;
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
                                  lock (objlock)
                                  {
                                      fidelityRisk.Risk.Beneficiaries.Add(beneficiary);
                                  }
                              });
                          }

                          ConcurrentBag<CompanyClause> clauses = new ConcurrentBag<CompanyClause>();
                          //clausulas
                          TP.Parallel.ForEach(riskClauses.Where(x => x.RiskId == item.RiskId), riskClause =>
                          {
                              clauses.Add(new CompanyClause { Id = riskClause.ClauseId });
                          });

                          fidelityRisk.Risk.Clauses = clauses.ToList();
                          var coverageProduct = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementRisks.First(x => x.RiskId == item.RiskId).EndorsementId, item.RiskId);
                          if (coverageProduct != null)
                          {
                              fidelityRisk.Risk.Coverages = coverageProduct;
                          }
                          else
                          {
                              errors.Add("No existe Coberturas parametrizadas");
                          }

                          companyPropertyRisks.Add(fidelityRisk);
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
        /// <returns>fidelityPolicy</returns>
        public List<CompanyFidelityRisk> GetCompanyFidelitiesByEndorsementId(int endorsementId)
        {
            List<CompanyFidelityRisk> companyFidelityRisk = new List<Models.CompanyFidelityRisk>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            RiskFidelityView view = new RiskFidelityView();
            ViewBuilder builder = new ViewBuilder("RiskFidelityView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();

            if (view.Risks.Count > 0)
            {
                List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                List<ISSEN.RiskFidelity> riskFidelities = view.RiskFidelities.Cast<ISSEN.RiskFidelity>().ToList();
                List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                List<ISSEN.RiskClause> riskClauses = view.RiskClauses.Cast<ISSEN.RiskClause>().ToList();

                foreach (ISSEN.Risk item in risks)
                {
                    dataFacade.LoadDynamicProperties(item);
                    CompanyFidelityRisk fidelityRisk = new CompanyFidelityRisk();

                     ModelAssembler.CreateFidelityRisk(item,
                        riskFidelities.Where(x => x.RiskId == item.RiskId).First(),
                        endorsementRisks.Where(x => x.RiskId == item.RiskId).First());

                    int insuredNameNum = fidelityRisk.Risk.MainInsured.CompanyName.NameNum;
                    fidelityRisk.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(fidelityRisk.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);


                    var companyName = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(fidelityRisk.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
                    fidelityRisk.Risk.MainInsured.CompanyName = new IssuanceCompanyName
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


                    fidelityRisk.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                    foreach (ISSEN.RiskBeneficiary riskBeneficiary in riskBeneficiaries.Where(x => x.RiskId == item.RiskId))
                    {
                        CompanyBeneficiary beneficiary = new CompanyBeneficiary();
                        beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(riskBeneficiary.BeneficiaryId.ToString(), InsuredSearchType.IndividualId).Cast<CompanyBeneficiary>().First();
                        beneficiary.BeneficiaryType = new CompanyBeneficiaryType { Id = riskBeneficiary.BeneficiaryTypeCode };
                        companyName = new CompanyName();
                        companyName = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(beneficiary.IndividualId, (CustomerType)beneficiary.CustomerType).First();
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
                                Id = companyName.Email.Id,
                                Description = companyName.Email.Description
                            }
                        };

                        fidelityRisk.Risk.Beneficiaries.Add(beneficiary);
                    }

                    List<CompanyClause> clauses = new List<CompanyClause>();
                    //clausulas
                    foreach (ISSEN.RiskClause riskClause in riskClauses.Where(x => x.RiskId == item.RiskId))
                    {
                        clauses.Add(new CompanyClause { Id = riskClause.ClauseId });
                    }
                    fidelityRisk.Risk.Clauses = clauses;

                    fidelityRisk.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(fidelityRisk.Risk.Policy.Id, endorsementId, item.RiskId);

                    companyFidelityRisk.Add(fidelityRisk);
                }
            }

            return companyFidelityRisk;
        }

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="fidelityRisk">Modelo fidelityRisk</param>
        public CompanyFidelityRisk CreateFidelityTemporal(CompanyFidelityRisk fidelityRisk, bool isMassive)
        {
            fidelityRisk.Risk.InfringementPolicies = ValidateAuthorizationPolicies(fidelityRisk);

            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "true";
            PendingOperation pendingOperation = new PendingOperation();
            CompanyPolicy policy = fidelityRisk.Risk.Policy;


            if (fidelityRisk.Risk.Id == 0)
            {
                pendingOperation.CreationDate = DateTime.Now;
                pendingOperation.ParentId = policy.Id;
                pendingOperation.Operation = JsonConvert.SerializeObject(fidelityRisk);



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
                pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(fidelityRisk.Risk.Id);
                if (pendingOperation != null)
                {
                    pendingOperation.Operation = JsonConvert.SerializeObject(fidelityRisk);

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
                int riskId = SaveCompanyFidelityTemporalTables(fidelityRisk);
                if (fidelityRisk.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Modification)
                {
                    fidelityRisk.Risk.RiskId = riskId;
                }
            }
            fidelityRisk.Risk.Id = pendingOperation.Id;
            fidelityRisk.Risk.Policy = policy;
            ////****************************GUARDAR TEMPORAL*********************************//
         
            ////****************************************************************************//
           return fidelityRisk;
        }

        public int SaveCompanyFidelityTemporalTables(CompanyFidelityRisk companyJudgement)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer =
            new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();
            UTILITES.GetDatatables d = new UTILITES.GetDatatables();

            UTILITES.CommonDataTables dts = d.GetcommonDataTables(companyJudgement.Risk);

            DataTable dataTable;
            NameValue[] parameters = new NameValue[1];

            DataTable dtTempRisk = dts.dtTempRisk;

            parameters[0] = new NameValue(dtTempRisk.TableName, dtTempRisk);

            //DataTable dtCOTempRisk = dts.dtCOTempRisk;
            //parameters[1] = new NameValue(dtCOTempRisk.TableName, dtCOTempRisk);

            //DataTable dtBeneficary = dts.dtBeneficary;
            //parameters[2] = new NameValue(dtBeneficary.TableName, dtBeneficary);

            //DataTable dtRiskPayer = dts.dtRiskPayer;
            //parameters[3] = new NameValue(dtRiskPayer.TableName, dtRiskPayer);

            //DataTable dtClause = dts.dtClause;
            //parameters[4] = new NameValue(dtClause.TableName, dtClause);

            //DataTable dtRiskClause = dts.dtRiskClause;
            //parameters[5] = new NameValue(dtRiskClause.TableName, dtRiskClause);

            //DataTable dtDeduct = dts.dtDeduct;
            //parameters[6] = new NameValue(dtDeduct.TableName, dtDeduct);

            //DataTable dtCoverClause = dts.dtCoverClause;
            //parameters[7] = new NameValue(dtCoverClause.TableName, dtCoverClause);

            //DataTable dtDynamic = dts.dtDynamic;
            //parameters[8] = new NameValue("INSERT_TEMP_DYNAMIC_PROPERTIES_RISK", dtDynamic);

            //DataTable dtDynamicCoverage = dts.dtDynamicCoverage;
            //parameters[9] = new NameValue("INSERT_TEMP_DYNAMIC_PROPERTIES_COVERAGE", dtDynamicCoverage);

            //DataTable dtTempRiskJudicialSurety = ModelAssembler.GetDataTableRiskFidelity(companyJudgement);
            //parameters[10] = new NameValue(dtTempRiskJudicialSurety.TableName, dtTempRiskJudicialSurety);
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {

               dataTable = pdb.ExecuteSPDataTable("TMP.SAVE_TEMPORAL_RISK_FIDELITY_TEMP", parameters);
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
        public List<PoliciesAut> ValidateAuthorizationPolicies(CompanyFidelityRisk companyFidelityRisk)
        {
            Rules.Facade facade = new Rules.Facade();
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();
            if (companyFidelityRisk != null && companyFidelityRisk.Risk.Policy != null)
            {
                var key = companyFidelityRisk.Risk.Policy.Prefix.Id + "," + (int)companyFidelityRisk.Risk.Policy.Product.CoveredRisk.CoveredRiskType;

               

               EntityAssembler.CreateFacadeGeneral(facade, companyFidelityRisk.Risk.Policy);
               

              EntityAssembler.CreateFacadeRiskFidelity(facade, companyFidelityRisk);
             

                /*Politica del riesgo*/
                policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_RISK));
               
                /*Politicas de cobertura*/
                if (companyFidelityRisk.Risk.Coverages != null)
                {
                    foreach (var coverage in companyFidelityRisk.Risk.Coverages)
                    {
                        

                      EntityAssembler.CreateFacadeCoverage(facade, coverage);
                        

                        policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_COVERAGE));
                    }
                }
            }

            return policiesAuts;
        }
        public List<CompanyFidelityRisk> GetCompanyFidelitiesByTemporalId(int temporalId)
        {
            List<CompanyFidelityRisk> companyContract = new List<CompanyFidelityRisk>();
            //PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(temporalId);
            List<PendingOperation> pendingOperations = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(temporalId);

            foreach (PendingOperation pendingOperation in pendingOperations)
            {
                CompanyFidelityRisk risk = JsonConvert.DeserializeObject<CompanyFidelityRisk>(pendingOperation.Operation);
                risk.Risk.Id = pendingOperation.Id;
                companyContract.Add(risk);
            }

            return companyContract;
        }

        //public List<CompanyFidelityRisk> GetCompanyFidelitiesRiskByEndorsementId(int endorsementId)
        //{
        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.Property(ISSEN.EndorsementOperation.Properties.EndorsementId, typeof(ISSEN.EndorsementOperation).Name).Equal().Constant(endorsementId);
        //    filter.And();
        //    filter.Property(ISSEN.EndorsementOperation.Properties.RiskNumber, typeof(ISSEN.EndorsementOperation).Name).IsNotNull();

        //    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ISSEN.EndorsementOperation), filter.GetPredicate()));
        //    List<CompanyFidelityRisk> companyFidelities = ModelAssembler.CreateFidelitiesRisk(businessCollection);

        //    if (companyFidelities.Count > 0)
        //    {
        //        if (companyFidelities[0].Coverages != null)
        //        {
        //            return companyFidelities;
        //        }
        //        else
        //        {
        //            return GetFidelitiesByPolicyIdEndorsementId(0, endorsementId);
        //        }
        //    }
        //    else
        //    {
        //        return GetFidelitiesByPolicyIdEndorsementId(0, endorsementId);
        //    }
        //}

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyFidelityRisk> companyFidelities)
        {
            if (companyPolicy == null || companyFidelities == null || companyFidelities.Count < 1)
            {
                throw new ArgumentException("Poliza y Riesgos Vacios");
            }
            ValidateInfringementPolicies(companyPolicy, companyFidelities);
            if (companyPolicy.InfringementPolicies?.Count == 0)
            {
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

                        TP.Parallel.ForEach(companyFidelities, companyFidelity =>
                        {
                            companyFidelity.Risk.Policy = companyPolicy;
                            if (companyFidelity.Risk.Status == RiskStatusType.Original || companyFidelity.Risk.Status == RiskStatusType.Included)
                            {
                                companyFidelity.Risk.Number = ++maxRiskCount;
                            }
                        });

                        ConcurrentBag<string> errors = new ConcurrentBag<string>();
                        Parallel.ForEach(companyFidelities, ParallelHelper.DebugParallelFor(), companyFidelity =>
                        {
                            try
                            {
                                CreateCompanyFidelity(companyFidelity);
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
                            DelegateService.underwritingService.DeleteTemporalByOperationId(companyPolicy.Id, 0,0, 0);
                          
                        }
                        catch (Exception)
                        {

                            throw new ValidationException(RES.Errors.ErrorDeleteTemp);
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

        private void ValidateInfringementPolicies(CompanyPolicy companyPolicy, List<CompanyFidelityRisk> companyFidelities)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();

            infringementPolicies.AddRange(companyPolicy.InfringementPolicies);
            companyFidelities.ForEach(x => infringementPolicies.AddRange(x.Risk.InfringementPolicies));

            companyPolicy.InfringementPolicies = DelegateService.AuthorizationPoliciesServiceCore.ValidateInfringementPolicies(infringementPolicies);
        }

        public void CreateCompanyFidelity(CompanyFidelityRisk companyFidelity)
        {
            /// jhgomez
            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();
            NameValue[] parameters = new NameValue[30];
            parameters[0] = new NameValue("@ENDORSEMENT_ID", companyFidelity.Risk.Policy.Endorsement.Id);
            parameters[1] = new NameValue("@POLICY_ID", companyFidelity.Risk.Policy.Endorsement.PolicyId);
            parameters[2] = new NameValue("@PAYER_ID", companyFidelity.Risk.Policy.Holder.IndividualId);

            parameters[3] = new NameValue("@DESCRIPTION", companyFidelity.Description);
            parameters[4] = new NameValue("@DISCOVERY_DATE", companyFidelity.DiscoveryDate);
            parameters[5] = new NameValue("@OCCUPATION_TYPE_CD", companyFidelity.IdOccupation);
            parameters[6] = new NameValue("@COMM_RISK_CLASS_CD", companyFidelity.Risk.RiskActivity.Id);
            parameters[7] = new NameValue("@RISK_COMMERCIAL_TYPE_CD", DBNull.Value);
            
            DataTable dtInsuredObjects = new DataTable("PARAM_TEMP_RISK_INSURED_OBJECT");
            dtInsuredObjects.Columns.Add("INSURED_OBJECT_ID", typeof(int));
            dtInsuredObjects.Columns.Add("INSURED_VALUE", typeof(decimal));
            dtInsuredObjects.Columns.Add("INSURED_PCT", typeof(decimal));
            dtInsuredObjects.Columns.Add("INSURED_RATE", typeof(decimal));

            foreach (CompanyCoverage item in companyFidelity.Risk.Coverages)
            {
                if (dtInsuredObjects.AsEnumerable().All(x => x.Field<int>("INSURED_OBJECT_ID") != item.InsuredObject.Id))
                {
                    DataRow dataRow = dtInsuredObjects.NewRow();
                    dataRow["INSURED_OBJECT_ID"] = item.InsuredObject.Id;
                    dataRow["INSURED_VALUE"] = item.InsuredObject.Amount;

                    if (companyFidelity.Risk.MainInsured != null && companyFidelity.Risk.Coverages != null && companyFidelity.Risk.Coverages.Any())
                    {
                        CompanyCoverage companyCoverage = companyFidelity.Risk.Coverages.Find(x => x.Id == item.Id);
                        if (companyCoverage != null)
                        {
                            CompanyInsuredObject companyInsuredObject = companyCoverage.InsuredObject;
                            dataRow["INSURED_PCT"] = DBNull.Value;
                            dataRow["INSURED_RATE"] = companyCoverage.InsuredObject.Rate ?? 0;
                        }
                    }
                    dtInsuredObjects.Rows.Add(dataRow);
                }
            }

            parameters[8] = new NameValue("@INSERT_TEMP_RISK_INSURED_OBJECT", dtInsuredObjects);

            parameters[9] = new NameValue("@INSURED_ID", companyFidelity.Risk.MainInsured.IndividualId);
            parameters[10] = new NameValue("@COVERED_RISK_TYPE_CD", (int)companyFidelity.Risk.CoveredRiskType.Value);

            if (companyFidelity.Risk.Status != null)
            {
                parameters[11] = new NameValue("@RISK_STATUS_CD", (int)companyFidelity.Risk.Status);
            }
            else
            {
                parameters[11] = new NameValue("@RISK_STATUS_CD", DBNull.Value);
            }

            if (companyFidelity.Risk.Text == null)
            {
                parameters[12] = new NameValue("@CONDITION_TEXT", DBNull.Value);
            }
            else
            {
                parameters[12] = new NameValue("@CONDITION_TEXT", companyFidelity.Risk.Text.TextBody);
            }

            if (companyFidelity.Risk.RatingZone != null)
            {
                parameters[13] = new NameValue("@RATING_ZONE_CD", companyFidelity.Risk.RatingZone.Id);
            }
            else
            {
                parameters[13] = new NameValue("@RATING_ZONE_CD", DBNull.Value);
            }

            parameters[14] = new NameValue("@COVER_GROUP_ID", companyFidelity.Risk.GroupCoverage.Id);
            parameters[15] = new NameValue("@IS_FACULTATIVE", false);

            if (companyFidelity.Risk.MainInsured.CompanyName.NameNum > 0)
            {
                parameters[16] = new NameValue("@NAME_NUM", companyFidelity.Risk.MainInsured.CompanyName.NameNum);
            }
            else
            {
                parameters[16] = new NameValue("@NAME_NUM", DBNull.Value);
            }

            parameters[17] = new NameValue("@LIMITS_RC_CD", 0);
            parameters[18] = new NameValue("@LIMIT_RC_SUM", 0);

            if (companyFidelity.Risk.SecondInsured != null && companyFidelity.Risk.SecondInsured.IndividualId > 0)
            {
                parameters[19] = new NameValue("@SECONDARY_INSURED_ID", companyFidelity.Risk.SecondInsured.IndividualId);
            }
            else
            {
                parameters[19] = new NameValue("@SECONDARY_INSURED_ID", DBNull.Value);
            }

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

            foreach (CompanyCoverage item in companyFidelity.Risk.Coverages)
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
            parameters[20] = new NameValue("@INSERT_TEMP_RISK_COVERAGE", dtCoverages);
            parameters[21] = new NameValue("@INSERT_TEMP_RISK_COVER_DEDUCT", dtDeductibles);

            DataTable dtBeneficiaries = new DataTable("PARAM_TEMP_RISK_BENEFICIARY");
            dtBeneficiaries.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICIARY_ID", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICIARY_TYPE_CD", typeof(int));
            dtBeneficiaries.Columns.Add("BENEFICT_PCT", typeof(decimal));
            dtBeneficiaries.Columns.Add("NAME_NUM", typeof(int));

            foreach (CompanyBeneficiary item in companyFidelity.Risk.Beneficiaries)
            {
                DataRow dataRow = dtBeneficiaries.NewRow();
                dataRow["CUSTOMER_TYPE_CD"] = item.CustomerType;
                dataRow["BENEFICIARY_ID"] = item.IndividualId;
                dataRow["BENEFICIARY_TYPE_CD"] = item.BeneficiaryType.Id;
                dataRow["BENEFICT_PCT"] = item.Participation;

                if (item.CustomerType == CustomerType.Individual && item.CompanyName.NameNum == 0)
                {
                    if (item.IndividualId == companyFidelity.Risk.MainInsured.IndividualId)
                    {
                        item.CompanyName = companyFidelity.Risk.MainInsured.CompanyName;
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
                                Id = item.CompanyName.Email.Id,
                                Description = item.CompanyName.Email.Description
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

            parameters[22] = new NameValue("@INSERT_TEMP_RISK_BENEFICIARY", dtBeneficiaries);

            DataTable dtClauses = new DataTable("PARAM_TEMP_CLAUSE");
            dtClauses.Columns.Add("CLAUSE_ID", typeof(int));
            dtClauses.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtClauses.Columns.Add("CLAUSE_STATUS_CD", typeof(int));
            dtClauses.Columns.Add("CLAUSE_ORIG_STATUS_CD", typeof(int));

            if (companyFidelity.Risk.Clauses != null)
            {
                foreach (CompanyClause item in companyFidelity.Risk.Clauses)
                {
                    DataRow dataRow = dtClauses.NewRow();
                    dataRow["CLAUSE_ID"] = item.Id;
                    dataRow["CLAUSE_STATUS_CD"] = (int)Sistran.Core.Application.CommonService.Enums.ClauseStatuses.Original;
                    dtClauses.Rows.Add(dataRow);
                }
            }

            parameters[23] = new NameValue("@INSERT_TEMP_CLAUSE", dtClauses);

            if (companyFidelity.Risk.DynamicProperties != null && companyFidelity.Risk.DynamicProperties.Count > 0)
            {
                DynamicPropertiesCollection dynamicCollectionRisk = new DynamicPropertiesCollection();
                for (int i = 0; i < companyFidelity.Risk.DynamicProperties.Count(); i++)
                {
                    DynamicProperty dinamycProperty = new DynamicProperty();
                    dinamycProperty.Id = companyFidelity.Risk.DynamicProperties[i].Id;
                    dinamycProperty.Value = companyFidelity.Risk.DynamicProperties[i].Value;
                    dynamicCollectionRisk[i] = dinamycProperty;
                }
                byte[] serializedValuesRisk = dynamicPropertiesSerializer.Serialize(dynamicCollectionRisk);
                parameters[24] = new NameValue("@DYNAMIC_PROPERTIES", serializedValuesRisk);
            }
            else
            {
                parameters[24] = new NameValue("@DYNAMIC_PROPERTIES", null);
            }

            parameters[25] = new NameValue("@INSPECTION_ID", DBNull.Value);

            DataTable dtDynamicProperties = new DataTable("INSERT_TEMP_DYNAMIC_PROPERTIES");
            dtDynamicProperties.Columns.Add("DYNAMIC_ID", typeof(int));
            dtDynamicProperties.Columns.Add("CONCEPT_VALUE", typeof(string));
            if (companyFidelity.Risk.DynamicProperties != null)
            {
                foreach (var item in companyFidelity.Risk.DynamicProperties)
                {
                    DataRow dataRow = dtDynamicProperties.NewRow();
                    dataRow["DYNAMIC_ID"] = item.Id;
                    dataRow["CONCEPT_VALUE"] = item.Value ?? "NO ASIGNADO";
                    dtDynamicProperties.Rows.Add(dataRow);
                }
            }
            parameters[26] = new NameValue("@INSERT_TEMP_DYNAMIC_PROPERTIES", dtDynamicProperties);

            DataTable dtDynamicPropertiesCoverage = new DataTable("PARAM_INSERT_TEMP_DYNAMIC_PROPERTIES_COVERAGE");
            dtDynamicPropertiesCoverage.Columns.Add("DYNAMIC_ID", typeof(int));
            dtDynamicPropertiesCoverage.Columns.Add("CONCEPT_VALUE", typeof(string));

            parameters[27] = new NameValue("@RISK_NUM", companyFidelity.Risk.Number);
            parameters[28] = new NameValue("@RISK_INSP_TYPE_CD", 1);
            parameters[29] = new NameValue("OPERATION", JsonConvert.SerializeObject(companyFidelity));
             
            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("ISS.RECORD_RISK_FINANCES", parameters);
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
                throw new ValidationException(RES.Errors.ErrorRecordEndorsement);
            }
        }

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        public CompanyFidelityRisk GetCompanyFidelityByRiskId(int riskId)
        {
            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(riskId);

            if (pendingOperation != null)
            {
                CompanyFidelityRisk companyFidelity = JsonConvert.DeserializeObject<CompanyFidelityRisk>(pendingOperation.Operation);
                companyFidelity.Risk.Id = pendingOperation.Id;
                companyFidelity.Risk.IsPersisted = true;

                return companyFidelity;
            }
            else
            {
                return null;
            }
        }

        public List<OccupationDTO> GetOccupations(List<IssuanceOccupation> occupations)
        {
            return DTOAssembler.CreateOccupations(occupations);
        }

        public List<CompanyFidelityRisk> GetCompanyFidelityRisksByInsuredId(int insuredId)
        {
            List<CompanyFidelityRisk> companyFidelityRisks = new List<CompanyFidelityRisk>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(insuredId);

            ClaimRiskFidelityView claimRiskFidelityView = new ClaimRiskFidelityView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskFidelityView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimRiskFidelityView);

            if (claimRiskFidelityView.RiskFidelities.Count > 0)
            {
                foreach (ISSEN.RiskFidelity entityRiskFidelity in claimRiskFidelityView.RiskFidelities)
                {
                    CompanyFidelityRisk companyFidelityRisk = new CompanyFidelityRisk();

                    ISSEN.Risk entityRisk = claimRiskFidelityView.Risks.Cast<ISSEN.Risk>().FirstOrDefault(x => x.RiskId == entityRiskFidelity.RiskId);
                    ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskFidelityView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().FirstOrDefault(x => x.RiskId == entityRiskFidelity.RiskId);
                    ISSEN.Policy entityPolicy = claimRiskFidelityView.Policies.Cast<ISSEN.Policy>().FirstOrDefault(x => x.PolicyId == entityEndorsementRisk.PolicyId);

                    PARAMEN.RiskCommercialClass entityRiskCommercialClass = claimRiskFidelityView.RiskCommercialClasses.Cast<PARAMEN.RiskCommercialClass>().FirstOrDefault(x => x.RiskCommercialClassCode == entityRiskFidelity.RiskCommercialClassCode);

                    companyFidelityRisk = ModelAssembler.CreateFidelityRisk(entityRisk, entityRiskFidelity, entityEndorsementRisk);

                    companyFidelityRisk.Risk.RiskActivity.Description = entityRiskCommercialClass?.Description;

                    companyFidelityRisk.Risk.Policy.DocumentNumber = entityPolicy.DocumentNumber;
                    companyFidelityRisk.Risk.Policy.Endorsement = new UnderwritingServices.CompanyEndorsement
                    {
                        Id = entityEndorsementRisk.EndorsementId
                    };

                    companyFidelityRisk.Risk.Description = (!string.IsNullOrEmpty(entityRiskFidelity.Description) ? entityRiskFidelity.Description + " - " : "") + entityRiskCommercialClass?.Description;

                    ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                    filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filterSumAssured.Equal();
                    filterSumAssured.Constant(entityEndorsementRisk.EndorsementId);

                    SumAssuredFidelityView assuredView = new SumAssuredFidelityView();
                    ViewBuilder builderAssured = new ViewBuilder("SumAssuredMarineView");
                    builderAssured.Filter = filterSumAssured.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                    decimal insuredAmount = 0;

                    foreach (var item in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                    {
                        insuredAmount += item.LimitAmount;
                    }

                    companyFidelityRisk.Risk.AmountInsured = insuredAmount;

                    companyFidelityRisks.Add(companyFidelityRisk);  
                }

                return companyFidelityRisks;
            }

            return companyFidelityRisks;
        }

        public CompanyFidelityRisk GetCompanyFidelityRiskByRiskId(int riskId)
        {
            CompanyFidelityRisk companyFidelityRisk = new CompanyFidelityRisk();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.RiskId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(riskId);

            ClaimRiskFidelityView claimRiskFidelityView = new ClaimRiskFidelityView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskFidelityView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimRiskFidelityView);

            if (claimRiskFidelityView.RiskFidelities.Count > 0)
            {
                ISSEN.RiskFidelity entityRiskFidelity = claimRiskFidelityView.RiskFidelities.Cast<ISSEN.RiskFidelity>().First();
                ISSEN.Risk entityRisk = claimRiskFidelityView.Risks.Cast<ISSEN.Risk>().FirstOrDefault(x => x.RiskId == entityRiskFidelity.RiskId);
                ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskFidelityView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().FirstOrDefault(x => x.RiskId == entityRiskFidelity.RiskId);
                ISSEN.Policy entityPolicy = claimRiskFidelityView.Policies.Cast<ISSEN.Policy>().FirstOrDefault(x => x.PolicyId == entityEndorsementRisk.PolicyId);

                PARAMEN.RiskCommercialClass entityRiskCommercialClass = claimRiskFidelityView.RiskCommercialClasses.Cast<PARAMEN.RiskCommercialClass>().FirstOrDefault(x => x.RiskCommercialClassCode == entityRiskFidelity.RiskCommercialClassCode);

                companyFidelityRisk = ModelAssembler.CreateFidelityRisk(entityRisk, entityRiskFidelity, entityEndorsementRisk);

                companyFidelityRisk.Risk.RiskActivity.Description = entityRiskCommercialClass?.Description;

                companyFidelityRisk.Risk.Policy.DocumentNumber = entityPolicy.DocumentNumber;
                companyFidelityRisk.Risk.Policy.Endorsement = new UnderwritingServices.CompanyEndorsement
                {
                    Id = entityEndorsementRisk.EndorsementId
                };

                companyFidelityRisk.Risk.Description = (!string.IsNullOrEmpty(entityRiskFidelity.Description) ? entityRiskFidelity.Description + " - " : "") + entityRiskCommercialClass?.Description;

                ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                filterSumAssured.Equal();
                filterSumAssured.Constant(entityEndorsementRisk.EndorsementId);

                SumAssuredFidelityView assuredView = new SumAssuredFidelityView();
                ViewBuilder builderAssured = new ViewBuilder("SumAssuredMarineView");
                builderAssured.Filter = filterSumAssured.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                decimal insuredAmount = 0;

                foreach (var item in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                {
                    insuredAmount += item.LimitAmount;
                }

                companyFidelityRisk.Risk.AmountInsured = insuredAmount; 

                return companyFidelityRisk;
            }

            return null;
        }

        public List<CompanyFidelityRisk> GetCompanyClaimFidelitiesByEndorsementId(int endorsementId)
        {
            List<CompanyFidelityRisk> companyFidelities = new List<CompanyFidelityRisk>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            ClaimRiskFidelityView claimRiskFidelityView = new ClaimRiskFidelityView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskFidelityView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimRiskFidelityView);

            if (claimRiskFidelityView.RiskFidelities.Count > 0)
            {
                foreach (ISSEN.RiskFidelity entityRiskFidelity in claimRiskFidelityView.RiskFidelities)
                {
                    CompanyFidelityRisk companyFidelityRisk = new CompanyFidelityRisk();

                    ISSEN.Risk entityRisk = claimRiskFidelityView.Risks.Cast<ISSEN.Risk>().FirstOrDefault(x => x.RiskId == entityRiskFidelity.RiskId);
                    ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskFidelityView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().FirstOrDefault(x => x.RiskId == entityRiskFidelity.RiskId);
                    ISSEN.Policy entityPolicy = claimRiskFidelityView.Policies.Cast<ISSEN.Policy>().FirstOrDefault(x => x.PolicyId == entityEndorsementRisk.PolicyId);

                    PARAMEN.RiskCommercialClass entityRiskCommercialClass = claimRiskFidelityView.RiskCommercialClasses.Cast<PARAMEN.RiskCommercialClass>().FirstOrDefault(x => x.RiskCommercialClassCode == entityRiskFidelity.RiskCommercialClassCode);

                    companyFidelityRisk = ModelAssembler.CreateFidelityRisk(entityRisk, entityRiskFidelity, entityEndorsementRisk);

                    companyFidelityRisk.Risk.RiskActivity.Description = entityRiskCommercialClass?.Description;

                    companyFidelityRisk.Risk.Policy.DocumentNumber = entityPolicy.DocumentNumber;
                    companyFidelityRisk.Risk.Policy.Endorsement = new UnderwritingServices.CompanyEndorsement
                    {
                        Id = entityEndorsementRisk.EndorsementId
                    };

                    companyFidelityRisk.Risk.Description = (!string.IsNullOrEmpty(entityRiskFidelity.Description) ? entityRiskFidelity.Description + " - " : "") + entityRiskCommercialClass?.Description;

                    ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                    filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filterSumAssured.Equal();
                    filterSumAssured.Constant(entityEndorsementRisk.EndorsementId);

                    SumAssuredFidelityView assuredView = new SumAssuredFidelityView();
                    ViewBuilder builderAssured = new ViewBuilder("SumAssuredMarineView");
                    builderAssured.Filter = filterSumAssured.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                    decimal insuredAmount = 0;

                    foreach (var item in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                    {
                        insuredAmount += item.LimitAmount;
                    }

                    companyFidelityRisk.Risk.AmountInsured = insuredAmount;

                    companyFidelities.Add(companyFidelityRisk);
                }

                return companyFidelities;
            }

            return companyFidelities;
        }
    }
}