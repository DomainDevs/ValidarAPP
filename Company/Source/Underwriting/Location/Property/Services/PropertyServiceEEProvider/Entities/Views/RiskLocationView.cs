using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Location.PropertyServices.EEProvider.Entities.View
{
    [Serializable()]
    public class RiskLocationView : BusinessView
    {
        public BusinessCollection EndorsementRisks
        {
            get { return this["EndorsementRisk"]; }
        }
        public BusinessCollection RiskLocations
        {
            get { return this["RiskLocation"]; }
        }
        public BusinessCollection Risks
        {
            get { return this["Risk"]; }
        }
        public BusinessCollection Addresses
        {
            get { return this["Address"]; }
        }
        public BusinessCollection Cities
        {
            get { return this["City"]; }
        }
        public BusinessCollection Countries
        {
            get { return this["Country"]; }
        }
        public BusinessCollection States
        {
            get { return this["State"]; }
        }
        public BusinessCollection Policies
        {
            get { return this["Policy"]; }
        }
    }
}
