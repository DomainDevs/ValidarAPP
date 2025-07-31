using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class PolicyInsuredObjectProductView : BusinessView
    {
        public BusinessCollection ProductList
        {
            get
            {
                return this["Product"];
            }
        }

        public BusinessCollection PolicyList
        {
            get
            {
                return this["Policy"];
            }
        }

        public BusinessCollection ProductGroupCoverageList
        {
            get
            {
                return this["ProductGroupCoverage"];
            }
        }

        public BusinessCollection GroupInsuredObjectList
        {
            get
            {
                return this["GroupInsuredObject"];
            }
        }
    }
}
