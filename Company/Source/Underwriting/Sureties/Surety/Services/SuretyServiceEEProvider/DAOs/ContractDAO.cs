using Newtonsoft.Json;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Assemblers;
using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Entities.View;
using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Models;
using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Resources;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.DTOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using ENT = Sistran.Company.Application.Issuance.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PER = Sistran.Core.Application.UniquePerson.Entities;
using Rules = Sistran.Core.Framework.Rules;
using TEM = Sistran.Core.Application.Temporary.Entities;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
using CU = Sistran.Core.Application.Utilities.Utility;
using UTILITES = Company.UnderwritingUtilities;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota;
using Sistran.Core.Integration.OperationQuotaServices.Enums;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Enums;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Company.Application.ExternalProxyServices.Models;
namespace Sistran.Company.Application.Sureties.SuretyServices.EEProvider.DAOs
{
    using Sistran.Company.Application.UniquePersonServices.V1.Models;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium;
    using Sureties.Models;
    using System.Threading;

    /// <summary>
    /// Datos Cumplimiento
    /// </summary>
    public class ContractDAO
    {

        /// <summary>
        /// Gets the company surety by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <returns></returns>
        public List<CompanyContract> GetCompanySuretyByEndorsementId(int endorsementId, int riskId)
        {
            List<CompanyContract> companyContracts = new List<CompanyContract>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            if (riskId > 0)
            {
                filter.And();
                filter.Property(ISSEN.EndorsementRisk.Properties.RiskId, typeof(ISSEN.EndorsementRisk).Name);
                filter.Equal();
                filter.Constant(riskId);
            }

            RiskSuretyView view = new RiskSuretyView();
            ViewBuilder builder = new ViewBuilder("RiskSuretyView") { Filter = filter.GetPredicate() };
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view != null && view.Risks != null && view.Risks.Count > 0)
            {
                var endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                var risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                var riskSureties = view.RiskSureties.Cast<ISSEN.RiskSurety>().ToList();
                var riskSuretyGuarantees = view.RiskSuretyGuarantees.Cast<ISSEN.RiskSuretyGuarantee>().ToList();
                var riskSuretyContracts = view.RiskSuretyContracts.Cast<ISSEN.RiskSuretyContract>().ToList();
                var riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                var policies = view.Policies.Cast<ISSEN.Policy>().ToList();

                object objlock = new object();
                ConcurrentBag<string> errors = new ConcurrentBag<string>();
                CU.Parallel.For(0, risks.Count, itemRow =>
                {
                    IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();
                    ISSEN.Risk item = null;
                    lock (objlock)
                    {
                        item = risks[itemRow];
                    }
                    dataFacade.LoadDynamicProperties(item);
                    ISSEN.EndorsementRisk endorsementRisk = endorsementRisks.First(x => x.RiskId == item.RiskId);

                    ContractDto contractDto = new ContractDto();
                    contractDto.Risk = item;
                    contractDto.Policy = policies.First(x => x.PolicyId == endorsementRisk.PolicyId);
                    contractDto.RiskSurety = riskSureties.First(x => x.RiskId == item.RiskId);
                    contractDto.RiskSuretyContract = riskSuretyContracts.First(x => x.RiskId == item.RiskId);
                    contractDto.EndorsementRisk = endorsementRisk;
                    var contract = ModelAssembler.CreateContract(contractDto);
                    Rules.Facade facade = new Rules.Facade();
                    if (contract != null)
                    {
                        int insuredNameNum = contract.Risk.MainInsured.CompanyName.NameNum;


                        var companyInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(contract.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                        if (companyInsured != null)
                        {
                            contract.Contractor.IndividualType = IndividualType.Company;
                            contract.Contractor.CustomerType = CustomerType.Individual;
                            contract.Risk.MainInsured = companyInsured;
                            var companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(contract.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();

                            contract.Risk.MainInsured.CompanyName = CU.Task.Run(() =>
                            {

                                return new IssuanceCompanyName
                                {
                                    NameNum = companyName.NameNum,
                                    TradeName = companyName.TradeName,
                                    Address = new IssuanceAddress
                                    {
                                        Id = companyName.Address.Id,
                                        Description = companyName.Address.Description,
                                        City = companyName.Address.City
                                    },
                                    Phone = companyName?.Phone == null ? null : new IssuancePhone
                                    {
                                        Id = companyName.Phone.Id,
                                        Description = companyName.Phone.Description
                                    },
                                    Email = companyName?.Email == null ? null : new IssuanceEmail
                                    {
                                        Id = companyName.Email.Id,
                                        Description = companyName.Email.Description
                                    },
                                };
                            }).Result;
                        }

                        if (contract.Contractor != null)
                        {
                            contract.Contractor.CompanyName = CU.Task.Run(() =>
                            {
                                var companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(contract.Contractor.IndividualId, CustomerType.Individual).First();
                                return new IssuanceCompanyName
                                {
                                    NameNum = companyName.NameNum,
                                    TradeName = companyName.TradeName,
                                    Address = new IssuanceAddress
                                    {
                                        Id = companyName.Address.Id,
                                        Description = companyName.Address.Description,
                                        City = companyName.Address.City
                                    },
                                    Phone = companyName?.Phone == null ? null : new IssuancePhone
                                    {
                                        Id = companyName.Phone.Id,
                                        Description = companyName.Phone.Description
                                    },
                                    Email = companyName?.Email == null ? null : new IssuanceEmail
                                    {
                                        Id = companyName.Email.Id,
                                        Description = companyName.Email.Description
                                    }
                                };
                            }).Result;
                            var ContractorIdentificacion = CU.Task.Run(() => { return DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(contract.Contractor.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault(); }).Result;
                            contract.Contractor.Name = ContractorIdentificacion.Name;
                            contract.Contractor.IdentificationDocument = new IssuanceIdentificationDocument
                            {
                                Number = ContractorIdentificacion.IdentificationDocument.Number,
                                DocumentType = new IssuanceDocumentType
                                {
                                    Id = ContractorIdentificacion.IdentificationDocument.DocumentType.Id,
                                }
                            };

                        }
                        else
                        {
                            errors.Add("Error Afianzado Vacio");
                        }
                        contract.Guarantees = new List<CiaRiskSuretyGuarantee>();
                        if (riskSuretyGuarantees != null && riskSuretyGuarantees.Any())
                        {
                            CU.Parallel.For(0, riskSuretyGuarantees.Where(x => x.RiskId == item.RiskId).Count(), itemrisk =>
                            {
                                var riskSuretyGuarante = riskSuretyGuarantees.Where(x => x.RiskId == item.RiskId).ToList()[itemrisk];
                                lock (objlock)
                                {
                                    contract.Guarantees.Add(new CiaRiskSuretyGuarantee
                                    {
                                        InsuredGuarantee = new InsuredGuarantee
                                        {
                                            Id = riskSuretyGuarante.GuaranteeId
                                        }
                                    });
                                }

                            });
                        }
                        contract.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                        var beneficiaries = riskBeneficiaries.Where(x => x.RiskId == item.RiskId).ToList();
                        if (beneficiaries != null && beneficiaries.Any())
                        {
                            CU.Parallel.For(0, beneficiaries.Count(), itemrisk =>
                            {

                                var beneficiary = ModelAssembler.CreateBeneficiary(beneficiaries[itemrisk]);
                                if (beneficiary != null)
                                {
                                    int Beneficiarie_associationType = GetDataAssociationType(beneficiary.IndividualId);
                                    if (beneficiary.AssociationType == null)
                                    {
                                        beneficiary.AssociationType = new IssuanceAssociationType();
                                    }

                                    beneficiary.AssociationType.Id = Beneficiarie_associationType == 0 ? 1 : Beneficiarie_associationType;


                                    int beneficiaryNameNum = beneficiary.CompanyName.NameNum;
                                    List<CompanyName> companyNames = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(beneficiary.IndividualId, beneficiary.CustomerType);
                                    var companyName = new CompanyName();
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
                                        Phone = companyName?.Phone == null ? null : new IssuancePhone
                                        {
                                            Id = companyName.Phone.Id,
                                            Description = companyName.Phone.Description
                                        },
                                        Email = companyName?.Email == null ? null : new IssuanceEmail
                                        {
                                            Id = companyName.Email.Id,
                                            Description = companyName.Email.Description
                                        }
                                    };
                                    var beneficiaryDocument = DelegateService.underwritingService.GetBeneficiariesByDescription(beneficiary.IndividualId.ToString(), InsuredSearchType.IndividualId);
                                    if (beneficiaryDocument != null && beneficiaryDocument.Any())
                                    {
                                        var beneficiaryData = beneficiaryDocument.First();
                                        beneficiary.Name = beneficiaryData.Name;
                                        beneficiary.IdentificationDocument = beneficiaryData.IdentificationDocument;
                                    }
                                    else
                                    {
                                        errors.Add("Error obteniendo nro Documento del Beneficiario");
                                    }
                                    lock (objlock)
                                    {
                                        contract.Risk.Beneficiaries.Add(beneficiary);
                                    }
                                }
                                else
                                {
                                    errors.Add("Error creando Beneficiario");
                                }
                            });
                        }
                        else
                        {
                            errors.Add("Error Creando Beneficiario");
                        }

                        var coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(endorsementRisks.First(x => x.RiskId == item.RiskId).PolicyId, endorsementId, item.RiskId);
                        if (coverages != null && coverages.Any())
                        {
                            contract.Risk.Coverages = coverages;
                        }
                        else
                        {
                            errors.Add("Error Obteniendo Coberturas" + item.RiskId.ToString());
                        }

                        lock (objlock)
                        {
                            companyContracts.Add(contract);
                        }
                    }
                    else
                    {
                        errors.Add("Error Creando Riesgo");

                    }
                });
            }

            return companyContracts;
        }

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="contract">Modelo Contract</param>
        public CompanyContract CreateContractTemporal(CompanyContract contract, bool isMassive)
        {
            //try
            //{
            if (contract == null)
            {
                throw new Exception(Errors.ErrorParameterContratEmpty);
            }
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            EventLog.WriteEntry("UnderwritingSurety", $"Inicia Creacion Politicas: {DateTime.Now}");
            contract.Risk.InfringementPolicies = ValidateAuthorizationPolicies(contract);
            EventLog.WriteEntry("UnderwritingSurety", $"Finaliza Creacion Politicas: {DateTime.Now}");
            string strUseReplicatedDatabase = DelegateService.commonService.GetKeyApplication("UseReplicatedDatabase");
            bool boolUseReplicatedDatabase = strUseReplicatedDatabase == "true";
            PendingOperation pendingOperation = new PendingOperation();
            EventLog.WriteEntry("UnderwritingSurety", $"Inicia Creacion Riesgo: {DateTime.Now}");
            if (contract.Risk.Id == 0)
            {
                pendingOperation.ParentId = contract.Risk.Policy.Id;
                pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(contract);

                if (isMassive && boolUseReplicatedDatabase)
                {
                    //Se guarda el JSON en la base de datos de réplica
                }
                else
                {
                    pendingOperation = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);
                }
                contract = COMUT.JsonHelper.DeserializeJson<CompanyContract>(pendingOperation.Operation);
            }
            else
            {
                pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(contract.Risk.Id);
                pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(contract);
                if (isMassive && boolUseReplicatedDatabase)
                {
                    //Se guarda el JSON en la base de datos de réplica
                }
                else
                {
                    pendingOperation = DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
                }
                contract = COMUT.JsonHelper.DeserializeJson<CompanyContract>(pendingOperation.Operation);
            }
            if (contract == null)
            {
                throw new Exception(Errors.ErrorUpdateRisk);
            }
            contract.Risk.Id = pendingOperation.Id;
            EventLog.WriteEntry("UnderwritingSurety", $"Finaliza Creacion Riesgo: {DateTime.Now}");
            stopWatch.Stop();
            //SaveCompanyContractTemporalTables(contract);
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.sureties.SuretyServices.EEProvider.DAOs.CreateVehicleTemporal");

            return contract;
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

        }

