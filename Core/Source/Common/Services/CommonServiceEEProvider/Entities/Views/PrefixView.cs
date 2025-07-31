using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.CommonServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class PrefixView : BusinessView
    {
        public BusinessCollection Prefix
        {
            get
            {
                return this["Prefix"];
            }
        }
        public BusinessCollection PrefixType
        {
            get
            {
                return this["PrefixType"];
            }
        }
    }
}
