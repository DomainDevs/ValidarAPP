
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UPEN = Sistran.Core.Application.UniquePerson.Entities;
using UUEN = Sistran.Core.Application.UniqueUser.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class AgencyDAO
    {
        public IssuanceAgency GetAgencyByAgentIdAgentAgencyId(int agentId, int agentAgencyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            string tableAlias = typeof(UPEN.AgentAgency).Name;
            filter.PropertyEquals(UPEN.AgentAgency.Properties.IndividualId, typeof(UPEN.AgentAgency).Name, agentId);
            filter.And();
            filter.PropertyEquals(UPEN.AgentAgency.Properties.AgentAgencyId, typeof(UPEN.AgentAgency).Name, agentAgencyId);

            IssuanceAgencyView view = new IssuanceAgencyView();
            ViewBuilder builder = new ViewBuilder("IssuanceAgencyView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.AgentAgencies.Count > 0)
            {
                return ModelAssembler.CreateAgency(view.AgentAgencies.Cast<UPEN.AgentAgency>().First(), view.Agents.Cast<UPEN.Agent>().First());
            }
            else
            {
                return null;
            }
        }

        public IssuanceAgency GetAgencyByUserId(int userId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(UPEN.Agent.Properties.IndividualId, "a"), "IndividualId"));

            select.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.AgentAgencyId, "aa"), "AgentAgencyId"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.BranchCode, "aa"), "BranchCode"));
            select.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.AgentCode, "aa"), "AgentCode"));

            Join join = new Join(new ClassNameTable(typeof(UUEN.UniqueUsers), "u"), new ClassNameTable(typeof(UPEN.IndividualRelationApp), "ira"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UUEN.UniqueUsers.Properties.PersonId, "u")
                .Equal()
                .Property(UPEN.IndividualRelationApp.Properties.ParentIndividualId, "ira")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UPEN.Agent), "a"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.IndividualRelationApp.Properties.ChildIndividualId, "ira")
                .Equal()
                .Property(UPEN.Agent.Properties.IndividualId, "a")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UPEN.AgentAgency), "aa"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.IndividualRelationApp.Properties.AgentAgencyId, "ira")
                .Equal()
                .Property(UPEN.AgentAgency.Properties.AgentAgencyId, "aa")
                .And()
                .Property(UPEN.Agent.Properties.IndividualId, "a")
                .Equal()
                .Property(UPEN.AgentAgency.Properties.IndividualId, "aa")
                .GetPredicate());


            filter.Property(UUEN.UniqueUsers.Properties.UserId, "u");
            filter.Equal();
            filter.Constant(userId);

            select.Table = join;
            select.Where = filter.GetPredicate();

            IssuanceAgency issuanceAgency = null;

            List<ParamConditionText> listConditionTextModel = new List<ParamConditionText>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    if(reader["IndividualId"]!=null)
                    {
                        issuanceAgency = new IssuanceAgency
                        {
                            Id = (int)reader["IndividualId"],
                            Agent = new IssuanceAgent
                            {
                                IndividualId = (int)reader["AgentCode"]
                            },
                            Branch = new CommonService.Models.Branch
                            {
                                Id = (int)reader["BranchCode"]
                            }
                        };
                    }
                    break;
                }
            }
            return issuanceAgency;
        }

        public IssuanceAgency GetAgencyByAgentCodeAgentTypeCode(int agentCode, int agentTypeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            string tableAlias = typeof(UPEN.AgentAgency).Name;
            filter.PropertyEquals(UPEN.AgentAgency.Properties.AgentCode, typeof(UPEN.AgentAgency).Name, agentCode);
            filter.And();
            filter.PropertyEquals(UPEN.AgentAgency.Properties.AgentTypeCode, typeof(UPEN.AgentAgency).Name, agentTypeId);

            IssuanceAgencyView view = new IssuanceAgencyView();
            ViewBuilder builder = new ViewBuilder("IssuanceAgencyView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.AgentAgencies.Count > 0)
            {
                return ModelAssembler.CreateAgency(view.AgentAgencies.Cast<UPEN.AgentAgency>().First(), view.Agents.Cast<UPEN.Agent>().First());
            }
            else
            {
                return null;
            }
        }

        public List<IssuanceAgency> GetAgencyAll()
        {
            BusinessCollection agency = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(AgentAgency)));
            List<IssuanceAgency> agencyModel = ModelAssembler.CreateAgencies(agency);
            return agencyModel;
        }
    }
}