using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.UniquePersonService.Entities.views
{
    [Serializable()]
    public class AgentPrefixAgentAgencyView : BusinessView
    {
        public BusinessCollection AgentPrefixes
        {
            get
            {
                return this["AgentPrefix"];
            }
        }

        public BusinessCollection Prefixes
        {
            get
            {
                return this["Prefix"];
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
