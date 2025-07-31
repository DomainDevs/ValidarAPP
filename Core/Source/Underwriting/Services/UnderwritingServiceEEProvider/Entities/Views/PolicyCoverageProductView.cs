using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class PolicyCoverageProductView : BusinessView
    {
        public BusinessCollection ProductList
        {
            get
            {
                return this["Product"];
            }
        }

        public BusinessCollection PolicyList
        {
            get
            {
                return this["Policy"];
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

        public BusinessCollection Endorsement
        {
            get
            {
                return this["Endorsement"];
            }
        }

        public BusinessCollection EndorsementRiskCoverage
        {
            get
            {
                return this["EndorsementRiskCoverage"];
            }
        }

        public BusinessCollection RiskCoverage
        {
            get
            {
                return this["RiskCoverage"];
            }
        }
    }
}
