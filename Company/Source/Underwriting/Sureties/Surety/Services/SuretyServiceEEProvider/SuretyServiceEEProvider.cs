using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Assemblers;
using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.BusinessModels;
using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.DAOs;
using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Resources;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Sureties.SuretyServices.EEProvider;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota;
using Sistran.Core.Services.UtilitiesServices.Enums;
using CU = Sistran.Core.Application.Utilities.Utility;
using Rules = Sistran.Core.Framework.Rules;
using SUMODEL = Sistran.Core.Application.Sureties.Models;

namespace Sistran.Company.Application.Sureties.SuretyServices.EEProvider
{
    /// <summary>
    /// Polizas de Cumplimiento
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.Sureties.SuretyServices.EEProvider.SuretyServiceEEProvider" />
    /// <seealso cref="Sistran.Company.Application.Sureties.SuretyServices.ISuretyService" />
    public class SuretyServiceEEProvider : SuretyServiceEEProviderCore, ISuretyService
    {
        object obj = new object();
        #region Base
        /// <summary>
        /// Cotiza la poliza especifica lo los riesgos que contenga
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <param name="contracts"></param>
        /// <param name="runRulesPre"></param>
        /// <param name="runRulesPost"></param>
        /// <returns></returns>
        /// <exception cref="Sistran.Core.Framework.BAF.BusinessException">Error Obtener Quotate</exception>
        public List<CompanyContract> QuotateSureties(CompanyPolicy companyPolicy, List<CompanyContract> contracts, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                ContractBusiness contractBusiness = new ContractBusiness();
                return contractBusiness.QuotateContracts(companyPolicy, contracts, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Cotiza el riesgo especifico
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="runRulesPre"></param>
        /// <param name="runRulesPost"></param>
        /// <returns></returns>
        /// <exception cref="Sistran.Core.Framework.BAF.BusinessException">Error Obtener Quotate</exception>
        public CompanyContract QuotateSurety(CompanyContract contract, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                ContractBusiness contractBusiness = new ContractBusiness();
                return contractBusiness.QuotateContract(contract, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Ejecutar Reglas de Riesgo
        /// </summary>
        /// <param name="contract">riesgo</param>
        /// <param name="ruleSetId">Id Regla</param>
        /// <returns></returns>
        public CompanyContract RunRulesRisk(CompanyContract contract, int ruleSetId)
        {
            try
            {
                ContractBusiness contractBusiness = new ContractBusiness();
                return contractBusiness.RunRulesRisk(contract, ruleSetId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Tarifar cobertura
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="coverage">Cobertura</param>
        /// <param name="runRulesPre">Ejecutar reglas pre</param>
        /// <param name="runRulesPost">Ejecutar reglas post</param>
        /// <returns>Cobertura</returns>
        public CompanyCoverage QuotateCoverage(CompanyContract contract, CompanyCoverage coverage, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.Quotate(contract, coverage, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Ejecutar Reglas de Cobertura
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="coverage">Cobertura</param>
        /// <param name="ruleSetId">Id Regla</param>
        /// <returns>Cobertura</returns>
        public CompanyCoverage RunRulesCoverage(CompanyContract contract, CompanyCoverage coverage, int ruleSetId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.RunRulesCoverage(contract, coverage, ruleSetId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="contract">Modelo Contract</param>
        public CompanyContract CreateSuretyTemporal(CompanyContract contract, bool isMassive)
        {
            try
            {
                ContractDAO contractDAO = new ContractDAO();
                contract = contractDAO.CreateContractTemporal(contract, isMassive);
                return CompanySaveCompanySuretyTemporal(contract);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Poliza de cumplimiento
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="endorsmentId"></param>
        /// <returns>SuretyPolicy</returns>
        public List<CompanyContract> GetCompanySuretiesByPolicyId(int policyId)
        {
            try
            {
                ContractDAO contractDao = new ContractDAO();
                var getPolicy = contractDao.GetCompanyContractsByPolicyId(policyId);
                return getPolicy;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="riskId"></param>
        /// <returns>
        /// Lista de CompanySurety
        /// </returns>
        public List<CompanyContract> GetCompanySuretyByEndorsementId(int endorsementId, int riskId = 0)
        {
            ContractDAO contractDAO = new ContractDAO();
            return contractDAO.GetCompanySuretyByEndorsementId(endorsementId, riskId);
        }

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>
        /// Riesgos
        /// </returns>
        public List<CompanyContract> GetCompanySuretiesByTemporalId(int temporalId)
        {
            ContractDAO contractDAO = new ContractDAO();
            return contractDAO.GetCompanySuretiesByTemporalId(temporalId);
        }

        /// <summary>
        /// Polizas asociadas a individual
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<CompanyContract> GetSuretiesByIndividualId(int individualId)
        {
            try
            {
                ContractDAO contractDAO = new ContractDAO();
                return contractDAO.GetContractsByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <param name="companyContracts">The company contracts.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">Error creando endoso</exception>
        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyContract> companyContracts)
        {
            try
            {
                ContractDAO contractDAO = new ContractDAO();
                return contractDAO.CreateEndorsement(companyPolicy, companyContracts);
            }
            catch (Exception ex)
            {
                //  throw new BusinessException("Error creando endoso", ex);
                if (ex?.Message != null && ex.Message != String.Empty)
                    throw new BusinessException(ex.Message);
                else
                    throw new BusinessException(Errors.ErrorCreatePolicy);
            }
        }

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>
        /// Riesgo
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyContract GetCompanySuretyByRiskId(int riskId)
        {
            try
            {
                ContractDAO contractDao = new ContractDAO();
                return contractDao.GetCompanyContractByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public CompanyContract GetCompanySuretyByRiskIdModuleType(int riskId, ModuleType moduleType)
        {
            try
            {
                ContractBusiness contractBusiness = new ContractBusiness();
                return contractBusiness.GetCompanySuretyByRiskIdModuleType(riskId, moduleType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion Base
        #region Emision
        /// <summary>
        /// Gets the risk surety by identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// </exception>
        public CompanyContract GetRiskSuretyById(int temporalId, int id)
        {
            try
            {
                var ciaContract = GetCompanySuretyByRiskId(id);
                if (ciaContract != null)
                {
                    ciaContract.Risk.RiskId = ciaContract.Risk.Id;
                    ciaContract = GetRiskDescriptions(ciaContract, temporalId);
                    return ciaContract;
                }
                else
                {
                    throw new Exception(Errors.NoRiskWasFound);
                }
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorSearchRisk);
            }
        }

        /// <summary>
        /// Gets the risk descriptions.
        /// </summary>
        /// <param name="risk">The risk.</param>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <returns></returns>
        private CompanyContract GetRiskDescriptions(CompanyContract risk, int temporalId)
        {
            CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

            switch (policy.Endorsement.EndorsementType)
            {
                case EndorsementType.Emission:
                    risk = GetDataEmission(risk);
                    break;
                default:
                    break;
            }

            return risk;
        }

        /// <summary>
        /// Gets the data emission.
        /// </summary>
        /// <param name="risk">The risk.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private CompanyContract GetDataEmission(CompanyContract risk)
        {
            if (risk == null || risk.Risk == null)
            {
                throw new Exception(Errors.ErrorParameterEmpty);
            }
            var policy = risk.Risk.Policy;
            if (risk.Risk.Beneficiaries == null)
            {
                risk.Risk.Beneficiaries = policy.DefaultBeneficiaries;
            }

            risk.Risk.Coverages?.AsParallel().ForAll(item =>
            item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original));


            return risk;
        }

        /// <summary>
        /// Gets the risk sureties by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<CompanyContract> GetRiskSuretiesById(int id)
        {
            var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(id, false);
            if (companyPolicy != null)
            {
                var contracts = GetCompanySuretiesByTemporalId(companyPolicy.Id);
                contracts.AsParallel().ForAll(item => item.Risk.Policy = companyPolicy);
                return contracts;
            }
            else
            {
                throw new Exception(Errors.NoRiskWasFound);
            }

        }

        /// <summary>
        /// Validates the available amount by temporal identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">
        /// </exception>
        public List<string> ValidateRiskByTemporalId(int temporalId)/// valida el cupo desde emision
        {
            try
            {
                List<string> vs = new List<string>();
                OperatingQuotaEventDTO operatingQuotaEventDTO = new OperatingQuotaEventDTO();
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                List<CompanyContract> companyContracts = GetCompanySuretiesByTemporalId(temporalId);
                //Validacion de Cupo operativo frente ala emision
                if (policy != null && companyContracts != null)
                {
                    CU.Parallel.ForEach(companyContracts, risk =>
                    {

                        if (risk.Risk.Text != null)
                        {
                            risk.Risk.Text.TextBody = unicode_iso8859(risk.Risk.Text.TextBody);
                        }

                        //Grupo Economico
                        operatingQuotaEventDTO = DelegateService.economicGroupIntegration.GetCumuloCupoEconomicGroupByIndividualIdByLineBusinessId(risk.Contractor.IndividualId, policy.Prefix.Id);
                        if (operatingQuotaEventDTO != null && operatingQuotaEventDTO.EconomicGroupEventDTO != null)
                        {
                            //Valida cupo operativo
                            if ((operatingQuotaEventDTO.IndividualOperatingQuota.ValueOpQuotaAMT - operatingQuotaEventDTO.ApplyEndorsement.AmountCoverage) / policy.ExchangeRate.SellAmount <= policy.Summary.AmountInsured / policy.ExchangeRate.SellAmount)
                            {
                                vs.Add(Errors.MessageValidateOperatingAvailable);
                            }
                            //Validacion de Fecha del Cupo vs Emision
                            if (operatingQuotaEventDTO.IndividualOperatingQuota.EndDateOpQuota <= policy.CurrentFrom)
                            {
                                vs.Add(Errors.QuotaIsNotCurrent);
                            }
                        }
                        else
                        {
                            //Consorcio
                            var IsConsort = IsConsortiumindividualId(risk.Contractor.IndividualId);
                            if (policy.Endorsement.EndorsementType == EndorsementType.Modification || policy.Endorsement.EndorsementType == EndorsementType.Cancellation || policy.Endorsement.EndorsementType == EndorsementType.EffectiveExtension)
                            {
                                operatingQuotaEventDTO = DelegateService.consortiumIntegrationService.GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId(risk.Contractor.IndividualId, policy.Prefix.Id, true,0);
                            }
                            else
                            {
                                operatingQuotaEventDTO = DelegateService.consortiumIntegrationService.GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId(risk.Contractor.IndividualId, policy.Prefix.Id, false,policy.Endorsement.Id);
                            }
                            if (operatingQuotaEventDTO != null && operatingQuotaEventDTO.consortiumEventDTO != null && IsConsort)
                            {
                                // valida vigencia del cupo por consorciado
                                List<OperatingQuotaEventDTO> operatingQuotaEventDTOs = DelegateService.consortiumIntegrationService.GetValidityParticipantCupoInConsortium(risk.Contractor.IndividualId, (long)policy.Summary.AmountInsured, policy.Prefix.Id);

                                if (operatingQuotaEventDTOs.Count > 0)
                                {
                                    CU.Parallel.ForEach(operatingQuotaEventDTOs, item =>
                                    {
                                        if (item.IndividualOperatingQuota.IndividualID != 0)
                                        {
                                            if (item.IndividualOperatingQuota.EndDateOpQuota < policy.CurrentFrom)
                                            {
                                                Person person = DelegateService.uniquePersonService.GetPersonByIndividualId(item.IdentificationId);
                                                if (person.IdentificationDocument != null && person.IdentificationDocument.Number != null)
                                                {
                                                    vs.Add("Consorciado: " + person.FullName + " con Numero de Documento : " + Convert.ToString(person.IdentificationDocument.Number) + Errors.QuotaIsNotCurrent);
                                                }
                                                else
                                                {
                                                    CompanyCompany companyCompany = DelegateService.uniquePersonService.GetCompanyByIndividualId(item.IdentificationId);

                                                    if (companyCompany.IdentificationDocument.Number != null)
                                                    {
                                                        vs.Add("Consorciado: " + companyCompany.FullName + " con Numero de Documento : " + Convert.ToString(companyCompany.IdentificationDocument.Number) + Errors.QuotaIsNotCurrent);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //valida si uno de los participantes no tiene cupo para el ramo 
                                            Person person = DelegateService.uniquePersonService.GetPersonByIndividualId(item.consortiumEventDTO.IndividualId);
                                            if (person.IdentificationDocument.Number != null)
                                            {
                                                vs.Add("Consorciado: " + person.FullName + " con Numero de Documento :" + Convert.ToString(person.IdentificationDocument.Number) + Errors.NotOperationQuota);
                                            }
                                            else
                                            {
                                                CompanyCompany companyCompany = DelegateService.uniquePersonService.GetCompanyByIndividualId(item.consortiumEventDTO.IndividualId);

                                                if (companyCompany.IdentificationDocument.Number != null)
                                                {
                                                    vs.Add("Consorciado: " + companyCompany.FullName + " con Numero de Documento :" + Convert.ToString(companyCompany.IdentificationDocument.Number) + Errors.NotOperationQuota);
                                                }
                                            }
                                        }
                                        //Valida cupo operativo
                                        if (item.consortiumEventDTO.IsConsortium)
                                        {
                                            Person person = DelegateService.uniquePersonService.GetPersonByIndividualId(item.IdentificationId);
                                            if (person.IdentificationDocument?.Number != null)
                                            {
                                                vs.Add("Consorciado: " + person.FullName + " con Numero de Documento :" + Convert.ToString(person.IdentificationDocument.Number) + Errors.NotOperationQuota);
                                            }
                                            else
                                            {
                                                CompanyCompany companyCompany = DelegateService.uniquePersonService.GetCompanyByIndividualId(item.IdentificationId);

                                                if (companyCompany.IdentificationDocument?.Number != null)
                                                {
                                                    vs.Add("Consorciado: " + companyCompany.FullName + " con Numero de Documento :" + Convert.ToString(companyCompany.IdentificationDocument.Number) + Errors.NotOperationQuota);
                                                }
                                            }
                                        }

                                    });

                                }
                            }
                            else
                            {
                                //Participante de consorcio
                                if (operatingQuotaEventDTO != null && operatingQuotaEventDTO.consortiumEventDTO != null && !IsConsort)
                                {
                                    //Valida cupo operativo
                                    if (operatingQuotaEventDTO.IndividualOperatingQuota != null && operatingQuotaEventDTO.ApplyEndorsement != null
                                        && (operatingQuotaEventDTO.IndividualOperatingQuota.ValueOpQuotaAMT - operatingQuotaEventDTO.ApplyEndorsement.AmountCoverage) / policy.ExchangeRate.SellAmount < policy.Summary.AmountInsured / policy.ExchangeRate.SellAmount)
                                    {
                                        vs.Add(Errors.MessageValidateOperatingAvailable);
                                    }

                                    //valida vigencia del cupo
                                    if (operatingQuotaEventDTO.IndividualOperatingQuota != null && operatingQuotaEventDTO.IndividualOperatingQuota.EndDateOpQuota < policy.CurrentFrom)
                                    {
                                        vs.Add(Errors.QuotaIsNotCurrent);
                                    }

                                }
                                else
                                {
                                    //Individual
                                    operatingQuotaEventDTO = DelegateService.OperationQuotaIntegrationService.GetOperatingQuotaEventByIndividualIdByLineBusinessId(risk.Contractor.IndividualId, policy.Prefix.Id);

                                    if (operatingQuotaEventDTO != null && operatingQuotaEventDTO.IdentificationId != 0 && operatingQuotaEventDTO.consortiumEventDTO == null)
                                    {
                                        //Valida cupo operativo
                                        if (operatingQuotaEventDTO.IndividualOperatingQuota != null && operatingQuotaEventDTO.ApplyEndorsement != null &&
                                            operatingQuotaEventDTO.IndividualOperatingQuota.ValueOpQuotaAMT - operatingQuotaEventDTO.ApplyEndorsement.AmountCoverage / policy.ExchangeRate.SellAmount < policy.Summary.AmountInsured / policy.ExchangeRate.SellAmount)
                                        {
                                            vs.Add(Errors.MessageValidateOperatingAvailable);
                                        }

                                        //valida vigencia del cupo
                                        if (operatingQuotaEventDTO.IndividualOperatingQuota != null && operatingQuotaEventDTO.IndividualOperatingQuota.EndDateOpQuota < policy.CurrentFrom)
                                        {
                                            vs.Add(Errors.QuotaIsNotCurrent);
                                        }
                                    }
                                    else
                                    {
                                        vs.Add(Errors.MessageValidateOperatingAvailable);
                                    }
                                }
                            }
                        }

                    });

                }
                // Objeto del contrato
                if (companyContracts.Any(x => x.Risk.Text?.TextBody == null) || companyContracts.Any(x => x.Risk.Text == null))
                {
                    vs.Add(Errors.NoDataTextRisk);
                }
                // Prospecto
                if (companyContracts.Any(x => x.Contractor.CustomerType == CustomerType.Prospect))
                {
                    vs.Add(Errors.ContractorNotPerson);
                }

                return vs;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public CompanyContract SaveCompanyRisk(int temporalId, CompanyContract surety)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (policy != null)
                {
                    bool existRisk = false;
                    List<CompanyContract> sureties = GetCompanySuretiesByTemporalId(temporalId);

                    if (sureties != null && sureties.Count > 0)
                    {
                        existRisk = ExistsRisk(temporalId, surety.Risk.Id, surety.Contractor.Name, sureties);
                    }
                    if (!existRisk)
                    {
                        if (surety != null && surety.Risk != null)
                        {
                            surety.Risk.Beneficiaries = new List<CompanyBeneficiary>();

                            surety.Risk.CoveredRiskType = policy.Product.CoveredRisk.CoveredRiskType;
                            surety.Risk.Policy = policy;

                            if (policy.Endorsement.EndorsementType == EndorsementType.Modification)
                            {
                                surety.Risk.Status = RiskStatusType.Included;
                            }

                            if (surety.Risk?.Id == 0)
                            {
                                int suretyCount = sureties?.Count ?? 0;
                                if (suretyCount < policy.Product.CoveredRisk.MaxRiskQuantity)
                                {
                                    if (policy.DefaultBeneficiaries != null && policy.DefaultBeneficiaries.Count > 0)
                                    {
                                        surety.Risk.Beneficiaries = policy.DefaultBeneficiaries;
                                    }
                                    else
                                    {
                                        if (surety.Risk.MainInsured != null)
                                        {
                                            surety.Risk.Beneficiaries.Add(ModelAssembler.CreateBeneficiaryFromInsured(surety.Risk.MainInsured));
                                        }
                                        else
                                        {
                                            throw new BusinessException(Errors.ErrorInsuredPrincipalIsEmpty);
                                        }
                                    }
                                }
                                else
                                {
                                    throw new BusinessException(Errors.ProductNotAddingMoreRisks);
                                }
                            }
                            else
                            {
                                if (policy.Endorsement.EndorsementType != null)
                                {
                                    switch (policy.Endorsement.EndorsementType.Value)
                                    {
                                        case EndorsementType.Emission:
                                        case EndorsementType.Renewal:
                                            surety = SetDataEmission(surety);
                                            break;
                                        case EndorsementType.Modification:
                                            surety = SetDataModification(surety);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    throw new BusinessException(Errors.ErrorEndorsementTypeEmpty);
                                }

                            }

                            Task<CompanyContract> suretyTask = CU.Task.Run(() =>
                            {
                                lock (obj)
                                {
                                    surety = CreateSuretyTemporal(surety, false);
                                    return surety;
                                }
                            });
                            suretyTask.Wait();

                            return surety;
                        }
                        else
                        {
                            throw new BusinessException(Errors.ErrorRiskExist);
                        }
                    }
                    else
                    {
                        throw new BusinessException(Errors.ProductNotAddingMoreRisks);
                    }

                }
                else
                {
                    throw new BusinessException(Errors.ErrorTemporalNotFound);
                }

            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    throw new BusinessException(ex.Message, ex);
                }
                else
                {
                    throw new Exception(Errors.ErrorSaveSurety, ex);
                }
            }
        }

        /// <summary>
        /// Sets the data emission.
        /// </summary>
        /// <param name="surety">The surety.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private CompanyContract SetDataEmission(CompanyContract surety)
        {
            try
            {
                CompanyContract suretyOld = GetCompanySuretyByRiskId(surety.Risk.Id);
                surety.ContractObject = suretyOld.ContractObject;
                surety.Guarantees = suretyOld.Guarantees;
                surety.Risk.Beneficiaries = suretyOld.Risk.Beneficiaries;
                surety.Risk.Text = suretyOld.Risk.Text;
                surety.Risk.Clauses = suretyOld.Risk.Clauses;
                surety.Risk.SecondInsured = suretyOld.Risk.SecondInsured;
                return surety;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// Sets the data modification.
        /// </summary>
        /// <param name="surety">The surety.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private CompanyContract SetDataModification(CompanyContract surety)
        {
            try
            {
                CompanyContract suretyOld = GetCompanySuretyByRiskId(surety.Risk.Id);

                surety.Risk.RiskId = suretyOld.Risk.RiskId;
                surety.Risk.Number = suretyOld.Risk.Number;
                surety.Risk.CoveredRiskType = suretyOld.Risk.CoveredRiskType;
                surety.Risk.Description = suretyOld.Risk.Description;
                surety.Risk.Beneficiaries = suretyOld.Risk.Beneficiaries;
                surety.Risk.Text = suretyOld.Risk.Text;
                surety.Risk.Text.TextBody = unicode_iso8859(surety.Risk.Text.TextBody);
                surety.Risk.Clauses = suretyOld.Risk.Clauses;
                surety.Risk.Status = suretyOld.Risk.Status;
                surety.Risk.OriginalStatus = suretyOld.Risk.OriginalStatus;
                surety.ContractObject = suretyOld.ContractObject;
                surety.Contractor = suretyOld.Contractor;
                surety.Country.Id = suretyOld.Country?.Id ?? surety.Country.Id;
                surety.State.Id = suretyOld.State?.Id ?? surety.State.Id;
                surety.City.Id = suretyOld.City?.Id ?? surety.City.Id;

                if (surety.Risk.MainInsured.IndividualId != suretyOld.Risk.MainInsured.IndividualId)
                {
                    surety.Risk.MainInsured = surety.Risk.MainInsured;
                }
                else
                {
                    int nameNum = surety.Risk.MainInsured.CompanyName.NameNum;
                    int phoneId = surety.Risk.MainInsured.CompanyName.Phone.Id;
                    int addressId = surety.Risk.MainInsured.CompanyName.Address.Id;
                    surety.Risk.MainInsured = suretyOld.Risk.MainInsured;
                    surety.Risk.MainInsured.CompanyName.NameNum = nameNum;
                    if (phoneId > 0)
                    {
                        surety.Risk.MainInsured.CompanyName.Phone.Id = phoneId;
                    }
                    if (addressId > 0)
                    {
                        surety.Risk.MainInsured.CompanyName.Address.Id = addressId;
                    }
                }
                if (suretyOld?.Guarantees != null && suretyOld.Guarantees.Count > 0)
                {
                    surety.Guarantees = suretyOld.Guarantees;
                }

                if (surety.Risk.Status != RiskStatusType.Included && surety.Risk.Status != RiskStatusType.Excluded)
                {
                    surety.Risk.Status = RiskStatusType.Modified;
                }

                return surety;
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// Deletes the risk.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">
        /// </exception>
        public bool DeleteRisk(int temporalId, int riskId)
        {
            try
            {
                bool result;
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (companyPolicy == null)
                {
                    throw new BusinessException(Errors.ErrorSearchPolicy);

                }

                CompanyContract contract = GetCompanySuretyByRiskId(riskId);
                if (contract != null && contract.Risk != null)
                {
                    if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Emission
                        || companyPolicy.Endorsement.EndorsementType == EndorsementType.Renewal || contract.Risk.Status == RiskStatusType.Included)
                    {
                        result = DelegateService.utilitiesServiceCore.DeletePendingOperation(riskId);
                        DelegateService.underwritingService.DeleteRisk(riskId);
                    }
                    else
                    {
                        contract.Risk.Policy = companyPolicy;
                        contract.Risk.Status = RiskStatusType.Excluded;
                        contract.Risk.Description = contract.Contractor.Name + " (" + EnumHelper.GetItemName<RiskStatusType>(contract.Risk.Status) + ")";
                        contract.Risk.IsPersisted = true;
                        contract = QuotateSurety(contract, true, true);
                        if (contract != null && contract.Risk != null && contract.Risk.Coverages != null)
                        {
                            contract.Risk.Coverages.AsParallel().ForAll(x => x.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(x.CoverStatus));
                            CreateSuretyTemporal(contract, false);
                            result = true;
                        }
                        else
                        {
                            throw new BusinessException(Errors.ErrorDeleteRisk);
                        }
                    }
                    if (!result)
                    {
                        throw new BusinessException(Errors.ErrorDeleteRisk);
                    }
                    return result;
                }
                else
                {
                    throw new BusinessException(Errors.ErrorSearchRisk);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Existses the risk.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="ContractName">Name of the contract.</param>
        /// <param name="contracts">The contracts.</param>
        /// <returns></returns>
        public bool ExistsRisk(int temporalId, int? riskId, string ContractName, List<CompanyContract> contracts = null)
        {
            bool exists = false;
            if (temporalId != 0 && contracts == null)
            {
                contracts = GetCompanySuretiesByTemporalId(temporalId);
            }

            if (contracts != null && contracts.Any())
            {
                object lockobj = new object();
                Parallel.For(0, contracts.Count, (i, loopState) =>
                {
                    if (contracts[i].Risk.Description == ContractName)
                    {
                        if (riskId.HasValue)
                        {
                            if (contracts[i].Risk.Id != riskId.Value)
                            {
                                exists = true;
                            }
                        }
                        else
                        {
                            lock (lockobj)
                            {
                                exists = true;
                            }
                        }
                        loopState.Break();

                    }

                });
            }

            return exists;
        }

        /// <summary>
        /// Converts the prospect to insured.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <param name="documentNumber">The document number.</param>
        /// <returns></returns>
        public bool ConvertProspectToInsured(int temporalId, int individualId, string documentNumber)
        {
            try
            {
                var result = DelegateService.underwritingService.ConvertProspectToHolder(temporalId, individualId, documentNumber);
                var contracts = GetCompanySuretiesByTemporalId(temporalId);
                if (contracts != null && contracts.Count > 0)
                {
                    CU.Parallel.For(0, contracts.Count, (int i) =>
                    {
                        if (contracts[i].Contractor.IdentificationDocument.Number == documentNumber)
                        {
                            var insured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(individualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual);

                            if (insured != null)
                            {
                                insured.Name = insured.Surname + " " + (string.IsNullOrEmpty(insured.SecondSurname) ? "" : insured.SecondSurname + " ") + insured.Name;
                                contracts[i].Contractor.IndividualId = insured.IndividualId;
                                contracts[i].Contractor.InsuredId = insured.InsuredId;
                                contracts[i].Contractor.Name = insured.Name;
                                contracts[i].Contractor.CustomerType = CustomerType.Individual;
                                contracts[i].Contractor.PaymentMethod = insured.PaymentMethod;
                                contracts[i].Contractor.BirthDate = insured.BirthDate;
                                contracts[i].Contractor.IdentificationDocument = insured.IdentificationDocument;
                            }


                            var companyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(contracts[i].Contractor.IndividualId, CustomerType.Individual).FirstOrDefault();
                            contracts[i].Contractor.CompanyName = new IssuanceCompanyName
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
                        }
                        var companyRisk = DelegateService.underwritingService.ConvertProspectToInsured(contracts[i].Risk, individualId, documentNumber);
                        contracts[i].Risk.Beneficiaries = companyRisk.Beneficiaries;
                        CreateSuretyTemporal(contracts[i], false);
                    });
                }

                return true;
            }
            catch (Exception)
            {

                throw new Exception(Errors.ErrorConvertingProspectIntoIndividual);
            }
        }

        public CompanyText SaveTexts(int riskId, CompanyText companyText)
        {
            CompanyContract risk = GetCompanySuretyByRiskId(riskId);

            if (risk != null)
            {
                risk.Risk.Text = companyText;

                risk = CreateSuretyTemporal(risk, false);
                return risk.Risk.Text;
            }
            else
            {
                throw new Exception(Errors.ErrorSearchRisk);
            }
        }

        /// <summary>
        /// Saves the contract object.
        /// </summary>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="companyText">The company text.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public CompanyText SaveContractObject(int riskId, CompanyText companyText)
        {
            CompanyContract risk = GetCompanySuretyByRiskId(riskId);

            if (risk != null)
            {
                risk.ContractObject = companyText;
                risk = CreateSuretyTemporal(risk, false);
                return risk.ContractObject;
            }
            else
            {
                throw new Exception(Errors.ErrorSearchRisk);
            }
        }

        public bool SaveCoverages(int tempId, int riskId, List<CompanyCoverage> coverages)
        {
            try
            {
                if (!ValidateCoverage(coverages))
                {
                    throw new Exception(Errors.ErrorCoverageRateEmpty);
                }
                Task<CompanyPolicy> policy = CU.Task.Run(() =>
                {
                    return DelegateService.underwritingService.GetCompanyPolicyByTemporalId(tempId, false);
                });
                CompanyContract contract = GetCompanySuretyByRiskId(riskId);
                policy.Wait();
                if (policy.Result != null && contract != null)
                {
                    CompanyPolicy suretyPolicy = policy.Result;
                    suretyPolicy.IsPersisted = true;
                    contract.Risk.Coverages = coverages;
                    contract.Risk.Policy = suretyPolicy;
                    contract = QuotateSurety(contract, false, true);
                    contract.Risk.Id = riskId;
                    CreateSuretyTemporal(contract, false);
                    return true;
                }
                else
                {
                    new Exception(Errors.NoExistTemporaryNoHaveCoverages);
                }
                return false;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Quotations the surety coverages.
        /// </summary>
        /// <param name="tempId">The temporary identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="coverage">The coverage.</param>
        /// <param name="runRules">if set to <c>true</c> [run rules].</param>
        /// <param name="listCompanyCoverage">The list company coverage.</param>
        /// <returns></returns>
        public List<CompanyCoverage> QuotationSuretyCoverages(int tempId, int riskId, CompanyCoverage coverage, bool runRules, List<CompanyCoverage> listCompanyCoverage, int policyId)
        {
            Rules.Facade Facade = new Rules.Facade();
            int index = -1;
            index = listCompanyCoverage?.FindIndex(c => c.Id == coverage.Id) == null ? -1 : (int)listCompanyCoverage?.FindIndex(c => c.Id == coverage.Id);
            if (listCompanyCoverage == null)
            {
                listCompanyCoverage = new List<CompanyCoverage>();
            }

            var coverageList = listCompanyCoverage.Copy();
            var coverageNew = QuotationSuretyCoverage(tempId, riskId, coverage, runRules, listCompanyCoverage);
            if (coverage.EndorsementType == EndorsementType.Emission)
            {
                if (listCompanyCoverage?.Count == 1 && !coverageNew.IsEnabledMinimumPremium)
                {
                    coverageNew = QuotationSuretyCoverage(tempId, riskId, coverageNew, runRules, null);
                }
                if (listCompanyCoverage?.Count > 1)
                {
                    coverageNew = QuotationSuretyCoverage(tempId, riskId, coverageNew, runRules, listCompanyCoverage);
                    if (listCompanyCoverage.Any(x => x.IsEnabledMinimumPremium))
                    {
                        foreach (CompanyCoverage coverageData in coverageList.Where(x => x.IsEnabledMinimumPremium))
                        {
                            var coverageBase = QuotationSuretyCoverage(tempId, riskId, coverageData, runRules, listCompanyCoverage);
                            coverageData.MinimumPremiumCoverage = coverageBase.PremiumAmount;
                            coverageData.PremiumAmount = coverageBase.PremiumAmount;
                            coverageData.RateType = coverageBase.RateType;
                            coverageData.CalculationType = coverageBase.CalculationType;
                            coverageData.Rate = coverageBase.Rate;
                        }
                    }

                    if (index == -1)
                    {
                        if (listCompanyCoverage.FirstOrDefault(x => x.Id == coverageNew.Id) != null)
                        {
                            int indexCoverage = listCompanyCoverage.IndexOf(listCompanyCoverage.Single(i => i.Id == coverageNew.Id));
                            listCompanyCoverage[indexCoverage] = coverageNew;
                        }
                        else
                        {
                            listCompanyCoverage.Add(coverageNew);
                        }
                    }
                    if (coverageList?.Count > 0)
                    {
                        if (coverageList.FirstOrDefault(x => x.Id == coverage.Id) == null)
                        {
                            coverageList.Add(coverageNew);
                        }
                        else
                        {
                            coverageList[index] = coverageNew;
                        }
                    }

                    return coverageList;
                }
                else
                {
                    if (coverageList.FirstOrDefault(x => x.Id == coverage.Id) == null)
                    {
                        coverageList.Add(coverageNew);
                    }
                    else
                    {
                        coverageList[index] = coverageNew;
                    }
                    return coverageList;
                }
            }
            else
            {
                if (index == -1)
                {
                    listCompanyCoverage.Add(coverageNew);
                }
                else
                {
                    listCompanyCoverage[index] = coverageNew;
                }
                return listCompanyCoverage;
            }

        }
        /// <summary>
        /// Quotations the surety coverage.
        /// </summary>
        /// <param name="tempId">The temporary identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="coverage">The coverages.</param>
        /// <param name="runRules">if set to <c>true</c> [run rules].</param>
        /// <returns></returns>
        public CompanyCoverage QuotationSuretyCoverage(int tempId, int riskId, CompanyCoverage coverage, bool runRules, List<CompanyCoverage> listCompanyCoverage)
        {
            CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(tempId, false);
            CompanyContract contract = GetCompanySuretyByRiskId(riskId);
            contract.Risk.Policy = policy;
            var endorsmentType = policy.Endorsement.EndorsementType;
            if (endorsmentType != EndorsementType.Emission && endorsmentType != EndorsementType.Renewal && endorsmentType != EndorsementType.EffectiveExtension)
            {
                if (runRules && coverage.CoverStatus == CoverageStatusType.NotModified && coverage.PremiumAmount != 0)
                {
                    coverage.CoverStatus = CoverageStatusType.Modified;
                    coverage.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Modified));
                }
            }

            if (endorsmentType == EndorsementType.Emission && listCompanyCoverage != null)
            {
                contract.Risk.Coverages = listCompanyCoverage;
                if (coverage != null)
                {
                    if (!contract.Risk.Coverages.Any(x => x.Id == coverage.Id))
                    {
                        contract.Risk.Coverages.Add(coverage);
                    }
                    else if (contract.Risk.Coverages.Any(x => x.Id == coverage.Id))
                    {
                        contract.Risk.Coverages.RemoveAll(x => x.Id == coverage.Id);
                        contract.Risk.Coverages.Add(coverage);
                    }
                }
            }
            if (endorsmentType == EndorsementType.Emission)
            {
                if (contract.Risk.Coverages == null && coverage != null)
                {
                    contract.Risk.Coverages = new List<CompanyCoverage>();
                    contract.Risk.Coverages.Add(coverage);
                }
            }
            return QuotateCoverage(contract, coverage, false, runRules); ;
        }

        /// <summary>
        /// Gets the coverages by product identifier group coverage identifier.
        /// </summary>
        /// <param name="TemporalId">The temporal identifier.</param>
        /// <param name="ciaContract"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException">
        /// </exception>
        public List<CompanyCoverage> GetCoveragesByProductIdGroupCoverageId(int TemporalId, CompanyContract ciaContract)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(TemporalId, false);
                if (policy != null)
                {
                    var coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(policy.Product.Id, ciaContract.Risk.GroupCoverage.Id, policy.Prefix.Id);
                    if (coverages != null)
                    {
                        coverages = GetCoveragesByRiskId(policy, coverages);
                        coverages = coverages.Where(x => x.IsSelected).ToList();
                        ciaContract.Risk.IsPersisted = true;
                        policy.IsPersisted = true;
                        ciaContract.Risk.Policy = policy;
                        //Ejecutar reglas Post
                        var ciacoverages = new ConcurrentBag<CompanyCoverage>();
                        object obj = new object();
                        ciaContract.Risk.Coverages = coverages;
                        CU.Parallel.For(0, coverages.Count, coverageRow =>
                        {
                            CompanyCoverage coverage;
                            lock (obj)
                            {
                                coverage = coverages[coverageRow];
                                if (coverage.RuleSetId.HasValue)
                                {
                                    coverage = RunRulesCoverage(ciaContract, coverage, coverage.RuleSetId.Value);
                                }
                                ciacoverages.Add(coverage);
                            }

                        });
                        return ciacoverages.ToList();
                    }
                    else
                    {
                        throw new BusinessException(Errors.ProductNotCoverages);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTemporalNotFound);
                }
            }
            catch (Exception)
            {

                throw new BusinessException(Errors.ErrorSearchCoverages);
            }
        }

        /// <summary>
        /// Gets the coverages by risk identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="coverages">The coverages.</param>
        /// <returns></returns>
        private List<CompanyCoverage> GetCoveragesByRiskId(CompanyPolicy policy, List<CompanyCoverage> coverages)
        {
            if (policy != null && policy.Id > 0)
            {
                string coverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original);
                coverages.AsParallel().ForAll(item =>
                {
                    item.EndorsementType = policy.Endorsement.EndorsementType;
                    item.CalculationType = Core.Services.UtilitiesServices.Enums.CalculationType.Prorate;
                    item.CurrentFrom = policy.CurrentFrom;
                    item.CurrentTo = policy.CurrentTo;
                    item.RateType = RateType.Percentage;
                    item.LimitAmount = 0;
                    item.SubLimitAmount = 0;
                    item.CoverStatus = CoverageStatusType.Original;
                    item.CoverStatusName = coverStatusName;
                });
            }

            return coverages;
        }
        #region Contragarantias
        /// <summary>
        /// Gets the insured guarantee by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<IssuanceGuarantee> GetInsuredGuaranteeByIndividualId(int individualId)
        {
            try
            {
                List<IssuanceGuarantee> guarantees = DelegateService.underwritingService.GetInsuredGuaranteesByIndividualId(individualId);
                if (guarantees.Count > 0)
                {
                    List<IssuanceGuarantee> guaranteesClose = guarantees.Where(x => x.InsuredGuarantee.IsCloseInd == true).ToList();
                    if (guaranteesClose.Count > 0)
                    {
                        List<SUMODEL.RiskSuretyGuarantee> guaranteesSureties = GetRiskSuretyGuaranteesByGuarantees(guaranteesClose);
                        if (guaranteesSureties.Count > 0)
                        {
                            guarantees = guarantees.Where(g => !guaranteesSureties.Distinct().Any(gs => gs.GuaranteeId == g.InsuredGuarantee.Id)).ToList();
                        }
                    }
                }
                return guarantees;
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorSearchGuarantees);

            }
        }

        public bool GetInsuredGuaranteeRelationPolicy(int guaranteeId)
        {
            try
            {
                ContractDAO contractDAO = new ContractDAO();
                return contractDAO.GetInsuredGuaranteeRelationPolicy(guaranteeId);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorSearchGuarantees);
            }
        }
        #region reglas
        /// <summary>
        /// Runs the rules risk surety.
        /// </summary>
        /// <param name="tempId">The temporary identifier.</param>
        /// <param name="ruleSetId">The rule set identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public CompanyContract RunRulesRiskSurety(int tempId, int? ruleSetId)
        {
            try
            {
                CompanyPolicy suretyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(tempId, false);
                if (suretyPolicy != null)
                {
                    var contract = new CompanyContract { Risk = new CompanyRisk { Policy = suretyPolicy } };
                    if (ruleSetId != null)
                    {

                        if (ruleSetId.GetValueOrDefault() > 0)
                        {
                            contract = RunRulesRisk(contract, ruleSetId.Value);
                        }
                    }
                    return contract;
                }
                else
                {
                    throw new Exception(Errors.ErrorSearchPolicy);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #endregion Contragarantias
        public List<CompanyClause> SaveClauses(int riskId, List<CompanyClause> clauses)
        {
            try
            {
                CompanyContract ciaContract = GetCompanySuretyByRiskId(riskId);

                if (ciaContract != null && ciaContract.Risk.Id > 0)
                {
                    ciaContract.Risk.Clauses = clauses;
                    ciaContract = CreateSuretyTemporal(ciaContract, false);

                    if (ciaContract != null)
                    {
                        return clauses;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorSaveClauses);
                    }
                }
                else
                {
                    throw new Exception(Errors.NoRiskWasFound);

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Excludes the coverage.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="riskCoverageId">The risk coverage identifier.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public CompanyCoverage ExcludeCoverage(int temporalId, int riskId, int riskCoverageId, string description)
        {
            Task<CompanyPolicy> suretyPolicy = CU.Task.Run(() => DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false));
            Task<CompanyContract> contract = CU.Task.Run(() =>
            {
                var result = GetCompanySuretyByRiskId(riskId);
                DataFacadeManager.Dispose();
                return result;
            }
            );
            suretyPolicy.Wait();
            contract.Wait();
            if (suretyPolicy != null && contract != null)
            {
                contract.Result.Risk.Policy = suretyPolicy.Result;
                var coverage = DelegateService.underwritingService.GetCompanyCoverageByRiskCoverageId(riskCoverageId);
                coverage.Description = description;
                coverage.SubLineBusiness = contract.Result.Risk.Coverages.First(x => x.RiskCoverageId == riskCoverageId).SubLineBusiness;
                coverage.Number = contract.Result.Risk.Coverages.First(x => x.RiskCoverageId == riskCoverageId).Number;

                if (coverage.CurrentFrom > suretyPolicy.Result.CurrentTo)
                {
                    throw new BusinessException(Errors.DateExclusionGreaterValidity);
                }
                coverage.EndorsementType = suretyPolicy.Result.Endorsement.EndorsementType;
                if (suretyPolicy.Result.CurrentFrom > coverage.CurrentFrom)
                {
                    coverage.CurrentFrom = suretyPolicy.Result.CurrentFrom;
                }
                coverage.CoverStatus = CoverageStatusType.Excluded;
                coverage = QuotateCoverage(contract.Result, coverage, false, false);
                coverage.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Excluded));
                return coverage;
            }
            else
            {
                throw new Exception(Errors.NoRiskWasFound);
            }
        }


        /// <summary>
        /// Updates the risks.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <returns></returns>
        public CompanyPolicy UpdateRisks(int temporalId)
        {
            try
            {
                Task<CompanyPolicy> companyPolicy = CU.Task.Run(() => DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false));
                Task<List<CompanyContract>> companyContracts = CU.Task.Run(() =>
                {
                    var result = GetCompanySuretiesByTemporalId(temporalId);
                    DataFacadeManager.Dispose();
                    return result;
                }
                );
                companyPolicy.Wait();
                companyContracts.Wait();
                if (companyPolicy.Result != null && companyContracts.Result != null && companyContracts.Result.Count > 0)
                {
                    CU.Parallel.For(0, companyContracts.Result.Count, suretyRow =>
                    {
                        var companyContract = companyContracts.Result[suretyRow];
                        if (companyContract.Contractor.IdentificationDocument.Number == companyPolicy.Result.Holder.IdentificationDocument.Number &&
                        companyContract.Contractor.IdentificationDocument.DocumentType.Id == companyPolicy.Result.Holder.IdentificationDocument.DocumentType.Id)
                        {
                            companyContract.Contractor.CompanyName = companyPolicy.Result.Holder.CompanyName;
                            companyContract.Contractor.IndividualId = companyPolicy.Result.Holder.IndividualId;
                            companyContract.Contractor.IndividualType = companyPolicy.Result.Holder.IndividualType;
                            companyContract.Contractor.CustomerType = companyPolicy.Result.Holder.CustomerType;
                            companyContract.Contractor.AssociationType = companyPolicy.Result.Holder.AssociationType;
                            companyContract.Contractor.CustomerTypeDescription = companyPolicy.Result.Holder.CustomerTypeDescription;
                            companyContract.Contractor.EconomicActivity = companyPolicy.Result.Holder.EconomicActivity;
                            companyContract.Contractor.IdentificationDocument = companyPolicy.Result.Holder.IdentificationDocument;
                            companyContract.Contractor.InsuredId = companyPolicy.Result.Holder.InsuredId;
                            companyContract.Contractor.Name = companyPolicy.Result.Holder.Name;
                            companyContract.Contractor.OwnerRoleCode = companyPolicy.Result.Holder.OwnerRoleCode;
                            companyContract.Contractor.PaymentMethod = companyPolicy.Result.Holder.PaymentMethod;
                            companyContract.Contractor.SecondSurname = companyPolicy.Result.Holder.SecondSurname;
                            companyContract.Contractor.Surname = companyPolicy.Result.Holder.Surname;
                            companyContract.Contractor.AssociationTypeId = companyPolicy.Result.Holder.AssociationType?.Id;
                            companyContract.Contractor.BirthDate = companyPolicy.Result.Holder.BirthDate;
                        }
                        companyContract.Risk.Policy = companyPolicy.Result;
                        var coveages = companyContract.Risk.Coverages ?? new List<CompanyCoverage>();
                        Parallel.For(0, coveages.Count, rowCoverage =>
                        {
                            if (coveages[rowCoverage].CurrentFrom < companyPolicy.Result.CurrentFrom)
                            {
                                coveages[rowCoverage].CurrentFrom = companyPolicy.Result.CurrentFrom;
                            }

                            if (coveages[rowCoverage].CurrentTo > companyPolicy.Result.CurrentTo)
                            {
                                coveages[rowCoverage].CurrentTo = companyPolicy.Result.CurrentTo;
                            }

                            if (coveages[rowCoverage].CurrentFrom > coveages[rowCoverage].CurrentTo)
                            {
                                coveages[rowCoverage].CurrentTo = coveages[rowCoverage].CurrentFrom;
                            }
                        });
                        var suretyCompany = QuotateSurety(companyContract, true, true);
                        CreateSuretyTemporal(suretyCompany, false);
                        // CompanySaveCompanySuretyTemporal(suretyCompany);

                    });
                    var ciaPolicy = DelegateService.underwritingService.UpdatePolicyComponents(companyPolicy.Result.Id);
                    return ciaPolicy;
                }
                else
                {
                    return companyPolicy.Result;
                }
            }
            catch (Exception e)
            {
                throw new BusinessException(e.GetBaseException().Message);
            }

        }

        /// <summary>
        /// CompanySaveCompanySuretyTemporal. Grabar Tablas Temporal de CompanyContract
        /// </summary>
        /// <param name="companyVehicle"> Modelo CompanyContract</param>
        /// <returns></returns>
        public CompanyContract CompanySaveCompanySuretyTemporal(CompanyContract companyContract)
        {
            try
            {
                ContractDAO contractDAO = new ContractDAO();
                if (companyContract.Risk.Policy.TemporalType != TemporalType.TempQuotation)
                {
                    companyContract = contractDAO.SaveCompanyContractTemporalTables(companyContract);
                }
                return companyContract;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// CompanySaveCompanySuretyTemporal. Grabar Tablas Temporal de CompanyContract
        /// </summary>
        /// <param name="companyVehicle"> Modelo CompanyContract</param>
        /// <returns></returns>
        public CompanyContract CompanyRiskSuretyQuotation(int RiskId)
        {
            try
            {
                ContractDAO contractDAO = new ContractDAO();
                CompanyContract contract = contractDAO.GetCompanyContractByRiskId(RiskId);
                contract = contractDAO.SaveCompanyContractTemporalTables(contract);

                //companyVehicle = vehicleDAO.CompanySaveVehicleTemporal(companyVehicle);
                return contract;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets the coverage by coverage identifier.
        /// </summary>
        /// <param name="coverageId">The coverage identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="groupCoverageId">The group coverage identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public CompanyCoverage GetCoverageByCoverageId(int coverageId, int riskId, int temporalId, int groupCoverageId)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                var contract = GetCompanySuretyByRiskId(riskId);
                var coverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, policy.Product.Id, groupCoverageId);
                if (policy != null && coverage != null)
                {
                    coverage.EndorsementType = policy.Endorsement.EndorsementType;
                    coverage.CurrentFrom = policy.CurrentFrom;
                    coverage.CurrentTo = policy.CurrentTo;
                    coverage.Days = Convert.ToInt32((coverage.CurrentTo.Value - coverage.CurrentFrom).TotalDays);
                    if (coverage.EndorsementType == EndorsementType.Modification)
                    {
                        coverage.CoverStatus = CoverageStatusType.Included;
                    }
                    coverage.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Included));
                    if (coverage.RuleSetId.HasValue)
                    {
                        coverage = RunRulesCoverage(contract, coverage, coverage.RuleSetId.Value);
                    }
                    return coverage;
                }
                else
                {
                    throw new Exception(Errors.ErrorSearchCoverage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Errors.ErrorSearchCoverage, ex);
            }
        }

        /// <summary>
        /// Gets the premium.
        /// </summary>
        /// <param name="ciaContract">TheCompany contract.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public CompanyContract GetPremium(CompanyContract ciaContract)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(ciaContract.Risk.Id, false);
                if (policy == null)
                {
                    throw new Exception(Errors.ErrorSearchPolicy);
                }
                policy.IsPersisted = true;
                ciaContract.Risk.Policy = policy;
                ciaContract.Risk.DynamicProperties.AsParallel().ForAll(x => { x.TypeName = ""; x.EntityId = 84; });
                ciaContract.Risk.IsPersisted = true;
                ciaContract = QuotateSurety(ciaContract, true, true);
                return ciaContract;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Creates the company policy.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="temporalType">Type of the temporal.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType, bool clearPolicies, CompanyModification companyModification)
        {
            try
            {
                CompanyPolicyResult companyPolicyResult = new CompanyPolicyResult();
                companyPolicyResult.IsError = false;
                companyPolicyResult.Errors = new List<ErrorBase>();
                string message = string.Empty;
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

                if (policy.PaymentPlan.PremiumFinance != null && policy.Endorsement.EndorsementType == EndorsementType.Modification)
                {
                    policy.PaymentPlan = DelegateService.underwritingService.GetDefaultPaymentPlan(policy.Product.Id);
                }
                policy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Today);
                policy.Errors = new List<ErrorBase>();
                if (policy == null)
                {
                    companyPolicyResult.IsError = true;
                    companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorTemporalNotFound });
                }
                else
                {
                    if (EndorsementType.Modification == policy.Endorsement.EndorsementType && companyModification != null)
                    {
                        if (policy.Endorsement.Text != null)
                        {
                            policy.Endorsement.Text.TextBody = companyModification.Text;
                            policy.Endorsement.Text.Observations = companyModification.Observations;
                            policy.Endorsement.TicketDate = companyModification.RegistrationDate;
                            policy.Endorsement.TicketNumber = companyModification.RegistrationNumber;
                        }
                    }
                    if (policy.Summary == null)
                    {
                        companyPolicyResult.IsError = true;
                        companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorTempPremiumZero });
                    }
                    else
                    {
                        if (temporalType != TempType.Quotation)
                        {
                            ValidateHolder(ref policy);
                        }
                        if (!policy.Errors.Any() && policy.Product.CoveredRisk != null)
                        {

                            List<CompanyContract> sureties = GetCompanySuretiesByTemporalId(policy.Id);
                            if (sureties != null && sureties.Any())
                            {
                                if (sureties[0].Risk.Coverages.Exists(x => x.EndorsementLimitAmount != x.DeclaredAmount || x.EndorsementLimitAmount != x.DeclaredAmount || x.SubLimitAmount != x.DeclaredAmount || x.LimitAmount != x.DeclaredAmount) && policy.Endorsement.EndorsementType==EndorsementType.Emission)
                                {
                                    throw new ArgumentException(Errors.ErrorCoveragesZero);
                                }
                                else { 
                                if (policy.Product.CoveredRisk.SubCoveredRiskType != SubCoveredRiskType.Lease && sureties.Any(x => string.IsNullOrEmpty(x.Risk?.Text?.TextBody)))
                                {
                                    companyPolicyResult.IsError = true;
                                    companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorContractObject });
                                }
                                else
                                {
                                    if (clearPolicies)
                                    {
                                        policy.InfringementPolicies.Clear();
                                        sureties.ForEach(x => x.Risk.InfringementPolicies.Clear());
                                    }

                                    policy = CreateEndorsement(policy, sureties);
                                    if (policy?.InfringementPolicies?.Count == 0)
                                    {
                                        DelegateService.underwritingService.SaveTextLarge(policy.Endorsement.PolicyId, policy.Endorsement.Id);
                                    }
                                }
                            }}
                            else
                            {
                                throw new ArgumentException(Errors.NoExistRisk);
                            }
                            if (temporalType != TempType.Quotation)
                            {
                                if (policy.InfringementPolicies.Any())
                                {
                                    companyPolicyResult.TemporalId = policy.Id;
                                    companyPolicyResult.InfringementPolicies = policy.InfringementPolicies;
                                }
                                companyPolicyResult.Message = string.Format("{0} : {1}", Errors.PolicyNumber, policy.DocumentNumber);
                                companyPolicyResult.DocumentNumber = policy.DocumentNumber;
                                companyPolicyResult.EndorsementId = policy.Endorsement.Id;
                                companyPolicyResult.EndorsementNumber = policy.Endorsement.Number;
                                companyPolicyResult.IsReinsured = policy.IsReinsured;
                                companyPolicyResult.PolicyId = policy.Endorsement.PolicyId;
                            }
                            else
                            {
                                companyPolicyResult.Message = string.Format("{0} : {1}", Errors.QuotationNumber, policy.Endorsement.QuotationId.ToString());
                                companyPolicyResult.DocumentNumber = Convert.ToDecimal(policy.Endorsement.QuotationId);

                            }
                        }
                        else
                        {
                            companyPolicyResult.IsError = true;

                            foreach (ErrorBase item in policy.Errors)
                            {
                                companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = string.Join(" - ", item.Error) });
                            }

                        }


                    }
                }
                return companyPolicyResult;
            }
            catch (Exception ex)
            {
                //  throw new BusinessException("Error creando endoso", ex);
                if (ex?.Message != null && ex.Message != String.Empty)
                    throw new BusinessException(ex.Message);
                else
                    throw new BusinessException(Errors.ErrorCreatePolicy);
            }

        }
        /// <summary>
        /// Validates the holder.
        /// </summary>
        /// <param name="policy">The policy.</param>
        public void ValidateHolder(ref CompanyPolicy policy)
        {
            if (policy.Holder != null)
            {
                if (policy.Holder.CustomerType == CustomerType.Prospect)
                {
                    policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorHolderNoInsuredRole });
                }
                else
                {
                    IssuanceInsured holder = DelegateService.underwritingService.GetHolderValidateByIndividualId(policy.Holder.IndividualId);

                    if (holder != null)
                    {
                        if (holder.InsuredId == 0)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorHolderNoInsuredRole });
                        }
                        else if (holder?.DeclinedDate > DateTime.MinValue)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorPolicyholderDisabled });
                        }
                    }
                    else
                    {
                        policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorConsultPolicyholder });
                    }

                    if (policy.Holder.PaymentMethod != null)
                    {
                        if (policy.Holder.PaymentMethod.Id == 0)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorPolicyholderDefaultPaymentPlan });
                        }
                    }

                    //Validación asegurado principal como prospecto
                    switch (policy.Product.CoveredRisk.CoveredRiskType)
                    {
                        case CoveredRiskType.Surety:
                            List<CompanyContract> sureties = GetCompanySuretiesByTemporalId(policy.Id);

                            var result = sureties.Select(x => x.Risk).Where(z => z.MainInsured?.CustomerType == CustomerType.Prospect).Count();
                            if (result > 0)
                            {
                                policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorInsuredNoInsuredRole });
                            }
                            break;
                    }
                }
            }

        }
        #region validaciones
        private bool ValidateCoverage(List<CompanyCoverage> coverages)
        {
            bool validate = true;
            object obj = new object();
            Parallel.For(0, coverages.Count, (fieldRow, procesState) =>
            {
                if (coverages[fieldRow].Rate == null)
                {
                    lock (obj)
                    {
                        validate = false;
                    }
                    procesState.Break();
                }
            });
            return validate;
        }
        #endregion validaciones
        #endregion emision



        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public bool IsConsortiumindividualId(int individualId)
        {
            Boolean band2 = DelegateService.uniquePersonService.IsConsortiumindividualIdR1(individualId);
            return band2;

        }

