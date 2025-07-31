using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Aircrafts.AircraftBusinessService.EEProvider.Views
{
    [Serializable()]
    public class RiskAircraftsView : BusinessView
    {

        public BusinessCollection EndorsementRisks
        {
            get
            {
                return this["EndorsementRisk"];
            }
        }
        public BusinessCollection EndorsementOperations
        {
            get
            {
                return this["EndorsementOperation"];
            }
        }
        public BusinessCollection EndoRiskCoverages
        {
            get
            {
                return this["EndoRiskCoverage"];
            }
        }
        public BusinessCollection RiskCoverages
        {
            get
            {
                return this["RiskCoverage"];
            }
        }
        public BusinessCollection RiskAircrafts
        {
            get
            {
                return this["RiskAircraft"];
            }
        }

        public BusinessCollection RiskAircraftMakes
        {
            get
            {
                return this["RiskAircraftMake"];
            }
        }

        public BusinessCollection RiskAircraftModels
        {
            get
            {
                return this["RiskAircraftModel"];
            }
        }

        public BusinessCollection RiskAircraftTypes
        {
            get
            {
                return this["RiskAircraftType"];
            }
        }

        public BusinessCollection RiskAircraftUses
        {
            get
            {
                return this["RiskAircraftUses"];
            }
        }

        public BusinessCollection RiskAircraftRegisters
        {
            get
            {
                return this["RiskAircraftRegister"];
            }
        }

        public BusinessCollection RiskAircraftOperators
        {
            get
            {
                return this["RiskAircraftOperator"];
            }
        }
    }
}
