using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class CompanyVehiclesSumAssuredView : BusinessView
    {
        public BusinessCollection EndoRiskCoverages
        {
            get
            {
                return this["EndorsementRiskCoverage"];
            }
        }

        public BusinessCollection RiskCoverages
        {
            get
            {
                return this["RiskCoverage"];
            }
        }
    }
}
