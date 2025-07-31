using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.Entities.views
{
    [Serializable()]
    public class AgentPrefixView : BusinessView
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
