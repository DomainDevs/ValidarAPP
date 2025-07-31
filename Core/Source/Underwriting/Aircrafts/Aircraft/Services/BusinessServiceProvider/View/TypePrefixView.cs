using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.Aircrafts.AircraftBusinessService.EEProvider.Views
{
    [Serializable()]
    public class AircraftTypePrefixView : BusinessView
    {

        public BusinessCollection AircraftTypePrefixs
        {
            get
            {
                return this["AircraftTypePrefix"];
            }
        }
        public BusinessCollection AircraftTypes
        {
            get
            {
                return this["AircraftType"];
            }
        }
    }
}
