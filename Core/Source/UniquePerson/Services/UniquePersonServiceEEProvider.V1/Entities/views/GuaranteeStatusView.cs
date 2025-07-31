using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.V1.Entities.views
{
    [Serializable()]
    public class GuaranteeStatusView : BusinessView
    {
        public BusinessCollection StatusRoutes
        {
            get
            {
                return this["StatusRoute"];
            }
        }
        public BusinessCollection GuaranteeStatuses
        {
            get
            {
                return this["GuaranteeStatus"];
            }
        }
    }
}