        /// <summary>
        /// Gets the company contracts by policy identifier.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <returns></returns>
        public List<CompanyContract> GetCompanyContractsByPolicyId(int policyId)
        {
            List<CompanyContract> companyContracts = new List<CompanyContract>();

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
            RiskSuretyView view = new RiskSuretyView();
            ViewBuilder builder = new ViewBuilder("RiskSuretyView") { Filter = filter.GetPredicate() };
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            DataFacadeManager.Dispose();


            if (view?.Risks?.Count > 0)
            {
                var endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                var risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                var riskSureties = view.RiskSureties.Cast<ISSEN.RiskSurety>().ToList();
                var riskSuretyGuarantees = view.RiskSuretyGuarantees.Cast<ISSEN.RiskSuretyGuarantee>().ToList();

                var insuredGuarantee = view.InsuredGuarantee?.Cast<UPEN.InsuredGuarantee>().ToList() ?? null;
                var riskSuretyContracts = view.RiskSuretyContracts.Cast<ISSEN.RiskSuretyContract>().ToList();
                var CoRiskSurety = view.CoRiskSurety?.Cast<ISSEN.CoRiskSurety>().ToList();
                List<ISSEN.RiskClause> riskClause = null;

                RiskSuretyClauseView viewClause = new RiskSuretyClauseView();
                ViewBuilder builderClause = new ViewBuilder("RiskSuretyClauseView") { Filter = filter.GetPredicate() };
                DataFacadeManager.Instance.GetDataFacade().FillView(builderClause, viewClause);
                DataFacadeManager.Dispose();
                if (viewClause?.RiskClauses?.Count > 0)
                {
                    riskClause = viewClause.RiskClauses?.Cast<ISSEN.RiskClause>().ToList();
                }


                var riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
                var policies = view.Policies.Cast<ISSEN.Policy>().ToList();
                if (risks?.Count > 0 && policies?.Count > 0 && riskSureties?.Count > 0)
                {
                    object objlock = new object();
                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    CU.Parallel.For(0, risks.Count, itemRow =>
                    {
                        try
                        {

                            ISSEN.Risk item = null;
                            lock (objlock)
                            {
                                item = risks[itemRow];
                            }
                            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();
                            dataFacade.LoadDynamicProperties(item);
                            dataFacade.Dispose();
                            ISSEN.EndorsementRisk endorsementRisk = endorsementRisks.First(x => x.RiskId == item.RiskId);
                            ContractDto contractDto = new ContractDto();
                            contractDto.Risk = item;
                            contractDto.Policy = policies.First(x => x.PolicyId == endorsementRisk.PolicyId);
                            contractDto.RiskSurety = riskSureties.First(x => x.RiskId == item.RiskId);
                            contractDto.RiskSuretyContract = riskSuretyContracts.FirstOrDefault(x => x.RiskId == item.RiskId);
                            contractDto.CoRiskSurety = CoRiskSurety?.FirstOrDefault(x => x.RiskId == item.RiskId);

                            contractDto.EndorsementRisk = endorsementRisk;
                            var contract = ModelAssembler.CreateContract(contractDto);
                            if (contract != null)
                            {
                                int insuredNameNum = contract.Risk.MainInsured.CompanyName.NameNum;

                                var companyInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(contract.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                                if (companyInsured != null)
                                {

                                    contract.Contractor.IndividualType = IndividualType.Company;
                                    contract.Contractor.CustomerType = CustomerType.Individual;
                                    contract.Risk.MainInsured = companyInsured;
                                    contract.Risk.MainInsured.CompanyName = CU.Task.Run(() =>
                                    {
                                        CompanyName companyName = new CompanyName();
                                        if (contractDto.Risk.NameNum != null)
                                        {
                                            companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(contract.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault(x => x.NameNum == contractDto.Risk.NameNum);
                                        }
                                        else
                                        {
                                            companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(contract.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
                                        }
                                        return new IssuanceCompanyName
                                        {
                                            NameNum = insuredNameNum != null ? insuredNameNum : companyName.NameNum,
                                            TradeName = companyName.TradeName,
                                            Address = new IssuanceAddress
                                            {
                                                Id = companyName.Address.Id,
                                                Description = companyName.Address.Description,
                                                City = companyName.Address.City
                                            },
                                            Phone = companyName?.Phone == null ? null : new IssuancePhone
                                            {
                                                Id = companyName.Phone.Id,
                                                Description = companyName.Phone.Description
                                            },
                                            Email = companyName?.Email == null ? null : new IssuanceEmail
                                            {
                                                Id = companyName.Email.Id,
                                                Description = companyName.Email.Description
                                            }
                                        };
                                    }).Result;
                                }
                                if (contract.Contractor != null)
                                {
                                    var companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(contract.Contractor.IndividualId, CustomerType.Individual).First();

                                    contract.Contractor.CompanyName = CU.Task.Run(() =>
                                    {
                                        return new IssuanceCompanyName
                                        {
                                            NameNum = companyName.NameNum,
                                            TradeName = companyName.TradeName,
                                            Address = new IssuanceAddress
                                            {
                                                Id = companyName.Address.Id,
                                                Description = companyName.Address.Description,
                                                City = companyName.Address.City
                                            },
                                            Phone = companyName?.Phone == null ? null : new IssuancePhone
                                            {
                                                Id = companyName.Phone.Id,
                                                Description = companyName.Phone.Description
                                            }
                                        };

                                    }).Result;

                                    if (companyName?.Email != null)
                                    {
                                        contract.Contractor.CompanyName.Email = companyName == null ? null : new IssuanceEmail
                                        {
                                            Id = companyName.Email.Id,
                                            Description = companyName.Email.Description
                                        };
                                    }
                                    var ContractorIdentificacion = CU.Task.Run(() => { return DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(contract.Contractor.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault(); }).Result;
                                    contract.Contractor.Name = ContractorIdentificacion.Name;
                                    contract.Contractor.InsuredId = ContractorIdentificacion.InsuredId;
                                    contract.Contractor.IdentificationDocument = new IssuanceIdentificationDocument
                                    {
                                        Number = ContractorIdentificacion.IdentificationDocument.Number,
                                        DocumentType = new IssuanceDocumentType
                                        {
                                            Id = ContractorIdentificacion.IdentificationDocument.DocumentType.Id,
                                        }
                                    };
                                }
                                else
                                {
                                    errors.Add("Error Creando Afianzado");
                                }
                                contract.Guarantees = new List<CiaRiskSuretyGuarantee>();
                                if (riskSuretyGuarantees != null && riskSuretyGuarantees.Any())
                                {
                                    CU.Parallel.For(0, riskSuretyGuarantees.Where(x => x.RiskId == item.RiskId).Count(), itemrisk =>
                                    {
                                        var riskSuretyGuarante = riskSuretyGuarantees.Where(x => x.RiskId == item.RiskId).ToList()[itemrisk];
                                        var firstInsuredGuarantee = insuredGuarantee?.FirstOrDefault(x => x.GuaranteeId == riskSuretyGuarante.GuaranteeId);
                                        lock (objlock)
                                        {
                                            contract.Guarantees.Add(new CiaRiskSuretyGuarantee
                                            {
                                                InsuredGuarantee = new InsuredGuarantee
                                                {
                                                    Id = riskSuretyGuarante.GuaranteeId,
                                                    Status = new GuaranteeStatus
                                                    {
                                                        Id = firstInsuredGuarantee?.GuaranteeStatusCode.GetValueOrDefault() ?? 0
                                                    },
                                                    IsCloseInd = firstInsuredGuarantee?.ClosedInd ?? false,
                                                    AppraisalAmount = firstInsuredGuarantee?.AppraisalAmount ?? firstInsuredGuarantee?.DocumentValueAmount
                                                }
                                            });
                                        }

                                    });
                                }
                                contract.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                                var beneficiaries = riskBeneficiaries.Where(x => x.RiskId == item.RiskId).ToList();
                                if (beneficiaries != null && beneficiaries.Any())
                                {
                                    CU.Parallel.For(0, beneficiaries.Count(), itemrisk =>
                                    {

                                        var beneficiary = ModelAssembler.CreateBeneficiary(beneficiaries[itemrisk]);
                                        if (beneficiary != null)
                                        {
                                            int Beneficiarie_associationType = GetDataAssociationType(beneficiary.IndividualId);
                                            if (beneficiary.AssociationType == null)
                                            {
                                                beneficiary.AssociationType = new IssuanceAssociationType();
                                            }

                                            beneficiary.AssociationType.Id = Beneficiarie_associationType == 0 ? 1 : Beneficiarie_associationType;

                                            int beneficiaryNameNum = beneficiary.CompanyName.NameNum;
                                            List<CompanyName> companyNames = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(beneficiary.IndividualId, beneficiary.CustomerType);
                                            CompanyName companyName = new CompanyName();
                                            if (companyNames != null && companyNames.Any())
                                            {
                                                if (beneficiaryNameNum == 0)
                                                {
                                                    bool noBeneficiary = false;
                                                    foreach (CompanyName companyN in companyNames)
                                                    {
                                                        if (companyN.IsMain)
                                                        {
                                                            companyName = companyNames.First(x => x.IsMain);
                                                            noBeneficiary = true;
                                                        }
                                                    }
                                                    if (noBeneficiary == false)
                                                    {
                                                        errors.Add("Beneficiario sin Razon Social por defecto");
                                                        companyName = new CompanyName();
                                                    }

                                                }
                                                else
                                                {
                                                    companyName = companyNames.First(x => x.NameNum == beneficiaryNameNum);
                                                }
                                                if (companyName.IsMain)
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
                                                        Phone = companyName?.Phone == null ? null : new IssuancePhone
                                                        {
                                                            Id = companyName.Phone.Id,
                                                            Description = companyName.Phone.Description
                                                        },
                                                        Email = companyName?.Email == null ? null : new IssuanceEmail
                                                        {
                                                            Id = companyName.Email.Id,
                                                            Description = string.IsNullOrEmpty(companyName.Email.Description) == true ? "correo@correofalta.co" : companyName.Email.Description
                                                        }
                                                    };
                                                }
                                            }
                                            else
                                            {
                                                errors.Add("Beneficario sin direccion principal o Telefono");
                                            }
                                            var beneficiaryDocument = DelegateService.underwritingService.GetBeneficiariesByDescription(beneficiary.IndividualId.ToString(), InsuredSearchType.IndividualId);
                                            if (beneficiaryDocument != null && beneficiaryDocument.Any())
                                            {
                                                var beneficiaryData = beneficiaryDocument.First();
                                                beneficiary.Name = beneficiaryData.Name;
                                                beneficiary.IdentificationDocument = beneficiaryData.IdentificationDocument;
                                            }
                                            else
                                            {
                                                errors.Add("Error obteniendo nro Documento del Beneficiario");
                                            }
                                            lock (objlock)
                                            {
                                                contract.Risk.Beneficiaries.Add(beneficiary);
                                            }
                                        }

                                    });
                                }
                                contract.Risk.Clauses = new List<CompanyClause>();

                                if (riskClause != null && riskClause.Count > 0)
                                {
                                    var clauses = riskClause.Where(x => x.RiskId == item.RiskId).ToList();
                                    if (clauses != null && clauses.Count > 0)
                                    {
                                        contract.Risk.Clauses = DelegateService.underwritingService.AddClauses(contract.Risk.Clauses, clauses.Select(x => x.ClauseId).ToList());
                                    }
                                    else
                                    {
                                        errors.Add("Error Obteniendo Clausulas");
                                    }
                                }
                                var coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementRisks.First(x => x.RiskId == item.RiskId).EndorsementId, item.RiskId);
                                if (coverages != null && coverages.Any())
                                {
                                    contract.Risk.Coverages = coverages;
                                }
                                else
                                {
                                    errors.Add("Error Obteniendo Coberturas" + item.RiskId.ToString());
                                }
                                //Se agrega consulta de datos de reserva tecnica
                                var dataReservaTecnica = GetRiskSuretyPost(item.RiskId);
                                contract.RiskSuretyPost = dataReservaTecnica;
                                //
                                contract.IsRetention = GetIsRetention(item.RiskId);
                                lock (objlock)
                                {
                                    companyContracts.Add(contract);
                                }
                            }
                            else
                            {
                                errors.Add("Error Creando el riesgo");
                            }

                        }
                        catch (Exception ex)
                        {

                            errors.Add(ex.GetBaseException().Message);
                        }

                    });
                    if (errors != null && errors.Any())
                    {
                        throw new Exception(string.Join(" ", errors));
                    }
                    else
                    {
                        return companyContracts;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener las vigencias coberturas por endosos de una poliza
        /// </summary>
        /// <param name="policyId">Id póliza</param>
        /// <param name="riskId">Id riesgo</param>    
        /// <returns>Lista de coberturas</returns>
        public List<CompanyCoverage> GetCoveragesByPolicyIdByRiskId(int policyId, int riskId)
        {
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("POLICY_ID", policyId);
            parameters[1] = new NameValue("RISK_ID", riskId);
            DataTable dataTable;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("ISS.READ_COVERAGES_INITIAL", parameters);
            }
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return null;
            }
            List<CompanyCoverage> coverages = new List<CompanyCoverage>();
            var datarows = dataTable.Rows.Cast<DataRow>().ToList();
            object obj = new object();
            CU.Parallel.For(0, datarows.Count, itemId =>
            {
                var arrayItem = datarows[itemId];
                CompanyCoverage coverage = new CompanyCoverage();
                coverage.Id = Convert.ToInt32(arrayItem[0]);
                coverage.CurrentFromOriginal = Convert.ToDateTime(arrayItem[1]);
                coverage.CurrentToOriginal = Convert.ToDateTime(arrayItem[2]);
                lock (obj)
                {
                    coverages.Add(coverage);
                }

            });
            return coverages;
        }

        public List<PoliciesAut> ValidateAuthorizationPolicies(CompanyContract companyContract)
        {
            if (companyContract.Contractor.SinisterCount == null && companyContract.Contractor.TechnicalCard == null && companyContract.Contractor.AssociationTypeId == null)
            {

                companyContract.Contractor.TechnicalCard = GetDataTechnicalCard(companyContract.Contractor.IndividualId);
                int associationType = GetDataAssociationType(companyContract.Contractor.IndividualId);
                companyContract.Contractor.AssociationTypeId = associationType == 0 ? null : (int?)associationType;
                companyContract.Contractor.IsConsortium = GetAssociationType(associationType);
                companyContract.Contractor.SinisterCount = GetCountSinister(companyContract.Contractor.IndividualId);

            }

            if ((companyContract.Contractor.AssociationType != null && companyContract.Contractor.AssociationType.Id == 0) || companyContract.Contractor.AssociationType == null)
            {
                int Contractor_associationType = GetDataAssociationType(companyContract.Contractor.IndividualId);
                if (companyContract.Contractor.AssociationType == null)
                {
                    companyContract.Contractor.AssociationType = new IssuanceAssociationType();
                }

                companyContract.Contractor.AssociationType.Id = Contractor_associationType == 0 ? 1 : Contractor_associationType;

            }

            if ((companyContract.Risk.MainInsured.AssociationType != null && companyContract.Risk.MainInsured.AssociationType.Id == 0) || companyContract.Risk.MainInsured.AssociationType == null)
            {
                int MainInsuredassociationType = GetDataAssociationType(companyContract.Risk.MainInsured.IndividualId);
                if (companyContract.Risk.MainInsured.AssociationType == null)
                {
                    companyContract.Risk.MainInsured.AssociationType = new IssuanceAssociationType();
                }

                companyContract.Risk.MainInsured.AssociationType.Id = MainInsuredassociationType == 0 ? 1 : MainInsuredassociationType;
            }
            if (companyContract.Risk != null && companyContract.Risk.Policy != null)
            {
                companyContract.Risk.Policy.PortfolioBalance = GetHasPortfolioBalance(companyContract);
            }

            var key = companyContract.Risk.Policy.Prefix.Id + "," + (int)companyContract.Risk.Policy.Product.CoveredRisk.CoveredRiskType;
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();

            Rules.Facade facade = DelegateService.underwritingService.CreateFacadeGeneral(companyContract.Risk.Policy);
            facade.SetConcept(RuleConceptPolicies.UserId, companyContract.Risk.Policy.UserId);
            EntityAssembler.CreateFacadeRiskContract(facade, companyContract);

            /*Politica del riesgo*/
            policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(10, key,/*facadeList*/  facade, FacadeType.RULE_FACADE_RISK));

            /*Politicas de cobertura*/
            if (companyContract.Risk.Coverages != null)
            {
                foreach (var coverage in companyContract.Risk.Coverages)
                {

                    EntityAssembler.CreateFacadeCoverage(facade, coverage);

                    policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(10, key, /*facadeList*/ facade, FacadeType.RULE_FACADE_COVERAGE));
                }
            }

            return policiesAuts;
        }

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>
        /// Riesgos
        /// </returns>
        /// <exception cref="Exception"></exception>
        public List<CompanyContract> GetCompanySuretiesByTemporalId(int temporalId)
        {
            ConcurrentBag<CompanyContract> companyContract = new ConcurrentBag<CompanyContract>();
            List<PendingOperation> pendingOperations = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(temporalId);
            object objlock = new object();
            if (pendingOperations != null && pendingOperations.Any())
            {
                CU.Parallel.For(0, pendingOperations.Count, id =>
                {
                    PendingOperation pendingOperation;
                    lock (objlock)
                    {
                        pendingOperation = pendingOperations[id];
                    }
                    if (!string.IsNullOrWhiteSpace(pendingOperation.Operation))
                    {
                        CompanyContract contract = COMUT.JsonHelper.DeserializeJson<CompanyContract>(pendingOperation.Operation);
                        contract.Risk.Id = pendingOperation.Id;
                        companyContract.Add(contract);
                    }
                });

                return companyContract.ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Polizas asociadas a individual de asegurado
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<CompanyContract> GetContractsByIndividualId(int individualId)
        {
            ConcurrentBag<CompanyContract> companyContracts = new ConcurrentBag<CompanyContract>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);

            RiskSuretyView view = new RiskSuretyView();
            ViewBuilder builder = new ViewBuilder("RiskSuretyView") { Filter = filter.GetPredicate() };

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();

            if (view.Risks != null && view.Risks.Count > 0)
            {
                var endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                var risks = view.Risks.Cast<ISSEN.Risk>().ToList();
                var riskSureties = view.RiskSureties.Cast<ISSEN.RiskSurety>().ToList();
                var riskSuretyContracts = view.RiskSuretyContracts.Cast<ISSEN.RiskSuretyContract>().ToList();
                var policies = view.Policies.Cast<ISSEN.Policy>().ToList();

                CU.Parallel.For(0, risks.Count, id =>
                {
                    var item = risks[id];
                    ISSEN.EndorsementRisk endorsementRisk = endorsementRisks.First(x => x.RiskId == item.RiskId);
                    ISSEN.RiskSuretyContract riskSuretyContract = riskSuretyContracts.Where(x => x.RiskId == item.RiskId).FirstOrDefault();
                    if (riskSuretyContract != null)
                    {
                        ContractDto contractDto = new ContractDto();
                        contractDto.Risk = item;
                        contractDto.Policy = policies.First(x => x.PolicyId == endorsementRisk.PolicyId);
                        contractDto.RiskSurety = riskSureties.First(x => x.RiskId == item.RiskId);
                        contractDto.RiskSuretyContract = riskSuretyContracts.First(x => x.RiskId == item.RiskId);
                        contractDto.EndorsementRisk = endorsementRisk;
                        var contract = ModelAssembler.CreateContract(contractDto);
                        contract.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(contract.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
                        var companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(contract.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
                        contract.Contractor.IndividualType = IndividualType.Company;
                        contract.Contractor.CustomerType = CustomerType.Individual;
                        contract.Risk.MainInsured.CompanyName = new IssuanceCompanyName
                        {
                            NameNum = companyName.NameNum,
                            TradeName = companyName.TradeName,
                            Address = new IssuanceAddress
                            {
                                Id = companyName.Address.Id,
                                Description = companyName.Address.Description,
                                City = companyName.Address.City
                            },
                            Phone = companyName?.Phone == null ? null : new IssuancePhone
                            {
                                Id = companyName.Phone.Id,
                                Description = companyName.Phone.Description
                            },
                            Email = companyName?.Email == null ? null : new IssuanceEmail
                            {
                                Id = companyName.Email.Id,
                                Description = companyName.Email.Description
                            }
                        };
                        companyContracts.Add(contract);
                    }
                });
            }

            return companyContracts.ToList();
        }

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        public CompanyContract GetCompanyContractByRiskId(int riskId)
        {
            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(riskId);

            if (pendingOperation != null)
            {
                CompanyContract companyContract = COMUT.JsonHelper.DeserializeJson<CompanyContract>(pendingOperation.Operation);
                if (companyContract.Risk != null)
                {
                    companyContract.Risk.Id = pendingOperation.Id;
                    companyContract.Risk.IsPersisted = true;
                }
                else
                {
                    return null;
                }

                return companyContract;
            }
            else
            {
                return null;
            }
        }

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyContract> companyContracts)
        {
            try
            {
                if (companyPolicy == null || companyContracts == null)
                {
                    throw new Exception(Errors.ErrorParameterContratEmpty);
                }

                if (companyPolicy.Endorsement.EndorsementType != EndorsementType.Emission && companyPolicy.Endorsement.EndorsementType != EndorsementType.EffectiveExtension)
                {
                    if (companyContracts.All(x => x.Risk.Status == RiskStatusType.NotModified))
                    {
                        throw new ArgumentException(Errors.RiskNotModified);
                    }
                }
                List<EndorsementDTO> endorsementsDTO = new List<EndorsementDTO>();
                endorsementsDTO = DelegateService.underwritingService.GetCompanyEndorsementsByFilterPolicy(companyPolicy.Branch.Id, companyPolicy.Prefix.Id, companyPolicy.DocumentNumber);
                ValidateInfringementPolicies(companyPolicy, companyContracts);
                if (companyPolicy.InfringementPolicies != null && companyPolicy.InfringementPolicies.Count == 0)
                {//
                    companyPolicy.InfringementPolicies = new List<PoliciesAut>();
                    CompanyPolicyDTO companyPolicyDto = new CompanyPolicyDTO { id = companyPolicy.Endorsement.PolicyId, EndorsmentId = companyPolicy.Endorsement.Id, DocumentNumber = companyPolicy.DocumentNumber };
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
                        try
                        {
                            int maxRiskCount = companyPolicy.Summary.RiskCount;
                            int policyId = companyPolicy.Endorsement.PolicyId;
                            int endorsementId = companyPolicy.Endorsement.Id;
                            int endorsementTypeId = (int)companyPolicy.Endorsement.EndorsementType;
                            EndorsementType endorsementType = (EndorsementType)endorsementTypeId;
                            int operationId = companyPolicy.Id;
                            CU.Parallel.For(0, companyContracts.Count, id =>
                            {
                                var companyContract = companyContracts[id];

                                companyContract.Risk.Policy = companyPolicy;
                                if (companyContract.Risk.Status == RiskStatusType.Original ||
                                        companyContract.Risk.Status == RiskStatusType.Included)
                                {
                                    companyContract.Risk.Number = id + 1;
                                    //Interlocked.Increment(ref maxRiskCount);
                                }

                            });
                            ConcurrentBag<string> errors = new ConcurrentBag<string>();
                            if (companyPolicy.Product.IsCollective)
                            {
                                Parallel.For(0, companyContracts.Count, ParallelHelper.DebugParallelFor(), id =>
                                {
                                    try
                                    {
                                        var suretyRisk = companyContracts[id];
                                        CreateRisk(suretyRisk);
                                    }
                                    catch (Exception ex)
                                    {
                                        errors.Add(ex.GetBaseException().Message);
                                    }
                                    finally
                                    {
                                        DataFacadeManager.Dispose();
                                    }

                                });
                            }
                            else
                            {
                                Parallel.For(0, companyContracts.Count, ParallelHelper.DebugParallelFor(), id =>
                                {
                                    try
                                    {
                                        var suretyRisk = companyContracts[id];
                                        CreateRisk(suretyRisk);
                                        List<OperatingQuotaEventDTO> operatingQuotaEvents = new List<OperatingQuotaEventDTO>();
                                        List<RiskConsortiumDTO> riskConsortiums = new List<RiskConsortiumDTO>();
                                        List<CompanyConsortium> coConsortiums = new List<CompanyConsortium>();

                                        foreach (CompanyCoverage item in companyContracts[id].Risk.Coverages)
                                        {
                                            OperatingQuotaEventDTO operatingQuotaEvent = new OperatingQuotaEventDTO();
                                            operatingQuotaEvent.ApplyEndorsement = new ApplyEndorsementDTO();
                                            CompanyInsured insured = new CompanyInsured();

                                            switch (companyPolicy.Endorsement.EndorsementType)
                                            {
                                                case EndorsementType.Emission:
                                                    operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_ENDORSEMENT;
                                                    operatingQuotaEvent.ApplyEndorsement.AmountCoverage = item.EndorsementLimitAmount;
                                                    insured = DelegateService.uniquePersonService.GetCompanyInsuredByIndividualId(companyContracts[id].Contractor.IndividualId);
                                                    coConsortiums = DelegateService.uniquePersonService.GetCoConsortiumsByInsuredCod(insured.InsuredCode);
                                                    break;
                                                case EndorsementType.Modification:
                                                    operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_MODIFY_ENDORSEMENT;
                                                    riskConsortiums = DelegateService.OperationQuotaIntegrationService.GetRiskConsortiumbyPolicy(endorsementsDTO[0].Id);//-endoso anterior

                                                    break;
                                                case EndorsementType.Cancellation:
                                                    operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_CANCELLATION_ENDORSEMENT;
                                                    operatingQuotaEvent.ApplyEndorsement.AmountCoverage = item.EndorsementLimitAmount * -1;
                                                    riskConsortiums = DelegateService.OperationQuotaIntegrationService.GetRiskConsortiumbyPolicy(endorsementsDTO[0].Id);//-endoso anterior

                                                    break;
                                                case EndorsementType.EffectiveExtension:
                                                    operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_EFFECTIVE_EXTENSION_ENDORSEMENT;
                                                    operatingQuotaEvent.ApplyEndorsement.AmountCoverage = item.EndorsementLimitAmount;
                                                    riskConsortiums = DelegateService.OperationQuotaIntegrationService.GetRiskConsortiumbyPolicy(endorsementsDTO[0].Id);//-endoso anterior

                                                    break;
                                                case EndorsementType.Renewal:
                                                    operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_RENEWAL_ENDORSMENT;
                                                    operatingQuotaEvent.ApplyEndorsement.AmountCoverage = item.EndorsementLimitAmount;
                                                    insured = DelegateService.uniquePersonService.GetCompanyInsuredByIndividualId(companyContracts[id].Contractor.IndividualId);
                                                    coConsortiums = DelegateService.uniquePersonService.GetCoConsortiumsByInsuredCod(insured.InsuredCode);
                                                    break;
                                                case EndorsementType.ChangeTermEndorsement:
                                                    operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_CHANGE_TERM_ENDORSEMENT;
                                                    operatingQuotaEvent.ApplyEndorsement.AmountCoverage = item.EndorsementLimitAmount;
                                                    insured = DelegateService.uniquePersonService.GetCompanyInsuredByIndividualId(companyContracts[id].Contractor.IndividualId);
                                                    coConsortiums = DelegateService.uniquePersonService.GetCoConsortiumsByInsuredCod(insured.InsuredCode);
                                                    break;
                                                case EndorsementType.ChangeAgentEndorsement:
                                                    operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_AGENT_CHANGE_ENDORSEMENT;
                                                    operatingQuotaEvent.ApplyEndorsement.AmountCoverage = item.EndorsementLimitAmount;
                                                    insured = DelegateService.uniquePersonService.GetCompanyInsuredByIndividualId(companyContracts[id].Contractor.IndividualId);
                                                    coConsortiums = DelegateService.uniquePersonService.GetCoConsortiumsByInsuredCod(insured.InsuredCode);
                                                    break;
                                                case EndorsementType.ChangeCoinsuranceEndorsement:
                                                    operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_CHANGE_COINSURANCE_ENDORSEMENT;
                                                    operatingQuotaEvent.ApplyEndorsement.AmountCoverage = item.EndorsementLimitAmount;
                                                    insured = DelegateService.uniquePersonService.GetCompanyInsuredByIndividualId(companyContracts[id].Contractor.IndividualId);
                                                    coConsortiums = DelegateService.uniquePersonService.GetCoConsortiumsByInsuredCod(insured.InsuredCode);
                                                    break;
                                                case EndorsementType.ChangePolicyHolderEndorsement:
                                                    operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_POLICY_HOLDER_CHANGE_ENDORSEMENT;
                                                    operatingQuotaEvent.ApplyEndorsement.AmountCoverage = item.EndorsementLimitAmount;
                                                    insured = DelegateService.uniquePersonService.GetCompanyInsuredByIndividualId(companyContracts[id].Contractor.IndividualId);
                                                    coConsortiums = DelegateService.uniquePersonService.GetCoConsortiumsByInsuredCod(insured.InsuredCode);
                                                    break;
                                                case EndorsementType.ChangeConsolidationEndorsement:
                                                    operatingQuotaEvent.OperatingQuotaEventType = (int)EnumEventOperationQuota.APPLY_CONSOLIDATION_CHANGE_ENDORSEMENT;
                                                    operatingQuotaEvent.ApplyEndorsement.AmountCoverage = item.EndorsementLimitAmount;
                                                    insured = DelegateService.uniquePersonService.GetCompanyInsuredByIndividualId(companyContracts[id].Contractor.IndividualId);
                                                    coConsortiums = DelegateService.uniquePersonService.GetCoConsortiumsByInsuredCod(insured.InsuredCode);
                                                    break;
                                                default:

                                                    break;
                                            }
                                            operatingQuotaEvent.IssueDate = companyPolicy.IssueDate;
                                            operatingQuotaEvent.IdentificationId = companyContracts[id].Contractor.IndividualId;
                                            operatingQuotaEvent.LineBusinessID = companyPolicy.Prefix.Id;
                                            operatingQuotaEvent.Policy_Init_Date = companyPolicy.CurrentFrom;
                                            operatingQuotaEvent.Policy_End_Date = companyPolicy.CurrentTo;
                                            operatingQuotaEvent.Cov_Init_Date = item.CurrentFrom;
                                            operatingQuotaEvent.Cov_End_Date = item.CurrentTo;

                                            operatingQuotaEvent.ApplyEndorsement.IndividualId = companyContracts[id].Contractor.IndividualId;
                                            operatingQuotaEvent.ApplyEndorsement.CurrencyType = companyPolicy.ExchangeRate.Currency.Id;
                                            operatingQuotaEvent.ApplyEndorsement.CurrencyTypeDesc = companyPolicy.ExchangeRate.Currency.Description;
                                            operatingQuotaEvent.ApplyEndorsement.CoverageId = item.Id;
                                            operatingQuotaEvent.ApplyEndorsement.IsSeriousOffer = item.IsSeriousOffer;
                                            if (operatingQuotaEvent.OperatingQuotaEventType == (int)EnumEventOperationQuota.APPLY_MODIFY_ENDORSEMENT && companyPolicy.Endorsement.ModificationTypeId == 0 ||
                                            operatingQuotaEvent.OperatingQuotaEventType == (int)EnumEventOperationQuota.APPLY_MODIFY_ENDORSEMENT && companyContracts.Any(x => x.Risk.Premium == 0))
                                            {
                                                operatingQuotaEvent.ApplyEndorsement.AmountCoverage = 0;
                                            }
                                            else
                                            {
                                                operatingQuotaEvent.ApplyEndorsement.AmountCoverage = item.EndorsementLimitAmount;
                                            }
                                            operatingQuotaEvent.ApplyEndorsement.RiskId = suretyRisk.Risk.Id;
                                            operatingQuotaEvent.ApplyEndorsement.Endorsement = companyPolicy.Endorsement.Id;
                                            operatingQuotaEvent.ApplyEndorsement.PolicyID = companyPolicy.Endorsement.PolicyId;
                                            operatingQuotaEvent.ApplyEndorsement.EndorsementType = (int)companyPolicy.Endorsement.EndorsementType;
                                            operatingQuotaEvent.ApplyEndorsement.ParticipationPercentage = 100;
                                            operatingQuotaEvents.Add(operatingQuotaEvent);
                                        }
                                        /*Valido participantes para RiskConsortium*/
                                        coConsortiums = coConsortiums.Where(x => x.Enabled).ToList();
                                        if (coConsortiums.Count > 0)
                                        {
                                            riskConsortiums.Add(new RiskConsortiumDTO { PolicyId = companyPolicy.Endorsement.PolicyId, EndorsementId = companyPolicy.Endorsement.Id, ConsortiumId = companyContracts[id].Contractor.IndividualId, IndividualId = 0, PjePart = 100, RiskId = suretyRisk.Risk.RiskId });

                                            foreach (CompanyConsortium item2 in coConsortiums)
                                            {
                                                riskConsortiums.Add(new RiskConsortiumDTO { PolicyId = companyPolicy.Endorsement.PolicyId, EndorsementId = companyPolicy.Endorsement.Id, ConsortiumId = companyContracts[id].Contractor.IndividualId, IndividualId = item2.IndividualId, PjePart = item2.ParticipationRate, RiskId = suretyRisk.Risk.RiskId });
                                            }
                                        }
                                        else
                                        {
                                            List<RiskConsortiumDTO> riskConsortiumDTO = new List<RiskConsortiumDTO>();
                                            foreach (RiskConsortiumDTO item2 in riskConsortiums)
                                            {
                                                item2.RiskId = suretyRisk.Risk.RiskId;
                                                item2.EndorsementId = companyPolicy.Endorsement.Id;
                                                riskConsortiumDTO.Add(item2);
                                            }
                                            riskConsortiums = riskConsortiumDTO;
                                        }

                                        if (DelegateService.OperationQuotaIntegrationService.InsertConsortium(riskConsortiums))
                                        {
                                            operatingQuotaEvents = DelegateService.OperationQuotaIntegrationService.InsertApplyEndorsementOperatingQuotaEvent(operatingQuotaEvents);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        errors.Add(ex.GetBaseException().Message);
                                    }
                                    finally
                                    {
                                        DataFacadeManager.Dispose();
                                    }
                                });
                            }

                            if (errors != null && errors.Any())
                            {
                                throw new Exception(Errors.ErrorRecordEndorsement);
                            }
                            DelegateService.underwritingService.CreateCompanyPolicyPayer(companyPolicy);
                            try
                            {
                                DelegateService.underwritingService.DeleteTemporalByOperationId(companyPolicy.Id, 0, 0, 0);
                                //DeletePendingOperation(companyPolicy.Id);
                                try
                                {
                                    DelegateService.underwritingService.SaveControlPolicy(policyId, endorsementId, operationId, (int)PolicyOrigin.Individual);

                                    if (DelegateService.commonService.GetParameterByParameterId((int)SuretyKeys.UND_ENABLED_REINSURANCE).BoolParameter.GetValueOrDefault())
                                    {
                                        companyPolicy.IsReinsured = DelegateService.underwritingReinsuranceWorkerIntegration.ReinsuranceIssue(policyId, endorsementId, companyPolicy.UserId) > 0;
                                    }
                                    else
                                    {
                                        //Valida 2g
                                        Thread.Sleep(5);
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
                            try
                            {
                                DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id, companyPolicy.Endorsement.EndorsementType.Value);
                                companyPolicy.Endorsement.PolicyId = companyPolicyDto.id;
                                companyPolicy.Endorsement.Id = companyPolicyDto.EndorsmentId;
                                companyPolicy.DocumentNumber = companyPolicyDto.DocumentNumber;
                            }
                            catch (Exception)
                            {
                                companyPolicy.Endorsement.PolicyId = companyPolicyDto.id;
                                companyPolicy.Endorsement.Id = companyPolicyDto.EndorsmentId;
                                companyPolicy.DocumentNumber = companyPolicyDto.DocumentNumber;
                            }
                            throw;
                        }

                    }
                    else
                    {
                        DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id, companyPolicy.Endorsement.EndorsementType.Value);
                        new Exception("Error creando poliza");
                    }
                }
                return companyPolicy;
            }
            catch (Exception ex)
            {
                // DelegateService.underwritingService.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id, companyPolicy.Endorsement.EndorsementType.Value);
                //throw;
                if (ex?.Message != null && ex.Message == Errors.RiskNotModified)
                {
                    throw new Core.Framework.BAF.BusinessException(ex.Message);
                }
                else
                {
                    throw new Core.Framework.BAF.BusinessException(Errors.ErrorCreatePolicy);
                }
            }

        }

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
        /// Creates the risk.
        /// </summary>
        /// <param name="companySurety">The company surety.</param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ValidationException">
        /// </exception>
        public void CreateRisk(CompanyContract companySurety)
        {
            try
            {
                if (companySurety == null)
                {
                    throw new Exception(Errors.ErrorParameterContratEmpty);
                }
                IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();

                NameValue[] parameters = new NameValue[60];
                parameters[0] = new NameValue("@ENDORSEMENT_ID", companySurety.Risk.Policy.Endorsement.Id);
                parameters[1] = new NameValue("@POLICY_ID", companySurety.Risk.Policy.Endorsement.PolicyId);
                parameters[2] = new NameValue("@PAYER_ID", companySurety.Risk.Policy.Holder.IndividualId);
                parameters[3] = new NameValue("@INSURED_ID", companySurety.Risk.MainInsured.IndividualId);
                parameters[4] = new NameValue("@COVERED_RISK_TYPE_CD", (int)companySurety.Risk.CoveredRiskType);
                parameters[5] = new NameValue("@RISK_STATUS_CD", (int)companySurety.Risk.Status);
                if (companySurety.Risk.Text == null)
                {
                    parameters[6] = new NameValue("@CONDITION_TEXT", "--", DbType.String);
                }
                else
                {
                    parameters[6] = new NameValue("@CONDITION_TEXT", companySurety.Risk.Text.TextBody);
                }
                parameters[7] = new NameValue("@RATING_ZONE_CD", DBNull.Value, DbType.Int32);
                parameters[8] = new NameValue("@COVER_GROUP_ID", companySurety.Risk.GroupCoverage.Id);
                parameters[9] = new NameValue("@IS_FACULTATIVE", companySurety.Isfacultative);
                if (companySurety.Risk.MainInsured.CompanyName != null && companySurety.Risk.MainInsured.CompanyName.NameNum > 0)
                {
                    parameters[10] = new NameValue("@NAME_NUM", companySurety.Risk.MainInsured.CompanyName.NameNum);
                }
                else
                {
                    parameters[10] = new NameValue("@NAME_NUM", DBNull.Value, DbType.Int32);
                }
                parameters[11] = new NameValue("@LIMITS_RC_CD", DBNull.Value, DbType.Int32);
                parameters[12] = new NameValue("@LIMIT_RC_SUM", 0, DbType.Int32);
                parameters[13] = new NameValue("@COMM_RISK_CLASS_CD", DBNull.Value, DbType.Int32);
                parameters[14] = new NameValue("@RISK_COMMERCIAL_TYPE_CD", DBNull.Value, DbType.Int32);
                if (companySurety.Risk.SecondInsured != null && companySurety.Risk.SecondInsured.IndividualId > 0)
                {
                    parameters[15] = new NameValue("@SECONDARY_INSURED_ID", companySurety.Risk.SecondInsured.IndividualId);
                }
                else
                {
                    parameters[15] = new NameValue("@SECONDARY_INSURED_ID", DBNull.Value, DbType.Int32);
                }
                parameters[16] = new NameValue("@ACTUAL_DATE", DateTime.Now);
                DataTable dtBeneficiaries = new DataTable("PARAM_TEMP_RISK_BENEFICIARY");
                dtBeneficiaries.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
                dtBeneficiaries.Columns.Add("BENEFICIARY_ID", typeof(int));
                dtBeneficiaries.Columns.Add("BENEFICIARY_TYPE_CD", typeof(int));
                dtBeneficiaries.Columns.Add("BENEFICT_PCT", typeof(decimal));
                dtBeneficiaries.Columns.Add("NAME_NUM", typeof(int));

                foreach (CompanyBeneficiary item in companySurety.Risk.Beneficiaries)
                {
                    DataRow dataRow = dtBeneficiaries.NewRow();
                    dataRow["CUSTOMER_TYPE_CD"] = item.CustomerType;
                    dataRow["BENEFICIARY_ID"] = item.IndividualId;
                    dataRow["BENEFICIARY_TYPE_CD"] = item.BeneficiaryType?.Id ?? 1;
                    dataRow["BENEFICT_PCT"] = item.Participation;

                    if (item.CustomerType == CustomerType.Individual && item.CompanyName != null && item.CompanyName.NameNum == 0)
                    {
                        if (item.IndividualId == companySurety.Risk.MainInsured.IndividualId)
                        {
                            item.CompanyName = companySurety.Risk.MainInsured.CompanyName;
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
                                    Description = item.CompanyName.Email != null ? item.CompanyName.Email.Description : null
                                }
                            };
                        }
                    }
                    if (item.CompanyName != null && item.CompanyName.NameNum > 0)
                    {
                        if (item.IndividualId == companySurety.Risk.MainInsured.IndividualId)
                        {
                            dataRow["NAME_NUM"] = companySurety.Risk.MainInsured.CompanyName.NameNum;
                        }
                        else
                        {
                            dataRow["NAME_NUM"] = item.CompanyName.NameNum;
                        }
                    }

                    dtBeneficiaries.Rows.Add(dataRow);
                }

                parameters[17] = new NameValue("@INSERT_TEMP_RISK_BENEFICIARY", dtBeneficiaries);
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
                //dtCoverages.Columns.Add("DIFF_MIN_PREMIUM_AMT", typeof(decimal));
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

                DataTable dtCoverageClauses = new DataTable("PARAM_TEMP_RISK_COVER_CLAUSE");
                dtCoverageClauses.Columns.Add("COVERAGE_ID", typeof(int));
                dtCoverageClauses.Columns.Add("CLAUSE_ID", typeof(int));
                dtCoverageClauses.Columns.Add("CLAUSE_STATUS_CD", typeof(int));
                dtCoverageClauses.Columns.Add("CLAUSE_ORIG_STATUS_CD", typeof(int));

                companySurety.Risk.Coverages = companySurety.Risk.Coverages.OrderBy(x => x.Id).ToList();
                foreach (CompanyCoverage item in companySurety.Risk.Coverages)
                {
                    DataRow dataRow = dtCoverages.NewRow();
                    dataRow["COVERAGE_ID"] = item.Id;
                    dataRow["IS_DECLARATIVE"] = item.IsDeclarative;
                    dataRow["IS_MIN_PREMIUM_DEPOSIT"] = item.IsMinPremiumDeposit;
                    dataRow["FIRST_RISK_TYPE_CD"] = (int)Sistran.Core.Application.UnderwritingServices.Enums.FirstRiskType.None;
                    dataRow["CALCULATION_TYPE_CD"] = item.CalculationType.Value;
                    dataRow["DECLARED_AMT"] = Math.Round(item.DeclaredAmount, 2);
                    dataRow["PREMIUM_AMT"] = Math.Round(item.PremiumAmount, 2);
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
                    dataRow["RISK_COVER_ID"] = item.Id;
                    dataRow["COVER_STATUS_CD"] = item.CoverStatus.Value;
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
                    //dataRow["DIFF_MIN_PREMIUM_AMT"] = 0;

                    dataRow["ENDORSEMENT_LIMIT_AMT"] = item.EndorsementLimitAmount;
                    dataRow["ENDORSEMENT_SUBLIMIT_AMT"] = item.EndorsementSublimitAmount;
                    dataRow["FLAT_RATE_PCT"] = item.FlatRatePorcentage;
                    dataRow["CONTRACT_AMOUNT_PCT"] = item.ContractAmountPercentage;


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
                    else
                    {
                        dataRow["DYNAMIC_PROPERTIES"] = DBNull.Value;
                    }

                    dataRow["SHORT_TERM_PCT"] = item.ShortTermPercentage;
                    dataRow["PREMIUM_AMT_DEPOSIT_PERCENT"] = 0;
                    dataRow["MAX_LIABILITY_AMT"] = item.MaxLiabilityAmount;
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
                parameters[18] = new NameValue("@INSERT_TEMP_RISK_COVERAGE", dtCoverages);
                parameters[19] = new NameValue("@INSERT_TEMP_RISK_COVER_DEDUCT", dtDeductibles);

                DataTable dtClauses = new DataTable("PARAM_TEMP_CLAUSE");
                dtClauses.Columns.Add("CLAUSE_ID", typeof(int));
                dtClauses.Columns.Add("ENDORSEMENT_ID", typeof(int));
                dtClauses.Columns.Add("CLAUSE_STATUS_CD", typeof(int));
                dtClauses.Columns.Add("CLAUSE_ORIG_STATUS_CD", typeof(int));

                if (companySurety.Risk.Clauses != null)
                {
                    foreach (CompanyClause item in companySurety.Risk.Clauses)
                    {
                        DataRow dataRow = dtClauses.NewRow();
                        dataRow["CLAUSE_ID"] = item.Id;
                        dataRow["CLAUSE_STATUS_CD"] = (int)Sistran.Core.Application.CommonService.Enums.ClauseStatuses.Original;
                        dtClauses.Rows.Add(dataRow);
                    }
                }

                parameters[20] = new NameValue("@INSERT_TEMP_CLAUSE", dtClauses);
                if (companySurety.Risk.DynamicProperties != null && companySurety.Risk.DynamicProperties.Count > 0)
                {
                    DynamicPropertiesCollection dynamicCollectionRisk = new DynamicPropertiesCollection();
                    for (int i = 0; i < companySurety.Risk.DynamicProperties.Count(); i++)
                    {
                        DynamicProperty dinamycProperty = new DynamicProperty();
                        dinamycProperty.Id = companySurety.Risk.DynamicProperties[i].Id;
                        dinamycProperty.Value = companySurety.Risk.DynamicProperties[i].Value;
                        dynamicCollectionRisk[i] = dinamycProperty;
                    }
                    byte[] serializedValuesRisk = dynamicPropertiesSerializer.Serialize(dynamicCollectionRisk);
                    parameters[21] = new NameValue("@DYNAMIC_PROPERTIES", serializedValuesRisk);
                }
                else
                {
                    parameters[21] = new NameValue("@DYNAMIC_PROPERTIES", DBNull.Value, DbType.Binary);
                }
                parameters[22] = new NameValue("@INSPECTION_ID", DBNull.Value, DbType.Int32);

                DataTable dtDynamicProperties = new DataTable("PARAM_TEMP_DYNAMIC_PROPERTIES");
                dtDynamicProperties.Columns.Add("DYNAMIC_ID", typeof(int));
                dtDynamicProperties.Columns.Add("CONCEPT_VALUE", typeof(string));

                if (companySurety.Risk.DynamicProperties != null)
                {
                    foreach (DynamicConcept item in companySurety.Risk.DynamicProperties)
                    {
                        DataRow dataRow = dtDynamicProperties.NewRow();
                        dataRow["DYNAMIC_ID"] = item.Id;
                        dataRow["CONCEPT_VALUE"] = item.Value ?? "NO ASIGNADO";
                        dtDynamicProperties.Rows.Add(dataRow);
                    }
                }

                parameters[23] = new NameValue("@INSERT_TEMP_DYNAMIC_PROPERTIES", dtDynamicProperties);


                DataTable dtDynamicPropertiesCoverage = new DataTable("PARAM_TEMP_DYNAMIC_PROPERTIES");
                dtDynamicPropertiesCoverage.Columns.Add("DYNAMIC_ID", typeof(int));
                dtDynamicPropertiesCoverage.Columns.Add("CONCEPT_VALUE", typeof(string));
                parameters[24] = new NameValue("@INSERT_TEMP_DYNAMIC_PROPERTIES_COVERAGE", dtDynamicPropertiesCoverage);

                parameters[25] = new NameValue("@RISK_NUM", companySurety.Risk.Number);
                parameters[26] = new NameValue("@INDIVIDUAL_ID", companySurety.Risk.MainInsured.IndividualId);
                parameters[27] = new NameValue("@SURETY_CONTRACT_TYPE_CD", companySurety.ContractType.Id);
                parameters[28] = new NameValue("@SURETY_CONTRACT_CATEGORIES_CD", companySurety.Class.Id);
                if (companySurety.SettledNumber != null)
                {
                    parameters[29] = new NameValue("@BID_NUMBER", companySurety.SettledNumber);
                }
                else
                {
                    parameters[29] = new NameValue("@BID_NUMBER", DBNull.Value, DbType.Int32);
                }
                if (companySurety.Contractor != null && companySurety.Contractor.CompanyName.Address.Description == null)
                {
                    parameters[30] = new NameValue("@CONTRACT_ADDRESS", DBNull.Value, DbType.String);
                }
                else
                {
                    parameters[30] = new NameValue("@CONTRACT_ADDRESS", companySurety.Contractor.CompanyName.Address.Description);
                }
                parameters[31] = new NameValue("@CONTRACT_AMT", companySurety.Value.Value);
                parameters[32] = new NameValue("@RISK_INSP_TYPE_CD", 1);
                parameters[33] = new NameValue("@OPERATION", JsonConvert.SerializeObject(companySurety));
                if (companySurety.ContractObject != null && companySurety.ContractObject.TextBody != null)
                {
                    parameters[34] = new NameValue("@OBJECT_CONTRACT", companySurety.ContractObject.TextBody);
                }
                else
                {
                    parameters[34] = new NameValue("@OBJECT_CONTRACT", DBNull.Value, DbType.String);

                }

                //author: Germán F. Grimaldi
                //date: 02/10/2018
                //Old
                /*
                 *if (companySurety.Guarantees != null && companySurety.Guarantees.Count > 0)
                    {
                        parameters[35] = new NameValue("@GUARANTEE_ID", companySurety.Guarantees[0].Id);
                    }
                    else
                    {
                        parameters[35] = new NameValue("@GUARANTEE_ID", DBNull.Value);
                    } 
                 */
                //End_Old
                /*Cuando viene más de una garantía debe de poderse validar e insertar todas las contra-garantías */
                {
                    DataTable dtGuarantees = new DataTable("INSERT_TEMP_CROSS_GUARANTEES");

                    dtGuarantees.Columns.Add("RISK_ID", typeof(int));
                    dtGuarantees.Columns.Add("GUARANTEE_ID", typeof(int));

                    if (companySurety.Guarantees != null && companySurety.Guarantees.Count > 0)
                    {
                        foreach (var guarantee_item in companySurety.Guarantees)
                        {
                            DataRow dataRowGuarantee = dtGuarantees.NewRow();
                            dataRowGuarantee["RISK_ID"] = companySurety.Risk.RiskId;
                            dataRowGuarantee["GUARANTEE_ID"] = guarantee_item.InsuredGuarantee.Id;
                            dtGuarantees.Rows.Add(dataRowGuarantee);
                        }
                    }
                    //else
                    //{
                    //    parameters[35] = new NameValue("@INSERT_TEMP_CROSS_GUARANTEES", DBNull.Value);
                    //}
                    parameters[35] = new NameValue("@INSERT_TEMP_CROSS_GUARANTEES", dtGuarantees);
                    //End
                }


                parameters[36] = new NameValue("@PROFESSIONAL_CARD_NUM", DBNull.Value, DbType.String);
                parameters[37] = new NameValue("@INSURED_CAUTION", DBNull.Value, DbType.String);
                parameters[38] = new NameValue("@COURT_NUM", DBNull.Value, DbType.String);
                parameters[39] = new NameValue("@ID_CARD_TYPE_CD", DBNull.Value, DbType.Int32);
                parameters[40] = new NameValue("@ID_CARD_NO", DBNull.Value, DbType.String);
                parameters[41] = new NameValue("@ARTICLE_CD", DBNull.Value, DbType.Int32);
                parameters[42] = new NameValue("@COURT_CD", DBNull.Value, DbType.Int32);
                parameters[43] = new NameValue("@INSURED_CAPACITY_OF_CD", DBNull.Value, DbType.Int32);

                if (companySurety.State?.Id > 0)
                {
                    parameters[44] = new NameValue("@STATE_CD", companySurety.State.Id);
                }
                else
                {
                    parameters[44] = new NameValue("@STATE_CD", DBNull.Value, DbType.Int32);
                }
                if (companySurety.City?.Id > 0)
                {
                    parameters[45] = new NameValue("@CITY_CD", companySurety.City.Id);
                }
                else
                {
                    parameters[45] = new NameValue("@CITY_CD", DBNull.Value, DbType.Int32);
                }

                if (companySurety.Country?.Id > 0)
                {
                    parameters[46] = new NameValue("@COUNTRY_CD", companySurety.Country.Id);
                }
                else
                {
                    parameters[46] = new NameValue("@COUNTRY_CD", DBNull.Value, DbType.Int32);
                }

                parameters[47] = new NameValue("@CONTRACTOR_ID", companySurety.Contractor.IndividualId);

                if (companySurety.RiskSuretyPost != null && companySurety.RiskSuretyPost.TempId > 0 && companySurety.RiskSuretyPost.UserId > 0)
                {
                    parameters[48] = new NameValue("@TEMP_ID", companySurety.RiskSuretyPost.TempId);
                    parameters[49] = new NameValue("@USER_ID", companySurety.RiskSuretyPost.UserId);
                    if (companySurety.RiskSuretyPost.ChkContractDate == true)
                    {
                        parameters[50] = new NameValue("@CONTRACT_DATE", companySurety.RiskSuretyPost.ContractDate);
                        parameters[51] = new NameValue("@DELEVERY_DATE", DBNull.Value, DbType.DateTime);

                    }
                    else
                    {
                        parameters[50] = new NameValue("@CONTRACT_DATE", DBNull.Value, DbType.DateTime);
                        parameters[51] = new NameValue("@DELEVERY_DATE", companySurety.RiskSuretyPost.IssueDate);

                    }
                }
                else if (companySurety.Risk.Policy.Endorsement != null)
                {
                    parameters[48] = new NameValue("@TEMP_ID", companySurety.Risk.Policy.Endorsement.TemporalId);
                    parameters[49] = new NameValue("@USER_ID", companySurety.Risk.Policy.Endorsement.UserId);

                    parameters[50] = new NameValue("@CONTRACT_DATE", DBNull.Value, DbType.DateTime);
                    parameters[51] = new NameValue("@DELEVERY_DATE", DBNull.Value, DbType.DateTime);
                }
                else
                {
                    parameters[48] = new NameValue("@TEMP_ID", DBNull.Value, DbType.Int32);
                    parameters[49] = new NameValue("@USER_ID", DBNull.Value, DbType.Int32);

                    parameters[50] = new NameValue("@CONTRACT_DATE", DBNull.Value, DbType.DateTime);
                    parameters[51] = new NameValue("@DELEVERY_DATE", DBNull.Value, DbType.DateTime);
                }


                DataTable dtCoverangePost = new DataTable("PARAM_TEMP_RISK_POSTCONTRACTUAL");
                dtCoverangePost.Columns.Add("COVERAGE_ID", typeof(int));
                dtCoverangePost.Columns.Add("RISK_COVER_ID", typeof(int));
                dtCoverangePost.Columns.Add("CURRENT_FROM", typeof(DateTime));
                dtCoverangePost.Columns.Add("CURRENT_TO", typeof(DateTime));
                foreach (CompanyCoverage item in companySurety.Risk.Coverages)
                {
                    if (item.IsPostcontractual)
                    {
                        DataRow dataRow = dtCoverangePost.NewRow();
                        dataRow["COVERAGE_ID"] = item.Id;
                        dataRow["RISK_COVER_ID"] = item.Id;
                        dataRow["CURRENT_FROM"] = item.CurrentFrom;
                        dataRow["CURRENT_TO"] = item.CurrentTo;
                        dtCoverangePost.Rows.Add(dataRow);
                    }

                }
                parameters[52] = new NameValue("@INSERT_TEMP_COVERANGES_POST", dtCoverangePost);
                parameters[53] = new NameValue("@IS_RETENTION", companySurety.IsRetention);
                parameters[54] = new NameValue("@PILE_AMT", Math.Truncate(companySurety.Aggregate));
                parameters[55] = new NameValue("@IS_NATIONAL", companySurety.Risk.IsNational);
                parameters[56] = new NameValue("@ADDRESS_ID", companySurety.Risk.MainInsured.CompanyName.Address.Id);
                parameters[57] = new NameValue("@PHONE_ID", companySurety.Risk.MainInsured.CompanyName.Phone.Id);
                parameters[58] = new NameValue("@IS_POST_CONTRACTUAL", companySurety.Risk.Coverages.Any(x => x.IsPostcontractual));
                parameters[59] = new NameValue("@INSERT_TEMP_RISK_COVER_CLAUSE", dtCoverageClauses);
                DataTable result;

                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataTable("ISS.RECORD_RISK_SURETY", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    if (!Convert.ToBoolean(result.Rows[0][0]))
                    {
                        throw new ValidationException((string)result.Rows[0][1]);
                    }
                    else
                    {
                        companySurety.Risk.RiskId = Convert.ToInt32(result.Rows[0][1]);
                    }
                }
                else
                {
                    throw new ValidationException(Errors.ErrorRecordEndorsement);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Listado de Coverturas con Post Contractual
        /// </summary>
        /// <returns></returns>
        private System.Collections.ArrayList GET_PRV_COVERAGE_POSTCONTRACTUAL()
        {
            System.Collections.ArrayList CoversId;
            DataTable result;

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("QUO.GET_PRV_COVERAGE_POSTCONTRACTUAL", null);
            }

            CoversId = new System.Collections.ArrayList(result.Rows.Count);

            if (result != null && result.Rows.Count > 0)
            {

                for (int i = 0; i < result.Rows.Count; i++)
                {
                    CoversId.Add((int)result.Rows[i][0]);
                }
            }
            return CoversId;
        }

        /// <summary>
        /// Validates the infringement policies.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <param name="companyContracts">The company contracts.</param>
        private void ValidateInfringementPolicies(CompanyPolicy companyPolicy, List<CompanyContract> companyContracts)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();

            infringementPolicies.AddRange(companyPolicy.InfringementPolicies);
            companyContracts.AsParallel().ForAll(x => infringementPolicies.AddRange(x.Risk.InfringementPolicies));

            companyPolicy.InfringementPolicies = DelegateService.AuthorizationPoliciesServiceCore.ValidateInfringementPolicies(infringementPolicies);
        }

        public CompanyRiskSuretyPost GetRiskSuretyPost(int riskId)
        {
            BusinessCollection<ENT.PrvRiskCoveragePost> businessCollection = new BusinessCollection<ENT.PrvRiskCoveragePost>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ENT.PrvRiskCoveragePost.Properties.RiskId, typeof(ENT.PrvRiskCoveragePost).Name);
            filter.Equal();
            filter.Constant(riskId);

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = daf.List<ENT.PrvRiskCoveragePost>(filter.GetPredicate());

            }

            if (businessCollection.Count > 0)
            {
                return ModelAssembler.CreateRiskPostS(businessCollection[0]);
            }

            return null;


        }

        #region IsRetention
        public Boolean GetIsRetention(int riskId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.CoRiskSurety.Properties.RiskId, typeof(ISSEN.CoRiskSurety).Name);
            filter.Equal();
            filter.Constant(riskId);

            BusinessCollection<ISSEN.CoRiskSurety> businessCollection = new BusinessCollection<ISSEN.CoRiskSurety>();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = daf.List<ISSEN.CoRiskSurety>(filter.GetPredicate());
            }

            if (businessCollection.Count > 0)
            {
                return businessCollection[0].IsRetention;
            }

            return false;

        }
        #endregion

