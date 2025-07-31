using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Agencias
    /// </summary>
    public class AgentAgencyDAO
    {
        /// <summary>
        /// Finds the specified individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <param name="agentAgencyId">The agent agency identifier.</param>
        /// <returns></returns>
        public static AgentAgency Find(int individualId, int agentAgencyId)
        {
            PrimaryKey key = AgentAgency.CreatePrimaryKey(individualId, agentAgencyId);
            AgentAgency agentAgency = (AgentAgency)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            return agentAgency;
        }
    }
}
