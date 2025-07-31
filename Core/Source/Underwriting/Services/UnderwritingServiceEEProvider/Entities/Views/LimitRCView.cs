using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class LimitRCView : BusinessView
    {
        public BusinessCollection LimitsRC
        {
            get
            {
                return this["CoLimitsRc"];
            }
        }

        public BusinessCollection LimitRCRelations
        {
            get
            {
                return this["CoLimitsRcRel"];
            }
        }
    }
}
