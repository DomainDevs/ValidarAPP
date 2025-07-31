using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.CollectiveServices.Entities.Views
{
    [Serializable]
    public class CollectiveEmissionView : BusinessView
    {
        public BusinessCollection CollectiveEmission
        {
            get
            {
                return this["CollectiveEmission"];
            }
        }
        public BusinessCollection MassiveLoad
        {
            get
            {
                return this["MassiveLoad"];
            }
        }
    }
}
