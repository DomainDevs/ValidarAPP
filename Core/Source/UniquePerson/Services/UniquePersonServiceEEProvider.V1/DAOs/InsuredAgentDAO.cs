using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Agente del Asegurado
    /// </summary>
    public class InsuredAgentDAO
    {
        /// <summary>
        /// Crear Agente del Asegurado
        /// </summary>
        /// <param name="insuredAgent">The insured agent.</param>
        /// <returns></returns>
        public void CreateInsuredAgent(Models.Agency agency, int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(InsuredAgent.Properties.InsuredIndId, typeof(InsuredAgent).Name);
            filter.Equal();
            filter.Constant(individualId);

            InsuredAgent entityInsuredAgent = (InsuredAgent)DataFacadeManager.Instance.GetDataFacade().List(typeof(InsuredAgent), filter.GetPredicate()).FirstOrDefault();

            if (entityInsuredAgent != null)
            {

                PrimaryKey primaryKey = InsuredAgent.CreatePrimaryKey(entityInsuredAgent.InsuredIndId, entityInsuredAgent.AgentIndId, entityInsuredAgent.AgentAgencyId);
                DataFacadeManager.Delete(primaryKey);
            }

            entityInsuredAgent = EntityAssembler.CreateInsuredAgent(individualId, agency);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityInsuredAgent);

        }
    }
}
