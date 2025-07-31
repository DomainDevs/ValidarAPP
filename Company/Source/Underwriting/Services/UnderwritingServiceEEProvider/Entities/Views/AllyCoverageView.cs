using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable]
    public class AllyCoverageView : BusinessView
    {
        public BusinessCollection AllyCoverages
        {
            get { return this["AllyCoverage"]; }
        }
        public BusinessCollection Coverages
        {
            get { return this["Coverage"]; }
        }

        public BusinessCollection CoverageAllyCoverages
        {
            get { return this["CoverageAllyCoverage"]; }
        }
    }
}
