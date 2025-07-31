using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class ProductCoverageGroupRiskView : BusinessView
    {
        public BusinessCollection ProductGroupCovers
        {
            get
            {
                return this["ProductGroupCover"];
            }
        }

        public BusinessCollection CoverGroupRiskTypes
        {
            get
            {
                return this["CoverGroupRiskType"];
            }
        }
    }
}
