using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Company.Application.ProductParamServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class CompanyCoverageAlliedView : BusinessView
    {
        public BusinessCollection Coverages
        {
            get
            {
                return this["Coverage"];
            }
        }

        public BusinessCollection AllyCoverages
        {
            get
            {
                return this["AllyCoverage"];
            }
        }

    }
}
