using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable]
    public class PayerCompView : BusinessView
    {
        public BusinessCollection PayerComponents
        {
            get { return this["PayerComp"]; }
        }
        public BusinessCollection Coverages
        {
            get { return this["Coverage"]; }
        }
        public BusinessCollection LineBusiness
        {
            get { return this["LineBusiness"]; }
        }
    }
}
