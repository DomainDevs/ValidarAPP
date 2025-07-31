using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyAgentBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyAgentBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        #region agent
        /// <summary>
        /// obtiene un agente
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public CompanyAgent GetCompanyAgentByIndividualId(int id)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperAgent();
                var result = coreProvider.GetAgentByIndividualId(id);
                return imapper.Map<Agent, CompanyAgent>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Crea el agente por IndividualId.
        /// </summary>
        /// <param name="companyAgent">Modelo Agente</param>
        /// <returns></returns>
        public CompanyAgent CreateCompanyAgent(CompanyAgent companyAgent)
        {
            try
            {
               var imapper = ModelAssembler.CreateMapperAgent();
                var agentCore = imapper.Map<CompanyAgent, Agent>(companyAgent);
                var result = coreProvider.CreateAgent(agentCore);
                return imapper.Map<Agent, CompanyAgent>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actauliza el Agente por InvidualId
        /// </summary>
        /// <param name="companyAgent"></param>
        /// <returns></returns>
        public CompanyAgent UpdateCompanyAgent(CompanyAgent companyAgent)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperAgent();
                var agentCore = imapper.Map<CompanyAgent, Agent>(companyAgent);
                var result = coreProvider.UpdateAgent(agentCore);
                return imapper.Map<Agent, CompanyAgent>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtiene la la agencia por inviduoId
        /// </summary>
        /// <param name="InvidualId">Codigo del InvidualId</param>
        /// <returns></returns>
        public List<CompanyAgency> GetCompanyAgencyByInvidualId(int InvidualId)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperAgencyByIndividual();
                List<CompanyAgency> ListCompanyAgencies = new List<CompanyAgency>();
                var result = coreProvider.GetAgencyByInvidualId(InvidualId);
                foreach (var item in result)
                {
                    var resultMapper = imapper.Map<Agency, CompanyAgency>(item);
                    ListCompanyAgencies.Add(resultMapper);
                }
                return ListCompanyAgencies;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene la la agencia por dabas De baja
        /// </summary>
        /// <param name="InvidualId">Codigo del InvidualId</param>
        /// <returns></returns>
        public List<CompanyAgency> GetActiveCompanyAgency(int InvidualId)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperAgencyByIndividual();
                List<CompanyAgency> ListCompanyAgencies = new List<CompanyAgency>();
                var result = coreProvider.GetActiveAgencyByInvidualId(InvidualId);
                foreach (var item in result)
                {
                    var resultMapper = imapper.Map<Agency, CompanyAgency>(item);
                    ListCompanyAgencies.Add(resultMapper);
                }
                return ListCompanyAgencies;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Crea las Agencias por inviduaId
        /// </summary>
        /// <param name="companyAgencies">Modelo de Agencias.</param>
        /// <param name="IndividualId">Codigo Invidual id</param>
        /// <returns></returns>
        public CompanyAgency CreateCompanyAgencyByInvidualId(CompanyAgency companyAgencies, int IndividualId)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperAgencyByIndividual();
                var agencyCore = imapper.Map<CompanyAgency, Agency>(companyAgencies);
                var result = coreProvider.CreateAgencyByInvidualId(agencyCore, IndividualId);
                return imapper.Map<Agency, CompanyAgency>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actualiza Agencia por InvidualId
        /// </summary>
        /// <param name="companyAgencies"></param>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        public CompanyAgency UpdateCompanyAgencyByInvidualId(CompanyAgency companyAgencies, int IndividualId)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperAgencyByIndividual();
                var agencyCore = imapper.Map<CompanyAgency, Agency>(companyAgencies);
                var result = coreProvider.UpdateAgencyByInvidualId(agencyCore, IndividualId);
                return imapper.Map<Agency, CompanyAgency>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtiene los Ramos por agente 
        /// </summary>
        /// <param name="IndivualId">codigo de la persona</param>
        /// <returns></returns>
        public List<CompanyPrefixs> GetCompanyPrefixesAgentIndividualId(int IndivualId)
        {
            try
            {
                var imapper = ModelAssembler.CreatePrefixAgeuntIndivialId();
                var result = coreProvider.GetPrefixesByAgentId(IndivualId);
                return imapper.Map<List<BasePrefix>, List<CompanyPrefixs>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Crea el ramo para la persona.
        /// </summary>
        /// <param name="companyPrefix">model de ramos </param>
        /// <param name="IndividualId">codigo de la persona</param>
        /// <returns></returns>
        public CompanyPrefixs CretaeCompanyPrefixesAgentIndividualId(CompanyPrefixs companyPrefix, int IndividualId)
        {
            try
            {
                var imapper = ModelAssembler.CreatePrefixAgeuntIndivialId();
                var agencyCore = imapper.Map<CompanyPrefixs, BasePrefix>(companyPrefix);
                var result = coreProvider.CreatePrefixesByAgentId(agencyCore, IndividualId);
                return imapper.Map<BasePrefix, CompanyPrefixs>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actualiza el ramo para la persona.
        /// </summary>
        /// <param name="companyPrefix">model de ramos </param>
        /// <param name="IndividualId">codigo de la persona</param>
        /// <returns></returns>
        public CompanyPrefixs UpdateCompanyPrefixesAgentIndividualId(CompanyPrefixs companyPrefix, int IndividualId)
        {
            try
            {
                var imapper = ModelAssembler.CreatePrefixAgeuntIndivialId();
                var agencyCore = imapper.Map<CompanyPrefixs, BasePrefix>(companyPrefix);
                var result = coreProvider.UpdatePrefixesByAgentId(agencyCore, IndividualId);
                return imapper.Map<BasePrefix, CompanyPrefixs>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Delete el ramo para la persona.
        /// </summary>
        /// <param name="companyPrefix">model de ramos </param>
        /// <param name="IndividualId">codigo de la persona</param>
        /// <returns></returns>
        public CompanyPrefixs DeleteCompanyPrefixesAgentIndividualId(CompanyPrefixs companyPrefix, int IndividualId)
        {
            try
            {
                var imapper = ModelAssembler.CreatePrefixAgeuntIndivialId();
                var agencyCore = imapper.Map<CompanyPrefixs, BasePrefix>(companyPrefix);
                var result = coreProvider.DeletePrefixesByAgentId(agencyCore, IndividualId);
                return imapper.Map<BasePrefix, CompanyPrefixs>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyComissionAgent> GetCompanyComissionIndividualId(int IndivualId)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperComissionIndividualId();
                List<CompanyComissionAgent> commissions = new List<CompanyComissionAgent>();
                var result = coreProvider.GetCommissionInvidualId(IndivualId);
                foreach (var item in result)
                {
                    var resultAutoiMapper = imapper.Map<Commission, CompanyComissionAgent>(item);
                    commissions.Add(resultAutoiMapper);
                }
                return commissions;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Crea el ramo para la persona.
        /// </summary>
        /// <param name="companyPrefix">model de ramos </param>
        /// <param name="IndividualId">codigo de la persona</param>
        /// <returns></returns>
        public CompanyComissionAgent CreateCompanyComissionIndividualId(CompanyComissionAgent companycomission, int IndividualId,int AgencyId)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperComissionIndividualId();
                var agencyCore = imapper.Map<CompanyComissionAgent, Commission>(companycomission);
                var result = coreProvider.CreateCommissionInvidualId(agencyCore, IndividualId, AgencyId);
                return imapper.Map<Commission, CompanyComissionAgent>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actualiza el ramo para la persona.
        /// </summary>
        /// <param name="companyPrefix">model de ramos </param>
        /// <param name="IndividualId">codigo de la persona</param>
        /// <returns></returns>
        public CompanyComissionAgent UpdateCompanyComissionIndividualId(CompanyComissionAgent companycomission, int IndividualId, int AgencyId)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperComissionIndividualId();
                var agencyCore = imapper.Map<CompanyComissionAgent, Commission>(companycomission);
                var result = coreProvider.UpdateCommissionInvidualId(agencyCore, IndividualId, AgencyId);
                return imapper.Map<Commission, CompanyComissionAgent>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Delete el ramo para la persona.
        /// </summary>
        /// <param name="companyPrefix">model de ramos </param>
        /// <param name="IndividualId">codigo de la persona</param>
        /// <returns></returns>
        public CompanyComissionAgent DeleteCompanyComissionIndividualId(CompanyComissionAgent companycomission)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperComissionIndividualId();
                var agencyCore = imapper.Map<CompanyComissionAgent, Commission>(companycomission);
                var result = coreProvider.DeleteAgentCommission(agencyCore);
                var resultExit = coreProvider.GetCommissionInvidualId(companycomission.IndividualId);
                if (result == true)
                {
                    var commisionsExit = coreProvider.GetCommissionInvidualId(companycomission.IndividualId);
                    imapper.Map<List<Commission>, List<CompanyComissionAgent>>(commisionsExit);
                    foreach (var item in commisionsExit)
                    {
                        var result1= imapper.Map<Commission, CompanyComissionAgent>(item);
                    }

                }
                return null;

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        internal List<CompanyAgency> GetCompanyAgenciesByAgentIdDescription(int agentId, string description)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperAgency();
                var result = coreProvider.GetAgenciesByAgentIdDescription(agentId, description);
                return imapper.Map<List<Agency>, List<CompanyAgency>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        internal List<CompanyAgency> GetCompanyAgenciesByAgentId(int agentId)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperAgency();
                var result = coreProvider.GetAgenciesByAgentId(agentId);
                return imapper.Map<List<Agency>, List<CompanyAgency>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyAgency GetCompanyAgencyByAgentCodeAgentTypeCode(int agentCode, int agentTypeCode)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperAgency();
                var result = coreProvider.GetAgencyByAgentCodeAgentTypeCode(agentCode, agentTypeCode);
                return imapper.Map<Agency, CompanyAgency>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion agent



    }
}
