using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    [Serializable()]
    public class ApplicationPremiumComponentView : BusinessView
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

        public BusinessCollection ApplicationPremiumComponents
        {
            get
            {
                return this["ApplicationPremiumComponent"];
            }
        }

        public BusinessCollection ApplicationPremiums
        {
            get
            {
                return this["ApplicationPremium"];
            }
        }
    }
}
