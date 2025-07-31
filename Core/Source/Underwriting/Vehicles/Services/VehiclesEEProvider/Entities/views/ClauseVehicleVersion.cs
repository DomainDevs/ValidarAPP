using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Vehicles.EEProvider.Entities.Views
{
    [Serializable()]
    public class ClauseVehicleVersion : BusinessView
    {
        public BusinessCollection VehicleVersion
        {
            get
            {
                return this["VehicleVersion"];
            }
        }

        public BusinessCollection VehicleMake
        {
            get
            {
                return this["VehicleMake"];
            }
        }

        public BusinessCollection VehicleModel
        {
            get
            {
                return this["VehicleModel"];
            }
        }


    }
}