        #region Guardar Temporales

        public CompanyContract SaveCompanyContractTemporalTables(CompanyContract companyContract)
        {


            IDynamicPropertiesSerializer dynamicPropertiesSerializer =
            new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();
            UTILITES.GetDatatables d = new UTILITES.GetDatatables();

            UTILITES.CommonDataTables dts = d.GetcommonDataTables(companyContract.Risk);

            DataTable dataTable;
            NameValue[] parameters = new NameValue[22];

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

            DataTable dtRiskCoverage = dts.dtRiskCoverage;
            parameters[5] = new NameValue(dtRiskCoverage.TableName, dtRiskCoverage);

            DataTable dtDeduct = dts.dtDeduct;
            parameters[6] = new NameValue(dtDeduct.TableName, dtDeduct);

            DataTable dtCoverClause = dts.dtCoverClause;
            parameters[7] = new NameValue(dtCoverClause.TableName, dtCoverClause);

            DataTable dtDynamic = dts.dtDynamic;
            parameters[8] = new NameValue("INSERT_TEMP_DYNAMIC_PROPERTIES_RISK", dtDynamic);

            DataTable dtDynamicCoverage = dts.dtDynamicCoverage;
            parameters[9] = new NameValue("INSERT_TEMP_DYNAMIC_PROPERTIES_COVERAGE", dtDynamicCoverage);

            DataTable dtTempRiskSurety = ModelAssembler.GetDataTableTempRISKSurety(companyContract);
            parameters[10] = new NameValue(dtTempRiskSurety.TableName, dtTempRiskSurety);

            DataTable dtTempRiskSuretyAvailable = ModelAssembler.GetDataTableTempSuretyAvailable(companyContract);
            parameters[11] = new NameValue(dtTempRiskSuretyAvailable.TableName, dtTempRiskSuretyAvailable);

            DataTable dtTempRiskSuretyContract = ModelAssembler.GetDataTableTempSuretyContract(companyContract);
            parameters[12] = new NameValue(dtTempRiskSuretyContract.TableName, dtTempRiskSuretyContract);

            DataTable dtTempRiskSuretyCGuarantee = ModelAssembler.GetDataTableTempSuretyGuarantee(companyContract);
            parameters[13] = new NameValue(dtTempRiskSuretyCGuarantee.TableName, dtTempRiskSuretyCGuarantee);

            DataTable dtTempRiskSuretyPost = ModelAssembler.GetDataTableTempSuretyPost(companyContract);
            parameters[14] = new NameValue(dtTempRiskSuretyPost.TableName, dtTempRiskSuretyPost);

            //if (companyContract.Risk.Policy.TemporalType != TemporalType.Quotation && companyContract.Risk.Policy.TemporalType != TemporalType.TempQuotation && dtTempRiskSuretyCGuarantee.Rows.Count == 0)
            //{
            //    throw new ValidationException(Errors.NoDataGuarantee);
            //}

            if (companyContract.Risk.MainInsured.IndividualId > 0)
            {
                parameters[15] = new NameValue("@INSURED_ID", companyContract.Risk.MainInsured.IndividualId);
            }
            else
            {
                parameters[15] = new NameValue("@INSURED_ID", DBNull.Value, DbType.Int32);
            }

            if (companyContract.Risk.MainInsured.CompanyName.Address.Id > 0)
            {
                parameters[16] = new NameValue("@ADDRESS_ID", companyContract.Risk.MainInsured.CompanyName.Address.Id);
            }
            else
            {
                parameters[16] = new NameValue("@ADDRESS_ID", DBNull.Value, DbType.Int32);
            }

            if (companyContract.Risk.MainInsured.CompanyName.Phone.Id > 0)
            {
                parameters[17] = new NameValue("@PHONE_ID", companyContract.Risk.MainInsured.CompanyName.Phone.Id);
            }
            else
            {
                parameters[17] = new NameValue("@PHONE_ID", DBNull.Value, DbType.Int32);
            }
            if (companyContract.Country?.Id > 0)
            {
                parameters[18] = new NameValue("@COUNTRY_CD", companyContract.Country.Id);
            }
            else
            {
                parameters[18] = new NameValue("@COUNTRY_CD", DBNull.Value, DbType.Int32);
            }

            if (companyContract.State?.Id > 0)
            {
                parameters[19] = new NameValue("@STATE_CD", companyContract.State.Id);
            }
            else
            {
                parameters[19] = new NameValue("@STATE_CD", DBNull.Value, DbType.Int32);
            }

            if (companyContract.City?.Id > 0)
            {
                parameters[20] = new NameValue("@CITY_CD", companyContract.City.Id);
            }
            else
            {
                parameters[20] = new NameValue("@CITY_CD", DBNull.Value, DbType.Int32);
            }

            parameters[21] = new NameValue("@IS_NATIONAL", companyContract.Risk.IsNational);

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {

                dataTable = pdb.ExecuteSPDataTable("TMP.CIA_SAVE_TEMPORAL_RISK_SURETY_TEMP", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (companyContract.Risk.Policy.Endorsement.EndorsementType != EndorsementType.Modification)
                {
                    companyContract.Risk.RiskId = Convert.ToInt32(dataTable.Rows[0][0]);
                }
                return companyContract;
            }
            else
            {
                throw new ValidationException(Errors.ErrorRecordEndorsement);
                //throw new ValidationException(Errors.ErrorCreateTemporalCompanyContract);//ErrrRecordTemporal "error al grabar riesgo
            }
        }
        #endregion

        #region DatosAfianzado
        public bool GetDataTechnicalCard(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PER.TechnicalCard.Properties.IndividualId, typeof(PER.TechnicalCard).Name);
            filter.Equal();
            filter.Constant(individualId);
            var result = DataFacadeManager.Instance.GetDataFacade().List<PER.TechnicalCard>(filter.GetPredicate());
            DataFacadeManager.Dispose();
            return result.Count > 0;
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

        public bool GetAssociationType(int associationType)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PER.CoAssociationType.Properties.AssociationTypeCode, typeof(PER.CoAssociationType).Name);
            filter.Equal();
            filter.Constant(associationType);
            var result = DataFacadeManager.Instance.GetDataFacade().List<PER.CoAssociationType>(filter.GetPredicate());
            DataFacadeManager.Dispose();
            if (result.Count > 0)
            {
                return result[0].IsConsortium;
            }
            return false;
        }

