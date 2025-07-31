using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.UniquePersonService.Entities.views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PersonBase = Sistran.Core.Application.UniquePersonService.Models.Base;
using Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    /// Agencias
    /// </summary>
    public class AgencyDAO
    {
        /// <summary>
        /// Obtener Agencias
        /// </summary> 
        /// <param name="agentId">Id Agente</param>
        /// <param name="description">Código o Nombre</param>
        /// <returns>Agencias</returns>
        public List<Models.Agency> GetAgenciesByAgentIdDescription(int agentId, string description)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            if (agentId > 0)
            {
                filter.Property(Agent.Properties.IndividualId, typeof(Agent).Name);
                filter.Equal();
                filter.Constant(agentId);
                filter.And();
            }

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
                filter.Property(Agent.Properties.CheckPayableTo, typeof(Agent).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
            }
            AgentAgencyView view = new AgentAgencyView();
            ViewBuilder builder = new ViewBuilder("AgentAgencyView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<Models.Agent> agents = ModelAssembler.CreateAgents(view.Agents);
            List<Models.Agency> agencies = ModelAssembler.CreateAgencies(view.AgentAgencies);

            foreach (Models.Agency item in agencies)
            {
                item.Agent.FullName = agents.First(x => x.IndividualId == item.Agent.IndividualId).FullName;
                item.Agent.DateDeclined = agents.First(x => x.IndividualId == item.Agent.IndividualId).DateDeclined;
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgenciesByAgentIdDescription");
            return agencies;
        }

		        /// <summary>
        /// Obtener Agencias habilitadas por ramo
        /// </summary>
        /// <param name="agentId">Id Agente</param>
        /// <param name="description">Código o Nombre</param>
        /// <param name="prefixId">Código o Nombre</param>
        /// <returns>Agencias</returns>
        public List<Models.Agency> GetAgenciesByAgentIdDescriptionIdPrefix(int agentId, string description, int prefixId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(Prefix.Properties.PrefixCode, typeof(Prefix).Name);
            filter.Equal();
            filter.Constant(prefixId);
            filter.And();

            if (agentId > 0)
            {
                filter.Property(Agent.Properties.IndividualId, typeof(Agent).Name);
                filter.Equal();
                filter.Constant(agentId);
                filter.And();
            }

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
                filter.Property(Agent.Properties.CheckPayableTo, typeof(Agent).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
            }
            AgentPrefixAgentAgencyView view = new AgentPrefixAgentAgencyView();
            ViewBuilder builder = new ViewBuilder("AgentPrefixAgentAgencyView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<Models.Agent> agents = ModelAssembler.CreateAgents(view.Agents);
            List<Models.Agency> agencies = ModelAssembler.CreateAgencies(view.AgentAgencies);
            List<Models.Agency> agenciesResult = new List<Models.Agency>();
            foreach (Models.Agency item in agencies)
            {
                item.Agent.DateDeclined = agents.First(x => x.IndividualId == item.Agent.IndividualId).DateDeclined;
                if (item.Agent.DateDeclined > DateTime.MinValue)
                {                  
                }
                else
                {
                    if (item.DateDeclined > DateTime.MinValue)
                    {                      
                    }
                    else
                    {
                        item.Agent.FullName = agents.First(x => x.IndividualId == item.Agent.IndividualId).FullName;
                        agenciesResult.Add(item);
                    }
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgenciesByAgentIdDescriptionIdPrefix");
            return agenciesResult;
        }

        /// <summary>
        /// Obtener lista de agencias Id agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        public List<Models.Agency> GetAgenciesByAgentId(int agentId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AgentAgency.Properties.IndividualId, typeof(AgentAgency).Name);
            filter.Equal();
            filter.Constant(agentId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(AgentAgency), filter.GetPredicate()));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgenciesByAgentId");
            return ModelAssembler.CreateAgencies(businessCollection);
        }

        /// <summary>
        /// Gets the agencies.
        /// </summary>
        /// <param name="agentId">The agent identifier.</param>
        /// <returns></returns>
        public List<Models.Agency> GetAgencies(int agentId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AgentAgency.Properties.AgentCode, typeof(AgentAgency).Name);
            filter.Equal();
            filter.Constant(agentId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(AgentAgency), filter.GetPredicate()));
            return ModelAssembler.CreateAgencies(businessCollection);
        }

        /// <summary>
        /// Obtener lista de agencias por el individualId
        /// </summary>
        /// <param name="agentId">individualId</param>
        /// <returns></returns>
        public List<Models.Agency> GetAgencyByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AgentAgency.Properties.IndividualId, typeof(AgentAgency).Name);
            filter.Equal();
            filter.Constant(individualId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(AgentAgency), filter.GetPredicate()));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgencyByIndividualId");
            return ModelAssembler.CreateAgencies(businessCollection);
        }

        /// <summary>
        /// Creates the agency.
        /// </summary>
        /// <param name="agency">The agency.</param>
        /// <param name="IndividualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.Agency CreateAgency(Models.Agency agency, int IndividualId)
        {
            PrimaryKey key = AgentAgency.CreatePrimaryKey(IndividualId, agency.Id);
            AgentAgency agencyEntity = (AgentAgency)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (agencyEntity == null)
            {
                AgentAgency agentEntityAux = EntityAssembler.CreateAgency(agency, IndividualId);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(agentEntityAux);
                return ModelAssembler.CreateAgency(agentEntityAux);
            }
            else
            {
                agencyEntity.AgentTypeCode = agency.AgentType.Id;
                agencyEntity.Description = agency.FullName;
                if (agency.AgentDeclinedType != null)
                {
                    agencyEntity.AgentDeclinedTypeCode = agency.AgentDeclinedType.Id;
                }
                else
                {
                    agencyEntity.AgentDeclinedTypeCode = null;
                }
                agencyEntity.DeclinedDate = agency.DateDeclined;
                agencyEntity.BranchCode = agency.Branch.Id;
                agencyEntity.Annotations = agency.Annotations;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(agencyEntity);
                return ModelAssembler.CreateAgency(agencyEntity);
            }
        }

        /// <summary>
        /// Obtener agencia por Identificadores
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <param name="agentAgencyId">Id agencia</param>
        /// <returns>Agencia</returns>
        public Models.Agency GetAgencyByAgentIdAgentAgencyId(int agentId, int agentAgencyId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            string tableAlias = typeof(AgentAgency).Name;
            filter.PropertyEquals(AgentAgency.Properties.IndividualId, typeof(AgentAgency).Name, agentId);
            filter.And();
            filter.PropertyEquals(AgentAgency.Properties.AgentAgencyId, typeof(AgentAgency).Name, agentAgencyId);
            AgentAgencyView view = new AgentAgencyView();
            ViewBuilder builder = new ViewBuilder("AgentAgencyView");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            if (!view.Agents.Any())
            {
                stopWatch.Stop();
                return null;
            }
            // la vista hace un join entonces si hay un agente debe haber una agencia
            stopWatch.Stop();
            Models.Agent agent = ModelAssembler.CreateAgent((Agent)view.Agents[0]);
            Models.Agency agency = ModelAssembler.CreateAgency((AgentAgency)view.AgentAgencies[0]);
            agency.Agent = agent;
            return agency;
        }
        /// <summary>
        /// Recupera una agencia por el indice único código de agencia - tipo de agencia 
        /// </summary>
        /// <param name="agentCode"></param>
        /// <param name="agentTypeCode"></param>
        /// <returns></returns>
        public Models.Agency GetAgencyByAgentCodeAgentTypeCode(int agentCode, int agentTypeCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            string tableAlias = typeof(AgentAgency).Name;
            filter.PropertyEquals(AgentAgency.Properties.AgentCode, tableAlias, agentCode);
            filter.And();
            filter.PropertyEquals(AgentAgency.Properties.AgentTypeCode, tableAlias, agentTypeCode);
            AgentAgencyView view = new AgentAgencyView();
            ViewBuilder builder = new ViewBuilder("AgentAgencyView");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            if (!view.Agents.Any())
            {
                return null;
            }
            // la vista hace un join entonces si hay un agente debe haber una agencia
            Models.Agent agent = ModelAssembler.CreateAgent((Agent)view.Agents[0]);
            Models.Agency agency = ModelAssembler.CreateAgency((AgentAgency)view.AgentAgencies[0]);
            agency.Agent = agent;

            return agency;
        }

        /// <summary>
        /// Obtener lista de agencias Id agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        public async Task<List<Models.Agency>> GetAgenciesByIds(List<int> Id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AgentAgency.Properties.IndividualId, typeof(AgentAgency).Name);
            filter.In();
            filter.ListValue();
            Id.AsParallel().ForAll(
                x =>
                {
                    filter.Constant(x);
                }
                );
            filter.EndList();
            var agentEntityResult = Task.Run(() =>
            {
                List<AgentAgency> agencies = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    agencies = daf.List(typeof(AgentAgency), filter.GetPredicate()).Cast<AgentAgency>().ToList();
                }             
                DataFacadeManager.Dispose();
                return agencies;
            });
            stopWatch.Stop();
            agentEntityResult.Wait();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgenciesByAgentId");
            return await Task.Run(() => ModelAssembler.CreateAgentcies(agentEntityResult.Result));
        }

        /// <summary>
        /// Obtener lista de agencias Id agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        public async Task<List<Models.Agency>> GetAgenciesByIdsAgency(List<PersonBase.BaseAgentAgency> Ids)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AgentAgency.Properties.IndividualId, typeof(AgentAgency).Name);
            filter.In();
            filter.ListValue();
            Ids.AsParallel().ForAll(
                x =>
                {
                    filter.Constant(x.Id);
                }
                );
            filter.EndList();
            filter.And();
            filter.Property(AgentAgency.Properties.AgentAgencyId, typeof(AgentAgency).Name);
            filter.In();
            filter.ListValue();
            Ids.AsParallel().ForAll(
                x =>
                {
                    filter.Constant(x.AgencyId);

                }
                );
            filter.EndList();
            var agentEntityResult = Task.Run(() =>
            {
                List<AgentAgency> agencies = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    agencies = daf.List(typeof(AgentAgency), filter.GetPredicate()).Cast<AgentAgency>().ToList();
                }
                DataFacadeManager.Dispose();
                return agencies;
            });
            stopWatch.Stop();
            agentEntityResult.Wait();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgenciesByAgentId");
            return await Task.Run(() => ModelAssembler.CreateAgentcies(agentEntityResult.Result));
        }
    }
}