using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Transports.TransportBusinessService.EEProvider.Views
{
    [Serializable()]
    public class RiskTransportsView : BusinessView
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
        public BusinessCollection RiskTransports
        {
            get
            {
                return this["RiskTransport"];
            }
        }
        public BusinessCollection RiskTransportMeans
        {
            get
            {
                return this["RiskTransportMean"];
            }
        }
        public BusinessCollection TransportMeans
        {
            get
            {
                return this["TransportMean"];
            }
        }
        public BusinessCollection TransportViaTypes
        {
            get
            {
                return this["TransportViaType"];
            }
        }
        public BusinessCollection TransportPackagingTypes
        {
            get
            {
                return this["TransportPackagingType"];
            }
        }
    }
}
