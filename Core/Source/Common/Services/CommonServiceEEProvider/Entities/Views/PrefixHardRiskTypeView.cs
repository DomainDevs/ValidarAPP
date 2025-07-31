using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.CommonServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class PrefixHardRiskTypeView : BusinessView
    {

        public BusinessCollection Prefix
        {
            get
            {
                return this["Prefix"];
            }
        }

        public BusinessCollection HardRiskTypes
        {
            get
            {
                return this["HardRiskType"];
            }
        }

    }
}
