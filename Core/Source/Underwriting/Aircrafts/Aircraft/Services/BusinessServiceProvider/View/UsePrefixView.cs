using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.Aircrafts.AircraftBusinessService.EEProvider.Views
{
    [Serializable()]
    public class AircraftsUsePrefixView : BusinessView
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
