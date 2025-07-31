using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.UniquePersonService.Entities.views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Sistran.Co.Application.Data;
using Sistran.Core.Application.CommonService.Models.Base;
using System.Threading.Tasks;
namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    /// Ramos del Agente
    /// </summary>
    public class AgentPrefixDAO
    {

        /// <summary>
        /// Gets the agent prefix by individual identifier.
        /// </summary>
        /// <param name="IndividualId">The individual identifier.</param>
        /// <returns></returns>
        public List<Models.AgentPrefix> GetAgentPrefixByIndividualId(int IndividualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AgentPrefix.Properties.IndividualId, typeof(AgentPrefix).Name);
            filter.Equal();
            filter.Constant(IndividualId);
            BusinessCollection businessCollection = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(AgentPrefix), filter.GetPredicate()));
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgentPrefixByIndividualId");
            return ModelAssembler.CreateAgentPrefixs(businessCollection);
        }

        /// <summary>
        /// Obtener lista de ramos comerciales por agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        public List<BasePrefix> GetPrefixesByAgentId(int agentId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            AgentPrefixView view = new AgentPrefixView();
            ViewBuilder builder = new ViewBuilder("AgentPrefixView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AgentPrefix.Properties.IndividualId, typeof(AgentPrefix).Name);
            filter.Equal();
            filter.Constant(agentId);
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetPrefixesByAgentId");
            return ModelAssembler.CreatePrefixes(view.Prefixes);
        }

        /// <summary>
        /// Creates the agent prefix.
        /// </summary>
        /// <param name="agentPrefix">The agent prefix.</param>
        /// <param name="IndivualId">The indivual identifier.</param>
        /// <returns></returns>
        public BasePrefix CreateAgentPrefix(BasePrefix agentPrefix, int IndivualId)
        {

            PrimaryKey key = AgentPrefix.CreatePrimaryKey(agentPrefix.Id, IndivualId);
            AgentPrefix agentPrefixEntity = (AgentPrefix)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (agentPrefixEntity == null)
            {
                AgentPrefix agentPrefixEntityAux = EntityAssembler.CreateAgentPrefix(agentPrefix, IndivualId);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(agentPrefixEntityAux);
                return ModelAssembler.CreatePrefix(agentPrefixEntityAux);
            }
            else
            {
                agentPrefixEntity.IndividualId = IndivualId;
                agentPrefixEntity.PrefixCode = agentPrefix.Id;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(agentPrefixEntity);
                return ModelAssembler.CreatePrefix(agentPrefixEntity);
            }
        }

        /// <summary>
        /// Deletes the agent prefix by indivual identifier.
        /// </summary>
        /// <param name="IndivualId">The indivual identifier.</param>
        public void deleteAgentPrefixByIndivualId(int IndivualId)
        {
            var list = GetPrefixesByAgentId(IndivualId);

            foreach (Prefix item in list)
            {
                PrimaryKey key = AgentPrefix.CreatePrimaryKey(item.Id, IndivualId);
                AgentPrefix agentPrefixEntity = (AgentPrefix)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                if (agentPrefixEntity != null)
                {
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(agentPrefixEntity);
                }
            }
        }

        public BasePrefix deleteAgentPrefix(BasePrefix prefix, int IndivualId)
        {
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("PREFIXCODE", prefix.Id);
            parameters[1] = new NameValue("INDIVIDUAL_ID", IndivualId);
            object result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPScalar("UP.DELETE_AGENT_PREFIX", parameters);
            }

            if (int.Parse(result.ToString()) == 0)
            {
                PrimaryKey key = AgentPrefix.CreatePrimaryKey(prefix.Id, IndivualId);
                AgentPrefix agentPrefixEntity = (AgentPrefix)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                if (agentPrefixEntity != null)
                {
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(agentPrefixEntity);
                }
                return null;
            }
            else
            {
                return prefix;
            }

        }
        /// <summary>
        /// Gets the agent prefix by individual identifier.
        /// </summary>
        /// <param name="IndividualId">The individual identifier.</param>
        /// <returns></returns>
        public async Task<List<Models.AgentPrefix>> GetAgentPrefixByIndividualId(List<int> IndividualIds)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AgentAgency.Properties.IndividualId, typeof(AgentAgency).Name);
            filter.In();
            filter.ListValue();
            IndividualIds.AsParallel().ForAll(
                x =>
                {
                    filter.Constant(x);
                }
                );
            filter.EndList();

            var agentPrefixiesResult = Task.Run(() =>
            {
                List<AgentPrefix> agentPrefixies = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    agentPrefixies = daf.List(typeof(AgentPrefix), filter.GetPredicate()).Cast<AgentPrefix>().ToList();
                }
                DataFacadeManager.Dispose();
                return agentPrefixies;
            });

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgentPrefixByIndividualId");
            agentPrefixiesResult.Wait();
            return await Task.Run(() => ModelAssembler.CreateAgentsPrefixies(agentPrefixiesResult.Result));
        }

    }
}
