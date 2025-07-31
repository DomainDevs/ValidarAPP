using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.Claims
{
    [Serializable()]
    public class ClaimInsuredAmountView : BusinessView
    {

        public BusinessCollection EndoRiskCoverages
        {
            get
            {
                return this["EndoRiskCoverage"];
            }
        }

        public BusinessCollection RiskCoverages
        {
            get
            {
                return this["RiskCoverage"];
            }
        }

        public BusinessCollection Claims
        {
            get
            {
                return this["Claim"];
            }
        }

        public BusinessCollection ClaimModifys
        {
            get
            {
                return this["ClaimModify"];
            }
        }

        public BusinessCollection ClaimCoverages
        {
            get
            {
                return this["ClaimCoverage"];
            }
        }
        public BusinessCollection ClaimCoverageAmounts
        {
            get
            {
                return this["ClaimCoverageAmount"];
            }
        }
    }
}
