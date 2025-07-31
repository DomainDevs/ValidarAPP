using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class AccessObjectProfileView : BusinessView
    {
        public BusinessCollection Accesses
        {
            get
            {
                return this["Accesses"];
            }
        }
        public BusinessCollection AccessParents
        {
            get
            {
                return this["AccessParent"];
            }
        }
        public BusinessCollection AccessObjects
        {
            get
            {
                return this["AccessObjects"];
            }
        }
        public BusinessCollection AccessProfiles
        {
            get
            {
                return this["AccessProfiles"];
            }
        }
        public BusinessCollection ProfileUniqueUsers
        {
            get
            {
                return this["ProfileUniqueUser"];
            }
        }
    }
}
