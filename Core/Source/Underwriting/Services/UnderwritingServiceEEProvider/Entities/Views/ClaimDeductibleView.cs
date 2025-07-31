using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class ClaimDeductibleView : BusinessView
    {

        public BusinessCollection EndorsementRiskCoverages
        {
            get
            {
                return this["EndorsementRiskCoverage"];
            }
        }

        public BusinessCollection RiskCoverages
        {
            get
            {
                return this["RiskCoverage"];
            }
        }

        public BusinessCollection RiskCoverDeducts
        {
            get
            {
                return this["RiskCoverDeduct"];
            }
        }

        public BusinessCollection DeductibleUnits
        {
            get
            {
                return this["DeductibleUnit"];
            }
        }

        public BusinessCollection MinimumDeductibleUnits
        {
            get
            {
                return this["MinimumDeductibleUnit"];
            }
        }

        public BusinessCollection DeductibleSubjects
        {
            get
            {
                return this["DeductibleSubject"];
            }
        }

        public BusinessCollection MinimumDeductibleSubjects
        {
            get
            {
                return this["MinimumDeductibleSubject"];
            }
        }

        public BusinessCollection Currencies
        {
            get
            {
                return this["Currency"];
            }
        }
    }
}
