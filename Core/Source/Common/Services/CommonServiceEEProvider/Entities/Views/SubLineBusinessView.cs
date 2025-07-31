using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.CommonServices.EEProvider.Entities.Views
{
    [Serializable]
    public class SubLineBusinessView : BusinessView
    {
        public BusinessCollection SubLineBusiness
        {
            get
            {
                return this["SubLineBusiness"];
            }
        }
        public BusinessCollection LineBusiness
        {
            get
            {
                return this["LineBusiness"];
            }
        }
    }
}
