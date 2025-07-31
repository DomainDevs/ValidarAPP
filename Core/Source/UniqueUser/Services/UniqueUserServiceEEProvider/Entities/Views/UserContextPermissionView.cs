using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class UserContextPermissionView : BusinessView
    {
        public BusinessCollection UserContextPermission
        {
            get
            {
                return this["UserContextPermission"];
            }
        }

        public BusinessCollection SecurityContext
        {
            get
            {
                return this["SecurityContext"];
            }
        }
    }
}
