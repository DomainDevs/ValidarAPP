using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.MassiveRenewalServices.Entities.Views
{
    [Serializable]
    public class MassiveRenewalView : BusinessView
    {
        public BusinessCollection MassiveRenewal
        {
            get
            {
                return this["MassiveRenewal"];
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
