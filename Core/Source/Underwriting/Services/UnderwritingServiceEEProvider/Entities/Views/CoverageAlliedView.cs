using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class CoverageAlliedView : BusinessView
    {
        public BusinessCollection Coverages
        {
            get
            {
                return this["Coverage"];
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
