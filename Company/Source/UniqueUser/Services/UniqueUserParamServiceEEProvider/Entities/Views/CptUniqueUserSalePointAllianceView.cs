using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Company.Application.UniqueUserParamService.EEProvider.Entities.Views
{
    [Serializable()]
    public class CptUniqueUserSalePointAllianceView : BusinessView
    {
        public BusinessCollection CptAllianceBranchSalePoint
        {
            get
            {
                return this["CptAllianceBranchSalePoint"];
            }
        }
        public BusinessCollection CptBranchAlliance
        {
            get
            {
                return this["CptBranchAlliance"];
            }
        }
        public BusinessCollection CptAgentAlliance
        {
            get
            {
                return this["CptAgentAlliance"];
            }
        }

        public BusinessCollection CptUniqueUserSalePointAlliance
        {
            get
            {
                return this["CptUniqueUserSalePointAlliance"];
            }
        }
    }
}
