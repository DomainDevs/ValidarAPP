using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.Entities.views
{
    [Serializable()]
    public class GuaranteeView : BusinessView
    {
        public BusinessCollection GuaranteeTypes
        {
            get
            {
                return this["GuaranteeType"];
            }
        }

        public BusinessCollection Guarantees
        {
            get
            {
                return this["Guarantee"];
            }
        }
    }
}
