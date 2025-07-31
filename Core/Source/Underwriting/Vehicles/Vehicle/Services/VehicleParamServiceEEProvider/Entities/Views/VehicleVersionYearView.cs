namespace Sistran.Core.Application.VehicleParamService.EEProvider.Entities.Views
{
    using System;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;

    public class VehicleVersionYearView : BusinessView
    {
        public BusinessCollection VehicleVersionYears
        {
            get
            {
                return this["VehicleVersionYear"];
            }
        }

        public BusinessCollection VehicleMakes
        {
            get
            {
                return this["VehicleMake"];
            }
        }

        public BusinessCollection VehicleModels
        {
            get
            {
                return this["VehicleModel"];
            }
        }

        public BusinessCollection VehicleVersions
        {
            get
            {
                return this["VehicleVersion"];
            }
        }
        
    }
}
