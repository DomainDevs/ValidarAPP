using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.V1.Entities.views
{
    [Serializable()]
    public class AgentPrefixViewV1 : BusinessView
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
    }
}
