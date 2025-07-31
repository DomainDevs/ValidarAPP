// -----------------------------------------------------------------------
// <copyright file="AgentAgencyDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author></author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniquePersonParamService.EEProvider.DAOs
{
    using modelsUPersonCompany = Sistran.Company.Application.UniquePersonParamService.Models;
    using Sistran.Core.Application.UniquePerson.Entities;    
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Queries;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sistran.Company.Application.UniquePerson.Entities;
    using Sistran.Company.Application.UniquePersonParamService.EEProvider.Assemblers;

    /// <summary>
    /// Agencias
    /// </summary>
    public class CompanyAgentAgencyDAO
    {
        /// <summary>
        /// Finds the specified individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <param name="agentAgencyId">The agent agency identifier.</param>
        /// <returns></returns>
        public static AgentAgency Find(int individualId,int agentAgencyId)
        {
            PrimaryKey key = AgentAgency.CreatePrimaryKey(individualId, agentAgencyId);
            AgentAgency agentAgency = (AgentAgency)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            return agentAgency;
        }

        /// <summary>
        /// Listado de agencias de un intermediario
        /// </summary>
        /// <param name="individualId">Identificador del agente</param>
        /// <returns>Listado de agencias del intermediario</returns>
        //public List<modelsUPersonCore.AgentAgency> GetAgenciesAgentByIndividualId(int individualId)
        //{
        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.Property(CptAgentAlliance.Properties.IndividualId, typeof(CptAgentAlliance).Name);
        //    filter.Equal();
        //    filter.Constant(individualId);
        //    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CptAgentAlliance), filter.GetPredicate()));
        //    return ModelAssembler.CreateAgentAgencies(businessCollection);
        //}

        //public modelsUPersonCore.AgentAgency CreateAgentAgency(modelsUPersonCore.AgentAgency agentAgency, int IndividualId)
        //{
        //    PrimaryKey key = CptAgentAlliance.CreatePrimaryKey(IndividualId, agentAgency.AgencyAgencyId, agentAgency.AllianceId);
        //    CptAgentAlliance agentAgencyEntity = (CptAgentAlliance)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        //    modelsUPersonCore.AgentAgency agency = new modelsUPersonCore.AgentAgency();
        //    if (agentAgency.Status == "create")
        //    {
        //        CptAgentAlliance agentEntityAux = EntityAssembler.CreateAgentAgency(agentAgency);
        //        DataFacadeManager.Instance.GetDataFacade().InsertObject(agentEntityAux);
        //        agency = ModelAssembler.CreateAgentAgency(agentEntityAux);
        //    }
        //    else if (agentAgency.Status == "update")
        //    {
        //        agentAgencyEntity.SpecialPrint = agentAgency.IsSpecialImpression;
        //        DataFacadeManager.Instance.GetDataFacade().UpdateObject(agentAgencyEntity);
        //        agency = ModelAssembler.CreateAgentAgency(agentAgencyEntity);
        //    }
        //    else if (agentAgency.Status == "delete")
        //    {
        //        agentAgencyEntity.SpecialPrint = agentAgency.IsSpecialImpression;
        //        DataFacadeManager.Instance.GetDataFacade().DeleteObject(agentAgencyEntity);
        //        agency = ModelAssembler.CreateAgentAgency(agentAgencyEntity);
        //    }
        //    return agency;
        //}

        /// <summary>
        /// Obtener Agencia por agente agente
        /// </summary>        
        /// <param name="description">Código de agente o Nombre de agencia</param>
        /// <returns>Agencias</returns>
        public List<modelsUPersonCompany.SmAgentAgency> GetAgenAgencyByAgentIdDescription(string description)
        {            
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            Int32 agencyCode = 0;
            Int32.TryParse(description, out agencyCode);

            if (agencyCode > 0)
            {
                filter.Property(AgentAgency.Properties.AgentCode, typeof(AgentAgency).Name);
                filter.Equal();
                filter.Constant(agencyCode);
            }
            else
            {
                filter.Property(AgentAgency.Properties.Description, typeof(AgentAgency).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
            }

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(AgentAgency), filter.GetPredicate()));

            List<modelsUPersonCompany.SmAgentAgency> agentAgencyList = ModelAssembler.MappSMAgentAgencyList(businessCollection);

            agentAgencyList = agentAgencyList.Where(x => x.DeclinedDate == null || x.DeclinedDate > DateTime.Now).ToList();
                        
            return agentAgencyList;

        }
    }
}
