using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.VehicleParamService.EEProvider.Entities.Views
{

    [Serializable()]
    public class ClauseVehicleModel : BusinessView
    {
       
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
