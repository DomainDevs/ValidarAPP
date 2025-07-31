using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class SubModuleView : BusinessView
    {
        public BusinessCollection Modules
        {
            get
            {
                return this["Modules"];
            }
        }
        public BusinessCollection Submodules
        {
            get
            {
                return this["Submodules"];
            }
        }
    }
}
