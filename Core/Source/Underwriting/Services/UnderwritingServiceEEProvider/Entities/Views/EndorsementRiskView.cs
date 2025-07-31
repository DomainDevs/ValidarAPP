using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class EndorsementRiskView : BusinessView
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
        public BusinessCollection Risks
        {
            get
            {
                return this["Risk"];
            }
        }
    }
}
