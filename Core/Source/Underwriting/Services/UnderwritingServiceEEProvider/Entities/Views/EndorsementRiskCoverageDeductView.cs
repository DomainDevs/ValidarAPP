using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class EndorsementRiskCoverageDeductView : BusinessView
    {
        public BusinessCollection EndorsementRiskCoverages
        {
            get { return this["EndorsementRiskCoverage"]; }
        }

        public BusinessCollection RiskCoverages
        {
            get { return this["RiskCoverage"]; }
        }

        public BusinessCollection RiskCoverDeducts
        {
            get { return this["RiskCoverDeduct"]; }
        }

        public BusinessCollection Coverages
        {
            get { return this["Coverage"]; }
        }

        public BusinessCollection SubLineBusiness
        {
            get { return this["SubLineBusiness"]; }
        }

        public BusinessCollection InsuredObjects
        {
            get { return this["InsuredObject"]; }
        }
    }
}