        public int GetCountSinister(int individualId)
        {
            int recordCount = 0;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            SelectQuery select = new SelectQuery();
            Join join = new Join(new ClassNameTable(typeof(PER.Insured), "a"), new ClassNameTable(typeof(TEM.FianzaPolicyClaims), "b"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(PER.Insured.Properties.InsuredCode, "a")
                    .Equal()
                    .Property(TEM.FianzaPolicyClaims.Properties.InsuredCode, "b")
                    .GetPredicate());
            filter.Property(PER.Insured.Properties.IndividualId, "a");
            filter.Equal();
            filter.Constant(individualId);
            select.Table = join;
            select.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    recordCount++;
                }
            }
            return recordCount;
        }
        /// <summary>
        /// Tiene carter pendiente
        /// </summary>
        /// <param name="policyNum"></param>
        /// <returns></returns>
        public decimal GetHasPortfolioBalance(CompanyContract companyContract)
        {
            decimal ValorPortafolio = 0;
            if (companyContract.Risk.Policy.DocumentNumber != 0)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(TEM.CoBorrowingPolicy.Properties.PolicyNum, typeof(TEM.CoBorrowingPolicy).Name);
                filter.Equal();
                filter.Constant(companyContract.Risk.Policy.DocumentNumber);
                filter.And();
                filter.Property(TEM.CoBorrowingPolicy.Properties.PolicyNum, typeof(TEM.CoBorrowingPolicy).Name);
                filter.Equal();
                filter.Constant(true);
                filter.And();
                filter.Property(TEM.CoBorrowingPolicy.Properties.BranchCode, typeof(TEM.CoBorrowingPolicy).Name);
                filter.Equal();
                filter.Constant(companyContract.Risk.Policy.Branch.Id);
                filter.And();
                filter.Property(TEM.CoBorrowingPolicy.Properties.PrefixCode, typeof(TEM.CoBorrowingPolicy).Name);
                filter.Equal();
                filter.Constant(companyContract.Risk.Policy.Prefix.Id);
                var result = DataFacadeManager.Instance.GetDataFacade().List<TEM.CoBorrowingPolicy>(filter.GetPredicate());
                DataFacadeManager.Dispose();
                if (result.Count > 0)
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        ValorPortafolio += result[i].PremiumAmount;
                    }

                }
            }
            return ValorPortafolio;
        }
        #endregion

        public bool GetInsuredGuaranteeRelationPolicy(int guaranteeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.RiskSuretyGuarantee.Properties.GuaranteeId, typeof(ISSEN.RiskSuretyGuarantee).Name);
            filter.Equal();
            filter.Constant(guaranteeId);
            filter.Or();
            filter.Property(ISSEN.RiskJudicialSuretyGuarantee.Properties.GuaranteeId, typeof(ISSEN.RiskJudicialSuretyGuarantee).Name);
            filter.Equal();
            filter.Constant(guaranteeId);

            RiskSuretyGuaranteeSearchView view = new RiskSuretyGuaranteeSearchView();
            ViewBuilder builder = new ViewBuilder("RiskSuretyGuaranteeSearchView") { Filter = filter.GetPredicate() };
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


        //#region Consulta de Poliza por Id
        //public List<CompanyContract> GetCompanyContractByPolicyId(int policyId)
        //{
        //    try
        //    {
        //        List<CompanyContract> companyContracts = new List<CompanyContract>();

        //        ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //        filter.Property(ISSEN.EndorsementRisk.Properties.PolicyId, typeof(ISSEN.EndorsementRisk).Name);
        //        filter.Equal();
        //        filter.Constant(policyId);
        //        filter.And();
        //        filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
        //        filter.Equal();
        //        filter.Constant(true);
        //        filter.And();
        //        filter.Not();
        //        filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
        //        filter.In();
        //        filter.ListValue();
        //        filter.Constant((int)RiskStatusType.Excluded);
        //        filter.Constant((int)RiskStatusType.Cancelled);
        //        filter.EndList();

        //        return GetCompanyContractByFilter(filter);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.GetBaseException().Message, ex);
        //    }

        //}

        //public List<CompanyContract> GetCompanyContractByFilter(ObjectCriteriaBuilder filter)
        //{
        //    List<CompanyContract> CompanyContract = new List<CompanyContract>();
        //    RiskSuretyView view = new RiskSuretyView();
        //    ViewBuilder builder = new ViewBuilder("RiskSuretyView")
        //    {
        //        Filter = filter.GetPredicate()
        //    };

        //    using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
        //    {
        //        daf.FillView(builder, view);
        //    }


        //    if (view.RiskSureties.Count == 0)
        //    {
        //        throw new ArgumentException(Errors.ErrorRiskEmpty);
        //    }
        //    List<ISSEN.Risk> risks = view.Risks.Cast<ISSEN.Risk>().ToList();
        //    List<ISSEN.EndorsementRisk> endorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
        //    List<ISSEN.CoRisk> coRisks = view.CoRisks.Cast<ISSEN.CoRisk>().ToList();
        //    List<ISSEN.RiskSurety> riskVehicles = view.RiskSureties.Cast<ISSEN.RiskSurety>().ToList();
        //    List<ISSEN.CoRiskSurety> coRiskVehicles = view.CoRiskSurety.Cast<ISSEN.CoRiskSurety>().ToList();
        //    List<ISSEN.RiskBeneficiary> riskBeneficiaries = view.RiskBeneficiaries.Cast<ISSEN.RiskBeneficiary>().ToList();
        //    //List<COMMEN.VehicleModel> vehicleModels = view.VehicleModels.Cast<COMMEN.VehicleModel>().ToList();
        //    //List<COMMEN.VehicleMake> vehicleMakes = view.VehicleMakes.Cast<COMMEN.VehicleMake>().ToList();
        //    //List<COMMEN.VehicleVersion> vehicleVersions = view.VehicleVersions.Cast<COMMEN.VehicleVersion>().ToList();
        //    //List<ISSEN.RiskClause> riskClauses = view.RiskClause.Cast<ISSEN.RiskClause>().ToList();

        //    List<LimitRc> limitsRc = DelegateService.underwritingService.GetLimitsRc();

        //    ConcurrentBag<CompanyContract> vehicles = new ConcurrentBag<CompanyContract>();
        //    ParallelHelper.ForEach(risks, item =>
        //    {
        //        try
        //        {
        //            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
        //            {
        //                daf.LoadDynamicProperties(item);
        //            }
        //            CompanyContract vehicle = new CompanyContract();

        //            vehicle = ModelAssembler.CreateVehicle(item,
        //                coRisks.First(x => x.RiskId == item.RiskId),
        //                riskVehicles.First(x => x.RiskId == item.RiskId),
        //                coRiskVehicles.First(x => x.RiskId == item.RiskId),
        //                endorsementRisks.First(x => x.RiskId == item.RiskId));

        //            if (vehicle.Risk.LimitRc != null)
        //            {
        //                vehicle.Risk.LimitRc.Description = limitsRc.First(l => l.Id == vehicle.Risk.LimitRc.Id).Description;
        //            }

        //            if (vehicle.Risk.Text != null)
        //            {
        //                vehicle.Risk.Text.TextBody = item.ConditionText;
        //            }
        //            else
        //            {
        //                vehicle.Risk.Text = new CompanyText()
        //                {
        //                    TextBody = item.ConditionText
        //                };
        //            }

        //            //vehicle.Make.Description = vehicleMakes.FirstOrDefault(x => x.VehicleMakeCode == vehicle.Make.Id).SmallDescription;
        //            //vehicle.Model.Description = vehicleModels.FirstOrDefault(x => x.VehicleModelCode == vehicle.Model.Id).SmallDescription;
        //            //vehicle.Version.Description = vehicleVersions.FirstOrDefault(x => x.VehicleVersionCode == vehicle.Version.Id).Description;

        //            CompanyIssuanceInsured companyInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(vehicle.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);
        //            if (companyInsured == null || companyInsured.IndividualId < 1)
        //            {
        //                throw new Exception(Errors.ErrorInsuredMain);
        //            }

        //            vehicle.Risk.MainInsured = companyInsured;
        //            vehicle.Risk.MainInsured.Name = vehicle.Risk.MainInsured.Name + " " + vehicle.Risk.MainInsured.Surname + " " + vehicle.Risk.MainInsured.SecondSurname;
        //            ConcurrentBag<CompanyClause> clauses = new ConcurrentBag<CompanyClause>();
        //            //clausulas
        //            //if (riskClauses != null && riskClauses.Count > 0)
        //            //{
        //            //    Parallel.ForEach(riskClauses.Where(x => x.RiskId == item.RiskId).ToList(), riskClause =>
        //            //    {
        //            //        clauses.Add(new CompanyClause { Id = riskClause.ClauseId });
        //            //    });
        //            //    vehicle.Risk.Clauses = clauses.ToList();
        //            //}
        //            CompanyName companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(vehicle.Risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
        //            IssuanceCompanyName IssCompanyName = new IssuanceCompanyName();
        //            IssCompanyName.NameNum = companyName.NameNum;
        //            IssCompanyName.TradeName = companyName.TradeName;
        //            if (companyName.Address != null)
        //            {
        //                IssCompanyName.Address = new IssuanceAddress
        //                {
        //                    Id = companyName.Address.Id,
        //                    Description = companyName.Address.Description,
        //                    City = companyName.Address.City
        //                };
        //            }
        //            if (companyName.Phone != null)
        //            {
        //                IssCompanyName.Phone = new IssuancePhone
        //                {
        //                    Id = companyName.Phone.Id,
        //                    Description = companyName.Phone.Description
        //                };
        //            }
        //            if (companyName.Email != null)
        //            {
        //                IssCompanyName.Email = new IssuanceEmail
        //                {
        //                    Id = companyName.Email.Id,
        //                    Description = companyName.Email.Description
        //                };
        //            }
        //            vehicle.Risk.Beneficiaries = new List<CompanyBeneficiary>();
        //            if (riskBeneficiaries != null && riskBeneficiaries.Count > 0)
        //            {
        //                vehicle.Risk.Beneficiaries = new List<CompanyBeneficiary>();
        //                Object objlock = new object();
        //                IMapper mapper = ModelAssembler.CreateBeneficiary();
        //                List<ISSEN.RiskBeneficiary> beneficiaries = riskBeneficiaries.Where(x => x.RiskId == item.RiskId).ToList();
        //                ParallelHelper.ForEach(beneficiaries, riskBeneficiary =>
        //                {
        //                    try
        //                    {
        //                        CompanyBeneficiary CiaBeneficiary = new CompanyBeneficiary();
        //                        Beneficiary beneficiaryRisk = DelegateService.underwritingService.GetBeneficiariesByDescription(riskBeneficiary.BeneficiaryId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
        //                        if (beneficiaryRisk != null)
        //                        {
        //                            CiaBeneficiary = mapper.Map<UNMOD.Beneficiary, CompanyBeneficiary>(beneficiaryRisk);
        //                            CiaBeneficiary.CustomerType = CustomerType.Individual;
        //                            CiaBeneficiary.BeneficiaryType = new CompanyBeneficiaryType { Id = riskBeneficiary.BeneficiaryTypeCode };
        //                            CiaBeneficiary.Participation = riskBeneficiary.BenefitPercentage;
        //                            beneficiaryRisk.IndividualType = beneficiaryRisk.IndividualType;
        //                            List<CompanyName> companyNames = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(CiaBeneficiary.IndividualId, (CustomerType)CiaBeneficiary.CustomerType);
        //                            companyName = new CompanyName();
        //                            if (companyNames.Exists(x => x.NameNum == 0 && x.IsMain))
        //                            {
        //                                companyName = companyNames.First(x => x.IsMain);
        //                            }
        //                            else
        //                            {
        //                                companyName = companyNames.First();
        //                            }

        //                            //CiaBeneficiary.CompanyName = new IssuanceCompanyName
        //                            //{
        //                            //    NameNum = companyName.NameNum,
        //                            //    TradeName = companyName.TradeName,
        //                            //    Address = new IssuanceAddress
        //                            //    {
        //                            //        Id = companyName.Address.Id,
        //                            //        Description = companyName.Address.Description,
        //                            //        City = companyName.Address.City
        //                            //    },
        //                            //    Phone = new IssuancePhone
        //                            //    {
        //                            //        Id = companyName.Phone.Id,
        //                            //        Description = companyName.Phone.Description
        //                            //    },
        //                            //    Email = new IssuanceEmail
        //                            //    {
        //                            //        Id = companyName.Email.Id,
        //                            //        Description = companyName.Email.Description
        //                            //    }
        //                            //};

        //                            IssuanceCompanyName issuanceCompanyName = new IssuanceCompanyName();
        //                            issuanceCompanyName.NameNum = companyName.NameNum;
        //                            issuanceCompanyName.TradeName = companyName.TradeName;
        //                            if (companyName.Address != null)
        //                            {

        //                                issuanceCompanyName.Address = new IssuanceAddress
        //                                {
        //                                    Id = companyName.Address.Id,
        //                                    Description = companyName.Address.Description,
        //                                    City = companyName.Address.City
        //                                };
        //                            }
        //                            if (companyName.Phone != null)
        //                            {
        //                                issuanceCompanyName.Phone = new IssuancePhone
        //                                {
        //                                    Id = companyName.Phone.Id,
        //                                    Description = companyName.Phone.Description
        //                                };
        //                            }

        //                            if (companyName.Email != null)
        //                            {
        //                                issuanceCompanyName.Email = new IssuanceEmail
        //                                {
        //                                    Id = companyName.Email.Id,
        //                                    Description = companyName.Email.Description
        //                                };
        //                            }

        //                            lock (objlock)
        //                            {
        //                                vehicle.Risk.Beneficiaries.Add(CiaBeneficiary);
        //                            }
        //                        }

        //                    }
        //                    catch (Exception)
        //                    {
        //                    }
        //                    finally
        //                    {
        //                        DataFacadeManager.Dispose();
        //                    }

        //                });
        //            }
        //            CompanyContract vehicleFasecolda = GetVehicleByMakeIdModelIdVersionId(vehicle.Make.Id, vehicle.Model.Id, vehicle.Version.Id);
        //            vehicle.Fasecolda = vehicleFasecolda.Fasecolda;

        //            //coberturas
        //            List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();

        //            //No se filtra por ID del endoso ya que en la tabla ENDO_RISK_COVERAGE se guardan los riesgos afectados con el último EndorsementId y no se actualizan los demás riesgos de la póliza
        //            vehicle.Risk.Coverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdByRiskId(endorsementRisks.First(x => x.RiskId == item.RiskId).PolicyId, vehicle.Risk.RiskId);

        //            //accesorios
        //            ObjectCriteriaBuilder filterAccessory = new ObjectCriteriaBuilder();
        //            filterAccessory.Property(ISSEN.EndorsementRiskCoverage.Properties.RiskId, typeof(ISSEN.EndorsementRiskCoverage).Name);
        //            filterAccessory.Equal();
        //            filterAccessory.Constant(item.RiskId);

        //            AccessoryView accessoryView = new AccessoryView();
        //            builder = new ViewBuilder("AccessoryView")
        //            {
        //                Filter = filterAccessory.GetPredicate()
        //            };

        //            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
        //            {
        //                daf.FillView(builder, accessoryView);
        //            }

        //            Sistran.Core.Application.CommonService.Models.Parameter parameter = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.OriginalAccessories);

        //            if (accessoryView.RiskCoverDetails != null && accessoryView.RiskCoverDetails.Count > 0)
        //            {
        //                vehicle.Accesories = ModelAssembler.CreateAccesories(accessoryView);

        //                foreach (Accessory accessory in vehicle.Accesories)
        //                {
        //                    CompanyCoverage companyCoverage = vehicle.Risk.Coverages.FirstOrDefault(x => x.RiskCoverageId == Convert.ToInt32(accessory.AccessoryId));
        //                    if (companyCoverage != null && companyCoverage.Id == parameter.NumberParameter.Value)
        //                    {
        //                        accessory.IsOriginal = true;
        //                    }
        //                }
        //                vehicle.PriceAccesories = vehicle.Accesories.Where(x => !x.IsOriginal).Sum(y => y.Amount);
        //            }
        //            vehicles.Add(vehicle);
        //        }
        //        catch (Exception)
        //        {
        //        }
        //        finally
        //        {
        //            DataFacadeManager.Dispose();
        //        }
        //    });
        //    return vehicles.ToList();
        //}
        //#endregion
    }
}
