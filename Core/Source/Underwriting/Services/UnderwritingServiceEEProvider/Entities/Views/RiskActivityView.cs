using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class RiskActivityView : BusinessView
    {
        public BusinessCollection RiskCommercialClasses
        {
            get
            {
                return this["RiskCommercialClass"];
            }
        }

        public BusinessCollection ProductRiskCommercialClasses
        {
            get
            {
                return this["ProductRiskCommercialClass"];
            }
        }
    }
}
