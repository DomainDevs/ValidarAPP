using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.UniquePersonParamService.EEProvider.DAOs
{
    using Sistran.Company.Application.UniquePersonParamService.Models;
    using entitiesUPersonCore = Sistran.Core.Application.UniquePerson.Entities;
    using Sistran.Core.Framework.Queries;
    using System;
    using System.Collections.Generic;
    using Sistran.Company.Application.UniquePerson.Entities;
    using Sistran.Company.Application.UniquePersonParamService.EEProvider.Entities.views;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Company.Application.UniquePersonParamService.EEProvider.Assemblers;

    public class AgencyDAO
    {
        /// <summary>
        /// Obtener aliado por Identificadores
        /// </summary>
        /// <param name="individualId">Id del individuo.</param>
        /// <param name="agentAgencyId">Id agencia</param>
        /// <returns>Listado de Aliados</returns>
        public List<SmAlly> GetAllyByIntermediary(int individualId, int agentAgencyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            string tableAlias = typeof(entitiesUPersonCore.AgentAgency).Name;
            filter.PropertyEquals(CptAgentAlliance.Properties.IndividualId, typeof(CptAgentAlliance).Name, individualId);
            filter.And();
            filter.PropertyEquals(CptAgentAlliance.Properties.AgentAgencyId, typeof(CptAgentAlliance).Name, agentAgencyId);
            CptAgentAllianceView view = new CptAgentAllianceView();
            ViewBuilder builder = new ViewBuilder("CptAgentAllianceView");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<SmAlly> SmAllyList = ModelAssembler.MappAllyList(view.CptAlliance);
            return SmAllyList;
        }
    }
}
