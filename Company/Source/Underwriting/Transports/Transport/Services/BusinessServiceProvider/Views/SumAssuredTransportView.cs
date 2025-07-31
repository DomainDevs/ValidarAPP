using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Company.Application.Transports.TransportBusinessService.EEProvider.Views
{
    [Serializable()]
    public class SumAssuredTransportView : BusinessView
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