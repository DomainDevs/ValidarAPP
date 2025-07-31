using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
namespace Sistran.Core.Application.Marines.MarineBusinessService.EEProvider.Views
{
    public class MarinesTypePrefixView : BusinessView
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
