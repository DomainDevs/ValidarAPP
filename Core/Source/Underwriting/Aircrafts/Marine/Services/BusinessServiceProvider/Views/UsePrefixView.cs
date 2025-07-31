using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.Marines.MarineBusinessService.EEProvider.Views
{
    class MarinesUsePrefixView : BusinessView
    {

        public BusinessCollection AircraftUses
        {
            get
            {
                return this["AircraftUse"];
            }
        }
        public BusinessCollection AircraftUsePrefixs
        {
            get
            {
                return this["AircraftUsePrefix"];
            }
        }
    }
}
