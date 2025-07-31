using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.ProductServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class AgentPrefixProductView : BusinessView
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

        public BusinessCollection ProductAgents
        {
            get
            {
                return this["ProductAgent"];
            }
        }
    }
}
