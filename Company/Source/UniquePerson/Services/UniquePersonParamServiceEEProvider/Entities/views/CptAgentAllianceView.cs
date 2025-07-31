using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Company.Application.UniquePersonParamService.EEProvider.Entities.views
{
    [Serializable()]
    public class CptAgentAllianceView : BusinessView
    {
        public BusinessCollection CptAgentAlliance
        {
            get
            {
                return this["CptAgentAlliance"];
            }
        }

        public BusinessCollection CptAlliance
        {
            get
            {
                return this["CptAlliance"];
            }
        }
    }
}
