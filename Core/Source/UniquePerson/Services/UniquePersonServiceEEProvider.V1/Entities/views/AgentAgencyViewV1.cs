using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.V1.Entities.views
{
    [Serializable()]
    public class AgentAgencyViewV1 : BusinessView
    {
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
