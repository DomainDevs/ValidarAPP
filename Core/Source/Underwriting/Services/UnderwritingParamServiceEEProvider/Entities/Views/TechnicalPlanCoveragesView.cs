using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    [Serializable()]
    public class TechnicalPlanCoveragesView : BusinessView
    {       
        public BusinessCollection TechnicalPlanCoverages
        {
            get
            {
                return this["TechnicalPlanCoverage"];
            }
        }
        public BusinessCollection TechnicalPlanRelatedCoverages
        {
            get
            {
                return this["TechnicalPlanRelatedCoverage"];
            }
        }

        public BusinessCollection TechnicalPlanRelatedAllyCoverages
        {
            get
            {
                return this["TechnicalPlanRelatedAllyCoverage"];
            }
        }

        public BusinessCollection TechnicalPlanAllyCoverages
        {
            get
            {
                return this["TechnicalPlanAllyCoverage"];
            }
        }
        
        public BusinessCollection TechnicalPlanPrincipalCoverages
        {
            get
            {
                return this["TechnicalPlanPrincipalCoverage"];
            }
        }

        public BusinessCollection TechnicalPlanInsuredObjects
        {
            get
            {
                return this["TechnicalPlanInsuredObject"];
            }
        }
    }
}
