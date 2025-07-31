using Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Tipos de Agente
    /// </summary>
    public class AgentTypeDAO
    {
        /// <summary>
        /// Gets the agent types.
        /// </summary>
        /// <returns></returns>
        public List<Models.AgentType> GetAgentTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(AgentType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetAgentTypes");
            return ModelAssembler.CreateAgentTypes(businessCollection);
        }

        /// <summary>
        ///Obtener typo de agente por id
        /// </summary>
        /// <param name="id">Identificador de agente</param>
        /// <returns>AgentType</returns>
        public Models.AgentType GetAgentTypeById(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = AgentType.CreatePrimaryKey(id);
            AgentType agentType = (AgentType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetColorById");
            return ModelAssembler.CreateAgentType(agentType);

        }
    }
}
