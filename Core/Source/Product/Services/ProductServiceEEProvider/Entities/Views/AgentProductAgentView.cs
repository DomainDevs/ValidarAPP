using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.ProductServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class AgentProductAgentView : BusinessView
    {
        public BusinessCollection Agents
        {
            get
            {
                return this["Agent"];
            }
        }        
        public BusinessCollection ProductAgents
        {
            get
            {
                return this["ProductAgent"];
            }
        }
        public BusinessCollection AgentAgencies
        {
            get
            {
                return this["AgentAgency"];
            }
        }

        public BusinessCollection IndividualsRelationApp
        {
            get
            {
                return this["IndividualRelationApp"];
            }
        }
    }
}
