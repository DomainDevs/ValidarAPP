using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.V1.Entities.views
{
    [Serializable()]
    public class AgencyIndividualRelationViewV1 : BusinessView
    {
        public BusinessCollection UniqueUsers
        {
            get
            {
                return this["UniqueUsers"];
            }
        }

        public BusinessCollection IndividualRelationsApp
        {
            get
            {
                return this["IndividualRelationApp"];
            }
        }

        public BusinessCollection Agents
        {
            get
            {
                return this["Agent"];
            }
        }

        public BusinessCollection AgentAgencies
        {
            get
            {
                return this["AgentAgency"];
            }
        }
    }
}