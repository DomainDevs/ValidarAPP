using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    [Serializable()]
    public class TempApplicationPremiumComponentView : BusinessView
    {
        public BusinessCollection ComponentTypes
        {
            get
            {
                return this["ComponentType"];
            }
        }

        public BusinessCollection Components
        {
            get
            {
                return this["Component"];
            }
        }

        public BusinessCollection TempApplicationPremiumComponents
        {
            get
            {
                return this["TempApplicationPremiumComponent"];
            }
        }

    }
}
