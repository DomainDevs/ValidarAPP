using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.Entities.Views
{
    [Serializable()]
    public class CompanyCoverageView : BusinessView
    {
        public BusinessCollection Prefix
        {
            get
            {
                return this["Prefix"];
            }
        }
        public BusinessCollection Product
        {
            get
            {
                return this["Product"];
            }
        }
        public BusinessCollection GroupCoverage
        {
            get
            {
                return this["GroupCoverage"];
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
