using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class CoverageProductView : BusinessView
    {
        public BusinessCollection Coverages
        {
            get
            {
                return this["Coverage"];
            }
        }

        public BusinessCollection InsuredObjects
        {
            get
            {
                return this["InsuredObject"];
            }
        }
                
        public BusinessCollection LinesBusiness
        {
            get
            {
                return this["LineBusiness"];
            }
        }

        public BusinessCollection SubLinesBusiness
        {
            get
            {
                return this["SubLineBusiness"];
            }
        }
    }
}
