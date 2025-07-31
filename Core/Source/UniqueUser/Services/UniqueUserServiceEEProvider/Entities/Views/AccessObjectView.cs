using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class AccessObjectView : BusinessView
    {
        public BusinessCollection Access
        {
            get
            {
                return this["Accesses"];
            }
        }
        public BusinessCollection AccessObjects
        {
            get
            {
                return this["AccessObjects"];
            }
        }

        public BusinessCollection Modules
        {
            get
            {
                return this["Modules"];
            }
        }

        public BusinessCollection SubModules
        {
            get
            {
                return this["Submodules"];
            }
        }
    }
}
