using Sistran.Core.Framework.DAF;
using System;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.View
{
    [Serializable()]
    public class SumAssuredView : BusinessView
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