using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class TemporalCoverageProductView : BusinessView
    {
        public BusinessCollection ProductList
        {
            get
            {
                return this["Product"];
            }
        }

        public BusinessCollection TempSubscriptionList
        {
            get
            {
                return this["TempSubscription"];
            }
        }

        public BusinessCollection ProductGroupCoverageList
        {
            get
            {
                return this["ProductGroupCoverage"];
            }
        }

        public BusinessCollection GroupInsuredObjectList
        {
            get
            {
                return this["GroupInsuredObject"];
            }
        }

        public BusinessCollection GroupCoverageList
        {
            get
            {
                return this["GroupCoverage"];
            }
        }

        public BusinessCollection TempRisks
        {
            get
            {
                return this["TempRisk"];
            }
        }

        public BusinessCollection TempRiskCoverage
        {
            get
            {
                return this["TempRiskCoverage"];
            }
        }
    }
}
