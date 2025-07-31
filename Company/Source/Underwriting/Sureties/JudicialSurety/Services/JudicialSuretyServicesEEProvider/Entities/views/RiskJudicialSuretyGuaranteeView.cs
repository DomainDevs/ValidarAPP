using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Entities.View
{
    [Serializable()]
    public class RiskJudicialSuretyGuaranteeView : BusinessView
    {
        public BusinessCollection Policies
        {
            get
            {
                return this["Policy"];
            }
        }

        public BusinessCollection Endorsements
        {
            get
            {
                return this["Endorsement"];
            }
        }

        public BusinessCollection EndorsementRisks
        {
            get
            {
                return this["EndorsementRisk"];
            }
        }

        public BusinessCollection RiskSuretyGuarantees
        {
            get
            {
                return this["RiskSuretyGuarantee"];
            }
        }

        public BusinessCollection RiskJudicialSuretyGuarantees
        {
            get
            {
                return this["RiskJudicialSuretyGuarantee"];
            }
        }
        
    }
}
