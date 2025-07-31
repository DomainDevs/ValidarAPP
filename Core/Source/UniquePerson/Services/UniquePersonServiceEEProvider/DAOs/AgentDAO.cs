using Sistran.Co.Application.Data;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.UniquePersonService.Entities.views;
using Sistran.Core.Application.UniquePersonService.Resources;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PersonBase = Sistran.Core.Application.UniquePersonService.Models.Base;
namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    /// Agente
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UniquePersonService.Models.Agent" />
    public class AgentDAO : Models.Agent
    {
        /// <summary>
        /// Gets the agent by filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public List<Models.Agent> GetAgentByFilter(ObjectCriteriaBuilder filter)
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Agent), filter.GetPredicate()));
            return ModelAssembler.CreateAgents(businessCollection);
        }

        /// <summary>
        /// Gets the agent by query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public List<Models.Agent> GetAgentByQuery(string query)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filterAgent = new ObjectCriteriaBuilder();
            filterAgent.Property(Agent.Properties.CheckPayableTo, typeof(Agent).Name);
            filterAgent.Like();
            filterAgent.Constant("%" + query + "%");

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Agent), filterAgent.GetPredicate()));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgentByQuery");
            return ModelAssembler.CreateAgents(businessCollection);
        }


        /// <summary>
        /// Gets the agents.
        /// </summary>
        /// <returns></returns>
        public List<Models.Agent> GetAgents()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filterAgent = new ObjectCriteriaBuilder();
            filterAgent.Property(Agent.Properties.DeclinedDate, typeof(Agent).Name);
            filterAgent.IsNull();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Agent), filterAgent.GetPredicate()));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgents");
            return ModelAssembler.CreateAgents(businessCollection);
        }
        //

        /// <summary>
        /// Gets the name of the agent by.
        /// </summary>
        /// <param name="nameAgent">The name agent.</param>
        /// <returns></returns>
        public List<Models.Agent> GetAgentByName(string nameAgent)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filterAgent = new ObjectCriteriaBuilder();
            filterAgent.Property(Agent.Properties.CheckPayableTo, typeof(Agent).Name);
            filterAgent.Equal();
            filterAgent.Constant(nameAgent);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Agent), filterAgent.GetPredicate()));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgentByName");
            return ModelAssembler.CreateAgents(businessCollection);
        }

        /// <summary>
        /// Creates the agent.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <returns></returns>
        public Models.Agent CreateAgent(Models.Agent agent)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Agent.Properties.IndividualId, typeof(Agent).Name);
            filter.Equal();
            filter.Constant(agent.IndividualId);
            PrimaryKey key = Agent.CreatePrimaryKey(agent.IndividualId);
            Agent agentEntity = (Agent)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (agentEntity == null)
            {
                Agent agentEntityAux = EntityAssembler.CreateAgent(agent);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(agentEntityAux);
                if (agent.Prefixes != null)
                {
                    AgentPrefixDAO agentPrefixDAO = new AgentPrefixDAO();
                    List<BasePrefix> prefixListExist = agentPrefixDAO.GetPrefixesByAgentId(agent.IndividualId);
                    List<BasePrefix> listToAdd = new List<BasePrefix>();
                    List<BasePrefix> listToDelete = new List<BasePrefix>();
                    List<BasePrefix> listToNoDelete = new List<BasePrefix>();
                    listToAdd = agent.Prefixes.Where(x => !prefixListExist.Exists(t => t.Id == x.Id)).ToList();
                    listToDelete = prefixListExist.Where(x => !agent.Prefixes.Exists(t => t.Id == x.Id)).ToList();

                    foreach (var item in listToDelete)
                    {
                        BasePrefix result = agentPrefixDAO.deleteAgentPrefix(item, agent.IndividualId);
                        if (result != null)
                        {
                            listToNoDelete.Add(result);
                        }
                    }
                    agent.Prefixes = listToNoDelete;
                    foreach (var item in listToAdd)
                    {
                        agentPrefixDAO.CreateAgentPrefix(item, agent.IndividualId);
                    }
                }
                if (agent.Agencies != null)
                {
                    AgencyDAO agentAgencyProvider = new AgencyDAO();
                    foreach (Models.Agency ma in agent.Agencies)
                    {
                        agentAgencyProvider.CreateAgency(ma, agentEntityAux.IndividualId);
                    }
                }

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.CreateAgent");
                return ModelAssembler.CreateAgent(agentEntityAux);
            }
            else
            {
                agentEntity.AgentTypeCode = agent.AgentType.Id;
                agentEntity.CheckPayableTo = agent.FullName;
                agentEntity.AgentDeclinedTypeCode = agent.AgentDeclinedType.Id;
                agentEntity.DeclinedDate = agent.DateDeclined;
                agentEntity.Annotations = agent.Annotations;
                agentEntity.ModifyDate = agent.DateModification;
                agentEntity.AgentGroupCode = agent.GroupAgent.Id;
                agentEntity.SalesChannelCode = agent.SalesChannel.Id;
                agentEntity.Locker = agent.Locker;

                if (agentEntity.AccExecutiveIndId != null)
                {
                    agentEntity.AccExecutiveIndId = agent.EmployeePerson.Id;
                }

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(agentEntity);


                if (agent.Prefixes != null)
                {
                    AgentPrefixDAO agentPrefixDAO = new AgentPrefixDAO();
                    List<BasePrefix> prefixListExist = agentPrefixDAO.GetPrefixesByAgentId(agent.IndividualId);
                    List<BasePrefix> listToAdd = new List<BasePrefix>();
                    List<BasePrefix> listToDelete = new List<BasePrefix>();
                    List<BasePrefix> listToNoDelete = new List<BasePrefix>();
                    listToAdd = agent.Prefixes.Where(x => !prefixListExist.Exists(t => t.Id == x.Id)).ToList();
                    listToDelete = prefixListExist.Where(x => !agent.Prefixes.Exists(t => t.Id == x.Id)).ToList();

                    foreach (var item in listToDelete)
                    {
                        BasePrefix result = agentPrefixDAO.deleteAgentPrefix(item, agent.IndividualId);
                        if (result != null)
                        {
                            listToNoDelete.Add(result);
                        }
                    }
                    agent.Prefixes = listToNoDelete;
                    foreach (var item in listToAdd)
                    {
                        agentPrefixDAO.CreateAgentPrefix(item, agent.IndividualId);
                    }
                }
                if (agent.Agencies != null)
                {
                    AgencyDAO agentAgencyProvider = new AgencyDAO();
                    foreach (Models.Agency ma in agent.Agencies)
                    {
                        agentAgencyProvider.CreateAgency(ma, agent.IndividualId);
                    }
                }
                if (agent.ComissionAgent != null)
                {
                    CommissionAgentDao agentCommissionProvider = new CommissionAgentDao();
                    foreach (Models.CommissionAgent ca in agent.ComissionAgent)
                    {
                        agentCommissionProvider.CreateAgentCommission(ca, agent.IndividualId);

                    }
                }

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.CreateAgent");
                Models.Agent ResultAgent = ModelAssembler.CreateAgent(agentEntity);
                ResultAgent.Prefixes = agent.Prefixes;
                return ResultAgent;
            }
        }

        /// <summary>
        /// Obtener agente por Id
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        public Models.Agent GetAgentByAgentId(int agentId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PrimaryKey key = Agent.CreatePrimaryKey(agentId);
            Agent agentEntity = (Agent)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            Models.Agent agentModel = ModelAssembler.CreateAgent(agentEntity);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgentByAgentId");
            return agentModel;
        }

        /// <summary>
        /// Obtener agente
        /// </summary>
        /// <param name="individualId">Id agente</param>
        /// <returns>Modelo agente</returns>
        public Models.Agent GetAgentByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = Agent.CreatePrimaryKey(individualId);
            Agent agentEntity = (Agent)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            if (agentEntity != null)
            {
                Models.Agent agent = ModelAssembler.CreateAgent(agentEntity);

                AgencyDAO agencyDAO = new AgencyDAO();
                agent.Agencies = agencyDAO.GetAgenciesByAgentId(agent.IndividualId);

                AgentPrefixDAO agentPrefixDAO = new AgentPrefixDAO();
                agent.Prefixes = agentPrefixDAO.GetPrefixesByAgentId(Convert.ToInt32(agent.IndividualId));

                CommissionAgentDao CommissionAgent = new CommissionAgentDao();
                agent.ComissionAgent = CommissionAgent.GetAgentCommissionByAgentId(Convert.ToInt32(agent.IndividualId));

                if (agentEntity.AccExecutiveIndId != null)
                {
                    EmployeeDAO employeeDAO = new EmployeeDAO();
                    agent.EmployeePerson = employeeDAO.GetEmployeePersonByIndividualId(Convert.ToInt32(agentEntity.AccExecutiveIndId));
                }

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgentByIndividualId");
                return agent;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgentByIndividualId");
                return null;
            }
        }

        /// <summary>
        /// Gets the full name of the agent by agent code.
        /// </summary>
        /// <param name="agentCode">The agent code.</param>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>
        public List<Models.Agent> GetAgentByAgentCodeFullName(int agentCode, string fullName)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (agentCode == 0)
            {
                filter.Property(Agent.Properties.CheckPayableTo, typeof(Agent).Name);
                filter.Like();
                filter.Constant(fullName + "%");
            }
            else
            {
                filter.Property(AgentAgency.Properties.AgentCode, typeof(AgentAgency).Name);
                filter.Equal();
                filter.Constant(agentCode);
            }

            AgentAgencyView view = new AgentAgencyView();
            ViewBuilder builder = new ViewBuilder("AgentAgencyView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<Models.Agent> agents = ModelAssembler.CreateAgents(view.Agents);
            List<Models.Agency> agencies = ModelAssembler.CreateAgencies(view.AgentAgencies);

            if (agents.Count == 1)
            {
                foreach (Models.Agent item in agents)
                {
                    List<Models.Agency> agency = (from p in agencies where p.Agent.IndividualId == item.IndividualId select p).ToList<Models.Agency>();
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgentByAgentCodeFullName");
            return agents;
        }

        /// <summary>
        /// Finds the specified individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public static Agent Find(int individualId)
        {
            PrimaryKey key = Agent.CreatePrimaryKey(individualId);
            Agent agent = (Agent)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            return agent;
        }

        /// <summary>
        /// Gets the full name of the agent by individual identifier.
        /// </summary>
        /// <param name="IndividualId">The individual identifier.</param>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>
        public List<Models.Agent> GetAgentByIndividualIdFullName(int IndividualId, string fullName)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (IndividualId == 0)
            {
                filter.Property(Agent.Properties.CheckPayableTo, typeof(Agent).Name);
                filter.Like();
                filter.Constant(fullName + "%");
            }
            else
            {
                filter.Property(Agent.Properties.IndividualId, typeof(Agent).Name);
                filter.Equal();
                filter.Constant(IndividualId);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgentByIndividualIdFullName");
            return GetAgentByFilter(filter);
        }

        /// <summary>
        /// Obtiene los agentes por ramo
        /// </summary>
        /// <param name="prefixId">id ramo</param>
        /// <returns>Lista de agentes por ramo</returns>
        public List<Models.Agent> GetAgentsByPrefix(int prefixId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<Models.Agent> ListAgent = new List<Models.Agent>();
            NameValue[] parameters = new NameValue[1];

            parameters[0] = new NameValue("PREFIX_CD", prefixId);

            DataTable result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("UP.GET_AGENT_BY_PREFIX_CD", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow arrayItem in result.Rows)
                {
                    ListAgent.Add(new Models.Agent
                    {
                        IndividualId = Convert.ToInt32(arrayItem[0]),
                        AgentType = new Models.AgentType() { Id = Convert.ToInt32(arrayItem[1]) },
                        FullName = arrayItem[2].ToString(),
                        AgentDeclinedType = new Models.AgentDeclinedType() { Id = DBNull.Value.Equals(arrayItem[5]) ? 0 : Convert.ToInt32(arrayItem[5]) },
                        DateCurrent = Convert.ToDateTime(arrayItem[3]),
                        DateDeclined = DBNull.Value.Equals(arrayItem[4]) ? Convert.ToDateTime(null) : Convert.ToDateTime(arrayItem[4]),
                        Annotations = arrayItem[9].ToString(),
                        DateModification = DBNull.Value.Equals(arrayItem[15]) ? Convert.ToDateTime(null) : Convert.ToDateTime(arrayItem[15])
                    });
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgentsByPrefix");
            return ListAgent;
        }

        public async Task<List<Models.Agent>> GetAgentsByIndividualIds(List<int> individualId)
        {
            if (individualId == null)
            {
                throw new ArgumentException("Individuos Vacio");
            }
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Agent.Properties.IndividualId, typeof(Agent).Name);
            filter.In();
            filter.ListValue();
            individualId.AsParallel().ForAll(
                x =>
                {
                    filter.Constant(x);
                }
                );
            filter.EndList();

            var agentEntityResult = Task.Run(() =>
              {
                  List<Agent> agentEntity = null;
                  using (var daf = DataFacadeManager.Instance.GetDataFacade())
                  {
                      agentEntity = daf.List(typeof(Agent), filter.GetPredicate()).Cast<Agent>().ToList();
                  }
                  DataFacadeManager.Dispose();
                  return agentEntity;
              });
            AgencyDAO agencyDAO = new AgencyDAO();
            var agencies = agencyDAO.GetAgenciesByIds(individualId);
            AgentPrefixDAO agentPrefixDAO = new AgentPrefixDAO();
            var agentsPrefixies = agentPrefixDAO.GetAgentPrefixByIndividualId(individualId);
            agencies.Wait();           
            var individualIds = agencies.Result.Select(x => new PersonBase.BaseAgentAgency { Id = x.Agent.IndividualId, AgencyId = x.Id }).Distinct().ToList();
            CommissionAgentDao commissionAgent = new CommissionAgentDao();
            var agentsComission = commissionAgent.GetAgentsCommissionByIndividualIds(individualIds);
            agentEntityResult.Wait();
            if (agentEntityResult != null)
            {

                var agents = ModelAssembler.CreateAgents(agentEntityResult.Result);

                agentsPrefixies.Wait();
                agentsComission.Wait();
                agents.AsParallel().ForAll(x =>
                {
                    x.Agencies = agencies.Result.Where(z => z.Agent.IndividualId == x.IndividualId).ToList();
                    x.Prefixes = agentsPrefixies.Result.Where(z => z.Id == x.IndividualId).Select(a => (BasePrefix)a.prefix).ToList();
                    x.ComissionAgent = agentsComission.Result.Where(z => z.IndividualId == x.IndividualId).ToList();
                });
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgentByIndividualId");
                return agents;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), typeof(AgentDAO).Assembly.FullName + "GetAgentByIndividualId");
                return null;
            }
        }
        public async Task<List<Models.Agent>> GetAgentsByIndividualIdsByAgencyId(List<PersonBase.BaseAgentAgency> IndividualIds, Int16 prefixId = -1)
        {
            if (IndividualIds == null)
            {
                throw new ArgumentException(Errors.ErrorParameterEmpty);
            }
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Agent.Properties.IndividualId, typeof(Agent).Name);
            filter.In();
            filter.ListValue();
            IndividualIds.AsParallel().ForAll(
                x =>
                {
                    filter.Constant(x.Id);
                }
                );
            filter.EndList();
            var agentEntityResult = Task.Run(() =>
            {
                List<Agent> agentEntity = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    agentEntity = daf.List(typeof(Agent), filter.GetPredicate()).Cast<Agent>().ToList();
                }
                DataFacadeManager.Dispose();
                return agentEntity;
            });
            AgencyDAO agencyDAO = new AgencyDAO();
            var agencies = agencyDAO.GetAgenciesByIdsAgency(IndividualIds);
            AgentPrefixDAO agentPrefixDAO = new AgentPrefixDAO();
            var agentsPrefixies = agentPrefixDAO.GetAgentPrefixByIndividualId(IndividualIds.Select(x => x.Id).ToList());
            agencies.Wait();
            var individualIds = agencies.Result.Select(x => new PersonBase.BaseAgentAgency { Id = x.Agent.IndividualId, AgencyId = x.Id }).Distinct().ToList();
            CommissionAgentDao commissionAgent = new CommissionAgentDao();
            var agentsComission = commissionAgent.GetAgentsCommissionByIndividualIds(individualIds, prefixId);
            agentEntityResult.Wait();
            if (agentEntityResult != null)
            {

                var agents = ModelAssembler.CreateAgents(agentEntityResult.Result);

                agentsPrefixies.Wait();
                agentsComission.Wait();
                agents.AsParallel().ForAll(x =>
                {
                    x.Agencies = agencies.Result.Where(z => z.Agent.IndividualId == x.IndividualId).ToList();
                    x.Prefixes = agentsPrefixies.Result.Where(z => z.Id == x.IndividualId).Select(a => (BasePrefix)a.prefix).ToList();
                    x.ComissionAgent = agentsComission.Result.Where(z => z.IndividualId == x.IndividualId).ToList();
                });

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgentByIndividualId");
                return agents;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), typeof(AgentDAO).Assembly.FullName + "GetAgentByIndividualId");
                return null;
            }
        }

    }
}