        public decimal GetAvailableCumulus(int individualId, int currencyCode, int prefixCode, System.DateTime issueDate)
        {

            return DelegateService.uniquePersonService.GetAvailableCumulus(individualId, currencyCode, prefixCode, issueDate);
        }

        public List<CompanyContract> GetCompanySuretiesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            try
            {
                ContractBusiness contractBusiness = new ContractBusiness();
                return contractBusiness.GetCompanySuretiesByEndorsementIdModuleType(endorsementId, moduleType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public decimal GetPremiumAmtByPolicyIdRiskNum(int PolicyId, int RiskNum)
        {
            try
            {
                ContractBusiness contractBusiness = new ContractBusiness();
                decimal premium = contractBusiness.GetPremiumAmtByPolicyIdRiskNum(PolicyId, RiskNum);
                return premium;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyContract> GetCompanyRisksSuretyByInsuredId(int insuredId)
        {
            try
            {
                ContractBusiness contractBusiness = new ContractBusiness();
                return contractBusiness.GetCompanyRisksSuretyByInsuredId(insuredId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyContract> GetCompanyRisksSuretyBySuretyId(int suretyId)
        {
            try
            {
                ContractBusiness contractBusiness = new ContractBusiness();
                return contractBusiness.GetCompanyRisksSuretyBySuretyId(suretyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyContract> GetCompanyRisksBySurety(string description)
        {
            try
            {
                ContractBusiness contractBusiness = new ContractBusiness();
                return contractBusiness.GetCompanyRisksBySurety(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanySummary ConvertProspectToInsuredRisk(CompanyPolicy companyPolicy, int individualId)
        {
            try
            {
                List<CompanyContract> companySurety = GetCompanyRisksSuretyBySuretyId(companyPolicy.Id);

                if (companySurety.Count > 0)
                {
                    foreach (CompanyContract surety in companySurety)
                    {
                        surety.Risk.Policy = companyPolicy;
                        if (surety.Risk.MainInsured.CustomerType == CustomerType.Prospect)
                        {

                            CompanyRisk risk = DelegateService.underwritingService.ConvertProspectToInsuredRisk(surety.Risk, individualId);
                            surety.Risk.Beneficiaries = risk.Beneficiaries;
                        }
                        List<CompanyBeneficiary> listBeneficiary = new List<CompanyBeneficiary>();
                        surety.Risk.Beneficiaries.ToList().ForEach(x =>
                        {
                            if (x.CustomerType == CustomerType.Prospect)
                            {
                                CompanyBeneficiary result = DelegateService.underwritingService.ConvertProspectToBeneficiary(x, individualId);
                                listBeneficiary.Add(result);
                            }
                            else
                            {
                                listBeneficiary.Add(x);
                            }
                        });
                        surety.Risk.Beneficiaries = listBeneficiary;
                        surety.Risk.Description = surety.Risk.MainInsured.Name;
                        CreateSuretyTemporal(surety, false);
                        CompanyRiskInsured companyRiskInsureds = new CompanyRiskInsured
                        {
                            Insured = surety.Risk.MainInsured,
                            Beneficiaries = surety.Risk.Beneficiaries
                        };
                        companyPolicy.Summary.RisksInsured[0] = companyRiskInsureds;
                        companyPolicy = DelegateService.underwritingService.CompanySavePolicyTemporal(companyPolicy, false);
                    }
                    return companyPolicy.Summary;
                }
                else
                {
                    return companyPolicy.Summary;
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorConvertingProspectIntoIndividual, ex);
            }
        }

        /// <summary>
        /// Elimina caracteres especiales 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string unicode_iso8859(string text)
        {
            if (text == null) text = "";

            text = text = text.Replace("\u001a", "");
            System.Text.Encoding iso = System.Text.Encoding.GetEncoding("iso8859-1");
            text = Regex.Replace(text, @"[/']", " ", RegexOptions.None);
            byte[] isoByte = iso.GetBytes(text);
            return iso.GetString(isoByte);
        }

        #region Consulta de Poliza por Id

        /// <summary>
        /// Obtener Vehiculos De Un Temporal
        /// </summary>
        /// <param name="policyId">Id Temporal</param>
        /// <returns>Vehiculos</returns>
        public List<CompanyContract> GetCompanyContractByPolicyId(int policyId)
        {
            try
            {
                ContractDAO contractDAO = new ContractDAO();
                return contractDAO.GetCompanyContractsByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets the company premium.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns></returns>
        public CompanyContract GetCompanyPremium(int policyId, CompanyContract companyContract, int temporalType)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);
                policy.IsPersisted = true;
                companyContract.Risk.Policy = policy;
                companyContract.Risk.Description = companyContract.Contractor.Name;
                companyContract = QuotateSurety(companyContract, true, true);
                companyContract?.Risk?.Coverages.AsParallel().ForAll(x =>
                {
                    x.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(x?.CoverStatus.Value));
                });
                return companyContract;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.GetBaseException().Message);
            }

        }
        #endregion

    }
}
