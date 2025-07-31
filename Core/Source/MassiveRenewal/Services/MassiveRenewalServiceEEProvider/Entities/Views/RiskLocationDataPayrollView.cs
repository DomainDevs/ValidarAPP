using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.MassiveRenewalServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class RiskLocationDataPayrollView : BusinessView
    {
        public BusinessCollection RiskLocations
        {
            get
            {
                return this["RiskLocation"];
            }
        }

        public BusinessCollection States
        {
            get
            {
                return this["State"];
            }
        }

        public BusinessCollection Cities
        {
            get
            {
                return this["City"];
            }
        }

        public BusinessCollection RiskCommercialClasses
        {
            get
            {
                return this["RiskCommercialClass"];
            }
        }
    }
}