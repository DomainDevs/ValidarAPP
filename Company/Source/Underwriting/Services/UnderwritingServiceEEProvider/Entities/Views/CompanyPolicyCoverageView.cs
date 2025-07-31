using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views
{
    public class CompanyPolicyCoverageView : PolicyCoverageView
    {
        public BusinessCollection CoCoverage
        {
            get
            {
                return this["CoCoverage"];
            }
        }
        public BusinessCollection InsuredObject
        {
            get
            {
                return this["InsuredObject"];
            }
        }

        public BusinessCollection GroupCoverages
        {
            get
            {
                return this["GroupCoverage"];
            }
        }

        public BusinessCollection CoverageAllied
        {
            get
            {
                return this["AllyCoverage"];
            }
        }

        public BusinessCollection RiskCoverClauses
        {
            get
            {
                return this["RiskCoverClause"];
            }
        }
    }
}
