using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Entities.Views
{
    [Serializable]
    public class MassiveEmissionView : BusinessView
    {
        public BusinessCollection MassiveEmission
        {
            get
            {
                return this["MassiveEmission"];
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
