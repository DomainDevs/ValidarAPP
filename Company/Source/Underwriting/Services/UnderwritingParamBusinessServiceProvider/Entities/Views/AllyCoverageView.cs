using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.Entities.Views
{
    [Serializable()]
    public class AllyCoverageView : BusinessView
    {
        public BusinessCollection PrincipalCoverage
        {
            get
            {
                return this["PrincipalCoverage"];
            }
        }

        public BusinessCollection Coverage
        {
            get
            {
                return this["Coverage"];
            }
        }
        
    }
}
