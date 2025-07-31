using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class UserProfileView : BusinessView
    {
        public BusinessCollection UniqueUsers
        {
            get
            {
                return this["UniqueUsers"];
            }
        }

        public BusinessCollection ProfileUniqueUsers
        {
            get
            {
                return this["ProfileUniqueUser"];
            }
        }

        public BusinessCollection Profiles
        {
            get
            {
                return this["Profiles"];
            }
        }
    }
}
