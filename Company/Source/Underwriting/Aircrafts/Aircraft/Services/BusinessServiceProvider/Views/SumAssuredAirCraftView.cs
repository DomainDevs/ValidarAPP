using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Company.Application.Aircrafts.AircraftBusinessService.EEProvider.Views
{
    [Serializable()]
    public class SumAssuredAirCraftView : BusinessView
    {
        public BusinessCollection EndoRiskCoverages
        {
            get { return this["EndorsementRiskCoverage"]; }
        }

        public BusinessCollection RiskCoverages
        {
            get { return this["RiskCoverage"]; }
        }
    }
}