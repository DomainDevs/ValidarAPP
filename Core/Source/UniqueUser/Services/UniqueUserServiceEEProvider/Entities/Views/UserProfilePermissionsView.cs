using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class UserProfilePermissionsView : BusinessView
    {
        public BusinessCollection ProfileAccessPermissions
        {
            get
            {
                return this["ProfileAccessPermissions"];
            }
        }

        public BusinessCollection ProfileUniqueUsers
        {
            get
            {
                return this["ProfileUniqueUser"];
            }
        }

        public BusinessCollection AccessPermissions
        {
            get
            {
                return this["AccessPermissions"];
            }
        }
    }
}
