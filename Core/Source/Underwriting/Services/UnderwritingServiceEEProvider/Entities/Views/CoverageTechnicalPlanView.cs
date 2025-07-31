using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class CoverageTechnicalPlanView : BusinessView
    {
        public BusinessCollection TechnicalPlanCoverages
        {
            get
            {
                return this["TechnicalPlanCoverage"];
            }
        }

        public BusinessCollection Coverages
        {
            get
            {
                return this["Coverage"];
            }
        }

        public BusinessCollection LineBusiness
        {
            get
            {
                return this["LineBusiness"];
            }
        }

        public BusinessCollection SubLineBusiness
        {
            get
            {
                return this["SubLineBusiness"];
            }
        }

        public BusinessCollection CoverageAllied
        {
            get
            {
                return this["AllyCoverage"];
            }
        }

    }
}
