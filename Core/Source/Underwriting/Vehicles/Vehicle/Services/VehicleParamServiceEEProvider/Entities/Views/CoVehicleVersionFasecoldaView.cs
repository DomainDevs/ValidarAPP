

namespace Sistran.Core.Application.VehicleParamService.EEProvider.Entities.Views
{
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;
    using System;


    [Serializable]
    public class CoVehicleVersionFasecoldaView: BusinessView
    {
        public BusinessCollection CoVehicleVersionFasecolda
        {
            get
            {
                return this["CoVehicleVersionFasecolda"];
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

        public BusinessCollection VehicleVersion
        {
            get
            {
                return this["VehicleVersion"];
            }
        }


    }
}
