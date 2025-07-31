using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.UniquePersonService.V1.Entities.views
{
    [Serializable()]
    public class AgentPrefixAgentAgencyViewV1 : BusinessView
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
