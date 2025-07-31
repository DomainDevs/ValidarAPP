using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    /// Tipo declinacion Agente
    /// </summary>
    public class AgentDeclinedTypeDAO
    {
        /// <summary>
        /// Obtener Tipo declinacion Agente
        /// </summary>
        /// <returns></returns>
        public List<Models.AgentDeclinedType> GetAgentDeclinedTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(AgentDeclinedType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgentDeclinedTypes");
            return ModelAssembler.CreateAgentDeclinedTypes(businessCollection);
        }
    }
}
