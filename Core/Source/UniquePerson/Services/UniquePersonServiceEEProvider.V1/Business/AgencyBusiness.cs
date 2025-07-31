using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Entities.views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class AgencyBusiness
    {
        
        public Models.Agency CreateAgency(Models.Agency agency, int individualId)
        {
            AgentAgency entityAgency = EntityAssembler.CreateAgency(agency, individualId);
            entityAgency = DataFacadeManager.Insert(entityAgency) as AgentAgency;
            return ModelAssembler.CreateAgency(entityAgency);
        }

        public List<Models.Agency> CreateAgencies(List<Models.Agency> agencies, int individualId)
        {
            foreach (Models.Agency agency in agencies)
            {
                CreateAgency(agency, individualId);
            }

            return agencies;
        }

        public Models.Agency UpdateAgency(Models.Agency agency, int individualId)
        {
            AgentAgency entityAgency = EntityAssembler.CreateAgency(agency, individualId);
            entityAgency.IndividualId = individualId;
            DataFacadeManager.Update(entityAgency);
            return ModelAssembler.CreateAgency(entityAgency);
        }

        public List<Models.Agency> UpdateAgencies(int individualId, List<Models.Agency> agencies)
        {
            foreach (Models.Agency agency in agencies)
            {
                UpdateAgency(agency, individualId);
            }

            return agencies;
        }

        public List<Models.Agency> GetAgenciesByAgentIdDescription(int agentId, string description)
        {
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

            AgentAgencyViewV1 view = new AgentAgencyViewV1();
            ViewBuilder builder = new ViewBuilder("AgentAgencyViewV1");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<Models.Agent> agents = ModelAssembler.CreateAgents(view.Agents);
            List<Models.Agency> agencies = ModelAssembler.CreateAgencies(view.AgentAgencies);

            foreach (Models.Agency item in agencies)
            {
                item.Agent.FullName = agents.First(x => x.IndividualId == item.Agent.IndividualId).FullName;
                item.Agent.DateDeclined = agents.First(x => x.IndividualId == item.Agent.IndividualId).DateDeclined;
            }

            return agencies;
        }

        public List<Models.Agency> GetAgenciesByAgentId(int agentId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AgentAgency.Properties.IndividualId, typeof(AgentAgency).Name);
            filter.Equal();
            filter.Constant(agentId);

            var business = DataFacadeManager.GetObjects(typeof(AgentAgency), filter.GetPredicate());
            return ModelAssembler.CreateAgencies(business);
        }
    }
}
