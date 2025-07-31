using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class HierarchyAccessView : BusinessView
    {
        public BusinessCollection CoHierarchyAccesses
        {
            get
            {
                return this["CoHierarchyAccess"];
            }
        }
        public BusinessCollection CoHierarchyAssociations
        {
            get
            {
                return this["CoHierarchyAssociation"];
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
