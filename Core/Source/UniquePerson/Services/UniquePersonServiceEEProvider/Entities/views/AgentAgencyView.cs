using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.Entities.views
{
    [Serializable()]
    public class AgentAgencyView : BusinessView
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
