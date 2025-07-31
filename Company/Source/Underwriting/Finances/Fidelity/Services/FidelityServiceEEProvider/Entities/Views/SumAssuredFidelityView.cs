using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
namespace Sistran.Company.Application.Finances.FidelityServices.EEProvider.Entities.View
{
    [Serializable()]
    public class SumAssuredFidelityView : BusinessView
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
