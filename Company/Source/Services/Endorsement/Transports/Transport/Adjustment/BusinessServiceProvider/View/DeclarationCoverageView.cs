
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Transports.Endorsement.Adjustment.BusinessService.EEProvider.View
{
    
    [Serializable()]
    public class DeclarationCoverageView : BusinessView
    {

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
        public BusinessCollection Coverages
        {
            get
            {
                return this["Coverage"];
            }
        }


        public BusinessCollection Endorsements
        {
            get
            {
                return this["Endorsement"];
            }
        }

        public BusinessCollection EndorsementTypes
        {
            get
            {
                return this["EndorsementType"];
            }
        }


    }
}
